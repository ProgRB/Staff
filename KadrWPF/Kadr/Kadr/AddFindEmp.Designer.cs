namespace Kadr
{
    partial class AddFindEmp
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btEmp_By_Resume = new System.Windows.Forms.Button();
            this.btNewEmp = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.btPer_Num = new System.Windows.Forms.Button();
            this.btFindEmp = new System.Windows.Forms.Button();
            this.mbBirth_Date = new System.Windows.Forms.MaskedTextBox();
            this.mbInn = new System.Windows.Forms.MaskedTextBox();
            this.mbInsurance_Num = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbEmp_Middle_Name = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbEmp_First_Name = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbEmp_Last_Name = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btExit = new System.Windows.Forms.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.gbOldEmp = new System.Windows.Forms.GroupBox();
            this.dgViewOldEmp = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbEmpResume = new System.Windows.Forms.GroupBox();
            this.dgViewEmpResume = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.gbOldEmp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewOldEmp)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbEmpResume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewEmpResume)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btEmp_By_Resume);
            this.groupBox1.Controls.Add(this.btNewEmp);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.btPer_Num);
            this.groupBox1.Controls.Add(this.btFindEmp);
            this.groupBox1.Controls.Add(this.mbBirth_Date);
            this.groupBox1.Controls.Add(this.mbInn);
            this.groupBox1.Controls.Add(this.mbInsurance_Num);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cbSubdiv_Name);
            this.groupBox1.Controls.Add(this.tbCode_Subdiv);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbEmp_Middle_Name);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbEmp_First_Name);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbEmp_Last_Name);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(791, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btEmp_By_Resume
            // 
            this.btEmp_By_Resume.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEmp_By_Resume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btEmp_By_Resume.Location = new System.Drawing.Point(617, 115);
            this.btEmp_By_Resume.Name = "btEmp_By_Resume";
            this.btEmp_By_Resume.Size = new System.Drawing.Size(151, 23);
            this.btEmp_By_Resume.TabIndex = 43;
            this.btEmp_By_Resume.Text = "Принять по Резюме";
            this.btEmp_By_Resume.UseVisualStyleBackColor = true;
            this.btEmp_By_Resume.Click += new System.EventHandler(this.btEmp_By_Resume_Click);
            // 
            // btNewEmp
            // 
            this.btNewEmp.CausesValidation = false;
            this.btNewEmp.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btNewEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btNewEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btNewEmp.Location = new System.Drawing.Point(617, 51);
            this.btNewEmp.Name = "btNewEmp";
            this.btNewEmp.Size = new System.Drawing.Size(151, 23);
            this.btNewEmp.TabIndex = 10;
            this.btNewEmp.Text = "Новый работник";
            this.btNewEmp.UseVisualStyleBackColor = true;
            this.btNewEmp.Click += new System.EventHandler(this.btNewEmp_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Location = new System.Drawing.Point(368, 23);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(111, 15);
            this.label19.TabIndex = 42;
            this.label19.Text = "Дата рождения";
            // 
            // btPer_Num
            // 
            this.btPer_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btPer_Num.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btPer_Num.Location = new System.Drawing.Point(617, 83);
            this.btPer_Num.Name = "btPer_Num";
            this.btPer_Num.Size = new System.Drawing.Size(151, 23);
            this.btPer_Num.TabIndex = 9;
            this.btPer_Num.Text = "Принять работника";
            this.btPer_Num.UseVisualStyleBackColor = true;
            this.btPer_Num.Click += new System.EventHandler(this.btPer_Num_Click);
            // 
            // btFindEmp
            // 
            this.btFindEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFindEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btFindEmp.Location = new System.Drawing.Point(617, 19);
            this.btFindEmp.Name = "btFindEmp";
            this.btFindEmp.Size = new System.Drawing.Size(151, 23);
            this.btFindEmp.TabIndex = 8;
            this.btFindEmp.Text = "Поиск данных";
            this.btFindEmp.UseVisualStyleBackColor = true;
            this.btFindEmp.Click += new System.EventHandler(this.btFindEmp_Click);
            // 
            // mbBirth_Date
            // 
            this.mbBirth_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbBirth_Date.Location = new System.Drawing.Point(494, 20);
            this.mbBirth_Date.Mask = "00/00/0000";
            this.mbBirth_Date.Name = "mbBirth_Date";
            this.mbBirth_Date.Size = new System.Drawing.Size(101, 21);
            this.mbBirth_Date.TabIndex = 3;
            this.mbBirth_Date.ValidatingType = typeof(System.DateTime);
            // 
            // mbInn
            // 
            this.mbInn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbInn.Location = new System.Drawing.Point(494, 51);
            this.mbInn.Mask = "999999999999";
            this.mbInn.Name = "mbInn";
            this.mbInn.Size = new System.Drawing.Size(101, 21);
            this.mbInn.TabIndex = 4;
            this.mbInn.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // mbInsurance_Num
            // 
            this.mbInsurance_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbInsurance_Num.Location = new System.Drawing.Point(494, 83);
            this.mbInsurance_Num.Mask = "999-999-999 99";
            this.mbInsurance_Num.Name = "mbInsurance_Num";
            this.mbInsurance_Num.Size = new System.Drawing.Size(101, 21);
            this.mbInsurance_Num.TabIndex = 5;
            this.mbInsurance_Num.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(368, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 37);
            this.label4.TabIndex = 41;
            this.label4.Text = "Страховое свидетельство";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(368, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 15);
            this.label13.TabIndex = 40;
            this.label13.Text = "ИНН";
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(207, 115);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(388, 23);
            this.cbSubdiv_Name.TabIndex = 7;
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(148, 115);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(53, 21);
            this.tbCode_Subdiv.TabIndex = 6;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(22, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 15);
            this.label5.TabIndex = 36;
            this.label5.Text = "Подразделение";
            // 
            // tbEmp_Middle_Name
            // 
            this.tbEmp_Middle_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEmp_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Middle_Name.Location = new System.Drawing.Point(148, 83);
            this.tbEmp_Middle_Name.Name = "tbEmp_Middle_Name";
            this.tbEmp_Middle_Name.Size = new System.Drawing.Size(194, 21);
            this.tbEmp_Middle_Name.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(22, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 15);
            this.label9.TabIndex = 39;
            this.label9.Text = "Отчество";
            // 
            // tbEmp_First_Name
            // 
            this.tbEmp_First_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEmp_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_First_Name.Location = new System.Drawing.Point(148, 51);
            this.tbEmp_First_Name.Name = "tbEmp_First_Name";
            this.tbEmp_First_Name.Size = new System.Drawing.Size(194, 21);
            this.tbEmp_First_Name.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(22, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 15);
            this.label8.TabIndex = 37;
            this.label8.Text = "Имя";
            // 
            // tbEmp_Last_Name
            // 
            this.tbEmp_Last_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEmp_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Last_Name.Location = new System.Drawing.Point(148, 20);
            this.tbEmp_Last_Name.Name = "tbEmp_Last_Name";
            this.tbEmp_Last_Name.Size = new System.Drawing.Size(194, 21);
            this.tbEmp_Last_Name.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(22, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 15);
            this.label6.TabIndex = 38;
            this.label6.Text = "Фамилия";
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(696, 7);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(72, 23);
            this.btExit.TabIndex = 9;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // gbOldEmp
            // 
            this.gbOldEmp.Controls.Add(this.dgViewOldEmp);
            this.gbOldEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOldEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbOldEmp.Location = new System.Drawing.Point(0, 152);
            this.gbOldEmp.Name = "gbOldEmp";
            this.gbOldEmp.Size = new System.Drawing.Size(791, 156);
            this.gbOldEmp.TabIndex = 4;
            this.gbOldEmp.TabStop = false;
            this.gbOldEmp.Text = "Список бывших сотрудников";
            // 
            // dgViewOldEmp
            // 
            this.dgViewOldEmp.AllowUserToAddRows = false;
            this.dgViewOldEmp.AllowUserToDeleteRows = false;
            this.dgViewOldEmp.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgViewOldEmp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewOldEmp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewOldEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewOldEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewOldEmp.Location = new System.Drawing.Point(3, 18);
            this.dgViewOldEmp.Name = "dgViewOldEmp";
            this.dgViewOldEmp.ReadOnly = true;
            this.dgViewOldEmp.RowHeadersWidth = 24;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewOldEmp.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgViewOldEmp.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewOldEmp.Size = new System.Drawing.Size(785, 135);
            this.dgViewOldEmp.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 463);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 36);
            this.panel1.TabIndex = 5;
            // 
            // gbEmpResume
            // 
            this.gbEmpResume.Controls.Add(this.dgViewEmpResume);
            this.gbEmpResume.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbEmpResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbEmpResume.Location = new System.Drawing.Point(0, 308);
            this.gbEmpResume.Name = "gbEmpResume";
            this.gbEmpResume.Size = new System.Drawing.Size(791, 155);
            this.gbEmpResume.TabIndex = 6;
            this.gbEmpResume.TabStop = false;
            this.gbEmpResume.Text = "Список резюме";
            // 
            // dgViewEmpResume
            // 
            this.dgViewEmpResume.AllowUserToAddRows = false;
            this.dgViewEmpResume.AllowUserToDeleteRows = false;
            this.dgViewEmpResume.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgViewEmpResume.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewEmpResume.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgViewEmpResume.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewEmpResume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewEmpResume.Location = new System.Drawing.Point(3, 18);
            this.dgViewEmpResume.Name = "dgViewEmpResume";
            this.dgViewEmpResume.ReadOnly = true;
            this.dgViewEmpResume.RowHeadersWidth = 24;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewEmpResume.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgViewEmpResume.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewEmpResume.Size = new System.Drawing.Size(785, 134);
            this.dgViewEmpResume.TabIndex = 2;
            // 
            // AddFindEmp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 499);
            this.Controls.Add(this.gbOldEmp);
            this.Controls.Add(this.gbEmpResume);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AddFindEmp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск бывшего работника в базе данных";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbOldEmp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewOldEmp)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gbEmpResume.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewEmpResume)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox mbInn;
        private System.Windows.Forms.MaskedTextBox mbInsurance_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbEmp_Middle_Name;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbEmp_First_Name;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbEmp_Last_Name;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.MaskedTextBox mbBirth_Date;
        private System.Windows.Forms.Button btNewEmp;
        private System.Windows.Forms.Button btFindEmp;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Button btPer_Num;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox gbOldEmp;
        public System.Windows.Forms.DataGridView dgViewOldEmp;
        private System.Windows.Forms.Button btEmp_By_Resume;
        private System.Windows.Forms.GroupBox gbEmpResume;
        public System.Windows.Forms.DataGridView dgViewEmpResume;
        private System.Windows.Forms.Panel panel1;
    }
}