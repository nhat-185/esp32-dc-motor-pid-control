#include <stdio.h>
#include <string.h>
#include <math.h>
#include "freertos/portmacro.h"
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/gpio.h"
#include "esp_log.h"
#include "led_strip.h"
#include "sdkconfig.h"
#include "esp_system.h"
#include "esp_spi_flash.h"
#include "esp_timer.h"
#include "freertos/queue.h"
#include "stddef.h"
#include "driver/ledc.h"
#include "esp_err.h"
#include "esp_wifi.h"
#include "nvs_flash.h"
#include "esp_event.h"
#include "esp_netif.h"
#include "freertos/event_groups.h"
#include "lwip/sockets.h"
#include "lwip/netdb.h"
#include "esp_http_server.h"
#include <stdlib.h>
#include "driver/adc.h"
#include "driver/dac.h"

static const char *TAG = "ESP32_STATUS_SERVER";

#define IN1_GPIO GPIO_NUM_22     // Điều khiển chiều IN1
#define IN2_GPIO GPIO_NUM_23     // Điều khiển chiều IN2
#define ENA_PWM_GPIO GPIO_NUM_21 // PWM tốc độ
#define ENCODER_A_PIN GPIO_NUM_18  // Chân A của encoder (có ngắt)
#define ENCODER_B_PIN GPIO_NUM_19  // Chân B của encoder (chỉ input, không ngắt)
#define PULSES_PER_REV 328.3f    // Đổi thành số thực để chia góc chính xác (số xung trên mỗi vòng quay của encoder)

// KHỞI TẠO IẾN TOÀN CỤC CHUNG 
portMUX_TYPE mux = portMUX_INITIALIZER_UNLOCKED;  
static QueueHandle_t pwm_queue;  // Hàng đợi để gửi giá trị PWM từ task PID sang task Webserver
volatile int32_t pulse_count = 0; // Bộ đếm xung của encoder

uint8_t control_mode = 0;           // 0: Điều khiển tốc độ, 1: Điều khiển góc
volatile bool reset_pos_flag = false; // Cờ báo reset gốc tọa độ
uint8_t pwm_output = 0;

// BIẾN CHO CHẾ ĐỘ TỐC ĐỘ 
volatile int32_t target_rpm = 0; 
volatile int32_t rpm_int = 0;    
float Kp = 0, Ki = 0, Kd = 0; 

// BIẾN CHO CHẾ ĐỘ GÓC 
volatile float target_angle = 0;    // Góc mục tiêu (độ)
volatile float current_angle = 0;   // Góc hiện tại (độ)
float Kp_pos = 2.5, Ki_pos = 0.1, Kd_pos = 0.05;

// BIẾN PID 
float integral = 0;
float prev_error = 0;
float derivative = 0;

// BIẾN AUTO-TUNE
volatile bool is_autotuning = false;
float relay_step = 50.0;     
float tune_base_pwm = 100.0; 
float tune_max_rpm = 0.0;
float tune_min_rpm = 10000.0;
int64_t last_zero_cross_time = 0;
int cross_count = 0;  // Biến đếm số lần qua điểm không (zero-crossing) trong quá trình autotune
float Tu_sum = 0.0;  // Biến tích lũy thời gian giữa các lần qua điểm không để tính chu kỳ Osccilation Tu sau khi đủ số lần qua điểm không
bool last_state_above = false;

// MOTOR & ENCODER


// Hàm xử lý ngắt cho encoder (chỉ ngắt trên chân A)
static void IRAM_ATTR encoder_isr_handler(void *arg) 
{
    int level_a = gpio_get_level(ENCODER_A_PIN);
    int level_b = gpio_get_level(ENCODER_B_PIN);

    portENTER_CRITICAL_ISR(&mux);
    if (level_a == level_b) pulse_count++; 
    else pulse_count--; 
    portEXIT_CRITICAL_ISR(&mux);
}


