namespace Kadr.Shtat
{
    partial class SelectSubdivFromTree
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
            this.treeSubdiv = new System.Windows.Forms.TreeView();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeSubdiv
            // 
            this.treeSubdiv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeSubdiv.Location = new System.Drawing.Point(2, -1);
            this.treeSubdiv.Margin = new System.Windows.Forms.Padding(4);
            this.treeSubdiv.Name = "treeSubdiv";
            this.treeSubdiv.PathSeparator = "/";
            this.treeSubdiv.ShowNodeToolTips = true;
            this.treeSubdiv.Size = new System.Drawing.Size(553, 450);
            this.treeSubdiv.TabIndex = 0;
            this.treeSubdiv.Resize += new System.EventHandler(this.treeStaff_Resize);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            this.formFrameSkinner1.IsFormMovable = false;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(483, 452);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(68, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Выход";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.BackColor = System.Drawing.Color.Transparent;
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Location = new System.Drawing.Point(391, 452);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(86, 23);
            this.btOk.TabIndex = 5;
            this.btOk.Text = "Выбрать";
            this.btOk.UseVisualStyleBackColor = false;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // SelectSubdivFromTree
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(557, 477);
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.treeSubdiv);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SelectSubdivFromTree";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор подразделения";
            this.Load += new System.EventHandler(this.SelectSubdivFromTree_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectSubdivFromTree_Paint);
            this.Shown += new System.EventHandler(this.SelectSubdivFromTree_Shown);
            this.Activated += new System.EventHandler(this.SelectSubdivFromTree_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectSubdivFromTree_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.SelectSubdivFromTree_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView treeSubdiv;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
    }
}