namespace Tabel
{
    partial class ReportForEconDev
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new Elegant.Ui.Label();
            this.label3 = new Elegant.Ui.Label();
            this.ssFilterForReport = new Kadr.Classes.SubdivSelector();
            this.dtpEndPeriod = new System.Windows.Forms.DateTimePicker();
            this.dtpBeginPeriod = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new Elegant.Ui.Button();
            this.btOrderTruancy = new Elegant.Ui.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ssFilterForReport);
            this.groupBox1.Controls.Add(this.dtpEndPeriod);
            this.groupBox1.Controls.Add(this.dtpBeginPeriod);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 87);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Id = "c34e75ef-3a99-47ea-aa76-0823bb30a21f";
            this.label4.Location = new System.Drawing.Point(315, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 18);
            this.label4.TabIndex = 35;
            this.label4.Text = "Окончание периода";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Id = "a0eb34c6-ecd4-4d6c-bb16-79662f2125a8";
            this.label3.Location = new System.Drawing.Point(22, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 15);
            this.label3.TabIndex = 35;
            this.label3.Text = "Начало периода";
            // 
            // ssFilterForReport
            // 
            this.ssFilterForReport.BackColor = System.Drawing.Color.Transparent;
            this.ssFilterForReport.ByRule = "TABLE";
            this.ssFilterForReport.Location = new System.Drawing.Point(22, 52);
            this.ssFilterForReport.Name = "ssFilterForReport";
            this.ssFilterForReport.Size = new System.Drawing.Size(497, 20);
            this.ssFilterForReport.TabIndex = 33;
            // 
            // dtpEndPeriod
            // 
            this.dtpEndPeriod.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpEndPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndPeriod.Location = new System.Drawing.Point(434, 17);
            this.dtpEndPeriod.Name = "dtpEndPeriod";
            this.dtpEndPeriod.Size = new System.Drawing.Size(85, 20);
            this.dtpEndPeriod.TabIndex = 32;
            this.dtpEndPeriod.Value = new System.DateTime(2013, 2, 16, 0, 0, 0, 0);
            // 
            // dtpBeginPeriod
            // 
            this.dtpBeginPeriod.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpBeginPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBeginPeriod.Location = new System.Drawing.Point(120, 17);
            this.dtpBeginPeriod.Name = "dtpBeginPeriod";
            this.dtpBeginPeriod.Size = new System.Drawing.Size(85, 20);
            this.dtpBeginPeriod.TabIndex = 31;
            this.dtpBeginPeriod.Value = new System.DateTime(2013, 2, 16, 0, 0, 0, 0);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btOrderTruancy);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 87);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(541, 37);
            this.panel1.TabIndex = 3;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "a1883447-d218-42f8-9485-e236545186f6";
            this.btExit.Location = new System.Drawing.Point(444, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            // 
            // btOrderTruancy
            // 
            this.btOrderTruancy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOrderTruancy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOrderTruancy.Id = "ce28ef55-5c63-4317-ac41-8dd9f0dec883";
            this.btOrderTruancy.Location = new System.Drawing.Point(323, 8);
            this.btOrderTruancy.Name = "btOrderTruancy";
            this.btOrderTruancy.Size = new System.Drawing.Size(115, 23);
            this.btOrderTruancy.TabIndex = 0;
            this.btOrderTruancy.Text = "Сформировать";
            this.btOrderTruancy.Click += new System.EventHandler(this.btOrderTruancy_Click);
            // 
            // ReportForEconDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 124);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportForEconDev";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Формирование отчета по оправдательному документу";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btOrderTruancy;
        private System.Windows.Forms.DateTimePicker dtpEndPeriod;
        private System.Windows.Forms.DateTimePicker dtpBeginPeriod;
        private Elegant.Ui.Label label4;
        private Elegant.Ui.Label label3;
        public Kadr.Classes.SubdivSelector ssFilterForReport;
    }
}