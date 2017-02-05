using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kadr
{
    public partial class ReopenConnect : Form
    {
        FormMain parent;
        public ReopenConnect(FormMain _parent)
        {
            InitializeComponent();
            parent = _parent;
        }

        /// <summary>
        /// Сворачивание программы в панель задач
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDisplace_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            parent.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Закрыть программу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClose_Click(object sender, EventArgs e)
        {
            parent.Close();
        }
    }
}
