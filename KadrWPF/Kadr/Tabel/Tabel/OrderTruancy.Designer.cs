namespace Tabel
{
    partial class OrderTruancy
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
            this.btExit = new Elegant.Ui.Button();
            this.btOrderTruancy = new Elegant.Ui.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deEndPeriod = new EditorsLibrary.DateEditor();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.deBeginPeriod = new EditorsLibrary.DateEditor();
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
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btOrderTruancy);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(257, 37);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "1738c324-3624-4dd2-b7cb-2a2f7afeb772";
            this.btExit.Location = new System.Drawing.Point(160, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btOrderTruancy
            // 
            this.btOrderTruancy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOrderTruancy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOrderTruancy.Id = "ab3cf7db-fec2-45b9-a2f5-023dedfae306";
            this.btOrderTruancy.Location = new System.Drawing.Point(39, 8);
            this.btOrderTruancy.Name = "btOrderTruancy";
            this.btOrderTruancy.Size = new System.Drawing.Size(115, 23);
            this.btOrderTruancy.TabIndex = 0;
            this.btOrderTruancy.Text = "Сформировать";
            this.btOrderTruancy.Click += new System.EventHandler(this.btOrderTruancy_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deEndPeriod);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.deBeginPeriod);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // deEndPeriod
            // 
            this.deEndPeriod.AutoSize = true;
            this.deEndPeriod.BackColor = System.Drawing.Color.White;
            this.deEndPeriod.Date = null;
            this.deEndPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deEndPeriod.Location = new System.Drawing.Point(160, 48);
            this.deEndPeriod.Name = "deEndPeriod";
            this.deEndPeriod.ReadOnly = false;
            this.deEndPeriod.Size = new System.Drawing.Size(77, 21);
            this.deEndPeriod.TabIndex = 1;
            this.deEndPeriod.TextDate = null;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(15, 18);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 15);
            this.label14.TabIndex = 28;
            this.label14.Text = "Начало периода";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(15, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 15);
            this.label1.TabIndex = 28;
            this.label1.Text = "Окончание периода";
            // 
            // deBeginPeriod
            // 
            this.deBeginPeriod.AutoSize = true;
            this.deBeginPeriod.BackColor = System.Drawing.Color.White;
            this.deBeginPeriod.Date = null;
            this.deBeginPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deBeginPeriod.Location = new System.Drawing.Point(160, 18);
            this.deBeginPeriod.Name = "deBeginPeriod";
            this.deBeginPeriod.ReadOnly = false;
            this.deBeginPeriod.Size = new System.Drawing.Size(77, 21);
            this.deBeginPeriod.TabIndex = 0;
            this.deBeginPeriod.TextDate = null;
            // 
            // OrderTruancy
            // 
            this.AcceptButton = this.btOrderTruancy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 118);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "OrderTruancy";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчет по прогульщикам";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private EditorsLibrary.DateEditor deEndPeriod;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label1;
        private EditorsLibrary.DateEditor deBeginPeriod;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btOrderTruancy;
    }
}