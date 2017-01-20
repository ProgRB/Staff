namespace Kadr
{
    partial class View_All_Transfer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btFindEmp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tB_per_num = new System.Windows.Forms.TextBox();
            this.tB_emp_middle_name = new System.Windows.Forms.TextBox();
            this.tB_emp_first_name = new System.Windows.Forms.TextBox();
            this.tB_emp_last_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.dgView_Transfer = new System.Windows.Forms.DataGridView();
            this.CODE_SUBDIV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CUR_WORK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PER_NUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COMB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE_POS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE_TRANSFER = new LibraryKadr.MDataGridViewCalendarColumn();
            this.END_TRANSFER = new LibraryKadr.MDataGridViewCalendarColumn();
            this.TYPE_TRANSFER_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView_Transfer)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tB_per_num);
            this.groupBox1.Controls.Add(this.tB_emp_middle_name);
            this.groupBox1.Controls.Add(this.tB_emp_first_name);
            this.groupBox1.Controls.Add(this.tB_emp_last_name);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(708, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Данные для поиска";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btFindEmp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 32);
            this.panel1.TabIndex = 9;
            // 
            // btFindEmp
            // 
            this.btFindEmp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFindEmp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btFindEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFindEmp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btFindEmp.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btFindEmp.Location = new System.Drawing.Point(611, 6);
            this.btFindEmp.Name = "btFindEmp";
            this.btFindEmp.Size = new System.Drawing.Size(78, 23);
            this.btFindEmp.TabIndex = 0;
            this.btFindEmp.Text = "Поиск";
            this.btFindEmp.UseVisualStyleBackColor = true;
            this.btFindEmp.Click += new System.EventHandler(this.btFindEmp_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Таб.№";
            // 
            // tB_per_num
            // 
            this.tB_per_num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_per_num.Location = new System.Drawing.Point(70, 23);
            this.tB_per_num.Name = "tB_per_num";
            this.tB_per_num.Size = new System.Drawing.Size(48, 21);
            this.tB_per_num.TabIndex = 0;
            // 
            // tB_emp_middle_name
            // 
            this.tB_emp_middle_name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tB_emp_middle_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_emp_middle_name.Location = new System.Drawing.Point(561, 23);
            this.tB_emp_middle_name.Name = "tB_emp_middle_name";
            this.tB_emp_middle_name.Size = new System.Drawing.Size(131, 21);
            this.tB_emp_middle_name.TabIndex = 3;
            // 
            // tB_emp_first_name
            // 
            this.tB_emp_first_name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tB_emp_first_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_emp_first_name.Location = new System.Drawing.Point(366, 23);
            this.tB_emp_first_name.Name = "tB_emp_first_name";
            this.tB_emp_first_name.Size = new System.Drawing.Size(113, 21);
            this.tB_emp_first_name.TabIndex = 2;
            // 
            // tB_emp_last_name
            // 
            this.tB_emp_last_name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tB_emp_last_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_emp_last_name.Location = new System.Drawing.Point(197, 23);
            this.tB_emp_last_name.Name = "tB_emp_last_name";
            this.tB_emp_last_name.Size = new System.Drawing.Size(124, 21);
            this.tB_emp_last_name.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(125, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Фамилия";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(485, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Отчество";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(327, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Имя";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btExit);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 435);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(708, 39);
            this.panel5.TabIndex = 3;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btExit.Location = new System.Drawing.Point(612, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(80, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // dgView_Transfer
            // 
            this.dgView_Transfer.AllowUserToAddRows = false;
            this.dgView_Transfer.AllowUserToDeleteRows = false;
            this.dgView_Transfer.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView_Transfer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgView_Transfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView_Transfer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CODE_SUBDIV,
            this.CUR_WORK,
            this.PER_NUM,
            this.COMB,
            this.FIO,
            this.CODE_POS,
            this.DATE_TRANSFER,
            this.END_TRANSFER,
            this.TYPE_TRANSFER_NAME,
            this.POS_NAME});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgView_Transfer.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgView_Transfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgView_Transfer.Location = new System.Drawing.Point(0, 86);
            this.dgView_Transfer.Name = "dgView_Transfer";
            this.dgView_Transfer.ReadOnly = true;
            this.dgView_Transfer.RowHeadersWidth = 24;
            this.dgView_Transfer.RowTemplate.ReadOnly = true;
            this.dgView_Transfer.Size = new System.Drawing.Size(708, 349);
            this.dgView_Transfer.TabIndex = 4;
            // 
            // CODE_SUBDIV
            // 
            this.CODE_SUBDIV.DataPropertyName = "CODE_SUBDIV";
            this.CODE_SUBDIV.HeaderText = "Подр.";
            this.CODE_SUBDIV.Name = "CODE_SUBDIV";
            this.CODE_SUBDIV.ReadOnly = true;
            this.CODE_SUBDIV.Width = 50;
            // 
            // CUR_WORK
            // 
            this.CUR_WORK.DataPropertyName = "CUR_WORK";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CUR_WORK.DefaultCellStyle = dataGridViewCellStyle2;
            this.CUR_WORK.HeaderText = "Текущая работа";
            this.CUR_WORK.Name = "CUR_WORK";
            this.CUR_WORK.ReadOnly = true;
            this.CUR_WORK.Width = 65;
            // 
            // PER_NUM
            // 
            this.PER_NUM.DataPropertyName = "PER_NUM";
            this.PER_NUM.HeaderText = "Таб.№";
            this.PER_NUM.Name = "PER_NUM";
            this.PER_NUM.ReadOnly = true;
            this.PER_NUM.Width = 50;
            // 
            // COMB
            // 
            this.COMB.DataPropertyName = "COMB";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.COMB.DefaultCellStyle = dataGridViewCellStyle3;
            this.COMB.HeaderText = "С.";
            this.COMB.Name = "COMB";
            this.COMB.ReadOnly = true;
            this.COMB.Width = 20;
            // 
            // FIO
            // 
            this.FIO.DataPropertyName = "FIO";
            this.FIO.HeaderText = "ФИО";
            this.FIO.Name = "FIO";
            this.FIO.ReadOnly = true;
            this.FIO.Width = 150;
            // 
            // CODE_POS
            // 
            this.CODE_POS.DataPropertyName = "CODE_POS";
            this.CODE_POS.HeaderText = "Шифр проф.";
            this.CODE_POS.Name = "CODE_POS";
            this.CODE_POS.ReadOnly = true;
            this.CODE_POS.Width = 60;
            // 
            // DATE_TRANSFER
            // 
            this.DATE_TRANSFER.DataPropertyName = "DATE_TRANSFER";
            this.DATE_TRANSFER.DateFormat = "dd.MM.yyyy";
            this.DATE_TRANSFER.HeaderText = "Дата перевода";
            this.DATE_TRANSFER.Name = "DATE_TRANSFER";
            this.DATE_TRANSFER.ReadOnly = true;
            this.DATE_TRANSFER.Width = 90;
            // 
            // END_TRANSFER
            // 
            this.END_TRANSFER.DataPropertyName = "END_TRANSFER";
            this.END_TRANSFER.DateFormat = "dd.MM.yyyy";
            this.END_TRANSFER.HeaderText = "Окончание перевода";
            this.END_TRANSFER.Name = "END_TRANSFER";
            this.END_TRANSFER.ReadOnly = true;
            this.END_TRANSFER.Width = 90;
            // 
            // TYPE_TRANSFER_NAME
            // 
            this.TYPE_TRANSFER_NAME.DataPropertyName = "TYPE_TRANSFER_NAME";
            this.TYPE_TRANSFER_NAME.HeaderText = "Тип перевода";
            this.TYPE_TRANSFER_NAME.Name = "TYPE_TRANSFER_NAME";
            this.TYPE_TRANSFER_NAME.ReadOnly = true;
            // 
            // POS_NAME
            // 
            this.POS_NAME.DataPropertyName = "POS_NAME";
            this.POS_NAME.HeaderText = "Наименование должности";
            this.POS_NAME.Name = "POS_NAME";
            this.POS_NAME.ReadOnly = true;
            this.POS_NAME.Width = 300;
            // 
            // View_All_Transfer
            // 
            this.AcceptButton = this.btFindEmp;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(708, 474);
            this.Controls.Add(this.dgView_Transfer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "View_All_Transfer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр переводов по сотруднику";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgView_Transfer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btFindEmp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tB_per_num;
        private System.Windows.Forms.TextBox tB_emp_middle_name;
        private System.Windows.Forms.TextBox tB_emp_first_name;
        private System.Windows.Forms.TextBox tB_emp_last_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.DataGridView dgView_Transfer;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE_SUBDIV;
        private System.Windows.Forms.DataGridViewTextBoxColumn CUR_WORK;
        private System.Windows.Forms.DataGridViewTextBoxColumn PER_NUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn COMB;
        private System.Windows.Forms.DataGridViewTextBoxColumn FIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE_POS;
        private LibraryKadr.MDataGridViewCalendarColumn DATE_TRANSFER;
        private LibraryKadr.MDataGridViewCalendarColumn END_TRANSFER;
        private System.Windows.Forms.DataGridViewTextBoxColumn TYPE_TRANSFER_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn POS_NAME;
    }
}