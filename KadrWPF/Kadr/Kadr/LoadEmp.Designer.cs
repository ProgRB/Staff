namespace Kadr
{
    partial class LoadEmp
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
            this.gbFind = new System.Windows.Forms.GroupBox();
            this.Emp_last_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Per_num = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Code_Subdiv = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btFind = new System.Windows.Forms.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.gbFind.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFind
            // 
            this.gbFind.Controls.Add(this.Emp_last_name);
            this.gbFind.Controls.Add(this.label2);
            this.gbFind.Controls.Add(this.Per_num);
            this.gbFind.Controls.Add(this.label1);
            this.gbFind.Controls.Add(this.Code_Subdiv);
            this.gbFind.Controls.Add(this.label13);
            this.gbFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFind.Location = new System.Drawing.Point(0, 0);
            this.gbFind.Name = "gbFind";
            this.gbFind.Size = new System.Drawing.Size(296, 82);
            this.gbFind.TabIndex = 0;
            this.gbFind.TabStop = false;
            // 
            // Emp_last_name
            // 
            this.Emp_last_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Emp_last_name.Location = new System.Drawing.Point(74, 47);
            this.Emp_last_name.Name = "Emp_last_name";
            this.Emp_last_name.Size = new System.Drawing.Size(215, 21);
            this.Emp_last_name.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 30;
            this.label2.Text = "ФИО";
            // 
            // Per_num
            // 
            this.Per_num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Per_num.Location = new System.Drawing.Point(216, 20);
            this.Per_num.Name = "Per_num";
            this.Per_num.Size = new System.Drawing.Size(73, 21);
            this.Per_num.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(162, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 30;
            this.label1.Text = "Таб.№";
            // 
            // Code_Subdiv
            // 
            this.Code_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Code_Subdiv.Location = new System.Drawing.Point(74, 20);
            this.Code_Subdiv.Name = "Code_Subdiv";
            this.Code_Subdiv.Size = new System.Drawing.Size(73, 21);
            this.Code_Subdiv.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(13, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 15);
            this.label13.TabIndex = 30;
            this.label13.Text = "Подр.";
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btFind);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 84);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(296, 38);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(190, 5);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btFind
            // 
            this.btFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFind.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btFind.Location = new System.Drawing.Point(97, 5);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(87, 23);
            this.btFind.TabIndex = 0;
            this.btFind.Text = "Найти";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // LoadEmp
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 122);
            this.ControlBox = false;
            this.Controls.Add(this.pnButton);
            this.Controls.Add(this.gbFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoadEmp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск в базе работников";
            this.gbFind.ResumeLayout(false);
            this.gbFind.PerformLayout();
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btFind;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        public System.Windows.Forms.TextBox Emp_last_name;
        public System.Windows.Forms.TextBox Per_num;
        public System.Windows.Forms.TextBox Code_Subdiv;
        public System.Windows.Forms.GroupBox gbFind;
        private System.Windows.Forms.Button btExit;
    }
}