// Hàm điều khiển motor: direction = 1 (quay thuận), direction = 0 (quay ngược), speed = 0-255
void motor_set(int direction, uint8_t speed)
{
    if (direction) {
        gpio_set_level(IN1_GPIO, 1);
        gpio_set_level(IN2_GPIO, 0);
    } else {
        gpio_set_level(IN1_GPIO, 0);
        gpio_set_level(IN2_GPIO, 1);
    }
    ledc_set_duty(LEDC_LOW_SPEED_MODE, LEDC_CHANNEL_0, speed);
    ledc_update_duty(LEDC_LOW_SPEED_MODE, LEDC_CHANNEL_0);
}

float filtered_rpm = 0;  // Biến toàn cục để lưu giá trị RPM đã lọc, giúp giảm nhiễu và ổn định điều khiển
float alpha = 0.05; 

// Hàm lọc nhiễu tốc độ RPM
float iir_filter(float input_rpm) {
    filtered_rpm = alpha * input_rpm + (1.0 - alpha) * filtered_rpm;
    return filtered_rpm;
}


// TASK ĐIỀU KHIỂN PID (TỐC ĐỘ & GÓC)



// Hàm chạy liên tục mỗi 10ms để đọc encoder, tính toán PID và điều khiển motor. Cũng xử lý cả chế độ autotune nếu được kích hoạt.
void pid_control_task(void *pvParameters)
{
	static int32_t last_count = 0; // Lưu số xung cũ để tính delta
	static int32_t prev_target_rpm = 0; // Lưu giá trị RPM mục tiêu cũ để phát hiện thay đổi chiều
    static int log_timer = 0; // Biến đếm thời gian in log
    TickType_t last_wake_time = xTaskGetTickCount();  

    while (1)
    {
        vTaskDelayUntil(&last_wake_time, pdMS_TO_TICKS(10)); // Chu kỳ 10ms

        // --- BƯỚC KHỞI TẠO LẠI GỐC TỌA ĐỘ KHI CHUYỂN TỪ TỐC ĐỘ SANG GÓC ---
        if (reset_pos_flag) {
            portENTER_CRITICAL(&mux);
            pulse_count = 0;      // Xóa sạch bộ đếm xung cũ
            portEXIT_CRITICAL(&mux);
            last_count = 0;
            current_angle = 0;    // Reset góc hiện tại về 0
            filtered_rpm = 0;
            integral = 0;         // Xóa tích phân để không bị giật
            prev_error = 0;
            reset_pos_flag = false; // Tắt cờ
        }

        // --- BƯỚC 1: ĐỌC DỮ LIỆU ENCODERCƠ BẢN ---
        portENTER_CRITICAL(&mux);
        int32_t current_pulses = pulse_count;
        int32_t delta = current_pulses - last_count;
        last_count = current_pulses;
        portEXIT_CRITICAL(&mux);

        // Quy đổi ra Góc hiện tại 
        current_angle = (float)current_pulses * 360.0f / PULSES_PER_REV;


        // IN RA TERMINAL ĐỂ CALIB (Mỗi 0.5 giây in 1 lần)
        log_timer++;
        if (log_timer >= 50) { 
            ESP_LOGI("CALIB", "So xung: %d | Goc HT: %.2f do", (int)current_pulses, current_angle);
            log_timer = 0;
        }


        // Quy đổi ra RPM hiện tại
        float revs = (float)delta / PULSES_PER_REV;
        float rpm_raw = revs * 60.0f / 0.01f;
        rpm_int = (int32_t)rpm_raw;
        float rpm_filtered = iir_filter(rpm_raw);  //Lọc IIR để RPM ổn định hơn

        // --- BƯỚC 2: PHÂN LUỒNG ĐIỀU KHIỂN ---

        // CHẾ ĐỘ A: PID AUTOTUNE (Tìm thông số tốc độ)
        if (is_autotuning)
        {
            float output = 0;
            if (rpm_filtered > target_rpm)
            {
                output = tune_base_pwm - relay_step; 
                if (!last_state_above) { 
                    last_state_above = true;
                    int64_t now = esp_timer_get_time(); 
                    if (cross_count > 0) {
                        float period = (now - last_zero_cross_time) / 1000000.0f; 
                        Tu_sum += period;
                    }
                    last_zero_cross_time = now;
                    cross_count++;
                    if (cross_count >= 5) {
                        is_autotuning = false; 
                        float Tu = Tu_sum / 4.0f; 
                        float a = (tune_max_rpm - tune_min_rpm) / 2.0f; 
                        float d = relay_step; 
                        if (a > 0) {

							// Tính Ku và sau đó tính Kp, Ki, Kd theo công thức Ziegler-Nichols
                            float Ku = (4.0f * d) / (3.14159f * a);
                            Kp = 0.6f * Ku; Ki = (1.2f * Ku) / Tu; Kd = (0.075f * Ku) * Tu;
                        }
                        integral = 0; prev_error = 0;
                        motor_set(0, 0); 
                    }
                }
            }
            else {
                output = tune_base_pwm + relay_step; 
                if (last_state_above) last_state_above = false; 
            }
            if (cross_count > 0) {
                if (rpm_filtered > tune_max_rpm) tune_max_rpm = rpm_filtered;
                if (rpm_filtered < tune_min_rpm) tune_min_rpm = rpm_filtered;
            }
            pwm_output = (uint8_t)output;
            if (target_rpm > 0) motor_set(1, pwm_output); else motor_set(0, pwm_output);
        }

        // CHẾ ĐỘ B: ĐIỀU KHIỂN GÓC (VỊ TRÍ)
        else if (control_mode == 1)
        {
            float error = target_angle - current_angle;
            integral += error * 0.01f;
            
            // Chống bão hòa cho vị trí
            if (integral > 150) integral = 150;
            if (integral < -150) integral = -150;

            float derivative_pos = (error - prev_error) / 0.01f;
            prev_error = error;

            float output = Kp_pos * error + Ki_pos * integral + Kd_pos * derivative_pos;

            if (output > 255) output = 255;
            if (output < -255) output = -255;

            // Deadzone: Dừng hẳn khi cách đích dưới 1 độ
            if (fabs(error) < 1.0f) {

				// Khi đã gần đích, dừng hẳn để tránh giật và giữ nguyên vị trí
                motor_set(0, 0);
                integral = 0;
                pwm_output = 0;
            } else {
                if (output >= 0) {
                    pwm_output = (uint8_t)output;
                    motor_set(1, pwm_output);
                } else {
                    pwm_output = (uint8_t)fabs(output);
                    motor_set(0, pwm_output);
                }
            }
        }

        // CHẾ ĐỘ C: ĐIỀU KHIỂN TỐC ĐỘ (MANUAL PID)
        else
        {
			// Nếu phát hiện thay đổi chiều quay (từ dương sang âm hoặc ngược lại), reset tích phân và đạo hàm để tránh giật mạnh
            if ((prev_target_rpm > 0 && target_rpm < 0) || (prev_target_rpm < 0 && target_rpm > 0)) {
                integral = 0; prev_error = 0;
            }
            prev_target_rpm = target_rpm;

            float error = target_rpm - rpm_filtered;
            integral += error * 0.01f;

            // Ngăn tích phân quá lớn gây vọt lố
            if (integral > 500) integral = 500;
            if (integral < -500) integral = -500;

            float derivative_spd = (error - prev_error) / 0.01f;
            prev_error = error;

			float output = Kp * error + Ki * integral + Kd * derivative_spd;  // Tính toán PID cho tốc độ

            if (output > 255) output = 255;
            if (output < -255) output = -255;

            if (target_rpm == 0) {
                motor_set(0, 0); integral = 0; pwm_output = 0;
            } else if (output >= 0) {
                pwm_output = (uint8_t)output; motor_set(1, pwm_output);
            } else {
                pwm_output = (uint8_t)fabs(output); motor_set(0, pwm_output);
            }
        }
    }
}


