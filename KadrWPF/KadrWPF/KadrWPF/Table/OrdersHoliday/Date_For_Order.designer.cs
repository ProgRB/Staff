namespace Tabel
{
    partial class Date_For_Order
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
            this.lb303 = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgEmp = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gbFilterEmp = new System.Windows.Forms.GroupBox();
            this.tbFilterEmp = new System.Windows.Forms.TextBox();
            this.tbResp = new System.Windows.Forms.TextBox();
            this.deWork_Date = new LibraryKadr.DateEditor();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btAddEmpForPay = new System.Windows.Forms.Button();
            this.btDeleteEmpForPay = new System.Windows.Forms.Button();
            this.dgEmpForPay = new System.Windows.Forms.DataGridView();
            this.btAddEmpForHoliday = new System.Windows.Forms.Button();
            this.btDeleteEmpForHoliday = new System.Windows.Forms.Button();
            this.dgEmpForHoliday = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
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
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpForPay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpForHoliday)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lb303);
            this.panel3.Controls.Add(this.lbInfo);
            this.panel3.Controls.Add(this.btExit);
            this.panel3.Controls.Add(this.btSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 573);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(854, 43);
            this.panel3.TabIndex = 2;
            // 
            // lb303
            // 
            this.lb303.AutoSize = true;
            this.lb303.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb303.Location = new System.Drawing.Point(323, 14);
            this.lb303.Name = "lb303";
            this.lb303.Size = new System.Drawing.Size(172, 15);
            this.lb303.TabIndex = 6;
            this.lb303.Text = "Доступно в счет отгулов";
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbInfo.Location = new System.Drawing.Point(17, 14);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(152, 15);
            this.lbInfo.TabIndex = 5;
            this.lbInfo.Text = "Доступно для оплаты";
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(767, 10);
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
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSave.Location = new System.Drawing.Point(673, 10);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 23);
            this.btSave.TabIndex = 2;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
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
            this.splitContainer2.Size = new System.Drawing.Size(854, 573);
            this.splitContainer2.SplitterDistance = 408;
            this.splitContainer2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgEmp);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(404, 569);
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
            this.dgEmp.RowHeadersWidth = 24;
            this.dgEmp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEmp.Size = new System.Drawing.Size(404, 439);
            this.dgEmp.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gbFilterEmp);
            this.panel4.Controls.Add(this.tbResp);
            this.panel4.Controls.Add(this.deWork_Date);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(404, 130);
            this.panel4.TabIndex = 3;
            // 
            // gbFilterEmp
            // 
            this.gbFilterEmp.Controls.Add(this.tbFilterEmp);
            this.gbFilterEmp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbFilterEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFilterEmp.Location = new System.Drawing.Point(0, 78);
            this.gbFilterEmp.Name = "gbFilterEmp";
            this.gbFilterEmp.Size = new System.Drawing.Size(404, 52);
            this.gbFilterEmp.TabIndex = 84;
            this.gbFilterEmp.TabStop = false;
            this.gbFilterEmp.Text = "Фильтр сотрудников по ФИО";
            // 
            // tbFilterEmp
            // 
            this.tbFilterEmp.Location = new System.Drawing.Point(16, 20);
            this.tbFilterEmp.Name = "tbFilterEmp";
            this.tbFilterEmp.Size = new System.Drawing.Size(372, 21);
            this.tbFilterEmp.TabIndex = 83;
            this.tbFilterEmp.TextChanged += new System.EventHandler(this.tbFilterEmp_TextChanged);
            // 
            // tbResp
            // 
            this.tbResp.BackColor = System.Drawing.Color.White;
            this.tbResp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbResp.Location = new System.Drawing.Point(16, 28);
            this.tbResp.Multiline = true;
            this.tbResp.Name = "tbResp";
            this.tbResp.Size = new System.Drawing.Size(276, 44);
            this.tbResp.TabIndex = 0;
            // 
            // deWork_Date
            // 
            this.deWork_Date.AutoSize = true;
            this.deWork_Date.BackColor = System.Drawing.SystemColors.Control;
            this.deWork_Date.Date = null;
            this.deWork_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deWork_Date.Location = new System.Drawing.Point(301, 28);
            this.deWork_Date.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.deWork_Date.Name = "deWork_Date";
            this.deWork_Date.ReadOnly = false;
            this.deWork_Date.Size = new System.Drawing.Size(87, 24);
            this.deWork_Date.TabIndex = 1;
            this.deWork_Date.TextDate = null;
            this.deWork_Date.Validating += new System.ComponentModel.CancelEventHandler(this.deWork_Date_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(298, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 15);
            this.label5.TabIndex = 82;
            this.label5.Text = "Дата";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(13, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(279, 15);
            this.label2.TabIndex = 80;
            this.label2.Text = "Ответственным за ППБ и ТБ назначить:";
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
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.btAddEmpForPay);
            this.splitContainer1.Panel1.Controls.Add(this.btDeleteEmpForPay);
            this.splitContainer1.Panel1.Controls.Add(this.dgEmpForPay);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btAddEmpForHoliday);
            this.splitContainer1.Panel2.Controls.Add(this.btDeleteEmpForHoliday);
            this.splitContainer1.Panel2.Controls.Add(this.dgEmpForHoliday);
            this.splitContainer1.Size = new System.Drawing.Size(442, 573);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 1;
            // 
            // btAddEmpForPay
            // 
            this.btAddEmpForPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddEmpForPay.Location = new System.Drawing.Point(3, 14);
            this.btAddEmpForPay.Name = "btAddEmpForPay";
            this.btAddEmpForPay.Size = new System.Drawing.Size(43, 26);
            this.btAddEmpForPay.TabIndex = 5;
            this.btAddEmpForPay.Text = ">>>";
            this.btAddEmpForPay.UseVisualStyleBackColor = true;
            this.btAddEmpForPay.Click += new System.EventHandler(this.btAddEmpForPay_Click);
            // 
            // btDeleteEmpForPay
            // 
            this.btDeleteEmpForPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeleteEmpForPay.Location = new System.Drawing.Point(3, 46);
            this.btDeleteEmpForPay.Name = "btDeleteEmpForPay";
            this.btDeleteEmpForPay.Size = new System.Drawing.Size(43, 26);
            this.btDeleteEmpForPay.TabIndex = 6;
            this.btDeleteEmpForPay.Text = "<<<";
            this.btDeleteEmpForPay.UseVisualStyleBackColor = true;
            this.btDeleteEmpForPay.Click += new System.EventHandler(this.btDeleteEmpForPay_Click);
            // 
            // dgEmpForPay
            // 
            this.dgEmpForPay.AllowUserToAddRows = false;
            this.dgEmpForPay.AllowUserToDeleteRows = false;
            this.dgEmpForPay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgEmpForPay.BackgroundColor = System.Drawing.Color.White;
            this.dgEmpForPay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgEmpForPay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmpForPay.Location = new System.Drawing.Point(52, 3);
            this.dgEmpForPay.Name = "dgEmpForPay";
            this.dgEmpForPay.RowHeadersWidth = 24;
            this.dgEmpForPay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEmpForPay.Size = new System.Drawing.Size(388, 273);
            this.dgEmpForPay.TabIndex = 4;
            // 
            // btAddEmpForHoliday
            // 
            this.btAddEmpForHoliday.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddEmpForHoliday.Location = new System.Drawing.Point(3, 14);
            this.btAddEmpForHoliday.Name = "btAddEmpForHoliday";
            this.btAddEmpForHoliday.Size = new System.Drawing.Size(43, 26);
            this.btAddEmpForHoliday.TabIndex = 7;
            this.btAddEmpForHoliday.Text = ">>>";
            this.btAddEmpForHoliday.UseVisualStyleBackColor = true;
            this.btAddEmpForHoliday.Click += new System.EventHandler(this.btAddEmpForHoliday_Click);
            // 
            // btDeleteEmpForHoliday
            // 
            this.btDeleteEmpForHoliday.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeleteEmpForHoliday.Location = new System.Drawing.Point(3, 46);
            this.btDeleteEmpForHoliday.Name = "btDeleteEmpForHoliday";
            this.btDeleteEmpForHoliday.Size = new System.Drawing.Size(43, 26);
            this.btDeleteEmpForHoliday.TabIndex = 8;
            this.btDeleteEmpForHoliday.Text = "<<<";
            this.btDeleteEmpForHoliday.UseVisualStyleBackColor = true;
            this.btDeleteEmpForHoliday.Click += new System.EventHandler(this.btDeleteEmpForHoliday_Click);
            // 
            // dgEmpForHoliday
            // 
            this.dgEmpForHoliday.AllowUserToAddRows = false;
            this.dgEmpForHoliday.AllowUserToDeleteRows = false;
            this.dgEmpForHoliday.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgEmpForHoliday.BackgroundColor = System.Drawing.Color.White;
            this.dgEmpForHoliday.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgEmpForHoliday.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmpForHoliday.Location = new System.Drawing.Point(52, 2);
            this.dgEmpForHoliday.Name = "dgEmpForHoliday";
            this.dgEmpForHoliday.RowHeadersWidth = 24;
            this.dgEmpForHoliday.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEmpForHoliday.Size = new System.Drawing.Size(388, 283);
            this.dgEmpForHoliday.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(3, 238);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 26);
            this.button1.TabIndex = 7;
            this.button1.Text = ">>>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Date_For_Order
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 616);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.panel3);
            this.Name = "Date_For_Order";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Данные дня приказа на выходные дни";
            this.Load += new System.EventHandler(this.Date_For_Order_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
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
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpForPay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmpForHoliday)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgEmp;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox gbFilterEmp;
        private System.Windows.Forms.TextBox tbFilterEmp;
        private System.Windows.Forms.TextBox tbResp;
        private LibraryKadr.DateEditor deWork_Date;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btAddEmpForPay;
        private System.Windows.Forms.Button btDeleteEmpForPay;
        private System.Windows.Forms.DataGridView dgEmpForPay;
        private System.Windows.Forms.Button btAddEmpForHoliday;
        private System.Windows.Forms.Button btDeleteEmpForHoliday;
        private System.Windows.Forms.DataGridView dgEmpForHoliday;
        private System.Windows.Forms.Label lb303;
        private System.Windows.Forms.Button button1;
    }
}