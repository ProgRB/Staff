namespace Tabel
{
    partial class EditGr_Work_Day
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btCompact_Time_Zone = new System.Windows.Forms.Button();
            this.btHoliday_Time_Zone = new System.Windows.Forms.Button();
            this.btTime_Zone = new System.Windows.Forms.Button();
            this.mbNumber_Day = new System.Windows.Forms.MaskedTextBox();
            this.cbCompact_time_zone_id = new System.Windows.Forms.ComboBox();
            this.cbTime_Zone = new System.Windows.Forms.ComboBox();
            this.cbHoliday_time_zone_id = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chSign_Evening_Time = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 174);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 38);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(488, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Location = new System.Drawing.Point(375, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(104, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chSign_Evening_Time);
            this.groupBox1.Controls.Add(this.btCompact_Time_Zone);
            this.groupBox1.Controls.Add(this.btHoliday_Time_Zone);
            this.groupBox1.Controls.Add(this.btTime_Zone);
            this.groupBox1.Controls.Add(this.mbNumber_Day);
            this.groupBox1.Controls.Add(this.cbCompact_time_zone_id);
            this.groupBox1.Controls.Add(this.cbTime_Zone);
            this.groupBox1.Controls.Add(this.cbHoliday_time_zone_id);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btCompact_Time_Zone
            // 
            this.btCompact_Time_Zone.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btCompact_Time_Zone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCompact_Time_Zone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCompact_Time_Zone.Location = new System.Drawing.Point(532, 131);
            this.btCompact_Time_Zone.Name = "btCompact_Time_Zone";
            this.btCompact_Time_Zone.Size = new System.Drawing.Size(31, 23);
            this.btCompact_Time_Zone.TabIndex = 15;
            this.btCompact_Time_Zone.Tag = "Compact";
            this.btCompact_Time_Zone.Text = "...";
            this.btCompact_Time_Zone.UseVisualStyleBackColor = true;
            // 
            // btHoliday_Time_Zone
            // 
            this.btHoliday_Time_Zone.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btHoliday_Time_Zone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btHoliday_Time_Zone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btHoliday_Time_Zone.Location = new System.Drawing.Point(532, 94);
            this.btHoliday_Time_Zone.Name = "btHoliday_Time_Zone";
            this.btHoliday_Time_Zone.Size = new System.Drawing.Size(31, 23);
            this.btHoliday_Time_Zone.TabIndex = 16;
            this.btHoliday_Time_Zone.Tag = "Holiday";
            this.btHoliday_Time_Zone.Text = "...";
            this.btHoliday_Time_Zone.UseVisualStyleBackColor = true;
            // 
            // btTime_Zone
            // 
            this.btTime_Zone.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btTime_Zone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btTime_Zone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btTime_Zone.Location = new System.Drawing.Point(532, 55);
            this.btTime_Zone.Name = "btTime_Zone";
            this.btTime_Zone.Size = new System.Drawing.Size(31, 23);
            this.btTime_Zone.TabIndex = 8;
            this.btTime_Zone.Tag = "Common";
            this.btTime_Zone.Text = "...";
            this.btTime_Zone.UseVisualStyleBackColor = true;
            // 
            // mbNumber_Day
            // 
            this.mbNumber_Day.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbNumber_Day.Location = new System.Drawing.Point(155, 21);
            this.mbNumber_Day.Mask = "00";
            this.mbNumber_Day.Name = "mbNumber_Day";
            this.mbNumber_Day.Size = new System.Drawing.Size(46, 21);
            this.mbNumber_Day.TabIndex = 0;
            // 
            // cbCompact_time_zone_id
            // 
            this.cbCompact_time_zone_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbCompact_time_zone_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCompact_time_zone_id.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCompact_time_zone_id.FormattingEnabled = true;
            this.cbCompact_time_zone_id.Location = new System.Drawing.Point(155, 131);
            this.cbCompact_time_zone_id.Name = "cbCompact_time_zone_id";
            this.cbCompact_time_zone_id.Size = new System.Drawing.Size(371, 23);
            this.cbCompact_time_zone_id.TabIndex = 14;
            // 
            // cbTime_Zone
            // 
            this.cbTime_Zone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbTime_Zone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbTime_Zone.BackColor = System.Drawing.Color.White;
            this.cbTime_Zone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbTime_Zone.FormattingEnabled = true;
            this.cbTime_Zone.Location = new System.Drawing.Point(155, 55);
            this.cbTime_Zone.Name = "cbTime_Zone";
            this.cbTime_Zone.Size = new System.Drawing.Size(371, 23);
            this.cbTime_Zone.TabIndex = 1;
            // 
            // cbHoliday_time_zone_id
            // 
            this.cbHoliday_time_zone_id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbHoliday_time_zone_id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbHoliday_time_zone_id.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbHoliday_time_zone_id.FormattingEnabled = true;
            this.cbHoliday_time_zone_id.Location = new System.Drawing.Point(155, 94);
            this.cbHoliday_time_zone_id.Name = "cbHoliday_time_zone_id";
            this.cbHoliday_time_zone_id.Size = new System.Drawing.Size(371, 23);
            this.cbHoliday_time_zone_id.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "Время работы для обычного дня";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(12, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 34);
            this.label4.TabIndex = 11;
            this.label4.Text = "Время работы для сокращенного дня";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Номер дня графика";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 35);
            this.label3.TabIndex = 12;
            this.label3.Text = "Время работы для выходного дня";
            // 
            // chSign_Evening_Time
            // 
            this.chSign_Evening_Time.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_Evening_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Evening_Time.Location = new System.Drawing.Point(287, 16);
            this.chSign_Evening_Time.Name = "chSign_Evening_Time";
            this.chSign_Evening_Time.Size = new System.Drawing.Size(276, 27);
            this.chSign_Evening_Time.TabIndex = 17;
            this.chSign_Evening_Time.Text = "Признак расчета вечернего времени";
            this.chSign_Evening_Time.UseVisualStyleBackColor = true;
            // 
            // EditGr_Work_Day
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 212);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditGr_Work_Day";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Данные дня графика";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbTime_Zone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mbNumber_Day;
        private System.Windows.Forms.Button btTime_Zone;
        private System.Windows.Forms.Button btCompact_Time_Zone;
        private System.Windows.Forms.Button btHoliday_Time_Zone;
        private System.Windows.Forms.ComboBox cbCompact_time_zone_id;
        private System.Windows.Forms.ComboBox cbHoliday_time_zone_id;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chSign_Evening_Time;
    }
}