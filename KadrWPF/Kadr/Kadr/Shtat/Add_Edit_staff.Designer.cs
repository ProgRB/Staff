
using System.Collections.Generic;
using System.Data;
using LibraryKadr;
using Staff;
namespace Kadr.Shtat
{
    partial class Add_Edit_staff
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numberOfPackage = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SetTemp = new System.Windows.Forms.CheckBox();
            this.gbMainData = new System.Windows.Forms.GroupBox();
            this.label_pos_name = new System.Windows.Forms.Label();
            this.code_pos = new System.Windows.Forms.MaskedTextBox();
            this.Degree = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.bt_find_pos = new System.Windows.Forms.Button();
            this.comments_pos = new Elegant.Ui.TextBox();
            this.gbVacant_ = new System.Windows.Forms.GroupBox();
            this.vacant_sign = new Elegant.Ui.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DateEndVacant = new System.Windows.Forms.MaskedTextBox();
            this.gbDateStaffs = new System.Windows.Forms.GroupBox();
            this.Date_Begin = new System.Windows.Forms.MaskedTextBox();
            this.Date_End = new System.Windows.Forms.MaskedTextBox();
            this.gbAccounData = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.harmful_add = new System.Windows.Forms.ComboBox();
            this.classific = new System.Windows.Forms.ComboBox();
            this.tariff_grid = new System.Windows.Forms.ComboBox();
            this.secret_add = new System.Windows.Forms.MaskedTextBox();
            this.order = new System.Windows.Forms.MaskedTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.type_personal = new System.Windows.Forms.ComboBox();
            this.tar_by_schema = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btSaveGroupShtat = new System.Windows.Forms.Button();
            this.btExcludeAndSaveShtat = new System.Windows.Forms.Button();
            this.btSaveShtat = new System.Windows.Forms.Button();
            this.btAddShtat = new System.Windows.Forms.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.screenTip1 = new Elegant.Ui.ScreenTip();
            this.btRemoveMainEmpSht = new System.Windows.Forms.Button();
            this.btEditMainEmp = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel_data = new System.Windows.Forms.Panel();
            this.tpMainEmpShtat = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.MainPer_num = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.MainMiddleName = new System.Windows.Forms.Label();
            this.MainName = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.MainFam = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.MainDate_hire = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.MainDopSoglSht = new System.Windows.Forms.MaskedTextBox();
            this.MainComb_AddSht = new System.Windows.Forms.MaskedTextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.MainSalary1 = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.MainSecret_add = new System.Windows.Forms.Label();
            this.MainHarmf_ad = new System.Windows.Forms.Label();
            this.MainSalary = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MainEmpPhoto = new System.Windows.Forms.PictureBox();
            this.tpReplEmpShtat = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.GridReplEmp = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btNewCombReplEmp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.ReplEmpPhoto = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.Repl_per_num = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.Repl_Middle_name = new System.Windows.Forms.Label();
            this.Repl_name = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.ReplFam = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.Repl_date_hire = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Repl_secret_add = new System.Windows.Forms.TextBox();
            this.Repl_harm_add = new System.Windows.Forms.TextBox();
            this.Repl_comb_add = new System.Windows.Forms.TextBox();
            this.Repl_dop_sogl = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.Repl_salary = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.panel_main_commands = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SS1 = new Kadr.Classes.SubdivSelector();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPackage)).BeginInit();
            this.gbMainData.SuspendLayout();
            this.gbVacant_.SuspendLayout();
            this.gbDateStaffs.SuspendLayout();
            this.gbAccounData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tar_by_schema)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_data.SuspendLayout();
            this.tpMainEmpShtat.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainEmpPhoto)).BeginInit();
            this.tpReplEmpShtat.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridReplEmp)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReplEmpPhoto)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel_main_commands.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Категория";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Должность";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 144);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "Количество единиц";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numberOfPackage
            // 
            this.numberOfPackage.Enabled = false;
            this.numberOfPackage.Location = new System.Drawing.Point(120, 148);
            this.numberOfPackage.Margin = new System.Windows.Forms.Padding(4);
            this.numberOfPackage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfPackage.Name = "numberOfPackage";
            this.screenTip1.GetScreenTip(this.numberOfPackage).Text = "Количество добавляемых однотипных единиц";
            this.numberOfPackage.Size = new System.Drawing.Size(78, 21);
            this.numberOfPackage.TabIndex = 8;
            this.numberOfPackage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(76, 22);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "Ввести с";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(117, 41);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "по";
            // 
            // SetTemp
            // 
            this.SetTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetTemp.AutoSize = true;
            this.SetTemp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SetTemp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SetTemp.Location = new System.Drawing.Point(195, 6);
            this.SetTemp.Name = "SetTemp";
            this.SetTemp.Size = new System.Drawing.Size(162, 19);
            this.SetTemp.TabIndex = 23;
            this.SetTemp.Text = "ВВЕСТИ ВРЕМЕННО";
            this.SetTemp.UseVisualStyleBackColor = true;
            this.SetTemp.Visible = false;
            // 
            // gbMainData
            // 
            this.gbMainData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMainData.BackColor = System.Drawing.SystemColors.Control;
            this.gbMainData.Controls.Add(this.label13);
            this.gbMainData.Controls.Add(this.label_pos_name);
            this.gbMainData.Controls.Add(this.code_pos);
            this.gbMainData.Controls.Add(this.Degree);
            this.gbMainData.Controls.Add(this.SS1);
            this.gbMainData.Controls.Add(this.bt_find_pos);
            this.gbMainData.Controls.Add(this.label2);
            this.gbMainData.Controls.Add(this.label3);
            this.gbMainData.Controls.Add(this.label4);
            this.gbMainData.Controls.Add(this.comments_pos);
            this.gbMainData.Controls.Add(this.numberOfPackage);
            this.gbMainData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbMainData.Location = new System.Drawing.Point(3, 4);
            this.gbMainData.Name = "gbMainData";
            this.gbMainData.Size = new System.Drawing.Size(681, 180);
            this.gbMainData.TabIndex = 100;
            this.gbMainData.TabStop = false;
            this.gbMainData.Text = "Основные данные";
            // 
            // label_pos_name
            // 
            this.label_pos_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_pos_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_pos_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_pos_name.Location = new System.Drawing.Point(217, 69);
            this.label_pos_name.Name = "label_pos_name";
            this.label_pos_name.Size = new System.Drawing.Size(456, 21);
            this.label_pos_name.TabIndex = 25;
            this.label_pos_name.Text = "Профессия не выбрана";
            this.label_pos_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // code_pos
            // 
            this.code_pos.Location = new System.Drawing.Point(120, 69);
            this.code_pos.Mask = "00000";
            this.code_pos.Name = "code_pos";
            this.code_pos.ResetOnSpace = false;
            this.code_pos.Size = new System.Drawing.Size(55, 21);
            this.code_pos.TabIndex = 9;
            this.code_pos.ValidatingType = typeof(System.DateTime);
            this.code_pos.Validating += new System.ComponentModel.CancelEventHandler(this.codePos_Validating);
            // 
            // Degree
            // 
            this.Degree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Degree.FormattingEnabled = true;
            this.Degree.Location = new System.Drawing.Point(120, 43);
            this.Degree.Name = "Degree";
            this.Degree.Size = new System.Drawing.Size(192, 23);
            this.Degree.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(7, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(109, 39);
            this.label13.TabIndex = 0;
            this.label13.Text = "Комментарий к професиии";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bt_find_pos
            // 
            this.bt_find_pos.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_find_pos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bt_find_pos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bt_find_pos.Location = new System.Drawing.Point(180, 69);
            this.bt_find_pos.Name = "bt_find_pos";
            this.bt_find_pos.Size = new System.Drawing.Size(30, 21);
            this.bt_find_pos.TabIndex = 24;
            this.bt_find_pos.Text = "...";
            this.bt_find_pos.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.bt_find_pos.UseVisualStyleBackColor = true;
            this.bt_find_pos.Click += new System.EventHandler(this.bt_find_pos_Click);
            // 
            // comments_pos
            // 
            this.comments_pos.BackColor = System.Drawing.SystemColors.Window;
            this.comments_pos.Id = "1874d5eb-0949-4475-9749-528ac57128fd";
            this.comments_pos.LabelText = "";
            this.comments_pos.Location = new System.Drawing.Point(120, 92);
            this.comments_pos.Multiline = true;
            this.comments_pos.Name = "comments_pos";
            this.comments_pos.Size = new System.Drawing.Size(296, 52);
            this.comments_pos.TabIndex = 7;
            this.comments_pos.TextEditorWidth = 327;
            this.comments_pos.UseVisualThemeForBackground = false;
            // 
            // gbVacant_
            // 
            this.gbVacant_.Controls.Add(this.vacant_sign);
            this.gbVacant_.Controls.Add(this.label6);
            this.gbVacant_.Controls.Add(this.DateEndVacant);
            this.gbVacant_.Location = new System.Drawing.Point(357, 186);
            this.gbVacant_.Name = "gbVacant_";
            this.gbVacant_.Size = new System.Drawing.Size(322, 66);
            this.gbVacant_.TabIndex = 102;
            this.gbVacant_.TabStop = false;
            this.gbVacant_.Text = "Признак вакансии";
            // 
            // vacant_sign
            // 
            this.vacant_sign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.vacant_sign.Id = "0f47420a-25af-405f-a7ee-e6db409f9690";
            this.vacant_sign.Location = new System.Drawing.Point(189, 14);
            this.vacant_sign.Name = "vacant_sign";
            this.vacant_sign.ScreenTip.Caption = "Признак вакансии";
            this.vacant_sign.ScreenTip.Text = "Если флажок установлен, то место вакантно";
            this.vacant_sign.Size = new System.Drawing.Size(86, 17);
            this.vacant_sign.TabIndex = 11;
            this.vacant_sign.Text = "Вакансия";
            this.vacant_sign.UseVisualThemeForBackground = false;
            this.vacant_sign.UseVisualThemeForForeground = false;
            this.vacant_sign.CheckedChanged += new System.EventHandler(this.vacant_sign_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(181, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Дата окончания вакансии";
            // 
            // DateEndVacant
            // 
            this.DateEndVacant.Location = new System.Drawing.Point(189, 34);
            this.DateEndVacant.Mask = "00/00/0000";
            this.DateEndVacant.Name = "DateEndVacant";
            this.DateEndVacant.ResetOnSpace = false;
            this.DateEndVacant.Size = new System.Drawing.Size(88, 21);
            this.DateEndVacant.TabIndex = 12;
            this.DateEndVacant.ValidatingType = typeof(System.DateTime);
            // 
            // gbDateStaffs
            // 
            this.gbDateStaffs.Controls.Add(this.Date_Begin);
            this.gbDateStaffs.Controls.Add(this.Date_End);
            this.gbDateStaffs.Controls.Add(this.label10);
            this.gbDateStaffs.Controls.Add(this.label9);
            this.gbDateStaffs.Location = new System.Drawing.Point(3, 186);
            this.gbDateStaffs.Name = "gbDateStaffs";
            this.gbDateStaffs.Size = new System.Drawing.Size(340, 66);
            this.gbDateStaffs.TabIndex = 101;
            this.gbDateStaffs.TabStop = false;
            this.gbDateStaffs.Text = "Даты штатной единицы:";
            // 
            // Date_Begin
            // 
            this.Date_Begin.Location = new System.Drawing.Point(150, 19);
            this.Date_Begin.Mask = "00/00/0000";
            this.Date_Begin.Name = "Date_Begin";
            this.Date_Begin.ResetOnSpace = false;
            this.Date_Begin.Size = new System.Drawing.Size(99, 21);
            this.Date_Begin.TabIndex = 9;
            this.Date_Begin.ValidatingType = typeof(System.DateTime);
            // 
            // Date_End
            // 
            this.Date_End.Location = new System.Drawing.Point(150, 41);
            this.Date_End.Mask = "00/00/0000";
            this.Date_End.Name = "Date_End";
            this.Date_End.ResetOnSpace = false;
            this.Date_End.Size = new System.Drawing.Size(99, 21);
            this.Date_End.TabIndex = 10;
            this.Date_End.ValidatingType = typeof(System.DateTime);
            // 
            // gbAccounData
            // 
            this.gbAccounData.Controls.Add(this.tariff_grid);
            this.gbAccounData.Controls.Add(this.label17);
            this.gbAccounData.Controls.Add(this.label5);
            this.gbAccounData.Controls.Add(this.harmful_add);
            this.gbAccounData.Controls.Add(this.classific);
            this.gbAccounData.Controls.Add(this.secret_add);
            this.gbAccounData.Controls.Add(this.order);
            this.gbAccounData.Controls.Add(this.label14);
            this.gbAccounData.Controls.Add(this.label11);
            this.gbAccounData.Controls.Add(this.type_personal);
            this.gbAccounData.Controls.Add(this.tar_by_schema);
            this.gbAccounData.Controls.Add(this.label21);
            this.gbAccounData.Controls.Add(this.label12);
            this.gbAccounData.Controls.Add(this.label8);
            this.gbAccounData.Location = new System.Drawing.Point(3, 253);
            this.gbAccounData.Name = "gbAccounData";
            this.gbAccounData.Size = new System.Drawing.Size(676, 132);
            this.gbAccounData.TabIndex = 103;
            this.gbAccounData.TabStop = false;
            this.gbAccounData.Text = "Бухгалтерские данные";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(492, 103);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 15);
            this.label17.TabIndex = 0;
            this.label17.Text = "Разряд";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Тарифная сетка";
            // 
            // harmful_add
            // 
            this.harmful_add.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.harmful_add.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.harmful_add.FormattingEnabled = true;
            this.harmful_add.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "12",
            "16",
            "20",
            "24"});
            this.harmful_add.Location = new System.Drawing.Point(550, 44);
            this.harmful_add.Name = "harmful_add";
            this.harmful_add.Size = new System.Drawing.Size(72, 23);
            this.harmful_add.TabIndex = 19;
            // 
            // classific
            // 
            this.classific.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.classific.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.classific.FormattingEnabled = true;
            this.classific.Location = new System.Drawing.Point(550, 99);
            this.classific.Name = "classific";
            this.classific.Size = new System.Drawing.Size(72, 23);
            this.classific.TabIndex = 22;
            this.classific.SelectedValueChanged += new System.EventHandler(this.classific_SelectedValueChanged);
            // 
            // tariff_grid
            // 
            this.tariff_grid.FormattingEnabled = true;
            this.tariff_grid.Location = new System.Drawing.Point(138, 99);
            this.tariff_grid.Name = "tariff_grid";
            this.tariff_grid.Size = new System.Drawing.Size(307, 23);
            this.tariff_grid.TabIndex = 17;
            this.tariff_grid.TextChanged += new System.EventHandler(this.tariff_grid_TextChanged);
            // 
            // secret_add
            // 
            this.secret_add.Location = new System.Drawing.Point(550, 72);
            this.secret_add.Name = "secret_add";
            this.secret_add.Size = new System.Drawing.Size(72, 21);
            this.secret_add.TabIndex = 18;
            // 
            // order
            // 
            this.order.BeepOnError = true;
            this.order.HideSelection = false;
            this.order.Location = new System.Drawing.Point(138, 46);
            this.order.Mask = "00000000";
            this.order.Name = "order";
            this.screenTip1.GetScreenTip(this.order).Caption = "Номер заказа";
            this.screenTip1.GetScreenTip(this.order).Text = "Введите номер заказа";
            this.order.Size = new System.Drawing.Size(88, 21);
            this.order.TabIndex = 16;
            this.order.Validating += new System.ComponentModel.CancelEventHandler(this.order_Validating);
            this.order.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.order_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(88, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 15);
            this.label14.TabIndex = 0;
            this.label14.Text = "Заказ";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Тип персонала";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // type_personal
            // 
            this.type_personal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.type_personal.FormattingEnabled = true;
            this.type_personal.Items.AddRange(new object[] {
            "АУП",
            "МОП",
            "ПТП"});
            this.type_personal.Location = new System.Drawing.Point(138, 72);
            this.type_personal.Name = "type_personal";
            this.type_personal.Size = new System.Drawing.Size(88, 23);
            this.type_personal.TabIndex = 21;
            this.type_personal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.type_personal_KeyDown);
            // 
            // tar_by_schema
            // 
            this.tar_by_schema.DecimalPlaces = 2;
            this.tar_by_schema.Location = new System.Drawing.Point(138, 21);
            this.tar_by_schema.Margin = new System.Windows.Forms.Padding(4);
            this.tar_by_schema.Name = "tar_by_schema";
            this.tar_by_schema.Size = new System.Drawing.Size(88, 21);
            this.tar_by_schema.TabIndex = 8;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(369, 75);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(180, 15);
            this.label21.TabIndex = 0;
            this.label21.Text = "Надбавка за секретность";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(325, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(226, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Процент надбавки за вредность";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(5, 17);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 32);
            this.label8.TabIndex = 0;
            this.label8.Text = "Коэффициент по штатному";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btSaveGroupShtat
            // 
            this.btSaveGroupShtat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveGroupShtat.ForeColor = System.Drawing.Color.Blue;
            this.btSaveGroupShtat.Location = new System.Drawing.Point(362, 4);
            this.btSaveGroupShtat.Name = "btSaveGroupShtat";
            this.btSaveGroupShtat.Size = new System.Drawing.Size(179, 22);
            this.btSaveGroupShtat.TabIndex = 24;
            this.btSaveGroupShtat.Text = "Сохранить изменения";
            this.btSaveGroupShtat.UseVisualStyleBackColor = true;
            this.btSaveGroupShtat.Visible = false;
            this.btSaveGroupShtat.Click += new System.EventHandler(this.btSaveGroup_Click);
            // 
            // btExcludeAndSaveShtat
            // 
            this.btExcludeAndSaveShtat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExcludeAndSaveShtat.ForeColor = System.Drawing.Color.Blue;
            this.btExcludeAndSaveShtat.Location = new System.Drawing.Point(516, 4);
            this.btExcludeAndSaveShtat.Name = "btExcludeAndSaveShtat";
            this.btExcludeAndSaveShtat.Size = new System.Drawing.Size(98, 22);
            this.btExcludeAndSaveShtat.TabIndex = 24;
            this.btExcludeAndSaveShtat.Text = "Извещение";
            this.btExcludeAndSaveShtat.UseVisualStyleBackColor = true;
            this.btExcludeAndSaveShtat.Visible = false;
            this.btExcludeAndSaveShtat.Click += new System.EventHandler(this.ExcludeAndSave_Click);
            // 
            // btSaveShtat
            // 
            this.btSaveShtat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveShtat.ForeColor = System.Drawing.Color.Blue;
            this.btSaveShtat.Location = new System.Drawing.Point(566, 4);
            this.btSaveShtat.Name = "btSaveShtat";
            this.screenTip1.GetScreenTip(this.btSaveShtat).Caption = "Сохранение";
            this.screenTip1.GetScreenTip(this.btSaveShtat).Text = "Сохранить изменения в выбранной единице";
            this.btSaveShtat.Size = new System.Drawing.Size(95, 22);
            this.btSaveShtat.TabIndex = 24;
            this.btSaveShtat.Text = "Сохранить";
            this.btSaveShtat.UseVisualStyleBackColor = true;
            this.btSaveShtat.Visible = false;
            this.btSaveShtat.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btAddShtat
            // 
            this.btAddShtat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddShtat.ForeColor = System.Drawing.Color.Blue;
            this.btAddShtat.Location = new System.Drawing.Point(561, 4);
            this.btAddShtat.Name = "btAddShtat";
            this.btAddShtat.Size = new System.Drawing.Size(80, 22);
            this.btAddShtat.TabIndex = 24;
            this.btAddShtat.Text = "Добавить";
            this.btAddShtat.UseVisualStyleBackColor = true;
            this.btAddShtat.Visible = false;
            this.btAddShtat.Click += new System.EventHandler(this.btOkAddShtat_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // btRemoveMainEmpSht
            // 
            this.btRemoveMainEmpSht.ForeColor = System.Drawing.Color.Blue;
            this.btRemoveMainEmpSht.Image = global::Kadr.Properties.Resources.ICO150;
            this.btRemoveMainEmpSht.Location = new System.Drawing.Point(644, 171);
            this.btRemoveMainEmpSht.Name = "btRemoveMainEmpSht";
            this.screenTip1.GetScreenTip(this.btRemoveMainEmpSht).Caption = "Подсказка";
            this.screenTip1.GetScreenTip(this.btRemoveMainEmpSht).Text = "Убрать работника с данной штатной единицы";
            this.btRemoveMainEmpSht.Size = new System.Drawing.Size(30, 26);
            this.btRemoveMainEmpSht.TabIndex = 29;
            this.btRemoveMainEmpSht.UseVisualStyleBackColor = true;
            this.btRemoveMainEmpSht.Click += new System.EventHandler(this.btRemoveMainEmp_Click);
            // 
            // btEditMainEmp
            // 
            this.btEditMainEmp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btEditMainEmp.ForeColor = System.Drawing.Color.Blue;
            this.btEditMainEmp.Image = global::Kadr.Properties.Resources.TrackChanges_Small;
            this.btEditMainEmp.Location = new System.Drawing.Point(609, 171);
            this.btEditMainEmp.Name = "btEditMainEmp";
            this.screenTip1.GetScreenTip(this.btEditMainEmp).Caption = "Подсказка";
            this.screenTip1.GetScreenTip(this.btEditMainEmp).Text = "Выбор нового работника на текущую штатную единицу";
            this.btEditMainEmp.Size = new System.Drawing.Size(30, 26);
            this.btEditMainEmp.TabIndex = 28;
            this.btEditMainEmp.UseVisualStyleBackColor = true;
            this.btEditMainEmp.Click += new System.EventHandler(this.btEditMainEmp_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tpMainEmpShtat);
            this.tabControl1.Controls.Add(this.tpReplEmpShtat);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(696, 419);
            this.tabControl1.TabIndex = 104;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel_data);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(688, 391);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основные данные";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel_data
            // 
            this.panel_data.BackColor = System.Drawing.SystemColors.Control;
            this.panel_data.Controls.Add(this.gbAccounData);
            this.panel_data.Controls.Add(this.gbVacant_);
            this.panel_data.Controls.Add(this.gbMainData);
            this.panel_data.Controls.Add(this.gbDateStaffs);
            this.panel_data.Location = new System.Drawing.Point(3, 3);
            this.panel_data.Name = "panel_data";
            this.panel_data.Size = new System.Drawing.Size(684, 389);
            this.panel_data.TabIndex = 0;
            // 
            // tpMainEmpShtat
            // 
            this.tpMainEmpShtat.Controls.Add(this.btRemoveMainEmpSht);
            this.tpMainEmpShtat.Controls.Add(this.groupBox4);
            this.tpMainEmpShtat.Controls.Add(this.btEditMainEmp);
            this.tpMainEmpShtat.Controls.Add(this.groupBox3);
            this.tpMainEmpShtat.Controls.Add(this.groupBox2);
            this.tpMainEmpShtat.Location = new System.Drawing.Point(4, 24);
            this.tpMainEmpShtat.Name = "tpMainEmpShtat";
            this.tpMainEmpShtat.Padding = new System.Windows.Forms.Padding(3);
            this.tpMainEmpShtat.Size = new System.Drawing.Size(688, 391);
            this.tpMainEmpShtat.TabIndex = 2;
            this.tpMainEmpShtat.Text = "Основной работник";
            this.tpMainEmpShtat.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Controls.Add(this.label26);
            this.groupBox4.Controls.Add(this.MainPer_num);
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.MainMiddleName);
            this.groupBox4.Controls.Add(this.MainName);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.MainFam);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.MainDate_hire);
            this.groupBox4.Location = new System.Drawing.Point(154, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(530, 167);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Общие данные";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(17, 107);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(81, 15);
            this.label28.TabIndex = 2;
            this.label28.Text = "Табельный";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(30, 80);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 15);
            this.label26.TabIndex = 2;
            this.label26.Text = "Отчество";
            // 
            // MainPer_num
            // 
            this.MainPer_num.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPer_num.AutoEllipsis = true;
            this.MainPer_num.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainPer_num.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainPer_num.Location = new System.Drawing.Point(102, 107);
            this.MainPer_num.Name = "MainPer_num";
            this.MainPer_num.Size = new System.Drawing.Size(418, 19);
            this.MainPer_num.TabIndex = 1;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(5, 137);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(95, 15);
            this.label30.TabIndex = 2;
            this.label30.Text = "Дата приёма";
            // 
            // MainMiddleName
            // 
            this.MainMiddleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainMiddleName.AutoEllipsis = true;
            this.MainMiddleName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainMiddleName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainMiddleName.Location = new System.Drawing.Point(102, 78);
            this.MainMiddleName.Name = "MainMiddleName";
            this.MainMiddleName.Size = new System.Drawing.Size(418, 19);
            this.MainMiddleName.TabIndex = 1;
            // 
            // MainName
            // 
            this.MainName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainName.AutoEllipsis = true;
            this.MainName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainName.Location = new System.Drawing.Point(102, 50);
            this.MainName.Name = "MainName";
            this.MainName.Size = new System.Drawing.Size(418, 19);
            this.MainName.TabIndex = 1;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(68, 51);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 15);
            this.label24.TabIndex = 2;
            this.label24.Text = "Имя";
            // 
            // MainFam
            // 
            this.MainFam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainFam.AutoEllipsis = true;
            this.MainFam.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainFam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainFam.Location = new System.Drawing.Point(102, 23);
            this.MainFam.Name = "MainFam";
            this.MainFam.Size = new System.Drawing.Size(418, 19);
            this.MainFam.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(35, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 15);
            this.label16.TabIndex = 2;
            this.label16.Text = "Фамилия";
            // 
            // MainDate_hire
            // 
            this.MainDate_hire.AutoEllipsis = true;
            this.MainDate_hire.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainDate_hire.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainDate_hire.Location = new System.Drawing.Point(102, 136);
            this.MainDate_hire.Name = "MainDate_hire";
            this.MainDate_hire.Size = new System.Drawing.Size(95, 19);
            this.MainDate_hire.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.MainDopSoglSht);
            this.groupBox3.Controls.Add(this.MainComb_AddSht);
            this.groupBox3.Controls.Add(this.label29);
            this.groupBox3.Controls.Add(this.label27);
            this.groupBox3.Controls.Add(this.MainSalary1);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.MainSecret_add);
            this.groupBox3.Controls.Add(this.MainHarmf_ad);
            this.groupBox3.Controls.Add(this.MainSalary);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Location = new System.Drawing.Point(4, 208);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(679, 184);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Коэффициент и надбавки";
            // 
            // MainDopSoglSht
            // 
            this.MainDopSoglSht.Location = new System.Drawing.Point(210, 47);
            this.MainDopSoglSht.Mask = "00/00/0000";
            this.MainDopSoglSht.Name = "MainDopSoglSht";
            this.MainDopSoglSht.ResetOnSpace = false;
            this.MainDopSoglSht.Size = new System.Drawing.Size(89, 21);
            this.MainDopSoglSht.TabIndex = 10;
            this.MainDopSoglSht.ValidatingType = typeof(System.DateTime);
            // 
            // MainComb_AddSht
            // 
            this.MainComb_AddSht.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MainComb_AddSht.Location = new System.Drawing.Point(210, 83);
            this.MainComb_AddSht.Name = "MainComb_AddSht";
            this.MainComb_AddSht.Size = new System.Drawing.Size(89, 14);
            this.MainComb_AddSht.TabIndex = 3;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(23, 139);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(188, 15);
            this.label29.TabIndex = 2;
            this.label29.Text = "Надбавка за секретность :";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(40, 112);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(170, 15);
            this.label27.TabIndex = 2;
            this.label27.Text = "Надбавка за вредность:";
            // 
            // MainSalary1
            // 
            this.MainSalary1.Enabled = false;
            this.MainSalary1.Location = new System.Drawing.Point(304, 20);
            this.MainSalary1.Name = "MainSalary1";
            this.MainSalary1.Size = new System.Drawing.Size(89, 21);
            this.MainSalary1.TabIndex = 0;
            this.MainSalary1.Visible = false;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(28, 86);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(182, 15);
            this.label25.TabIndex = 2;
            this.label25.Text = "Надбавка за совмещение:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(87, 49);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(124, 15);
            this.label22.TabIndex = 2;
            this.label22.Text = "Доп. соглашение:";
            // 
            // MainSecret_add
            // 
            this.MainSecret_add.AutoEllipsis = true;
            this.MainSecret_add.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainSecret_add.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSecret_add.Location = new System.Drawing.Point(210, 135);
            this.MainSecret_add.Name = "MainSecret_add";
            this.MainSecret_add.Size = new System.Drawing.Size(89, 19);
            this.MainSecret_add.TabIndex = 1;
            // 
            // MainHarmf_ad
            // 
            this.MainHarmf_ad.AutoEllipsis = true;
            this.MainHarmf_ad.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainHarmf_ad.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainHarmf_ad.Location = new System.Drawing.Point(210, 109);
            this.MainHarmf_ad.Name = "MainHarmf_ad";
            this.MainHarmf_ad.Size = new System.Drawing.Size(89, 19);
            this.MainHarmf_ad.TabIndex = 1;
            // 
            // MainSalary
            // 
            this.MainSalary.AutoEllipsis = true;
            this.MainSalary.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainSalary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSalary.Location = new System.Drawing.Point(210, 20);
            this.MainSalary.Name = "MainSalary";
            this.MainSalary.Size = new System.Drawing.Size(89, 19);
            this.MainSalary.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(197, 15);
            this.label15.TabIndex = 2;
            this.label15.Text = "Фактический коэффициент:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.MainEmpPhoto);
            this.groupBox2.Location = new System.Drawing.Point(4, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(144, 202);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // MainEmpPhoto
            // 
            this.MainEmpPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainEmpPhoto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainEmpPhoto.Location = new System.Drawing.Point(4, 11);
            this.MainEmpPhoto.Name = "MainEmpPhoto";
            this.MainEmpPhoto.Size = new System.Drawing.Size(134, 186);
            this.MainEmpPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MainEmpPhoto.TabIndex = 0;
            this.MainEmpPhoto.TabStop = false;
            // 
            // tpReplEmpShtat
            // 
            this.tpReplEmpShtat.Controls.Add(this.groupBox7);
            this.tpReplEmpShtat.Controls.Add(this.groupBox5);
            this.tpReplEmpShtat.Controls.Add(this.groupBox6);
            this.tpReplEmpShtat.Location = new System.Drawing.Point(4, 24);
            this.tpReplEmpShtat.Name = "tpReplEmpShtat";
            this.tpReplEmpShtat.Padding = new System.Windows.Forms.Padding(3);
            this.tpReplEmpShtat.Size = new System.Drawing.Size(688, 391);
            this.tpReplEmpShtat.TabIndex = 3;
            this.tpReplEmpShtat.Text = "Совмещающий сотрудник";
            this.tpReplEmpShtat.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Controls.Add(this.ReplEmpPhoto);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(3, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 0, 0, 5);
            this.groupBox7.Size = new System.Drawing.Size(682, 201);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.GridReplEmp);
            this.groupBox8.Controls.Add(this.toolStrip1);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(136, 14);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(546, 182);
            this.groupBox8.TabIndex = 28;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Совмещающий(-ие) работник(-и):";
            // 
            // GridReplEmp
            // 
            this.GridReplEmp.AllowUserToAddRows = false;
            this.GridReplEmp.AllowUserToDeleteRows = false;
            this.GridReplEmp.AllowUserToResizeRows = false;
            this.GridReplEmp.BackgroundColor = System.Drawing.Color.White;
            this.GridReplEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridReplEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridReplEmp.Location = new System.Drawing.Point(3, 42);
            this.GridReplEmp.Name = "GridReplEmp";
            this.GridReplEmp.ReadOnly = true;
            this.GridReplEmp.RowHeadersVisible = false;
            this.GridReplEmp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridReplEmp.Size = new System.Drawing.Size(540, 137);
            this.GridReplEmp.TabIndex = 32;
            this.GridReplEmp.SelectionChanged += new System.EventHandler(this.GridReplEmp_SelectionChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btNewCombReplEmp,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(3, 17);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(540, 25);
            this.toolStrip1.TabIndex = 33;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btNewCombReplEmp
            // 
            this.btNewCombReplEmp.Image = global::Kadr.Properties.Resources.ClearDrawing;
            this.btNewCombReplEmp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btNewCombReplEmp.Name = "btNewCombReplEmp";
            this.btNewCombReplEmp.Size = new System.Drawing.Size(77, 22);
            this.btNewCombReplEmp.Text = "Добавить";
            this.btNewCombReplEmp.Click += new System.EventHandler(this.btSetReplEmp_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::Kadr.Properties.Resources.Prepare_Large;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(104, 22);
            this.toolStripButton2.Text = "Редактировать";
            this.toolStripButton2.Click += new System.EventHandler(this.btEditReplEmp_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::Kadr.Properties.Resources.Remove_SmallNew;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(70, 22);
            this.toolStripButton3.Text = "Удалить";
            this.toolStripButton3.Click += new System.EventHandler(this.btClearReplEmp_Click);
            // 
            // ReplEmpPhoto
            // 
            this.ReplEmpPhoto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ReplEmpPhoto.Dock = System.Windows.Forms.DockStyle.Left;
            this.ReplEmpPhoto.Location = new System.Drawing.Point(4, 14);
            this.ReplEmpPhoto.Name = "ReplEmpPhoto";
            this.ReplEmpPhoto.Size = new System.Drawing.Size(132, 182);
            this.ReplEmpPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ReplEmpPhoto.TabIndex = 0;
            this.ReplEmpPhoto.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Controls.Add(this.Repl_per_num);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.Repl_Middle_name);
            this.groupBox5.Controls.Add(this.Repl_name);
            this.groupBox5.Controls.Add(this.label37);
            this.groupBox5.Controls.Add(this.ReplFam);
            this.groupBox5.Controls.Add(this.label39);
            this.groupBox5.Controls.Add(this.Repl_date_hire);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(3, 204);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(682, 103);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Общие данные";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(17, 81);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(81, 15);
            this.label31.TabIndex = 2;
            this.label31.Text = "Табельный";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(30, 61);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(71, 15);
            this.label32.TabIndex = 2;
            this.label32.Text = "Отчество";
            // 
            // Repl_per_num
            // 
            this.Repl_per_num.AutoEllipsis = true;
            this.Repl_per_num.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Repl_per_num.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Repl_per_num.Location = new System.Drawing.Point(102, 81);
            this.Repl_per_num.Name = "Repl_per_num";
            this.Repl_per_num.Size = new System.Drawing.Size(104, 19);
            this.Repl_per_num.TabIndex = 1;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(218, 82);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(95, 15);
            this.label34.TabIndex = 2;
            this.label34.Text = "Дата приёма";
            // 
            // Repl_Middle_name
            // 
            this.Repl_Middle_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Repl_Middle_name.AutoEllipsis = true;
            this.Repl_Middle_name.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Repl_Middle_name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Repl_Middle_name.Location = new System.Drawing.Point(102, 59);
            this.Repl_Middle_name.Name = "Repl_Middle_name";
            this.Repl_Middle_name.Size = new System.Drawing.Size(570, 19);
            this.Repl_Middle_name.TabIndex = 1;
            // 
            // Repl_name
            // 
            this.Repl_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Repl_name.AutoEllipsis = true;
            this.Repl_name.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Repl_name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Repl_name.Location = new System.Drawing.Point(102, 38);
            this.Repl_name.Name = "Repl_name";
            this.Repl_name.Size = new System.Drawing.Size(570, 19);
            this.Repl_name.TabIndex = 1;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(68, 38);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(35, 15);
            this.label37.TabIndex = 2;
            this.label37.Text = "Имя";
            // 
            // ReplFam
            // 
            this.ReplFam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplFam.AutoEllipsis = true;
            this.ReplFam.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ReplFam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ReplFam.Location = new System.Drawing.Point(102, 16);
            this.ReplFam.Name = "ReplFam";
            this.ReplFam.Size = new System.Drawing.Size(570, 19);
            this.ReplFam.TabIndex = 1;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(35, 18);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(69, 15);
            this.label39.TabIndex = 2;
            this.label39.Text = "Фамилия";
            // 
            // Repl_date_hire
            // 
            this.Repl_date_hire.AutoEllipsis = true;
            this.Repl_date_hire.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Repl_date_hire.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Repl_date_hire.Location = new System.Drawing.Point(315, 81);
            this.Repl_date_hire.Name = "Repl_date_hire";
            this.Repl_date_hire.Size = new System.Drawing.Size(95, 19);
            this.Repl_date_hire.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Repl_secret_add);
            this.groupBox6.Controls.Add(this.Repl_harm_add);
            this.groupBox6.Controls.Add(this.Repl_comb_add);
            this.groupBox6.Controls.Add(this.Repl_dop_sogl);
            this.groupBox6.Controls.Add(this.label41);
            this.groupBox6.Controls.Add(this.label42);
            this.groupBox6.Controls.Add(this.Repl_salary);
            this.groupBox6.Controls.Add(this.label43);
            this.groupBox6.Controls.Add(this.label44);
            this.groupBox6.Controls.Add(this.label45);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox6.Location = new System.Drawing.Point(3, 307);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(682, 81);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Коэффициент и надбавки";
            // 
            // Repl_secret_add
            // 
            this.Repl_secret_add.Enabled = false;
            this.Repl_secret_add.Location = new System.Drawing.Point(511, 56);
            this.Repl_secret_add.Name = "Repl_secret_add";
            this.Repl_secret_add.Size = new System.Drawing.Size(89, 21);
            this.Repl_secret_add.TabIndex = 0;
            // 
            // Repl_harm_add
            // 
            this.Repl_harm_add.Enabled = false;
            this.Repl_harm_add.Location = new System.Drawing.Point(511, 35);
            this.Repl_harm_add.Name = "Repl_harm_add";
            this.Repl_harm_add.Size = new System.Drawing.Size(89, 21);
            this.Repl_harm_add.TabIndex = 0;
            // 
            // Repl_comb_add
            // 
            this.Repl_comb_add.Enabled = false;
            this.Repl_comb_add.Location = new System.Drawing.Point(511, 13);
            this.Repl_comb_add.Name = "Repl_comb_add";
            this.Repl_comb_add.Size = new System.Drawing.Size(89, 21);
            this.Repl_comb_add.TabIndex = 0;
            // 
            // Repl_dop_sogl
            // 
            this.Repl_dop_sogl.Enabled = false;
            this.Repl_dop_sogl.Location = new System.Drawing.Point(202, 35);
            this.Repl_dop_sogl.Name = "Repl_dop_sogl";
            this.Repl_dop_sogl.Size = new System.Drawing.Size(89, 21);
            this.Repl_dop_sogl.TabIndex = 0;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(324, 59);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(188, 15);
            this.label41.TabIndex = 2;
            this.label41.Text = "Надбавка за секретность :";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(341, 38);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(170, 15);
            this.label42.TabIndex = 2;
            this.label42.Text = "Надбавка за вредность:";
            // 
            // Repl_salary
            // 
            this.Repl_salary.Enabled = false;
            this.Repl_salary.Location = new System.Drawing.Point(202, 13);
            this.Repl_salary.Name = "Repl_salary";
            this.Repl_salary.Size = new System.Drawing.Size(89, 21);
            this.Repl_salary.TabIndex = 0;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(330, 16);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(182, 15);
            this.label43.TabIndex = 2;
            this.label43.Text = "Надбавка за совмещение:";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(79, 38);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(124, 15);
            this.label44.TabIndex = 2;
            this.label44.Text = "Доп. соглашение:";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(7, 16);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(197, 15);
            this.label45.TabIndex = 2;
            this.label45.Text = "Фактический коэффициент:";
            // 
            // panel_main_commands
            // 
            this.panel_main_commands.Controls.Add(this.SetTemp);
            this.panel_main_commands.Controls.Add(this.btSaveGroupShtat);
            this.panel_main_commands.Controls.Add(this.btExcludeAndSaveShtat);
            this.panel_main_commands.Controls.Add(this.btAddShtat);
            this.panel_main_commands.Controls.Add(this.btSaveShtat);
            this.panel_main_commands.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_main_commands.Location = new System.Drawing.Point(0, 419);
            this.panel_main_commands.Name = "panel_main_commands";
            this.panel_main_commands.Size = new System.Drawing.Size(696, 30);
            this.panel_main_commands.TabIndex = 105;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.BackColor = System.Drawing.Color.Gainsboro;
            this.toolTip1.ForeColor = System.Drawing.Color.Black;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // SS1
            // 
            this.SS1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SS1.BackColor = System.Drawing.Color.Transparent;
            this.SS1.ByRule = "";
            this.SS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SS1.Location = new System.Drawing.Point(22, 20);
            this.SS1.Name = "SS1";
            this.SS1.Size = new System.Drawing.Size(651, 19);
            this.SS1.TabIndex = 9;
            // 
            // Add_Edit_staff
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(696, 449);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel_main_commands);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Add_Edit_staff";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление Штатной единицы";
            this.Load += new System.EventHandler(this.Add_staff_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPackage)).EndInit();
            this.gbMainData.ResumeLayout(false);
            this.gbMainData.PerformLayout();
            this.gbVacant_.ResumeLayout(false);
            this.gbVacant_.PerformLayout();
            this.gbDateStaffs.ResumeLayout(false);
            this.gbDateStaffs.PerformLayout();
            this.gbAccounData.ResumeLayout(false);
            this.gbAccounData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tar_by_schema)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_data.ResumeLayout(false);
            this.tpMainEmpShtat.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainEmpPhoto)).EndInit();
            this.tpReplEmpShtat.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridReplEmp)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReplEmpPhoto)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel_main_commands.ResumeLayout(false);
            this.panel_main_commands.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numberOfPackage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private Elegant.Ui.TextBox comments_pos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox DateEndVacant;
        private System.Windows.Forms.MaskedTextBox Date_End;
        private System.Windows.Forms.MaskedTextBox Date_Begin;
        private Elegant.Ui.CheckBox vacant_sign;
        private System.Windows.Forms.Button btSaveShtat;
        private System.Windows.Forms.Button btAddShtat;
        private System.Windows.Forms.GroupBox gbAccounData;
        private System.Windows.Forms.GroupBox gbDateStaffs;
        private System.Windows.Forms.GroupBox gbVacant_;
        private System.Windows.Forms.GroupBox gbMainData;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox type_personal;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.MaskedTextBox order;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox classific;
        private System.Windows.Forms.ComboBox tariff_grid;
        private Elegant.Ui.ScreenTip screenTip1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox SetTemp;
        private System.Windows.Forms.Button btExcludeAndSaveShtat;
        private System.Windows.Forms.MaskedTextBox secret_add;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox harmful_add;
        private System.Windows.Forms.Button btSaveGroupShtat;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel_data;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpMainEmpShtat;
        private System.Windows.Forms.TabPage tpReplEmpShtat;
        private System.Windows.Forms.Label MainFam;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox MainEmpPhoto;
        private System.Windows.Forms.Label MainMiddleName;
        private System.Windows.Forms.Label MainName;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label MainPer_num;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox MainSalary1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label MainDate_hire;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button btRemoveMainEmpSht;
        private System.Windows.Forms.Button btEditMainEmp;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label Repl_per_num;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label Repl_Middle_name;
        private System.Windows.Forms.Label Repl_name;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label ReplFam;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label Repl_date_hire;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox Repl_secret_add;
        private System.Windows.Forms.TextBox Repl_harm_add;
        private System.Windows.Forms.TextBox Repl_comb_add;
        private System.Windows.Forms.TextBox Repl_dop_sogl;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox Repl_salary;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.PictureBox ReplEmpPhoto;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataGridView GridReplEmp;
        private System.Windows.Forms.MaskedTextBox MainComb_AddSht;
        private System.Windows.Forms.MaskedTextBox MainDopSoglSht;
        private Kadr.Classes.SubdivSelector SS1;
        private System.Windows.Forms.Label MainSalary;
        private System.Windows.Forms.Label MainSecret_add;
        private System.Windows.Forms.Label MainHarmf_ad;
        private System.Windows.Forms.ComboBox Degree;
        private System.Windows.Forms.MaskedTextBox code_pos;
        private System.Windows.Forms.Button bt_find_pos;
        private System.Windows.Forms.Label label_pos_name;
        private System.Windows.Forms.NumericUpDown tar_by_schema;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btNewCombReplEmp;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Panel panel_main_commands;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}