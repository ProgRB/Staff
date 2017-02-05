namespace ARM_PROP
{
    partial class StrSotr
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
            this.btExit = new Elegant.Ui.Button();
            this.btSave = new Elegant.Ui.Button();
            this.dgvStrSotrud = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMiddle_name = new System.Windows.Forms.TextBox();
            this.tbFirst_name = new System.Windows.Forms.TextBox();
            this.tbLast_name = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPodr = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrSotrud)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btExit
            // 
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Id = "15bb48d0-b9b7-4dc2-982f-05afe46bdd56";
            this.btExit.Location = new System.Drawing.Point(429, 138);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 42;
            this.btExit.Text = "Отмена";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Id = "3478aa35-d77f-468c-a6f5-c35e10676840";
            this.btSave.Location = new System.Drawing.Point(337, 138);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 41;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // dgvStrSotrud
            // 
            this.dgvStrSotrud.AllowUserToAddRows = false;
            this.dgvStrSotrud.AllowUserToDeleteRows = false;
            this.dgvStrSotrud.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStrSotrud.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStrSotrud.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStrSotrud.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStrSotrud.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvStrSotrud.Location = new System.Drawing.Point(0, 0);
            this.dgvStrSotrud.Name = "dgvStrSotrud";
            this.dgvStrSotrud.ReadOnly = true;
            this.dgvStrSotrud.Size = new System.Drawing.Size(519, 133);
            this.dgvStrSotrud.TabIndex = 40;
            this.dgvStrSotrud.SelectionChanged += new System.EventHandler(this.dgvStrSotrud_SelectionChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(45, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Отчество";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(70, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Имя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(43, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Фамилия";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Подразделение";
            // 
            // tbMiddle_name
            // 
            this.tbMiddle_name.BackColor = System.Drawing.Color.White;
            this.tbMiddle_name.Location = new System.Drawing.Point(112, 102);
            this.tbMiddle_name.Name = "tbMiddle_name";
            this.tbMiddle_name.ReadOnly = true;
            this.tbMiddle_name.Size = new System.Drawing.Size(226, 20);
            this.tbMiddle_name.TabIndex = 25;
            // 
            // tbFirst_name
            // 
            this.tbFirst_name.BackColor = System.Drawing.Color.White;
            this.tbFirst_name.Location = new System.Drawing.Point(112, 67);
            this.tbFirst_name.Name = "tbFirst_name";
            this.tbFirst_name.ReadOnly = true;
            this.tbFirst_name.Size = new System.Drawing.Size(226, 20);
            this.tbFirst_name.TabIndex = 24;
            // 
            // tbLast_name
            // 
            this.tbLast_name.BackColor = System.Drawing.Color.White;
            this.tbLast_name.Location = new System.Drawing.Point(112, 32);
            this.tbLast_name.Name = "tbLast_name";
            this.tbLast_name.ReadOnly = true;
            this.tbLast_name.Size = new System.Drawing.Size(226, 20);
            this.tbLast_name.TabIndex = 23;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Location = new System.Drawing.Point(6, 139);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(510, 167);
            this.panel1.TabIndex = 43;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.tbPodr);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tbLast_name);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.tbFirst_name);
            this.panel2.Controls.Add(this.tbMiddle_name);
            this.panel2.Location = new System.Drawing.Point(158, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(347, 132);
            this.panel2.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(6, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 46;
            this.label5.Text = "Подразделение";
            // 
            // tbPodr
            // 
            this.tbPodr.BackColor = System.Drawing.Color.White;
            this.tbPodr.Location = new System.Drawing.Point(112, 3);
            this.tbPodr.Name = "tbPodr";
            this.tbPodr.ReadOnly = true;
            this.tbPodr.Size = new System.Drawing.Size(110, 20);
            this.tbPodr.TabIndex = 45;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(5, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 158);
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // StrSotr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 308);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvStrSotrud);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StrSotr";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сторонние сотрудники";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrSotrud)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btSave;
        private System.Windows.Forms.DataGridView dgvStrSotrud;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMiddle_name;
        private System.Windows.Forms.TextBox tbFirst_name;
        private System.Windows.Forms.TextBox tbLast_name;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbPodr;
        private System.Windows.Forms.Label label5;
    }
}