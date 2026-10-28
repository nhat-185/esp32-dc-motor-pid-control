namespace giaodien
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.connect_btn = new System.Windows.Forms.Button();
            this.disconnect_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.on_btn = new System.Windows.Forms.Button();
            this.off_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.kp = new System.Windows.Forms.TextBox();
            this.ki = new System.Windows.Forms.TextBox();
            this.kd = new System.Windows.Forms.TextBox();
            this.config = new System.Windows.Forms.Button();
            this.status_motor = new System.Windows.Forms.Button();
            this.status_connect = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbl_pid = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxBaudrate = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rotation = new System.Windows.Forms.Label();
            this.reverse = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.stop_chart = new System.Windows.Forms.Button();
            this.draw_chart = new System.Windows.Forms.Button();
            this.re_chart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.speed_bar = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_SetAngle = new System.Windows.Forms.Button();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speed_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 335);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // connect_btn
            // 
            this.connect_btn.BackColor = System.Drawing.Color.Lime;
            this.connect_btn.Location = new System.Drawing.Point(26, 67);
            this.connect_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(208, 65);
            this.connect_btn.TabIndex = 3;
            this.connect_btn.Text = "CONNECT";
            this.connect_btn.UseVisualStyleBackColor = false;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // disconnect_btn
            // 
            this.disconnect_btn.BackColor = System.Drawing.Color.Red;
            this.disconnect_btn.Location = new System.Drawing.Point(26, 161);
            this.disconnect_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.disconnect_btn.Name = "disconnect_btn";
            this.disconnect_btn.Size = new System.Drawing.Size(208, 71);
            this.disconnect_btn.TabIndex = 4;
            this.disconnect_btn.Text = "DISCONNECT";
            this.disconnect_btn.UseVisualStyleBackColor = false;
            this.disconnect_btn.Click += new System.EventHandler(this.disconnect_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 29);
            this.label2.TabIndex = 6;
            this.label2.Text = "Speed";
            // 
            // on_btn
            // 
            this.on_btn.BackColor = System.Drawing.Color.Lime;
            this.on_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.on_btn.Location = new System.Drawing.Point(6, 42);
            this.on_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.on_btn.Name = "on_btn";
            this.on_btn.Size = new System.Drawing.Size(102, 56);
            this.on_btn.TabIndex = 7;
            this.on_btn.Text = "ON";
            this.on_btn.UseVisualStyleBackColor = false;
            this.on_btn.Click += new System.EventHandler(this.on_btn_Click);
            // 
            // off_btn
            // 
            this.off_btn.BackColor = System.Drawing.Color.Red;
            this.off_btn.Location = new System.Drawing.Point(172, 43);
            this.off_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.off_btn.Name = "off_btn";
            this.off_btn.Size = new System.Drawing.Size(102, 55);
            this.off_btn.TabIndex = 8;
            this.off_btn.Text = "OFF";
            this.off_btn.UseVisualStyleBackColor = false;
            this.off_btn.Click += new System.EventHandler(this.off_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "Kp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 29);
            this.label4.TabIndex = 10;
            this.label4.Text = "Ki";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 29);
            this.label5.TabIndex = 11;
            this.label5.Text = "Kd";
            // 
            // kp
            // 
            this.kp.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kp.Location = new System.Drawing.Point(129, 64);
            this.kp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.kp.Name = "kp";
            this.kp.Size = new System.Drawing.Size(100, 34);
            this.kp.TabIndex = 12;
            // 
            // ki
            // 
            this.ki.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ki.Location = new System.Drawing.Point(129, 110);
            this.ki.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ki.Name = "ki";
            this.ki.Size = new System.Drawing.Size(100, 34);
            this.ki.TabIndex = 13;
            // 
            // kd
            // 
            this.kd.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kd.Location = new System.Drawing.Point(129, 153);
            this.kd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.kd.Name = "kd";
            this.kd.Size = new System.Drawing.Size(100, 34);
            this.kd.TabIndex = 14;
            // 
            // config
            // 
            this.config.BackColor = System.Drawing.Color.Lime;
            this.config.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.config.Location = new System.Drawing.Point(36, 252);
            this.config.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.config.Name = "config";
            this.config.Size = new System.Drawing.Size(193, 86);
            this.config.TabIndex = 15;
            this.config.Text = "CONFIG CONTROL";
            this.config.UseVisualStyleBackColor = false;
            this.config.Click += new System.EventHandler(this.config_Click);
            // 
            // status_motor
            // 
            this.status_motor.Location = new System.Drawing.Point(6, 126);
            this.status_motor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.status_motor.Name = "status_motor";
            this.status_motor.Size = new System.Drawing.Size(121, 48);
            this.status_motor.TabIndex = 16;
            this.status_motor.Text = "Status";
            this.status_motor.UseVisualStyleBackColor = true;
            this.status_motor.Click += new System.EventHandler(this.status_motor_Click);
            // 
            // status_connect
            // 
            this.status_connect.AutoSize = true;
            this.status_connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_connect.Location = new System.Drawing.Point(99, 340);
            this.status_connect.Name = "status_connect";
            this.status_connect.Size = new System.Drawing.Size(61, 20);
            this.status_connect.TabIndex = 19;
            this.status_connect.Text = "status";
            this.status_connect.Click += new System.EventHandler(this.status_connect_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbl_pid
            // 
            this.lbl_pid.AutoSize = true;
            this.lbl_pid.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pid.Location = new System.Drawing.Point(84, 25);
            this.lbl_pid.Name = "lbl_pid";
            this.lbl_pid.Size = new System.Drawing.Size(81, 29);
            this.lbl_pid.TabIndex = 20;
            this.lbl_pid.Text = "status";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(16, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 29);
            this.label7.TabIndex = 21;
            this.label7.Text = "PID:";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed.Location = new System.Drawing.Point(129, 198);
            this.txtSpeed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(100, 34);
            this.txtSpeed.TabIndex = 22;
            this.txtSpeed.TextChanged += new System.EventHandler(this.txtSpeed_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAngle);
            this.groupBox1.Controls.Add(this.btn_SetAngle);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cbxBaudrate);
            this.groupBox1.Controls.Add(this.connect_btn);
            this.groupBox1.Controls.Add(this.disconnect_btn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.status_connect);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(249, 755);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CONNECT WIFI";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(27, 421);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(171, 75);
            this.button1.TabIndex = 24;
            this.button1.Text = "Auto Tune";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(-5, 264);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 29);
            this.label8.TabIndex = 23;
            this.label8.Text = "Baudrate";
            // 
            // cbxBaudrate
            // 
            this.cbxBaudrate.FormattingEnabled = true;
            this.cbxBaudrate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbxBaudrate.Location = new System.Drawing.Point(122, 261);
            this.cbxBaudrate.Name = "cbxBaudrate";
            this.cbxBaudrate.Size = new System.Drawing.Size(121, 37);
            this.cbxBaudrate.TabIndex = 20;
            this.cbxBaudrate.SelectedIndexChanged += new System.EventHandler(this.cbxBaudrate_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.rotation);
            this.groupBox2.Controls.Add(this.reverse);
            this.groupBox2.Controls.Add(this.on_btn);
            this.groupBox2.Controls.Add(this.off_btn);
            this.groupBox2.Controls.Add(this.status_motor);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 44);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(296, 707);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controller";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lbl_pid);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtSpeed);
            this.groupBox3.Controls.Add(this.kp);
            this.groupBox3.Controls.Add(this.ki);
            this.groupBox3.Controls.Add(this.kd);
            this.groupBox3.Controls.Add(this.config);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(33, 296);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(241, 392);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PID CONTROL";
            // 
            // rotation
            // 
            this.rotation.AutoSize = true;
            this.rotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotation.Location = new System.Drawing.Point(133, 145);
            this.rotation.Name = "rotation";
            this.rotation.Size = new System.Drawing.Size(73, 20);
            this.rotation.TabIndex = 30;
            this.rotation.Text = "rotation";
            // 
            // reverse
            // 
            this.reverse.BackColor = System.Drawing.Color.Yellow;
            this.reverse.Location = new System.Drawing.Point(69, 195);
            this.reverse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reverse.Name = "reverse";
            this.reverse.Size = new System.Drawing.Size(158, 59);
            this.reverse.TabIndex = 17;
            this.reverse.Text = "Reverse";
            this.reverse.UseVisualStyleBackColor = false;
            this.reverse.Click += new System.EventHandler(this.reverse_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // stop_chart
            // 
            this.stop_chart.BackColor = System.Drawing.Color.Red;
            this.stop_chart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stop_chart.Location = new System.Drawing.Point(1031, 197);
            this.stop_chart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stop_chart.Name = "stop_chart";
            this.stop_chart.Size = new System.Drawing.Size(176, 96);
            this.stop_chart.TabIndex = 18;
            this.stop_chart.Text = "Stop ";
            this.stop_chart.UseVisualStyleBackColor = false;
            this.stop_chart.Click += new System.EventHandler(this.stop_chart_Click);
            // 
            // draw_chart
            // 
            this.draw_chart.BackColor = System.Drawing.Color.Lime;
            this.draw_chart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.draw_chart.Location = new System.Drawing.Point(1031, 86);
            this.draw_chart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.draw_chart.Name = "draw_chart";
            this.draw_chart.Size = new System.Drawing.Size(176, 86);
            this.draw_chart.TabIndex = 17;
            this.draw_chart.Text = "Generate ";
            this.draw_chart.UseVisualStyleBackColor = false;
            this.draw_chart.Click += new System.EventHandler(this.draw_chart_Click);
            // 
            // re_chart
            // 
            this.re_chart.BackColor = System.Drawing.Color.Yellow;
            this.re_chart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.re_chart.Location = new System.Drawing.Point(1031, 307);
            this.re_chart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.re_chart.Name = "re_chart";
            this.re_chart.Size = new System.Drawing.Size(176, 87);
            this.re_chart.TabIndex = 19;
            this.re_chart.Text = "Reset";
            this.re_chart.UseVisualStyleBackColor = false;
            this.re_chart.Click += new System.EventHandler(this.re_chart_Click);
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(321, 55);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(704, 563);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.speed_bar);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.chart1);
            this.groupBox4.Controls.Add(this.re_chart);
            this.groupBox4.Controls.Add(this.draw_chart);
            this.groupBox4.Controls.Add(this.stop_chart);
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(267, 15);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(1244, 755);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CHART SPEED";
            // 
            // speed_bar
            // 
            this.speed_bar.Location = new System.Drawing.Point(515, 634);
            this.speed_bar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.speed_bar.Maximum = 330;
            this.speed_bar.Name = "speed_bar";
            this.speed_bar.Size = new System.Drawing.Size(348, 56);
            this.speed_bar.TabIndex = 27;
            this.speed_bar.TickFrequency = 5;
            this.speed_bar.Value = 100;
            this.speed_bar.Scroll += new System.EventHandler(this.speed_bar_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(315, 658);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 32);
            this.label6.TabIndex = 28;
            this.label6.Text = "Set Speed: ";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // btn_SetAngle
            // 
            this.btn_SetAngle.Location = new System.Drawing.Point(53, 538);
            this.btn_SetAngle.Name = "btn_SetAngle";
            this.btn_SetAngle.Size = new System.Drawing.Size(130, 48);
            this.btn_SetAngle.TabIndex = 25;
            this.btn_SetAngle.Text = "set angle";
            this.btn_SetAngle.UseVisualStyleBackColor = true;
            this.btn_SetAngle.Click += new System.EventHandler(this.btn_SetAngle_Click);
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(71, 592);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(100, 34);
            this.txtAngle.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1523, 829);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speed_bar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Button disconnect_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button on_btn;
        private System.Windows.Forms.Button off_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox kp;
        private System.Windows.Forms.TextBox ki;
        private System.Windows.Forms.TextBox kd;
        private System.Windows.Forms.Button config;
        private System.Windows.Forms.Button status_motor;
        private System.Windows.Forms.Label status_connect;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbl_pid;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button reverse;
        private System.Windows.Forms.Label rotation;
        private System.Windows.Forms.Button stop_chart;
        private System.Windows.Forms.Button draw_chart;
        private System.Windows.Forms.Button re_chart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TrackBar speed_bar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxBaudrate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.Button btn_SetAngle;
    }
}

