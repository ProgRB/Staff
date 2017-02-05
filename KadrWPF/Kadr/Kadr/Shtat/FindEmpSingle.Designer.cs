namespace Kadr.Shtat
{
    partial class FindEmpSingle
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.emp_last_name = new Elegant.Ui.TextBox();
            this.emp_first_name = new Elegant.Ui.TextBox();
            this.emp_middle_name = new Elegant.Ui.TextBox();
            this.Per_num = new System.Windows.Forms.MaskedTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btFind = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grid_resultFindEmpShtat = new System.Windows.Forms.DataGridView();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.clPerNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clSignComb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clFIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clPosName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_resultFindEmpShtat)).BeginInit();
            this.SuspendLayout();
            // 
            // emp_last_name
            // 
            this.emp_last_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emp_last_name.BackColor = System.Drawing.Color.White;
            this.emp_last_name.Id = "8d9241a2-468c-47fc-b62a-ec4e20f50af8";
            this.emp_last_name.LabelText = "";
            this.emp_last_name.Location = new System.Drawing.Point(180, 40);
            this.emp_last_name.Name = "emp_last_name";
            this.emp_last_name.Size = new System.Drawing.Size(394, 24);
            this.emp_last_name.TabIndex = 3;
            this.emp_last_name.TextEditorWidth = 327;
            this.emp_last_name.UseVisualThemeForBackground = false;
            this.emp_last_name.UseVisualThemeForForeground = false;
            this.emp_last_name.KeyUp += new System.Windows.Forms.KeyEventHandler(this.emp_last_name_KeyUp);
            // 
            // emp_first_name
            // 
            this.emp_first_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emp_first_name.BackColor = System.Drawing.Color.White;
            this.emp_first_name.Id = "cbbf74c5-85a0-4eea-8f48-bf2581364ed3";
            this.emp_first_name.LabelText = "";
            this.emp_first_name.Location = new System.Drawing.Point(180, 65);
            this.emp_first_name.Name = "emp_first_name";
            this.emp_first_name.Size = new System.Drawing.Size(394, 24);
            this.emp_first_name.TabIndex = 4;
            this.emp_first_name.TextEditorWidth = 327;
            this.emp_first_name.UseVisualThemeForBackground = false;
            this.emp_first_name.UseVisualThemeForForeground = false;
            this.emp_first_name.KeyUp += new System.Windows.Forms.KeyEventHandler(this.emp_last_name_KeyUp);
            // 
            // emp_middle_name
            // 
            this.emp_middle_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emp_middle_name.BackColor = System.Drawing.Color.White;
            this.emp_middle_name.Id = "c065d9cb-10e1-4b0c-94f8-bc39178fbca6";
            this.emp_middle_name.LabelText = "";
            this.emp_middle_name.Location = new System.Drawing.Point(180, 90);
            this.emp_middle_name.Name = "emp_middle_name";
            this.emp_middle_name.Size = new System.Drawing.Size(394, 24);
            this.emp_middle_name.TabIndex = 5;
            this.emp_middle_name.TextEditorWidth = 327;
            this.emp_middle_name.UseVisualThemeForBackground = false;
            this.emp_middle_name.UseVisualThemeForForeground = false;
            this.emp_middle_name.KeyUp += new System.Windows.Forms.KeyEventHandler(this.emp_last_name_KeyUp);
            // 
            // Per_num
            // 
            this.Per_num.Location = new System.Drawing.Point(180, 17);
            this.Per_num.Mask = "00000";
            this.Per_num.Name = "Per_num";
            this.Per_num.Size = new System.Drawing.Size(63, 22);
            this.Per_num.TabIndex = 2;
            this.Per_num.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.Per_num.KeyUp += new System.Windows.Forms.KeyEventHandler(this.emp_last_name_KeyUp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.btFind);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.btCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btOk);
            this.splitContainer1.Size = new System.Drawing.Size(621, 392);
            this.splitContainer1.SplitterDistance = 152;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 12;
            this.splitContainer1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.emp_last_name);
            this.groupBox1.Controls.Add(this.emp_first_name);
            this.groupBox1.Controls.Add(this.emp_middle_name);
            this.groupBox1.Controls.Add(this.Per_num);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(608, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры фильтра:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Отчество";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Имя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Фамилия";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Табельный номер";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btFind
            // 
            this.btFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFind.ForeColor = System.Drawing.Color.Blue;
            this.btFind.Location = new System.Drawing.Point(517, 123);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(87, 24);
            this.btFind.TabIndex = 6;
            this.btFind.Text = "Поиск";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.grid_resultFindEmpShtat);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(5, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(609, 197);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Результаты:";
            // 
            // grid_resultFindEmpShtat
            // 
            this.grid_resultFindEmpShtat.AllowUserToAddRows = false;
            this.grid_resultFindEmpShtat.AllowUserToDeleteRows = false;
            this.grid_resultFindEmpShtat.AllowUserToResizeRows = false;
            this.grid_resultFindEmpShtat.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_resultFindEmpShtat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_resultFindEmpShtat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clPerNum,
            this.clSignComb,
            this.clFIO,
            this.clPosName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_resultFindEmpShtat.DefaultCellStyle = dataGridViewCellStyle2;
            this.grid_resultFindEmpShtat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_resultFindEmpShtat.Location = new System.Drawing.Point(3, 18);
            this.grid_resultFindEmpShtat.MultiSelect = false;
            this.grid_resultFindEmpShtat.Name = "grid_resultFindEmpShtat";
            this.grid_resultFindEmpShtat.ReadOnly = true;
            this.grid_resultFindEmpShtat.RowHeadersWidth = 20;
            this.grid_resultFindEmpShtat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_resultFindEmpShtat.Size = new System.Drawing.Size(603, 176);
            this.grid_resultFindEmpShtat.TabIndex = 7;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(517, 203);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 23);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Location = new System.Drawing.Point(420, 203);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(91, 23);
            this.btOk.TabIndex = 8;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // clPerNum
            // 
            this.clPerNum.DataPropertyName = "PER_NUM";
            this.clPerNum.HeaderText = "Таб.№";
            this.clPerNum.Name = "clPerNum";
            this.clPerNum.ReadOnly = true;
            this.clPerNum.Width = 60;
            // 
            // clSignComb
            // 
            this.clSignComb.DataPropertyName = "SIGN_COMB";
            this.clSignComb.HeaderText = "Совм.";
            this.clSignComb.Name = "clSignComb";
            this.clSignComb.ReadOnly = true;
            this.clSignComb.Width = 60;
            // 
            // clFIO
            // 
            this.clFIO.DataPropertyName = "FIO";
            this.clFIO.HeaderText = "ФИО";
            this.clFIO.Name = "clFIO";
            this.clFIO.ReadOnly = true;
            this.clFIO.Width = 250;
            // 
            // clPosName
            // 
            this.clPosName.DataPropertyName = "POS_NAME";
            this.clPosName.HeaderText = "Должность";
            this.clPosName.Name = "clPosName";
            this.clPosName.ReadOnly = true;
            this.clPosName.Width = 250;
            // 
            // FindEmpSingle
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(621, 392);
            this.Controls.Add(this.splitContainer1);
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindEmpSingle";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск сотрудника";
            this.TopMost = true;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_resultFindEmpShtat)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.TextBox emp_first_name;
        private Elegant.Ui.TextBox emp_middle_name;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.DataGridView grid_resultFindEmpShtat;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Elegant.Ui.TextBox emp_last_name;
        private System.Windows.Forms.MaskedTextBox Per_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn clPerNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn clSignComb;
        private System.Windows.Forms.DataGridViewTextBoxColumn clFIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn clPosName;
    }
}