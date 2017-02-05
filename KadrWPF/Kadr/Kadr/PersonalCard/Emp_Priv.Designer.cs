namespace Kadr
{
    partial class Emp_Priv
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.deDate_Give = new EditorsLibrary.DateEditor();
            this.deDate_Start_Priv = new EditorsLibrary.DateEditor();
            this.deDate_End_Priv = new EditorsLibrary.DateEditor();
            this.cbBase_Doc_ID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbType_Priv_ID = new System.Windows.Forms.ComboBox();
            this.label59 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.tbNum_Priv = new System.Windows.Forms.TextBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.chMSE = new System.Windows.Forms.CheckBox();
            this.chIRP = new System.Windows.Forms.CheckBox();
            this.pnButton.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 187);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(555, 1);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 188);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(555, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(453, 9);
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
            this.btSave.Location = new System.Drawing.Point(357, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(90, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chIRP);
            this.groupBox2.Controls.Add(this.chMSE);
            this.groupBox2.Controls.Add(this.deDate_Give);
            this.groupBox2.Controls.Add(this.deDate_Start_Priv);
            this.groupBox2.Controls.Add(this.deDate_End_Priv);
            this.groupBox2.Controls.Add(this.cbBase_Doc_ID);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbType_Priv_ID);
            this.groupBox2.Controls.Add(this.label59);
            this.groupBox2.Controls.Add(this.label62);
            this.groupBox2.Controls.Add(this.label56);
            this.groupBox2.Controls.Add(this.label60);
            this.groupBox2.Controls.Add(this.label53);
            this.groupBox2.Controls.Add(this.tbNum_Priv);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(555, 187);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // deDate_Give
            // 
            this.deDate_Give.AutoSize = true;
            this.deDate_Give.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Give.Date = null;
            this.deDate_Give.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Give.Location = new System.Drawing.Point(455, 18);
            this.deDate_Give.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Give.Name = "deDate_Give";
            this.deDate_Give.ReadOnly = false;
            this.deDate_Give.Size = new System.Drawing.Size(76, 24);
            this.deDate_Give.TabIndex = 1;
            this.deDate_Give.TextDate = null;
            // 
            // deDate_Start_Priv
            // 
            this.deDate_Start_Priv.AutoSize = true;
            this.deDate_Start_Priv.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Start_Priv.Date = null;
            this.deDate_Start_Priv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Start_Priv.Location = new System.Drawing.Point(168, 119);
            this.deDate_Start_Priv.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Start_Priv.Name = "deDate_Start_Priv";
            this.deDate_Start_Priv.ReadOnly = false;
            this.deDate_Start_Priv.Size = new System.Drawing.Size(76, 24);
            this.deDate_Start_Priv.TabIndex = 4;
            this.deDate_Start_Priv.TextDate = null;
            // 
            // deDate_End_Priv
            // 
            this.deDate_End_Priv.AutoSize = true;
            this.deDate_End_Priv.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_End_Priv.Date = null;
            this.deDate_End_Priv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_End_Priv.Location = new System.Drawing.Point(455, 119);
            this.deDate_End_Priv.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_End_Priv.Name = "deDate_End_Priv";
            this.deDate_End_Priv.ReadOnly = false;
            this.deDate_End_Priv.Size = new System.Drawing.Size(76, 24);
            this.deDate_End_Priv.TabIndex = 5;
            this.deDate_End_Priv.TextDate = null;
            // 
            // cbBase_Doc_ID
            // 
            this.cbBase_Doc_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbBase_Doc_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbBase_Doc_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBase_Doc_ID.FormattingEnabled = true;
            this.cbBase_Doc_ID.Location = new System.Drawing.Point(168, 85);
            this.cbBase_Doc_ID.Name = "cbBase_Doc_ID";
            this.cbBase_Doc_ID.Size = new System.Drawing.Size(363, 23);
            this.cbBase_Doc_ID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(286, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 15);
            this.label1.TabIndex = 84;
            this.label1.Text = "Дата окончания льготы";
            // 
            // cbType_Priv_ID
            // 
            this.cbType_Priv_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbType_Priv_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbType_Priv_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbType_Priv_ID.FormattingEnabled = true;
            this.cbType_Priv_ID.Location = new System.Drawing.Point(168, 51);
            this.cbType_Priv_ID.Name = "cbType_Priv_ID";
            this.cbType_Priv_ID.Size = new System.Drawing.Size(363, 23);
            this.cbType_Priv_ID.TabIndex = 2;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label59.Location = new System.Drawing.Point(361, 21);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(94, 15);
            this.label59.TabIndex = 83;
            this.label59.Text = "Дата выдачи";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label62.Location = new System.Drawing.Point(22, 88);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(80, 15);
            this.label62.TabIndex = 80;
            this.label62.Text = "Основание";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label56.Location = new System.Drawing.Point(22, 122);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(144, 15);
            this.label56.TabIndex = 79;
            this.label56.Text = "Дата начала льготы";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label60.Location = new System.Drawing.Point(22, 21);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(51, 15);
            this.label60.TabIndex = 82;
            this.label60.Text = "Номер";
            // 
            // label53
            // 
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label53.Location = new System.Drawing.Point(22, 48);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(125, 38);
            this.label53.TabIndex = 81;
            this.label53.Text = "Наименование льготы";
            // 
            // tbNum_Priv
            // 
            this.tbNum_Priv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Priv.Location = new System.Drawing.Point(168, 18);
            this.tbNum_Priv.Name = "tbNum_Priv";
            this.tbNum_Priv.Size = new System.Drawing.Size(76, 21);
            this.tbNum_Priv.TabIndex = 0;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // chMSE
            // 
            this.chMSE.AutoSize = true;
            this.chMSE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chMSE.Location = new System.Drawing.Point(20, 160);
            this.chMSE.Name = "chMSE";
            this.chMSE.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chMSE.Size = new System.Drawing.Size(224, 19);
            this.chMSE.TabIndex = 85;
            this.chMSE.Text = "Наличие справки МСЭ (ВТЭК)";
            this.chMSE.UseVisualStyleBackColor = true;
            // 
            // chIRP
            // 
            this.chIRP.AutoSize = true;
            this.chIRP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chIRP.Location = new System.Drawing.Point(415, 160);
            this.chIRP.Name = "chIRP";
            this.chIRP.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chIRP.Size = new System.Drawing.Size(116, 19);
            this.chIRP.TabIndex = 86;
            this.chIRP.Text = "Наличие ИПР";
            this.chIRP.UseVisualStyleBackColor = true;
            // 
            // Emp_Priv
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(555, 228);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Emp_Priv";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Emp_Priv";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Emp_Priv_FormClosing);
            this.pnButton.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbBase_Doc_ID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbType_Priv_ID;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox tbNum_Priv;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private EditorsLibrary.DateEditor deDate_Give;
        private EditorsLibrary.DateEditor deDate_Start_Priv;
        private EditorsLibrary.DateEditor deDate_End_Priv;
        private System.Windows.Forms.CheckBox chIRP;
        private System.Windows.Forms.CheckBox chMSE;
    }
}