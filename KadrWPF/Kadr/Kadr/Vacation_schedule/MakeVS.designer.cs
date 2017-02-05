namespace Kadr.Vacation_schedule
{
    partial class MakeVS
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GridMakeVS = new System.Windows.Forms.DataGridView();
            this.menu_make_vs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.проверитьНапоминаниеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grid_vacs_emp = new System.Windows.Forms.DataGridView();
            this.screenTip1 = new Elegant.Ui.ScreenTip();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.viewVacsControl1 = new Kadr.Vacation_schedule.ViewVacsControl();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridMakeVS)).BeginInit();
            this.menu_make_vs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_vacs_emp)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GridMakeVS);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 212);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сотрудники:";
            // 
            // GridMakeVS
            // 
            this.GridMakeVS.AllowUserToAddRows = false;
            this.GridMakeVS.AllowUserToDeleteRows = false;
            this.GridMakeVS.AllowUserToResizeRows = false;
            this.GridMakeVS.BackgroundColor = System.Drawing.Color.White;
            this.GridMakeVS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridMakeVS.ContextMenuStrip = this.menu_make_vs;
            this.GridMakeVS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridMakeVS.Location = new System.Drawing.Point(3, 18);
            this.GridMakeVS.MultiSelect = false;
            this.GridMakeVS.Name = "GridMakeVS";
            this.GridMakeVS.ReadOnly = true;
            this.GridMakeVS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridMakeVS.Size = new System.Drawing.Size(469, 191);
            this.GridMakeVS.TabIndex = 0;
            // 
            // menu_make_vs
            // 
            this.menu_make_vs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.редактироватьToolStripMenuItem,
            this.toolStripMenuItem2,
            this.проверитьНапоминаниеToolStripMenuItem});
            this.menu_make_vs.Name = "menu_make_vs";
            this.menu_make_vs.Size = new System.Drawing.Size(214, 54);
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.редактироватьToolStripMenuItem.Text = "Редактировать";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(210, 6);
            // 
            // проверитьНапоминаниеToolStripMenuItem
            // 
            this.проверитьНапоминаниеToolStripMenuItem.Name = "проверитьНапоминаниеToolStripMenuItem";
            this.проверитьНапоминаниеToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.проверитьНапоминаниеToolStripMenuItem.Text = "Проверить напоминания!";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 194);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid_vacs_emp);
            this.splitContainer1.Size = new System.Drawing.Size(475, 291);
            this.splitContainer1.SplitterDistance = 212;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 11;
            // 
            // grid_vacs_emp
            // 
            this.grid_vacs_emp.AllowUserToAddRows = false;
            this.grid_vacs_emp.AllowUserToDeleteRows = false;
            this.grid_vacs_emp.BackgroundColor = System.Drawing.Color.White;
            this.grid_vacs_emp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_vacs_emp.DefaultCellStyle = dataGridViewCellStyle1;
            this.grid_vacs_emp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_vacs_emp.Location = new System.Drawing.Point(0, 0);
            this.grid_vacs_emp.Name = "grid_vacs_emp";
            this.grid_vacs_emp.ReadOnly = true;
            this.grid_vacs_emp.Size = new System.Drawing.Size(475, 73);
            this.grid_vacs_emp.TabIndex = 0;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(754, 485);
            this.elementHost1.TabIndex = 12;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.viewVacsControl1;
            // 
            // MakeVS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(754, 485);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MakeVS";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Формирование Графиков отпусков";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridMakeVS)).EndInit();
            this.menu_make_vs.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_vacs_emp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView GridMakeVS;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ContextMenuStrip menu_make_vs;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem проверитьНапоминаниеToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Elegant.Ui.ScreenTip screenTip1;
        private System.Windows.Forms.DataGridView grid_vacs_emp;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private ViewVacsControl viewVacsControl1;

    }
}