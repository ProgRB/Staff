namespace Kadr
{
    partial class TransferToFR
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
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.cbPos_Name = new System.Windows.Forms.ComboBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.tbCode_Degree = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDegree_Name = new System.Windows.Forms.ComboBox();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCode_Pos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbDismiss = new System.Windows.Forms.GroupBox();
            this.deTransferD = new EditorsLibrary.DateEditor();
            this.deTr_Date_Dismiss = new EditorsLibrary.DateEditor();
            this.label30 = new System.Windows.Forms.Label();
            this.cbChan_SignDismiss = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cbReason_dismiss = new System.Windows.Forms.ComboBox();
            this.cbBase_doc = new System.Windows.Forms.ComboBox();
            this.tbTr_Num_Dismiss = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btPreview = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.gbAll.SuspendLayout();
            this.gbDismiss.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // gbAll
            // 
            this.gbAll.Controls.Add(this.cbPos_Name);
            this.gbAll.Controls.Add(this.cbSubdiv_Name);
            this.gbAll.Controls.Add(this.tbCode_Degree);
            this.gbAll.Controls.Add(this.label5);
            this.gbAll.Controls.Add(this.cbDegree_Name);
            this.gbAll.Controls.Add(this.tbPer_Num);
            this.gbAll.Controls.Add(this.label4);
            this.gbAll.Controls.Add(this.tbCode_Subdiv);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Controls.Add(this.tbCode_Pos);
            this.gbAll.Controls.Add(this.label2);
            this.gbAll.Controls.Add(this.gbDismiss);
            this.gbAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAll.Location = new System.Drawing.Point(0, 0);
            this.gbAll.Name = "gbAll";
            this.gbAll.Size = new System.Drawing.Size(653, 520);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            // 
            // cbPos_Name
            // 
            this.cbPos_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbPos_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPos_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPos_Name.FormattingEnabled = true;
            this.cbPos_Name.Location = new System.Drawing.Point(206, 75);
            this.cbPos_Name.Name = "cbPos_Name";
            this.cbPos_Name.Size = new System.Drawing.Size(426, 23);
            this.cbPos_Name.TabIndex = 86;
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(206, 45);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(426, 23);
            this.cbSubdiv_Name.TabIndex = 84;
            // 
            // tbCode_Degree
            // 
            this.tbCode_Degree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Degree.Location = new System.Drawing.Point(142, 104);
            this.tbCode_Degree.Multiline = true;
            this.tbCode_Degree.Name = "tbCode_Degree";
            this.tbCode_Degree.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Degree.TabIndex = 93;
            this.tbCode_Degree.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Degree_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(20, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 101;
            this.label5.Text = "Категория";
            // 
            // cbDegree_Name
            // 
            this.cbDegree_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbDegree_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDegree_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDegree_Name.FormattingEnabled = true;
            this.cbDegree_Name.Location = new System.Drawing.Point(206, 104);
            this.cbDegree_Name.Name = "cbDegree_Name";
            this.cbDegree_Name.Size = new System.Drawing.Size(426, 23);
            this.cbDegree_Name.TabIndex = 94;
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_Num.Location = new System.Drawing.Point(142, 16);
            this.tbPer_Num.MaxLength = 5;
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(58, 21);
            this.tbPer_Num.TabIndex = 82;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(20, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 15);
            this.label4.TabIndex = 97;
            this.label4.Text = "Табельный №";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(142, 45);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Subdiv.TabIndex = 83;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(20, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 95;
            this.label1.Text = "Подразделение";
            // 
            // tbCode_Pos
            // 
            this.tbCode_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Pos.Location = new System.Drawing.Point(142, 75);
            this.tbCode_Pos.Name = "tbCode_Pos";
            this.tbCode_Pos.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Pos.TabIndex = 85;
            this.tbCode_Pos.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Pos_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(20, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 96;
            this.label2.Text = "Профессия";
            // 
            // gbDismiss
            // 
            this.gbDismiss.Controls.Add(this.deTransferD);
            this.gbDismiss.Controls.Add(this.deTr_Date_Dismiss);
            this.gbDismiss.Controls.Add(this.label30);
            this.gbDismiss.Controls.Add(this.cbChan_SignDismiss);
            this.gbDismiss.Controls.Add(this.label27);
            this.gbDismiss.Controls.Add(this.label26);
            this.gbDismiss.Controls.Add(this.label21);
            this.gbDismiss.Controls.Add(this.cbReason_dismiss);
            this.gbDismiss.Controls.Add(this.cbBase_doc);
            this.gbDismiss.Controls.Add(this.tbTr_Num_Dismiss);
            this.gbDismiss.Controls.Add(this.label8);
            this.gbDismiss.Controls.Add(this.label13);
            this.gbDismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbDismiss.Location = new System.Drawing.Point(21, 136);
            this.gbDismiss.Name = "gbDismiss";
            this.gbDismiss.Size = new System.Drawing.Size(611, 110);
            this.gbDismiss.TabIndex = 22;
            this.gbDismiss.TabStop = false;
            this.gbDismiss.Text = "Приказ об увольнении";
            // 
            // deTransferD
            // 
            this.deTransferD.AutoSize = true;
            this.deTransferD.Date = null;
            this.deTransferD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deTransferD.Location = new System.Drawing.Point(525, 49);
            this.deTransferD.Margin = new System.Windows.Forms.Padding(5);
            this.deTransferD.Name = "deTransferD";
            this.deTransferD.Size = new System.Drawing.Size(73, 24);
            this.deTransferD.TabIndex = 3;
            // 
            // deTr_Date_Dismiss
            // 
            this.deTr_Date_Dismiss.AutoSize = true;
            this.deTr_Date_Dismiss.Date = null;
            this.deTr_Date_Dismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deTr_Date_Dismiss.Location = new System.Drawing.Point(56, 78);
            this.deTr_Date_Dismiss.Margin = new System.Windows.Forms.Padding(5);
            this.deTr_Date_Dismiss.Name = "deTr_Date_Dismiss";
            this.deTr_Date_Dismiss.Size = new System.Drawing.Size(73, 24);
            this.deTr_Date_Dismiss.TabIndex = 1;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label30.Location = new System.Drawing.Point(74, 25);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(146, 15);
            this.label30.TabIndex = 82;
            this.label30.Text = "Признак канцелярии";
            // 
            // cbChan_SignDismiss
            // 
            this.cbChan_SignDismiss.AutoSize = true;
            this.cbChan_SignDismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbChan_SignDismiss.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbChan_SignDismiss.Location = new System.Drawing.Point(56, 26);
            this.cbChan_SignDismiss.Name = "cbChan_SignDismiss";
            this.cbChan_SignDismiss.Size = new System.Drawing.Size(15, 14);
            this.cbChan_SignDismiss.TabIndex = 81;
            this.cbChan_SignDismiss.UseVisualStyleBackColor = true;
            this.cbChan_SignDismiss.CheckedChanged += new System.EventHandler(this.cbChan_SignDismiss_CheckedChanged_1);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Location = new System.Drawing.Point(446, 52);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(79, 15);
            this.label27.TabIndex = 70;
            this.label27.Text = "Дата увол.";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label26.Location = new System.Drawing.Point(135, 81);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(64, 15);
            this.label26.TabIndex = 68;
            this.label26.Text = "Причина";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(135, 52);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(80, 15);
            this.label21.TabIndex = 68;
            this.label21.Text = "Основание";
            // 
            // cbReason_dismiss
            // 
            this.cbReason_dismiss.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbReason_dismiss.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbReason_dismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbReason_dismiss.FormattingEnabled = true;
            this.cbReason_dismiss.Location = new System.Drawing.Point(221, 78);
            this.cbReason_dismiss.Name = "cbReason_dismiss";
            this.cbReason_dismiss.Size = new System.Drawing.Size(378, 23);
            this.cbReason_dismiss.TabIndex = 4;
            // 
            // cbBase_doc
            // 
            this.cbBase_doc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbBase_doc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbBase_doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBase_doc.FormattingEnabled = true;
            this.cbBase_doc.Location = new System.Drawing.Point(221, 49);
            this.cbBase_doc.Name = "cbBase_doc";
            this.cbBase_doc.Size = new System.Drawing.Size(219, 23);
            this.cbBase_doc.TabIndex = 2;
            // 
            // tbTr_Num_Dismiss
            // 
            this.tbTr_Num_Dismiss.Enabled = false;
            this.tbTr_Num_Dismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTr_Num_Dismiss.Location = new System.Drawing.Point(56, 49);
            this.tbTr_Num_Dismiss.Name = "tbTr_Num_Dismiss";
            this.tbTr_Num_Dismiss.Size = new System.Drawing.Size(73, 21);
            this.tbTr_Num_Dismiss.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(13, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 15);
            this.label8.TabIndex = 17;
            this.label8.Text = "Дата";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(13, 52);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(20, 15);
            this.label13.TabIndex = 30;
            this.label13.Text = "№";
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btPreview);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btEdit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 252);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(653, 38);
            this.pnButton.TabIndex = 2;
            // 
            // btPreview
            // 
            this.btPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btPreview.Location = new System.Drawing.Point(455, 6);
            this.btPreview.Name = "btPreview";
            this.btPreview.Size = new System.Drawing.Size(87, 23);
            this.btPreview.TabIndex = 2;
            this.btPreview.Text = "Просмотр";
            this.btPreview.UseVisualStyleBackColor = true;
            this.btPreview.Click += new System.EventHandler(this.btPreview_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(548, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 3;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btEdit.Location = new System.Drawing.Point(229, 6);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(127, 23);
            this.btEdit.TabIndex = 0;
            this.btEdit.Text = "Редактировать";
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSave.Location = new System.Drawing.Point(362, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(87, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // TransferToFR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 290);
            this.Controls.Add(this.pnButton);
            this.Controls.Add(this.gbAll);
            this.MaximizeBox = false;
            this.Name = "TransferToFR";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transfer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Transfer_FormClosing);
            this.gbAll.ResumeLayout(false);
            this.gbAll.PerformLayout();
            this.gbDismiss.ResumeLayout(false);
            this.gbDismiss.PerformLayout();
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox gbAll;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btPreview;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox gbDismiss;
        private EditorsLibrary.DateEditor deTransferD;
        private EditorsLibrary.DateEditor deTr_Date_Dismiss;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.CheckBox cbChan_SignDismiss;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbReason_dismiss;
        private System.Windows.Forms.ComboBox cbBase_doc;
        private System.Windows.Forms.TextBox tbTr_Num_Dismiss;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbPos_Name;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.TextBox tbCode_Degree;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDegree_Name;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCode_Pos;
        private System.Windows.Forms.Label label2;
    }
}