namespace ARM_PROP
{
    partial class FindJurnal
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
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btFind = new System.Windows.Forms.Button();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbEmp_Middle_Name = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbEmp_First_Name = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbEmp_Last_Name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.gbFind.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFind
            // 
            this.gbFind.Controls.Add(this.tbPer_Num);
            this.gbFind.Controls.Add(this.pnButton);
            this.gbFind.Controls.Add(this.cbSubdiv_Name);
            this.gbFind.Controls.Add(this.label4);
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
            this.gbFind.Size = new System.Drawing.Size(606, 205);
            this.gbFind.TabIndex = 1;
            this.gbFind.TabStop = false;
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
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btFind);
            this.pnButton.Location = new System.Drawing.Point(0, 155);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(602, 48);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(492, 16);
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
            this.btFind.Location = new System.Drawing.Point(399, 16);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(87, 23);
            this.btFind.TabIndex = 0;
            this.btFind.Text = "Поиск";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
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
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // FindJurnal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 205);
            this.Controls.Add(this.gbFind);
            this.Name = "FindJurnal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск";
            this.gbFind.ResumeLayout(false);
            this.gbFind.PerformLayout();
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFind;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbEmp_Middle_Name;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbEmp_First_Name;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbEmp_Last_Name;
        private System.Windows.Forms.Label label2;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
    }
}