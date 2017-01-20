namespace Kadr
{
    partial class ListEmpTemp
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
            this.pnButton = new System.Windows.Forms.Panel();
            this.dgViewEmpTemp = new System.Windows.Forms.DataGridView();
            this.btAddEmp = new System.Windows.Forms.Button();
            this.btEditEmp = new System.Windows.Forms.Button();
            this.btDeleteEmp = new System.Windows.Forms.Button();
            this.pnButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewEmpTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // pnButton
            // 
            this.pnButton.Controls.Add(this.btDeleteEmp);
            this.pnButton.Controls.Add(this.btEditEmp);
            this.pnButton.Controls.Add(this.btAddEmp);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnButton.Location = new System.Drawing.Point(0, 0);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(32, 470);
            this.pnButton.TabIndex = 2;
            // 
            // dgViewEmpTemp
            // 
            this.dgViewEmpTemp.AllowUserToAddRows = false;
            this.dgViewEmpTemp.AllowUserToDeleteRows = false;
            this.dgViewEmpTemp.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewEmpTemp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewEmpTemp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewEmpTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewEmpTemp.Location = new System.Drawing.Point(32, 0);
            this.dgViewEmpTemp.Name = "dgViewEmpTemp";
            this.dgViewEmpTemp.ReadOnly = true;
            this.dgViewEmpTemp.RowHeadersWidth = 24;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewEmpTemp.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgViewEmpTemp.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewEmpTemp.Size = new System.Drawing.Size(670, 470);
            this.dgViewEmpTemp.TabIndex = 3;
            this.dgViewEmpTemp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgViewEmpTemp_KeyPress);
            // 
            // btAddEmp
            // 
            this.btAddEmp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btAddEmp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAddEmp.Image = global::KadrWPF.Properties.Resources.Portrait_Large;
            this.btAddEmp.Location = new System.Drawing.Point(0, 0);
            this.btAddEmp.Name = "btAddEmp";
            this.btAddEmp.Size = new System.Drawing.Size(32, 31);
            this.btAddEmp.TabIndex = 0;
            this.btAddEmp.UseVisualStyleBackColor = true;
            this.btAddEmp.Click += new System.EventHandler(this.btAddEmp_Click);
            // 
            // btEditEmp
            // 
            this.btEditEmp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btEditEmp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btEditEmp.Image = global::KadrWPF.Properties.Resources.Prepare_Large;
            this.btEditEmp.Location = new System.Drawing.Point(0, 31);
            this.btEditEmp.Name = "btEditEmp";
            this.btEditEmp.Size = new System.Drawing.Size(32, 31);
            this.btEditEmp.TabIndex = 1;
            this.btEditEmp.UseVisualStyleBackColor = true;
            this.btEditEmp.Click += new System.EventHandler(this.btEditEmp_Click);
            // 
            // btDeleteEmp
            // 
            this.btDeleteEmp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btDeleteEmp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btDeleteEmp.Image = global::KadrWPF.Properties.Resources.Remove;
            this.btDeleteEmp.Location = new System.Drawing.Point(0, 62);
            this.btDeleteEmp.Name = "btDeleteEmp";
            this.btDeleteEmp.Size = new System.Drawing.Size(32, 31);
            this.btDeleteEmp.TabIndex = 2;
            this.btDeleteEmp.UseVisualStyleBackColor = true;
            this.btDeleteEmp.Click += new System.EventHandler(this.btDeleteEmp_Click);
            // 
            // ListEmpTemp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgViewEmpTemp);
            this.Controls.Add(this.pnButton);
            this.Name = "ListEmpTemp";
            this.Size = new System.Drawing.Size(702, 470);
            this.pnButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewEmpTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btDeleteEmp;
        private System.Windows.Forms.Button btEditEmp;
        private System.Windows.Forms.Button btAddEmp;
        public System.Windows.Forms.DataGridView dgViewEmpTemp;

    }
}