// Hàm khởi tạo GPIO và PWM cho motor
void motor_init(void)  
{
    gpio_set_direction(IN1_GPIO, GPIO_MODE_OUTPUT);
    gpio_set_direction(IN2_GPIO, GPIO_MODE_OUTPUT);

    ledc_timer_config_t pwm_timer = {
		.duty_resolution = LEDC_TIMER_8_BIT,   // Độ phân giải 8 bit (0-255)
        .freq_hz = 5000,                     
        .speed_mode = LEDC_LOW_SPEED_MODE,
        .timer_num = LEDC_TIMER_0};
    ledc_timer_config(&pwm_timer);

    ledc_channel_config_t pwm_channel = {
        .channel = LEDC_CHANNEL_0,
        .duty = 0,
        .gpio_num = ENA_PWM_GPIO,
        .speed_mode = LEDC_LOW_SPEED_MODE,
        .timer_sel = LEDC_TIMER_0};
    ledc_channel_config(&pwm_channel);
}


// CÁC HÀM HTTP HANDLER



// Đọc tham số thực từ URL
esp_err_t handle_float_param(httpd_req_t *req, float *target, const char *name) {
    char query[64], param[16];
    if (httpd_req_get_url_query_str(req, query, sizeof(query)) == ESP_OK) {
        if (httpd_query_key_value(query, "value", param, sizeof(param)) == ESP_OK) {
            *target = atof(param);
        } else { return ESP_FAIL; }
    } else { return ESP_FAIL; }
    httpd_resp_sendstr(req, "OK");
    return ESP_OK;
}


