namespace StaffReports
{
    partial class OrderOnEncouraging
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btExecute = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgEmp = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gbFilterEmp = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFilter_Per_Num = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFilter_Code_Subdiv = new System.Windows.Forms.TextBox();
            this.tbFilter_FIO = new System.Windows.Forms.TextBox();
            this.deDate_Filter = new LibraryKadr.DateEditor();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgEmpForOrder = new System.Windows.Forms.DataGridView();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmp)).BeginInit();
            this.panel4.SuspendLayout();
            this.gbFilterEmp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpForOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btExit);
            this.panel3.Controls.Add(this.btExecute);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 434);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(880, 43);
            this.panel3.TabIndex = 3;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(793, 10);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 3;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // btExecute
            // 
            this.btExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExecute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExecute.Location = new System.Drawing.Point(663, 10);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(124, 23);
            this.btExecute.TabIndex = 2;
            this.btExecute.Text = "Сформировать";
            this.btExecute.UseVisualStyleBackColor = true;
            this.btExecute.Click += new System.EventHandler(this.btExecute_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            this.splitContainer2.Panel1MinSize = 400;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(880, 434);
            this.splitContainer2.SplitterDistance = 420;
            this.splitContainer2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgEmp);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(416, 430);
            this.panel1.TabIndex = 8;
            // 
            // dgEmp
            // 
            this.dgEmp.AllowUserToAddRows = false;
            this.dgEmp.AllowUserToDeleteRows = false;
            this.dgEmp.BackgroundColor = System.Drawing.Color.White;
            this.dgEmp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEmp.Location = new System.Drawing.Point(0, 130);
            this.dgEmp.Name = "dgEmp";
            this.dgEmp.ReadOnly = true;
            this.dgEmp.RowHeadersWidth = 24;
            this.dgEmp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEmp.Size = new System.Drawing.Size(416, 300);
            this.dgEmp.TabIndex = 0;
            this.dgEmp.DoubleClick += new System.EventHandler(this.dgEmp_DoubleClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gbFilterEmp);
            this.panel4.Controls.Add(this.deDate_Filter);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(416, 130);
            this.panel4.TabIndex = 3;
            // 
            // gbFilterEmp
            // 
            this.gbFilterEmp.Controls.Add(this.label3);
            this.gbFilterEmp.Controls.Add(this.tbFilter_Per_Num);
            this.gbFilterEmp.Controls.Add(this.label2);
            this.gbFilterEmp.Controls.Add(this.label1);
            this.gbFilterEmp.Controls.Add(this.tbFilter_Code_Subdiv);
            this.gbFilterEmp.Controls.Add(this.tbFilter_FIO);
            this.gbFilterEmp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbFilterEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFilterEmp.Location = new System.Drawing.Point(0, 46);
            this.gbFilterEmp.Name = "gbFilterEmp";
            this.gbFilterEmp.Size = new System.Drawing.Size(416, 84);
            this.gbFilterEmp.TabIndex = 84;
            this.gbFilterEmp.TabStop = false;
            this.gbFilterEmp.Text = "Фильтрация сотрудников";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(203, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 15);
            this.label3.TabIndex = 88;
            this.label3.Text = "Табельный номер";
            // 
            // tbFilter_Per_Num
            // 
            this.tbFilter_Per_Num.Location = new System.Drawing.Point(334, 23);
            this.tbFilter_Per_Num.Name = "tbFilter_Per_Num";
            this.tbFilter_Per_Num.Size = new System.Drawing.Size(63, 21);
            this.tbFilter_Per_Num.TabIndex = 87;
            this.tbFilter_Per_Num.TextChanged += new System.EventHandler(this.tbFilter_Per_Num_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 86;
            this.label2.Text = "ФИО";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 85;
            this.label1.Text = "Подразделение";
            // 
            // tbFilter_Code_Subdiv
            // 
            this.tbFilter_Code_Subdiv.Location = new System.Drawing.Point(133, 23);
            this.tbFilter_Code_Subdiv.Name = "tbFilter_Code_Subdiv";
            this.tbFilter_Code_Subdiv.Size = new System.Drawing.Size(63, 21);
            this.tbFilter_Code_Subdiv.TabIndex = 84;
            this.tbFilter_Code_Subdiv.TextChanged += new System.EventHandler(this.tbFilter_Code_Subdiv_TextChanged);
            // 
            // tbFilter_FIO
            // 
            this.tbFilter_FIO.Location = new System.Drawing.Point(69, 50);
            this.tbFilter_FIO.Name = "tbFilter_FIO";
            this.tbFilter_FIO.Size = new System.Drawing.Size(328, 21);
            this.tbFilter_FIO.TabIndex = 83;
            this.tbFilter_FIO.TextChanged += new System.EventHandler(this.tbFilter_FIO_TextChanged);
            // 
            // deDate_Filter
            // 
            this.deDate_Filter.AutoSize = true;
            this.deDate_Filter.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Filter.Date = null;
            this.deDate_Filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Filter.Location = new System.Drawing.Point(317, 12);
            this.deDate_Filter.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.deDate_Filter.Name = "deDate_Filter";
            this.deDate_Filter.ReadOnly = false;
            this.deDate_Filter.Size = new System.Drawing.Size(80, 24);
            this.deDate_Filter.TabIndex = 1;
            this.deDate_Filter.TextDate = null;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(15, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(279, 15);
            this.label5.TabIndex = 82;
            this.label5.Text = "Дата формирования списка работников";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgEmpForOrder);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(456, 434);
            this.splitContainer1.SplitterDistance = 401;
            this.splitContainer1.TabIndex = 1;
            // 
            // dgEmpForOrder
            // 
            this.dgEmpForOrder.AllowUserToAddRows = false;
            this.dgEmpForOrder.AllowUserToDeleteRows = false;
            this.dgEmpForOrder.BackgroundColor = System.Drawing.Color.White;
            this.dgEmpForOrder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgEmpForOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmpForOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEmpForOrder.Location = new System.Drawing.Point(0, 0);
            this.dgEmpForOrder.Name = "dgEmpForOrder";
            this.dgEmpForOrder.ReadOnly = true;
            this.dgEmpForOrder.RowHeadersWidth = 24;
            this.dgEmpForOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEmpForOrder.Size = new System.Drawing.Size(452, 430);
            this.dgEmpForOrder.TabIndex = 4;
            this.dgEmpForOrder.DoubleClick += new System.EventHandler(this.dgEmpForOrder_DoubleClick);
            // 
            // OrderOnEncouraging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 477);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.panel3);
            this.MaximizeBox = false;
            this.Name = "OrderOnEncouraging";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Формирование приказа о поощрении";
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmp)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.gbFilterEmp.ResumeLayout(false);
            this.gbFilterEmp.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpForOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btExecute;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgEmp;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox gbFilterEmp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFilter_Per_Num;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFilter_Code_Subdiv;
        private System.Windows.Forms.TextBox tbFilter_FIO;
        private LibraryKadr.DateEditor deDate_Filter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgEmpForOrder;
    }
}