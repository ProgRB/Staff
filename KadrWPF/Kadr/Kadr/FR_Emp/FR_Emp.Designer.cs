namespace Kadr
{
    partial class FR_Emp
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
            this.tcFR_Emp = new System.Windows.Forms.TabControl();
            this.tpFR_Emp = new System.Windows.Forms.TabPage();
            this.dgFR_Emp = new System.Windows.Forms.DataGridView();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btDismissFR_Emp = new System.Windows.Forms.Button();
            this.btEditFR_Emp = new System.Windows.Forms.Button();
            this.btAddFR_Emp = new System.Windows.Forms.Button();
            this.tpFR_EmpDismiss = new System.Windows.Forms.TabPage();
            this.dgFR_EmpDismiss = new System.Windows.Forms.DataGridView();
            this.tcFR_Emp.SuspendLayout();
            this.tpFR_Emp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFR_Emp)).BeginInit();
            this.pnButton.SuspendLayout();
            this.tpFR_EmpDismiss.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFR_EmpDismiss)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // tcFR_Emp
            // 
            this.tcFR_Emp.Controls.Add(this.tpFR_Emp);
            this.tcFR_Emp.Controls.Add(this.tpFR_EmpDismiss);
            this.tcFR_Emp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcFR_Emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tcFR_Emp.Location = new System.Drawing.Point(0, 0);
            this.tcFR_Emp.Name = "tcFR_Emp";
            this.tcFR_Emp.SelectedIndex = 0;
            this.tcFR_Emp.Size = new System.Drawing.Size(648, 443);
            this.tcFR_Emp.TabIndex = 26;
            this.tcFR_Emp.SelectedIndexChanged += new System.EventHandler(this.tcFR_Emp_SelectedIndexChanged);
            // 
            // tpFR_Emp
            // 
            this.tpFR_Emp.Controls.Add(this.dgFR_Emp);
            this.tpFR_Emp.Controls.Add(this.pnButton);
            this.tpFR_Emp.Location = new System.Drawing.Point(4, 24);
            this.tpFR_Emp.Name = "tpFR_Emp";
            this.tpFR_Emp.Padding = new System.Windows.Forms.Padding(3);
            this.tpFR_Emp.Size = new System.Drawing.Size(640, 415);
            this.tpFR_Emp.TabIndex = 0;
            this.tpFR_Emp.Text = "Сотрудники";
            this.tpFR_Emp.UseVisualStyleBackColor = true;
            // 
            // dgFR_Emp
            // 
            this.dgFR_Emp.AllowUserToAddRows = false;
            this.dgFR_Emp.AllowUserToDeleteRows = false;
            this.dgFR_Emp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgFR_Emp.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgFR_Emp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgFR_Emp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFR_Emp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgFR_Emp.Location = new System.Drawing.Point(3, 3);
            this.dgFR_Emp.Name = "dgFR_Emp";
            this.dgFR_Emp.ReadOnly = true;
            this.dgFR_Emp.RowHeadersWidth = 25;
            this.dgFR_Emp.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgFR_Emp.Size = new System.Drawing.Size(634, 373);
            this.dgFR_Emp.TabIndex = 27;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btDismissFR_Emp);
            this.pnButton.Controls.Add(this.btEditFR_Emp);
            this.pnButton.Controls.Add(this.btAddFR_Emp);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(3, 376);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(634, 36);
            this.pnButton.TabIndex = 26;
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(522, 7);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(83, 23);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btDismissFR_Emp
            // 
            this.btDismissFR_Emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDismissFR_Emp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btDismissFR_Emp.Location = new System.Drawing.Point(365, 7);
            this.btDismissFR_Emp.Name = "btDismissFR_Emp";
            this.btDismissFR_Emp.Size = new System.Drawing.Size(151, 23);
            this.btDismissFR_Emp.TabIndex = 0;
            this.btDismissFR_Emp.Text = "Уволить работника";
            this.btDismissFR_Emp.UseVisualStyleBackColor = true;
            this.btDismissFR_Emp.Click += new System.EventHandler(this.btDismissFR_Emp_Click);
            // 
            // btEditFR_Emp
            // 
            this.btEditFR_Emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditFR_Emp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btEditFR_Emp.Location = new System.Drawing.Point(180, 7);
            this.btEditFR_Emp.Name = "btEditFR_Emp";
            this.btEditFR_Emp.Size = new System.Drawing.Size(179, 23);
            this.btEditFR_Emp.TabIndex = 0;
            this.btEditFR_Emp.Text = "Редактировать данные";
            this.btEditFR_Emp.UseVisualStyleBackColor = true;
            this.btEditFR_Emp.Click += new System.EventHandler(this.btEditFR_Emp_Click);
            // 
            // btAddFR_Emp
            // 
            this.btAddFR_Emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAddFR_Emp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btAddFR_Emp.Location = new System.Drawing.Point(17, 7);
            this.btAddFR_Emp.Name = "btAddFR_Emp";
            this.btAddFR_Emp.Size = new System.Drawing.Size(157, 23);
            this.btAddFR_Emp.TabIndex = 0;
            this.btAddFR_Emp.Text = "Добавить работника";
            this.btAddFR_Emp.UseVisualStyleBackColor = true;
            this.btAddFR_Emp.Click += new System.EventHandler(this.btAddFR_Emp_Click);
            // 
            // tpFR_EmpDismiss
            // 
            this.tpFR_EmpDismiss.Controls.Add(this.dgFR_EmpDismiss);
            this.tpFR_EmpDismiss.Location = new System.Drawing.Point(4, 24);
            this.tpFR_EmpDismiss.Name = "tpFR_EmpDismiss";
            this.tpFR_EmpDismiss.Padding = new System.Windows.Forms.Padding(3);
            this.tpFR_EmpDismiss.Size = new System.Drawing.Size(640, 415);
            this.tpFR_EmpDismiss.TabIndex = 1;
            this.tpFR_EmpDismiss.Text = "Уволенные";
            this.tpFR_EmpDismiss.UseVisualStyleBackColor = true;
            // 
            // dgFR_EmpDismiss
            // 
            this.dgFR_EmpDismiss.AllowUserToAddRows = false;
            this.dgFR_EmpDismiss.AllowUserToDeleteRows = false;
            this.dgFR_EmpDismiss.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgFR_EmpDismiss.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgFR_EmpDismiss.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgFR_EmpDismiss.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFR_EmpDismiss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgFR_EmpDismiss.Location = new System.Drawing.Point(3, 3);
            this.dgFR_EmpDismiss.Name = "dgFR_EmpDismiss";
            this.dgFR_EmpDismiss.ReadOnly = true;
            this.dgFR_EmpDismiss.RowHeadersWidth = 25;
            this.dgFR_EmpDismiss.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgFR_EmpDismiss.Size = new System.Drawing.Size(634, 409);
            this.dgFR_EmpDismiss.TabIndex = 1;
            // 
            // FR_Emp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 443);
            this.Controls.Add(this.tcFR_Emp);
            this.Name = "FR_Emp";
            this.Text = "FR_Emp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FR_Emp_FormClosing);
            this.tcFR_Emp.ResumeLayout(false);
            this.tpFR_Emp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFR_Emp)).EndInit();
            this.pnButton.ResumeLayout(false);
            this.tpFR_EmpDismiss.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFR_EmpDismiss)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.TabControl tcFR_Emp;
        private System.Windows.Forms.TabPage tpFR_Emp;
        private System.Windows.Forms.TabPage tpFR_EmpDismiss;
        private System.Windows.Forms.DataGridView dgFR_EmpDismiss;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btDismissFR_Emp;
        private System.Windows.Forms.Button btEditFR_Emp;
        private System.Windows.Forms.Button btAddFR_Emp;
        private System.Windows.Forms.Button btExit;
        public System.Windows.Forms.DataGridView dgFR_Emp;
    }
}