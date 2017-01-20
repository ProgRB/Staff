using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using System.Reflection;
using LibraryKadr;
using WpfControlLibrary;

namespace Kadr
{
    public partial class Sort_Emp : System.Windows.Forms.UserControl
    {
        System.Windows.Forms.Control _listemp;
        /// <summary>
        /// Конструктор формы поиска
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Sort_Emp(System.Windows.Forms.Control listemp)
        {
            InitializeComponent();
            _listemp = listemp;
        }

        /// <summary>
        /// Событие нажатия кнопки поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFind_Click(object sender, EventArgs e)
        {
            string sort = "";
            if (btSubdivSorter.Checked)
            {
                sort = "order by CODE_SUBDIV";
            }
            if (btPer_NumSorter.Checked)
            {
                sort = "order by PER_NUM";
            }
            if (btSubdivPer_NumSorter.Checked)
            {
                sort = "order by CODE_SUBDIV, PER_NUM";
            }
            if (btFIOSorter.Checked)
            {
                sort = "order by EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME";
            }
            if (btSubdivFIOSorter.Checked)
            {
                sort = "order by CODE_SUBDIV, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME";
            }
            if (btBirth_DateSorter.Checked)
            {
                sort = "order by EMP_BIRTH_DATE, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME";
            }
            if (btDay_BirthSorter.Checked)
            {
                sort = "order by to_char(EMP_BIRTH_DATE,'mm.dd.yyyy'), EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME";
            }
            if (btDate_HireSorter.Checked)
            {
                sort = "order by DATE_HIRE, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME";
            }
            if (btDate_DismissSorter.Checked)
            {
                sort = "order by DATE_TRANSFER, EMP_LAST_NAME, EMP_FIRST_NAME, EMP_MIDDLE_NAME";
            }
            if (sort == "")
            {
                MessageBox.Show("Вы не выбрали критерии сортировки", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Sorter(sort);
        }

        /// <summary>
        /// Метод меняет сортировку списка работников и перезаполняет таблицу
        /// </summary>
        /// <param name="sort">Строка сортировки</param>
        void Sorter(string sort)
        {
            string tnom, strSelect;
            int pos;

            tnom = ((ListEmp)_listemp).dgEmp.Rows[((ListEmp)_listemp).bsEmp.Position].Cells["per_num"].Value.ToString();
            pos = ((ListEmp)_listemp).dtEmp.SelectCommand.CommandText.IndexOf("order by");
            strSelect = ((ListEmp)_listemp).dtEmp.SelectCommand.CommandText.Substring(0, pos) + sort;
            ((ListEmp)_listemp).bsEmp.PositionChanged -= ((ListEmp)_listemp).PositionChange;
            ((ListEmp)_listemp).dtEmp.Clear();
            ((ListEmp)_listemp).dtEmp.SelectCommand.CommandText = strSelect;
            ((ListEmp)_listemp).dtEmp.Fill();
            ((ListEmp)_listemp).bsEmp.PositionChanged += new EventHandler(((ListEmp)_listemp).PositionChange);
            ((ListEmp)_listemp).bsEmp.Position = ((ListEmp)_listemp).bsEmp.Find("PER_NUM", tnom);
            ((ListEmp)_listemp).dgEmp.Focus();
        }

        /// <summary>
        /// Событие нажатия кнопки перехода на первую запись в списке сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUp_Click(object sender, EventArgs e)
        {
            try
            {
                ((ListEmp)_listemp).bsEmp.MoveFirst();
                ((ListEmp)_listemp).dgEmp.Focus();
            }
            catch
            { }
        }

        /// <summary>
        /// Событие нажатия кнопки перехода на последнюю запись в списке сотрудников.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDown_Click(object sender, EventArgs e)
        {
            try
            {
                ((ListEmp)_listemp).bsEmp.MoveLast();
                ((ListEmp)_listemp).dgEmp.Focus();
            }
            catch
            { }
        }
    }
}