// Đọc tham số nguyên từ URL
esp_err_t handle_int_param(httpd_req_t *req, int32_t *target, const char *name) {
    char query[64], param[16];
    if (httpd_req_get_url_query_str(req, query, sizeof(query)) == ESP_OK) {
        if (httpd_query_key_value(query, "value", param, sizeof(param)) == ESP_OK) {
            *target = (int32_t)atoi(param);
        } else { return ESP_FAIL; }
    } else { return ESP_FAIL; }
    httpd_resp_sendstr(req, "OK");
    return ESP_OK;
}

static esp_err_t status_handler(httpd_req_t *req) {
    char query[64], param[16];
    if (httpd_req_get_url_query_str(req, query, sizeof(query)) == ESP_OK) {
        if (httpd_query_key_value(query, "value", param, sizeof(param)) == ESP_OK) {
            if (strcmp(param, "200") == 0) {
                httpd_resp_sendstr(req, "ESP32 connected!");
                return ESP_OK;
            }
        }
    }
    return ESP_FAIL;
}

// xử lý Motor 

// Hàm nhận giá trị tốc độ từ URL và chuyển sang chế độ điều khiển tốc độ
static esp_err_t speed_handler(httpd_req_t *req) {
    esp_err_t ret = handle_int_param(req, (int32_t *)&target_rpm, "speed");
    if (ret == ESP_OK) {
        control_mode = 0; // Chuyển sang Tốc độ
        integral = 0;     
        prev_error = 0;
    }
    return ret;
}


// Hàm nhận giá trị góc từ URL và chuyển sang chế độ điều khiển góc
static esp_err_t angle_handler(httpd_req_t *req) {
    char query[64], param[16];
    if (httpd_req_get_url_query_str(req, query, sizeof(query)) == ESP_OK) {
        if (httpd_query_key_value(query, "value", param, sizeof(param)) == ESP_OK) {
            target_angle = atof(param);
            
            // Yêu cầu reset gốc tọa độ nếu đang ở chế độ tốc độ
            if (control_mode == 0) {
                reset_pos_flag = true;
            }

            control_mode = 1; // Chuyển sang Góc
            integral = 0; 
            prev_error = 0;
            ESP_LOGI("WEB", "Mode GOC: Target %.2f", target_angle);
        }
    }
    httpd_resp_sendstr(req, "Angle Set OK");
    return ESP_OK;
}

// Lấy dữ liệu về C# 
static esp_err_t rpm_handler(httpd_req_t *req) {
    char resp[32]; snprintf(resp, sizeof(resp), "%d", (int)rpm_int); 
    httpd_resp_sendstr(req, resp); return ESP_OK;
}
static esp_err_t target_handler(httpd_req_t *req) {
    char resp[32]; snprintf(resp, sizeof(resp), "%d", (int)target_rpm);
    httpd_resp_sendstr(req, resp); return ESP_OK;
}
static esp_err_t pwm_handler(httpd_req_t *req) {
    char resp[32]; snprintf(resp, sizeof(resp), "%d", pwm_output);
    httpd_resp_sendstr(req, resp); return ESP_OK;
}
static esp_err_t get_angle_handler(httpd_req_t *req) {
    char resp[32]; snprintf(resp, sizeof(resp), "%.2f", current_angle);
    httpd_resp_sendstr(req, resp); return ESP_OK;
}

