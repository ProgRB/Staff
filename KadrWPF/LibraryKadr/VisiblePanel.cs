using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryKadr
{
    public class VisiblePanel : Panel
    {
        public VisiblePanel()
        {            
            Label lbVisible = new Label();
            lbVisible.Parent = this;
            lbVisible.BackColor = Color.White;
            lbVisible.ForeColor = Color.DimGray;
            lbVisible.Dock = DockStyle.Fill;
            lbVisible.Text = "Нет данных для отображения";
            lbVisible.TextAlign = ContentAlignment.MiddleCenter;
        }

    }

    public class Label_BMW : Label
    {
        public Label_BMW(int _x, int _y, float _fontsize, FontStyle _fontstyle, Color _color, string _text, string _name)
        {
            this.Location = new Point(_x,_y);
            this.AutoSize = true;
            this.Font = new Font("Microsoft Sans Serif", _fontsize, _fontstyle, GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = _color;
            this.Visible = true;
            this.Text = _text;
            this.Name = _name;
        }
    }

    public class TextBox_BMW : TextBox
    {
        public TextBox_BMW(int _x, int _y, int _w, int _h, int _tab, float _fontsize, FontStyle _fontstyle, Color _backcolor, Color _forecolor, string _name)
        {
            this.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right)));
            this.BackColor = _backcolor;
            this.Enabled = false;
            this.Font = new Font("Microsoft Sans Serif", _fontsize, _fontstyle, GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = _forecolor;
            this.Location = new Point(_x, _y);
            this.Multiline = true;
            this.Visible = true;
            this.Name = _name;
            this.Size = new Size(_w, _h);
            this.TabIndex = _tab;
        }
    }
    public class ComboBox_BMW : ComboBox
    {
        public ComboBox_BMW(int _x, int _y, int _w, int _h, int _tab, float _fontsize, FontStyle _fontstyle, Color _backcolor,
            Color _forecolor, string _name)
        {
            this.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right)));
            this.FormattingEnabled = true;
            this.Enabled = false;
            this.Visible = true;
            this.BackColor = _backcolor;
            this.ForeColor = _forecolor;
            this.Font = new Font("Microsoft Sans Serif", _fontsize, _fontstyle, GraphicsUnit.Point, ((byte)(204)));
            this.Location = new Point(_x, _y);
            this.Name = _name;
            this.Size = new Size(_w, _h);
            this.TabIndex = _tab;            
        }
    }
}
