using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System;

namespace LibraryKadr
{
    public class HiddenPanel:GroupBox
    {
        private int RealSize;
        private GraphicsPath p;
        private bool _ishide=false;
        [Browsable(true)]
        public int ActualHeight
        { 
            get { return RealSize; } 
            set { RealSize = value; } 
        }
        public event EventHandler StateHideChanded;
        [Browsable(true)]
        public bool IsHidden
        {
            get { return _ishide; }
            set 
            {
                _ishide = value;
                HideStateChanged(this, EventArgs.Empty);
                if (StateHideChanded!=null)
                    StateHideChanded(this, EventArgs.Empty);
            }
        }
        public HiddenPanel()
            : base()
        {
            Panel down_button = new Panel();
            down_button.Name = "down_button";
            down_button.BackColor = this.BackColor;
            down_button.BackgroundImage = Resource.uparrow1616;
            down_button.Width = 16;
            down_button.Height = 16;
            down_button.Left = this.Width - 30;
            down_button.Top = 0;
            down_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            down_button.Click += new EventHandler(HideButtonClick);
            down_button.BackColor = Color.Transparent;
            this.SuspendLayout();
            this.Controls.Add(down_button);
            this.ResumeLayout();
            this.Controls["down_button"].BringToFront();
            RealSize = base.Height;
            this.HideStateChanged(this, EventArgs.Empty);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            this.Controls["down_button"].Location = new Point(this.Width - 30, 0);
            base.OnSizeChanged(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (_ishide)
                IsHidden = false;
        }

        protected override void WndProc(ref Message m)
        {
            if (_ishide && m.Msg == 0x0214)
                return;
            base.WndProc(ref m);
        }

        private void HideStateChanged(object sender, EventArgs e)
        {
            if (!_ishide)
            {
                foreach (Control c in this.Controls)
                    c.Visible = true;
                this.Height = RealSize;
                
            }
            else
            {
                foreach (Control c in this.Controls)
                    if (c.Name != "down_button") c.Visible = false;
                this.Controls["down_button"].BringToFront();
                this.Height = 10 + (int)this.Font.GetHeight();
            }
            ((Panel)this.Controls["down_button"]).BackgroundImage = ((this.Dock == DockStyle.Bottom?!_ishide:_ishide) ? Resource.downarrow1616 : Resource.uparrow1616);
        }

        private void HideButtonClick(object sender, EventArgs e)
        {
            IsHidden = !IsHidden;
        }

    }
}