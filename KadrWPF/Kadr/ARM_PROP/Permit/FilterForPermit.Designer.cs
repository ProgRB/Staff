namespace ARM_PROP
{
    partial class FilterForPermit
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btApply = new Elegant.Ui.Button();
            this.btExit = new Elegant.Ui.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbEmp_Last_Name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPer_num = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btApply);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 37);
            this.panel1.TabIndex = 1;
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btApply.Id = "56596b2c-1e34-463f-9ec7-0a0b7d3ffe14";
            this.btApply.Location = new System.Drawing.Point(136, 8);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(101, 23);
            this.btApply.TabIndex = 0;
            this.btApply.Text = "Применить";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "95cf5ef2-9af8-4d89-ad90-6895570cf70a";
            this.btExit.Location = new System.Drawing.Point(247, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbEmp_Last_Name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbPer_num);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbCode_Subdiv);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tbEmp_Last_Name
            // 
            this.tbEmp_Last_Name.BackColor = System.Drawing.Color.White;
            this.tbEmp_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Last_Name.Location = new System.Drawing.Point(148, 68);
            this.tbEmp_Last_Name.Name = "tbEmp_Last_Name";
            this.tbEmp_Last_Name.Size = new System.Drawing.Size(174, 21);
            this.tbEmp_Last_Name.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(18, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 46;
            this.label2.Text = "Фамилия";
            // 
            // tbPer_num
            // 
            this.tbPer_num.BackColor = System.Drawing.Color.White;
            this.tbPer_num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_num.Location = new System.Drawing.Point(148, 41);
            this.tbPer_num.Name = "tbPer_num";
            this.tbPer_num.Size = new System.Drawing.Size(77, 21);
            this.tbPer_num.TabIndex = 1;
            this.tbPer_num.Leave += new System.EventHandler(this.tbPer_num_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 15);
            this.label1.TabIndex = 47;
            this.label1.Text = "Табельный №";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.BackColor = System.Drawing.Color.White;
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(148, 14);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(77, 21);
            this.tbCode_Subdiv.TabIndex = 0;
            this.tbCode_Subdiv.Leave += new System.EventHandler(this.tbCode_Subdiv_Leave);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(18, 17);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(112, 15);
            this.label17.TabIndex = 45;
            this.label17.Text = "Подразделение";
            // 
            // FilterForPermit
            // 
            this.AcceptButton = this.btApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(339, 138);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterForPermit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Фильтрация списка работников";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Button btApply;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox tbEmp_Last_Name;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbPer_num;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label17;
    }
}