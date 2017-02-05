namespace Kadr.Shtat
{
    partial class SelectInterval
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
            this.mtBegDate = new System.Windows.Forms.MaskedTextBox();
            this.mtEndDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.subdivSelector1 = new Kadr.Classes.SubdivSelector();
            this.Degree = new Elegant.Ui.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mtBegDate
            // 
            this.mtBegDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mtBegDate.Location = new System.Drawing.Point(143, 55);
            this.mtBegDate.Mask = "00/00/0000";
            this.mtBegDate.Name = "mtBegDate";
            this.mtBegDate.Size = new System.Drawing.Size(80, 21);
            this.mtBegDate.TabIndex = 0;
            this.mtBegDate.ValidatingType = typeof(System.DateTime);
            // 
            // mtEndDate
            // 
            this.mtEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mtEndDate.Location = new System.Drawing.Point(268, 55);
            this.mtEndDate.Mask = "00/00/0000";
            this.mtEndDate.Name = "mtEndDate";
            this.mtEndDate.Size = new System.Drawing.Size(80, 21);
            this.mtEndDate.TabIndex = 1;
            this.mtEndDate.ValidatingType = typeof(System.DateTime);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(590, 33);
            this.label1.TabIndex = 2;
            this.label1.Text = "Введите интервал для формирования отчета об изменениях штатного расписания и выбе" +
                "рите подразделение:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(235, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "по ";
            // 
            // btOk
            // 
            this.btOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Location = new System.Drawing.Point(388, 9);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(110, 23);
            this.btOk.TabIndex = 5;
            this.btOk.Text = "Формировать";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(524, 9);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Выход";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.subdivSelector1);
            this.groupBox1.Controls.Add(this.Degree);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.mtEndDate);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.mtBegDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(623, 182);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // subdivSelector1
            // 
            this.subdivSelector1.BackColor = System.Drawing.Color.Transparent;
            this.subdivSelector1.ByRule = "STAFF_SHTAT_PREVIEW";
            this.subdivSelector1.Location = new System.Drawing.Point(41, 98);
            this.subdivSelector1.Name = "subdivSelector1";
            this.subdivSelector1.Size = new System.Drawing.Size(562, 19);
            this.subdivSelector1.TabIndex = 7;
            // 
            // Degree
            // 
            this.Degree.BackColor = System.Drawing.SystemColors.Window;
            this.Degree.DrawMode = System.Windows.Forms.DrawMode.Normal;
            this.Degree.DroppedDown = false;
            this.Degree.FormatInfo = null;
            this.Degree.FormatString = "";
            this.Degree.FormattingEnabled = false;
            this.Degree.Id = "ab14826c-cc9a-40cc-900f-e3e480f4579d";
            this.Degree.LabelText = "";
            this.Degree.Location = new System.Drawing.Point(139, 120);
            this.Degree.Name = "Degree";
            this.Degree.Size = new System.Drawing.Size(464, 21);
            this.Degree.Sorted = false;
            this.Degree.TabIndex = 6;
            this.Degree.TextEditorWidth = 445;
            this.Degree.UseVisualThemeForBackground = false;
            this.Degree.UseVisualThemeForForeground = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btOk);
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 144);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 35);
            this.panel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(55, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Категория:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(44, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Изменение с";
            // 
            // SelectInterval
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(623, 182);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SelectInterval";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменение штатного расписания (ф.о. 003-098)";
            this.Load += new System.EventHandler(this.SelectInterval_Load);
            this.Shown += new System.EventHandler(this.SelectInterval_Activated);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox mtBegDate;
        private System.Windows.Forms.MaskedTextBox mtEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private Elegant.Ui.ComboBox Degree;
        private System.Windows.Forms.Label label7;
        private Kadr.Classes.SubdivSelector subdivSelector1;
    }
}