namespace LibraryKadr
{
    partial class Diagram
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hScroll = new Elegant.Ui.HorizontalScrollBar();
            this.vScroll = new Elegant.Ui.VerticalScrollBar();
            this.g_cap = new System.Windows.Forms.GroupBox();
            this.g_cap.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScroll
            // 
            this.hScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScroll.Id = "9064b2a4-06f3-4b12-ba91-39390679ab40";
            this.hScroll.LargeChange = 10;
            this.hScroll.Location = new System.Drawing.Point(3, 343);
            this.hScroll.Maximum = 100;
            this.hScroll.Minimum = 0;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size(492, 16);
            this.hScroll.SmallChange = 1;
            this.hScroll.TabIndex = 0;
            this.hScroll.Text = "horizontalScrollBar1";
            this.hScroll.Value = 0;
            this.hScroll.ValueChanged += new System.EventHandler<Elegant.Ui.ScrollBarValueChangedEventArgs>(this.hScroll_ValueChanged);
            // 
            // vScroll
            // 
            this.vScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScroll.Id = "eb148e2c-c907-454a-bc99-4d66679348aa";
            this.vScroll.LargeChange = 10;
            this.vScroll.Location = new System.Drawing.Point(495, 16);
            this.vScroll.Maximum = 100;
            this.vScroll.Minimum = 0;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size(16, 343);
            this.vScroll.SmallChange = 1;
            this.vScroll.TabIndex = 1;
            this.vScroll.Text = "verticalScrollBar1";
            this.vScroll.Value = 0;
            this.vScroll.ValueChanged += new System.EventHandler<Elegant.Ui.ScrollBarValueChangedEventArgs>(this.vScroll_ValueChanged);
            // 
            // g_cap
            // 
            this.g_cap.Controls.Add(this.hScroll);
            this.g_cap.Controls.Add(this.vScroll);
            this.g_cap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g_cap.Location = new System.Drawing.Point(0, 0);
            this.g_cap.Name = "g_cap";
            this.g_cap.Size = new System.Drawing.Size(514, 362);
            this.g_cap.TabIndex = 2;
            this.g_cap.TabStop = false;
            this.g_cap.Text = "groupBox1";
            // 
            // Diagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.g_cap);
            this.Name = "Diagram";
            this.Size = new System.Drawing.Size(514, 362);
            this.Load += new System.EventHandler(this.Diagram_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Diagram_Paint);
            this.g_cap.ResumeLayout(false);
            this.g_cap.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.HorizontalScrollBar hScroll;
        private Elegant.Ui.VerticalScrollBar vScroll;
        private System.Windows.Forms.GroupBox g_cap;
    }
}
