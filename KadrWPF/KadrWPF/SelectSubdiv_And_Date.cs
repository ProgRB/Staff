using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Staff;
using LibraryKadr;

namespace Kadr
{
    public partial class SelectSubdiv_And_Date : Form
    {
        private int _subdiv_id = 0;
        private string _code_subdiv = "0";

        public DateTime SelectedDate
        {
            get { return dpDate.Value; }
        }

        public int Subdiv_ID
        {
            get { return _subdiv_id; }
        }

        public string Code_Subdiv
        {
            get { return _code_subdiv; }
        }

        /// <summary>
        /// Форма выбора подразделения, ввода периода или выбора оправ. документа
        /// При инициализации все компоненты скрыты от пользователя. В зависимости от входных параметров
        /// компоненты становятся видимыми.
        /// </summary>
        public SelectSubdiv_And_Date()
        {
            InitializeComponent();            
        }
        
        private void btSelect_Click(object sender, EventArgs e)
        {
            btSelect.Focus();
            if (ssSubdivForRep.Visible && ssSubdivForRep.subdiv_id == null)
            {
                MessageBox.Show("Не выбрано подразделение!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ssSubdivForRep.Focus();
                return;
            }
            else
            {
                _subdiv_id = Convert.ToInt32(ssSubdivForRep.subdiv_id);
                _code_subdiv = ssSubdivForRep.CodeSubdiv;
            }
            if (dpDate.Value == null)
            {
                MessageBox.Show("Дата не установлена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dpDate.Focus();
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