// Autotune & PID 

// Hàm trả các hệ số PID hiện tại về cho C# để hiển thị sau khi đã được auto-tune
static esp_err_t autotune_handler(httpd_req_t *req) {
    tune_base_pwm = 100.0; target_rpm = 50; 
    tune_max_rpm = 0.0; tune_min_rpm = 10000.0;
    cross_count = 0; Tu_sum = 0.0;
    last_state_above = false;
    last_zero_cross_time = esp_timer_get_time();
    is_autotuning = true;
    httpd_resp_sendstr(req, "Autotune Started");
    return ESP_OK;
}
static esp_err_t kp_handler(httpd_req_t *req) { return handle_float_param(req, &Kp, "kp"); }
static esp_err_t ki_handler(httpd_req_t *req) { return handle_float_param(req, &Ki, "ki"); }
static esp_err_t kd_handler(httpd_req_t *req) { return handle_float_param(req, &Kd, "kd"); }
static esp_err_t get_kp_handler(httpd_req_t *req) { char resp[16]; snprintf(resp, sizeof(resp), "%.3f", Kp); httpd_resp_sendstr(req, resp); return ESP_OK; }
static esp_err_t get_ki_handler(httpd_req_t *req) { char resp[16]; snprintf(resp, sizeof(resp), "%.3f", Ki); httpd_resp_sendstr(req, resp); return ESP_OK; }
static esp_err_t get_kd_handler(httpd_req_t *req) { char resp[16]; snprintf(resp, sizeof(resp), "%.3f", Kd); httpd_resp_sendstr(req, resp); return ESP_OK; }



// KHỞI TẠO WEBSERVER & WIFI


// Tạo Webserver và đăng ký các API endpoint để điều khiển và lấy dữ liệu
httpd_handle_t start_webserver(void)
{
    httpd_config_t config = HTTPD_DEFAULT_CONFIG();
    config.max_uri_handlers = 24; 
    httpd_handle_t server = NULL;

    if (httpd_start(&server, &config) == ESP_OK)
    {
        httpd_uri_t _rpm = { .uri = "/rpm", .method = HTTP_GET, .handler = rpm_handler}; httpd_register_uri_handler(server, &_rpm);
        httpd_uri_t _target = { .uri = "/target", .method = HTTP_GET, .handler = target_handler}; httpd_register_uri_handler(server, &_target);
        httpd_uri_t _pwm = { .uri = "/pwm", .method = HTTP_GET, .handler = pwm_handler}; httpd_register_uri_handler(server, &_pwm);
        httpd_uri_t _kp = { .uri = "/kp", .method = HTTP_GET, .handler = kp_handler}; httpd_register_uri_handler(server, &_kp);
        httpd_uri_t _ki = { .uri = "/ki", .method = HTTP_GET, .handler = ki_handler}; httpd_register_uri_handler(server, &_ki);
        httpd_uri_t _kd = { .uri = "/kd", .method = HTTP_GET, .handler = kd_handler}; httpd_register_uri_handler(server, &_kd);
        httpd_uri_t _speed = { .uri = "/speed", .method = HTTP_GET, .handler = speed_handler}; httpd_register_uri_handler(server, &_speed);
        httpd_uri_t _status = { .uri = "/status", .method = HTTP_GET, .handler = status_handler}; httpd_register_uri_handler(server, &_status);
        httpd_uri_t _angle = { .uri = "/set_angle", .method = HTTP_GET, .handler = angle_handler}; httpd_register_uri_handler(server, &_angle);
        httpd_uri_t _get_angle = { .uri = "/get_angle", .method = HTTP_GET, .handler = get_angle_handler }; httpd_register_uri_handler(server, &_get_angle);
            
        httpd_uri_t _auto = { .uri = "/autotune", .method = HTTP_GET, .handler = autotune_handler }; httpd_register_uri_handler(server, &_auto);
        httpd_uri_t _gkp = { .uri = "/get_kp", .method = HTTP_GET, .handler = get_kp_handler }; httpd_register_uri_handler(server, &_gkp);
        httpd_uri_t _gki = { .uri = "/get_ki", .method = HTTP_GET, .handler = get_ki_handler }; httpd_register_uri_handler(server, &_gki);
        httpd_uri_t _gkd = { .uri = "/get_kd", .method = HTTP_GET, .handler = get_kd_handler }; httpd_register_uri_handler(server, &_gkd);
    }
    return server;   
}


