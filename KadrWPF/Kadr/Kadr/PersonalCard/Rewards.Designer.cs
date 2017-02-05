namespace Kadr
{
    partial class Rewards
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
            this.cbReward_Name = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRew_Doc_Name = new System.Windows.Forms.TextBox();
            this.deDate_Reward = new EditorsLibrary.DateEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.chGov_Sign = new System.Windows.Forms.CheckBox();
            this.label59 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.tbNum_Reward = new System.Windows.Forms.TextBox();
            this.tbType_Reward = new System.Windows.Forms.TextBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.pnButton.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(579, 1);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 186);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(579, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(477, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(76, 23);
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
            this.btSave.Location = new System.Drawing.Point(383, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbReward_Name);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbRew_Doc_Name);
            this.groupBox2.Controls.Add(this.deDate_Reward);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chGov_Sign);
            this.groupBox2.Controls.Add(this.label59);
            this.groupBox2.Controls.Add(this.label62);
            this.groupBox2.Controls.Add(this.label60);
            this.groupBox2.Controls.Add(this.label53);
            this.groupBox2.Controls.Add(this.tbNum_Reward);
            this.groupBox2.Controls.Add(this.tbType_Reward);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(579, 185);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // cbReward_Name
            // 
            this.cbReward_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbReward_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbReward_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbReward_Name.FormattingEnabled = true;
            this.cbReward_Name.Location = new System.Drawing.Point(150, 26);
            this.cbReward_Name.Name = "cbReward_Name";
            this.cbReward_Name.Size = new System.Drawing.Size(406, 23);
            this.cbReward_Name.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(24, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 33);
            this.label2.TabIndex = 75;
            this.label2.Text = "Наименование документа";
            // 
            // tbRew_Doc_Name
            // 
            this.tbRew_Doc_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRew_Doc_Name.Location = new System.Drawing.Point(150, 94);
            this.tbRew_Doc_Name.Name = "tbRew_Doc_Name";
            this.tbRew_Doc_Name.Size = new System.Drawing.Size(406, 21);
            this.tbRew_Doc_Name.TabIndex = 2;
            this.tbRew_Doc_Name.Validating += new System.ComponentModel.CancelEventHandler(this.tbRew_Doc_Name_Validating);
            // 
            // deDate_Reward
            // 
            this.deDate_Reward.AutoSize = true;
            this.deDate_Reward.Date = null;
            this.deDate_Reward.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Reward.Location = new System.Drawing.Point(480, 128);
            this.deDate_Reward.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Reward.Name = "deDate_Reward";
            this.deDate_Reward.Size = new System.Drawing.Size(76, 24);
            this.deDate_Reward.TabIndex = 4;
            this.deDate_Reward.TextDate = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(166, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 15);
            this.label1.TabIndex = 73;
            this.label1.Text = "Признак правительственной награды";
            // 
            // chGov_Sign
            // 
            this.chGov_Sign.AutoSize = true;
            this.chGov_Sign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chGov_Sign.Location = new System.Drawing.Point(150, 162);
            this.chGov_Sign.Name = "chGov_Sign";
            this.chGov_Sign.Size = new System.Drawing.Size(281, 19);
            this.chGov_Sign.TabIndex = 4;
            this.chGov_Sign.Text = "Признак правительственной награды";
            this.chGov_Sign.UseVisualStyleBackColor = true;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label59.Location = new System.Drawing.Point(356, 131);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(116, 15);
            this.label59.TabIndex = 71;
            this.label59.Text = "Дата документа";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label62.Location = new System.Drawing.Point(24, 64);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(91, 15);
            this.label62.TabIndex = 68;
            this.label62.Text = "Тип награды";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label60.Location = new System.Drawing.Point(24, 131);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(126, 15);
            this.label60.TabIndex = 70;
            this.label60.Text = "Номер документа";
            // 
            // label53
            // 
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label53.Location = new System.Drawing.Point(24, 19);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(118, 35);
            this.label53.TabIndex = 69;
            this.label53.Text = "Наименование награды";
            // 
            // tbNum_Reward
            // 
            this.tbNum_Reward.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Reward.Location = new System.Drawing.Point(150, 128);
            this.tbNum_Reward.Name = "tbNum_Reward";
            this.tbNum_Reward.Size = new System.Drawing.Size(75, 21);
            this.tbNum_Reward.TabIndex = 3;
            // 
            // tbType_Reward
            // 
            this.tbType_Reward.BackColor = System.Drawing.Color.White;
            this.tbType_Reward.Enabled = false;
            this.tbType_Reward.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbType_Reward.Location = new System.Drawing.Point(150, 61);
            this.tbType_Reward.Name = "tbType_Reward";
            this.tbType_Reward.Size = new System.Drawing.Size(406, 21);
            this.tbType_Reward.TabIndex = 1;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // Rewards
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(579, 226);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Rewards";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rewards";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Rewards_FormClosing);
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
        private System.Windows.Forms.CheckBox chGov_Sign;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox tbNum_Reward;
        private System.Windows.Forms.TextBox tbType_Reward;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Label label1;
        private EditorsLibrary.DateEditor deDate_Reward;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRew_Doc_Name;
        private System.Windows.Forms.ComboBox cbReward_Name;
    }
}