namespace Kadr
{
    partial class Find_Emp
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
            this.gbFind = new System.Windows.Forms.GroupBox();
            this.mbInn = new System.Windows.Forms.MaskedTextBox();
            this.mbInsurance_Num = new System.Windows.Forms.MaskedTextBox();
            this.mbNum_Passport = new System.Windows.Forms.MaskedTextBox();
            this.mbSeria_Passport = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbNum_Med_Polus = new System.Windows.Forms.TextBox();
            this.tbSer_Med_Polus = new System.Windows.Forms.TextBox();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.cbType_Per_Doc_ID = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbEmp_Middle_Name = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbEmp_First_Name = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbEmp_Last_Name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btFind = new System.Windows.Forms.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.gbFind.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFind
            // 
            this.gbFind.Controls.Add(this.mbInn);
            this.gbFind.Controls.Add(this.mbInsurance_Num);
            this.gbFind.Controls.Add(this.mbNum_Passport);
            this.gbFind.Controls.Add(this.mbSeria_Passport);
            this.gbFind.Controls.Add(this.label3);
            this.gbFind.Controls.Add(this.label6);
            this.gbFind.Controls.Add(this.label27);
            this.gbFind.Controls.Add(this.label5);
            this.gbFind.Controls.Add(this.label21);
            this.gbFind.Controls.Add(this.label13);
            this.gbFind.Controls.Add(this.tbNum_Med_Polus);
            this.gbFind.Controls.Add(this.tbSer_Med_Polus);
            this.gbFind.Controls.Add(this.tbPer_Num);
            this.gbFind.Controls.Add(this.cbSubdiv_Name);
            this.gbFind.Controls.Add(this.cbType_Per_Doc_ID);
            this.gbFind.Controls.Add(this.label28);
            this.gbFind.Controls.Add(this.label4);
            this.gbFind.Controls.Add(this.label7);
            this.gbFind.Controls.Add(this.tbCode_Subdiv);
            this.gbFind.Controls.Add(this.label1);
            this.gbFind.Controls.Add(this.tbEmp_Middle_Name);
            this.gbFind.Controls.Add(this.label9);
            this.gbFind.Controls.Add(this.tbEmp_First_Name);
            this.gbFind.Controls.Add(this.label8);
            this.gbFind.Controls.Add(this.tbEmp_Last_Name);
            this.gbFind.Controls.Add(this.label2);
            this.gbFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.gbFind.Location = new System.Drawing.Point(0, 0);
            this.gbFind.Name = "gbFind";
            this.gbFind.Size = new System.Drawing.Size(614, 242);
            this.gbFind.TabIndex = 0;
            this.gbFind.TabStop = false;
            // 
            // mbInn
            // 
            this.mbInn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbInn.Location = new System.Drawing.Point(476, 84);
            this.mbInn.Mask = "999999999999";
            this.mbInn.Name = "mbInn";
            this.mbInn.Size = new System.Drawing.Size(113, 21);
            this.mbInn.TabIndex = 6;
            this.mbInn.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // mbInsurance_Num
            // 
            this.mbInsurance_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbInsurance_Num.Location = new System.Drawing.Point(476, 111);
            this.mbInsurance_Num.Mask = "999-999-999 99";
            this.mbInsurance_Num.Name = "mbInsurance_Num";
            this.mbInsurance_Num.Size = new System.Drawing.Size(113, 21);
            this.mbInsurance_Num.TabIndex = 7;
            this.mbInsurance_Num.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // mbNum_Passport
            // 
            this.mbNum_Passport.Location = new System.Drawing.Point(322, 211);
            this.mbNum_Passport.Name = "mbNum_Passport";
            this.mbNum_Passport.Size = new System.Drawing.Size(82, 21);
            this.mbNum_Passport.TabIndex = 12;
            this.mbNum_Passport.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // mbSeria_Passport
            // 
            this.mbSeria_Passport.Location = new System.Drawing.Point(192, 211);
            this.mbSeria_Passport.Name = "mbSeria_Passport";
            this.mbSeria_Passport.Size = new System.Drawing.Size(63, 21);
            this.mbSeria_Passport.TabIndex = 11;
            this.mbSeria_Passport.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(350, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 37);
            this.label3.TabIndex = 28;
            this.label3.Text = "Страховое свидетельство";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(267, 214);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 15);
            this.label6.TabIndex = 28;
            this.label6.Text = "номер";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Location = new System.Drawing.Point(267, 158);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(49, 15);
            this.label27.TabIndex = 28;
            this.label27.Text = "номер";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(139, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 15);
            this.label5.TabIndex = 28;
            this.label5.Text = "серия";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(139, 158);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(46, 15);
            this.label21.TabIndex = 28;
            this.label21.Text = "серия";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(350, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 15);
            this.label13.TabIndex = 28;
            this.label13.Text = "ИНН";
            // 
            // tbNum_Med_Polus
            // 
            this.tbNum_Med_Polus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Med_Polus.Location = new System.Drawing.Point(322, 155);
            this.tbNum_Med_Polus.Name = "tbNum_Med_Polus";
            this.tbNum_Med_Polus.Size = new System.Drawing.Size(82, 21);
            this.tbNum_Med_Polus.TabIndex = 9;
            // 
            // tbSer_Med_Polus
            // 
            this.tbSer_Med_Polus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSer_Med_Polus.Location = new System.Drawing.Point(192, 155);
            this.tbSer_Med_Polus.Name = "tbSer_Med_Polus";
            this.tbSer_Med_Polus.Size = new System.Drawing.Size(63, 21);
            this.tbSer_Med_Polus.TabIndex = 8;
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_Num.Location = new System.Drawing.Point(142, 20);
            this.tbPer_Num.MaxLength = 5;
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(53, 21);
            this.tbPer_Num.TabIndex = 0;
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(201, 47);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(388, 23);
            this.cbSubdiv_Name.TabIndex = 2;
            // 
            // cbType_Per_Doc_ID
            // 
            this.cbType_Per_Doc_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbType_Per_Doc_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbType_Per_Doc_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbType_Per_Doc_ID.FormattingEnabled = true;
            this.cbType_Per_Doc_ID.Location = new System.Drawing.Point(142, 182);
            this.cbType_Per_Doc_ID.Name = "cbType_Per_Doc_ID";
            this.cbType_Per_Doc_ID.Size = new System.Drawing.Size(447, 23);
            this.cbType_Per_Doc_ID.TabIndex = 10;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label28.Location = new System.Drawing.Point(16, 185);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(72, 15);
            this.label28.TabIndex = 17;
            this.label28.Text = "Документ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(16, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 15);
            this.label4.TabIndex = 21;
            this.label4.Text = "Табельный номер";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(16, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Медиц. полис";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(142, 47);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(53, 21);
            this.tbCode_Subdiv.TabIndex = 1;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(16, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Подразделение";
            // 
            // tbEmp_Middle_Name
            // 
            this.tbEmp_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Middle_Name.Location = new System.Drawing.Point(142, 128);
            this.tbEmp_Middle_Name.Name = "tbEmp_Middle_Name";
            this.tbEmp_Middle_Name.Size = new System.Drawing.Size(182, 21);
            this.tbEmp_Middle_Name.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(16, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "Отчество";
            // 
            // tbEmp_First_Name
            // 
            this.tbEmp_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_First_Name.Location = new System.Drawing.Point(142, 101);
            this.tbEmp_First_Name.Name = "tbEmp_First_Name";
            this.tbEmp_First_Name.Size = new System.Drawing.Size(182, 21);
            this.tbEmp_First_Name.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(16, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "Имя";
            // 
            // tbEmp_Last_Name
            // 
            this.tbEmp_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Last_Name.Location = new System.Drawing.Point(142, 74);
            this.tbEmp_Last_Name.Name = "tbEmp_Last_Name";
            this.tbEmp_Last_Name.Size = new System.Drawing.Size(182, 21);
            this.tbEmp_Last_Name.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(16, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Фамилия";
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btFind);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 242);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(614, 38);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(504, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // btFind
            // 
            this.btFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btFind.Location = new System.Drawing.Point(411, 6);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(87, 23);
            this.btFind.TabIndex = 0;
            this.btFind.Text = "Поиск";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // Find_Emp
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(614, 280);
            this.Controls.Add(this.gbFind);
            this.Controls.Add(this.pnButton);
            this.MaximizeBox = false;
            this.Name = "Find_Emp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск данных";
            this.gbFind.ResumeLayout(false);
            this.gbFind.PerformLayout();
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFind;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.ComboBox cbType_Per_Doc_ID;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbEmp_Last_Name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNum_Med_Polus;
        private System.Windows.Forms.TextBox tbSer_Med_Polus;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.MaskedTextBox mbNum_Passport;
        private System.Windows.Forms.MaskedTextBox mbSeria_Passport;
        private System.Windows.Forms.MaskedTextBox mbInn;
        private System.Windows.Forms.MaskedTextBox mbInsurance_Num;
        private System.Windows.Forms.TextBox tbEmp_Middle_Name;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbEmp_First_Name;
        private System.Windows.Forms.Label label8;
    }
}