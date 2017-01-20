namespace Tabel
{
    partial class Find_EmpTable
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
            this.btFind = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.gbFind = new System.Windows.Forms.GroupBox();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbEmp_Middle_Name = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbEmp_First_Name = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbEmp_Last_Name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chSign_Comb = new System.Windows.Forms.CheckBox();
            this.pnButton.SuspendLayout();
            this.gbFind.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btFind);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 134);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(344, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btFind
            // 
            this.btFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFind.Location = new System.Drawing.Point(136, 8);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(92, 23);
            this.btFind.TabIndex = 0;
            this.btFind.Text = "Поиск";
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(238, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(85, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            // 
            // gbFind
            // 
            this.gbFind.Controls.Add(this.chSign_Comb);
            this.gbFind.Controls.Add(this.tbPer_Num);
            this.gbFind.Controls.Add(this.label4);
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
            this.gbFind.Size = new System.Drawing.Size(344, 134);
            this.gbFind.TabIndex = 0;
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
            // tbEmp_Middle_Name
            // 
            this.tbEmp_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Middle_Name.Location = new System.Drawing.Point(142, 101);
            this.tbEmp_Middle_Name.Name = "tbEmp_Middle_Name";
            this.tbEmp_Middle_Name.Size = new System.Drawing.Size(182, 21);
            this.tbEmp_Middle_Name.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(16, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "Отчество";
            // 
            // tbEmp_First_Name
            // 
            this.tbEmp_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_First_Name.Location = new System.Drawing.Point(142, 74);
            this.tbEmp_First_Name.Name = "tbEmp_First_Name";
            this.tbEmp_First_Name.Size = new System.Drawing.Size(182, 21);
            this.tbEmp_First_Name.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(16, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "Имя";
            // 
            // tbEmp_Last_Name
            // 
            this.tbEmp_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Last_Name.Location = new System.Drawing.Point(142, 47);
            this.tbEmp_Last_Name.Name = "tbEmp_Last_Name";
            this.tbEmp_Last_Name.Size = new System.Drawing.Size(182, 21);
            this.tbEmp_Last_Name.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(16, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Фамилия";
            // 
            // chSign_Comb
            // 
            this.chSign_Comb.AutoSize = true;
            this.chSign_Comb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Comb.ForeColor = System.Drawing.Color.Black;
            this.chSign_Comb.Location = new System.Drawing.Point(211, 22);
            this.chSign_Comb.Name = "chSign_Comb";
            this.chSign_Comb.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chSign_Comb.Size = new System.Drawing.Size(111, 19);
            this.chSign_Comb.TabIndex = 22;
            this.chSign_Comb.Text = "Совмещение";
            this.chSign_Comb.UseVisualStyleBackColor = true;
            // 
            // Find_EmpTable
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 174);
            this.Controls.Add(this.gbFind);
            this.Controls.Add(this.pnButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Find_EmpTable";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск данных";
            this.pnButton.ResumeLayout(false);
            this.gbFind.ResumeLayout(false);
            this.gbFind.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFind;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbEmp_Middle_Name;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbEmp_First_Name;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbEmp_Last_Name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.CheckBox chSign_Comb;
    }
}