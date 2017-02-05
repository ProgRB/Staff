namespace Kadr.Shtat
{
    partial class TreeStaffEdit
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.btExit = new System.Windows.Forms.Button();
            this.screenTip1 = new Elegant.Ui.ScreenTip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TreeStaff = new System.Windows.Forms.TreeView();
            this.GridStaffs = new System.Windows.Forms.DataGridView();
            this.btFromTree = new Elegant.Ui.Button();
            this.btToTree = new Elegant.Ui.Button();
            this.subdivSelector1 = new Kadr.Classes.SubdivSelector();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridStaffs)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Location = new System.Drawing.Point(5, -1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(786, 527);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.ForeColor = System.Drawing.Color.Blue;
            this.btExit.Location = new System.Drawing.Point(676, 528);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(108, 24);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 19);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TreeStaff);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GridStaffs);
            this.splitContainer1.Panel2.Controls.Add(this.subdivSelector1);
            this.splitContainer1.Panel2.Controls.Add(this.btFromTree);
            this.splitContainer1.Panel2.Controls.Add(this.btToTree);
            this.splitContainer1.Size = new System.Drawing.Size(778, 504);
            this.splitContainer1.SplitterDistance = 328;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            // 
            // TreeStaff
            // 
            this.TreeStaff.AllowDrop = true;
            this.TreeStaff.CheckBoxes = true;
            this.TreeStaff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeStaff.HideSelection = false;
            this.TreeStaff.Location = new System.Drawing.Point(0, 0);
            this.TreeStaff.Name = "TreeStaff";
            this.screenTip1.GetScreenTip(this.TreeStaff).Caption = "Подразделение";
            this.screenTip1.GetScreenTip(this.TreeStaff).Text = "dsafsf";
            this.TreeStaff.ShowNodeToolTips = true;
            this.TreeStaff.Size = new System.Drawing.Size(328, 504);
            this.TreeStaff.TabIndex = 0;
            this.TreeStaff.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.TreeStaff_NodeMouseHover);
            this.TreeStaff.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.TreeStaff_ChangeUICues);
            this.TreeStaff.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeStaff_DragDrop);
            this.TreeStaff.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeStaff_DragEnter);
            this.TreeStaff.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeStaff_ItemDrag);
            this.TreeStaff.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeStaff_DragOver);
            // 
            // GridStaffs
            // 
            this.GridStaffs.AllowDrop = true;
            this.GridStaffs.AllowUserToAddRows = false;
            this.GridStaffs.AllowUserToDeleteRows = false;
            this.GridStaffs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GridStaffs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.GridStaffs.BackgroundColor = System.Drawing.Color.White;
            this.GridStaffs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridStaffs.Location = new System.Drawing.Point(111, 30);
            this.GridStaffs.Name = "GridStaffs";
            this.GridStaffs.ReadOnly = true;
            this.GridStaffs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridStaffs.Size = new System.Drawing.Size(328, 472);
            this.GridStaffs.TabIndex = 1;
            // 
            // btFromTree
            // 
            this.btFromTree.BackColor = System.Drawing.SystemColors.Control;
            this.btFromTree.Id = "e68c39fb-2c53-4bc2-9950-1c62d2ccfbd9";
            this.btFromTree.Location = new System.Drawing.Point(8, 412);
            this.btFromTree.Name = "btFromTree";
            this.btFromTree.Size = new System.Drawing.Size(100, 49);
            this.btFromTree.TabIndex = 3;
            this.btFromTree.Text = "Убрать из подчинения";
            this.btFromTree.Click += new System.EventHandler(this.btFromTree_Click);
            // 
            // btToTree
            // 
            this.btToTree.Id = "b8761ba9-ce0c-41d0-a73c-cd827598aaaa";
            this.btToTree.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btToTree.Location = new System.Drawing.Point(8, 138);
            this.btToTree.Name = "btToTree";
            this.btToTree.ScreenTip.Caption = "Подсказка";
            this.btToTree.ScreenTip.Text = "Нажмите чтобы добавить в подчинение выделенные штатные единицы";
            this.btToTree.Size = new System.Drawing.Size(100, 40);
            this.btToTree.TabIndex = 2;
            this.btToTree.Text = "Добавить в подчинение";
            this.btToTree.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btToTree.Click += new System.EventHandler(this.btToTree_Click);
            // 
            // subdivSelector1
            // 
            this.subdivSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subdivSelector1.BackColor = System.Drawing.Color.Transparent;
            this.subdivSelector1.Enabled = true;
            this.subdivSelector1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.subdivSelector1.Location = new System.Drawing.Point(12, 8);
            this.subdivSelector1.Name = "subdivSelector1";
            this.subdivSelector1.Size = new System.Drawing.Size(427, 29);
            this.subdivSelector1.TabIndex = 4;
            this.subdivSelector1.SubdivChanged += new System.EventHandler(this.subdiv_TextChanged);
            // 
            // TreeStaffEdit
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(796, 554);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TreeStaffEdit";
            this.Text = "Редактирование подчиненности";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TreeStaffEdit_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TreeStaffEdit_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridStaffs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView TreeStaff;
        private System.Windows.Forms.DataGridView GridStaffs;
        private Elegant.Ui.Button btFromTree;
        private Elegant.Ui.Button btToTree;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Button btExit;
        private Elegant.Ui.ScreenTip screenTip1;
        private Kadr.Classes.SubdivSelector subdivSelector1;

    }
}