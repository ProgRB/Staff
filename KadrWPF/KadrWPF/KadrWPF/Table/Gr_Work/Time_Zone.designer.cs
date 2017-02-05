namespace Tabel
{
    partial class Time_Zone
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Time_Zone));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSelectTime_Zone = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgTime_Zone = new System.Windows.Forms.DataGridView();
            this.tsTime_Zone = new System.Windows.Forms.ToolStrip();
            this.tsbAddTime_Zone = new System.Windows.Forms.ToolStripButton();
            this.tsbEditTime_Zone = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteTime_Zone = new System.Windows.Forms.ToolStripButton();
            this.dgTime_Interval = new System.Windows.Forms.DataGridView();
            this.tsTime_Interval = new System.Windows.Forms.ToolStrip();
            this.btAddTime_Interval = new System.Windows.Forms.ToolStripButton();
            this.btEditTime_Interval = new System.Windows.Forms.ToolStripButton();
            this.btDeleteTime_Interval = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgGr_Work = new System.Windows.Forms.DataGridView();
            this.GR_WORK_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE_SUBDIV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE_END_GRAPH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTime_Zone)).BeginInit();
            this.tsTime_Zone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTime_Interval)).BeginInit();
            this.tsTime_Interval.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgGr_Work)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btSelectTime_Zone);
            this.panel2.Controls.Add(this.btExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 402);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(772, 35);
            this.panel2.TabIndex = 75;
            // 
            // btSelectTime_Zone
            // 
            this.btSelectTime_Zone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectTime_Zone.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btSelectTime_Zone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSelectTime_Zone.Location = new System.Drawing.Point(594, 6);
            this.btSelectTime_Zone.Name = "btSelectTime_Zone";
            this.btSelectTime_Zone.Size = new System.Drawing.Size(75, 23);
            this.btSelectTime_Zone.TabIndex = 2;
            this.btSelectTime_Zone.Text = "Выбрать";
            this.btSelectTime_Zone.Visible = false;
            this.btSelectTime_Zone.Click += new System.EventHandler(this.btSelectTime_Zone_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(679, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgTime_Zone);
            this.splitContainer1.Panel1.Controls.Add(this.tsTime_Zone);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.dgTime_Interval);
            this.splitContainer1.Panel2.Controls.Add(this.tsTime_Interval);
            this.splitContainer1.Size = new System.Drawing.Size(772, 402);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 76;
            // 
            // dgTime_Zone
            // 
            this.dgTime_Zone.AllowUserToAddRows = false;
            this.dgTime_Zone.AllowUserToDeleteRows = false;
            this.dgTime_Zone.BackgroundColor = System.Drawing.Color.White;
            this.dgTime_Zone.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTime_Zone.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgTime_Zone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTime_Zone.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgTime_Zone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTime_Zone.Location = new System.Drawing.Point(0, 25);
            this.dgTime_Zone.Name = "dgTime_Zone";
            this.dgTime_Zone.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTime_Zone.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgTime_Zone.RowHeadersWidth = 24;
            this.dgTime_Zone.Size = new System.Drawing.Size(264, 377);
            this.dgTime_Zone.TabIndex = 61;
            // 
            // tsTime_Zone
            // 
            this.tsTime_Zone.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddTime_Zone,
            this.tsbEditTime_Zone,
            this.tsbDeleteTime_Zone});
            this.tsTime_Zone.Location = new System.Drawing.Point(0, 0);
            this.tsTime_Zone.Name = "tsTime_Zone";
            this.tsTime_Zone.Size = new System.Drawing.Size(264, 25);
            this.tsTime_Zone.TabIndex = 60;
            this.tsTime_Zone.Text = "toolStrip1";
            // 
            // tsbAddTime_Zone
            // 
            this.tsbAddTime_Zone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddTime_Zone.Enabled = false;
            this.tsbAddTime_Zone.Image = global::KadrWPF.Properties.Resources.document_new_1616;
            this.tsbAddTime_Zone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddTime_Zone.Name = "tsbAddTime_Zone";
            this.tsbAddTime_Zone.Size = new System.Drawing.Size(23, 22);
            this.tsbAddTime_Zone.Text = "toolStripButton1";
            this.tsbAddTime_Zone.ToolTipText = "Добавить";
            this.tsbAddTime_Zone.Click += new System.EventHandler(this.tsbAddTime_Zone_Click);
            // 
            // tsbEditTime_Zone
            // 
            this.tsbEditTime_Zone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditTime_Zone.Enabled = false;
            this.tsbEditTime_Zone.Image = global::KadrWPF.Properties.Resources.table_edit_1616;
            this.tsbEditTime_Zone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditTime_Zone.Name = "tsbEditTime_Zone";
            this.tsbEditTime_Zone.Size = new System.Drawing.Size(23, 22);
            this.tsbEditTime_Zone.Text = "toolStripButton2";
            this.tsbEditTime_Zone.ToolTipText = "Редактировать";
            this.tsbEditTime_Zone.Click += new System.EventHandler(this.tsbEditTime_Zone_Click);
            // 
            // tsbDeleteTime_Zone
            // 
            this.tsbDeleteTime_Zone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteTime_Zone.Enabled = false;
            this.tsbDeleteTime_Zone.Image = global::KadrWPF.Properties.Resources.Delete;
            this.tsbDeleteTime_Zone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteTime_Zone.Name = "tsbDeleteTime_Zone";
            this.tsbDeleteTime_Zone.Size = new System.Drawing.Size(23, 22);
            this.tsbDeleteTime_Zone.Text = "toolStripButton3";
            this.tsbDeleteTime_Zone.ToolTipText = "Удалить";
            this.tsbDeleteTime_Zone.Click += new System.EventHandler(this.tsbDeleteTime_Zone_Click);
            // 
            // dgTime_Interval
            // 
            this.dgTime_Interval.AllowUserToAddRows = false;
            this.dgTime_Interval.AllowUserToDeleteRows = false;
            this.dgTime_Interval.BackgroundColor = System.Drawing.Color.White;
            this.dgTime_Interval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTime_Interval.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgTime_Interval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTime_Interval.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgTime_Interval.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgTime_Interval.Location = new System.Drawing.Point(0, 25);
            this.dgTime_Interval.Name = "dgTime_Interval";
            this.dgTime_Interval.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTime_Interval.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgTime_Interval.RowHeadersWidth = 24;
            this.dgTime_Interval.Size = new System.Drawing.Size(504, 194);
            this.dgTime_Interval.TabIndex = 64;
            // 
            // tsTime_Interval
            // 
            this.tsTime_Interval.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btAddTime_Interval,
            this.btEditTime_Interval,
            this.btDeleteTime_Interval});
            this.tsTime_Interval.Location = new System.Drawing.Point(0, 0);
            this.tsTime_Interval.Name = "tsTime_Interval";
            this.tsTime_Interval.Size = new System.Drawing.Size(504, 25);
            this.tsTime_Interval.TabIndex = 63;
            this.tsTime_Interval.Text = "toolStrip2";
            // 
            // btAddTime_Interval
            // 
            this.btAddTime_Interval.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAddTime_Interval.Enabled = false;
            this.btAddTime_Interval.Image = global::KadrWPF.Properties.Resources.document_new_1616;
            this.btAddTime_Interval.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAddTime_Interval.Name = "btAddTime_Interval";
            this.btAddTime_Interval.Size = new System.Drawing.Size(23, 22);
            this.btAddTime_Interval.Text = "toolStripButton1";
            this.btAddTime_Interval.ToolTipText = "Добавить";
            this.btAddTime_Interval.Click += new System.EventHandler(this.btAddTime_Interval_Click);
            // 
            // btEditTime_Interval
            // 
            this.btEditTime_Interval.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btEditTime_Interval.Enabled = false;
            this.btEditTime_Interval.Image = global::KadrWPF.Properties.Resources.table_edit_1616;
            this.btEditTime_Interval.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btEditTime_Interval.Name = "btEditTime_Interval";
            this.btEditTime_Interval.Size = new System.Drawing.Size(23, 22);
            this.btEditTime_Interval.Text = "toolStripButton2";
            this.btEditTime_Interval.ToolTipText = "Редактировать";
            this.btEditTime_Interval.Click += new System.EventHandler(this.btEditTime_Interval_Click);
            // 
            // btDeleteTime_Interval
            // 
            this.btDeleteTime_Interval.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDeleteTime_Interval.Enabled = false;
            this.btDeleteTime_Interval.Image = global::KadrWPF.Properties.Resources.Delete;
            this.btDeleteTime_Interval.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDeleteTime_Interval.Name = "btDeleteTime_Interval";
            this.btDeleteTime_Interval.Size = new System.Drawing.Size(23, 22);
            this.btDeleteTime_Interval.Text = "toolStripButton3";
            this.btDeleteTime_Interval.ToolTipText = "Удалить";
            this.btDeleteTime_Interval.Click += new System.EventHandler(this.btDeleteTime_Interval_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgGr_Work);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 219);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 183);
            this.groupBox1.TabIndex = 65;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Графики работы и подразделения";
            // 
            // dgGr_Work
            // 
            this.dgGr_Work.AllowUserToAddRows = false;
            this.dgGr_Work.AllowUserToDeleteRows = false;
            this.dgGr_Work.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgGr_Work.BackgroundColor = System.Drawing.Color.White;
            this.dgGr_Work.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGr_Work.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgGr_Work.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgGr_Work.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GR_WORK_NAME,
            this.CODE_SUBDIV,
            this.DATE_END_GRAPH});
            this.dgGr_Work.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgGr_Work.Location = new System.Drawing.Point(3, 17);
            this.dgGr_Work.Name = "dgGr_Work";
            this.dgGr_Work.ReadOnly = true;
            this.dgGr_Work.RowHeadersWidth = 24;
            this.dgGr_Work.Size = new System.Drawing.Size(498, 163);
            this.dgGr_Work.TabIndex = 0;
            // 
            // GR_WORK_NAME
            // 
            this.GR_WORK_NAME.DataPropertyName = "GR_WORK_NAME";
            this.GR_WORK_NAME.HeaderText = "Наименование графика работы";
            this.GR_WORK_NAME.Name = "GR_WORK_NAME";
            this.GR_WORK_NAME.ReadOnly = true;
            // 
            // CODE_SUBDIV
            // 
            this.CODE_SUBDIV.DataPropertyName = "CODE_SUBDIV";
            this.CODE_SUBDIV.HeaderText = "Подр.";
            this.CODE_SUBDIV.Name = "CODE_SUBDIV";
            this.CODE_SUBDIV.ReadOnly = true;
            // 
            // DATE_END_GRAPH
            // 
            this.DATE_END_GRAPH.DataPropertyName = "DATE_END_GRAPH";
            this.DATE_END_GRAPH.HeaderText = "Дата окончания";
            this.DATE_END_GRAPH.Name = "DATE_END_GRAPH";
            this.DATE_END_GRAPH.ReadOnly = true;
            // 
            // Time_Zone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 437);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Time_Zone";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Time_Zone";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTime_Zone)).EndInit();
            this.tsTime_Zone.ResumeLayout(false);
            this.tsTime_Zone.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTime_Interval)).EndInit();
            this.tsTime_Interval.ResumeLayout(false);
            this.tsTime_Interval.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgGr_Work)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgTime_Zone;
        private System.Windows.Forms.ToolStrip tsTime_Zone;
        private System.Windows.Forms.ToolStripButton tsbAddTime_Zone;
        private System.Windows.Forms.ToolStripButton tsbEditTime_Zone;
        private System.Windows.Forms.ToolStripButton tsbDeleteTime_Zone;
        private System.Windows.Forms.ToolStrip tsTime_Interval;
        private System.Windows.Forms.ToolStripButton btAddTime_Interval;
        private System.Windows.Forms.ToolStripButton btEditTime_Interval;
        private System.Windows.Forms.ToolStripButton btDeleteTime_Interval;
        private System.Windows.Forms.DataGridView dgTime_Interval;
        public System.Windows.Forms.Button btSelectTime_Zone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgGr_Work;
        private System.Windows.Forms.DataGridViewTextBoxColumn GR_WORK_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE_SUBDIV;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATE_END_GRAPH;
    }
}