namespace Tabel
{
    partial class Gr_Work
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gr_Work));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btTime_Zone = new Elegant.Ui.Button();
            this.btExit = new Elegant.Ui.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgAccessSubdiv = new System.Windows.Forms.DataGridView();
            this.tsGR = new System.Windows.Forms.ToolStrip();
            this.tsbAdd_Gr = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit_Gr = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete_Gr = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tstCode_Subdiv = new System.Windows.Forms.ToolStripTextBox();
            this.dgGR_Work = new System.Windows.Forms.DataGridView();
            this.dtpDate_End_Graph = new LibraryKadr.ToolStripDateTimePicker();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAccessSubdiv)).BeginInit();
            this.tsGR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgGR_Work)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btTime_Zone);
            this.panel2.Controls.Add(this.btExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 437);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(860, 35);
            this.panel2.TabIndex = 76;
            // 
            // btTime_Zone
            // 
            this.btTime_Zone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btTime_Zone.Id = "20219c08-b9cb-4d91-b498-b9c5c27e8b9e";
            this.btTime_Zone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btTime_Zone.Location = new System.Drawing.Point(22, 6);
            this.btTime_Zone.Name = "btTime_Zone";
            this.btTime_Zone.Size = new System.Drawing.Size(147, 23);
            this.btTime_Zone.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", ((System.Drawing.Image)(resources.GetObject("btTime_Zone.SmallImages.Images"))))});
            this.btTime_Zone.TabIndex = 4;
            this.btTime_Zone.Text = "Временные зоны";
            this.btTime_Zone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btTime_Zone.Click += new System.EventHandler(this.btTime_Zone_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "14245208-d9e9-4c85-ac2b-810583a0d07c";
            this.btExit.Location = new System.Drawing.Point(767, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgAccessSubdiv);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 260);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(860, 177);
            this.groupBox1.TabIndex = 79;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список подразделений, которым доступен данный график";
            // 
            // dgAccessSubdiv
            // 
            this.dgAccessSubdiv.AllowUserToAddRows = false;
            this.dgAccessSubdiv.AllowUserToDeleteRows = false;
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
            this.dgAccessSubdiv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAccessSubdiv.Location = new System.Drawing.Point(3, 17);
            this.dgAccessSubdiv.Name = "dgAccessSubdiv";
            this.dgAccessSubdiv.ReadOnly = true;
            this.dgAccessSubdiv.RowHeadersWidth = 24;
            this.dgAccessSubdiv.Size = new System.Drawing.Size(854, 157);
            this.dgAccessSubdiv.TabIndex = 81;
            // 
            // tsGR
            // 
            this.tsGR.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsGR.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAdd_Gr,
            this.tsbEdit_Gr,
            this.tsbDelete_Gr,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tstCode_Subdiv,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.dtpDate_End_Graph});
            this.tsGR.Location = new System.Drawing.Point(0, 0);
            this.tsGR.Name = "tsGR";
            this.tsGR.Size = new System.Drawing.Size(860, 29);
            this.tsGR.TabIndex = 82;
            this.tsGR.Text = "toolStrip1";
            // 
            // tsbAdd_Gr
            // 
            this.tsbAdd_Gr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAdd_Gr.Image = global::Tabel.Properties.Resources.document_new_62;
            this.tsbAdd_Gr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd_Gr.Name = "tsbAdd_Gr";
            this.tsbAdd_Gr.Size = new System.Drawing.Size(23, 26);
            this.tsbAdd_Gr.Text = "toolStripButton1";
            this.tsbAdd_Gr.ToolTipText = "Добавить";
            this.tsbAdd_Gr.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbEdit_Gr
            // 
            this.tsbEdit_Gr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEdit_Gr.Image = global::Tabel.Properties.Resources.table_edit1;
            this.tsbEdit_Gr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit_Gr.Name = "tsbEdit_Gr";
            this.tsbEdit_Gr.Size = new System.Drawing.Size(23, 26);
            this.tsbEdit_Gr.Text = "toolStripButton2";
            this.tsbEdit_Gr.ToolTipText = "Редактировать";
            this.tsbEdit_Gr.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbDelete_Gr
            // 
            this.tsbDelete_Gr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete_Gr.Image = global::Tabel.Properties.Resources.Delete;
            this.tsbDelete_Gr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete_Gr.Name = "tsbDelete_Gr";
            this.tsbDelete_Gr.Size = new System.Drawing.Size(23, 26);
            this.tsbDelete_Gr.Text = "toolStripButton3";
            this.tsbDelete_Gr.ToolTipText = "Удалить";
            this.tsbDelete_Gr.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(206, 26);
            this.toolStripLabel1.Text = "Шифр подразделения для фильтрации";
            // 
            // tstCode_Subdiv
            // 
            this.tstCode_Subdiv.Name = "tstCode_Subdiv";
            this.tstCode_Subdiv.Size = new System.Drawing.Size(50, 29);
            this.tstCode_Subdiv.TextChanged += new System.EventHandler(this.tstCode_Subdiv_TextChanged);
            // 
            // dgGR_Work
            // 
            this.dgGR_Work.AllowUserToAddRows = false;
            this.dgGR_Work.AllowUserToDeleteRows = false;
            this.dgGR_Work.BackgroundColor = System.Drawing.Color.White;
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgGR_Work.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgGR_Work.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgGR_Work.Location = new System.Drawing.Point(0, 29);
            this.dgGR_Work.Name = "dgGR_Work";
            this.dgGR_Work.ReadOnly = true;
            this.dgGR_Work.RowHeadersWidth = 24;
            this.dgGR_Work.Size = new System.Drawing.Size(860, 231);
            this.dgGR_Work.TabIndex = 83;
            this.dgGR_Work.SelectionChanged += new System.EventHandler(this.dgGR_Work_SelectionChanged);
            this.dgGR_Work.DoubleClick += new System.EventHandler(this.tsbEdit_Click);
            // 
            // dtpDate_End_Graph
            // 
            this.dtpDate_End_Graph.BackColor = System.Drawing.Color.Transparent;
            this.dtpDate_End_Graph.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpDate_End_Graph.Name = "dtpDate_End_Graph";
            this.dtpDate_End_Graph.Size = new System.Drawing.Size(106, 26);
            this.dtpDate_End_Graph.Text = "toolStripDateTimePicker1";
            this.dtpDate_End_Graph.Value = new System.DateTime(2015, 1, 26, 14, 27, 51, 427);
            this.dtpDate_End_Graph.ValueChanged += new System.EventHandler(this.toolStripDateTimePicker1_ValueChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(109, 26);
            this.toolStripLabel2.Text = "Окончание графика";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // Gr_Work
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 472);
            this.Controls.Add(this.dgGR_Work);
            this.Controls.Add(this.tsGR);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Name = "Gr_Work";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Графики работы";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAccessSubdiv)).EndInit();
            this.tsGR.ResumeLayout(false);
            this.tsGR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgGR_Work)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private Elegant.Ui.Button btExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgAccessSubdiv;
        private System.Windows.Forms.DataGridView dgGR_Work;
        private System.Windows.Forms.ToolStrip tsGR;
        private System.Windows.Forms.ToolStripButton tsbAdd_Gr;
        private System.Windows.Forms.ToolStripButton tsbEdit_Gr;
        private System.Windows.Forms.ToolStripButton tsbDelete_Gr;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tstCode_Subdiv;
        private Elegant.Ui.Button btTime_Zone;
        private LibraryKadr.ToolStripDateTimePicker dtpDate_End_Graph;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }
}