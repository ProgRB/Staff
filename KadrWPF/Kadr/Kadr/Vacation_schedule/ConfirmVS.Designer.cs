namespace Kadr.Vacation_schedule
{
    partial class ConfirmVS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmVS));
            this.gridConfirmVS = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsbtPrivateKard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtEditVs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.only_conf = new System.Windows.Forms.CheckBox();
            this.panel_conf_commmands = new System.Windows.Forms.ToolStrip();
            this.btAddVS = new System.Windows.Forms.ToolStripButton();
            this.btEditVS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btPrivateKardVS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btConfirmVS = new System.Windows.Forms.ToolStripButton();
            this.btCancelConfirmVS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtRefreshConfVS = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtAllSubdivVacConfirmStatistic = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfirmVS)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel_conf_commmands.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridConfirmVS
            // 
            this.gridConfirmVS.AllowUserToAddRows = false;
            this.gridConfirmVS.AllowUserToDeleteRows = false;
            this.gridConfirmVS.AllowUserToResizeRows = false;
            this.gridConfirmVS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridConfirmVS.BackgroundColor = System.Drawing.Color.White;
            this.gridConfirmVS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridConfirmVS.ContextMenuStrip = this.contextMenuStrip1;
            this.gridConfirmVS.Location = new System.Drawing.Point(3, 36);
            this.gridConfirmVS.MultiSelect = false;
            this.gridConfirmVS.Name = "gridConfirmVS";
            this.gridConfirmVS.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue;
            this.gridConfirmVS.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridConfirmVS.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gridConfirmVS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridConfirmVS.Size = new System.Drawing.Size(936, 484);
            this.gridConfirmVS.TabIndex = 0;
            this.gridConfirmVS.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridConfirmVS_CellDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtPrivateKard,
            this.toolStripMenuItem1,
            this.tsbtEditVs,
            this.toolStripSeparator1,
            this.tsbtRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 82);
            // 
            // tsbtPrivateKard
            // 
            this.tsbtPrivateKard.BackColor = System.Drawing.SystemColors.Control;
            this.tsbtPrivateKard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsbtPrivateKard.Image = ((System.Drawing.Image)(resources.GetObject("tsbtPrivateKard.Image")));
            this.tsbtPrivateKard.Name = "tsbtPrivateKard";
            this.tsbtPrivateKard.Size = new System.Drawing.Size(202, 22);
            this.tsbtPrivateKard.Text = "Личная карточка";
            this.tsbtPrivateKard.Click += new System.EventHandler(this.личнаяКарточкаToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(199, 6);
            // 
            // tsbtEditVs
            // 
            this.tsbtEditVs.Image = ((System.Drawing.Image)(resources.GetObject("tsbtEditVs.Image")));
            this.tsbtEditVs.Name = "tsbtEditVs";
            this.tsbtEditVs.Size = new System.Drawing.Size(202, 22);
            this.tsbtEditVs.Text = "Редактировать отпуск";
            this.tsbtEditVs.Click += new System.EventHandler(this.btEditVS_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // tsbtRefresh
            // 
            this.tsbtRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbtRefresh.Image")));
            this.tsbtRefresh.Name = "tsbtRefresh";
            this.tsbtRefresh.Size = new System.Drawing.Size(202, 22);
            this.tsbtRefresh.Text = "Обновить";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.BackColor = System.Drawing.SystemColors.Control;
            this.checkAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkAll.Location = new System.Drawing.Point(7, 11);
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(121, 19);
            this.checkAll.TabIndex = 2;
            this.checkAll.Text = "Отметить все";
            this.checkAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkAll.UseVisualStyleBackColor = false;
            this.checkAll.CheckedChanged += new System.EventHandler(this.checkAll_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridConfirmVS);
            this.groupBox1.Controls.Add(this.only_conf);
            this.groupBox1.Controls.Add(this.checkAll);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(942, 526);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // only_conf
            // 
            this.only_conf.AutoSize = true;
            this.only_conf.BackColor = System.Drawing.SystemColors.Control;
            this.only_conf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.only_conf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.only_conf.Location = new System.Drawing.Point(143, 11);
            this.only_conf.Name = "only_conf";
            this.only_conf.Size = new System.Drawing.Size(277, 19);
            this.only_conf.TabIndex = 2;
            this.only_conf.Text = "Показывать только неутвержденные";
            this.only_conf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.only_conf.UseVisualStyleBackColor = false;
            this.only_conf.CheckedChanged += new System.EventHandler(this.only_conf_CheckedChanged);
            // 
            // panel_conf_commmands
            // 
            this.panel_conf_commmands.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.panel_conf_commmands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btAddVS,
            this.btEditVS,
            this.toolStripSeparator4,
            this.btPrivateKardVS,
            this.toolStripSeparator2,
            this.btConfirmVS,
            this.btCancelConfirmVS,
            this.toolStripSeparator3,
            this.tsbtRefreshConfVS,
            this.toolStripSeparator5,
            this.tsbtAllSubdivVacConfirmStatistic});
            this.panel_conf_commmands.Location = new System.Drawing.Point(0, 0);
            this.panel_conf_commmands.Name = "panel_conf_commmands";
            this.panel_conf_commmands.Size = new System.Drawing.Size(942, 25);
            this.panel_conf_commmands.TabIndex = 3;
            this.panel_conf_commmands.Text = "toolStrip1";
            // 
            // btAddVS
            // 
            this.btAddVS.Image = ((System.Drawing.Image)(resources.GetObject("btAddVS.Image")));
            this.btAddVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAddVS.Name = "btAddVS";
            this.btAddVS.Size = new System.Drawing.Size(115, 22);
            this.btAddVS.Text = "Добавить отпуск";
            this.btAddVS.Click += new System.EventHandler(this.btAddVS_Click);
            // 
            // btEditVS
            // 
            this.btEditVS.Image = ((System.Drawing.Image)(resources.GetObject("btEditVS.Image")));
            this.btEditVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btEditVS.Name = "btEditVS";
            this.btEditVS.Size = new System.Drawing.Size(106, 22);
            this.btEditVS.Text = "Редактировать";
            this.btEditVS.Click += new System.EventHandler(this.btEditVS_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btPrivateKardVS
            // 
            this.btPrivateKardVS.Image = ((System.Drawing.Image)(resources.GetObject("btPrivateKardVS.Image")));
            this.btPrivateKardVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btPrivateKardVS.Name = "btPrivateKardVS";
            this.btPrivateKardVS.Size = new System.Drawing.Size(115, 22);
            this.btPrivateKardVS.Text = "Личная карточка";
            this.btPrivateKardVS.Click += new System.EventHandler(this.личнаяКарточкаToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btConfirmVS
            // 
            this.btConfirmVS.Image = global::Kadr.Properties.Resources.confirm1616;
            this.btConfirmVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btConfirmVS.Name = "btConfirmVS";
            this.btConfirmVS.Size = new System.Drawing.Size(92, 22);
            this.btConfirmVS.Text = "Согласовано";
            this.btConfirmVS.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // btCancelConfirmVS
            // 
            this.btCancelConfirmVS.Image = ((System.Drawing.Image)(resources.GetObject("btCancelConfirmVS.Image")));
            this.btCancelConfirmVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCancelConfirmVS.Name = "btCancelConfirmVS";
            this.btCancelConfirmVS.Size = new System.Drawing.Size(149, 22);
            this.btCancelConfirmVS.Text = "Отменить согласование";
            this.btCancelConfirmVS.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtRefreshConfVS
            // 
            this.tsbtRefreshConfVS.Image = global::Kadr.Properties.Resources.arrow_refresh_mini;
            this.tsbtRefreshConfVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtRefreshConfVS.Name = "tsbtRefreshConfVS";
            this.tsbtRefreshConfVS.Size = new System.Drawing.Size(77, 22);
            this.tsbtRefreshConfVS.Text = "Обновить";
            this.tsbtRefreshConfVS.Click += new System.EventHandler(this.FillGrid);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtAllSubdivVacConfirmStatistic
            // 
            this.tsbtAllSubdivVacConfirmStatistic.Image = global::Kadr.Properties.Resources.confirm_list;
            this.tsbtAllSubdivVacConfirmStatistic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtAllSubdivVacConfirmStatistic.Name = "tsbtAllSubdivVacConfirmStatistic";
            this.tsbtAllSubdivVacConfirmStatistic.Size = new System.Drawing.Size(195, 22);
            this.tsbtAllSubdivVacConfirmStatistic.Text = "Общая статистика утверждения";
            this.tsbtAllSubdivVacConfirmStatistic.Click += new System.EventHandler(this.tsbtAllSubdivVacConfirmStatistic_Click);
            // 
            // ConfirmVS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(942, 551);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel_conf_commmands);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "ConfirmVS";
            this.Text = "Согласование графиков отпусков:";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Confirm_Vac_Schedule_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.gridConfirmVS)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel_conf_commmands.ResumeLayout(false);
            this.panel_conf_commmands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridConfirmVS;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.CheckBox checkAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox only_conf;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsbtPrivateKard;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsbtEditVs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsbtRefresh;
        private System.Windows.Forms.ToolStrip panel_conf_commmands;
        private System.Windows.Forms.ToolStripButton btAddVS;
        private System.Windows.Forms.ToolStripButton btEditVS;
        private System.Windows.Forms.ToolStripButton btPrivateKardVS;
        private System.Windows.Forms.ToolStripButton btConfirmVS;
        private System.Windows.Forms.ToolStripButton btCancelConfirmVS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbtRefreshConfVS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbtAllSubdivVacConfirmStatistic;
    }
}