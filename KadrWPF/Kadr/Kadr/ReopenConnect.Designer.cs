namespace Kadr
{
    partial class ReopenConnect
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1 = new Elegant.Ui.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.btDisplace = new Elegant.Ui.Button();
            this.lbMessage = new Elegant.Ui.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            this.formFrameSkinner1.TitleFont = new System.Drawing.Font("Trebuchet MS", 10F, System.Drawing.FontStyle.Bold);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 49);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(483, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btClose);
            this.panel1.Controls.Add(this.btDisplace);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Id = "9355820a-672c-43e2-ae3f-865df40aa5ab";
            this.panel1.Location = new System.Drawing.Point(0, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 30);
            this.panel1.TabIndex = 3;
            this.panel1.Text = "panel1";
            // 
            // btClose
            // 
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClose.Location = new System.Drawing.Point(334, 3);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(155, 24);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "Закрыть программу";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btDisplace
            // 
            this.btDisplace.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDisplace.Id = "f87947c5-4aaf-468e-b7c0-fe59af1d53f7";
            this.btDisplace.Location = new System.Drawing.Point(221, 3);
            this.btDisplace.Name = "btDisplace";
            this.btDisplace.Size = new System.Drawing.Size(110, 23);
            this.btDisplace.TabIndex = 0;
            this.btDisplace.Text = "Свернуть окно";
            this.btDisplace.Visible = false;
            this.btDisplace.Click += new System.EventHandler(this.btDisplace_Click);
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = false;
            this.lbMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbMessage.Id = "5c6a11e3-0cdd-4dd1-8154-d040bbe28f16";
            this.lbMessage.Location = new System.Drawing.Point(0, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(497, 41);
            this.lbMessage.TabIndex = 4;
            this.lbMessage.Text = "Здесь отображается текст сообщения";
            this.lbMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReopenConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 108);
            this.ControlBox = false;
            this.Controls.Add(this.lbMessage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReopenConnect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "АСУ \"Кадры\"";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private Elegant.Ui.Panel panel1;
        private Elegant.Ui.Button btDisplace;
        public Elegant.Ui.Label lbMessage;
        private System.Windows.Forms.Button btClose;

    }
}