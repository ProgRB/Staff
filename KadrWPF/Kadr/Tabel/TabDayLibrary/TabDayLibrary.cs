using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;


namespace TabDayLibrary
{
    public class TabDayItem : System.Windows.Forms.UserControl
    {
        public static string DayOfWeekRus(DayOfWeek DW)
        {
            switch (DW)
            {
                case DayOfWeek.Monday:
                    return "пн";
                case DayOfWeek.Tuesday:
                    return "вт";
                case DayOfWeek.Wednesday:
                    return "ср";
                case DayOfWeek.Thursday:
                    return "чт";
                case DayOfWeek.Friday:
                    return "пт";
                case DayOfWeek.Saturday:
                    return "сб";
                case DayOfWeek.Sunday:
                    return "вс";
                default:
                    return "";
            }
        }

        private System.Windows.Forms.Label DayNum;       //число
        private System.Windows.Forms.Label DayWeek;      //день недели
        private System.Windows.Forms.TextBox TabText;    //текст для табеля
        public Button TabBtn;
        private System.Windows.Forms.Label DayHour;      //кол-во часов по проходной
        public Color fColorFocus = Color.FromArgb(0xD0, 0xFF, 0x40);//0xD0, 0xFF, 0x40 0xFF, 0xE0, 0xB0
        private Color fColorSelect = Color.FromArgb(0xFF, 0xE0, 0xB0);
        public Color fColorDefault = Color.FromArgb(0xF0, 0xF0, 0xF0);
        private void _OnResize(object sender, System.EventArgs e)
        {
            DayHour.MinimumSize = new Size(this.Width, 20);
        }

        public TabDayItem()
        {
            Width = 80;
            Height = 110;            
            DoubleBuffered = true;
            BackColor = fColorDefault;
            DayNum = new System.Windows.Forms.Label();
            DayNum.Parent = this;
            DayWeek = new System.Windows.Forms.Label();
            DayWeek.Parent = this;
            TabText = new System.Windows.Forms.TextBox();
            TabBtn = new Button();
            TabBtn.Parent = this;
            DayHour = new System.Windows.Forms.Label();
            DayHour.Parent = this;
            fDateOfDay = DateTime.Now;
            DayNum.BackColor = Color.FromArgb(0x80, 0xA0, 0xA0, 0xA0);
            DayNum.ForeColor = Color.FromArgb(64, 64, 0x40);//FromArgb(64, 64, 0x40);
            DayNum.Size = new Size(80, 18);
            DayNum.Font = new Font(DayNum.Font, FontStyle.Bold);
            DayNum.DataBindings.Add("Font", this, "Font");
            DayNum.TextAlign = ContentAlignment.BottomCenter;
            DayNum.Text = fDateOfDay.Day.ToString();
            DayNum.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            DayNum.MouseEnter += new EventHandler(_OnMouseEnter);
            DayNum.MouseLeave += new EventHandler(_OnMouseLeave);
            DayNum.Click += new EventHandler(_OnClick);
            DayWeek.BackColor = Color.FromArgb(0x80, 0xA0, 0xA0, 0xA0);
            DayWeek.ForeColor = Color.FromArgb(64, 64, 0x40);
            DayWeek.Size = new Size(80, 20);
            DayWeek.Font = new Font(DayNum.Font, FontStyle.Bold);
            DayWeek.Top = DayNum.Bottom;
            DayWeek.TextAlign = ContentAlignment.TopCenter;
            DayWeek.Text = DayOfWeekRus(fDateOfDay.DayOfWeek).ToUpper();
            DayWeek.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            DayWeek.DataBindings.Add("Font", this, "Font");
            DayWeek.MouseEnter += new EventHandler(_OnMouseEnter);
            DayWeek.MouseLeave += new EventHandler(_OnMouseLeave);
            DayWeek.Click += new EventHandler(_OnClick);            
            //TabBtn.Size = new Size(64, 26);
            TabBtn.Size = new Size(72, 26);
            TabBtn.Top = DayWeek.Bottom + 4;
            TabBtn.Left = 4;
            //TabBtn.Left = 8;
            TabBtn.FlatStyle = FlatStyle.System;
            TabBtn.TextAlign = ContentAlignment.MiddleCenter;
            TabBtn.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            TabBtn.DataBindings.Add("Font", this, "Font");
            TabBtn.MouseEnter += new EventHandler(_OnMouseEnter);
            TabBtn.MouseLeave += new EventHandler(_OnMouseLeave);
            TabBtn.Enter += new EventHandler(_OnTabTextEnter);
            TabBtn.Leave += new EventHandler(_OnTabTextLeave);
            DayHour.Top = TabBtn.Bottom + 2;
            DayHour.AutoSize = true;
            DayHour.MinimumSize = new Size(this.Width, 20);
            DayHour.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            DayHour.TextAlign = ContentAlignment.TopCenter;
            DayHour.DataBindings.Add("Font", this, "Font");
            DayHour.MouseEnter += new EventHandler(_OnMouseEnter);
            DayHour.MouseLeave += new EventHandler(_OnMouseLeave);
            DayHour.Click += new EventHandler(_OnClick);
            this.Resize += _OnResize;
            this.MouseEnter += new EventHandler(_OnMouseEnter);
            this.MouseLeave += new EventHandler(_OnMouseLeave);
            this.Click += new EventHandler(_OnClick);
            this.Enter += new EventHandler(_OnTabTextEnter);
            this.Leave += new EventHandler(_OnTabTextLeave);
        }

