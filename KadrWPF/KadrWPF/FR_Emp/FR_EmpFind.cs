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
    public partial class FR_EmpFind : Form
    {
        /// <summary>
        /// Строка поиска.
        /// </summary>
        public StringBuilder str_find = new StringBuilder();
        /// <summary>
        /// Строка сортировки.
        /// </summary>
        public StringBuilder sort = new StringBuilder();
        /// <summary>
        /// Конструктор формы поиска сторонних сотрудников.
        /// </summary>
        /// <param name="_connection">Строка подключения.</param>
        public FR_EmpFind()
        {
            InitializeComponent();
            /// Привязка комбобоксов к справочникам подразделений и должностей.
            cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                new LinkArgument(FR_Emp.subdivFR, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbSubdiv_Name.SelectedItem = null;
            cbPos_Name.AddBindingSource(POSITION_seq.ColumnsName.POS_ID.ToString(),
                new LinkArgument(FR_Emp.positionFR, POSITION_seq.ColumnsName.POS_NAME));
            cbPos_Name.SelectedItem = null;     
        }

        OracleDataTable oracleTable;
        /// <summary>
        /// Свойство возвращает таблицу, заполненную искомыми данными
        /// </summary>
        public OracleDataTable OracleDataTable
        {
            get
            {
                return oracleTable;
            }
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            /// Очищаем строки сортировки и поиска.
            sort.Remove(0, sort.Length);
            str_find.Remove(0, str_find.Length);
            /// Проверяем заполнение полей поиска и формируем строки поиска и сортировки.
            if (tbFR_Last_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(em.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    FR_EMP_seq.ColumnsName.FR_LAST_NAME, tbFR_Last_Name.Text.Trim()));
                sort.Append(sort.Length == 0 ? "fr_last_name" : ", fr_last_name");
            }
            if (tbFR_First_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(em.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    FR_EMP_seq.ColumnsName.FR_FIRST_NAME.ToString(), tbFR_First_Name.Text.Trim()));
                sort.Append(sort.Length == 0 ? "fr_first_name" : ", fr_first_name");
            }
            if (tbFR_Middle_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(em.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    FR_EMP_seq.ColumnsName.FR_MIDDLE_NAME.ToString(), tbFR_Middle_Name.Text.Trim()));
                sort.Append(sort.Length == 0 ? "fr_middle_name" : ", fr_middle_name");
            }
            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_find.Append(string.Format("{0} em.{1} = {2}", str_find.Length != 0 ? " and" : "",
                    FR_EMP_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue));
                sort.Append(sort.Length == 0 ? "subdiv_name" : ", subdiv_name");
            }
            if (cbPos_Name.SelectedValue != null && cbPos_Name.Text != "")
            {
                str_find.Append(string.Format("{0} em.{1} = {2}", str_find.Length != 0 ? " and" : "",
                    FR_EMP_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.SelectedValue));
                sort.Append(sort.Length == 0 ? "pos_name" : ", pos_name");
            }

            /// Если ввели хотя бы один реквизит осуществляем поиск.
            if (str_find.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                /// Вставляем необходимые слова в строки.
                str_find.Insert(0, " where ");                
                sort.Insert(0, " order by ");
                /// Проверяем есть ли сортировка по ФИО. Если нет, добавляем ее.
                int pos = sort.ToString().IndexOf("fr_last_name,fr_first_name,fr_middle_name");
                if (pos == -1)
                    sort.Append(",fr_last_name,fr_first_name,fr_middle_name");
                /// Формируем строку запроса сторонних сотрудников. Создаем и заполняем таблицу.
                string sql = string.Format(Queries.GetQuery("SelectListFR_Emp.sql"),
                    Connect.Schema, str_find.ToString(), sort.ToString());
                oracleTable = new OracleDataTable(sql, Connect.CurConnect);
                oracleTable.Fill();   
                /// Проверяем наличие данных.
                if (oracleTable.Rows.Count == 0)
                {
                    MessageBox.Show("В базе данных не найдена введенная информация!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    str_find.Remove(0, str_find.Length);
                    return;
                }           
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
