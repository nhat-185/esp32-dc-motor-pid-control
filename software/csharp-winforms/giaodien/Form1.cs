using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace giaodien
{
    public partial class Form1 : Form
    {
        #region --- VARIABLES ---

        string esp32IP = "http://192.168.4.1";
        double elapsedTime = 0;
        HttpClient httpClient = new HttpClient();

        private const int filterSize = 20;
        private Queue<double> speedBuffer = new Queue<double>();

        #endregion

        public Form1()
        {
            InitializeComponent();
            // Tối ưu: Đặt timeout 10 giây để tránh lỗi văng app khi Auto-tune chờ lâu
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupChart();
            UpdateUIStatus(false); // Khóa các nút khi chưa kết nối
        }

        private void SetupChart()
        {
         
        {
                var area = chart1.ChartAreas[0];
                chart1.Series[0].ChartType = SeriesChartType.Line;
                chart1.Series[0].Name = "Speed";
                area.AxisX.Title = "Thời gian (s)";
                area.AxisY.Title = "Tốc độ (RPM)";
                area.AxisX.MajorGrid.LineColor = Color.LightGray;
                area.AxisY.MajorGrid.LineColor = Color.LightGray;
                chart1.Series[0].Color = Color.Blue;
                chart1.Series[0].BorderWidth = 2;
                area.AxisX.LabelStyle.Format = "0.0";

                area.AxisY.Minimum = -400;
                area.AxisY.Maximum = 400;

                // THÊM DÒNG NÀY ĐỂ CHIA VẠCH MỖI 100 ĐƠN VỊ:
                area.AxisY.Interval = 100;
            }
        }

        #region --- NETWORK FUNCTIONS ---

        private async Task SendHttp(string path)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(esp32IP + path);
                if (response.IsSuccessStatusCode)
                {
                    lbl_pid.Text = "Lệnh: " + path + " - OK";
                    lbl_pid.ForeColor = Color.Green;
                }
            }
            catch
            {
                lbl_pid.Text = "Lỗi kết nối!";
                lbl_pid.ForeColor = Color.Red;
            }
        }

        private void UpdateUIStatus(bool connected)
        {
            // Group điều khiển
            on_btn.Enabled = connected;
            off_btn.Enabled = connected;
            config.Enabled = connected;
            reverse.Enabled = connected;

            // Group đồ thị
            draw_chart.Enabled = connected;
            stop_chart.Enabled = connected;
            re_chart.Enabled = connected;

            // Group kết nối
            disconnect_btn.Enabled = connected;
            connect_btn.Enabled = !connected;

            // Nút AutoTune
            button1.Enabled = connected;

            status_connect.Text = connected ? "Đã kết nối" : "Chưa kết nối";
            status_connect.ForeColor = connected ? Color.Green : Color.Gray;
        }

        #endregion

        #region --- BUTTON EVENTS ---

        private async void connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                status_connect.Text = "Đang thử kết nối...";
                // Kiểm tra xem ESP32 có sống không bằng cách gọi thử /rpm
                var response = await httpClient.GetAsync(esp32IP + "/rpm");

                if (response.IsSuccessStatusCode)
                {
                    timer2.Start(); // Bật timer cập nhật trạng thái quay
                    UpdateUIStatus(true);
                }
            }
            catch
            {
                MessageBox.Show("Không tìm thấy ESP32. Hãy kết nối WiFi: ESP32_BTL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateUIStatus(false);
            }
        }

        private void disconnect_btn_Click(object sender, EventArgs e)
        {
            // Phải tắt cả 2 timer để không báo lỗi mạng ngầm
            timer1.Stop();
            timer2.Stop();
            UpdateUIStatus(false);
        }

        private async void config_Click(object sender, EventArgs e)
        {
            await SendHttp("/kp?value=" + kp.Text);
            await SendHttp("/ki?value=" + ki.Text);
            await SendHttp("/kd?value=" + kd.Text);
            await SendHttp("/speed?value=" + txtSpeed.Text);
        }

        private async void cbxBaudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxBaudrate.SelectedItem != null)
            {
                string selectedBaudrate = cbxBaudrate.SelectedItem.ToString();
                await SendHttp("/baudrate?value=" + selectedBaudrate);
            }
        }

        private async void on_btn_Click(object sender, EventArgs e) => await SendHttp("/speed?value=" + txtSpeed.Text);

        private async void off_btn_Click(object sender, EventArgs e) => await SendHttp("/speed?value=0");

        private async void reverse_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSpeed.Text, out int speed_m))
            {
                speed_m = -speed_m;
                txtSpeed.Text = speed_m.ToString();
                await SendHttp("/speed?value=" + speed_m);
            }
        }

        private void speed_bar_Scroll(object sender, EventArgs e) => txtSpeed.Text = speed_bar.Value.ToString();

        #endregion

        #region --- AUTO-TUNE (CỰC QUAN TRỌNG) ---

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Khóa toàn bộ để an toàn
                button1.Enabled = false;
                config.Enabled = false;
                on_btn.Enabled = false;
                off_btn.Enabled = false;

                // Tạm ngưng vẽ đồ thị và check trạng thái để NHƯỜNG MẠNG cho ESP32 chạy Auto-tune
                timer1.Stop();
                timer2.Stop();

                lbl_pid.Text = "Đang Auto-Tuning... (Chờ 6 giây)";
                lbl_pid.ForeColor = Color.Orange;

                // 1. Kích hoạt
                HttpResponseMessage response = await httpClient.GetAsync(esp32IP + "/autotune");
                if (!response.IsSuccessStatusCode)
                {
                    lbl_pid.Text = "Lỗi kích hoạt Auto-tune!";
                    return;
                }

                // 2. Chờ ESP32 thực hiện dao động Ziegler-Nichols (Delay 6.5s)
                await Task.Delay(6500);

                // 3. Lấy kết quả
                string newKp = await httpClient.GetStringAsync(esp32IP + "/get_kp");
                string newKi = await httpClient.GetStringAsync(esp32IP + "/get_ki");
                string newKd = await httpClient.GetStringAsync(esp32IP + "/get_kd");

                // 4. Hiển thị
                kp.Text = newKp.Trim();
                ki.Text = newKi.Trim();
                kd.Text = newKd.Trim();

                lbl_pid.Text = "Auto-tune thành công!";
                lbl_pid.ForeColor = Color.DeepSkyBlue;
            }
            catch (Exception ex)
            {
                lbl_pid.Text = "Lỗi Auto-tune: " + ex.Message;
                lbl_pid.ForeColor = Color.Red;
            }
            finally
            {
                // Mở khóa lại và bật lại các Timer
                UpdateUIStatus(true);
                timer1.Start();
                timer2.Start();
            }
        }

        #endregion

        #region --- TIMER & CHART ---

        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string result = await httpClient.GetStringAsync(esp32IP + "/rpm");
                if (double.TryParse(result, out double speed))
                {
                    // Lọc trung bình trượt
                    speedBuffer.Enqueue(speed);
                    if (speedBuffer.Count > filterSize) speedBuffer.Dequeue();
                    double filteredSpeed = speedBuffer.Average();

                    elapsedTime += (timer1.Interval / 1000.0);
                    chart1.Series[0].Points.AddXY(elapsedTime, filteredSpeed);

                    // Cuộn đồ thị (giữ khung 10s hiển thị)
                    if (elapsedTime > 10)
                    {
                        chart1.ChartAreas[0].AxisX.Minimum = elapsedTime - 10;
                        chart1.ChartAreas[0].AxisX.Maximum = elapsedTime;
                        if (chart1.Series[0].Points.Count > 100) chart1.Series[0].Points.RemoveAt(0);
                    }
                }
            }
            catch
            {
                timer1.Stop();
                UpdateUIStatus(false);
            }
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                string result = await httpClient.GetStringAsync(esp32IP + "/rpm");
                if (double.TryParse(result, out double speed))
                {
                    if (Math.Abs(speed) < 1)
                    {
                        rotation.Text = "Dừng";
                        status_motor.BackColor = Color.Red;
                    }
                    else
                    {
                        rotation.Text = speed > 0 ? "Thuận" : "Ngược";
                        status_motor.BackColor = Color.Green;
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi ngầm để app không bị crash
            }
        }

        private void draw_chart_Click(object sender, EventArgs e) => timer1.Start();
        private void stop_chart_Click(object sender, EventArgs e) => timer1.Stop();
        private void re_chart_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            elapsedTime = 0;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 10;
        }

        #endregion

        #region --- UNUSED UI EVENTS (DO NOT DELETE) ---
        // Các hàm rác (Click nhầm) - Giữ để không lỗi Designer
        private void label1_Click(object sender, EventArgs e) { }
        private void txtSpeed_TextChanged(object sender, EventArgs e) { }
        private void status_motor_Click(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void chart1_Click(object sender, EventArgs e) { }
        private void status_connect_Click(object sender, EventArgs e) { }
        #endregion

        private async void btn_SetAngle_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng có nhập gì không
            if (!string.IsNullOrEmpty(txtAngle.Text))
            {
                // Gửi lệnh đến đường dẫn /set_angle mà chúng ta đã cấu hình ở ESP32
                await SendHttp("/set_angle?value=" + txtAngle.Text);
            }
        }
    }
}