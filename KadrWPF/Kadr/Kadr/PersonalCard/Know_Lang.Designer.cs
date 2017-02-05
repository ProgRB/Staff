namespace Kadr
{
    partial class Know_Lang
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
            this.cbLevel_ID = new System.Windows.Forms.ComboBox();
            this.cbLang_ID = new System.Windows.Forms.ComboBox();
            this.label60 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLEVEL_IN_EF = new System.Windows.Forms.TextBox();
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
            this.pnButton.Location = new System.Drawing.Point(0, 120);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(494, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(394, 9);
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
            this.btSave.Location = new System.Drawing.Point(300, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLEVEL_IN_EF);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbLevel_ID);
            this.groupBox1.Controls.Add(this.cbLang_ID);
            this.groupBox1.Controls.Add(this.label60);
            this.groupBox1.Controls.Add(this.label53);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cbLevel_ID
            // 
            this.cbLevel_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbLevel_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbLevel_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbLevel_ID.FormattingEnabled = true;
            this.cbLevel_ID.Location = new System.Drawing.Point(204, 47);
            this.cbLevel_ID.Name = "cbLevel_ID";
            this.cbLevel_ID.Size = new System.Drawing.Size(265, 23);
            this.cbLevel_ID.TabIndex = 1;
            // 
            // cbLang_ID
            // 
            this.cbLang_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbLang_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbLang_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbLang_ID.FormattingEnabled = true;
            this.cbLang_ID.Location = new System.Drawing.Point(204, 15);
            this.cbLang_ID.Name = "cbLang_ID";
            this.cbLang_ID.Size = new System.Drawing.Size(265, 23);
            this.cbLang_ID.TabIndex = 0;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label60.Location = new System.Drawing.Point(13, 18);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(151, 15);
            this.label60.TabIndex = 43;
            this.label60.Text = "Наименование языка";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label53.Location = new System.Drawing.Point(13, 50);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(186, 15);
            this.label53.TabIndex = 42;
            this.label53.Text = "Степень владения языком";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 15);
            this.label1.TabIndex = 44;
            this.label1.Text = "Уровень владения языком по шкале EF ";
            // 
            // tbLEVEL_IN_EF
            // 
            this.tbLEVEL_IN_EF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLEVEL_IN_EF.Location = new System.Drawing.Point(377, 79);
            this.tbLEVEL_IN_EF.Name = "tbLEVEL_IN_EF";
            this.tbLEVEL_IN_EF.Size = new System.Drawing.Size(92, 21);
            this.tbLEVEL_IN_EF.TabIndex = 2;
            // 
            // Know_Lang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 160);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Know_Lang";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Know_Lang";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Know_Lang_FormClosing);
            this.pnButton.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbLevel_ID;
        private System.Windows.Forms.ComboBox cbLang_ID;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label53;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLEVEL_IN_EF;
    }
}