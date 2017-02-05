namespace Kadr
{
    partial class HBSubdiv
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btSaveSubdiv = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btDeleteSubdiv = new System.Windows.Forms.Button();
            this.btEditSubdiv = new System.Windows.Forms.Button();
            this.btAddSubdiv = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgSubdiv = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deSub_Date_End = new EditorsLibrary.DateEditor();
            this.deSub_Date_Start = new EditorsLibrary.DateEditor();
            this.cbGr_Work_ID = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbType_Subdiv = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbWork_Type_ID = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbService_ID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbPos_Actual_Sign = new System.Windows.Forms.Label();
            this.chSub_Actual_Sign = new System.Windows.Forms.CheckBox();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.lbName2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbName1 = new System.Windows.Forms.Label();
            this.tbName_Subdiv = new System.Windows.Forms.TextBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.pnButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSubdiv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btSaveSubdiv);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btDeleteSubdiv);
            this.pnButton.Controls.Add(this.btEditSubdiv);
            this.pnButton.Controls.Add(this.btAddSubdiv);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 465);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(859, 40);
            this.pnButton.TabIndex = 2;
            // 
            // btSaveSubdiv
            // 
            this.btSaveSubdiv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveSubdiv.BackColor = System.Drawing.SystemColors.Control;
            this.btSaveSubdiv.Enabled = false;
            this.btSaveSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSaveSubdiv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btSaveSubdiv.Location = new System.Drawing.Point(653, 9);
            this.btSaveSubdiv.Name = "btSaveSubdiv";
            this.btSaveSubdiv.Size = new System.Drawing.Size(88, 23);
            this.btSaveSubdiv.TabIndex = 3;
            this.btSaveSubdiv.Text = "Сохранить";
            this.btSaveSubdiv.UseVisualStyleBackColor = false;
            this.btSaveSubdiv.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.BackColor = System.Drawing.SystemColors.Control;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(747, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(88, 23);
            this.btExit.TabIndex = 4;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = false;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btDeleteSubdiv
            // 
            this.btDeleteSubdiv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeleteSubdiv.BackColor = System.Drawing.SystemColors.Control;
            this.btDeleteSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeleteSubdiv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btDeleteSubdiv.Location = new System.Drawing.Point(559, 9);
            this.btDeleteSubdiv.Name = "btDeleteSubdiv";
            this.btDeleteSubdiv.Size = new System.Drawing.Size(88, 23);
            this.btDeleteSubdiv.TabIndex = 2;
            this.btDeleteSubdiv.Text = "Удалить";
            this.btDeleteSubdiv.UseVisualStyleBackColor = false;
            this.btDeleteSubdiv.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btEditSubdiv
            // 
            this.btEditSubdiv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditSubdiv.BackColor = System.Drawing.SystemColors.Control;
            this.btEditSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditSubdiv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btEditSubdiv.Location = new System.Drawing.Point(431, 9);
            this.btEditSubdiv.Name = "btEditSubdiv";
            this.btEditSubdiv.Size = new System.Drawing.Size(122, 23);
            this.btEditSubdiv.TabIndex = 1;
            this.btEditSubdiv.Text = "Редактировать";
            this.btEditSubdiv.UseVisualStyleBackColor = false;
            this.btEditSubdiv.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btAddSubdiv
            // 
            this.btAddSubdiv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddSubdiv.BackColor = System.Drawing.SystemColors.Control;
            this.btAddSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddSubdiv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btAddSubdiv.Location = new System.Drawing.Point(337, 9);
            this.btAddSubdiv.Name = "btAddSubdiv";
            this.btAddSubdiv.Size = new System.Drawing.Size(88, 23);
            this.btAddSubdiv.TabIndex = 0;
            this.btAddSubdiv.Text = "Добавить";
            this.btAddSubdiv.UseVisualStyleBackColor = false;
            this.btAddSubdiv.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgSubdiv);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Size = new System.Drawing.Size(859, 465);
            this.splitContainer.SplitterDistance = 530;
            this.splitContainer.TabIndex = 3;
            // 
            // dgSubdiv
            // 
            this.dgSubdiv.AllowUserToAddRows = false;
            this.dgSubdiv.AllowUserToDeleteRows = false;
            this.dgSubdiv.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSubdiv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgSubdiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSubdiv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSubdiv.Location = new System.Drawing.Point(0, 0);
            this.dgSubdiv.Name = "dgSubdiv";
            this.dgSubdiv.ReadOnly = true;
            this.dgSubdiv.RowHeadersWidth = 25;
            this.dgSubdiv.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgSubdiv.Size = new System.Drawing.Size(530, 465);
            this.dgSubdiv.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deSub_Date_End);
            this.groupBox1.Controls.Add(this.deSub_Date_Start);
            this.groupBox1.Controls.Add(this.cbGr_Work_ID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbType_Subdiv);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbWork_Type_ID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbService_ID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbPos_Actual_Sign);
            this.groupBox1.Controls.Add(this.chSub_Actual_Sign);
            this.groupBox1.Controls.Add(this.tbCode_Subdiv);
            this.groupBox1.Controls.Add(this.lbName2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbName1);
            this.groupBox1.Controls.Add(this.tbName_Subdiv);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 465);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Реквизиты";
            // 
            // deSub_Date_End
            // 
            this.deSub_Date_End.AutoSize = true;
            this.deSub_Date_End.BackColor = System.Drawing.SystemColors.Control;
            this.deSub_Date_End.Date = null;
            this.deSub_Date_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deSub_Date_End.Location = new System.Drawing.Point(156, 165);
            this.deSub_Date_End.Margin = new System.Windows.Forms.Padding(5);
            this.deSub_Date_End.Name = "deSub_Date_End";
            this.deSub_Date_End.ReadOnly = false;
            this.deSub_Date_End.Size = new System.Drawing.Size(73, 24);
            this.deSub_Date_End.TabIndex = 3;
            this.deSub_Date_End.TextDate = null;
            // 
            // deSub_Date_Start
            // 
            this.deSub_Date_Start.AutoSize = true;
            this.deSub_Date_Start.BackColor = System.Drawing.SystemColors.Control;
            this.deSub_Date_Start.Date = null;
            this.deSub_Date_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deSub_Date_Start.Location = new System.Drawing.Point(10, 165);
            this.deSub_Date_Start.Margin = new System.Windows.Forms.Padding(5);
            this.deSub_Date_Start.Name = "deSub_Date_Start";
            this.deSub_Date_Start.ReadOnly = false;
            this.deSub_Date_Start.Size = new System.Drawing.Size(73, 24);
            this.deSub_Date_Start.TabIndex = 2;
            this.deSub_Date_Start.TextDate = null;
            // 
            // cbGr_Work_ID
            // 
            this.cbGr_Work_ID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGr_Work_ID.Enabled = false;
            this.cbGr_Work_ID.FormattingEnabled = true;
            this.cbGr_Work_ID.Location = new System.Drawing.Point(11, 391);
            this.cbGr_Work_ID.Name = "cbGr_Work_ID";
            this.cbGr_Work_ID.Size = new System.Drawing.Size(303, 23);
            this.cbGr_Work_ID.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 373);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(219, 15);
            this.label6.TabIndex = 59;
            this.label6.Text = "График работы подразделения";
            // 
            // cbType_Subdiv
            // 
            this.cbType_Subdiv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbType_Subdiv.Enabled = false;
            this.cbType_Subdiv.FormattingEnabled = true;
            this.cbType_Subdiv.Location = new System.Drawing.Point(10, 342);
            this.cbType_Subdiv.Name = "cbType_Subdiv";
            this.cbType_Subdiv.Size = new System.Drawing.Size(303, 23);
            this.cbType_Subdiv.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 324);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 15);
            this.label5.TabIndex = 59;
            this.label5.Text = "Тип подразделения";
            // 
            // cbWork_Type_ID
            // 
            this.cbWork_Type_ID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbWork_Type_ID.Enabled = false;
            this.cbWork_Type_ID.FormattingEnabled = true;
            this.cbWork_Type_ID.Location = new System.Drawing.Point(10, 291);
            this.cbWork_Type_ID.Name = "cbWork_Type_ID";
            this.cbWork_Type_ID.Size = new System.Drawing.Size(303, 23);
            this.cbWork_Type_ID.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 15);
            this.label4.TabIndex = 59;
            this.label4.Text = "Характер работы";
            // 
            // cbService_ID
            // 
            this.cbService_ID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbService_ID.Enabled = false;
            this.cbService_ID.FormattingEnabled = true;
            this.cbService_ID.Location = new System.Drawing.Point(10, 241);
            this.cbService_ID.Name = "cbService_ID";
            this.cbService_ID.Size = new System.Drawing.Size(303, 23);
            this.cbService_ID.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 59;
            this.label3.Text = "Служба";
            // 
            // lbPos_Actual_Sign
            // 
            this.lbPos_Actual_Sign.AutoSize = true;
            this.lbPos_Actual_Sign.Location = new System.Drawing.Point(28, 199);
            this.lbPos_Actual_Sign.Name = "lbPos_Actual_Sign";
            this.lbPos_Actual_Sign.Size = new System.Drawing.Size(205, 15);
            this.lbPos_Actual_Sign.TabIndex = 4;
            this.lbPos_Actual_Sign.Text = "Действующее подразделение";
            // 
            // chSub_Actual_Sign
            // 
            this.chSub_Actual_Sign.AutoSize = true;
            this.chSub_Actual_Sign.Enabled = false;
            this.chSub_Actual_Sign.Location = new System.Drawing.Point(11, 198);
            this.chSub_Actual_Sign.Name = "chSub_Actual_Sign";
            this.chSub_Actual_Sign.Size = new System.Drawing.Size(224, 19);
            this.chSub_Actual_Sign.TabIndex = 4;
            this.chSub_Actual_Sign.Text = "Действующее подразделение";
            this.chSub_Actual_Sign.UseVisualStyleBackColor = true;
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.BackColor = System.Drawing.Color.White;
            this.tbCode_Subdiv.Enabled = false;
            this.tbCode_Subdiv.Location = new System.Drawing.Point(10, 45);
            this.tbCode_Subdiv.Multiline = true;
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(100, 21);
            this.tbCode_Subdiv.TabIndex = 0;
            // 
            // lbName2
            // 
            this.lbName2.AutoSize = true;
            this.lbName2.Location = new System.Drawing.Point(11, 27);
            this.lbName2.Name = "lbName2";
            this.lbName2.Size = new System.Drawing.Size(154, 15);
            this.lbName2.TabIndex = 1;
            this.lbName2.Text = "Шифр подразделения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Дата ограничения";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Дата введения";
            // 
            // lbName1
            // 
            this.lbName1.AutoSize = true;
            this.lbName1.Location = new System.Drawing.Point(11, 71);
            this.lbName1.Name = "lbName1";
            this.lbName1.Size = new System.Drawing.Size(214, 15);
            this.lbName1.TabIndex = 1;
            this.lbName1.Text = "Наименование подразделения";
            // 
            // tbName_Subdiv
            // 
            this.tbName_Subdiv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName_Subdiv.BackColor = System.Drawing.Color.White;
            this.tbName_Subdiv.Enabled = false;
            this.tbName_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbName_Subdiv.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbName_Subdiv.Location = new System.Drawing.Point(10, 89);
            this.tbName_Subdiv.Multiline = true;
            this.tbName_Subdiv.Name = "tbName_Subdiv";
            this.tbName_Subdiv.Size = new System.Drawing.Size(303, 54);
            this.tbName_Subdiv.TabIndex = 1;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // HBSubdiv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 505);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnButton);
            this.Name = "HBSubdiv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник подразделений";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.HBSubdiv_Activated);
            this.pnButton.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSubdiv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        public System.Windows.Forms.Button btSaveSubdiv;
        public System.Windows.Forms.Button btExit;
        public System.Windows.Forms.Button btDeleteSubdiv;
        public System.Windows.Forms.Button btEditSubdiv;
        public System.Windows.Forms.Button btAddSubdiv;
        public System.Windows.Forms.SplitContainer splitContainer;
        public System.Windows.Forms.DataGridView dgSubdiv;
        public System.Windows.Forms.GroupBox groupBox1;
        private EditorsLibrary.DateEditor deSub_Date_End;
        private EditorsLibrary.DateEditor deSub_Date_Start;
        private System.Windows.Forms.ComboBox cbService_ID;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lbPos_Actual_Sign;
        private System.Windows.Forms.CheckBox chSub_Actual_Sign;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        public System.Windows.Forms.Label lbName2;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lbName1;
        public System.Windows.Forms.TextBox tbName_Subdiv;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ComboBox cbWork_Type_ID;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbType_Subdiv;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbGr_Work_ID;
        public System.Windows.Forms.Label label6;
    }
}