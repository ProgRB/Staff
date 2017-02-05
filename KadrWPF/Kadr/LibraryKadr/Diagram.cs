using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace LibraryKadr
{
    public partial class Diagram : UserControl
    {
        public float CellWidth=10,SpaceColumn=10;
        public List<float> table;
        public List<string> captions;
        public Color AxiesColor=Color.Black, DiagramColor= Color.Red,CellColor=Color.Silver,DBackColor=Color.White;
        /// <summary>
        /// Центр Осей координат, относительно Нижнего левого угла
        /// </summary>
        public Point CenterAxies;
        public Image i1;
        public void DrawDiagram()
        {
            float mx = -100000;
            if (table == null) return;
            //int Center1= new Point(Center.
            foreach (float t in table)
                mx=Math.Max(t,mx);
            i1 = new Bitmap((int)(CenterAxies.X+(table.Count+1)*(CellWidth+SpaceColumn)),this.Height);
            Graphics g = Graphics.FromImage(i1);//создаем графикс)))
            Pen p = new Pen(AxiesColor);
            Pen p1 = new Pen(DiagramColor);
            Pen p2 = new Pen(CellColor);
            g.Clear(DBackColor);
            p.Width = 4;
            p.Color = Color.Black;
            Point CenterAxies1 = new Point(CenterAxies.X, Height - CenterAxies.Y);
            g.DrawLine(p, CenterAxies1, new Point((int)((table.Count+1)*(CellWidth+SpaceColumn)), CenterAxies1.Y));
            g.DrawLine(p, CenterAxies1, new Point(CenterAxies1.X, 0));
            p.Color = Color.LightCoral;
            List<float> ch = new List<float>();
            System.Drawing.Font fnt = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            float K=(float)((float)(this.Height-CenterAxies.Y)/ mx*0.9);
            if (mx>0)
            for (int i = 0; i < table.Count; i++)
            {
                p1.Width = 1;
                p1.Color = Color.Red;
                g.FillRectangle(p.Brush, (int)(CenterAxies1.X + i * (CellWidth + SpaceColumn) + SpaceColumn), (int)(CenterAxies1.Y - table[i] * K- 2), CellWidth, table[i]*K);
                g.DrawRectangle(p1, (int)(CenterAxies1.X + i * (CellWidth + SpaceColumn) + SpaceColumn), (int)(CenterAxies1.Y - table[i]*K - 2), CellWidth, table[i] * K);
                p1.Color = Color.Black;
                g.DrawString(captions[i], fnt, p1.Brush, new PointF((float)(CenterAxies1.X + i * (CellWidth + SpaceColumn) + SpaceColumn), CenterAxies1.Y));
                float mn = 10000;
                for (int j = 0; j < ch.Count; j++)
                    mn =Math.Min(Math.Abs(table[i] - ch[j]),mn);
                if (K * mn > 10)
                {
                    ch.Add(table[i]);
                    g.DrawString(table[i].ToString(), fnt, p2.Brush, new PointF((CenterAxies1.X - table[i].ToString().Length * 10 - 5), CenterAxies1.Y - 10 - table[i] * K));
                    p1.Width = 1;
                    p1.Color = Color.Gray;
                    g.DrawLine(p1, CenterAxies1.X, CenterAxies1.Y - table[i] * K - 2, CenterAxies1.X + table.Count * (CellWidth + SpaceColumn) + SpaceColumn, CenterAxies1.Y - table[i] * K - 2);
                }
                
            }
            vScroll.Maximum = this.Height;
            hScroll.Maximum = (int)(CenterAxies.X+table.Count*(CellWidth+SpaceColumn)+SpaceColumn);
            Diagram_Paint(null, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
        }
        public Diagram()
        {
            InitializeComponent();
        }
        public Diagram(int _Width,int _Height)
        {
            InitializeComponent();
            this.Width = _Width;
            this.Height = _Height;
        }
        private void Diagram_Paint(object sender, PaintEventArgs e)
        {
            if (i1 != null)
            { 
                e.Graphics.Clear(DBackColor);
                e.Graphics.DrawImage(i1, -hScroll.Value, -vScroll.Value);
            }
        }

        private void hScroll_ValueChanged(object sender, Elegant.Ui.ScrollBarValueChangedEventArgs e)
        {
            Diagram_Paint(null, new PaintEventArgs(this.CreateGraphics(),new Rectangle(0,0,this.Width,this.Height)));
        }

        private void vScroll_ValueChanged(object sender, Elegant.Ui.ScrollBarValueChangedEventArgs e)
        {
           Diagram_Paint(null, new PaintEventArgs(this.CreateGraphics(),new Rectangle(0,0,this.Width,this.Height)));
        }

        private void Diagram_Load(object sender, EventArgs e)
        {

        }
        
    }
}
