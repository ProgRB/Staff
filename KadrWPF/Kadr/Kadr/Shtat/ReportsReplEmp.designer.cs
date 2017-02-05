namespace Kadr.Shtat
{
    partial class ReportsReplEmp
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
            this.GridReplReports = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbCheckAll = new LibraryKadr.ToolStripCheckBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsLabelCountRows = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbOnlyNotClose = new LibraryKadr.ToolStripCheckBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsCBTypeList = new System.Windows.Forms.ToolStripComboBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.btFormReport = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.STip = new Elegant.Ui.ScreenTip();
            this.btOpenExcept = new System.Windows.Forms.Button();
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sub_sel_repl = new Kadr.Classes.SubdivSelector();
            ((System.ComponentModel.ISupportInitialize)(this.GridReplReports)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridReplReports
            // 
            this.GridReplReports.AllowUserToAddRows = false;
            this.GridReplReports.AllowUserToDeleteRows = false;
            this.GridReplReports.AllowUserToResizeRows = false;
            this.GridReplReports.BackgroundColor = System.Drawing.Color.White;
            this.GridReplReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridReplReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridReplReports.Location = new System.Drawing.Point(3, 42);
            this.GridReplReports.Name = "GridReplReports";
            this.GridReplReports.RowHeadersVisible = false;
            this.GridReplReports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.GridReplReports.Size = new System.Drawing.Size(856, 244);
            this.GridReplReports.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GridReplReports);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(862, 289);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Замещающие сотрудники по подразделению:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbCheckAll,
            this.toolStripSeparator1,
            this.tsLabelCountRows,
            this.toolStripSeparator2,
            this.tscbOnlyNotClose,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.tsCBTypeList});
            this.toolStrip1.Location = new System.Drawing.Point(3, 17);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(856, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbCheckAll
            // 
            this.cbCheckAll.BackColor = System.Drawing.Color.Transparent;
            this.cbCheckAll.Checked = false;
            this.cbCheckAll.Name = "cbCheckAll";
            this.cbCheckAll.Size = new System.Drawing.Size(96, 22);
            this.cbCheckAll.Text = "Отметить все";
            this.cbCheckAll.CheckedChanged += new System.EventHandler(this.cbCheckAll_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsLabelCountRows
            // 
            this.tsLabelCountRows.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsLabelCountRows.Name = "tsLabelCountRows";
            this.tsLabelCountRows.Size = new System.Drawing.Size(143, 22);
            this.tsLabelCountRows.Text = "Сотрудников выбрано:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tscbOnlyNotClose
            // 
            this.tscbOnlyNotClose.BackColor = System.Drawing.Color.Transparent;
            this.tscbOnlyNotClose.Checked = false;
            this.tscbOnlyNotClose.Name = "tscbOnlyNotClose";
            this.tscbOnlyNotClose.Size = new System.Drawing.Size(168, 22);
            this.tscbOnlyNotClose.Text = "Показать только открытые";
            this.tscbOnlyNotClose.CheckedChanged += new System.EventHandler(this.tscbOnlyNotClose_CheckedChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(93, 22);
            this.toolStripLabel1.Text = "Тип документов:";
            // 
            // tsCBTypeList
            // 
            this.tsCBTypeList.AutoSize = false;
            this.tsCBTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsCBTypeList.Items.AddRange(new object[] {
            "Замещения",
            "Совмещения"});
            this.tsCBTypeList.Name = "tsCBTypeList";
            this.tsCBTypeList.Size = new System.Drawing.Size(140, 21);
            this.tsCBTypeList.ToolTipText = "Тип документов";
            this.tsCBTypeList.SelectedIndexChanged += new System.EventHandler(this.tscbOnlyNotClose_CheckedChanged);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // btFormReport
            // 
            this.btFormReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFormReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFormReport.ForeColor = System.Drawing.Color.Blue;
            this.btFormReport.Image = global::Kadr.Properties.Resources.next_gray3232;
            this.btFormReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btFormReport.Location = new System.Drawing.Point(674, 7);
            this.btFormReport.Name = "btFormReport";
            this.btFormReport.Size = new System.Drawing.Size(87, 24);
            this.btFormReport.TabIndex = 3;
            this.btFormReport.Text = "Далее";
            this.btFormReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btFormReport.UseVisualStyleBackColor = true;
            this.btFormReport.Click += new System.EventHandler(this.btFormReport_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(776, 7);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(77, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Выход";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOpenExcept
            // 
            this.btOpenExcept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOpenExcept.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOpenExcept.ForeColor = System.Drawing.Color.Blue;
            this.btOpenExcept.Location = new System.Drawing.Point(12, 7);
            this.btOpenExcept.Name = "btOpenExcept";
            this.btOpenExcept.Size = new System.Drawing.Size(194, 22);
            this.btOpenExcept.TabIndex = 3;
            this.btOpenExcept.Text = "Исключения склонений";
            this.btOpenExcept.UseVisualStyleBackColor = true;
            this.btOpenExcept.Click += new System.EventHandler(this.btOpenExcept_Click);
            // 
            // dateStart
            // 
            this.dateStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateStart.Location = new System.Drawing.Point(177, 30);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(197, 21);
            this.dateStart.TabIndex = 4;
            this.dateStart.ValueChanged += new System.EventHandler(this.ReportServiceRecordRepl_Load);
            // 
            // dateEnd
            // 
            this.dateEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateEnd.Location = new System.Drawing.Point(404, 30);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Size = new System.Drawing.Size(200, 21);
            this.dateEnd.TabIndex = 4;
            this.dateEnd.ValueChanged += new System.EventHandler(this.ReportServiceRecordRepl_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(31, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Замещения за период c";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.sub_sel_repl);
            this.panel1.Controls.Add(this.dateStart);
            this.panel1.Controls.Add(this.dateEnd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(862, 60);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(380, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "по";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btCancel);
            this.panel2.Controls.Add(this.btOpenExcept);
            this.panel2.Controls.Add(this.btFormReport);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 349);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(862, 37);
            this.panel2.TabIndex = 4;
            // 
            // sub_sel_repl
            // 
            this.sub_sel_repl.BackColor = System.Drawing.Color.Transparent;
            this.sub_sel_repl.ByRule = "";
            this.sub_sel_repl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sub_sel_repl.IsAllPlantSelectable = true;
            this.sub_sel_repl.Location = new System.Drawing.Point(76, 4);
            this.sub_sel_repl.Name = "sub_sel_repl";
            this.sub_sel_repl.Size = new System.Drawing.Size(528, 21);
            this.sub_sel_repl.subdiv_id = null;
            this.sub_sel_repl.TabIndex = 6;
            // 
            // ReportsReplEmp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(862, 386);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "ReportsReplEmp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выберите записи для формирования служебной";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReportServiceRecordRepl_FormClosed);
            this.Load += new System.EventHandler(this.ReportServiceRecordRepl_Load);
            this.Shown += new System.EventHandler(this.ReportsReplEmp_Shown);
            this.EnabledChanged += new System.EventHandler(this.ReportServiceRecordRepl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridReplReports)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GridReplReports;
        private System.Windows.Forms.GroupBox groupBox1;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btFormReport;
        private Elegant.Ui.ScreenTip STip;
        private System.Windows.Forms.Button btOpenExcept;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Kadr.Classes.SubdivSelector sub_sel_repl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private LibraryKadr.ToolStripCheckBox cbCheckAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tsLabelCountRows;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private LibraryKadr.ToolStripCheckBox tscbOnlyNotClose;
        private System.Windows.Forms.ToolStripComboBox tsCBTypeList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}