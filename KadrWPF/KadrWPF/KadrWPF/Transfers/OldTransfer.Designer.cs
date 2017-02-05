namespace Kadr
{
    partial class OldTransfer
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
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.gbAll = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbForm_Operation = new System.Windows.Forms.ComboBox();
            this.tbCode_Form_Operation = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbClassific = new System.Windows.Forms.TextBox();
            this.tbSalary = new System.Windows.Forms.TextBox();
            this.deDate_Transfer = new LibraryKadr.DateEditor();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSign_Comb = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deTr_Date_Order = new LibraryKadr.DateEditor();
            this.tbTr_Num_Order = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.deDate_Contr = new LibraryKadr.DateEditor();
            this.tbContr_Emp = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbPos_Name = new System.Windows.Forms.ComboBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.cbChar_Work_ID = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCode_Degree = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDegree_Name = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCode_Pos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.deDate_End_Contr = new LibraryKadr.DateEditor();
            this.label6 = new System.Windows.Forms.Label();
            this.pnButton.SuspendLayout();
            this.gbAll.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 318);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(665, 38);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(560, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSave.Location = new System.Drawing.Point(467, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(87, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // gbAll
            // 
            this.gbAll.Controls.Add(this.deDate_End_Contr);
            this.gbAll.Controls.Add(this.label6);
            this.gbAll.Controls.Add(this.label17);
            this.gbAll.Controls.Add(this.cbForm_Operation);
            this.gbAll.Controls.Add(this.tbCode_Form_Operation);
            this.gbAll.Controls.Add(this.label22);
            this.gbAll.Controls.Add(this.label16);
            this.gbAll.Controls.Add(this.tbClassific);
            this.gbAll.Controls.Add(this.tbSalary);
            this.gbAll.Controls.Add(this.deDate_Transfer);
            this.gbAll.Controls.Add(this.label9);
            this.gbAll.Controls.Add(this.cbSign_Comb);
            this.gbAll.Controls.Add(this.groupBox1);
            this.gbAll.Controls.Add(this.groupBox4);
            this.gbAll.Controls.Add(this.cbPos_Name);
            this.gbAll.Controls.Add(this.cbSubdiv_Name);
            this.gbAll.Controls.Add(this.cbChar_Work_ID);
            this.gbAll.Controls.Add(this.label7);
            this.gbAll.Controls.Add(this.tbCode_Degree);
            this.gbAll.Controls.Add(this.label5);
            this.gbAll.Controls.Add(this.cbDegree_Name);
            this.gbAll.Controls.Add(this.label3);
            this.gbAll.Controls.Add(this.tbPer_Num);
            this.gbAll.Controls.Add(this.label4);
            this.gbAll.Controls.Add(this.tbCode_Subdiv);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Controls.Add(this.tbCode_Pos);
            this.gbAll.Controls.Add(this.label2);
            this.gbAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAll.Location = new System.Drawing.Point(0, 0);
            this.gbAll.Name = "gbAll";
            this.gbAll.Size = new System.Drawing.Size(665, 317);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(18, 258);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(130, 15);
            this.label17.TabIndex = 87;
            this.label17.Text = "Вид производства";
            // 
            // cbForm_Operation
            // 
            this.cbForm_Operation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbForm_Operation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbForm_Operation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbForm_Operation.FormattingEnabled = true;
            this.cbForm_Operation.Location = new System.Drawing.Point(217, 255);
            this.cbForm_Operation.Name = "cbForm_Operation";
            this.cbForm_Operation.Size = new System.Drawing.Size(426, 23);
            this.cbForm_Operation.TabIndex = 12;
            this.cbForm_Operation.Leave += new System.EventHandler(this.cbForm_Operation_Leave);
            // 
            // tbCode_Form_Operation
            // 
            this.tbCode_Form_Operation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Form_Operation.Location = new System.Drawing.Point(153, 255);
            this.tbCode_Form_Operation.Multiline = true;
            this.tbCode_Form_Operation.Name = "tbCode_Form_Operation";
            this.tbCode_Form_Operation.Size = new System.Drawing.Size(58, 23);
            this.tbCode_Form_Operation.TabIndex = 11;
            this.tbCode_Form_Operation.Leave += new System.EventHandler(this.tbCode_Form_Operation_Leave);
            this.tbCode_Form_Operation.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Form_Operation_Validating);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(294, 289);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(55, 15);
            this.label22.TabIndex = 83;
            this.label22.Text = "Разряд";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point(18, 289);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(175, 15);
            this.label16.TabIndex = 84;
            this.label16.Text = "Тарифный коэффициент";
            // 
            // tbClassific
            // 
            this.tbClassific.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbClassific.Location = new System.Drawing.Point(355, 286);
            this.tbClassific.Name = "tbClassific";
            this.tbClassific.Size = new System.Drawing.Size(73, 21);
            this.tbClassific.TabIndex = 82;
            // 
            // tbSalary
            // 
            this.tbSalary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSalary.Location = new System.Drawing.Point(217, 286);
            this.tbSalary.Name = "tbSalary";
            this.tbSalary.Size = new System.Drawing.Size(58, 21);
            this.tbSalary.TabIndex = 81;
            // 
            // deDate_Transfer
            // 
            this.deDate_Transfer.AutoSize = true;
            this.deDate_Transfer.Date = null;
            this.deDate_Transfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Transfer.Location = new System.Drawing.Point(153, 111);
            this.deDate_Transfer.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Transfer.Name = "deDate_Transfer";
            this.deDate_Transfer.ReadOnly = false;
            this.deDate_Transfer.Size = new System.Drawing.Size(73, 24);
            this.deDate_Transfer.TabIndex = 5;
            this.deDate_Transfer.TextDate = null;
            this.deDate_Transfer.Validating += new System.ComponentModel.CancelEventHandler(this.deDate_Transfer_Validating);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(551, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 15);
            this.label9.TabIndex = 80;
            this.label9.Text = "Совмещение";
            // 
            // cbSign_Comb
            // 
            this.cbSign_Comb.AutoSize = true;
            this.cbSign_Comb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSign_Comb.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbSign_Comb.Location = new System.Drawing.Point(534, 23);
            this.cbSign_Comb.Name = "cbSign_Comb";
            this.cbSign_Comb.Size = new System.Drawing.Size(111, 19);
            this.cbSign_Comb.TabIndex = 79;
            this.cbSign_Comb.Text = "Совмещение";
            this.cbSign_Comb.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deTr_Date_Order);
            this.groupBox1.Controls.Add(this.tbTr_Num_Order);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(456, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 87);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Приказ о переводе";
            // 
            // deTr_Date_Order
            // 
            this.deTr_Date_Order.AutoSize = true;
            this.deTr_Date_Order.Date = null;
            this.deTr_Date_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deTr_Date_Order.Location = new System.Drawing.Point(89, 54);
            this.deTr_Date_Order.Margin = new System.Windows.Forms.Padding(5);
            this.deTr_Date_Order.Name = "deTr_Date_Order";
            this.deTr_Date_Order.ReadOnly = false;
            this.deTr_Date_Order.Size = new System.Drawing.Size(73, 24);
            this.deTr_Date_Order.TabIndex = 1;
            this.deTr_Date_Order.TextDate = null;
            // 
            // tbTr_Num_Order
            // 
            this.tbTr_Num_Order.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbTr_Num_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTr_Num_Order.Location = new System.Drawing.Point(89, 27);
            this.tbTr_Num_Order.Name = "tbTr_Num_Order";
            this.tbTr_Num_Order.Size = new System.Drawing.Size(73, 21);
            this.tbTr_Num_Order.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(16, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 15);
            this.label12.TabIndex = 66;
            this.label12.Text = "Дата";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(16, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 15);
            this.label13.TabIndex = 65;
            this.label13.Text = "Номер";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.deDate_Contr);
            this.groupBox4.Controls.Add(this.tbContr_Emp);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox4.Location = new System.Drawing.Point(241, 101);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(187, 87);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Трудовой договор";
            // 
            // deDate_Contr
            // 
            this.deDate_Contr.AutoSize = true;
            this.deDate_Contr.Date = null;
            this.deDate_Contr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Contr.Location = new System.Drawing.Point(89, 54);
            this.deDate_Contr.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Contr.Name = "deDate_Contr";
            this.deDate_Contr.ReadOnly = false;
            this.deDate_Contr.Size = new System.Drawing.Size(73, 24);
            this.deDate_Contr.TabIndex = 1;
            this.deDate_Contr.TextDate = null;
            // 
            // tbContr_Emp
            // 
            this.tbContr_Emp.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbContr_Emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbContr_Emp.Location = new System.Drawing.Point(89, 27);
            this.tbContr_Emp.Name = "tbContr_Emp";
            this.tbContr_Emp.Size = new System.Drawing.Size(73, 21);
            this.tbContr_Emp.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(16, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 15);
            this.label10.TabIndex = 66;
            this.label10.Text = "Дата";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(16, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 15);
            this.label11.TabIndex = 65;
            this.label11.Text = "Номер";
            // 
            // cbPos_Name
            // 
            this.cbPos_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbPos_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPos_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPos_Name.FormattingEnabled = true;
            this.cbPos_Name.Location = new System.Drawing.Point(217, 71);
            this.cbPos_Name.Name = "cbPos_Name";
            this.cbPos_Name.Size = new System.Drawing.Size(426, 23);
            this.cbPos_Name.TabIndex = 4;
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(217, 46);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(426, 23);
            this.cbSubdiv_Name.TabIndex = 2;
            // 
            // cbChar_Work_ID
            // 
            this.cbChar_Work_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbChar_Work_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbChar_Work_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbChar_Work_ID.FormattingEnabled = true;
            this.cbChar_Work_ID.Location = new System.Drawing.Point(153, 197);
            this.cbChar_Work_ID.Name = "cbChar_Work_ID";
            this.cbChar_Work_ID.Size = new System.Drawing.Size(490, 23);
            this.cbChar_Work_ID.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(18, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 15);
            this.label7.TabIndex = 62;
            this.label7.Text = "Дата перевода";
            // 
            // tbCode_Degree
            // 
            this.tbCode_Degree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Degree.Location = new System.Drawing.Point(153, 226);
            this.tbCode_Degree.Multiline = true;
            this.tbCode_Degree.Name = "tbCode_Degree";
            this.tbCode_Degree.Size = new System.Drawing.Size(58, 23);
            this.tbCode_Degree.TabIndex = 9;
            this.tbCode_Degree.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Degree_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(18, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 64;
            this.label5.Text = "Категория";
            // 
            // cbDegree_Name
            // 
            this.cbDegree_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbDegree_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDegree_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDegree_Name.FormattingEnabled = true;
            this.cbDegree_Name.Location = new System.Drawing.Point(217, 226);
            this.cbDegree_Name.Name = "cbDegree_Name";
            this.cbDegree_Name.Size = new System.Drawing.Size(426, 23);
            this.cbDegree_Name.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(18, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 15);
            this.label3.TabIndex = 63;
            this.label3.Text = "Срок договора";
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.BackColor = System.Drawing.Color.White;
            this.tbPer_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_Num.Location = new System.Drawing.Point(153, 21);
            this.tbPer_Num.MaxLength = 5;
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(58, 21);
            this.tbPer_Num.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(18, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 15);
            this.label4.TabIndex = 50;
            this.label4.Text = "Табельный номер";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(153, 46);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Subdiv.TabIndex = 1;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(18, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 48;
            this.label1.Text = "Подразделение";
            // 
            // tbCode_Pos
            // 
            this.tbCode_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Pos.Location = new System.Drawing.Point(153, 73);
            this.tbCode_Pos.Name = "tbCode_Pos";
            this.tbCode_Pos.Size = new System.Drawing.Size(58, 21);
            this.tbCode_Pos.TabIndex = 3;
            this.tbCode_Pos.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Pos_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(18, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 49;
            this.label2.Text = "Профессия";
            // 
            // deDate_End_Contr
            // 
            this.deDate_End_Contr.AutoSize = true;
            this.deDate_End_Contr.Date = null;
            this.deDate_End_Contr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_End_Contr.Location = new System.Drawing.Point(153, 152);
            this.deDate_End_Contr.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_End_Contr.Name = "deDate_End_Contr";
            this.deDate_End_Contr.ReadOnly = false;
            this.deDate_End_Contr.Size = new System.Drawing.Size(73, 24);
            this.deDate_End_Contr.TabIndex = 88;
            this.deDate_End_Contr.TextDate = null;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(18, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 15);
            this.label6.TabIndex = 89;
            this.label6.Text = "Срок работы";
            // 
            // OldTransfer
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(665, 356);
            this.Controls.Add(this.gbAll);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OldTransfer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransferOld";
            this.pnButton.ResumeLayout(false);
            this.gbAll.ResumeLayout(false);
            this.gbAll.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox gbAll;
        private System.Windows.Forms.ComboBox cbPos_Name;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.ComboBox cbChar_Work_ID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbCode_Degree;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDegree_Name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCode_Pos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbContr_Emp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbTr_Num_Order;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbSign_Comb;
        private LibraryKadr.DateEditor deDate_Transfer;
        private LibraryKadr.DateEditor deTr_Date_Order;
        private LibraryKadr.DateEditor deDate_Contr;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbClassific;
        private System.Windows.Forms.TextBox tbSalary;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbForm_Operation;
        private System.Windows.Forms.TextBox tbCode_Form_Operation;
        private LibraryKadr.DateEditor deDate_End_Contr;
        private System.Windows.Forms.Label label6;
    }
}