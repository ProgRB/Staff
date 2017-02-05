namespace VacationSchedule
{
    partial class ConfirmPercentView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgSubdivConfirm = new System.Windows.Forms.DataGridView();
            this.btClose = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsYearCurrent = new LibraryKadr.ToolStripNumbericUpDown();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE_SUBDIV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBDIV_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ALL_COUNT_VAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CONFIRM_VACS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PERCENT_CONFIRM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSubdivConfirm)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgSubdivConfirm);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(3, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 363);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подразделения";
            // 
            // dgSubdivConfirm
            // 
            this.dgSubdivConfirm.AllowUserToAddRows = false;
            this.dgSubdivConfirm.AllowUserToDeleteRows = false;
            this.dgSubdivConfirm.AllowUserToResizeRows = false;
            this.dgSubdivConfirm.BackgroundColor = System.Drawing.Color.White;
            this.dgSubdivConfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSubdivConfirm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CODE_SUBDIV,
            this.SUBDIV_NAME,
            this.ALL_COUNT_VAC,
            this.CONFIRM_VACS,
            this.PERCENT_CONFIRM});
            this.dgSubdivConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSubdivConfirm.Location = new System.Drawing.Point(3, 17);
            this.dgSubdivConfirm.Name = "dgSubdivConfirm";
            this.dgSubdivConfirm.ReadOnly = true;
            this.dgSubdivConfirm.RowHeadersVisible = false;
            this.dgSubdivConfirm.Size = new System.Drawing.Size(537, 343);
            this.dgSubdivConfirm.TabIndex = 0;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClose.Location = new System.Drawing.Point(7, 394);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(91, 23);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "Закрыть";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtRefresh,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tsYearCurrent});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(549, 29);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtRefresh
            // 
            //this.tsbtRefresh.Image = global::Kadr.Properties.Resources.RefreshDocViewHS;
            this.tsbtRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtRefresh.Name = "tsbtRefresh";
            this.tsbtRefresh.Size = new System.Drawing.Size(77, 26);
            this.tsbtRefresh.Text = "Обновить";
            this.tsbtRefresh.ToolTipText = "Обновить результаты просмотра";
            this.tsbtRefresh.Click += new System.EventHandler(this.tsbtRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(26, 26);
            this.toolStripLabel1.Text = "Год";
            // 
            // tsYearCurrent
            // 
            this.tsYearCurrent.BackColor = System.Drawing.Color.Transparent;
            this.tsYearCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsYearCurrent.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.tsYearCurrent.Minimun = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.tsYearCurrent.Name = "tsYearCurrent";
            this.tsYearCurrent.Size = new System.Drawing.Size(126, 26);
            this.tsYearCurrent.Text = "toolStripNumbericUpDown1";
            this.tsYearCurrent.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CODE_SUBDIV";
            this.dataGridViewTextBoxColumn1.HeaderText = "Подразделение";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "SUBDIV_NAME";
            this.dataGridViewTextBoxColumn2.HeaderText = "Наименование";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "PERCENT_CONFIRM";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "##.00%";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn3.HeaderText = "Утверждено отпусков";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ToolTipText = "Процентная часть утвержденных отпусков в году";
            // 
            // CODE_SUBDIV
            // 
            this.CODE_SUBDIV.DataPropertyName = "CODE_SUBDIV";
            this.CODE_SUBDIV.HeaderText = "Подразделение";
            this.CODE_SUBDIV.Name = "CODE_SUBDIV";
            this.CODE_SUBDIV.ReadOnly = true;
            this.CODE_SUBDIV.Width = 80;
            // 
            // SUBDIV_NAME
            // 
            this.SUBDIV_NAME.DataPropertyName = "SUBDIV_NAME";
            this.SUBDIV_NAME.HeaderText = "Наименование";
            this.SUBDIV_NAME.Name = "SUBDIV_NAME";
            this.SUBDIV_NAME.ReadOnly = true;
            this.SUBDIV_NAME.Width = 150;
            // 
            // ALL_COUNT_VAC
            // 
            this.ALL_COUNT_VAC.DataPropertyName = "ALL_COUNT_VACS";
            this.ALL_COUNT_VAC.HeaderText = "Всего отпусков";
            this.ALL_COUNT_VAC.Name = "ALL_COUNT_VAC";
            this.ALL_COUNT_VAC.ReadOnly = true;
            // 
            // CONFIRM_VACS
            // 
            this.CONFIRM_VACS.DataPropertyName = "CONFIRM_VACS";
            this.CONFIRM_VACS.HeaderText = "Из них утверждено";
            this.CONFIRM_VACS.Name = "CONFIRM_VACS";
            this.CONFIRM_VACS.ReadOnly = true;
            // 
            // PERCENT_CONFIRM
            // 
            this.PERCENT_CONFIRM.DataPropertyName = "PERCENT_CONFIRM";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "00.00%";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.PERCENT_CONFIRM.DefaultCellStyle = dataGridViewCellStyle3;
            this.PERCENT_CONFIRM.HeaderText = "Утверждено отпусков %";
            this.PERCENT_CONFIRM.Name = "PERCENT_CONFIRM";
            this.PERCENT_CONFIRM.ReadOnly = true;
            this.PERCENT_CONFIRM.ToolTipText = "Процентная часть утвержденных отпусков в году";
            // 
            // ConfirmPercentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(549, 422);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmPercentView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Статистика утверждения подразделений";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSubdivConfirm)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgSubdivConfirm;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private LibraryKadr.ToolStripNumbericUpDown tsYearCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE_SUBDIV;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBDIV_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ALL_COUNT_VAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn CONFIRM_VACS;
        private System.Windows.Forms.DataGridViewTextBoxColumn PERCENT_CONFIRM;
    }
}