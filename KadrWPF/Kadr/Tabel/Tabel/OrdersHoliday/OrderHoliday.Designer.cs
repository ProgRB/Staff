namespace Tabel
{
    partial class OrderHoliday
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderHoliday));
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btPrintCoupon = new System.Windows.Forms.Button();
            this.btClosingOrder = new System.Windows.Forms.Button();
            this.btEditOrder = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btOrder = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lvEmp = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lvEmp2 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgDate_For_Order = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btDeleteDate_For_Order = new System.Windows.Forms.Button();
            this.btEditDate_For_Order = new System.Windows.Forms.Button();
            this.btAddDate_For_Order = new System.Windows.Forms.Button();
            this.pnData = new System.Windows.Forms.Panel();
            this.gbClosign_Order = new System.Windows.Forms.GroupBox();
            this.btSaveDate_Closing = new System.Windows.Forms.Button();
            this.chDate_Closing_Order = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.deDate_Closing_Order = new EditorsLibrary.DateEditor();
            this.chSign_Order_Plant = new System.Windows.Forms.CheckBox();
            this.cbPay_Type = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbNum_Order = new System.Windows.Forms.TextBox();
            this.deDate_Order = new EditorsLibrary.DateEditor();
            this.tbBase = new System.Windows.Forms.TextBox();
            this.tbOfficial = new System.Windows.Forms.TextBox();
            this.label82 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDate_For_Order)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnData.SuspendLayout();
            this.gbClosign_Order.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btPrintCoupon);
            this.pnButton.Controls.Add(this.btClosingOrder);
            this.pnButton.Controls.Add(this.btEditOrder);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Controls.Add(this.btOrder);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 479);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(765, 50);
            this.pnButton.TabIndex = 1;
            // 
            // btPrintCoupon
            // 
            this.btPrintCoupon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPrintCoupon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btPrintCoupon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btPrintCoupon.Location = new System.Drawing.Point(564, 5);
            this.btPrintCoupon.Name = "btPrintCoupon";
            this.btPrintCoupon.Size = new System.Drawing.Size(88, 39);
            this.btPrintCoupon.TabIndex = 5;
            this.btPrintCoupon.Text = "Талоны на работу";
            this.btPrintCoupon.UseVisualStyleBackColor = true;
            this.btPrintCoupon.Click += new System.EventHandler(this.btPrintCoupon_Click);
            // 
            // btClosingOrder
            // 
            this.btClosingOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClosingOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClosingOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btClosingOrder.Location = new System.Drawing.Point(12, 5);
            this.btClosingOrder.Name = "btClosingOrder";
            this.btClosingOrder.Size = new System.Drawing.Size(159, 39);
            this.btClosingOrder.TabIndex = 4;
            this.btClosingOrder.Text = "Закрыть приказ без проверки лимитов";
            this.btClosingOrder.UseVisualStyleBackColor = true;
            this.btClosingOrder.Click += new System.EventHandler(this.btClosingOrder_Click);
            // 
            // btEditOrder
            // 
            this.btEditOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btEditOrder.Location = new System.Drawing.Point(282, 5);
            this.btEditOrder.Name = "btEditOrder";
            this.btEditOrder.Size = new System.Drawing.Size(88, 39);
            this.btEditOrder.TabIndex = 0;
            this.btEditOrder.Text = "Изменить приказ";
            this.btEditOrder.UseVisualStyleBackColor = true;
            this.btEditOrder.Click += new System.EventHandler(this.btEditOrder_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btExit.Location = new System.Drawing.Point(658, 5);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 39);
            this.btExit.TabIndex = 3;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btSave.Location = new System.Drawing.Point(376, 5);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 39);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Сохранить изменения";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btOrder
            // 
            this.btOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btOrder.Location = new System.Drawing.Point(470, 5);
            this.btOrder.Name = "btOrder";
            this.btOrder.Size = new System.Drawing.Size(88, 39);
            this.btOrder.TabIndex = 2;
            this.btOrder.Text = "Просмотр и печать";
            this.btOrder.UseVisualStyleBackColor = true;
            this.btOrder.Click += new System.EventHandler(this.btOrder_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 0;
            // 
            // lvEmp
            // 
            this.lvEmp.Location = new System.Drawing.Point(0, 0);
            this.lvEmp.Name = "lvEmp";
            this.lvEmp.Size = new System.Drawing.Size(121, 97);
            this.lvEmp.TabIndex = 0;
            this.lvEmp.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Сотрудники подразделения";
            this.columnHeader1.Width = 300;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 0;
            // 
            // lvEmp2
            // 
            this.lvEmp2.Location = new System.Drawing.Point(0, 0);
            this.lvEmp2.Name = "lvEmp2";
            this.lvEmp2.Size = new System.Drawing.Size(121, 97);
            this.lvEmp2.TabIndex = 0;
            this.lvEmp2.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Сотрудники подразделения";
            this.columnHeader4.Width = 300;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.pnData);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(765, 479);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(3, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 195);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Рабочие дни по приказу";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgDate_For_Order);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(753, 175);
            this.panel1.TabIndex = 16;
            // 
            // dgDate_For_Order
            // 
            this.dgDate_For_Order.AllowUserToAddRows = false;
            this.dgDate_For_Order.AllowUserToDeleteRows = false;
            this.dgDate_For_Order.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgDate_For_Order.BackgroundColor = System.Drawing.Color.White;
            this.dgDate_For_Order.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDate_For_Order.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDate_For_Order.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDate_For_Order.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDate_For_Order.Location = new System.Drawing.Point(0, 0);
            this.dgDate_For_Order.Name = "dgDate_For_Order";
            this.dgDate_For_Order.ReadOnly = true;
            this.dgDate_For_Order.Size = new System.Drawing.Size(726, 175);
            this.dgDate_For_Order.TabIndex = 0;
            this.dgDate_For_Order.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgDate_For_Order_CellFormatting);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btDeleteDate_For_Order);
            this.panel2.Controls.Add(this.btEditDate_For_Order);
            this.panel2.Controls.Add(this.btAddDate_For_Order);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(726, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(27, 175);
            this.panel2.TabIndex = 19;
            // 
            // btDeleteDate_For_Order
            // 
            this.btDeleteDate_For_Order.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btDeleteDate_For_Order.BackgroundImage")));
            this.btDeleteDate_For_Order.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btDeleteDate_For_Order.Dock = System.Windows.Forms.DockStyle.Top;
            this.btDeleteDate_For_Order.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btDeleteDate_For_Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDeleteDate_For_Order.Location = new System.Drawing.Point(0, 52);
            this.btDeleteDate_For_Order.Name = "btDeleteDate_For_Order";
            this.btDeleteDate_For_Order.Size = new System.Drawing.Size(27, 26);
            this.btDeleteDate_For_Order.TabIndex = 2;
            this.btDeleteDate_For_Order.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btDeleteDate_For_Order.UseVisualStyleBackColor = true;
            this.btDeleteDate_For_Order.Click += new System.EventHandler(this.btDeleteDate_For_Order_Click);
            // 
            // btEditDate_For_Order
            // 
            this.btEditDate_For_Order.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btEditDate_For_Order.BackgroundImage")));
            this.btEditDate_For_Order.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEditDate_For_Order.Dock = System.Windows.Forms.DockStyle.Top;
            this.btEditDate_For_Order.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btEditDate_For_Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEditDate_For_Order.Location = new System.Drawing.Point(0, 26);
            this.btEditDate_For_Order.Name = "btEditDate_For_Order";
            this.btEditDate_For_Order.Size = new System.Drawing.Size(27, 26);
            this.btEditDate_For_Order.TabIndex = 1;
            this.btEditDate_For_Order.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btEditDate_For_Order.UseVisualStyleBackColor = true;
            this.btEditDate_For_Order.Click += new System.EventHandler(this.btEditDate_For_Order_Click);
            // 
            // btAddDate_For_Order
            // 
            this.btAddDate_For_Order.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btAddDate_For_Order.BackgroundImage")));
            this.btAddDate_For_Order.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAddDate_For_Order.Dock = System.Windows.Forms.DockStyle.Top;
            this.btAddDate_For_Order.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btAddDate_For_Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddDate_For_Order.Location = new System.Drawing.Point(0, 0);
            this.btAddDate_For_Order.Name = "btAddDate_For_Order";
            this.btAddDate_For_Order.Size = new System.Drawing.Size(27, 26);
            this.btAddDate_For_Order.TabIndex = 0;
            this.btAddDate_For_Order.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btAddDate_For_Order.UseVisualStyleBackColor = true;
            this.btAddDate_For_Order.Click += new System.EventHandler(this.btAddDate_For_Order_Click);
            // 
            // pnData
            // 
            this.pnData.Controls.Add(this.gbClosign_Order);
            this.pnData.Controls.Add(this.chSign_Order_Plant);
            this.pnData.Controls.Add(this.cbPay_Type);
            this.pnData.Controls.Add(this.label15);
            this.pnData.Controls.Add(this.tbNum_Order);
            this.pnData.Controls.Add(this.deDate_Order);
            this.pnData.Controls.Add(this.tbBase);
            this.pnData.Controls.Add(this.tbOfficial);
            this.pnData.Controls.Add(this.label82);
            this.pnData.Controls.Add(this.label4);
            this.pnData.Controls.Add(this.label3);
            this.pnData.Controls.Add(this.label1);
            this.pnData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnData.Location = new System.Drawing.Point(3, 16);
            this.pnData.Name = "pnData";
            this.pnData.Size = new System.Drawing.Size(759, 265);
            this.pnData.TabIndex = 0;
            // 
            // gbClosign_Order
            // 
            this.gbClosign_Order.Controls.Add(this.btSaveDate_Closing);
            this.gbClosign_Order.Controls.Add(this.chDate_Closing_Order);
            this.gbClosign_Order.Controls.Add(this.label2);
            this.gbClosign_Order.Controls.Add(this.deDate_Closing_Order);
            this.gbClosign_Order.Location = new System.Drawing.Point(555, -1);
            this.gbClosign_Order.Name = "gbClosign_Order";
            this.gbClosign_Order.Size = new System.Drawing.Size(204, 82);
            this.gbClosign_Order.TabIndex = 75;
            this.gbClosign_Order.TabStop = false;
            this.gbClosign_Order.Text = "Закрытие приказа";
            // 
            // btSaveDate_Closing
            // 
            this.btSaveDate_Closing.BackgroundImage = global::Tabel.Properties.Resources.Save_Large;
            this.btSaveDate_Closing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btSaveDate_Closing.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btSaveDate_Closing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSaveDate_Closing.Location = new System.Drawing.Point(168, 19);
            this.btSaveDate_Closing.Name = "btSaveDate_Closing";
            this.btSaveDate_Closing.Size = new System.Drawing.Size(27, 26);
            this.btSaveDate_Closing.TabIndex = 75;
            this.btSaveDate_Closing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btSaveDate_Closing.UseVisualStyleBackColor = true;
            this.btSaveDate_Closing.Click += new System.EventHandler(this.btSaveDate_Closing_Click);
            // 
            // chDate_Closing_Order
            // 
            this.chDate_Closing_Order.AutoSize = true;
            this.chDate_Closing_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chDate_Closing_Order.Location = new System.Drawing.Point(6, 23);
            this.chDate_Closing_Order.Name = "chDate_Closing_Order";
            this.chDate_Closing_Order.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chDate_Closing_Order.Size = new System.Drawing.Size(126, 19);
            this.chDate_Closing_Order.TabIndex = 74;
            this.chDate_Closing_Order.Text = "Приказ закрыт";
            this.chDate_Closing_Order.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(7, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 73;
            this.label2.Text = "Дата закрытия";
            // 
            // deDate_Closing_Order
            // 
            this.deDate_Closing_Order.AutoSize = true;
            this.deDate_Closing_Order.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Closing_Order.Date = null;
            this.deDate_Closing_Order.Enabled = false;
            this.deDate_Closing_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Closing_Order.Location = new System.Drawing.Point(116, 51);
            this.deDate_Closing_Order.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.deDate_Closing_Order.Name = "deDate_Closing_Order";
            this.deDate_Closing_Order.ReadOnly = false;
            this.deDate_Closing_Order.Size = new System.Drawing.Size(79, 24);
            this.deDate_Closing_Order.TabIndex = 72;
            this.deDate_Closing_Order.TextDate = null;
            // 
            // chSign_Order_Plant
            // 
            this.chSign_Order_Plant.AutoSize = true;
            this.chSign_Order_Plant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Order_Plant.Location = new System.Drawing.Point(406, 8);
            this.chSign_Order_Plant.Name = "chSign_Order_Plant";
            this.chSign_Order_Plant.Size = new System.Drawing.Size(143, 19);
            this.chSign_Order_Plant.TabIndex = 2;
            this.chSign_Order_Plant.Text = "Приказ по заводу";
            this.chSign_Order_Plant.UseVisualStyleBackColor = true;
            // 
            // cbPay_Type
            // 
            this.cbPay_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPay_Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPay_Type.FormattingEnabled = true;
            this.cbPay_Type.Location = new System.Drawing.Point(121, 36);
            this.cbPay_Type.Name = "cbPay_Type";
            this.cbPay_Type.Size = new System.Drawing.Size(427, 24);
            this.cbPay_Type.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(9, 39);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 15);
            this.label15.TabIndex = 71;
            this.label15.Text = "Тип документа";
            // 
            // tbNum_Order
            // 
            this.tbNum_Order.BackColor = System.Drawing.Color.White;
            this.tbNum_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Order.Location = new System.Drawing.Point(121, 6);
            this.tbNum_Order.Name = "tbNum_Order";
            this.tbNum_Order.Size = new System.Drawing.Size(59, 21);
            this.tbNum_Order.TabIndex = 0;
            // 
            // deDate_Order
            // 
            this.deDate_Order.AutoSize = true;
            this.deDate_Order.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Order.Date = null;
            this.deDate_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Order.Location = new System.Drawing.Point(302, 6);
            this.deDate_Order.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.deDate_Order.Name = "deDate_Order";
            this.deDate_Order.ReadOnly = false;
            this.deDate_Order.Size = new System.Drawing.Size(84, 24);
            this.deDate_Order.TabIndex = 1;
            this.deDate_Order.TextDate = null;
            // 
            // tbBase
            // 
            this.tbBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBase.BackColor = System.Drawing.Color.White;
            this.tbBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBase.Location = new System.Drawing.Point(9, 82);
            this.tbBase.Multiline = true;
            this.tbBase.Name = "tbBase";
            this.tbBase.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbBase.Size = new System.Drawing.Size(741, 126);
            this.tbBase.TabIndex = 4;
            // 
            // tbOfficial
            // 
            this.tbOfficial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOfficial.BackColor = System.Drawing.Color.White;
            this.tbOfficial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbOfficial.Location = new System.Drawing.Point(293, 214);
            this.tbOfficial.Multiline = true;
            this.tbOfficial.Name = "tbOfficial";
            this.tbOfficial.Size = new System.Drawing.Size(457, 44);
            this.tbOfficial.TabIndex = 5;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label82.Location = new System.Drawing.Point(9, 64);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(514, 15);
            this.label82.TabIndex = 69;
            this.label82.Text = "Основание приказа - В связи со срочностью выполнения следующих работ:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 67;
            this.label4.Text = "№ приказа";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(198, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 65;
            this.label3.Text = "Дата приказа";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 35);
            this.label1.TabIndex = 66;
            this.label1.Text = "Должностное лицо подразделения (кому необходимо организовать рабочие дни)";
            // 
            // OrderHoliday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 529);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnButton);
            this.MaximizeBox = false;
            this.Name = "OrderHoliday";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Приказ на работу в выходные дни";
            this.pnButton.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDate_For_Order)).EndInit();
            this.panel2.ResumeLayout(false);
            this.pnData.ResumeLayout(false);
            this.pnData.PerformLayout();
            this.gbClosign_Order.ResumeLayout(false);
            this.gbClosign_Order.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btOrder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lvEmp;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView lvEmp2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btEditOrder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnData;
        private System.Windows.Forms.TextBox tbNum_Order;
        private EditorsLibrary.DateEditor deDate_Order;
        private System.Windows.Forms.TextBox tbBase;
        private System.Windows.Forms.TextBox tbOfficial;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgDate_For_Order;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Button btDeleteDate_For_Order;
        public System.Windows.Forms.Button btEditDate_For_Order;
        public System.Windows.Forms.Button btAddDate_For_Order;
        private System.Windows.Forms.ComboBox cbPay_Type;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chSign_Order_Plant;
        private System.Windows.Forms.GroupBox gbClosign_Order;
        private System.Windows.Forms.CheckBox chDate_Closing_Order;
        private System.Windows.Forms.Label label2;
        private EditorsLibrary.DateEditor deDate_Closing_Order;
        public System.Windows.Forms.Button btSaveDate_Closing;
        private System.Windows.Forms.Button btClosingOrder;
        private System.Windows.Forms.Button btPrintCoupon;
    }
}