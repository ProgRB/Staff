using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryKadr
{
    public partial class TimeEditorClass : UserControl
    {
        public TextBox tbHMEditor;
        public TextBox tbDecEditor;
        public Label lblHM;
        public Label lblDec;
        private int fSeconds;
        public int Seconds 
        {
            get 
            { 
                return fSeconds; 
            }
            set 
            { 
                fSeconds = value;
                if (OnChangeTime != null)
                    OnChangeTime(this, new EventArgs());
                if (!tbDecEditor.Focused)
                    tbDecEditor.Text = ((float)(fSeconds/3600.0)).ToString("f2");
                if (!tbHMEditor.Focused)
                    tbHMEditor.Text = (fSeconds/3600).ToString("d2") + ":" + ((fSeconds-(fSeconds/3600)*3600)/60).ToString("d2");
            }
        }
        public event EventHandler OnChangeTime;

        public TimeEditorClass()
        {
            //InitializeComponent();
            AutoScaleMode = AutoScaleMode.Font;
            Width = 172;
            Height = 48;
            //Font = new Font(Font.Name, 10); 
            tbDecEditor = new TextBox();
            tbDecEditor.Parent = this;
            tbDecEditor.Left = 120;
            tbDecEditor.Width = 48;
            
            lblDec = new Label();
            lblDec.Parent = this;
            lblDec.Text = "Десятичный формат:";
            lblDec.AutoSize = true;
            lblDec.Top = 2;
            
            tbHMEditor = new TextBox();
            tbHMEditor.Parent = this;
            tbHMEditor.Top = tbDecEditor.Bottom + 2;
            tbHMEditor.Left = 120;
            tbHMEditor.Width = 48;
            //tbHMEditor.ReadOnly = true;
            
            //tbHMEditor.Enabled = false;
            lblHM = new Label();
            lblHM.Parent = this;
            lblHM.Text = "Формат времени:";
            lblHM.AutoSize = true;
            lblHM.Top = tbHMEditor.Top + 2;
            
            tbDecEditor.TextChanged += new EventHandler(tbDecEditor_TextChanged);
            tbHMEditor.TextChanged += new EventHandler(tbHMEditor_TextChanged);
            //Application.DoEvents();
            
        }

        void tbHMEditor_TextChanged(object sender, EventArgs e)
        {
            //DateTime D = DateTime.Parse(tbDecEditor.Text, "HH:mm");
            //Seconds = (int)(Num * 3600);
        }

        void tbDecEditor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float Num = float.Parse(tbDecEditor.Text);
                tbDecEditor.BackColor = SystemColors.Window;
                tbHMEditor.BackColor = Color.FromArgb(0xF0, 0xF0, 0xE0);
                Seconds = ((int)(Num * 3600) / 60 * 60);
                if (OnChangeTime != null)
                    OnChangeTime(this, new EventArgs());
            }
            catch
            {
                tbDecEditor.BackColor = Color.LightCoral;
                Seconds = Seconds;
            }
        }
    }
}
