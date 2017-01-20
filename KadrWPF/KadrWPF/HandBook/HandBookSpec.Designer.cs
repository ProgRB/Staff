namespace Kadr
{
    partial class HandBookSpec
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btSave = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbNote = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbEdu_Sign = new System.Windows.Forms.Label();
            this.chEdu_Sign = new System.Windows.Forms.CheckBox();
            this.tbName3 = new System.Windows.Forms.TextBox();
            this.lbName3 = new System.Windows.Forms.Label();
            this.tbName2 = new System.Windows.Forms.TextBox();
            this.lbName2 = new System.Windows.Forms.Label();
            this.lbName1 = new System.Windows.Forms.Label();
            this.tbName1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gbNote.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btDelete);
            this.panel1.Controls.Add(this.btEdit);
            this.panel1.Controls.Add(this.btAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 408);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 40);
            this.panel1.TabIndex = 1;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.BackColor = System.Drawing.SystemColors.Control;
            this.btSave.Enabled = false;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btSave.Location = new System.Drawing.Point(567, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 3;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
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
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btDelete.Location = new System.Drawing.Point(473, 9);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(88, 23);
            this.btDelete.TabIndex = 2;
            this.btDelete.Text = "Удалить";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.BackColor = System.Drawing.SystemColors.Control;
            this.btEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btEdit.Location = new System.Drawing.Point(345, 9);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(122, 23);
            this.btEdit.TabIndex = 1;
            this.btEdit.Text = "Редактировать";
            this.btEdit.UseVisualStyleBackColor = false;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.btAdd.Location = new System.Drawing.Point(251, 9);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(88, 23);
            this.btAdd.TabIndex = 0;
            this.btAdd.Text = "Добавить";
            this.btAdd.UseVisualStyleBackColor = false;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Size = new System.Drawing.Size(773, 408);
            this.splitContainer.SplitterDistance = 389;
            this.splitContainer.TabIndex = 17;
            // 
            // dgView
            // 
            this.dgView.AllowUserToAddRows = false;
            this.dgView.AllowUserToDeleteRows = false;
            this.dgView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgView.Location = new System.Drawing.Point(0, 0);
            this.dgView.Name = "dgView";
            this.dgView.ReadOnly = true;
            this.dgView.RowHeadersWidth = 25;
            this.dgView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgView.Size = new System.Drawing.Size(389, 408);
            this.dgView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gbNote);
            this.groupBox1.Controls.Add(this.lbEdu_Sign);
            this.groupBox1.Controls.Add(this.chEdu_Sign);
            this.groupBox1.Controls.Add(this.tbName3);
            this.groupBox1.Controls.Add(this.lbName3);
            this.groupBox1.Controls.Add(this.tbName2);
            this.groupBox1.Controls.Add(this.lbName2);
            this.groupBox1.Controls.Add(this.lbName1);
            this.groupBox1.Controls.Add(this.tbName1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(213)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 408);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Реквизиты";
            // 
            // gbNote
            // 
            this.gbNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbNote.Controls.Add(this.label1);
            this.gbNote.Location = new System.Drawing.Point(10, 210);
            this.gbNote.Name = "gbNote";
            this.gbNote.Size = new System.Drawing.Size(358, 192);
            this.gbNote.TabIndex = 5;
            this.gbNote.TabStop = false;
            this.gbNote.Text = "Примечание для заполнения шаблонов";
            this.gbNote.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 105);
            this.label1.TabIndex = 1;
            this.label1.Text = "L - римское число\r\n\r\n9 - число (обязательное)\r\n\r\n0 - число (необязательное)\r\n\r\nC " +
                "- любой символ";
            // 
            // lbEdu_Sign
            // 
            this.lbEdu_Sign.AutoSize = true;
            this.lbEdu_Sign.Location = new System.Drawing.Point(30, 176);
            this.lbEdu_Sign.Name = "lbEdu_Sign";
            this.lbEdu_Sign.Size = new System.Drawing.Size(215, 15);
            this.lbEdu_Sign.TabIndex = 4;
            this.lbEdu_Sign.Text = "Признак высшего образования";
            this.lbEdu_Sign.Visible = false;
            // 
            // chEdu_Sign
            // 
            this.chEdu_Sign.AutoSize = true;
            this.chEdu_Sign.Enabled = false;
            this.chEdu_Sign.Location = new System.Drawing.Point(10, 178);
            this.chEdu_Sign.Name = "chEdu_Sign";
            this.chEdu_Sign.Size = new System.Drawing.Size(15, 14);
            this.chEdu_Sign.TabIndex = 2;
            this.chEdu_Sign.UseVisualStyleBackColor = true;
            this.chEdu_Sign.Visible = false;
            // 
            // tbName3
            // 
            this.tbName3.BackColor = System.Drawing.Color.White;
            this.tbName3.Enabled = false;
            this.tbName3.Location = new System.Drawing.Point(199, 140);
            this.tbName3.Multiline = true;
            this.tbName3.Name = "tbName3";
            this.tbName3.Size = new System.Drawing.Size(169, 21);
            this.tbName3.TabIndex = 1;
            this.tbName3.Visible = false;
            // 
            // lbName3
            // 
            this.lbName3.AutoSize = true;
            this.lbName3.Location = new System.Drawing.Point(197, 122);
            this.lbName3.Name = "lbName3";
            this.lbName3.Size = new System.Drawing.Size(113, 15);
            this.lbName3.TabIndex = 1;
            this.lbName3.Text = "Шаблон номера";
            this.lbName3.Visible = false;
            // 
            // tbName2
            // 
            this.tbName2.BackColor = System.Drawing.Color.White;
            this.tbName2.Enabled = false;
            this.tbName2.Location = new System.Drawing.Point(10, 140);
            this.tbName2.Multiline = true;
            this.tbName2.Name = "tbName2";
            this.tbName2.Size = new System.Drawing.Size(100, 21);
            this.tbName2.TabIndex = 1;
            // 
            // lbName2
            // 
            this.lbName2.AutoSize = true;
            this.lbName2.Location = new System.Drawing.Point(11, 122);
            this.lbName2.Name = "lbName2";
            this.lbName2.Size = new System.Drawing.Size(138, 15);
            this.lbName2.TabIndex = 1;
            this.lbName2.Text = "Код специальности";
            // 
            // lbName1
            // 
            this.lbName1.AutoSize = true;
            this.lbName1.Location = new System.Drawing.Point(11, 40);
            this.lbName1.Name = "lbName1";
            this.lbName1.Size = new System.Drawing.Size(213, 15);
            this.lbName1.TabIndex = 1;
            this.lbName1.Text = "Наименование специальности";
            // 
            // tbName1
            // 
            this.tbName1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName1.BackColor = System.Drawing.Color.White;
            this.tbName1.Enabled = false;
            this.tbName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbName1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbName1.Location = new System.Drawing.Point(10, 58);
            this.tbName1.Multiline = true;
            this.tbName1.Name = "tbName1";
            this.tbName1.Size = new System.Drawing.Size(358, 49);
            this.tbName1.TabIndex = 0;
            // 
            // HandBookSpec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 448);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panel1);
            this.Name = "HandBookSpec";
            this.Text = "HandBookSpec";
            this.panel1.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbNote.ResumeLayout(false);
            this.gbNote.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button btSave;
        public System.Windows.Forms.Button btExit;
        public System.Windows.Forms.Button btDelete;
        public System.Windows.Forms.Button btEdit;
        public System.Windows.Forms.Button btAdd;
        public System.Windows.Forms.SplitContainer splitContainer;
        public System.Windows.Forms.DataGridView dgView;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label lbName2;
        public System.Windows.Forms.Label lbName1;
        public System.Windows.Forms.TextBox tbName1;
        public System.Windows.Forms.Label lbEdu_Sign;
        private System.Windows.Forms.CheckBox chEdu_Sign;
        private System.Windows.Forms.TextBox tbName2;
        private System.Windows.Forms.GroupBox gbNote;
        private System.Windows.Forms.TextBox tbName3;
        public System.Windows.Forms.Label lbName3;
        public System.Windows.Forms.Label label1;
    }
}