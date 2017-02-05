namespace Tabel
{
    partial class Gr_Work_Day
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.tsGr_Work_Day = new System.Windows.Forms.ToolStrip();
            this.tsbAddGR_Work_Day = new System.Windows.Forms.ToolStripButton();
            this.tsbEditGR_Work_Day = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteGR_Work_Day = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsbAccessSubdiv = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chSign_Shiftman = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chSign_Summarize = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mbHours_Dinner = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mbHours_For_Graph = new System.Windows.Forms.MaskedTextBox();
            this.mbHours_For_Norm_106 = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.mbHours_For_Norm = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chSign_Shorten = new System.Windows.Forms.CheckBox();
            this.deDate_End_Graph = new EditorsLibrary.DateEditor();
            this.label6 = new System.Windows.Forms.Label();
            this.mbCount_Day = new System.Windows.Forms.MaskedTextBox();
            this.chSign_Floating = new System.Windows.Forms.CheckBox();
            this.chSign_compact_day_work = new System.Windows.Forms.CheckBox();
            this.chSign_holiday_work = new System.Windows.Forms.CheckBox();
            this.tbGr_Work_Name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btExit = new Elegant.Ui.Button();
            this.btSave = new Elegant.Ui.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgTime_Interval = new System.Windows.Forms.DataGridView();
            this.dgAccessSubdiv = new System.Windows.Forms.DataGridView();
            this.dgGr_Work_Day = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.cbACCESS_TEMPLATE = new System.Windows.Forms.ComboBox();
            this.tsGr_Work_Day.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTime_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgAccessSubdiv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgGr_Work_Day)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // tsGr_Work_Day
            // 
            this.tsGr_Work_Day.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsGr_Work_Day.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddGR_Work_Day,
            this.tsbEditGR_Work_Day,
            this.tsbDeleteGR_Work_Day,
            this.toolStripLabel1,
            this.tsbAccessSubdiv});
            this.tsGr_Work_Day.Location = new System.Drawing.Point(0, 0);
            this.tsGr_Work_Day.Name = "tsGr_Work_Day";
            this.tsGr_Work_Day.Size = new System.Drawing.Size(802, 25);
            this.tsGr_Work_Day.TabIndex = 3;
            this.tsGr_Work_Day.Text = "toolStrip1";
            // 
            // tsbAddGR_Work_Day
            // 
            this.tsbAddGR_Work_Day.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddGR_Work_Day.Image = global::Tabel.Properties.Resources.document_new_62;
            this.tsbAddGR_Work_Day.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddGR_Work_Day.Name = "tsbAddGR_Work_Day";
            this.tsbAddGR_Work_Day.Size = new System.Drawing.Size(23, 22);
            this.tsbAddGR_Work_Day.Text = "toolStripButton1";
            this.tsbAddGR_Work_Day.ToolTipText = "Добавить";
            this.tsbAddGR_Work_Day.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbEditGR_Work_Day
            // 
            this.tsbEditGR_Work_Day.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditGR_Work_Day.Image = global::Tabel.Properties.Resources.table_edit1;
            this.tsbEditGR_Work_Day.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditGR_Work_Day.Name = "tsbEditGR_Work_Day";
            this.tsbEditGR_Work_Day.Size = new System.Drawing.Size(23, 22);
            this.tsbEditGR_Work_Day.Text = "toolStripButton2";
            this.tsbEditGR_Work_Day.ToolTipText = "Редактировать";
            this.tsbEditGR_Work_Day.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbDeleteGR_Work_Day
            // 
            this.tsbDeleteGR_Work_Day.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteGR_Work_Day.Image = global::Tabel.Properties.Resources.Delete;
            this.tsbDeleteGR_Work_Day.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteGR_Work_Day.Name = "tsbDeleteGR_Work_Day";
            this.tsbDeleteGR_Work_Day.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteGR_Work_Day.Text = "toolStripButton3";
            this.tsbDeleteGR_Work_Day.ToolTipText = "Удалить";
            this.tsbDeleteGR_Work_Day.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(229, 22);
            this.toolStripLabel1.Text = "      Редактирование списка подразделений";
            // 
            // tsbAccessSubdiv
            // 
            this.tsbAccessSubdiv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAccessSubdiv.Image = global::Tabel.Properties.Resources.table_edit1;
            this.tsbAccessSubdiv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAccessSubdiv.Name = "tsbAccessSubdiv";
            this.tsbAccessSubdiv.Size = new System.Drawing.Size(23, 22);
            this.tsbAccessSubdiv.Text = "Редактировать список подразделений";
            this.tsbAccessSubdiv.ToolTipText = "Редактировать список подразделений";
            this.tsbAccessSubdiv.Click += new System.EventHandler(this.tsbAdd_Subdiv_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbACCESS_TEMPLATE);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.chSign_Shiftman);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.chSign_Shorten);
            this.panel1.Controls.Add(this.deDate_End_Graph);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.mbCount_Day);
            this.panel1.Controls.Add(this.chSign_Floating);
            this.panel1.Controls.Add(this.chSign_compact_day_work);
            this.panel1.Controls.Add(this.chSign_holiday_work);
            this.panel1.Controls.Add(this.tbGr_Work_Name);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 289);
            this.panel1.TabIndex = 0;
            // 
            // chSign_Shiftman
            // 
            this.chSign_Shiftman.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSign_Shiftman.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_Shiftman.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Shiftman.Location = new System.Drawing.Point(113, 260);
            this.chSign_Shiftman.Name = "chSign_Shiftman";
            this.chSign_Shiftman.Size = new System.Drawing.Size(674, 22);
            this.chSign_Shiftman.TabIndex = 10;
            this.chSign_Shiftman.Text = "Признак сменного графика работы - сменщики (используется при расчете средней зарп" +
    "латы)";
            this.chSign_Shiftman.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chSign_Summarize);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.mbHours_Dinner);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(3, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(796, 46);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Суммированный учет рабочего времени";
            // 
            // chSign_Summarize
            // 
            this.chSign_Summarize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSign_Summarize.AutoSize = true;
            this.chSign_Summarize.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_Summarize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Summarize.Location = new System.Drawing.Point(22, 22);
            this.chSign_Summarize.Name = "chSign_Summarize";
            this.chSign_Summarize.Size = new System.Drawing.Size(363, 19);
            this.chSign_Summarize.TabIndex = 0;
            this.chSign_Summarize.Text = "признак суммированного учета рабочего времени";
            this.chSign_Summarize.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(454, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "количество часов обеденного перерыва";
            // 
            // mbHours_Dinner
            // 
            this.mbHours_Dinner.BackColor = System.Drawing.Color.White;
            this.mbHours_Dinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbHours_Dinner.Location = new System.Drawing.Point(741, 20);
            this.mbHours_Dinner.Name = "mbHours_Dinner";
            this.mbHours_Dinner.Size = new System.Drawing.Size(43, 21);
            this.mbHours_Dinner.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mbHours_For_Graph);
            this.groupBox1.Controls.Add(this.mbHours_For_Norm_106);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.mbHours_For_Norm);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(3, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(796, 49);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Кол-во часов по норме в день в зависимости от продолжительности рабочей недели";
            // 
            // mbHours_For_Graph
            // 
            this.mbHours_For_Graph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbHours_For_Graph.BackColor = System.Drawing.Color.White;
            this.mbHours_For_Graph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbHours_For_Graph.Location = new System.Drawing.Point(373, 24);
            this.mbHours_For_Graph.Name = "mbHours_For_Graph";
            this.mbHours_For_Graph.Size = new System.Drawing.Size(43, 21);
            this.mbHours_For_Graph.TabIndex = 1;
            // 
            // mbHours_For_Norm_106
            // 
            this.mbHours_For_Norm_106.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbHours_For_Norm_106.BackColor = System.Drawing.Color.White;
            this.mbHours_For_Norm_106.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbHours_For_Norm_106.Location = new System.Drawing.Point(741, 24);
            this.mbHours_For_Norm_106.Name = "mbHours_For_Norm_106";
            this.mbHours_For_Norm_106.Size = new System.Drawing.Size(43, 21);
            this.mbHours_For_Norm_106.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(19, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "для 102 ш.о.";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(618, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "для 106/124 ш.о.";
            // 
            // mbHours_For_Norm
            // 
            this.mbHours_For_Norm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbHours_For_Norm.BackColor = System.Drawing.Color.White;
            this.mbHours_For_Norm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbHours_For_Norm.Location = new System.Drawing.Point(114, 24);
            this.mbHours_For_Norm.Name = "mbHours_For_Norm";
            this.mbHours_For_Norm.Size = new System.Drawing.Size(43, 21);
            this.mbHours_For_Norm.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(278, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "для 111 ш.о.";
            // 
            // chSign_Shorten
            // 
            this.chSign_Shorten.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSign_Shorten.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_Shorten.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Shorten.Location = new System.Drawing.Point(113, 238);
            this.chSign_Shorten.Name = "chSign_Shorten";
            this.chSign_Shorten.Size = new System.Drawing.Size(674, 22);
            this.chSign_Shorten.TabIndex = 8;
            this.chSign_Shorten.Text = "Признак сокращенного графика работы (начислять 246 в.о. - детские до 3 лет)";
            this.chSign_Shorten.UseVisualStyleBackColor = true;
            // 
            // deDate_End_Graph
            // 
            this.deDate_End_Graph.AutoSize = true;
            this.deDate_End_Graph.BackColor = System.Drawing.Color.White;
            this.deDate_End_Graph.Date = null;
            this.deDate_End_Graph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_End_Graph.Location = new System.Drawing.Point(16, 239);
            this.deDate_End_Graph.Name = "deDate_End_Graph";
            this.deDate_End_Graph.ReadOnly = false;
            this.deDate_End_Graph.Size = new System.Drawing.Size(77, 21);
            this.deDate_End_Graph.TabIndex = 4;
            this.deDate_End_Graph.TextDate = null;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(13, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 63);
            this.label6.TabIndex = 9;
            this.label6.Text = "Дата окончания работы графика";
            // 
            // mbCount_Day
            // 
            this.mbCount_Day.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbCount_Day.BackColor = System.Drawing.Color.White;
            this.mbCount_Day.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbCount_Day.Location = new System.Drawing.Point(744, 24);
            this.mbCount_Day.Mask = "00";
            this.mbCount_Day.Name = "mbCount_Day";
            this.mbCount_Day.Size = new System.Drawing.Size(43, 21);
            this.mbCount_Day.TabIndex = 1;
            // 
            // chSign_Floating
            // 
            this.chSign_Floating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSign_Floating.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_Floating.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Floating.Location = new System.Drawing.Point(113, 216);
            this.chSign_Floating.Name = "chSign_Floating";
            this.chSign_Floating.Size = new System.Drawing.Size(674, 22);
            this.chSign_Floating.TabIndex = 7;
            this.chSign_Floating.Text = "Признак плавающего графика работы (разные смены работы)";
            this.chSign_Floating.UseVisualStyleBackColor = true;
            // 
            // chSign_compact_day_work
            // 
            this.chSign_compact_day_work.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSign_compact_day_work.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_compact_day_work.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_compact_day_work.Location = new System.Drawing.Point(113, 194);
            this.chSign_compact_day_work.Name = "chSign_compact_day_work";
            this.chSign_compact_day_work.Size = new System.Drawing.Size(674, 22);
            this.chSign_compact_day_work.TabIndex = 6;
            this.chSign_compact_day_work.Text = "Признак работы графика в сокращенный день (график не прерывается в сокращенный де" +
    "нь)";
            this.chSign_compact_day_work.UseVisualStyleBackColor = true;
            // 
            // chSign_holiday_work
            // 
            this.chSign_holiday_work.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chSign_holiday_work.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chSign_holiday_work.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_holiday_work.Location = new System.Drawing.Point(113, 172);
            this.chSign_holiday_work.Name = "chSign_holiday_work";
            this.chSign_holiday_work.Size = new System.Drawing.Size(674, 22);
            this.chSign_holiday_work.TabIndex = 5;
            this.chSign_holiday_work.Text = "Признак работы графика в выходной день (график не прерывается в выходной день)";
            this.chSign_holiday_work.UseVisualStyleBackColor = true;
            // 
            // tbGr_Work_Name
            // 
            this.tbGr_Work_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGr_Work_Name.BackColor = System.Drawing.Color.White;
            this.tbGr_Work_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGr_Work_Name.Location = new System.Drawing.Point(117, 13);
            this.tbGr_Work_Name.Multiline = true;
            this.tbGr_Work_Name.Name = "tbGr_Work_Name";
            this.tbGr_Work_Name.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbGr_Work_Name.Size = new System.Drawing.Size(478, 47);
            this.tbGr_Work_Name.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(604, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 34);
            this.label5.TabIndex = 0;
            this.label5.Text = "Количество дней в графике работы";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 56);
            this.label1.TabIndex = 0;
            this.label1.Text = "Наименование графика работы";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btExit);
            this.panel2.Controls.Add(this.btSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 635);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 35);
            this.panel2.TabIndex = 2;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "aa08f5f7-72ea-4367-93d1-47bb9218ade1";
            this.btExit.Location = new System.Drawing.Point(707, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Id = "7eddb2d0-0624-48d6-83c3-53412f45eee1";
            this.btSave.Location = new System.Drawing.Point(588, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(104, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.dgTime_Interval, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgAccessSubdiv, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgGr_Work_Day, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 314);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.68016F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.31984F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(802, 321);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // dgTime_Interval
            // 
            this.dgTime_Interval.AllowUserToAddRows = false;
            this.dgTime_Interval.AllowUserToDeleteRows = false;
            this.dgTime_Interval.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTime_Interval.BackgroundColor = System.Drawing.Color.White;
            this.dgTime_Interval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTime_Interval.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgTime_Interval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTime_Interval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTime_Interval.Location = new System.Drawing.Point(3, 135);
            this.dgTime_Interval.Name = "dgTime_Interval";
            this.dgTime_Interval.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTime_Interval.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgTime_Interval.RowHeadersWidth = 24;
            this.dgTime_Interval.Size = new System.Drawing.Size(796, 94);
            this.dgTime_Interval.TabIndex = 9;
            // 
            // dgAccessSubdiv
            // 
            this.dgAccessSubdiv.AllowUserToAddRows = false;
            this.dgAccessSubdiv.AllowUserToDeleteRows = false;
            this.dgAccessSubdiv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgAccessSubdiv.BackgroundColor = System.Drawing.Color.White;
            this.dgAccessSubdiv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAccessSubdiv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgAccessSubdiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAccessSubdiv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAccessSubdiv.Location = new System.Drawing.Point(3, 235);
            this.dgAccessSubdiv.Name = "dgAccessSubdiv";
            this.dgAccessSubdiv.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAccessSubdiv.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgAccessSubdiv.RowHeadersWidth = 24;
            this.dgAccessSubdiv.Size = new System.Drawing.Size(796, 83);
            this.dgAccessSubdiv.TabIndex = 8;
            // 
            // dgGr_Work_Day
            // 
            this.dgGr_Work_Day.AllowUserToAddRows = false;
            this.dgGr_Work_Day.AllowUserToDeleteRows = false;
            this.dgGr_Work_Day.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgGr_Work_Day.BackgroundColor = System.Drawing.Color.White;
            this.dgGr_Work_Day.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGr_Work_Day.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgGr_Work_Day.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgGr_Work_Day.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgGr_Work_Day.Location = new System.Drawing.Point(3, 3);
            this.dgGr_Work_Day.Name = "dgGr_Work_Day";
            this.dgGr_Work_Day.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGr_Work_Day.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgGr_Work_Day.RowHeadersWidth = 24;
            this.dgGr_Work_Day.Size = new System.Drawing.Size(796, 126);
            this.dgGr_Work_Day.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(13, 291);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(226, 15);
            this.label8.TabIndex = 11;
            this.label8.Text = "Наименование шаблона доступа";
            this.label8.Visible = false;
            // 
            // cbACCESS_TEMPLATE
            // 
            this.cbACCESS_TEMPLATE.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbACCESS_TEMPLATE.BackColor = System.Drawing.Color.White;
            this.cbACCESS_TEMPLATE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbACCESS_TEMPLATE.FormattingEnabled = true;
            this.cbACCESS_TEMPLATE.Location = new System.Drawing.Point(244, 288);
            this.cbACCESS_TEMPLATE.Name = "cbACCESS_TEMPLATE";
            this.cbACCESS_TEMPLATE.Size = new System.Drawing.Size(543, 23);
            this.cbACCESS_TEMPLATE.TabIndex = 12;
            this.cbACCESS_TEMPLATE.Visible = false;
            // 
            // Gr_Work_Day
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 670);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tsGr_Work_Day);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Gr_Work_Day";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "График работы";
            this.tsGr_Work_Day.ResumeLayout(false);
            this.tsGr_Work_Day.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTime_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgAccessSubdiv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgGr_Work_Day)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ToolStrip tsGr_Work_Day;
        private System.Windows.Forms.ToolStripButton tsbAddGR_Work_Day;
        private System.Windows.Forms.ToolStripButton tsbEditGR_Work_Day;
        private System.Windows.Forms.ToolStripButton tsbDeleteGR_Work_Day;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbGr_Work_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chSign_compact_day_work;
        private System.Windows.Forms.CheckBox chSign_holiday_work;
        private System.Windows.Forms.MaskedTextBox mbCount_Day;
        private System.Windows.Forms.CheckBox chSign_Floating;
        private System.Windows.Forms.MaskedTextBox mbHours_For_Norm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox mbHours_Dinner;
        private System.Windows.Forms.CheckBox chSign_Summarize;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton tsbAccessSubdiv;
        private System.Windows.Forms.MaskedTextBox mbHours_For_Graph;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgTime_Interval;
        private System.Windows.Forms.DataGridView dgAccessSubdiv;
        private System.Windows.Forms.DataGridView dgGr_Work_Day;
        private System.Windows.Forms.Label label6;
        private EditorsLibrary.DateEditor deDate_End_Graph;
        private System.Windows.Forms.CheckBox chSign_Shorten;
        private System.Windows.Forms.MaskedTextBox mbHours_For_Norm_106;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chSign_Shiftman;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbACCESS_TEMPLATE;
    }
}