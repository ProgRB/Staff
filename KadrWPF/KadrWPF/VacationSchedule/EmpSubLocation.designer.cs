namespace VacationSchedule
{
    partial class EmpSubLocation
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgEmps = new System.Windows.Forms.DataGridView();
            this.dgclPerNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgclFIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgclSignComb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgclCodeDegree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgclPosName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.tsbtRefreshEmps = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsdpCurDate = new LibraryKadr.ToolStripDateTimePicker();
            this.dgWorkerRegion = new System.Windows.Forms.DataGridView();
            this.dgclRegionWorker = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgclDateStart = new LibraryKadr.MDataGridViewCalendarColumn();
            this.dgclDateEnd = new LibraryKadr.MDataGridViewCalendarColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsbtAddWorkerRegionVS = new System.Windows.Forms.ToolStripButton();
            this.tsbtDeleteWorkerRegionVS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtSaveWorkerRegionVS = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgRegions = new System.Windows.Forms.DataGridView();
            this.dgclRegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgclCodeRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgclDateStartRegion = new LibraryKadr.MDataGridViewCalendarColumn();
            this.dgclDateEndRegion = new LibraryKadr.MDataGridViewCalendarColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtNewRegionVS = new System.Windows.Forms.ToolStripButton();
            this.tsbtDeleteRegionVS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtSaveRegionsVac = new System.Windows.Forms.ToolStripButton();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.subdivSelector1 = new LibraryKadr.Helpers.SubdivSelector();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmps)).BeginInit();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWorkerRegion)).BeginInit();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRegions)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 668);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сотрудники";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgEmps);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgWorkerRegion);
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer2.Size = new System.Drawing.Size(755, 649);
            this.splitContainer2.SplitterDistance = 467;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // dgEmps
            // 
            this.dgEmps.AllowUserToAddRows = false;
            this.dgEmps.AllowUserToDeleteRows = false;
            this.dgEmps.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgEmps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgclPerNum,
            this.dgclFIO,
            this.dgclSignComb,
            this.dgclCodeDegree,
            this.dgclPosName});
            this.dgEmps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEmps.Location = new System.Drawing.Point(0, 29);
            this.dgEmps.MultiSelect = false;
            this.dgEmps.Name = "dgEmps";
            this.dgEmps.ReadOnly = true;
            this.dgEmps.RowHeadersVisible = false;
            this.dgEmps.Size = new System.Drawing.Size(755, 438);
            this.dgEmps.TabIndex = 0;
            this.dgEmps.SelectionChanged += new System.EventHandler(this.dgEmps_SelectionChanged);
            // 
            // dgclPerNum
            // 
            this.dgclPerNum.DataPropertyName = "PER_NUM";
            this.dgclPerNum.HeaderText = "Таб.№";
            this.dgclPerNum.Name = "dgclPerNum";
            this.dgclPerNum.ReadOnly = true;
            this.dgclPerNum.Width = 50;
            // 
            // dgclFIO
            // 
            this.dgclFIO.DataPropertyName = "FIO";
            this.dgclFIO.HeaderText = "ФИО";
            this.dgclFIO.Name = "dgclFIO";
            this.dgclFIO.ReadOnly = true;
            this.dgclFIO.Width = 150;
            // 
            // dgclSignComb
            // 
            this.dgclSignComb.DataPropertyName = "SIGN_COMB";
            this.dgclSignComb.HeaderText = "Совмещ.";
            this.dgclSignComb.Name = "dgclSignComb";
            this.dgclSignComb.ReadOnly = true;
            this.dgclSignComb.Width = 30;
            // 
            // dgclCodeDegree
            // 
            this.dgclCodeDegree.DataPropertyName = "CODE_DEGREE";
            this.dgclCodeDegree.HeaderText = "Категория";
            this.dgclCodeDegree.Name = "dgclCodeDegree";
            this.dgclCodeDegree.ReadOnly = true;
            this.dgclCodeDegree.Width = 30;
            // 
            // dgclPosName
            // 
            this.dgclPosName.DataPropertyName = "POS_NAME";
            this.dgclPosName.HeaderText = "Должность";
            this.dgclPosName.Name = "dgclPosName";
            this.dgclPosName.ReadOnly = true;
            this.dgclPosName.Width = 250;
            // 
            // toolStrip3
            // 
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtRefreshEmps,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.tsdpCurDate});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(755, 29);
            this.toolStrip3.TabIndex = 1;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // tsbtRefreshEmps
            // 
            this.tsbtRefreshEmps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtRefreshEmps.Image = global::KadrWPF.Properties.Resources.gtk_refresh;
            this.tsbtRefreshEmps.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtRefreshEmps.Name = "tsbtRefreshEmps";
            this.tsbtRefreshEmps.Size = new System.Drawing.Size(23, 26);
            this.tsbtRefreshEmps.Text = "toolStripButton1";
            this.tsbtRefreshEmps.ToolTipText = "Обновить список сотрудников";
            this.tsbtRefreshEmps.Click += new System.EventHandler(this.tsbtRefreshEmps_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(118, 26);
            this.toolStripLabel1.Text = "Сотрудники на дату:";
            // 
            // tsdpCurDate
            // 
            this.tsdpCurDate.BackColor = System.Drawing.Color.Transparent;
            this.tsdpCurDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsdpCurDate.Name = "tsdpCurDate";
            this.tsdpCurDate.Size = new System.Drawing.Size(206, 26);
            this.tsdpCurDate.Text = "toolStripDateTimePicker1";
            this.tsdpCurDate.ToolTipText = "Дата, на которую отображаются работники подразделения";
            this.tsdpCurDate.Value = new System.DateTime(2014, 8, 12, 10, 5, 30, 552);
            // 
            // dgWorkerRegion
            // 
            this.dgWorkerRegion.AllowUserToAddRows = false;
            this.dgWorkerRegion.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgWorkerRegion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWorkerRegion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgclRegionWorker,
            this.dgclDateStart,
            this.dgclDateEnd});
            this.dgWorkerRegion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgWorkerRegion.Location = new System.Drawing.Point(70, 0);
            this.dgWorkerRegion.Name = "dgWorkerRegion";
            this.dgWorkerRegion.RowHeadersWidth = 25;
            this.dgWorkerRegion.Size = new System.Drawing.Size(685, 176);
            this.dgWorkerRegion.TabIndex = 1;
            this.dgWorkerRegion.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgWorkerRegion_CurrentCellDirtyStateChanged);
            this.dgWorkerRegion.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgWorkerRegion_DataError);
            // 
            // dgclRegionWorker
            // 
            this.dgclRegionWorker.DataPropertyName = "REGION_SUBDIV_ID";
            this.dgclRegionWorker.HeaderText = "Участок/бюро/группа";
            this.dgclRegionWorker.Name = "dgclRegionWorker";
            this.dgclRegionWorker.Width = 150;
            // 
            // dgclDateStart
            // 
            this.dgclDateStart.DataPropertyName = "DATE_START_WORK";
            this.dgclDateStart.DateFormat = "dd.MM.yyyy";
            this.dgclDateStart.HeaderText = "Дата начала работы на участке";
            this.dgclDateStart.Name = "dgclDateStart";
            // 
            // dgclDateEnd
            // 
            this.dgclDateEnd.DataPropertyName = "DATE_END_WORK";
            this.dgclDateEnd.DateFormat = "dd.MM.yyyy";
            this.dgclDateEnd.HeaderText = "Дата окончания работы";
            this.dgclDateEnd.Name = "dgclDateEnd";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtAddWorkerRegionVS,
            this.tsbtDeleteWorkerRegionVS,
            this.toolStripSeparator1,
            this.tsbtSaveWorkerRegionVS});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(70, 176);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsbtAddWorkerRegionVS
            // 
            this.tsbtAddWorkerRegionVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtAddWorkerRegionVS.Name = "tsbtAddWorkerRegionVS";
            this.tsbtAddWorkerRegionVS.Size = new System.Drawing.Size(67, 19);
            this.tsbtAddWorkerRegionVS.Text = "Добавить";
            this.tsbtAddWorkerRegionVS.Click += new System.EventHandler(this.tsbtAddWorkerRegionVS_Click);
            // 
            // tsbtDeleteWorkerRegionVS
            // 
            this.tsbtDeleteWorkerRegionVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtDeleteWorkerRegionVS.Name = "tsbtDeleteWorkerRegionVS";
            this.tsbtDeleteWorkerRegionVS.Size = new System.Drawing.Size(67, 19);
            this.tsbtDeleteWorkerRegionVS.Text = "Удалить";
            this.tsbtDeleteWorkerRegionVS.Click += new System.EventHandler(this.tsbtDeleteWorkerRegionVS_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(67, 6);
            // 
            // tsbtSaveWorkerRegionVS
            // 
            this.tsbtSaveWorkerRegionVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtSaveWorkerRegionVS.Name = "tsbtSaveWorkerRegionVS";
            this.tsbtSaveWorkerRegionVS.Size = new System.Drawing.Size(67, 19);
            this.tsbtSaveWorkerRegionVS.Text = "Сохранить";
            this.tsbtSaveWorkerRegionVS.Click += new System.EventHandler(this.tsbtSaveWorkerRegionVS_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1183, 668);
            this.splitContainer1.SplitterDistance = 416;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgRegions);
            this.groupBox2.Controls.Add(this.elementHost1);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 668);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Участки";
            // 
            // dgRegions
            // 
            this.dgRegions.AllowUserToAddRows = false;
            this.dgRegions.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgRegions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRegions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgclRegionName,
            this.dgclCodeRegion,
            this.dgclDateStartRegion,
            this.dgclDateEndRegion});
            this.dgRegions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgRegions.Location = new System.Drawing.Point(3, 64);
            this.dgRegions.Name = "dgRegions";
            this.dgRegions.RowHeadersVisible = false;
            this.dgRegions.Size = new System.Drawing.Size(410, 601);
            this.dgRegions.TabIndex = 1;
            // 
            // dgclRegionName
            // 
            this.dgclRegionName.DataPropertyName = "REGION_NAME";
            this.dgclRegionName.HeaderText = "Наименование";
            this.dgclRegionName.Name = "dgclRegionName";
            this.dgclRegionName.ToolTipText = "Наименование участка/бюро/группы";
            this.dgclRegionName.Width = 200;
            // 
            // dgclCodeRegion
            // 
            this.dgclCodeRegion.DataPropertyName = "CODE_REGION";
            this.dgclCodeRegion.HeaderText = "Код участка/ бюро";
            this.dgclCodeRegion.Name = "dgclCodeRegion";
            this.dgclCodeRegion.Width = 60;
            // 
            // dgclDateStartRegion
            // 
            this.dgclDateStartRegion.DataPropertyName = "DATE_START_REG";
            this.dgclDateStartRegion.DateFormat = "dd.MM.yyyy";
            this.dgclDateStartRegion.HeaderText = "Дата начала действия";
            this.dgclDateStartRegion.Name = "dgclDateStartRegion";
            // 
            // dgclDateEndRegion
            // 
            this.dgclDateEndRegion.DataPropertyName = "DATE_END_REG";
            this.dgclDateEndRegion.DateFormat = "dd.MM.yyyy";
            this.dgclDateEndRegion.HeaderText = "Дата окончания действия";
            this.dgclDateEndRegion.Name = "dgclDateEndRegion";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtNewRegionVS,
            this.tsbtDeleteRegionVS,
            this.toolStripSeparator3,
            this.tsbtSaveRegionsVac});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(410, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtNewRegionVS
            // 
            this.tsbtNewRegionVS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtNewRegionVS.Image = global::KadrWPF.Properties.Resources.document_new_1616;
            this.tsbtNewRegionVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtNewRegionVS.Name = "tsbtNewRegionVS";
            this.tsbtNewRegionVS.Size = new System.Drawing.Size(23, 22);
            this.tsbtNewRegionVS.Text = "toolStripButton4";
            this.tsbtNewRegionVS.ToolTipText = "Добавить новый участок/бюро/группу";
            this.tsbtNewRegionVS.Click += new System.EventHandler(this.tsbtNewRegionVS_Click);
            // 
            // tsbtDeleteRegionVS
            // 
            this.tsbtDeleteRegionVS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtDeleteRegionVS.Image = global::KadrWPF.Properties.Resources.Delete_Small;
            this.tsbtDeleteRegionVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtDeleteRegionVS.Name = "tsbtDeleteRegionVS";
            this.tsbtDeleteRegionVS.Size = new System.Drawing.Size(23, 22);
            this.tsbtDeleteRegionVS.Text = "toolStripButton5";
            this.tsbtDeleteRegionVS.ToolTipText = "Удалить выбранный участок";
            this.tsbtDeleteRegionVS.Click += new System.EventHandler(this.tsbtDeleteRegionVS_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtSaveRegionsVac
            // 
            this.tsbtSaveRegionsVac.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtSaveRegionsVac.Image = global::KadrWPF.Properties.Resources.document_save;
            this.tsbtSaveRegionsVac.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtSaveRegionsVac.Name = "tsbtSaveRegionsVac";
            this.tsbtSaveRegionsVac.Size = new System.Drawing.Size(23, 22);
            this.tsbtSaveRegionsVac.Text = "toolStripButton2";
            this.tsbtSaveRegionsVac.ToolTipText = "Сохранить изменения";
            this.tsbtSaveRegionsVac.Click += new System.EventHandler(this.tsbtSaveRegionsVac_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Top;
            this.elementHost1.Location = new System.Drawing.Point(3, 41);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(410, 23);
            this.elementHost1.TabIndex = 2;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.subdivSelector1;
            // 
            // EmpSubLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 668);
            this.Controls.Add(this.splitContainer1);
            this.Name = "EmpSubLocation";
            this.Text = "Участки сотрудников";
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmps)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWorkerRegion)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRegions)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbtAddWorkerRegionVS;
        private System.Windows.Forms.ToolStripButton tsbtDeleteWorkerRegionVS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtSaveWorkerRegionVS;
        private System.Windows.Forms.DataGridView dgWorkerRegion;
        private System.Windows.Forms.DataGridView dgEmps;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclPerNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclFIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclSignComb;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclCodeDegree;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclPosName;
        private System.Windows.Forms.DataGridView dgRegions;
        private System.Windows.Forms.ToolStripButton tsbtNewRegionVS;
        private System.Windows.Forms.ToolStripButton tsbtDeleteRegionVS;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton tsbtRefreshEmps;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private LibraryKadr.ToolStripDateTimePicker tsdpCurDate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbtSaveRegionsVac;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclRegionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgclCodeRegion;
        private LibraryKadr.MDataGridViewCalendarColumn dgclDateStartRegion;
        private LibraryKadr.MDataGridViewCalendarColumn dgclDateEndRegion;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgclRegionWorker;
        private LibraryKadr.MDataGridViewCalendarColumn dgclDateStart;
        private LibraryKadr.MDataGridViewCalendarColumn dgclDateEnd;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private LibraryKadr.Helpers.SubdivSelector subdivSelector1;
    }
}