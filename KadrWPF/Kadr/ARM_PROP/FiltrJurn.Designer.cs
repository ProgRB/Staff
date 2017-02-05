namespace ARM_PROP
{
    partial class FiltrJurn
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
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.bt_Ok = new Elegant.Ui.Button();
            this.bt_Cancel = new Elegant.Ui.Button();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new Elegant.Ui.Label();
            this.label2 = new Elegant.Ui.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.Location = new System.Drawing.Point(103, 20);
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(88, 20);
            this.tbPer_Num.TabIndex = 5;
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
            this.cbSubdiv_Name.Location = new System.Drawing.Point(197, 51);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(252, 21);
            this.cbSubdiv_Name.TabIndex = 4;
            this.cbSubdiv_Name.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // bt_Ok
            // 
            this.bt_Ok.Id = "c3c7c666-c095-4a85-ad80-e11290096a35";
            this.bt_Ok.Location = new System.Drawing.Point(211, 80);
            this.bt_Ok.Name = "bt_Ok";
            this.bt_Ok.Size = new System.Drawing.Size(118, 23);
            this.bt_Ok.TabIndex = 9;
            this.bt_Ok.Text = "Применить фильтр";
            this.bt_Ok.Click += new System.EventHandler(this.bt_Ok_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Id = "b06b79e2-5498-4ca7-b984-14c920fbb117";
            this.bt_Cancel.Location = new System.Drawing.Point(346, 80);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cancel.TabIndex = 10;
            this.bt_Cancel.Text = "Отмена";
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Cancel_Click);
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Location = new System.Drawing.Point(103, 52);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(88, 20);
            this.tbCode_Subdiv.TabIndex = 11;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // label1
            // 
            this.label1.Id = "c8fb4977-601e-4f33-b297-9f57a2ef445a";
            this.label1.Location = new System.Drawing.Point(7, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Подразделение";
            // 
            // label2
            // 
            this.label2.Id = "87c971c5-b711-4e08-bb3b-cd931827e83e";
            this.label2.Location = new System.Drawing.Point(7, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Табельный номер";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Controls.Add(this.tbCode_Subdiv);
            this.gbFilter.Controls.Add(this.bt_Cancel);
            this.gbFilter.Controls.Add(this.bt_Ok);
            this.gbFilter.Controls.Add(this.cbSubdiv_Name);
            this.gbFilter.Controls.Add(this.tbPer_Num);
            this.gbFilter.Location = new System.Drawing.Point(4, 3);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(453, 114);
            this.gbFilter.TabIndex = 9;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Отфильтровать";
            // 
            // FiltrJurn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 124);
            this.Controls.Add(this.gbFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FiltrJurn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Фильтр подразделений";
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox gbFilter;
        private Elegant.Ui.Label label2;
        private Elegant.Ui.Label label1;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private Elegant.Ui.Button bt_Cancel;
        private Elegant.Ui.Button bt_Ok;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.TextBox tbPer_Num;
    }
}