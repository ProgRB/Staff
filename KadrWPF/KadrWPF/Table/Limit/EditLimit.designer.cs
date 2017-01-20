namespace Tabel
{
    partial class EditLimit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditLimit));
            this.gbReg_Doc = new System.Windows.Forms.GroupBox();
            this.tbLimit_Number_Doc = new System.Windows.Forms.TextBox();
            this.deLimit_Date_Doc = new LibraryKadr.DateEditor();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbLOD = new System.Windows.Forms.GroupBox();
            this.dgLimitOnDegree = new System.Windows.Forms.DataGridView();
            this.pnButtonEditOrder = new System.Windows.Forms.Panel();
            this.btSaveLOD = new System.Windows.Forms.Button();
            this.tsbDeleteLOD = new System.Windows.Forms.Button();
            this.tsbAddLOD = new System.Windows.Forms.Button();
            this.deEnd_Limit = new LibraryKadr.DateEditor();
            this.deBegin_Limit = new LibraryKadr.DateEditor();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSaveOrderLimit = new System.Windows.Forms.Button();
            this.gbReg_Doc.SuspendLayout();
            this.gbLOD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLimitOnDegree)).BeginInit();
            this.pnButtonEditOrder.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // gbReg_Doc
            // 
            this.gbReg_Doc.Controls.Add(this.tbLimit_Number_Doc);
            this.gbReg_Doc.Controls.Add(this.deLimit_Date_Doc);
            this.gbReg_Doc.Controls.Add(this.label2);
            this.gbReg_Doc.Controls.Add(this.label3);
            this.gbReg_Doc.Controls.Add(this.gbLOD);
            this.gbReg_Doc.Controls.Add(this.deEnd_Limit);
            this.gbReg_Doc.Controls.Add(this.deBegin_Limit);
            this.gbReg_Doc.Controls.Add(this.label14);
            this.gbReg_Doc.Controls.Add(this.label13);
            this.gbReg_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReg_Doc.Location = new System.Drawing.Point(0, 0);
            this.gbReg_Doc.Name = "gbReg_Doc";
            this.gbReg_Doc.Size = new System.Drawing.Size(593, 363);
            this.gbReg_Doc.TabIndex = 0;
            this.gbReg_Doc.TabStop = false;
            // 
            // tbLimit_Number_Doc
            // 
            this.tbLimit_Number_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLimit_Number_Doc.Location = new System.Drawing.Point(126, 16);
            this.tbLimit_Number_Doc.Name = "tbLimit_Number_Doc";
            this.tbLimit_Number_Doc.Size = new System.Drawing.Size(58, 21);
            this.tbLimit_Number_Doc.TabIndex = 0;
            // 
            // deLimit_Date_Doc
            // 
            this.deLimit_Date_Doc.AutoSize = true;
            this.deLimit_Date_Doc.BackColor = System.Drawing.Color.White;
            this.deLimit_Date_Doc.Date = null;
            this.deLimit_Date_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deLimit_Date_Doc.Location = new System.Drawing.Point(498, 16);
            this.deLimit_Date_Doc.Name = "deLimit_Date_Doc";
            this.deLimit_Date_Doc.ReadOnly = false;
            this.deLimit_Date_Doc.Size = new System.Drawing.Size(73, 21);
            this.deLimit_Date_Doc.TabIndex = 1;
            this.deLimit_Date_Doc.TextDate = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(14, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 61;
            this.label2.Text = "Номер приказа";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(393, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 62;
            this.label3.Text = "Дата приказа";
            // 
            // gbLOD
            // 
            this.gbLOD.Controls.Add(this.dgLimitOnDegree);
            this.gbLOD.Controls.Add(this.pnButtonEditOrder);
            this.gbLOD.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbLOD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbLOD.Location = new System.Drawing.Point(3, 74);
            this.gbLOD.Name = "gbLOD";
            this.gbLOD.Size = new System.Drawing.Size(587, 286);
            this.gbLOD.TabIndex = 4;
            this.gbLOD.TabStop = false;
            this.gbLOD.Text = "Количество часов по плану по категориям";
            // 
            // dgLimitOnDegree
            // 
            this.dgLimitOnDegree.AllowUserToAddRows = false;
            this.dgLimitOnDegree.AllowUserToDeleteRows = false;
            this.dgLimitOnDegree.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgLimitOnDegree.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgLimitOnDegree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgLimitOnDegree.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgLimitOnDegree.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLimitOnDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgLimitOnDegree.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgLimitOnDegree.Location = new System.Drawing.Point(3, 17);
            this.dgLimitOnDegree.Name = "dgLimitOnDegree";
            this.dgLimitOnDegree.RowHeadersWidth = 24;
            this.dgLimitOnDegree.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgLimitOnDegree.Size = new System.Drawing.Size(554, 266);
            this.dgLimitOnDegree.TabIndex = 0;
            // 
            // pnButtonEditOrder
            // 
            this.pnButtonEditOrder.BackColor = System.Drawing.SystemColors.Control;
            this.pnButtonEditOrder.Controls.Add(this.btSaveLOD);
            this.pnButtonEditOrder.Controls.Add(this.tsbDeleteLOD);
            this.pnButtonEditOrder.Controls.Add(this.tsbAddLOD);
            this.pnButtonEditOrder.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnButtonEditOrder.Location = new System.Drawing.Point(557, 17);
            this.pnButtonEditOrder.Name = "pnButtonEditOrder";
            this.pnButtonEditOrder.Size = new System.Drawing.Size(27, 266);
            this.pnButtonEditOrder.TabIndex = 1;
            // 
            // btSaveLOD
            // 
            this.btSaveLOD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btSaveLOD.BackgroundImage")));
            this.btSaveLOD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btSaveLOD.Dock = System.Windows.Forms.DockStyle.Top;
            this.btSaveLOD.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btSaveLOD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSaveLOD.Location = new System.Drawing.Point(0, 52);
            this.btSaveLOD.Name = "btSaveLOD";
            this.btSaveLOD.Size = new System.Drawing.Size(27, 26);
            this.btSaveLOD.TabIndex = 2;
            this.btSaveLOD.UseVisualStyleBackColor = true;
            this.btSaveLOD.Click += new System.EventHandler(this.btSaveLOD_Click);
            // 
            // tsbDeleteLOD
            // 
            this.tsbDeleteLOD.BackgroundImage = global::KadrWPF.Properties.Resources.Delete;
            this.tsbDeleteLOD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tsbDeleteLOD.Dock = System.Windows.Forms.DockStyle.Top;
            this.tsbDeleteLOD.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.tsbDeleteLOD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tsbDeleteLOD.Location = new System.Drawing.Point(0, 26);
            this.tsbDeleteLOD.Name = "tsbDeleteLOD";
            this.tsbDeleteLOD.Size = new System.Drawing.Size(27, 26);
            this.tsbDeleteLOD.TabIndex = 1;
            this.tsbDeleteLOD.UseVisualStyleBackColor = true;
            this.tsbDeleteLOD.Click += new System.EventHandler(this.tsbDeleteLOD_Click);
            // 
            // tsbAddLOD
            // 
            this.tsbAddLOD.BackgroundImage = global::KadrWPF.Properties.Resources.document_new_1616;
            this.tsbAddLOD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tsbAddLOD.Dock = System.Windows.Forms.DockStyle.Top;
            this.tsbAddLOD.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.tsbAddLOD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tsbAddLOD.Location = new System.Drawing.Point(0, 0);
            this.tsbAddLOD.Name = "tsbAddLOD";
            this.tsbAddLOD.Size = new System.Drawing.Size(27, 26);
            this.tsbAddLOD.TabIndex = 0;
            this.tsbAddLOD.UseVisualStyleBackColor = true;
            this.tsbAddLOD.Click += new System.EventHandler(this.tsbAddLOD_Click);
            // 
            // deEnd_Limit
            // 
            this.deEnd_Limit.AutoSize = true;
            this.deEnd_Limit.BackColor = System.Drawing.Color.White;
            this.deEnd_Limit.Date = null;
            this.deEnd_Limit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deEnd_Limit.Location = new System.Drawing.Point(498, 43);
            this.deEnd_Limit.Name = "deEnd_Limit";
            this.deEnd_Limit.ReadOnly = false;
            this.deEnd_Limit.Size = new System.Drawing.Size(73, 21);
            this.deEnd_Limit.TabIndex = 3;
            this.deEnd_Limit.TextDate = null;
            this.deEnd_Limit.Validating += new System.ComponentModel.CancelEventHandler(this.deEnd_Limit_Validating);
            // 
            // deBegin_Limit
            // 
            this.deBegin_Limit.AutoSize = true;
            this.deBegin_Limit.BackColor = System.Drawing.Color.White;
            this.deBegin_Limit.Date = null;
            this.deBegin_Limit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deBegin_Limit.Location = new System.Drawing.Point(126, 43);
            this.deBegin_Limit.Name = "deBegin_Limit";
            this.deBegin_Limit.ReadOnly = false;
            this.deBegin_Limit.Size = new System.Drawing.Size(72, 21);
            this.deBegin_Limit.TabIndex = 2;
            this.deBegin_Limit.TextDate = null;
            this.deBegin_Limit.Validating += new System.ComponentModel.CancelEventHandler(this.deBegin_Limit_Validating);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(14, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(92, 15);
            this.label14.TabIndex = 26;
            this.label14.Text = "Дата начала";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(377, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 15);
            this.label13.TabIndex = 27;
            this.label13.Text = "Дата окончания";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSaveOrderLimit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 363);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(593, 37);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(496, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSaveOrderLimit
            // 
            this.btSaveOrderLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveOrderLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSaveOrderLimit.Location = new System.Drawing.Point(382, 8);
            this.btSaveOrderLimit.Name = "btSaveOrderLimit";
            this.btSaveOrderLimit.Size = new System.Drawing.Size(104, 23);
            this.btSaveOrderLimit.TabIndex = 0;
            this.btSaveOrderLimit.Text = "Сохранить";
            this.btSaveOrderLimit.Click += new System.EventHandler(this.btSave_Click);
            // 
            // EditLimit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 400);
            this.Controls.Add(this.gbReg_Doc);
            this.Controls.Add(this.panel1);
            this.Name = "EditLimit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditLimit";
            this.gbReg_Doc.ResumeLayout(false);
            this.gbReg_Doc.PerformLayout();
            this.gbLOD.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgLimitOnDegree)).EndInit();
            this.pnButtonEditOrder.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbReg_Doc;
        private LibraryKadr.DateEditor deEnd_Limit;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSaveOrderLimit;
        private System.Windows.Forms.GroupBox gbLOD;
        private System.Windows.Forms.DataGridView dgLimitOnDegree;
        private System.Windows.Forms.Panel pnButtonEditOrder;
        private System.Windows.Forms.Button btSaveLOD;
        private System.Windows.Forms.Button tsbDeleteLOD;
        private System.Windows.Forms.Button tsbAddLOD;
        private System.Windows.Forms.TextBox tbLimit_Number_Doc;
        private LibraryKadr.DateEditor deLimit_Date_Doc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private LibraryKadr.DateEditor deBegin_Limit;
    }
}