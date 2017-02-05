namespace Kadr
{
    partial class TimeExecute
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
            this.lbTimeExecute = new Elegant.Ui.Label();
            this.pbPercentExecute = new System.Windows.Forms.ProgressBar();
            this.lbPercentExecute = new Elegant.Ui.Label();
            this.SuspendLayout();
            // 
            // lbTimeExecute
            // 
            this.lbTimeExecute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTimeExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbTimeExecute.Id = "1e1bb120-c20b-4caf-b811-114009103561";
            this.lbTimeExecute.Location = new System.Drawing.Point(6, 6);
            this.lbTimeExecute.Name = "lbTimeExecute";
            this.lbTimeExecute.Size = new System.Drawing.Size(454, 19);
            this.lbTimeExecute.TabIndex = 1;
            this.lbTimeExecute.Text = "Продолжительность работы";
            // 
            // pbPercentExecute
            // 
            this.pbPercentExecute.Location = new System.Drawing.Point(6, 28);
            this.pbPercentExecute.Name = "pbPercentExecute";
            this.pbPercentExecute.Size = new System.Drawing.Size(421, 23);
            this.pbPercentExecute.TabIndex = 2;
            // 
            // lbPercentExecute
            // 
            this.lbPercentExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPercentExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPercentExecute.Id = "e3cbf90c-fc18-443f-9f2f-6ff72be164eb";
            this.lbPercentExecute.Location = new System.Drawing.Point(430, 31);
            this.lbPercentExecute.Name = "lbPercentExecute";
            this.lbPercentExecute.Size = new System.Drawing.Size(38, 19);
            this.lbPercentExecute.TabIndex = 3;
            this.lbPercentExecute.Text = "100%";
            // 
            // TimeExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Kadr.Properties.Resources.Gradient_background2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(469, 65);
            this.Controls.Add(this.lbPercentExecute);
            this.Controls.Add(this.pbPercentExecute);
            this.Controls.Add(this.lbTimeExecute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Продолжительность работы";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TimeExecute_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Elegant.Ui.Label lbTimeExecute;
        public System.Windows.Forms.ProgressBar pbPercentExecute;
        public Elegant.Ui.Label lbPercentExecute;

    }
}