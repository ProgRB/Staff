namespace Kadr
{
    partial class HBPosition
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btSavePosition = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btDeletePosition = new System.Windows.Forms.Button();
            this.btEditPosition = new System.Windows.Forms.Button();
            this.btAddPosition = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgPosition = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dePos_Date_End = new EditorsLibrary.DateEditor();
            this.dePos_Date_Start = new EditorsLibrary.DateEditor();
            this.cbFrom_Pos_ID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbPos_Actual_Sign = new System.Windows.Forms.Label();
            this.chPos_Actual_Sign = new System.Windows.Forms.CheckBox();
            this.tbCode_Pos = new System.Windows.Forms.TextBox();
            this.lbName2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbName1 = new System.Windows.Forms.Label();
            this.tbName_Pos = new System.Windows.Forms.TextBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.chPos_Chief_Or_Deputy_Sign = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPosition)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btSavePosition);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btDeletePosition);
            this.pnButton.Controls.Add(this.btEditPosition);
            this.pnButton.Controls.Add(this.btAddPosition);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 408);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(773, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btSavePosition
            // 
            this.btSavePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSavePosition.BackColor = System.Drawing.SystemColors.Control;
            this.btSavePosition.Enabled = false;
            this.btSavePosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSavePosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btSavePosition.Location = new System.Drawing.Point(567, 9);
            this.btSavePosition.Name = "btSavePosition";
            this.btSavePosition.Size = new System.Drawing.Size(88, 23);
            this.btSavePosition.TabIndex = 3;
            this.btSavePosition.Text = "Сохранить";
            this.btSavePosition.UseVisualStyleBackColor = false;
            this.btSavePosition.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.BackColor = System.Drawing.SystemColors.Control;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(661, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(88, 23);
            this.btExit.TabIndex = 4;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = false;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btDeletePosition
            // 
            this.btDeletePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeletePosition.BackColor = System.Drawing.SystemColors.Control;
            this.btDeletePosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeletePosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btDeletePosition.Location = new System.Drawing.Point(473, 9);
            this.btDeletePosition.Name = "btDeletePosition";
            this.btDeletePosition.Size = new System.Drawing.Size(88, 23);
            this.btDeletePosition.TabIndex = 2;
            this.btDeletePosition.Text = "Удалить";
            this.btDeletePosition.UseVisualStyleBackColor = false;
            this.btDeletePosition.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btEditPosition
            // 
            this.btEditPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditPosition.BackColor = System.Drawing.SystemColors.Control;
            this.btEditPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btEditPosition.Location = new System.Drawing.Point(345, 9);
            this.btEditPosition.Name = "btEditPosition";
            this.btEditPosition.Size = new System.Drawing.Size(122, 23);
            this.btEditPosition.TabIndex = 1;
            this.btEditPosition.Text = "Редактировать";
            this.btEditPosition.UseVisualStyleBackColor = false;
            this.btEditPosition.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btAddPosition
            // 
            this.btAddPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddPosition.BackColor = System.Drawing.SystemColors.Control;
            this.btAddPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btAddPosition.Location = new System.Drawing.Point(251, 9);
            this.btAddPosition.Name = "btAddPosition";
            this.btAddPosition.Size = new System.Drawing.Size(88, 23);
            this.btAddPosition.TabIndex = 0;
            this.btAddPosition.Text = "Добавить";
            this.btAddPosition.UseVisualStyleBackColor = false;
            this.btAddPosition.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgPosition);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Size = new System.Drawing.Size(773, 408);
            this.splitContainer.SplitterDistance = 477;
            this.splitContainer.TabIndex = 0;
            // 
            // dgPosition
            // 
            this.dgPosition.AllowUserToAddRows = false;
            this.dgPosition.AllowUserToDeleteRows = false;
            this.dgPosition.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPosition.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgPosition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPosition.Location = new System.Drawing.Point(0, 0);
            this.dgPosition.Name = "dgPosition";
            this.dgPosition.ReadOnly = true;
            this.dgPosition.RowHeadersWidth = 25;
            this.dgPosition.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgPosition.Size = new System.Drawing.Size(477, 408);
            this.dgPosition.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chPos_Chief_Or_Deputy_Sign);
            this.groupBox1.Controls.Add(this.dePos_Date_End);
            this.groupBox1.Controls.Add(this.dePos_Date_Start);
            this.groupBox1.Controls.Add(this.cbFrom_Pos_ID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbPos_Actual_Sign);
            this.groupBox1.Controls.Add(this.chPos_Actual_Sign);
            this.groupBox1.Controls.Add(this.tbCode_Pos);
            this.groupBox1.Controls.Add(this.lbName2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbName1);
            this.groupBox1.Controls.Add(this.tbName_Pos);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 408);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Реквизиты";
            // 
            // dePos_Date_End
            // 
            this.dePos_Date_End.AutoSize = true;
            this.dePos_Date_End.BackColor = System.Drawing.SystemColors.Control;
            this.dePos_Date_End.Date = null;
            this.dePos_Date_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dePos_Date_End.Location = new System.Drawing.Point(156, 225);
            this.dePos_Date_End.Margin = new System.Windows.Forms.Padding(5);
            this.dePos_Date_End.Name = "dePos_Date_End";
            this.dePos_Date_End.ReadOnly = false;
            this.dePos_Date_End.Size = new System.Drawing.Size(73, 24);
            this.dePos_Date_End.TabIndex = 3;
            this.dePos_Date_End.TextDate = null;
            // 
            // dePos_Date_Start
            // 
            this.dePos_Date_Start.AutoSize = true;
            this.dePos_Date_Start.BackColor = System.Drawing.SystemColors.Control;
            this.dePos_Date_Start.Date = null;
            this.dePos_Date_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dePos_Date_Start.Location = new System.Drawing.Point(10, 225);
            this.dePos_Date_Start.Margin = new System.Windows.Forms.Padding(5);
            this.dePos_Date_Start.Name = "dePos_Date_Start";
            this.dePos_Date_Start.ReadOnly = false;
            this.dePos_Date_Start.Size = new System.Drawing.Size(73, 24);
            this.dePos_Date_Start.TabIndex = 2;
            this.dePos_Date_Start.TextDate = null;
            // 
            // cbFrom_Pos_ID
            // 
            this.cbFrom_Pos_ID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFrom_Pos_ID.Enabled = false;
            this.cbFrom_Pos_ID.FormattingEnabled = true;
            this.cbFrom_Pos_ID.Location = new System.Drawing.Point(10, 316);
            this.cbFrom_Pos_ID.Name = "cbFrom_Pos_ID";
            this.cbFrom_Pos_ID.Size = new System.Drawing.Size(270, 23);
            this.cbFrom_Pos_ID.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 15);
            this.label3.TabIndex = 59;
            this.label3.Text = "С какой должности";
            // 
            // lbPos_Actual_Sign
            // 
            this.lbPos_Actual_Sign.AutoSize = true;
            this.lbPos_Actual_Sign.Location = new System.Drawing.Point(31, 264);
            this.lbPos_Actual_Sign.Name = "lbPos_Actual_Sign";
            this.lbPos_Actual_Sign.Size = new System.Drawing.Size(221, 15);
            this.lbPos_Actual_Sign.TabIndex = 4;
            this.lbPos_Actual_Sign.Text = "Признак актуальной должности";
            // 
            // chPos_Actual_Sign
            // 
            this.chPos_Actual_Sign.AutoSize = true;
            this.chPos_Actual_Sign.Enabled = false;
            this.chPos_Actual_Sign.Location = new System.Drawing.Point(11, 263);
            this.chPos_Actual_Sign.Name = "chPos_Actual_Sign";
            this.chPos_Actual_Sign.Size = new System.Drawing.Size(234, 19);
            this.chPos_Actual_Sign.TabIndex = 4;
            this.chPos_Actual_Sign.Text = "Признак высшего образования";
            this.chPos_Actual_Sign.UseVisualStyleBackColor = true;
            // 
            // tbCode_Pos
            // 
            this.tbCode_Pos.BackColor = System.Drawing.Color.White;
            this.tbCode_Pos.Enabled = false;
            this.tbCode_Pos.Location = new System.Drawing.Point(10, 55);
            this.tbCode_Pos.Multiline = true;
            this.tbCode_Pos.Name = "tbCode_Pos";
            this.tbCode_Pos.Size = new System.Drawing.Size(100, 21);
            this.tbCode_Pos.TabIndex = 0;
            // 
            // lbName2
            // 
            this.lbName2.AutoSize = true;
            this.lbName2.Location = new System.Drawing.Point(11, 37);
            this.lbName2.Name = "lbName2";
            this.lbName2.Size = new System.Drawing.Size(124, 15);
            this.lbName2.TabIndex = 1;
            this.lbName2.Text = "Шифр должности";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Дата ограничения";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Дата введения";
            // 
            // lbName1
            // 
            this.lbName1.AutoSize = true;
            this.lbName1.Location = new System.Drawing.Point(11, 94);
            this.lbName1.Name = "lbName1";
            this.lbName1.Size = new System.Drawing.Size(184, 15);
            this.lbName1.TabIndex = 1;
            this.lbName1.Text = "Наименование должности";
            // 
            // tbName_Pos
            // 
            this.tbName_Pos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName_Pos.BackColor = System.Drawing.Color.White;
            this.tbName_Pos.Enabled = false;
            this.tbName_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbName_Pos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbName_Pos.Location = new System.Drawing.Point(10, 112);
            this.tbName_Pos.Multiline = true;
            this.tbName_Pos.Name = "tbName_Pos";
            this.tbName_Pos.Size = new System.Drawing.Size(270, 79);
            this.tbName_Pos.TabIndex = 1;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // chPos_Chief_Or_Deputy_Sign
            // 
            this.chPos_Chief_Or_Deputy_Sign.Enabled = false;
            this.chPos_Chief_Or_Deputy_Sign.Location = new System.Drawing.Point(10, 358);
            this.chPos_Chief_Or_Deputy_Sign.Name = "chPos_Chief_Or_Deputy_Sign";
            this.chPos_Chief_Or_Deputy_Sign.Size = new System.Drawing.Size(270, 34);
            this.chPos_Chief_Or_Deputy_Sign.TabIndex = 60;
            this.chPos_Chief_Or_Deputy_Sign.Text = "Признак руководящей должности или заместителя руководителя";
            this.chPos_Chief_Or_Deputy_Sign.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(27, 358);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(256, 34);
            this.label4.TabIndex = 61;
            this.label4.Text = "Признак руководящей должности или заместителя руководителя";
            // 
            // HBPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 448);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnButton);
            this.Name = "HBPosition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник должностей";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.HBPosition_Activated);
            this.pnButton.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPosition)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        public System.Windows.Forms.Button btSavePosition;
        public System.Windows.Forms.Button btExit;
        public System.Windows.Forms.Button btDeletePosition;
        public System.Windows.Forms.Button btEditPosition;
        public System.Windows.Forms.Button btAddPosition;
        public System.Windows.Forms.SplitContainer splitContainer;
        public System.Windows.Forms.DataGridView dgPosition;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label lbPos_Actual_Sign;
        private System.Windows.Forms.CheckBox chPos_Actual_Sign;
        private System.Windows.Forms.TextBox tbCode_Pos;
        public System.Windows.Forms.Label lbName2;
        public System.Windows.Forms.Label lbName1;
        public System.Windows.Forms.TextBox tbName_Pos;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ComboBox cbFrom_Pos_ID;
        public System.Windows.Forms.Label label3;
        private EditorsLibrary.DateEditor dePos_Date_End;
        private EditorsLibrary.DateEditor dePos_Date_Start;
        private System.Windows.Forms.CheckBox chPos_Chief_Or_Deputy_Sign;
        public System.Windows.Forms.Label label4;
    }
}