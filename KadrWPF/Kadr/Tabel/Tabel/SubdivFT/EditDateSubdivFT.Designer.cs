namespace Tabel
{
    partial class EditDateSubdivFT
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btSave = new Elegant.Ui.Button();
            this.btExit = new Elegant.Ui.Button();
            this.deDate_Advance = new EditorsLibrary.DateEditor();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.deDate_Salary = new EditorsLibrary.DateEditor();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chSign_Processing = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(370, 37);
            this.panel1.TabIndex = 4;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Id = "88000b32-c522-4240-ac26-38e015b24c6b";
            this.btSave.Location = new System.Drawing.Point(161, 8);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(105, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "4628e4c1-ed98-47b3-b298-c929c6f8a399";
            this.btExit.Location = new System.Drawing.Point(278, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // deDate_Advance
            // 
            this.deDate_Advance.AutoSize = true;
            this.deDate_Advance.BackColor = System.Drawing.Color.White;
            this.deDate_Advance.Date = null;
            this.deDate_Advance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Advance.Location = new System.Drawing.Point(275, 19);
            this.deDate_Advance.Name = "deDate_Advance";
            this.deDate_Advance.ReadOnly = false;
            this.deDate_Advance.Size = new System.Drawing.Size(77, 21);
            this.deDate_Advance.TabIndex = 27;
            this.deDate_Advance.TextDate = null;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(15, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(224, 15);
            this.label14.TabIndex = 28;
            this.label14.Text = "Дата закрытия табеля на аванс";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(15, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 15);
            this.label1.TabIndex = 28;
            this.label1.Text = "Дата закрытия табеля на зарплату";
            // 
            // deDate_Salary
            // 
            this.deDate_Salary.AutoSize = true;
            this.deDate_Salary.BackColor = System.Drawing.Color.White;
            this.deDate_Salary.Date = null;
            this.deDate_Salary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Salary.Location = new System.Drawing.Point(275, 46);
            this.deDate_Salary.Name = "deDate_Salary";
            this.deDate_Salary.ReadOnly = false;
            this.deDate_Salary.Size = new System.Drawing.Size(77, 21);
            this.deDate_Salary.TabIndex = 27;
            this.deDate_Salary.TextDate = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chSign_Processing);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.deDate_Salary);
            this.groupBox1.Controls.Add(this.deDate_Advance);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 110);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // chSign_Processing
            // 
            this.chSign_Processing.AutoSize = true;
            this.chSign_Processing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chSign_Processing.Location = new System.Drawing.Point(303, 81);
            this.chSign_Processing.Name = "chSign_Processing";
            this.chSign_Processing.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chSign_Processing.Size = new System.Drawing.Size(15, 14);
            this.chSign_Processing.TabIndex = 29;
            this.chSign_Processing.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(15, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 15);
            this.label2.TabIndex = 28;
            this.label2.Text = "Признак обработки подразделения";
            // 
            // EditDateSubdivFT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 147);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditDateSubdivFT";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование дат";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Button btSave;
        private Elegant.Ui.Button btExit;
        private EditorsLibrary.DateEditor deDate_Salary;
        private System.Windows.Forms.Label label1;
        private EditorsLibrary.DateEditor deDate_Advance;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chSign_Processing;
        private System.Windows.Forms.Label label2;
    }
}