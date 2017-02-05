namespace Kadr
{
    partial class Attest
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
            System.Windows.Forms.Label label7;
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.deDate_Attest = new LibraryKadr.DateEditor();
            this.deDate_Protocol = new LibraryKadr.DateEditor();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNum_Protocol = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deDate_Base_Doc = new LibraryKadr.DateEditor();
            this.tbNum_Base_Doc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbBase_Doc_ID = new System.Windows.Forms.ComboBox();
            this.label62 = new System.Windows.Forms.Label();
            this.tbThema = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRecom = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSolution = new System.Windows.Forms.TextBox();
            this.label53 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            this.pnButton.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label7.ForeColor = System.Drawing.Color.Black;
            label7.Location = new System.Drawing.Point(317, 55);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(116, 15);
            label7.TabIndex = 88;
            label7.Text = "Дата основания";
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSave);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 288);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(558, 40);
            this.pnButton.TabIndex = 2;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(456, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
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
            this.btSave.Location = new System.Drawing.Point(363, 9);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(87, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.deDate_Attest);
            this.groupBox4.Controls.Add(this.deDate_Protocol);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.tbNum_Protocol);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(558, 69);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // deDate_Attest
            // 
            this.deDate_Attest.AutoSize = true;
            this.deDate_Attest.Date = null;
            this.deDate_Attest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Attest.Location = new System.Drawing.Point(451, 42);
            this.deDate_Attest.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Attest.Name = "deDate_Attest";
            this.deDate_Attest.Size = new System.Drawing.Size(80, 24);
            this.deDate_Attest.TabIndex = 2;
            this.deDate_Attest.TextDate = null;
            // 
            // deDate_Protocol
            // 
            this.deDate_Protocol.AutoSize = true;
            this.deDate_Protocol.Date = null;
            this.deDate_Protocol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Protocol.Location = new System.Drawing.Point(451, 17);
            this.deDate_Protocol.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Protocol.Name = "deDate_Protocol";
            this.deDate_Protocol.Size = new System.Drawing.Size(80, 24);
            this.deDate_Protocol.TabIndex = 1;
            this.deDate_Protocol.TextDate = null;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(25, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 32);
            this.label5.TabIndex = 82;
            this.label5.Text = "Номер протокола";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(317, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 15);
            this.label3.TabIndex = 86;
            this.label3.Text = "Дата аттестации";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(317, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 15);
            this.label4.TabIndex = 86;
            this.label4.Text = "Дата протокола";
            // 
            // tbNum_Protocol
            // 
            this.tbNum_Protocol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Protocol.Location = new System.Drawing.Point(136, 17);
            this.tbNum_Protocol.Name = "tbNum_Protocol";
            this.tbNum_Protocol.Size = new System.Drawing.Size(92, 21);
            this.tbNum_Protocol.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.deDate_Base_Doc);
            this.groupBox3.Controls.Add(label7);
            this.groupBox3.Controls.Add(this.tbNum_Base_Doc);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cbBase_Doc_ID);
            this.groupBox3.Controls.Add(this.label62);
            this.groupBox3.Controls.Add(this.tbThema);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tbRecom);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.tbSolution);
            this.groupBox3.Controls.Add(this.label53);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.groupBox3.Location = new System.Drawing.Point(0, 69);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(558, 219);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Аттестация";
            // 
            // deDate_Base_Doc
            // 
            this.deDate_Base_Doc.AutoSize = true;
            this.deDate_Base_Doc.Date = null;
            this.deDate_Base_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Base_Doc.Location = new System.Drawing.Point(451, 53);
            this.deDate_Base_Doc.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Base_Doc.Name = "deDate_Base_Doc";
            this.deDate_Base_Doc.Size = new System.Drawing.Size(80, 24);
            this.deDate_Base_Doc.TabIndex = 2;
            this.deDate_Base_Doc.TextDate = null;
            // 
            // tbNum_Base_Doc
            // 
            this.tbNum_Base_Doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNum_Base_Doc.Location = new System.Drawing.Point(136, 52);
            this.tbNum_Base_Doc.Name = "tbNum_Base_Doc";
            this.tbNum_Base_Doc.Size = new System.Drawing.Size(92, 21);
            this.tbNum_Base_Doc.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(25, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 17);
            this.label6.TabIndex = 86;
            this.label6.Text = "№ основания";
            // 
            // cbBase_Doc_ID
            // 
            this.cbBase_Doc_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbBase_Doc_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbBase_Doc_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBase_Doc_ID.FormattingEnabled = true;
            this.cbBase_Doc_ID.Location = new System.Drawing.Point(136, 22);
            this.cbBase_Doc_ID.Name = "cbBase_Doc_ID";
            this.cbBase_Doc_ID.Size = new System.Drawing.Size(396, 23);
            this.cbBase_Doc_ID.TabIndex = 0;
            // 
            // label62
            // 
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label62.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label62.Location = new System.Drawing.Point(25, 25);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(93, 18);
            this.label62.TabIndex = 83;
            this.label62.Text = "Основание";
            // 
            // tbThema
            // 
            this.tbThema.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbThema.Location = new System.Drawing.Point(136, 81);
            this.tbThema.Name = "tbThema";
            this.tbThema.Size = new System.Drawing.Size(396, 21);
            this.tbThema.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(25, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 84;
            this.label1.Text = "Тема";
            // 
            // tbRecom
            // 
            this.tbRecom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRecom.Location = new System.Drawing.Point(136, 153);
            this.tbRecom.Multiline = true;
            this.tbRecom.Name = "tbRecom";
            this.tbRecom.Size = new System.Drawing.Size(396, 54);
            this.tbRecom.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(25, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 84;
            this.label2.Text = "Рекомендации";
            // 
            // tbSolution
            // 
            this.tbSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSolution.Location = new System.Drawing.Point(136, 107);
            this.tbSolution.Multiline = true;
            this.tbSolution.Name = "tbSolution";
            this.tbSolution.Size = new System.Drawing.Size(396, 40);
            this.tbSolution.TabIndex = 4;
            // 
            // label53
            // 
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label53.Location = new System.Drawing.Point(25, 110);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(105, 37);
            this.label53.TabIndex = 84;
            this.label53.Text = "Решение комиссии";
            // 
            // Attest
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(558, 328);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Attest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Attest_FormClosing);
            this.pnButton.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNum_Protocol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbBase_Doc_ID;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.TextBox tbThema;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbRecom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSolution;
        private System.Windows.Forms.Label label53;
        private LibraryKadr.DateEditor deDate_Attest;
        private LibraryKadr.DateEditor deDate_Protocol;
        private System.Windows.Forms.TextBox tbNum_Base_Doc;
        private System.Windows.Forms.Label label6;
        private LibraryKadr.DateEditor deDate_Base_Doc;
    }
}