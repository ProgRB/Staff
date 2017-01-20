namespace Kadr
{
    partial class Prof_Train
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
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.cbBase_Doc_ID = new System.Windows.Forms.ComboBox();
            this.label62 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mbDate_Doc = new System.Windows.Forms.MaskedTextBox();
            this.mbDate_End = new System.Windows.Forms.MaskedTextBox();
            this.mbDate_Start = new System.Windows.Forms.MaskedTextBox();
            this.cbProf_ID = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName_Doc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNum_Doc = new System.Windows.Forms.TextBox();
            
            this.deDate_Doc = new LibraryKadr.DateEditor();
            this.deDate_End = new LibraryKadr.DateEditor();
            this.deDate_Start = new LibraryKadr.DateEditor();
            this.pnButton.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 206);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(601, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(499, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSave.Location = new System.Drawing.Point(405, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // cbBase_Doc_ID
            // 
            this.cbBase_Doc_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbBase_Doc_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbBase_Doc_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBase_Doc_ID.FormattingEnabled = true;
            this.cbBase_Doc_ID.Location = new System.Drawing.Point(152, 74);
            this.cbBase_Doc_ID.Name = "cbBase_Doc_ID";
            this.cbBase_Doc_ID.Size = new System.Drawing.Size(423, 23);
            this.cbBase_Doc_ID.TabIndex = 3;
            // 
            // label62
            // 
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label62.Location = new System.Drawing.Point(25, 77);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(93, 18);
            this.label62.TabIndex = 83;
            this.label62.Text = "Основание";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.deDate_Start);
            this.groupBox4.Controls.Add(this.deDate_End);
            this.groupBox4.Controls.Add(this.deDate_Doc);
            this.groupBox4.Controls.Add(this.mbDate_Doc);
            this.groupBox4.Controls.Add(this.mbDate_End);
            this.groupBox4.Controls.Add(this.mbDate_Start);
            this.groupBox4.Controls.Add(this.cbProf_ID);
            this.groupBox4.Controls.Add(this.cbBase_Doc_ID);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label62);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.tbName_Doc);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.tbNum_Doc);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(601, 206);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // mbDate_Doc
            // 
            this.mbDate_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDate_Doc.Location = new System.Drawing.Point(117, 143);
            this.mbDate_Doc.Mask = "00/00/0000";
            this.mbDate_Doc.Name = "mbDate_Doc";
            this.mbDate_Doc.Size = new System.Drawing.Size(83, 21);
            this.mbDate_Doc.TabIndex = 5;
            this.mbDate_Doc.ValidatingType = typeof(System.DateTime);
            this.mbDate_Doc.Visible = false;
            // 
            // mbDate_End
            // 
            this.mbDate_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDate_End.Location = new System.Drawing.Point(443, 44);
            this.mbDate_End.Mask = "00/00/0000";
            this.mbDate_End.Name = "mbDate_End";
            this.mbDate_End.Size = new System.Drawing.Size(82, 21);
            this.mbDate_End.TabIndex = 2;
            this.mbDate_End.ValidatingType = typeof(System.DateTime);
            this.mbDate_End.Visible = false;
            // 
            // mbDate_Start
            // 
            this.mbDate_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDate_Start.Location = new System.Drawing.Point(443, 15);
            this.mbDate_Start.Mask = "00/00/0000";
            this.mbDate_Start.Name = "mbDate_Start";
            this.mbDate_Start.Size = new System.Drawing.Size(82, 21);
            this.mbDate_Start.TabIndex = 1;
            this.mbDate_Start.ValidatingType = typeof(System.DateTime);
            this.mbDate_Start.Visible = false;
            // 
            // cbProf_ID
            // 
            this.cbProf_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbProf_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbProf_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbProf_ID.FormattingEnabled = true;
            this.cbProf_ID.Location = new System.Drawing.Point(152, 108);
            this.cbProf_ID.Name = "cbProf_ID";
            this.cbProf_ID.Size = new System.Drawing.Size(423, 23);
            this.cbProf_ID.TabIndex = 4;
            this.cbProf_ID.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(25, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 33);
            this.label6.TabIndex = 83;
            this.label6.Text = "Профессия по диплому";
            this.label6.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(25, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 32);
            this.label2.TabIndex = 83;
            this.label2.Text = "Наименование документа";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(25, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 82;
            this.label1.Text = "Дата документа";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(25, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 23);
            this.label5.TabIndex = 82;
            this.label5.Text = "Номер документа";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(259, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 15);
            this.label3.TabIndex = 86;
            this.label3.Text = "Дата окончания переподготовки";
            // 
            // tbName_Doc
            // 
            this.tbName_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbName_Doc.Location = new System.Drawing.Point(152, 173);
            this.tbName_Doc.Name = "tbName_Doc";
            this.tbName_Doc.Size = new System.Drawing.Size(423, 21);
            this.tbName_Doc.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(259, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 15);
            this.label4.TabIndex = 86;
            this.label4.Text = "Дата начала переподготовки";
            // 
            // tbNum_Doc
            // 
            this.tbNum_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Doc.Location = new System.Drawing.Point(152, 17);
            this.tbNum_Doc.Name = "tbNum_Doc";
            this.tbNum_Doc.Size = new System.Drawing.Size(83, 21);
            this.tbNum_Doc.TabIndex = 0;
            // 
            // formFrameSkinner1
            // 
            
            // 
            // deDate_Doc
            // 
            this.deDate_Doc.Date = null;
            this.deDate_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Doc.Location = new System.Drawing.Point(152, 143);
            this.deDate_Doc.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Doc.Name = "deDate_Doc";
            this.deDate_Doc.Size = new System.Drawing.Size(83, 24);
            this.deDate_Doc.TabIndex = 5;
            // 
            // deDate_End
            // 
            this.deDate_End.Date = null;
            this.deDate_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_End.Location = new System.Drawing.Point(493, 44);
            this.deDate_End.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_End.Name = "deDate_End";
            this.deDate_End.Size = new System.Drawing.Size(82, 24);
            this.deDate_End.TabIndex = 2;
            // 
            // deDate_Start
            // 
            this.deDate_Start.Date = null;
            this.deDate_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Start.Location = new System.Drawing.Point(493, 15);
            this.deDate_Start.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Start.Name = "deDate_Start";
            this.deDate_Start.Size = new System.Drawing.Size(82, 24);
            this.deDate_Start.TabIndex = 1;
            // 
            // Prof_Train
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(601, 246);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Prof_Train";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prof_Train";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Prof_Train_FormClosing);
            this.pnButton.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.ComboBox cbBase_Doc_ID;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNum_Doc;
        private System.Windows.Forms.TextBox tbName_Doc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProf_ID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mbDate_Doc;
        private System.Windows.Forms.MaskedTextBox mbDate_End;
        private System.Windows.Forms.MaskedTextBox mbDate_Start;
        
        private LibraryKadr.DateEditor deDate_Start;
        private LibraryKadr.DateEditor deDate_End;
        private LibraryKadr.DateEditor deDate_Doc;
    }
}