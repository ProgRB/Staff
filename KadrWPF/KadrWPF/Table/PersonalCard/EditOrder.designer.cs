namespace Tabel
{
    partial class EditOrder
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btAddOrder = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deDate_Order = new LibraryKadr.DateEditor();
            this.lbDate_Order = new System.Windows.Forms.Label();
            this.tbOrder_Name = new System.Windows.Forms.TextBox();
            this.dgOrders = new System.Windows.Forms.DataGridView();
            this.ORDER_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORDER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgOrderEmp = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOrders)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOrderEmp)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btAddOrder);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 416);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(723, 37);
            this.panel1.TabIndex = 3;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(631, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btAddOrder
            // 
            this.btAddOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddOrder.Location = new System.Drawing.Point(16, 8);
            this.btAddOrder.Name = "btAddOrder";
            this.btAddOrder.Size = new System.Drawing.Size(94, 23);
            this.btAddOrder.TabIndex = 0;
            this.btAddOrder.Text = "Добавить";
            this.btAddOrder.Click += new System.EventHandler(this.btAddOrder_Click);
            // 
            // btSave
            // 
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Location = new System.Drawing.Point(119, 8);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(94, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Выбрать";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // formFrameSkinner1
            // 
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(723, 416);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deDate_Order);
            this.groupBox1.Controls.Add(this.lbDate_Order);
            this.groupBox1.Controls.Add(this.tbOrder_Name);
            this.groupBox1.Controls.Add(this.dgOrders);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 416);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // deDate_Order
            // 
            this.deDate_Order.AutoSize = true;
            this.deDate_Order.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Order.Date = null;
            this.deDate_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Order.Location = new System.Drawing.Point(179, 379);
            this.deDate_Order.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.deDate_Order.Name = "deDate_Order";
            this.deDate_Order.ReadOnly = false;
            this.deDate_Order.Size = new System.Drawing.Size(112, 21);
            this.deDate_Order.TabIndex = 37;
            this.deDate_Order.TextDate = null;
            // 
            // lbDate_Order
            // 
            this.lbDate_Order.AutoSize = true;
            this.lbDate_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDate_Order.Location = new System.Drawing.Point(19, 380);
            this.lbDate_Order.Name = "lbDate_Order";
            this.lbDate_Order.Size = new System.Drawing.Size(162, 15);
            this.lbDate_Order.TabIndex = 38;
            this.lbDate_Order.Text = "Дата установки заказа";
            // 
            // tbOrder_Name
            // 
            this.tbOrder_Name.BackColor = System.Drawing.Color.White;
            this.tbOrder_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbOrder_Name.Location = new System.Drawing.Point(179, 347);
            this.tbOrder_Name.Name = "tbOrder_Name";
            this.tbOrder_Name.Size = new System.Drawing.Size(113, 21);
            this.tbOrder_Name.TabIndex = 27;
            this.tbOrder_Name.TextChanged += new System.EventHandler(this.tbOrder_Name_TextChanged);
            // 
            // dgOrders
            // 
            this.dgOrders.AllowUserToAddRows = false;
            this.dgOrders.AllowUserToDeleteRows = false;
            this.dgOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOrders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ORDER_NAME,
            this.ORDER_ID});
            this.dgOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgOrders.Location = new System.Drawing.Point(3, 16);
            this.dgOrders.MultiSelect = false;
            this.dgOrders.Name = "dgOrders";
            this.dgOrders.ReadOnly = true;
            this.dgOrders.RowHeadersWidth = 24;
            this.dgOrders.Size = new System.Drawing.Size(306, 322);
            this.dgOrders.TabIndex = 26;
            // 
            // ORDER_NAME
            // 
            this.ORDER_NAME.DataPropertyName = "ORDER_NAME";
            this.ORDER_NAME.HeaderText = "Номер заказа";
            this.ORDER_NAME.Name = "ORDER_NAME";
            this.ORDER_NAME.ReadOnly = true;
            this.ORDER_NAME.Width = 200;
            // 
            // ORDER_ID
            // 
            this.ORDER_ID.DataPropertyName = "ORDER_ID";
            this.ORDER_ID.HeaderText = "Заказ ID";
            this.ORDER_ID.Name = "ORDER_ID";
            this.ORDER_ID.ReadOnly = true;
            this.ORDER_ID.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(19, 350);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(152, 15);
            this.label15.TabIndex = 25;
            this.label15.Text = "Номер заказа (поиск)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgOrderEmp);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 416);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // dgOrderEmp
            // 
            this.dgOrderEmp.AllowUserToAddRows = false;
            this.dgOrderEmp.AllowUserToDeleteRows = false;
            this.dgOrderEmp.BackgroundColor = System.Drawing.Color.White;
            this.dgOrderEmp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOrderEmp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgOrderEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOrderEmp.Location = new System.Drawing.Point(3, 16);
            this.dgOrderEmp.MultiSelect = false;
            this.dgOrderEmp.Name = "dgOrderEmp";
            this.dgOrderEmp.ReadOnly = true;
            this.dgOrderEmp.RowHeadersWidth = 24;
            this.dgOrderEmp.Size = new System.Drawing.Size(401, 397);
            this.dgOrderEmp.TabIndex = 27;
            // 
            // EditOrder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(723, 453);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выберите номер заказа";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOrders)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOrderEmp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btAddOrder;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        public LibraryKadr.DateEditor deDate_Order;
        public System.Windows.Forms.Label lbDate_Order;
        private System.Windows.Forms.TextBox tbOrder_Name;
        private System.Windows.Forms.DataGridView dgOrders;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgOrderEmp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDER_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDER_ID;
    }
}