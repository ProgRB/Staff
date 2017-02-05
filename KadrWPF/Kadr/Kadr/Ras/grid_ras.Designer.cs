namespace Kadr
{
    partial class grid_ras
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
            this.dgEmp = new System.Windows.Forms.DataGridView();
            this.cmGrid_ras = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.chShowAllTransfer = new System.Windows.Forms.CheckBox();
            this.gbAccount_Data = new System.Windows.Forms.GroupBox();
            this.dgAccount_data = new System.Windows.Forms.DataGridView();
            this.gbTransfer = new System.Windows.Forms.GroupBox();
            this.dgTransfer = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmp)).BeginInit();
            this.gbAccount_Data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAccount_data)).BeginInit();
            this.gbTransfer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransfer)).BeginInit();
            this.SuspendLayout();
            // 
            // dgEmp
            // 
            this.dgEmp.AllowUserToAddRows = false;
            this.dgEmp.AllowUserToDeleteRows = false;
            this.dgEmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgEmp.BackgroundColor = System.Drawing.Color.White;
            this.dgEmp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmp.ContextMenuStrip = this.cmGrid_ras;
            this.dgEmp.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgEmp.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgEmp.Location = new System.Drawing.Point(1, 1);
            this.dgEmp.Name = "dgEmp";
            this.dgEmp.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmp.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgEmp.RowHeadersWidth = 24;
            this.dgEmp.Size = new System.Drawing.Size(1048, 229);
            this.dgEmp.TabIndex = 5;
            this.dgEmp.SelectionChanged += new System.EventHandler(this.dgEmp_SelectionChanged);
            // 
            // cmGrid_ras
            // 
            this.cmGrid_ras.Name = "cmGrid_ras";
            this.cmGrid_ras.Size = new System.Drawing.Size(61, 4);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // chShowAllTransfer
            // 
            this.chShowAllTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chShowAllTransfer.AutoSize = true;
            this.chShowAllTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chShowAllTransfer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(66)))), ((int)(((byte)(140)))));
            this.chShowAllTransfer.Location = new System.Drawing.Point(12, 236);
            this.chShowAllTransfer.Name = "chShowAllTransfer";
            this.chShowAllTransfer.Size = new System.Drawing.Size(224, 20);
            this.chShowAllTransfer.TabIndex = 2;
            this.chShowAllTransfer.Text = "Отображать все переводы";
            this.chShowAllTransfer.UseVisualStyleBackColor = true;
            this.chShowAllTransfer.CheckedChanged += new System.EventHandler(this.chShowAllTransfer_CheckedChanged);
            // 
            // gbAccount_Data
            // 
            this.gbAccount_Data.Controls.Add(this.dgAccount_data);
            this.gbAccount_Data.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbAccount_Data.Location = new System.Drawing.Point(0, 473);
            this.gbAccount_Data.Name = "gbAccount_Data";
            this.gbAccount_Data.Size = new System.Drawing.Size(1049, 129);
            this.gbAccount_Data.TabIndex = 9;
            this.gbAccount_Data.TabStop = false;
            // 
            // dgAccount_data
            // 
            this.dgAccount_data.AllowUserToAddRows = false;
            this.dgAccount_data.AllowUserToDeleteRows = false;
            this.dgAccount_data.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgAccount_data.BackgroundColor = System.Drawing.Color.White;
            this.dgAccount_data.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAccount_data.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgAccount_data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAccount_data.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgAccount_data.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgAccount_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAccount_data.Location = new System.Drawing.Point(3, 16);
            this.dgAccount_data.Name = "dgAccount_data";
            this.dgAccount_data.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAccount_data.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgAccount_data.RowHeadersWidth = 24;
            this.dgAccount_data.Size = new System.Drawing.Size(1043, 110);
            this.dgAccount_data.TabIndex = 1;
            // 
            // gbTransfer
            // 
            this.gbTransfer.Controls.Add(this.dgTransfer);
            this.gbTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbTransfer.Location = new System.Drawing.Point(0, 262);
            this.gbTransfer.Name = "gbTransfer";
            this.gbTransfer.Size = new System.Drawing.Size(1049, 211);
            this.gbTransfer.TabIndex = 10;
            this.gbTransfer.TabStop = false;
            this.gbTransfer.Text = "Переводы";
            // 
            // dgTransfer
            // 
            this.dgTransfer.AllowUserToAddRows = false;
            this.dgTransfer.AllowUserToDeleteRows = false;
            this.dgTransfer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgTransfer.BackgroundColor = System.Drawing.Color.White;
            this.dgTransfer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTransfer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTransfer.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTransfer.Location = new System.Drawing.Point(3, 18);
            this.dgTransfer.Name = "dgTransfer";
            this.dgTransfer.ReadOnly = true;
            this.dgTransfer.RowHeadersWidth = 24;
            this.dgTransfer.RowTemplate.ReadOnly = true;
            this.dgTransfer.Size = new System.Drawing.Size(1043, 190);
            this.dgTransfer.TabIndex = 0;
            this.dgTransfer.SelectionChanged += new System.EventHandler(this.dgTransfer_SelectionChanged);
            // 
            // grid_ras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 602);
            this.Controls.Add(this.gbTransfer);
            this.Controls.Add(this.gbAccount_Data);
            this.Controls.Add(this.chShowAllTransfer);
            this.Controls.Add(this.dgEmp);
            this.Name = "grid_ras";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список сотрудников АРМ Бухгалтера";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.grid_ras_FormClosing);
            this.Load += new System.EventHandler(this.grid_ras_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmp)).EndInit();
            this.gbAccount_Data.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAccount_data)).EndInit();
            this.gbTransfer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTransfer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dgEmp;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ContextMenuStrip cmGrid_ras;
        private System.Windows.Forms.CheckBox chShowAllTransfer;
        private System.Windows.Forms.GroupBox gbTransfer;
        public System.Windows.Forms.DataGridView dgTransfer;
        private System.Windows.Forms.GroupBox gbAccount_Data;
        public System.Windows.Forms.DataGridView dgAccount_data;
    }
}