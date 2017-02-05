namespace Kadr
{
    partial class PrevTransfer
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
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.deDate_Transfer = new EditorsLibrary.DateEditor();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSign_Comb = new System.Windows.Forms.CheckBox();
            this.deTr_Date_Order = new EditorsLibrary.DateEditor();
            this.tbTr_Num_Order = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cbPos_Name = new System.Windows.Forms.ComboBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCode_Pos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnButton.SuspendLayout();
            this.gbAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 144);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(665, 38);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(560, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
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
            this.btSave.Location = new System.Drawing.Point(467, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(87, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // gbAll
            // 
            this.gbAll.Controls.Add(this.label12);
            this.gbAll.Controls.Add(this.deTr_Date_Order);
            this.gbAll.Controls.Add(this.deDate_Transfer);
            this.gbAll.Controls.Add(this.tbTr_Num_Order);
            this.gbAll.Controls.Add(this.label13);
            this.gbAll.Controls.Add(this.label9);
            this.gbAll.Controls.Add(this.cbSign_Comb);
            this.gbAll.Controls.Add(this.cbPos_Name);
            this.gbAll.Controls.Add(this.cbSubdiv_Name);
            this.gbAll.Controls.Add(this.label7);
            this.gbAll.Controls.Add(this.tbPer_Num);
            this.gbAll.Controls.Add(this.label4);
            this.gbAll.Controls.Add(this.tbCode_Subdiv);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Controls.Add(this.tbCode_Pos);
            this.gbAll.Controls.Add(this.label2);
            this.gbAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAll.Location = new System.Drawing.Point(0, 0);
            this.gbAll.Name = "gbAll";
            this.gbAll.Size = new System.Drawing.Size(665, 144);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            // 
            // deDate_Transfer
            // 
            this.deDate_Transfer.AutoSize = true;
            this.deDate_Transfer.Date = null;
            this.deDate_Transfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Transfer.Location = new System.Drawing.Point(153, 111);
            this.deDate_Transfer.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Transfer.Name = "deDate_Transfer";
            this.deDate_Transfer.Size = new System.Drawing.Size(73, 24);
            this.deDate_Transfer.TabIndex = 6;
            this.deDate_Transfer.Validating += new System.ComponentModel.CancelEventHandler(this.deDate_Transfer_Validating);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(551, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 15);
            this.label9.TabIndex = 1;
            this.label9.Text = "Совмещение";
            // 
            // cbSign_Comb
            // 
            this.cbSign_Comb.AutoSize = true;
            this.cbSign_Comb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSign_Comb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbSign_Comb.Location = new System.Drawing.Point(536, 23);
            this.cbSign_Comb.Name = "cbSign_Comb";
            this.cbSign_Comb.Size = new System.Drawing.Size(111, 19);
            this.cbSign_Comb.TabIndex = 1;
            this.cbSign_Comb.Text = "Совмещение";
            this.cbSign_Comb.UseVisualStyleBackColor = true;
            // 
            // deTr_Date_Order
            // 
            this.deTr_Date_Order.AutoSize = true;
            this.deTr_Date_Order.Date = null;
            this.deTr_Date_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deTr_Date_Order.Location = new System.Drawing.Point(570, 111);
            this.deTr_Date_Order.Margin = new System.Windows.Forms.Padding(5);
            this.deTr_Date_Order.Name = "deTr_Date_Order";
            this.deTr_Date_Order.Size = new System.Drawing.Size(73, 24);
            this.deTr_Date_Order.TabIndex = 8;
            // 
            // tbTr_Num_Order
            // 
            this.tbTr_Num_Order.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbTr_Num_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTr_Num_Order.Location = new System.Drawing.Point(374, 111);
            this.tbTr_Num_Order.Name = "tbTr_Num_Order";
            this.tbTr_Num_Order.Size = new System.Drawing.Size(52, 21);
            this.tbTr_Num_Order.TabIndex = 7;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(464, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 15);
            this.label12.TabIndex = 66;
            this.label12.Text = "Дата приказа";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(259, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(109, 15);
            this.label13.TabIndex = 65;
            this.label13.Text = "Номер приказа";
            // 
            // cbPos_Name
            // 
            this.cbPos_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbPos_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPos_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPos_Name.FormattingEnabled = true;
            this.cbPos_Name.Location = new System.Drawing.Point(217, 78);
            this.cbPos_Name.Name = "cbPos_Name";
            this.cbPos_Name.Size = new System.Drawing.Size(426, 23);
            this.cbPos_Name.TabIndex = 5;
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(217, 50);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(426, 23);
            this.cbSubdiv_Name.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(21, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 15);
            this.label7.TabIndex = 62;
            this.label7.Text = "Дата перевода";
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.BackColor = System.Drawing.Color.White;
            this.tbPer_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_Num.Location = new System.Drawing.Point(153, 21);
            this.tbPer_Num.MaxLength = 5;
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(58, 21);
            this.tbPer_Num.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(21, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 15);
            this.label4.TabIndex = 50;
            this.label4.Text = "Табельный номер";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(153, 50);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Subdiv.TabIndex = 2;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(21, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 48;
            this.label1.Text = "Подразделение";
            // 
            // tbCode_Pos
            // 
            this.tbCode_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Pos.Location = new System.Drawing.Point(153, 80);
            this.tbCode_Pos.Name = "tbCode_Pos";
            this.tbCode_Pos.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Pos.TabIndex = 4;
            this.tbCode_Pos.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Pos_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(21, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 49;
            this.label2.Text = "Профессия";
            // 
            // OldTransfer
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(665, 182);
            this.Controls.Add(this.gbAll);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OldTransfer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransferOld";
            this.pnButton.ResumeLayout(false);
            this.gbAll.ResumeLayout(false);
            this.gbAll.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox gbAll;
        private System.Windows.Forms.ComboBox cbPos_Name;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCode_Pos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox tbTr_Num_Order;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbSign_Comb;
        private EditorsLibrary.DateEditor deDate_Transfer;
        private EditorsLibrary.DateEditor deTr_Date_Order;
    }
}