        void _OnTabTextEnter(object sender, EventArgs e)
        {
            //this.BackColor = fColorFocus;// Color.FromArgb(0xD0, 0xFF, 0x40);
        }

        void _OnTabTextLeave(object sender, EventArgs e)
        {
            //this.BackColor = fColorDefault;
        }

        void _OnClick(object sender, EventArgs e)
        {
            this.Focus();
            //TabText.Focus();
        }

        void _OnMouseEnter(object sender, EventArgs e)
        {
            //this.BackColor = fColorSelect;// Color.FromArgb(0xFF, 0xD0, 0x40);
        }

        void _OnMouseLeave(object sender, EventArgs e)
        {
            ////if (TabText.Focused)
            //if (TabBtn.Focused)
            //{
            //    this.BackColor = fColorFocus;
            //} 
            //else
            //{
            //    this.BackColor = fColorDefault;// Color.FromArgb(0xF0, 0xF0, 0xF0);
            //}
        }

        void _TabTextKeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)e.KeyChar.ToString().ToUpper()[0];
        }

        void _OnTabTextChanged(object sender, EventArgs e)
        {           
            ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
        }
        private DateTime fDateOfDay;
        public DateTime DateOfDay
        {
            get 
            {
                return fDateOfDay;
            }
            set 
            {
                fDateOfDay = value;
                DayNum.Text = fDateOfDay.Day.ToString();
                DayWeek.Text = DayOfWeekRus(fDateOfDay.DayOfWeek).ToUpper();
            }
        }
        public override string Text 
        {
            get 
            {
                //return TabText.Text;
                return TabBtn.Text;
            }
            set 
            {
                //TabText.Text = value;
                TabBtn.Text = value;
            }
        }
        public string NoteText
        {
            get
            {
                return DayHour.Text;
            }
            set
            {
                DayHour.Text = value;
            }
        }
        public Color DayWeekColor
        {
            get
            {
                return DayWeek.ForeColor;
            }
            set
            {
                DayWeek.ForeColor = value;
                DayNum.ForeColor = value;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TabDayItem
            // 
            this.Name = "TabDayItem";
            this.Size = new System.Drawing.Size(215, 198);
            this.ResumeLayout(false);

        }
    }
    

    public class TabDayGrid : System.Windows.Forms.UserControl 
    {
        private System.Windows.Forms.Label lblGridCaption;
        private FlowLayoutPanel FLPanel;
        private void _OnResize(object sender, System.EventArgs e)
        {
            FLPanel.Height = this.Height - lblGridCaption.Size.Height;
        }
        public TabDayGrid(System.Windows.Forms.Control P) 
        {
            this.Dock = DockStyle.None;
            this.Width = 400;
            this.Height = 300;
            this.Parent = P;
            this.BackColor = Color.White;
            lblGridCaption = new System.Windows.Forms.Label();
            lblGridCaption.Dock = DockStyle.Top;
            this.Controls.Add(lblGridCaption);
            lblGridCaption.AutoSize = true;
            lblGridCaption.Font = new Font(Font.Name, Font.Size+4, Font.Style);
            lblGridCaption.BackColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
            FLPanel = new FlowLayoutPanel();
            FLPanel.Dock = DockStyle.Fill;
            this.Controls.Add(FLPanel);
            FLPanel.AutoScroll = true;
            FLPanel.BackColor = Color.FromArgb(0xFF, 0xFF, 0xFF);//0xD0, 0xFF, 0x40 0xFF, 0xFF, 0xFF
            this.Resize += _OnResize;
            this.FontChanged += new EventHandler(_OnFontChanged);
        }

        void _OnFontChanged(object sender, EventArgs e)
        {
            FLPanel.Font = new Font(((System.Windows.Forms.UserControl)sender).Font, ((System.Windows.Forms.UserControl)sender).Font.Style);
            lblGridCaption.Font = new Font(((System.Windows.Forms.UserControl)sender).Font.Name, ((System.Windows.Forms.UserControl)sender).Font.Size + 4, ((System.Windows.Forms.UserControl)sender).Font.Style);
        }

        public void FillDays(DateTime BeginDate, DateTime EndDate)
        {
            for (DateTime i = BeginDate; i <= EndDate; i=i.AddDays(1))
            {
                TabDayItem TD = new TabDayItem();
                TD.DateOfDay = i;// TD.DateOfDay.AddDays(i);
                TD.Width = 56;
                TD.Height = 96;
                TD.NoteText = "";
                TD.AutoSize = true;
                Padding M = new Padding(1);
                TD.Margin = M;
                TD.TabBtn.MouseClick += new MouseEventHandler(TD_MouseClick);
                TD.Enter += new EventHandler(TD_Enter);
                FLPanel.Controls.Add(TD);
            }
        
        }

        void TD_Enter(object sender, EventArgs e)
        {
            if (fOnItemClick != null)
            {
                fOnItemClick((TabDayItem)(sender));
            }

        }

        void TD_MouseClick(object sender, MouseEventArgs e)
        {
            if (fOnItemBtnClick != null)
            {
                fOnItemBtnClick((TabDayItem)(((Button)sender).Parent));
            }
        }

        void TD_Click(object sender, EventArgs e)
        {
            if (fOnItemBtnClick != null)
            {
                fOnItemBtnClick((TabDayItem)sender);
            }
        }

        public TabDayItem this[int i] 
        {
            get 
            {
                return (TabDayItem)FLPanel.Controls[i];
            }            
        }

        public string GridCaption
        {
            get 
            {
                return lblGridCaption.Text;
            }
            set 
            {
                lblGridCaption.Text = value;
                if (value == "")
                {
                    lblGridCaption.Visible = false;
                    FLPanel.Dock = DockStyle.Fill;
                }
                else
                {
                    //lblGridCaption.AutoSize = true;
                    lblGridCaption.Visible = true;
                    //lblGridCaption.AutoSize = false;
                    FLPanel.Height = this.Height - lblGridCaption.Size.Height;
                    FLPanel.Dock = DockStyle.Bottom;
                   
                }                
            }
        }
        public delegate void ItemClick(TabDayItem Item);
        private event ItemClick fOnItemClick;
        public event ItemClick OnItemClick
        {
            add
            {
                fOnItemClick += value;
            }
            remove
            {
                fOnItemClick -= value;
            }
        }
        public delegate void ItemBtnClick(TabDayItem Item);
        private event ItemBtnClick fOnItemBtnClick;
        public event ItemBtnClick OnItemBtnClick 
        {
            add 
            {
                fOnItemBtnClick += value;
            }
            remove
            {
                fOnItemBtnClick -= value;
            }
        }
    }

}
