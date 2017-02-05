namespace Tabel
{
    partial class Edit_Group_Master
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btSave = new System.Windows.Forms.Button();
            this.btDeleteGM = new System.Windows.Forms.Button();
            this.btEditGM = new System.Windows.Forms.Button();
            this.btAddGM = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mbName_GM = new System.Windows.Forms.MaskedTextBox();
            this.deEnd_Group = new LibraryKadr.DateEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.deBegin_Group = new LibraryKadr.DateEditor();
            this.lbDate_Order = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.pnWorkReg_Doc = new System.Windows.Forms.Panel();
            this.dgHistoryGM = new System.Windows.Forms.DataGridView();
            this.btRefresh_Group_Master = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnWorkReg_Doc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgHistoryGM)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 357);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(533, 37);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(441, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.BackgroundImage = global::KadrWPF.Properties.Resources.document_save;
            this.btSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSave.Location = new System.Drawing.Point(0, 78);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(27, 26);
            this.btSave.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btSave, "Сохранить изменения");
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btDeleteGM
            // 
            this.btDeleteGM.BackgroundImage = global::KadrWPF.Properties.Resources.Remove;
            this.btDeleteGM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btDeleteGM.Dock = System.Windows.Forms.DockStyle.Top;
            this.btDeleteGM.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btDeleteGM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDeleteGM.Location = new System.Drawing.Point(0, 52);
            this.btDeleteGM.Name = "btDeleteGM";
            this.btDeleteGM.Size = new System.Drawing.Size(27, 26);
            this.btDeleteGM.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btDeleteGM, "Удалить группу мастера");
            this.btDeleteGM.UseVisualStyleBackColor = true;
            this.btDeleteGM.Click += new System.EventHandler(this.btDeleteGM_Click);
            // 
            // btEditGM
            // 
            this.btEditGM.BackgroundImage = global::KadrWPF.Properties.Resources.Prepare_Large;
            this.btEditGM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEditGM.Dock = System.Windows.Forms.DockStyle.Top;
            this.btEditGM.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btEditGM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEditGM.Location = new System.Drawing.Point(0, 26);
            this.btEditGM.Name = "btEditGM";
            this.btEditGM.Size = new System.Drawing.Size(27, 26);
            this.btEditGM.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btEditGM, "Редактировать группу мастера");
            this.btEditGM.UseVisualStyleBackColor = true;
            this.btEditGM.Click += new System.EventHandler(this.btEditGM_Click);
            // 
            // btAddGM
            // 
            this.btAddGM.BackgroundImage = global::KadrWPF.Properties.Resources.document_new_3232;
            this.btAddGM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAddGM.Dock = System.Windows.Forms.DockStyle.Top;
            this.btAddGM.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btAddGM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddGM.Location = new System.Drawing.Point(0, 0);
            this.btAddGM.Name = "btAddGM";
            this.btAddGM.Size = new System.Drawing.Size(27, 26);
            this.btAddGM.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btAddGM, "Добавить группу мастера");
            this.btAddGM.UseVisualStyleBackColor = true;
            this.btAddGM.Click += new System.EventHandler(this.btAddGM_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mbName_GM);
            this.groupBox1.Controls.Add(this.deEnd_Group);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.deBegin_Group);
            this.groupBox1.Controls.Add(this.lbDate_Order);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 303);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 54);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // mbName_GM
            // 
            this.mbName_GM.BackColor = System.Drawing.Color.Gainsboro;
            this.mbName_GM.Location = new System.Drawing.Point(83, 22);
            this.mbName_GM.Name = "mbName_GM";
            this.mbName_GM.Size = new System.Drawing.Size(43, 20);
            this.mbName_GM.TabIndex = 0;
            // 
            // deEnd_Group
            // 
            this.deEnd_Group.AutoSize = true;
            this.deEnd_Group.BackColor = System.Drawing.Color.Gainsboro;
            this.deEnd_Group.Date = null;
            this.deEnd_Group.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deEnd_Group.Location = new System.Drawing.Point(438, 22);
            this.deEnd_Group.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.deEnd_Group.Name = "deEnd_Group";
            this.deEnd_Group.ReadOnly = false;
            this.deEnd_Group.Size = new System.Drawing.Size(70, 21);
            this.deEnd_Group.TabIndex = 2;
            this.deEnd_Group.TextDate = null;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(320, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 32);
            this.label1.TabIndex = 74;
            this.label1.Text = "Дата окончания работы";
            // 
            // deBegin_Group
            // 
            this.deBegin_Group.AutoSize = true;
            this.deBegin_Group.BackColor = System.Drawing.Color.Gainsboro;
            this.deBegin_Group.Date = null;
            this.deBegin_Group.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deBegin_Group.Location = new System.Drawing.Point(235, 22);
            this.deBegin_Group.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.deBegin_Group.Name = "deBegin_Group";
            this.deBegin_Group.ReadOnly = false;
            this.deBegin_Group.Size = new System.Drawing.Size(71, 21);
            this.deBegin_Group.TabIndex = 1;
            this.deBegin_Group.TextDate = null;
            // 
            // lbDate_Order
            // 
            this.lbDate_Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDate_Order.Location = new System.Drawing.Point(139, 16);
            this.lbDate_Order.Name = "lbDate_Order";
            this.lbDate_Order.Size = new System.Drawing.Size(100, 32);
            this.lbDate_Order.TabIndex = 73;
            this.lbDate_Order.Text = "Дата начала работы";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(12, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 43);
            this.label15.TabIndex = 70;
            this.label15.Text = "Группа мастера";
            // 
            // pnWorkReg_Doc
            // 
            this.pnWorkReg_Doc.BackColor = System.Drawing.SystemColors.Control;
            this.pnWorkReg_Doc.Controls.Add(this.btRefresh_Group_Master);
            this.pnWorkReg_Doc.Controls.Add(this.btSave);
            this.pnWorkReg_Doc.Controls.Add(this.btDeleteGM);
            this.pnWorkReg_Doc.Controls.Add(this.btEditGM);
            this.pnWorkReg_Doc.Controls.Add(this.btAddGM);
            this.pnWorkReg_Doc.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnWorkReg_Doc.Location = new System.Drawing.Point(506, 0);
            this.pnWorkReg_Doc.Name = "pnWorkReg_Doc";
            this.pnWorkReg_Doc.Size = new System.Drawing.Size(27, 303);
            this.pnWorkReg_Doc.TabIndex = 9;
            // 
            // dgHistoryGM
            // 
            this.dgHistoryGM.AllowUserToAddRows = false;
            this.dgHistoryGM.AllowUserToDeleteRows = false;
            this.dgHistoryGM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgHistoryGM.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgHistoryGM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgHistoryGM.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgHistoryGM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgHistoryGM.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgHistoryGM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgHistoryGM.Location = new System.Drawing.Point(0, 0);
            this.dgHistoryGM.Margin = new System.Windows.Forms.Padding(0);
            this.dgHistoryGM.Name = "dgHistoryGM";
            this.dgHistoryGM.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgHistoryGM.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgHistoryGM.RowHeadersWidth = 20;
            this.dgHistoryGM.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgHistoryGM.Size = new System.Drawing.Size(506, 303);
            this.dgHistoryGM.TabIndex = 10;
            // 
            // btRefresh_Group_Master
            // 
            this.btRefresh_Group_Master.BackgroundImage = global::KadrWPF.Properties.Resources.adept_update_3232;
            this.btRefresh_Group_Master.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btRefresh_Group_Master.Dock = System.Windows.Forms.DockStyle.Top;
            this.btRefresh_Group_Master.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btRefresh_Group_Master.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRefresh_Group_Master.Location = new System.Drawing.Point(0, 104);
            this.btRefresh_Group_Master.Name = "btRefresh_Group_Master";
            this.btRefresh_Group_Master.Size = new System.Drawing.Size(27, 26);
            this.btRefresh_Group_Master.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btRefresh_Group_Master, "Корректировка перевода для группы мастера");
            this.btRefresh_Group_Master.UseVisualStyleBackColor = true;
            this.btRefresh_Group_Master.Click += new System.EventHandler(this.btRefresh_Group_Master_Click);
            // 
            // Edit_Group_Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 394);
            this.Controls.Add(this.dgHistoryGM);
            this.Controls.Add(this.pnWorkReg_Doc);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Edit_Group_Master";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Группа мастера";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnWorkReg_Doc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgHistoryGM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dgHistoryGM;
        private System.Windows.Forms.Panel pnWorkReg_Doc;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btDeleteGM;
        private System.Windows.Forms.Button btEditGM;
        private System.Windows.Forms.Button btAddGM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox mbName_GM;
        public LibraryKadr.DateEditor deEnd_Group;
        public System.Windows.Forms.Label label1;
        public LibraryKadr.DateEditor deBegin_Group;
        public System.Windows.Forms.Label lbDate_Order;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btRefresh_Group_Master;
    }
}