// Hàm cho phép ESP32 phát WiFi riêng ở chế độ Access Point để C# có thể kết nối và điều khiển qua Webserver
void wifi_init_softap(void)
{
    ESP_ERROR_CHECK(esp_netif_init());                  
    ESP_ERROR_CHECK(esp_event_loop_create_default());   
    esp_netif_create_default_wifi_ap();                 

    wifi_init_config_t cfg = WIFI_INIT_CONFIG_DEFAULT();
    ESP_ERROR_CHECK(esp_wifi_init(&cfg));

    wifi_config_t ap_config = {
        .ap = {
            .ssid = "ESP32_BTL",
            .ssid_len = strlen("ESP32_BTL"),
            .password = "123456789",
            .channel = 1,
            .max_connection = 4,
            .authmode = WIFI_AUTH_WPA_WPA2_PSK},
    };

    ESP_ERROR_CHECK(esp_wifi_set_mode(WIFI_MODE_AP));
    ESP_ERROR_CHECK(esp_wifi_set_config(WIFI_IF_AP, &ap_config));
    ESP_ERROR_CHECK(esp_wifi_start());
    ESP_LOGI(TAG, "WiFi AP started. SSID:%s", ap_config.ap.ssid);
}


// Hàm khởi tạo WiFi và Webserver trong một task riêng biệt để không ảnh hưởng đến task điều khiển PID, đảm bảo cả hai hoạt động mượt mà và ổn định
void wifi_web_task(void *pvParameters)
{
    esp_err_t ret = nvs_flash_init();
    if (ret == ESP_ERR_NVS_NO_FREE_PAGES || ret == ESP_ERR_NVS_NEW_VERSION_FOUND) {
        ESP_ERROR_CHECK(nvs_flash_erase());
        ret = nvs_flash_init();
    }
    ESP_ERROR_CHECK(ret);
    wifi_init_softap();
    start_webserver();
    vTaskDelete(NULL);
}


// Hàm main, khởi tạo motor, encoder, WiFi và Webserver, đồng thời tạo task điều khiển PID để chạy liên tục trong nền, đảm bảo hệ thống hoạt động ổn định và đáp ứng nhanh các lệnh từ C# qua Webserver.
void app_main(void)
{
    motor_init();
    pwm_queue = xQueueCreate(1, sizeof(uint8_t));
    xTaskCreatePinnedToCore(pid_control_task, "pid_control_task", 2048, NULL, 5, NULL, 0);  // tạo task điều khiển PID
	xTaskCreatePinnedToCore(wifi_web_task, "wifi_web_task", 8192, NULL, 3, NULL, 1);  // tạo task WiFi & Webserver
    
    gpio_config_t io_conf = {
		.intr_type = GPIO_INTR_POSEDGE, // Ngắt cạnh lên của A
		.mode = GPIO_MODE_INPUT, // A là input
		.pin_bit_mask = (1ULL << ENCODER_A_PIN),  // Chỉ cấu hình chân A để ngắt
		.pull_up_en = GPIO_PULLUP_ENABLE };        // Kích hoạt pull-up cho A
	gpio_config(&io_conf);    // Cấu hình chân A với ngắt cạnh lên và pull-up
	gpio_set_direction(ENCODER_B_PIN, GPIO_MODE_INPUT); // B là input
	gpio_set_pull_mode(ENCODER_B_PIN, GPIO_PULLUP_ONLY);  // Kích hoạt pull-up cho B
    
	gpio_install_isr_service(0);  // Cài đặt ngắt GPIO
	gpio_isr_handler_add(ENCODER_A_PIN, encoder_isr_handler, NULL);  // Gắn hàm xử lý ngắt cho chân A
}