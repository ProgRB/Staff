namespace ARM_PROP
{
    partial class EditPermit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditPermit));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.deDate_Start_Permit = new LibraryKadr.DateEditor();
            this.deDate_End_Permit = new LibraryKadr.DateEditor();
            this.cbPermit_Name = new System.Windows.Forms.ComboBox();
            this.deDate_Doc_Permit = new LibraryKadr.DateEditor();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbNum_Doc_Permit = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.dgEmpPermit = new System.Windows.Forms.DataGridView();
            this.tsButton = new System.Windows.Forms.ToolStrip();
            this.tsbAddPermit = new System.Windows.Forms.ToolStripButton();
            this.tsbEditPermit = new System.Windows.Forms.ToolStripButton();
            this.tsbDeletePermit = new System.Windows.Forms.ToolStripButton();
            this.tsbSavePermit = new System.Windows.Forms.ToolStripButton();
            this.tsbRefreshPermit = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tschShowAllPermit = new LibraryKadr.ToolStripCheckBox();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbEmp_middle_name = new System.Windows.Forms.TextBox();
            this.tbEmp_first_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPos_name = new System.Windows.Forms.TextBox();
            this.tbEmp_last_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.tbPer_num = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpPermit)).BeginInit();
            this.tsButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 462);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 37);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(612, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.tsButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(704, 307);
            this.panel2.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.deDate_Start_Permit);
            this.groupBox2.Controls.Add(this.deDate_End_Permit);
            this.groupBox2.Controls.Add(this.cbPermit_Name);
            this.groupBox2.Controls.Add(this.deDate_Doc_Permit);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.tbNum_Doc_Permit);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.dgEmpPermit);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(704, 282);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Данные по разрешениям работника";
            // 
            // deDate_Start_Permit
            // 
            this.deDate_Start_Permit.AutoSize = true;
            this.deDate_Start_Permit.BackColor = System.Drawing.Color.White;
            this.deDate_Start_Permit.Date = null;
            this.deDate_Start_Permit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Start_Permit.Location = new System.Drawing.Point(194, 255);
            this.deDate_Start_Permit.Name = "deDate_Start_Permit";
            this.deDate_Start_Permit.ReadOnly = false;
            this.deDate_Start_Permit.Size = new System.Drawing.Size(77, 21);
            this.deDate_Start_Permit.TabIndex = 40;
            this.deDate_Start_Permit.TextDate = null;
            // 
            // deDate_End_Permit
            // 
            this.deDate_End_Permit.AutoSize = true;
            this.deDate_End_Permit.BackColor = System.Drawing.Color.White;
            this.deDate_End_Permit.Date = null;
            this.deDate_End_Permit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_End_Permit.Location = new System.Drawing.Point(610, 255);
            this.deDate_End_Permit.Name = "deDate_End_Permit";
            this.deDate_End_Permit.ReadOnly = false;
            this.deDate_End_Permit.Size = new System.Drawing.Size(77, 21);
            this.deDate_End_Permit.TabIndex = 41;
            this.deDate_End_Permit.TextDate = null;
            // 
            // cbPermit_Name
            // 
            this.cbPermit_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPermit_Name.BackColor = System.Drawing.Color.White;
            this.cbPermit_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPermit_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPermit_Name.FormattingEnabled = true;
            this.cbPermit_Name.Location = new System.Drawing.Point(194, 225);
            this.cbPermit_Name.Name = "cbPermit_Name";
            this.cbPermit_Name.Size = new System.Drawing.Size(493, 23);
            this.cbPermit_Name.TabIndex = 39;
            // 
            // deDate_Doc_Permit
            // 
            this.deDate_Doc_Permit.AutoSize = true;
            this.deDate_Doc_Permit.BackColor = System.Drawing.Color.White;
            this.deDate_Doc_Permit.Date = null;
            this.deDate_Doc_Permit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Doc_Permit.Location = new System.Drawing.Point(610, 197);
            this.deDate_Doc_Permit.Name = "deDate_Doc_Permit";
            this.deDate_Doc_Permit.ReadOnly = false;
            this.deDate_Doc_Permit.Size = new System.Drawing.Size(77, 21);
            this.deDate_Doc_Permit.TabIndex = 38;
            this.deDate_Doc_Permit.TextDate = null;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(436, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(173, 15);
            this.label9.TabIndex = 36;
            this.label9.Text = "Дата служебной записки";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(11, 255);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(177, 15);
            this.label10.TabIndex = 36;
            this.label10.Text = "Дата начала разрешения";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(404, 255);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(200, 15);
            this.label11.TabIndex = 36;
            this.label11.Text = "Дата окончания разрешения";
            // 
            // tbNum_Doc_Permit
            // 
            this.tbNum_Doc_Permit.BackColor = System.Drawing.Color.White;
            this.tbNum_Doc_Permit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Doc_Permit.Location = new System.Drawing.Point(194, 197);
            this.tbNum_Doc_Permit.Name = "tbNum_Doc_Permit";
            this.tbNum_Doc_Permit.Size = new System.Drawing.Size(77, 21);
            this.tbNum_Doc_Permit.TabIndex = 37;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(11, 220);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 33);
            this.label12.TabIndex = 35;
            this.label12.Text = "Наименование разрешения";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(11, 200);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(152, 15);
            this.label16.TabIndex = 33;
            this.label16.Text = "№ служебной записки";
            // 
            // dgEmpPermit
            // 
            this.dgEmpPermit.AllowUserToAddRows = false;
            this.dgEmpPermit.AllowUserToDeleteRows = false;
            this.dgEmpPermit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgEmpPermit.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgEmpPermit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmpPermit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgEmpPermit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmpPermit.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgEmpPermit.Location = new System.Drawing.Point(3, 17);
            this.dgEmpPermit.Name = "dgEmpPermit";
            this.dgEmpPermit.ReadOnly = true;
            this.dgEmpPermit.RowHeadersWidth = 24;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgEmpPermit.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgEmpPermit.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgEmpPermit.Size = new System.Drawing.Size(698, 173);
            this.dgEmpPermit.TabIndex = 6;
            this.dgEmpPermit.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgEmpPermit_CellFormatting);
            this.dgEmpPermit.DoubleClick += new System.EventHandler(this.tsbEditPermit_Click);
            // 
            // tsButton
            // 
            this.tsButton.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddPermit,
            this.tsbEditPermit,
            this.tsbDeletePermit,
            this.tsbSavePermit,
            this.tsbRefreshPermit,
            this.toolStripLabel1,
            this.tschShowAllPermit});
            this.tsButton.Location = new System.Drawing.Point(0, 0);
            this.tsButton.Name = "tsButton";
            this.tsButton.Size = new System.Drawing.Size(704, 25);
            this.tsButton.TabIndex = 5;
            this.tsButton.Text = "toolStrip1";
            // 
            // tsbAddPermit
            // 
            this.tsbAddPermit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddPermit.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddPermit.Image")));
            this.tsbAddPermit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddPermit.Name = "tsbAddPermit";
            this.tsbAddPermit.Size = new System.Drawing.Size(23, 22);
            this.tsbAddPermit.Text = "Добавить разрешение";
            this.tsbAddPermit.Click += new System.EventHandler(this.tsbAddPermit_Click);
            // 
            // tsbEditPermit
            // 
            this.tsbEditPermit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditPermit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditPermit.Image")));
            this.tsbEditPermit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditPermit.Name = "tsbEditPermit";
            this.tsbEditPermit.Size = new System.Drawing.Size(23, 22);
            this.tsbEditPermit.Text = "Редактировать разрешение";
            this.tsbEditPermit.Click += new System.EventHandler(this.tsbEditPermit_Click);
            // 
            // tsbDeletePermit
            // 
            this.tsbDeletePermit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeletePermit.Image = ((System.Drawing.Image)(resources.GetObject("tsbDeletePermit.Image")));
            this.tsbDeletePermit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeletePermit.Name = "tsbDeletePermit";
            this.tsbDeletePermit.Size = new System.Drawing.Size(23, 22);
            this.tsbDeletePermit.Text = "Удалить разрешение";
            this.tsbDeletePermit.Click += new System.EventHandler(this.tsbDeletePermit_Click);
            // 
            // tsbSavePermit
            // 
            this.tsbSavePermit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSavePermit.Image = ((System.Drawing.Image)(resources.GetObject("tsbSavePermit.Image")));
            this.tsbSavePermit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSavePermit.Name = "tsbSavePermit";
            this.tsbSavePermit.Size = new System.Drawing.Size(23, 22);
            this.tsbSavePermit.Text = "Сохранить разрешение";
            this.tsbSavePermit.Click += new System.EventHandler(this.tsbSavePermit_Click);
            // 
            // tsbRefreshPermit
            // 
            this.tsbRefreshPermit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefreshPermit.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefreshPermit.Image")));
            this.tsbRefreshPermit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefreshPermit.Name = "tsbRefreshPermit";
            this.tsbRefreshPermit.Size = new System.Drawing.Size(23, 22);
            this.tsbRefreshPermit.Text = "Отменить изменения";
            this.tsbRefreshPermit.Click += new System.EventHandler(this.tsbRefreshPermit_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "            ";
            // 
            // tschShowAllPermit
            // 
            this.tschShowAllPermit.BackColor = System.Drawing.Color.Transparent;
            this.tschShowAllPermit.Checked = false;
            this.tschShowAllPermit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tschShowAllPermit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(66)))), ((int)(((byte)(140)))));
            this.tschShowAllPermit.Name = "tschShowAllPermit";
            this.tschShowAllPermit.Size = new System.Drawing.Size(353, 22);
            this.tschShowAllPermit.Text = "Отображать все разрешения (текущие и архивные)";
            // 
            // pbPhoto
            // 
            this.pbPhoto.Enabled = false;
            this.pbPhoto.Location = new System.Drawing.Point(13, 12);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(134, 137);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPhoto.TabIndex = 24;
            this.pbPhoto.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pbPhoto);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(161, 155);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbEmp_middle_name);
            this.groupBox1.Controls.Add(this.tbEmp_first_name);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbPos_name);
            this.groupBox1.Controls.Add(this.tbEmp_last_name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbCode_Subdiv);
            this.groupBox1.Controls.Add(this.tbPer_num);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(161, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 155);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // tbEmp_middle_name
            // 
            this.tbEmp_middle_name.BackColor = System.Drawing.Color.White;
            this.tbEmp_middle_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_middle_name.Location = new System.Drawing.Point(319, 70);
            this.tbEmp_middle_name.Name = "tbEmp_middle_name";
            this.tbEmp_middle_name.ReadOnly = true;
            this.tbEmp_middle_name.Size = new System.Drawing.Size(208, 21);
            this.tbEmp_middle_name.TabIndex = 4;
            // 
            // tbEmp_first_name
            // 
            this.tbEmp_first_name.BackColor = System.Drawing.Color.White;
            this.tbEmp_first_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_first_name.Location = new System.Drawing.Point(319, 43);
            this.tbEmp_first_name.Name = "tbEmp_first_name";
            this.tbEmp_first_name.ReadOnly = true;
            this.tbEmp_first_name.Size = new System.Drawing.Size(208, 21);
            this.tbEmp_first_name.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(13, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "Должность";
            // 
            // tbPos_name
            // 
            this.tbPos_name.BackColor = System.Drawing.Color.White;
            this.tbPos_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPos_name.Location = new System.Drawing.Point(16, 97);
            this.tbPos_name.Multiline = true;
            this.tbPos_name.Name = "tbPos_name";
            this.tbPos_name.ReadOnly = true;
            this.tbPos_name.Size = new System.Drawing.Size(511, 51);
            this.tbPos_name.TabIndex = 5;
            // 
            // tbEmp_last_name
            // 
            this.tbEmp_last_name.BackColor = System.Drawing.Color.White;
            this.tbEmp_last_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_last_name.Location = new System.Drawing.Point(319, 16);
            this.tbEmp_last_name.Name = "tbEmp_last_name";
            this.tbEmp_last_name.ReadOnly = true;
            this.tbEmp_last_name.Size = new System.Drawing.Size(208, 21);
            this.tbEmp_last_name.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(13, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Подразделение";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.BackColor = System.Drawing.Color.White;
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(127, 16);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.ReadOnly = true;
            this.tbCode_Subdiv.Size = new System.Drawing.Size(77, 21);
            this.tbCode_Subdiv.TabIndex = 0;
            // 
            // tbPer_num
            // 
            this.tbPer_num.BackColor = System.Drawing.Color.White;
            this.tbPer_num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_num.Location = new System.Drawing.Point(127, 46);
            this.tbPer_num.Name = "tbPer_num";
            this.tbPer_num.ReadOnly = true;
            this.tbPer_num.Size = new System.Drawing.Size(77, 21);
            this.tbPer_num.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(230, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 23;
            this.label5.Text = "Отчество";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(230, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Имя";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(230, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "Фамилия";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(13, 49);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 15);
            this.label17.TabIndex = 23;
            this.label17.Text = "Табельный №";
            // 
            // EditPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 499);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditPermit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование данных работника";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpPermit)).EndInit();
            this.tsButton.ResumeLayout(false);
            this.tsButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        public System.Windows.Forms.DataGridView dgEmpPermit;
        private System.Windows.Forms.ToolStrip tsButton;
        private System.Windows.Forms.ToolStripButton tsbAddPermit;
        private System.Windows.Forms.ToolStripButton tsbEditPermit;
        private System.Windows.Forms.ToolStripButton tsbDeletePermit;
        private System.Windows.Forms.ToolStripButton tsbSavePermit;
        private System.Windows.Forms.ToolStripButton tsbRefreshPermit;
        private LibraryKadr.DateEditor deDate_Start_Permit;
        private LibraryKadr.DateEditor deDate_End_Permit;
        private System.Windows.Forms.ComboBox cbPermit_Name;
        private LibraryKadr.DateEditor deDate_Doc_Permit;
        private System.Windows.Forms.TextBox tbNum_Doc_Permit;
        private System.Windows.Forms.PictureBox pbPhoto;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbEmp_middle_name;
        private System.Windows.Forms.TextBox tbEmp_first_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPos_name;
        private System.Windows.Forms.TextBox tbEmp_last_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.TextBox tbPer_num;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private LibraryKadr.ToolStripCheckBox tschShowAllPermit;
    }
}