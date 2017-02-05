namespace Tabel
{
    partial class Access_Gr_Subdiv
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvSubdiv = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lvAccessSubdiv = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btDeleteSubdivAll = new System.Windows.Forms.Button();
            this.btAddSubdivAll = new System.Windows.Forms.Button();
            this.btDeleteSubdiv = new System.Windows.Forms.Button();
            this.btAddSubdiv = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btExit);
            this.panel3.Controls.Add(this.btSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 457);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(867, 43);
            this.panel3.TabIndex = 2;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(780, 10);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 3;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSave.Location = new System.Drawing.Point(686, 10);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 2;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lvSubdiv);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 451);
            this.panel1.TabIndex = 7;
            // 
            // lvSubdiv
            // 
            this.lvSubdiv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4});
            this.lvSubdiv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvSubdiv.FullRowSelect = true;
            this.lvSubdiv.Location = new System.Drawing.Point(0, 0);
            this.lvSubdiv.Name = "lvSubdiv";
            this.lvSubdiv.Size = new System.Drawing.Size(395, 451);
            this.lvSubdiv.TabIndex = 4;
            this.lvSubdiv.UseCompatibleStateImageBehavior = false;
            this.lvSubdiv.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Шифр";
            this.columnHeader1.Width = 57;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Наименование подразделений";
            this.columnHeader4.Width = 334;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.27969F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.684274F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.03604F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lvAccessSubdiv, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(867, 457);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // lvAccessSubdiv
            // 
            this.lvAccessSubdiv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.lvAccessSubdiv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAccessSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvAccessSubdiv.FullRowSelect = true;
            this.lvAccessSubdiv.Location = new System.Drawing.Point(461, 3);
            this.lvAccessSubdiv.Name = "lvAccessSubdiv";
            this.lvAccessSubdiv.Size = new System.Drawing.Size(403, 451);
            this.lvAccessSubdiv.TabIndex = 14;
            this.lvAccessSubdiv.UseCompatibleStateImageBehavior = false;
            this.lvAccessSubdiv.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Шифр";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Наименование подразделения";
            this.columnHeader3.Width = 337;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btDeleteSubdivAll);
            this.panel2.Controls.Add(this.btAddSubdivAll);
            this.panel2.Controls.Add(this.btDeleteSubdiv);
            this.panel2.Controls.Add(this.btAddSubdiv);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(404, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(51, 451);
            this.panel2.TabIndex = 13;
            // 
            // btDeleteSubdivAll
            // 
            this.btDeleteSubdivAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeleteSubdivAll.Location = new System.Drawing.Point(3, 115);
            this.btDeleteSubdivAll.Name = "btDeleteSubdivAll";
            this.btDeleteSubdivAll.Size = new System.Drawing.Size(43, 26);
            this.btDeleteSubdivAll.TabIndex = 8;
            this.btDeleteSubdivAll.Text = "<<<";
            this.btDeleteSubdivAll.UseVisualStyleBackColor = true;
            this.btDeleteSubdivAll.Click += new System.EventHandler(this.btDeleteSubdivAll_Click);
            // 
            // btAddSubdivAll
            // 
            this.btAddSubdivAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddSubdivAll.Location = new System.Drawing.Point(3, 35);
            this.btAddSubdivAll.Name = "btAddSubdivAll";
            this.btAddSubdivAll.Size = new System.Drawing.Size(43, 26);
            this.btAddSubdivAll.TabIndex = 7;
            this.btAddSubdivAll.Text = ">>>";
            this.btAddSubdivAll.UseVisualStyleBackColor = true;
            this.btAddSubdivAll.Click += new System.EventHandler(this.btAddSubdivAll_Click);
            // 
            // btDeleteSubdiv
            // 
            this.btDeleteSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeleteSubdiv.Location = new System.Drawing.Point(3, 83);
            this.btDeleteSubdiv.Name = "btDeleteSubdiv";
            this.btDeleteSubdiv.Size = new System.Drawing.Size(43, 26);
            this.btDeleteSubdiv.TabIndex = 6;
            this.btDeleteSubdiv.Text = "<";
            this.btDeleteSubdiv.UseVisualStyleBackColor = true;
            this.btDeleteSubdiv.Click += new System.EventHandler(this.btDeleteSubdiv_Click);
            // 
            // btAddSubdiv
            // 
            this.btAddSubdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddSubdiv.Location = new System.Drawing.Point(3, 3);
            this.btAddSubdiv.Name = "btAddSubdiv";
            this.btAddSubdiv.Size = new System.Drawing.Size(43, 26);
            this.btAddSubdiv.TabIndex = 5;
            this.btAddSubdiv.Text = ">";
            this.btAddSubdiv.UseVisualStyleBackColor = true;
            this.btAddSubdiv.Click += new System.EventHandler(this.btAddSubdiv_Click);
            // 
            // Access_Gr_Subdiv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 500);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel3);
            this.Name = "Access_Gr_Subdiv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Доступ графика работы подразделениям завода";
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lvSubdiv;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView lvAccessSubdiv;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btDeleteSubdivAll;
        private System.Windows.Forms.Button btAddSubdivAll;
        private System.Windows.Forms.Button btDeleteSubdiv;
        private System.Windows.Forms.Button btAddSubdiv;
    }
}