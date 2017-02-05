namespace Kadr.Shtat
{
    partial class AddToTreeEmp
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
            this.menuTreeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.rename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.delete = new System.Windows.Forms.ToolStripMenuItem();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeStaff = new System.Windows.Forms.TreeView();
            this.GridEmp = new System.Windows.Forms.DataGridView();
            this.btExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SS1 = new Kadr.Classes.SubdivSelector();
            this.listStaffsShtat = new System.Windows.Forms.DataGridView();
            this.check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.s_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.staffs_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btFromTree = new Elegant.Ui.Button();
            this.btToTree = new Elegant.Ui.Button();
            this.menuTreeNode.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridEmp)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listStaffsShtat)).BeginInit();
            this.SuspendLayout();
            // 
            // menuTreeNode
            // 
            this.menuTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add,
            this.toolStripSeparator1,
            this.rename,
            this.toolStripSeparator2,
            this.delete});
            this.menuTreeNode.Name = "menuTreeNode";
            this.menuTreeNode.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuTreeNode.Size = new System.Drawing.Size(165, 82);
            this.menuTreeNode.Opening += new System.ComponentModel.CancelEventHandler(this.menuTreeNode_Opening);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(164, 22);
            this.add.Text = "Добавить";
            this.add.Click += new System.EventHandler(this.добавитьУзелToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // rename
            // 
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(164, 22);
            this.rename.Text = "Редактировать";
            this.rename.Click += new System.EventHandler(this.rename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // delete
            // 
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(164, 22);
            this.delete.Text = "Удалить";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Ошибка";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btExit);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btFromTree);
            this.splitContainer1.Panel2.Controls.Add(this.btToTree);
            this.splitContainer1.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Size = new System.Drawing.Size(821, 560);
            this.splitContainer1.SplitterDistance = 364;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.NoMoveVert;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeStaff);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.GridEmp);
            this.splitContainer2.Size = new System.Drawing.Size(364, 560);
            this.splitContainer2.SplitterDistance = 395;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeStaff
            // 
            this.treeStaff.BackColor = System.Drawing.Color.White;
            this.treeStaff.ContextMenuStrip = this.menuTreeNode;
            this.treeStaff.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeStaff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeStaff.HideSelection = false;
            this.treeStaff.Location = new System.Drawing.Point(0, 0);
            this.treeStaff.Name = "treeStaff";
            this.treeStaff.Size = new System.Drawing.Size(360, 391);
            this.treeStaff.TabIndex = 0;
            this.treeStaff.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeStaff_AfterCollapse);
            this.treeStaff.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeStaff_BeforeExpand);
            this.treeStaff.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeStaff_AfterSelect);
            this.treeStaff.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeStaff_NodeMouseClick);
            // 
            // GridEmp
            // 
            this.GridEmp.AllowUserToAddRows = false;
            this.GridEmp.AllowUserToDeleteRows = false;
            this.GridEmp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridEmp.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridEmp.BackgroundColor = System.Drawing.SystemColors.Info;
            this.GridEmp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GridEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridEmp.Cursor = System.Windows.Forms.Cursors.Default;
            this.GridEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridEmp.Location = new System.Drawing.Point(0, 0);
            this.GridEmp.Name = "GridEmp";
            this.GridEmp.ReadOnly = true;
            this.GridEmp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridEmp.Size = new System.Drawing.Size(360, 158);
            this.GridEmp.TabIndex = 5;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.ForeColor = System.Drawing.Color.Blue;
            this.btExit.Location = new System.Drawing.Point(321, 529);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(112, 25);
            this.btExit.TabIndex = 10;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.SS1);
            this.groupBox1.Controls.Add(this.listStaffsShtat);
            this.groupBox1.Location = new System.Drawing.Point(53, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 523);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подразделение и список штатных единиц в нем";
            // 
            // SS1
            // 
            this.SS1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SS1.BackColor = System.Drawing.Color.Transparent;
            this.SS1.Enabled = true;
            this.SS1.Location = new System.Drawing.Point(6, 21);
            this.SS1.Name = "SS1";
            this.SS1.Size = new System.Drawing.Size(378, 20);
            this.SS1.TabIndex = 9;
            this.SS1.SubdivChanged += new System.EventHandler(this.subdiv_TextChanged);
            // 
            // listStaffsShtat
            // 
            this.listStaffsShtat.AllowUserToAddRows = false;
            this.listStaffsShtat.AllowUserToDeleteRows = false;
            this.listStaffsShtat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listStaffsShtat.BackgroundColor = System.Drawing.Color.White;
            this.listStaffsShtat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listStaffsShtat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.check,
            this.pos,
            this.fam,
            this.name,
            this.s_name,
            this.staffs_id});
            this.listStaffsShtat.Location = new System.Drawing.Point(5, 43);
            this.listStaffsShtat.Name = "listStaffsShtat";
            this.listStaffsShtat.RowHeadersVisible = false;
            this.listStaffsShtat.Size = new System.Drawing.Size(384, 474);
            this.listStaffsShtat.TabIndex = 8;
            // 
            // check
            // 
            this.check.Frozen = true;
            this.check.HeaderText = " ";
            this.check.Name = "check";
            // 
            // pos
            // 
            this.pos.Frozen = true;
            this.pos.HeaderText = "Профессия";
            this.pos.Name = "pos";
            this.pos.ReadOnly = true;
            // 
            // fam
            // 
            this.fam.Frozen = true;
            this.fam.HeaderText = "Фамилия";
            this.fam.Name = "fam";
            this.fam.ReadOnly = true;
            // 
            // name
            // 
            this.name.Frozen = true;
            this.name.HeaderText = "Имя";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // s_name
            // 
            this.s_name.Frozen = true;
            this.s_name.HeaderText = "Отчество";
            this.s_name.Name = "s_name";
            this.s_name.ReadOnly = true;
            // 
            // staffs_id
            // 
            this.staffs_id.Frozen = true;
            this.staffs_id.HeaderText = "s";
            this.staffs_id.Name = "staffs_id";
            this.staffs_id.ReadOnly = true;
            this.staffs_id.Visible = false;
            // 
            // btFromTree
            // 
            this.btFromTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btFromTree.Id = "b1d3cb18-514e-406f-982a-7229a464d4b5";
            this.btFromTree.Location = new System.Drawing.Point(5, 422);
            this.btFromTree.Name = "btFromTree";
            this.btFromTree.Size = new System.Drawing.Size(45, 54);
            this.btFromTree.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Kadr.Properties.Resources.ExcludeFromTree)});
            this.btFromTree.TabIndex = 7;
            this.btFromTree.Click += new System.EventHandler(this.btFromTree_Click);
            // 
            // btToTree
            // 
            this.btToTree.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btToTree.Id = "ba26d070-de91-45b1-a49e-16d0d5a26731";
            this.btToTree.LargeImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Kadr.Properties.Resources.add_to_tree)});
            this.btToTree.Location = new System.Drawing.Point(5, 119);
            this.btToTree.Name = "btToTree";
            this.btToTree.Size = new System.Drawing.Size(45, 54);
            this.btToTree.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Kadr.Properties.Resources.add_to_tree)});
            this.btToTree.TabIndex = 6;
            this.btToTree.MouseLeave += new System.EventHandler(this.btToTree_MouseLeave);
            this.btToTree.Click += new System.EventHandler(this.btToTree_Click);
            this.btToTree.MouseEnter += new System.EventHandler(this.btToTree_MouseEnter);
            // 
            // AddToTreeEmp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(821, 560);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "AddToTreeEmp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подразделения";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.AddToTreeEmp_Load);
            this.menuTreeNode.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridEmp)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listStaffsShtat)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeStaff;
        private System.Windows.Forms.ContextMenuStrip menuTreeNode;
        private System.Windows.Forms.ToolStripMenuItem add;
        private System.Windows.Forms.ToolStripMenuItem delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridView GridEmp;
        private Elegant.Ui.Button btFromTree;
        private System.Windows.Forms.DataGridView listStaffsShtat;
        private Elegant.Ui.Button btToTree;
        private System.Windows.Forms.ToolStripMenuItem rename;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn check;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn fam;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn s_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn staffs_id;
        private Kadr.Classes.SubdivSelector SS1;
    }
}