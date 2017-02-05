namespace Tabel
{
    partial class EditGr_Work
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btViewGr_Work = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btSaveGR = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbGr_work_day_num = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.deGr_work_date_begin = new LibraryKadr.DateEditor();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgGR_Work = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpDate_End_Graph = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgHistoryGraph = new System.Windows.Forms.DataGridView();
            this.pnWorkReg_Doc = new System.Windows.Forms.Panel();
            this.tsbDeleteGR_Work = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudYearFilterHistoryGraph = new System.Windows.Forms.NumericUpDown();
            this.chFilterToYearHistoryGraph = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgGR_Work)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgHistoryGraph)).BeginInit();
            this.pnWorkReg_Doc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYearFilterHistoryGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btViewGr_Work);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSaveGR);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 470);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1001, 37);
            this.panel1.TabIndex = 2;
            // 
            // btViewGr_Work
            // 
            this.btViewGr_Work.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btViewGr_Work.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btViewGr_Work.Location = new System.Drawing.Point(17, 7);
            this.btViewGr_Work.Name = "btViewGr_Work";
            this.btViewGr_Work.Size = new System.Drawing.Size(208, 23);
            this.btViewGr_Work.TabIndex = 5;
            this.btViewGr_Work.Text = "Просмотр графика работы";
            this.btViewGr_Work.Click += new System.EventHandler(this.dgGR_Work_DoubleClick);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(902, 7);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(84, 23);
            this.btExit.TabIndex = 4;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSaveGR
            // 
            this.btSaveGR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveGR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSaveGR.Location = new System.Drawing.Point(795, 7);
            this.btSaveGR.Name = "btSaveGR";
            this.btSaveGR.Size = new System.Drawing.Size(98, 23);
            this.btSaveGR.TabIndex = 3;
            this.btSaveGR.Text = "Сохранить";
            this.btSaveGR.Click += new System.EventHandler(this.btSaveGR_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbGr_work_day_num);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.deGr_work_date_begin);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 358);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 106);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(11, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(457, 15);
            this.label6.TabIndex = 39;
            this.label6.Text = "Промежуток работы для графика на дату начала работы графика ";
            // 
            // cbGr_work_day_num
            // 
            this.cbGr_work_day_num.BackColor = System.Drawing.Color.White;
            this.cbGr_work_day_num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbGr_work_day_num.FormattingEnabled = true;
            this.cbGr_work_day_num.Location = new System.Drawing.Point(14, 68);
            this.cbGr_work_day_num.Name = "cbGr_work_day_num";
            this.cbGr_work_day_num.Size = new System.Drawing.Size(461, 23);
            this.cbGr_work_day_num.TabIndex = 38;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(11, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(391, 15);
            this.label14.TabIndex = 37;
            this.label14.Text = "Дата начала работы сотрудника по выбранному графику";
            // 
            // deGr_work_date_begin
            // 
            this.deGr_work_date_begin.AutoSize = true;
            this.deGr_work_date_begin.BackColor = System.Drawing.SystemColors.Control;
            this.deGr_work_date_begin.Date = null;
            this.deGr_work_date_begin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deGr_work_date_begin.Location = new System.Drawing.Point(404, 18);
            this.deGr_work_date_begin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.deGr_work_date_begin.Name = "deGr_work_date_begin";
            this.deGr_work_date_begin.ReadOnly = false;
            this.deGr_work_date_begin.Size = new System.Drawing.Size(71, 21);
            this.deGr_work_date_begin.TabIndex = 2;
            this.deGr_work_date_begin.TextDate = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1001, 470);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgGR_Work);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 464);
            this.panel2.TabIndex = 7;
            // 
            // dgGR_Work
            // 
            this.dgGR_Work.AllowUserToAddRows = false;
            this.dgGR_Work.AllowUserToDeleteRows = false;
            this.dgGR_Work.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgGR_Work.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgGR_Work.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGR_Work.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgGR_Work.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgGR_Work.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgGR_Work.Location = new System.Drawing.Point(0, 48);
            this.dgGR_Work.Margin = new System.Windows.Forms.Padding(0);
            this.dgGR_Work.Name = "dgGR_Work";
            this.dgGR_Work.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGR_Work.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgGR_Work.RowHeadersWidth = 24;
            this.dgGR_Work.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgGR_Work.Size = new System.Drawing.Size(494, 310);
            this.dgGR_Work.TabIndex = 62;
            this.dgGR_Work.SelectionChanged += new System.EventHandler(this.dgGR_Work_SelectionChanged);
            this.dgGR_Work.DoubleClick += new System.EventHandler(this.dgGR_Work_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpDate_End_Graph);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(494, 48);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            // 
            // dtpDate_End_Graph
            // 
            this.dtpDate_End_Graph.Checked = false;
            this.dtpDate_End_Graph.CustomFormat = "";
            this.dtpDate_End_Graph.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate_End_Graph.Location = new System.Drawing.Point(332, 19);
            this.dtpDate_End_Graph.Name = "dtpDate_End_Graph";
            this.dtpDate_End_Graph.ShowCheckBox = true;
            this.dtpDate_End_Graph.Size = new System.Drawing.Size(143, 20);
            this.dtpDate_End_Graph.TabIndex = 38;
            this.dtpDate_End_Graph.ValueChanged += new System.EventHandler(this.dtpDate_End_Graph_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(11, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 15);
            this.label2.TabIndex = 37;
            this.label2.Text = "Фильтр по дате окончания работы графика";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgHistoryGraph);
            this.panel3.Controls.Add(this.pnWorkReg_Doc);
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(503, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(495, 464);
            this.panel3.TabIndex = 8;
            // 
            // dgHistoryGraph
            // 
            this.dgHistoryGraph.AllowUserToAddRows = false;
            this.dgHistoryGraph.AllowUserToDeleteRows = false;
            this.dgHistoryGraph.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgHistoryGraph.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgHistoryGraph.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgHistoryGraph.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgHistoryGraph.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgHistoryGraph.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgHistoryGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgHistoryGraph.Location = new System.Drawing.Point(0, 48);
            this.dgHistoryGraph.Margin = new System.Windows.Forms.Padding(0);
            this.dgHistoryGraph.Name = "dgHistoryGraph";
            this.dgHistoryGraph.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgHistoryGraph.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgHistoryGraph.RowHeadersWidth = 24;
            this.dgHistoryGraph.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgHistoryGraph.Size = new System.Drawing.Size(468, 416);
            this.dgHistoryGraph.TabIndex = 59;
            // 
            // pnWorkReg_Doc
            // 
            this.pnWorkReg_Doc.BackColor = System.Drawing.SystemColors.Control;
            this.pnWorkReg_Doc.Controls.Add(this.tsbDeleteGR_Work);
            this.pnWorkReg_Doc.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnWorkReg_Doc.Location = new System.Drawing.Point(468, 48);
            this.pnWorkReg_Doc.Name = "pnWorkReg_Doc";
            this.pnWorkReg_Doc.Size = new System.Drawing.Size(27, 416);
            this.pnWorkReg_Doc.TabIndex = 58;
            // 
            // tsbDeleteGR_Work
            // 
            this.tsbDeleteGR_Work.BackgroundImage = global::KadrWPF.Properties.Resources.Remove;
            this.tsbDeleteGR_Work.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tsbDeleteGR_Work.Dock = System.Windows.Forms.DockStyle.Top;
            this.tsbDeleteGR_Work.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.tsbDeleteGR_Work.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tsbDeleteGR_Work.Location = new System.Drawing.Point(0, 0);
            this.tsbDeleteGR_Work.Name = "tsbDeleteGR_Work";
            this.tsbDeleteGR_Work.Size = new System.Drawing.Size(27, 26);
            this.tsbDeleteGR_Work.TabIndex = 8;
            this.toolTip1.SetToolTip(this.tsbDeleteGR_Work, "Удаление графика работы");
            this.tsbDeleteGR_Work.UseVisualStyleBackColor = true;
            this.tsbDeleteGR_Work.Click += new System.EventHandler(this.tsbDeleteGR_Work_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudYearFilterHistoryGraph);
            this.groupBox3.Controls.Add(this.chFilterToYearHistoryGraph);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(495, 48);
            this.groupBox3.TabIndex = 62;
            this.groupBox3.TabStop = false;
            // 
            // nudYearFilterHistoryGraph
            // 
            this.nudYearFilterHistoryGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nudYearFilterHistoryGraph.Location = new System.Drawing.Point(250, 18);
            this.nudYearFilterHistoryGraph.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudYearFilterHistoryGraph.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudYearFilterHistoryGraph.Name = "nudYearFilterHistoryGraph";
            this.nudYearFilterHistoryGraph.Size = new System.Drawing.Size(53, 21);
            this.nudYearFilterHistoryGraph.TabIndex = 14;
            this.nudYearFilterHistoryGraph.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // chFilterToYearHistoryGraph
            // 
            this.chFilterToYearHistoryGraph.AutoSize = true;
            this.chFilterToYearHistoryGraph.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chFilterToYearHistoryGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chFilterToYearHistoryGraph.Location = new System.Drawing.Point(27, 19);
            this.chFilterToYearHistoryGraph.Name = "chFilterToYearHistoryGraph";
            this.chFilterToYearHistoryGraph.Size = new System.Drawing.Size(208, 19);
            this.chFilterToYearHistoryGraph.TabIndex = 13;
            this.chFilterToYearHistoryGraph.Text = "Отключить фильтр по году";
            this.chFilterToYearHistoryGraph.UseVisualStyleBackColor = true;
            // 
            // EditGr_Work
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(1001, 507);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditGr_Work";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор графика работы";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgGR_Work)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgHistoryGraph)).EndInit();
            this.pnWorkReg_Doc.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYearFilterHistoryGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSaveGR;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        public LibraryKadr.DateEditor deGr_work_date_begin;
        public System.Windows.Forms.ComboBox cbGr_work_day_num;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgHistoryGraph;
        private System.Windows.Forms.Panel pnWorkReg_Doc;
        private System.Windows.Forms.Button tsbDeleteGR_Work;
        private System.Windows.Forms.Button btViewGr_Work;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgGR_Work;
        private System.Windows.Forms.DateTimePicker dtpDate_End_Graph;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudYearFilterHistoryGraph;
        private System.Windows.Forms.CheckBox chFilterToYearHistoryGraph;
    }
}