namespace Kadr
{
    partial class HBConditionWork
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgCondition_Of_Work = new System.Windows.Forms.DataGridView();
            this.CONDITIONS_OF_WORK_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBCLASS_NUMBER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsCondition = new System.Windows.Forms.ToolStrip();
            this.tsbAddCondition = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDeleteCondition = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSaveCondition = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCancelCondition = new System.Windows.Forms.ToolStripButton();
            this.dgCondition_Of_Work_Percent = new System.Windows.Forms.DataGridView();
            this.CONDITIONS_OF_WORK_PERCENT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mDataGridViewCalendarColumn1 = new LibraryKadr.MDataGridViewCalendarColumn();
            this.mDataGridViewCalendarColumn2 = new LibraryKadr.MDataGridViewCalendarColumn();
            this.tsConditionPercent = new System.Windows.Forms.ToolStrip();
            this.tsbAddPercentCond = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDeletePercentCond = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSavePercentCond = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCancelPercentCond = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCondition_Of_Work)).BeginInit();
            this.tsCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCondition_Of_Work_Percent)).BeginInit();
            this.tsConditionPercent.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgCondition_Of_Work);
            this.splitContainer1.Panel1.Controls.Add(this.tsCondition);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgCondition_Of_Work_Percent);
            this.splitContainer1.Panel2.Controls.Add(this.tsConditionPercent);
            this.splitContainer1.Size = new System.Drawing.Size(816, 505);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgCondition_Of_Work
            // 
            this.dgCondition_Of_Work.AllowUserToAddRows = false;
            this.dgCondition_Of_Work.AllowUserToDeleteRows = false;
            this.dgCondition_Of_Work.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgCondition_Of_Work.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCondition_Of_Work.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCondition_Of_Work.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCondition_Of_Work.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CONDITIONS_OF_WORK_ID,
            this.SUBCLASS_NUMBER});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCondition_Of_Work.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgCondition_Of_Work.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCondition_Of_Work.Location = new System.Drawing.Point(0, 25);
            this.dgCondition_Of_Work.Name = "dgCondition_Of_Work";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCondition_Of_Work.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgCondition_Of_Work.RowHeadersWidth = 25;
            this.dgCondition_Of_Work.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgCondition_Of_Work.Size = new System.Drawing.Size(211, 480);
            this.dgCondition_Of_Work.TabIndex = 61;
            this.dgCondition_Of_Work.SelectionChanged += new System.EventHandler(this.dgCondition_Of_Work_SelectionChanged);
            // 
            // CONDITIONS_OF_WORK_ID
            // 
            this.CONDITIONS_OF_WORK_ID.DataPropertyName = "CONDITIONS_OF_WORK_ID";
            this.CONDITIONS_OF_WORK_ID.HeaderText = "CONDITIONS_OF_WORK_ID";
            this.CONDITIONS_OF_WORK_ID.Name = "CONDITIONS_OF_WORK_ID";
            this.CONDITIONS_OF_WORK_ID.Visible = false;
            // 
            // SUBCLASS_NUMBER
            // 
            this.SUBCLASS_NUMBER.DataPropertyName = "SUBCLASS_NUMBER";
            this.SUBCLASS_NUMBER.HeaderText = "№ подкласса";
            this.SUBCLASS_NUMBER.Name = "SUBCLASS_NUMBER";
            // 
            // tsCondition
            // 
            this.tsCondition.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsCondition.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddCondition,
            this.toolStripSeparator4,
            this.tsbDeleteCondition,
            this.toolStripSeparator5,
            this.tsbSaveCondition,
            this.toolStripSeparator6,
            this.tsbCancelCondition});
            this.tsCondition.Location = new System.Drawing.Point(0, 0);
            this.tsCondition.Name = "tsCondition";
            this.tsCondition.Size = new System.Drawing.Size(211, 25);
            this.tsCondition.TabIndex = 60;
            this.tsCondition.Text = "toolStrip2";
            // 
            // tsbAddCondition
            // 
            this.tsbAddCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddCondition.Image = global::KadrWPF.Properties.Resources.document_new_1616;
            this.tsbAddCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddCondition.Name = "tsbAddCondition";
            this.tsbAddCondition.Size = new System.Drawing.Size(23, 22);
            this.tsbAddCondition.Text = "Добавить подкласс условий труда";
            this.tsbAddCondition.Click += new System.EventHandler(this.tsbAddCondition_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDeleteCondition
            // 
            this.tsbDeleteCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteCondition.Image = global::KadrWPF.Properties.Resources.Remove_Small;
            this.tsbDeleteCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteCondition.Name = "tsbDeleteCondition";
            this.tsbDeleteCondition.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteCondition.Text = "toolStripButton2";
            this.tsbDeleteCondition.Click += new System.EventHandler(this.tsbDeleteCondition_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSaveCondition
            // 
            this.tsbSaveCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveCondition.Image = global::KadrWPF.Properties.Resources.document_save;
            this.tsbSaveCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveCondition.Name = "tsbSaveCondition";
            this.tsbSaveCondition.Size = new System.Drawing.Size(23, 22);
            this.tsbSaveCondition.Text = "toolStripButton3";
            this.tsbSaveCondition.Click += new System.EventHandler(this.tsbSaveCondition_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCancelCondition
            // 
            this.tsbCancelCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCancelCondition.Image = global::KadrWPF.Properties.Resources.UndoSmall;
            this.tsbCancelCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancelCondition.Name = "tsbCancelCondition";
            this.tsbCancelCondition.Size = new System.Drawing.Size(23, 22);
            this.tsbCancelCondition.Text = "toolStripButton4";
            this.tsbCancelCondition.Click += new System.EventHandler(this.tsbCancelCondition_Click);
            // 
            // dgCondition_Of_Work_Percent
            // 
            this.dgCondition_Of_Work_Percent.AllowUserToAddRows = false;
            this.dgCondition_Of_Work_Percent.AllowUserToDeleteRows = false;
            this.dgCondition_Of_Work_Percent.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgCondition_Of_Work_Percent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCondition_Of_Work_Percent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgCondition_Of_Work_Percent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCondition_Of_Work_Percent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CONDITIONS_OF_WORK_PERCENT_ID,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.mDataGridViewCalendarColumn1,
            this.mDataGridViewCalendarColumn2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCondition_Of_Work_Percent.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgCondition_Of_Work_Percent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCondition_Of_Work_Percent.Location = new System.Drawing.Point(0, 25);
            this.dgCondition_Of_Work_Percent.Name = "dgCondition_Of_Work_Percent";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCondition_Of_Work_Percent.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgCondition_Of_Work_Percent.RowHeadersWidth = 25;
            this.dgCondition_Of_Work_Percent.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgCondition_Of_Work_Percent.Size = new System.Drawing.Size(601, 480);
            this.dgCondition_Of_Work_Percent.TabIndex = 62;
            // 
            // CONDITIONS_OF_WORK_PERCENT_ID
            // 
            this.CONDITIONS_OF_WORK_PERCENT_ID.DataPropertyName = "CONDITIONS_OF_WORK_PERCENT_ID";
            this.CONDITIONS_OF_WORK_PERCENT_ID.HeaderText = "CONDITIONS_OF_WORK_PERCENT_ID";
            this.CONDITIONS_OF_WORK_PERCENT_ID.Name = "CONDITIONS_OF_WORK_PERCENT_ID";
            this.CONDITIONS_OF_WORK_PERCENT_ID.Visible = false;
            this.CONDITIONS_OF_WORK_PERCENT_ID.Width = 300;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CONDITIONS_OF_WORK_ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "CONDITIONS_OF_WORK_ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "PERCENT_ADD_RATE";
            this.dataGridViewTextBoxColumn3.HeaderText = "Доп.тариф страх-го взноса";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // mDataGridViewCalendarColumn1
            // 
            this.mDataGridViewCalendarColumn1.DataPropertyName = "DATE_START_PERCENT";
            this.mDataGridViewCalendarColumn1.DateFormat = "dd.MM.yyyy";
            this.mDataGridViewCalendarColumn1.HeaderText = "Дата начала действия процентов";
            this.mDataGridViewCalendarColumn1.Name = "mDataGridViewCalendarColumn1";
            this.mDataGridViewCalendarColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mDataGridViewCalendarColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.mDataGridViewCalendarColumn1.Width = 180;
            // 
            // mDataGridViewCalendarColumn2
            // 
            this.mDataGridViewCalendarColumn2.DataPropertyName = "DATE_END_PERCENT";
            this.mDataGridViewCalendarColumn2.DateFormat = "dd.MM.yyyy";
            this.mDataGridViewCalendarColumn2.HeaderText = "Дата окончания действия процентов";
            this.mDataGridViewCalendarColumn2.Name = "mDataGridViewCalendarColumn2";
            this.mDataGridViewCalendarColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mDataGridViewCalendarColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.mDataGridViewCalendarColumn2.Width = 200;
            // 
            // tsConditionPercent
            // 
            this.tsConditionPercent.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsConditionPercent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddPercentCond,
            this.toolStripSeparator1,
            this.tsbDeletePercentCond,
            this.toolStripSeparator2,
            this.tsbSavePercentCond,
            this.toolStripSeparator3,
            this.tsbCancelPercentCond});
            this.tsConditionPercent.Location = new System.Drawing.Point(0, 0);
            this.tsConditionPercent.Name = "tsConditionPercent";
            this.tsConditionPercent.Size = new System.Drawing.Size(601, 25);
            this.tsConditionPercent.TabIndex = 61;
            this.tsConditionPercent.Text = "toolStrip1";
            // 
            // tsbAddPercentCond
            // 
            this.tsbAddPercentCond.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddPercentCond.Image = global::KadrWPF.Properties.Resources.document_new_1616;
            this.tsbAddPercentCond.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddPercentCond.Name = "tsbAddPercentCond";
            this.tsbAddPercentCond.Size = new System.Drawing.Size(23, 22);
            this.tsbAddPercentCond.Text = "toolStripButton1";
            this.tsbAddPercentCond.Click += new System.EventHandler(this.tsbAddPercentCond_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDeletePercentCond
            // 
            this.tsbDeletePercentCond.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeletePercentCond.Image = global::KadrWPF.Properties.Resources.Remove_Small;
            this.tsbDeletePercentCond.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeletePercentCond.Name = "tsbDeletePercentCond";
            this.tsbDeletePercentCond.Size = new System.Drawing.Size(23, 22);
            this.tsbDeletePercentCond.Text = "toolStripButton2";
            this.tsbDeletePercentCond.Click += new System.EventHandler(this.tsbDeletePercentCond_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSavePercentCond
            // 
            this.tsbSavePercentCond.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSavePercentCond.Image = global::KadrWPF.Properties.Resources.document_save;
            this.tsbSavePercentCond.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSavePercentCond.Name = "tsbSavePercentCond";
            this.tsbSavePercentCond.Size = new System.Drawing.Size(23, 22);
            this.tsbSavePercentCond.Text = "toolStripButton3";
            this.tsbSavePercentCond.Click += new System.EventHandler(this.tsbSavePercentCond_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCancelPercentCond
            // 
            this.tsbCancelPercentCond.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCancelPercentCond.Image = global::KadrWPF.Properties.Resources.UndoSmall;
            this.tsbCancelPercentCond.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancelPercentCond.Name = "tsbCancelPercentCond";
            this.tsbCancelPercentCond.Size = new System.Drawing.Size(23, 22);
            this.tsbCancelPercentCond.Text = "toolStripButton4";
            this.tsbCancelPercentCond.Click += new System.EventHandler(this.tsbCancelPercentCond_Click);
            // 
            // HBConditionWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 505);
            this.Controls.Add(this.splitContainer1);
            this.Name = "HBConditionWork";
            this.Text = "Справочник условий труда";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCondition_Of_Work)).EndInit();
            this.tsCondition.ResumeLayout(false);
            this.tsCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCondition_Of_Work_Percent)).EndInit();
            this.tsConditionPercent.ResumeLayout(false);
            this.tsConditionPercent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.DataGridView dgCondition_Of_Work;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONDITIONS_OF_WORK_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBCLASS_NUMBER;
        private System.Windows.Forms.ToolStrip tsCondition;
        private System.Windows.Forms.ToolStripButton tsbAddCondition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbDeleteCondition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbSaveCondition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbCancelCondition;
        public System.Windows.Forms.DataGridView dgCondition_Of_Work_Percent;
        private System.Windows.Forms.ToolStrip tsConditionPercent;
        private System.Windows.Forms.ToolStripButton tsbAddPercentCond;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbDeletePercentCond;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbSavePercentCond;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbCancelPercentCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONDITIONS_OF_WORK_PERCENT_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private LibraryKadr.MDataGridViewCalendarColumn mDataGridViewCalendarColumn1;
        private LibraryKadr.MDataGridViewCalendarColumn mDataGridViewCalendarColumn2;
    }
}