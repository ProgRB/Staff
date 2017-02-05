namespace ARM_PROP
{
    partial class AddForm
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
            this.btAdd = new Elegant.Ui.Button();
            this.btExit = new Elegant.Ui.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMiddle_name = new System.Windows.Forms.TextBox();
            this.tbFirst_name = new System.Windows.Forms.TextBox();
            this.tbLast_name = new System.Windows.Forms.TextBox();
            this.dgvNaruh = new System.Windows.Forms.DataGridView();
            this.btSave = new Elegant.Ui.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNaruh)).BeginInit();
            this.SuspendLayout();
            // 
            // btAdd
            // 
            this.btAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAdd.Id = "01bd37e8-7a6b-41f2-9e25-895ce569e187";
            this.btAdd.Location = new System.Drawing.Point(237, 240);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(142, 23);
            this.btAdd.TabIndex = 6;
            this.btAdd.Text = "Добавить нарушителя";
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btExit
            // 
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "50bd6652-fed8-489c-b7fa-e6e1a95c8d5b";
            this.btExit.Location = new System.Drawing.Point(390, 240);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 7;
            this.btExit.Text = "Отмена";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbMiddle_name);
            this.panel1.Controls.Add(this.tbFirst_name);
            this.panel1.Controls.Add(this.tbLast_name);
            this.panel1.Location = new System.Drawing.Point(6, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 82);
            this.panel1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(7, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Отчество";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(248, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Имя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(5, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Фамилия";
            // 
            // tbMiddle_name
            // 
            this.tbMiddle_name.BackColor = System.Drawing.Color.White;
            this.tbMiddle_name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbMiddle_name.Location = new System.Drawing.Point(72, 49);
            this.tbMiddle_name.Name = "tbMiddle_name";
            this.tbMiddle_name.Size = new System.Drawing.Size(173, 20);
            this.tbMiddle_name.TabIndex = 20;
            // 
            // tbFirst_name
            // 
            this.tbFirst_name.BackColor = System.Drawing.Color.White;
            this.tbFirst_name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbFirst_name.Location = new System.Drawing.Point(283, 14);
            this.tbFirst_name.Name = "tbFirst_name";
            this.tbFirst_name.Size = new System.Drawing.Size(173, 20);
            this.tbFirst_name.TabIndex = 19;
            // 
            // tbLast_name
            // 
            this.tbLast_name.BackColor = System.Drawing.Color.White;
            this.tbLast_name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbLast_name.Location = new System.Drawing.Point(72, 14);
            this.tbLast_name.Name = "tbLast_name";
            this.tbLast_name.Size = new System.Drawing.Size(173, 20);
            this.tbLast_name.TabIndex = 18;
            // 
            // dgvNaruh
            // 
            this.dgvNaruh.AllowUserToAddRows = false;
            this.dgvNaruh.AllowUserToDeleteRows = false;
            this.dgvNaruh.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNaruh.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNaruh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNaruh.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvNaruh.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvNaruh.GridColor = System.Drawing.Color.DarkGray;
            this.dgvNaruh.Location = new System.Drawing.Point(0, 0);
            this.dgvNaruh.Name = "dgvNaruh";
            this.dgvNaruh.ReadOnly = true;
            this.dgvNaruh.Size = new System.Drawing.Size(475, 144);
            this.dgvNaruh.TabIndex = 10;
            // 
            // btSave
            // 
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Id = "58c30ebf-62d4-4833-9060-c3062acfc31d";
            this.btSave.Location = new System.Drawing.Point(150, 240);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 11;
            this.btSave.Text = "Выбрать";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 268);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.dgvNaruh);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Нарушитель";
            this.Load += new System.EventHandler(this.AddForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNaruh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.Button btAdd;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMiddle_name;
        private System.Windows.Forms.TextBox tbFirst_name;
        private System.Windows.Forms.TextBox tbLast_name;
        private System.Windows.Forms.DataGridView dgvNaruh;
        private Elegant.Ui.Button btSave;
    }
}