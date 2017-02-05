namespace Tabel
{
    partial class OrdersOnHoliday
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.STip = new Elegant.Ui.ScreenTip();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbPay_Type = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btAddOrderOnFIO = new System.Windows.Forms.Button();
            this.btCopyOrder = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btAddOrder = new System.Windows.Forms.Button();
            this.btDeleteOrder = new System.Windows.Forms.Button();
            this.btEditOrder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgOrder_On_Holiday = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgDate_For_Order = new System.Windows.Forms.DataGridView();
            this.ssOrderOnHoliday = new Kadr.Classes.SubdivSelector();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOrder_On_Holiday)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDate_For_Order)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.label1.Location = new System.Drawing.Point(21, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Период:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.ssOrderOnHoliday);
            this.panel1.Controls.Add(this.cbPay_Type);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.dtpBegin);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 82);
            this.panel1.TabIndex = 1;
            // 
            // cbPay_Type
            // 
            this.cbPay_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPay_Type.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPay_Type.FormattingEnabled = true;
            this.cbPay_Type.Location = new System.Drawing.Point(481, 45);
            this.cbPay_Type.Name = "cbPay_Type";
            this.cbPay_Type.Size = new System.Drawing.Size(367, 21);
            this.cbPay_Type.TabIndex = 26;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.label15.Location = new System.Drawing.Point(378, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(97, 19);
            this.label15.TabIndex = 27;
            this.label15.Text = "Тип документа";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(256, 45);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(102, 21);
            this.dtpEnd.TabIndex = 7;
            // 
            // dtpBegin
            // 
            this.dtpBegin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpBegin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBegin.Location = new System.Drawing.Point(122, 45);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(102, 21);
            this.dtpBegin.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.label3.Location = new System.Drawing.Point(102, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "с";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.label2.Location = new System.Drawing.Point(232, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "по";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btAddOrderOnFIO);
            this.panel2.Controls.Add(this.btCopyOrder);
            this.panel2.Controls.Add(this.btExit);
            this.panel2.Controls.Add(this.btAddOrder);
            this.panel2.Controls.Add(this.btDeleteOrder);
            this.panel2.Controls.Add(this.btEditOrder);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 444);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(875, 41);
            this.panel2.TabIndex = 2;
            // 
            // btAddOrderOnFIO
            // 
            this.btAddOrderOnFIO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddOrderOnFIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddOrderOnFIO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btAddOrderOnFIO.Location = new System.Drawing.Point(196, 9);
            this.btAddOrderOnFIO.Name = "btAddOrderOnFIO";
            this.btAddOrderOnFIO.Size = new System.Drawing.Size(251, 24);
            this.btAddOrderOnFIO.TabIndex = 7;
            this.btAddOrderOnFIO.Text = "Добавить пофамильный приказ";
            this.btAddOrderOnFIO.UseVisualStyleBackColor = true;
            this.btAddOrderOnFIO.Visible = false;
            this.btAddOrderOnFIO.Click += new System.EventHandler(this.btAddOrderOnFIO_Click);
            // 
            // btCopyOrder
            // 
            this.btCopyOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCopyOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCopyOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btCopyOrder.Location = new System.Drawing.Point(6, 9);
            this.btCopyOrder.Name = "btCopyOrder";
            this.btCopyOrder.Size = new System.Drawing.Size(168, 24);
            this.btCopyOrder.TabIndex = 6;
            this.btCopyOrder.Text = "Копировать приказ";
            this.btCopyOrder.UseVisualStyleBackColor = true;
            this.btCopyOrder.Click += new System.EventHandler(this.btCopyOrder_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btExit.Location = new System.Drawing.Point(776, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 5;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btAddOrder
            // 
            this.btAddOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btAddOrder.Location = new System.Drawing.Point(453, 9);
            this.btAddOrder.Name = "btAddOrder";
            this.btAddOrder.Size = new System.Drawing.Size(87, 24);
            this.btAddOrder.TabIndex = 4;
            this.btAddOrder.Text = "Добавить";
            this.btAddOrder.UseVisualStyleBackColor = true;
            this.btAddOrder.Click += new System.EventHandler(this.btAddOrder_Click);
            // 
            // btDeleteOrder
            // 
            this.btDeleteOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeleteOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDeleteOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btDeleteOrder.Location = new System.Drawing.Point(683, 9);
            this.btDeleteOrder.Name = "btDeleteOrder";
            this.btDeleteOrder.Size = new System.Drawing.Size(87, 24);
            this.btDeleteOrder.TabIndex = 4;
            this.btDeleteOrder.Text = "Удалить";
            this.btDeleteOrder.UseVisualStyleBackColor = true;
            this.btDeleteOrder.Click += new System.EventHandler(this.btDeleteOrder_Click);
            // 
            // btEditOrder
            // 
            this.btEditOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btEditOrder.Location = new System.Drawing.Point(546, 9);
            this.btEditOrder.Name = "btEditOrder";
            this.btEditOrder.Size = new System.Drawing.Size(131, 24);
            this.btEditOrder.TabIndex = 4;
            this.btEditOrder.Text = "Редактировать";
            this.btEditOrder.UseVisualStyleBackColor = true;
            this.btEditOrder.Click += new System.EventHandler(this.btEditOrder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgOrder_On_Holiday);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 362);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список приказов за выбранный период";
            // 
            // dgOrder_On_Holiday
            // 
            this.dgOrder_On_Holiday.AllowUserToAddRows = false;
            this.dgOrder_On_Holiday.AllowUserToDeleteRows = false;
            this.dgOrder_On_Holiday.BackgroundColor = System.Drawing.Color.White;
            this.dgOrder_On_Holiday.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgOrder_On_Holiday.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgOrder_On_Holiday.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOrder_On_Holiday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOrder_On_Holiday.Location = new System.Drawing.Point(3, 18);
            this.dgOrder_On_Holiday.MultiSelect = false;
            this.dgOrder_On_Holiday.Name = "dgOrder_On_Holiday";
            this.dgOrder_On_Holiday.ReadOnly = true;
            this.dgOrder_On_Holiday.RowHeadersVisible = false;
            this.dgOrder_On_Holiday.RowHeadersWidth = 24;
            this.dgOrder_On_Holiday.Size = new System.Drawing.Size(869, 196);
            this.dgOrder_On_Holiday.TabIndex = 4;
            this.dgOrder_On_Holiday.SelectionChanged += new System.EventHandler(this.dgOrder_On_Holiday_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgDate_For_Order);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(869, 145);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дни работы по приказу";
            // 
            // dgDate_For_Order
            // 
            this.dgDate_For_Order.AllowUserToAddRows = false;
            this.dgDate_For_Order.AllowUserToDeleteRows = false;
            this.dgDate_For_Order.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgDate_For_Order.BackgroundColor = System.Drawing.Color.White;
            this.dgDate_For_Order.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDate_For_Order.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgDate_For_Order.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDate_For_Order.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDate_For_Order.Location = new System.Drawing.Point(3, 18);
            this.dgDate_For_Order.MultiSelect = false;
            this.dgDate_For_Order.Name = "dgDate_For_Order";
            this.dgDate_For_Order.ReadOnly = true;
            this.dgDate_For_Order.RowHeadersVisible = false;
            this.dgDate_For_Order.RowHeadersWidth = 24;
            this.dgDate_For_Order.Size = new System.Drawing.Size(863, 124);
            this.dgDate_For_Order.TabIndex = 5;
            this.dgDate_For_Order.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgDate_For_Order_CellFormatting);
            // 
            // ssOrderOnHoliday
            // 
            this.ssOrderOnHoliday.BackColor = System.Drawing.Color.Transparent;
            this.ssOrderOnHoliday.ByRule = "TABLE";
            this.ssOrderOnHoliday.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ssOrderOnHoliday.IsAllPlantSelectable = true;
            this.ssOrderOnHoliday.Location = new System.Drawing.Point(24, 15);
            this.ssOrderOnHoliday.Name = "ssOrderOnHoliday";
            this.ssOrderOnHoliday.Size = new System.Drawing.Size(824, 19);
            this.ssOrderOnHoliday.subdiv_id = null;
            this.ssOrderOnHoliday.TabIndex = 80;
            this.ssOrderOnHoliday.SubdivChanged += new System.EventHandler(this.ssOrderOnHoliday_SubdivChanged);
            // 
            // OrdersOnHoliday
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(875, 485);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "OrdersOnHoliday";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Приказы на работу в выходные дни";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOrder_On_Holiday)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDate_For_Order)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private Elegant.Ui.ScreenTip STip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btEditOrder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btAddOrder;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.DataGridView dgOrder_On_Holiday;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgDate_For_Order;
        private System.Windows.Forms.Button btDeleteOrder;
        private System.Windows.Forms.ComboBox cbPay_Type;
        private System.Windows.Forms.Label label15;
        private Kadr.Classes.SubdivSelector ssOrderOnHoliday;
        private System.Windows.Forms.Button btCopyOrder;
        private System.Windows.Forms.Button btAddOrderOnFIO;
    }
}