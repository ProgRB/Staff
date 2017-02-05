namespace ARM_PROP
{
    partial class ListPermit
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgPermit = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgEmpPermit = new System.Windows.Forms.DataGridView();
            this.chShowAllPermit = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPermit)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpPermit)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dgPermit, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 214F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(749, 475);
            this.tableLayoutPanel1.TabIndex = 39;
            // 
            // dgPermit
            // 
            this.dgPermit.AllowUserToAddRows = false;
            this.dgPermit.AllowUserToDeleteRows = false;
            this.dgPermit.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgPermit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPermit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgPermit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPermit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPermit.Location = new System.Drawing.Point(4, 4);
            this.dgPermit.Name = "dgPermit";
            this.dgPermit.ReadOnly = true;
            this.dgPermit.RowHeadersWidth = 24;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgPermit.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgPermit.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgPermit.Size = new System.Drawing.Size(741, 252);
            this.dgPermit.TabIndex = 40;
            this.dgPermit.SelectionChanged += new System.EventHandler(this.dgPermit_SelectionChanged);
            this.dgPermit.DoubleClick += new System.EventHandler(this.dgPermit_DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.chShowAllPermit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 263);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(741, 208);
            this.panel2.TabIndex = 41;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgEmpPermit);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(66)))), ((int)(((byte)(140)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 29);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(741, 179);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Данные по разрешениям работника";
            // 
            // dgEmpPermit
            // 
            this.dgEmpPermit.AllowUserToAddRows = false;
            this.dgEmpPermit.AllowUserToDeleteRows = false;
            this.dgEmpPermit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgEmpPermit.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgEmpPermit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmpPermit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgEmpPermit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmpPermit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEmpPermit.Location = new System.Drawing.Point(3, 17);
            this.dgEmpPermit.Name = "dgEmpPermit";
            this.dgEmpPermit.ReadOnly = true;
            this.dgEmpPermit.RowHeadersWidth = 24;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dgEmpPermit.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgEmpPermit.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgEmpPermit.Size = new System.Drawing.Size(735, 159);
            this.dgEmpPermit.TabIndex = 6;
            this.dgEmpPermit.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgEmpPermit_CellFormatting);
            // 
            // chShowAllPermit
            // 
            this.chShowAllPermit.AutoSize = true;
            this.chShowAllPermit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chShowAllPermit.Dock = System.Windows.Forms.DockStyle.Top;
            this.chShowAllPermit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chShowAllPermit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(66)))), ((int)(((byte)(140)))));
            this.chShowAllPermit.Location = new System.Drawing.Point(0, 0);
            this.chShowAllPermit.Name = "chShowAllPermit";
            this.chShowAllPermit.Padding = new System.Windows.Forms.Padding(20, 5, 0, 5);
            this.chShowAllPermit.Size = new System.Drawing.Size(741, 29);
            this.chShowAllPermit.TabIndex = 9;
            this.chShowAllPermit.Text = "Отображать все разрешения (текущие и архивные)";
            this.chShowAllPermit.UseVisualStyleBackColor = true;
            this.chShowAllPermit.CheckedChanged += new System.EventHandler(this.chShowAllPermit_CheckedChanged);
            // 
            // ListPermit
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ListPermit";
            this.Size = new System.Drawing.Size(749, 475);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPermit)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpPermit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.DataGridView dgPermit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chShowAllPermit;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.DataGridView dgEmpPermit;
    }
}