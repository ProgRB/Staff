namespace ARM_PROP
{
    partial class PrintJurnal
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
            this.label7 = new System.Windows.Forms.Label();
            this.cbPriznak = new System.Windows.Forms.ComboBox();
            this.tbFirst = new System.Windows.Forms.TextBox();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.tbMiddle = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Print = new Elegant.Ui.Button();
            this.Close = new Elegant.Ui.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(0, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "Признак задержания";
            // 
            // cbPriznak
            // 
            this.cbPriznak.BackColor = System.Drawing.Color.White;
            this.cbPriznak.FormattingEnabled = true;
            this.cbPriznak.Location = new System.Drawing.Point(138, 103);
            this.cbPriznak.Name = "cbPriznak";
            this.cbPriznak.Size = new System.Drawing.Size(231, 21);
            this.cbPriznak.TabIndex = 6;
            // 
            // tbFirst
            // 
            this.tbFirst.BackColor = System.Drawing.Color.White;
            this.tbFirst.Location = new System.Drawing.Point(138, 71);
            this.tbFirst.Name = "tbFirst";
            this.tbFirst.ReadOnly = true;
            this.tbFirst.Size = new System.Drawing.Size(136, 20);
            this.tbFirst.TabIndex = 3;
            // 
            // tbLast
            // 
            this.tbLast.BackColor = System.Drawing.Color.White;
            this.tbLast.Location = new System.Drawing.Point(318, 72);
            this.tbLast.Name = "tbLast";
            this.tbLast.ReadOnly = true;
            this.tbLast.Size = new System.Drawing.Size(135, 20);
            this.tbLast.TabIndex = 4;
            // 
            // tbMiddle
            // 
            this.tbMiddle.BackColor = System.Drawing.Color.White;
            this.tbMiddle.Location = new System.Drawing.Point(526, 71);
            this.tbMiddle.Name = "tbMiddle";
            this.tbMiddle.ReadOnly = true;
            this.tbMiddle.Size = new System.Drawing.Size(142, 20);
            this.tbMiddle.TabIndex = 5;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(459, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 13);
            this.label16.TabIndex = 88;
            this.label16.Text = "Отчество";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(280, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 87;
            this.label15.Text = "Имя";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(71, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 86;
            this.label1.Text = "Фамилия";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox1.Location = new System.Drawing.Point(138, 132);
            this.maskedTextBox1.Mask = "00/00/0000";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(100, 21);
            this.maskedTextBox1.TabIndex = 7;
            this.maskedTextBox1.ValidatingType = typeof(System.DateTime);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(244, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 98;
            this.label2.Text = "по";
            // 
            // maskedTextBox2
            // 
            this.maskedTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox2.Location = new System.Drawing.Point(269, 132);
            this.maskedTextBox2.Mask = "00/00/0000";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(100, 21);
            this.maskedTextBox2.TabIndex = 8;
            this.maskedTextBox2.ValidatingType = typeof(System.DateTime);
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Location = new System.Drawing.Point(140, 40);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(88, 20);
            this.tbCode_Subdiv.TabIndex = 1;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Items.AddRange(new object[] {
            "1 отдел",
            "10 отдел",
            "106 отдел",
            "109 отдел",
            "11 отдел",
            "11 цех",
            "110 отдел",
            "111 отдел",
            "114 отдел",
            "116 цех",
            "119 отдел",
            "12 отдел",
            "12 цех",
            "13 отдел",
            "132 отдел",
            "139 отдел",
            "15 отдел",
            "16 отдел",
            "16 цех",
            "17 отдел",
            "17 цех",
            "18 цех",
            "180 отдел",
            "19 отдел",
            "20 отдел",
            "21 отдел",
            "22 отдел",
            "23 отдел",
            "24 цех",
            "27 отдел",
            "28 цех",
            "3 отдел",
            "33 отдел",
            "36 цех",
            "39 отдел",
            "40 цех",
            "41 отдел",
            "43 отдел",
            "45 отдел",
            "49 цех",
            "5 отдел",
            "50 цех",
            "51 цех",
            "53 цех",
            "54 цех",
            "55 цех",
            "6 отдел",
            "60 цех",
            "61 цех",
            "62 отдел",
            "7 отдел",
            "78 отдел",
            "9 отдел",
            "95 цех",
            "ЗАС-133",
            "ЗШЗ-123",
            "ИЗ-30",
            "МСЗ",
            "МСЗ-146",
            "Пуско-наладка системы. Новосибирск.",
            "Котокель\t\t"});
            this.cbSubdiv_Name.Location = new System.Drawing.Point(234, 39);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(434, 21);
            this.cbSubdiv_Name.TabIndex = 2;
            this.cbSubdiv_Name.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.Location = new System.Drawing.Point(140, 6);
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(88, 20);
            this.tbPer_Num.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(20, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 104;
            this.label4.Text = "Табельный номер";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(32, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 105;
            this.label5.Text = "Подразделение";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(0, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 106;
            this.label6.Text = "Период задержания с";
            // 
            // Print
            // 
            this.Print.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Print.Id = "67fb850e-5d8a-4797-abf8-e005b6b9c26f";
            this.Print.Location = new System.Drawing.Point(462, 137);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(75, 23);
            this.Print.TabIndex = 107;
            this.Print.Text = "Печать";
            // 
            // Close
            // 
            this.Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Close.Id = "161b98ff-c69b-47c8-8c6e-f7d88edac86e";
            this.Close.Location = new System.Drawing.Point(561, 137);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 108;
            this.Close.Text = "Отмена";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // PrintJurnal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 168);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Print);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbFirst);
            this.Controls.Add(this.tbLast);
            this.Controls.Add(this.tbCode_Subdiv);
            this.Controls.Add(this.tbMiddle);
            this.Controls.Add(this.cbSubdiv_Name);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tbPer_Num);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.maskedTextBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbPriznak);
            this.Name = "PrintJurnal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Печать журнала нарушений";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbPriznak;
        private System.Windows.Forms.TextBox tbFirst;
        private System.Windows.Forms.TextBox tbLast;
        private System.Windows.Forms.TextBox tbMiddle;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox2;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Elegant.Ui.Button Print;
        private Elegant.Ui.Button Close;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
    }
}