namespace Kadr
{
    partial class Family
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deBirth_Date = new LibraryKadr.DateEditor();
            this.mbBirth_Year = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRel_ID = new System.Windows.Forms.ComboBox();
            this.label82 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.tbRel_Last_Name = new System.Windows.Forms.TextBox();
            this.tbRel_First_Name = new System.Windows.Forms.TextBox();
            this.tbRel_Middle_Name = new System.Windows.Forms.TextBox();
            
            this.pnButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 155);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(397, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(295, 9);
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
            this.btSave.Location = new System.Drawing.Point(201, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deBirth_Date);
            this.groupBox1.Controls.Add(this.mbBirth_Year);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbRel_ID);
            this.groupBox1.Controls.Add(this.label82);
            this.groupBox1.Controls.Add(this.label83);
            this.groupBox1.Controls.Add(this.label84);
            this.groupBox1.Controls.Add(this.label85);
            this.groupBox1.Controls.Add(this.label86);
            this.groupBox1.Controls.Add(this.tbRel_Last_Name);
            this.groupBox1.Controls.Add(this.tbRel_First_Name);
            this.groupBox1.Controls.Add(this.tbRel_Middle_Name);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // deBirth_Date
            // 
            this.deBirth_Date.AutoSize = true;
            this.deBirth_Date.Date = null;
            this.deBirth_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deBirth_Date.Location = new System.Drawing.Point(169, 97);
            this.deBirth_Date.Margin = new System.Windows.Forms.Padding(5);
            this.deBirth_Date.Name = "deBirth_Date";
            this.deBirth_Date.Size = new System.Drawing.Size(80, 24);
            this.deBirth_Date.TabIndex = 3;
            this.deBirth_Date.TextDate = null;
            // 
            // mbBirth_Year
            // 
            this.mbBirth_Year.BackColor = System.Drawing.Color.White;
            this.mbBirth_Year.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbBirth_Year.Location = new System.Drawing.Point(322, 97);
            this.mbBirth_Year.Mask = "0000";
            this.mbBirth_Year.Name = "mbBirth_Year";
            this.mbBirth_Year.Size = new System.Drawing.Size(48, 21);
            this.mbBirth_Year.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(285, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 60;
            this.label2.Text = "Год";
            // 
            // cbRel_ID
            // 
            this.cbRel_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbRel_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbRel_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbRel_ID.FormattingEnabled = true;
            this.cbRel_ID.Location = new System.Drawing.Point(169, 123);
            this.cbRel_ID.Name = "cbRel_ID";
            this.cbRel_ID.Size = new System.Drawing.Size(201, 23);
            this.cbRel_ID.TabIndex = 5;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label82.Location = new System.Drawing.Point(24, 72);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(71, 15);
            this.label82.TabIndex = 56;
            this.label82.Text = "Отчество";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label83.Location = new System.Drawing.Point(24, 100);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(111, 15);
            this.label83.TabIndex = 53;
            this.label83.Text = "Дата рождения";
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label84.Location = new System.Drawing.Point(24, 126);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(123, 15);
            this.label84.TabIndex = 55;
            this.label84.Text = "Степень родства";
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label85.Location = new System.Drawing.Point(24, 18);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(69, 15);
            this.label85.TabIndex = 58;
            this.label85.Text = "Фамилия";
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label86.Location = new System.Drawing.Point(24, 45);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(35, 15);
            this.label86.TabIndex = 57;
            this.label86.Text = "Имя";
            // 
            // tbRel_Last_Name
            // 
            this.tbRel_Last_Name.BackColor = System.Drawing.Color.White;
            this.tbRel_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRel_Last_Name.Location = new System.Drawing.Point(169, 15);
            this.tbRel_Last_Name.Name = "tbRel_Last_Name";
            this.tbRel_Last_Name.Size = new System.Drawing.Size(201, 21);
            this.tbRel_Last_Name.TabIndex = 0;
            this.tbRel_Last_Name.Validating += new System.ComponentModel.CancelEventHandler(this.tbRel_Last_Name_Validating);
            // 
            // tbRel_First_Name
            // 
            this.tbRel_First_Name.BackColor = System.Drawing.Color.White;
            this.tbRel_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRel_First_Name.Location = new System.Drawing.Point(169, 42);
            this.tbRel_First_Name.Name = "tbRel_First_Name";
            this.tbRel_First_Name.Size = new System.Drawing.Size(201, 21);
            this.tbRel_First_Name.TabIndex = 1;
            this.tbRel_First_Name.Validating += new System.ComponentModel.CancelEventHandler(this.tbRel_First_Name_Validating);
            // 
            // tbRel_Middle_Name
            // 
            this.tbRel_Middle_Name.BackColor = System.Drawing.Color.White;
            this.tbRel_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRel_Middle_Name.Location = new System.Drawing.Point(169, 69);
            this.tbRel_Middle_Name.Name = "tbRel_Middle_Name";
            this.tbRel_Middle_Name.Size = new System.Drawing.Size(201, 21);
            this.tbRel_Middle_Name.TabIndex = 2;
            this.tbRel_Middle_Name.Validating += new System.ComponentModel.CancelEventHandler(this.tbRel_Middle_Name_Validating);
            // 
            // formFrameSkinner1
            // 
            
            // 
            // Family
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(397, 195);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Family";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relative";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Family_FormClosing);
            this.pnButton.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbRel_ID;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.TextBox tbRel_Last_Name;
        private System.Windows.Forms.TextBox tbRel_First_Name;
        private System.Windows.Forms.TextBox tbRel_Middle_Name;
        
        private System.Windows.Forms.MaskedTextBox mbBirth_Year;
        private System.Windows.Forms.Label label2;
        private LibraryKadr.DateEditor deBirth_Date;
        public System.Windows.Forms.Button btSave;
    }
}