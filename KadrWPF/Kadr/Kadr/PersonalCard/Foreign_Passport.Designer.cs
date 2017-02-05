namespace Kadr
{
    partial class Foreign_Passport
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
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbSeria_FP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbWho_Given_FP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.deWhen_Given_FP = new EditorsLibrary.DateEditor();
            this.dePeriod_FP = new EditorsLibrary.DateEditor();
            this.deLease_FP = new EditorsLibrary.DateEditor();
            this.tbNum_FP = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.pnButton.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label7.ForeColor = System.Drawing.Color.Black;
            label7.Location = new System.Drawing.Point(345, 30);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(51, 15);
            label7.TabIndex = 88;
            label7.Text = "Номер";
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 154);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(518, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(420, 9);
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
            this.btSave.Location = new System.Drawing.Point(327, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(87, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.deLease_FP);
            this.groupBox3.Controls.Add(this.dePeriod_FP);
            this.groupBox3.Controls.Add(label4);
            this.groupBox3.Controls.Add(label1);
            this.groupBox3.Controls.Add(this.deWhen_Given_FP);
            this.groupBox3.Controls.Add(label3);
            this.groupBox3.Controls.Add(label7);
            this.groupBox3.Controls.Add(this.tbNum_FP);
            this.groupBox3.Controls.Add(this.tbSeria_FP);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tbWho_Given_FP);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(518, 154);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // tbSeria_FP
            // 
            this.tbSeria_FP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSeria_FP.Location = new System.Drawing.Point(98, 27);
            this.tbSeria_FP.Name = "tbSeria_FP";
            this.tbSeria_FP.Size = new System.Drawing.Size(92, 21);
            this.tbSeria_FP.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(18, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 15);
            this.label6.TabIndex = 86;
            this.label6.Text = "Серия";
            // 
            // tbWho_Given_FP
            // 
            this.tbWho_Given_FP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbWho_Given_FP.Location = new System.Drawing.Point(98, 54);
            this.tbWho_Given_FP.Multiline = true;
            this.tbWho_Given_FP.Name = "tbWho_Given_FP";
            this.tbWho_Given_FP.Size = new System.Drawing.Size(396, 54);
            this.tbWho_Given_FP.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(17, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 15);
            this.label2.TabIndex = 84;
            this.label2.Text = "Кем выдан";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // deWhen_Given_FP
            // 
            this.deWhen_Given_FP.AutoSize = true;
            this.deWhen_Given_FP.Date = null;
            this.deWhen_Given_FP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deWhen_Given_FP.Location = new System.Drawing.Point(98, 120);
            this.deWhen_Given_FP.Margin = new System.Windows.Forms.Padding(5);
            this.deWhen_Given_FP.Name = "deWhen_Given_FP";
            this.deWhen_Given_FP.Size = new System.Drawing.Size(80, 24);
            this.deWhen_Given_FP.TabIndex = 3;
            this.deWhen_Given_FP.TextDate = null;
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label3.ForeColor = System.Drawing.Color.Black;
            label3.Location = new System.Drawing.Point(17, 115);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(60, 36);
            label3.TabIndex = 88;
            label3.Text = "Дата выдачи";
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label1.ForeColor = System.Drawing.Color.Black;
            label1.Location = new System.Drawing.Point(187, 115);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 34);
            label1.TabIndex = 88;
            label1.Text = "Срок действия";
            // 
            // dePeriod_FP
            // 
            this.dePeriod_FP.AutoSize = true;
            this.dePeriod_FP.Date = null;
            this.dePeriod_FP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dePeriod_FP.Location = new System.Drawing.Point(262, 120);
            this.dePeriod_FP.Margin = new System.Windows.Forms.Padding(5);
            this.dePeriod_FP.Name = "dePeriod_FP";
            this.dePeriod_FP.Size = new System.Drawing.Size(80, 24);
            this.dePeriod_FP.TabIndex = 4;
            this.dePeriod_FP.TextDate = null;
            // 
            // deLease_FP
            // 
            this.deLease_FP.AutoSize = true;
            this.deLease_FP.Date = null;
            this.deLease_FP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deLease_FP.Location = new System.Drawing.Point(414, 120);
            this.deLease_FP.Margin = new System.Windows.Forms.Padding(5);
            this.deLease_FP.Name = "deLease_FP";
            this.deLease_FP.Size = new System.Drawing.Size(80, 24);
            this.deLease_FP.TabIndex = 5;
            this.deLease_FP.TextDate = null;
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label4.ForeColor = System.Drawing.Color.Black;
            label4.Location = new System.Drawing.Point(363, 115);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(51, 35);
            label4.TabIndex = 88;
            label4.Text = "Дата сдачи";
            // 
            // tbNum_FP
            // 
            this.tbNum_FP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_FP.Location = new System.Drawing.Point(402, 27);
            this.tbNum_FP.Name = "tbNum_FP";
            this.tbNum_FP.Size = new System.Drawing.Size(92, 21);
            this.tbNum_FP.TabIndex = 1;
            // 
            // Foreign_Passport
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(518, 194);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Foreign_Passport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Attest_FormClosing);
            this.pnButton.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbWho_Given_FP;
        private System.Windows.Forms.Label label2;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.TextBox tbSeria_FP;
        private System.Windows.Forms.Label label6;
        private EditorsLibrary.DateEditor deWhen_Given_FP;
        private EditorsLibrary.DateEditor deLease_FP;
        private EditorsLibrary.DateEditor dePeriod_FP;
        private System.Windows.Forms.TextBox tbNum_FP;
    }
}