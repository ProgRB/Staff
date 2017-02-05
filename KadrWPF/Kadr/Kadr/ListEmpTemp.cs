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
using LibraryKadr;


namespace Kadr
{
    public partial class ListEmpTemp : Form
    {
        FormMain parentForm;
        public BindingSource bsEmp;
        OracleDataTable dtEmp;

        /// <summary>
        /// Конструктор формы списка претендентов на работу
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_dtEmp">Таблица содержит данные претендентов</param>
        /// <param name="_parentform">Родительская форма</param>
        public ListEmpTemp(OracleDataTable _dtEmp, FormMain _parentForm)
        {
            InitializeComponent();         
            parentForm = _parentForm;
            dtEmp = _dtEmp;
            dtEmp.Fill();
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;            
            dgViewEmpTemp.DataSource = bsEmp;
            dgViewEmpTemp.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgViewEmpTemp.Columns["CODE_SUBDIV"].Width = 50;
            dgViewEmpTemp.Columns["CODE_SUBDIV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgViewEmpTemp.Columns["per_num"].HeaderText = "Таб.№";
            dgViewEmpTemp.Columns["per_num"].Width = 55;
            dgViewEmpTemp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgViewEmpTemp.Columns["emp_last_name"].HeaderText = "Фамилия";
            dgViewEmpTemp.Columns["emp_last_name"].Width = 140;
            dgViewEmpTemp.Columns["emp_first_name"].HeaderText = "Имя";
            dgViewEmpTemp.Columns["emp_first_name"].Width = 120;
            dgViewEmpTemp.Columns["emp_middle_name"].HeaderText = "Отчество";
            dgViewEmpTemp.Columns["emp_middle_name"].Width = 160;
            dgViewEmpTemp.Columns["emp_birth_date"].HeaderText = "Дата рождения";
            dgViewEmpTemp.Columns["emp_birth_date"].Width = 80;
            dgViewEmpTemp.Columns["sign_comb"].HeaderText = "Совм.";
            dgViewEmpTemp.Columns["sign_comb"].Width = 50;
            try
            {
                dgViewEmpTemp.Columns["dismiss"].HeaderText = "Увол.";
                dgViewEmpTemp.Columns["dismiss"].Width = 50;
            }
            catch { }
            dgViewEmpTemp.Columns["transfer_id"].Visible = false;
            dgViewEmpTemp.Columns["pos_id"].Visible = false;
            dgViewEmpTemp.Columns["date_hire"].Visible = false;
            dgViewEmpTemp.Columns["CODE_POS"].HeaderText = "Шифр профессии";
            dgViewEmpTemp.Columns["CODE_POS"].Width = 90;
            dgViewEmpTemp.Columns["POS_NAME"].HeaderText = "Наименование профессия";
            dgViewEmpTemp.Columns["POS_NAME"].Width = 500;
            dgViewEmpTemp.Invalidate();
            ColumnWidthSaver.FillWidthOfColumn(dgViewEmpTemp);
            dgViewEmpTemp.DoubleClick += new EventHandler(parentForm.btEditEmp_Click);
        }        

        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListEmpTemp_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.btDeleteEmp.Enabled = false;
            parentForm.btEditEmp.Enabled = false;
            ColumnWidthSaver.SaveWidthOfAllColumns(dgViewEmpTemp);
        }

        /// <summary>
        /// Метод обновляет данные в датагриде
        /// </summary>
        public void EnterDataGridView()
        {
            dtEmp.Clear();
            dtEmp.Fill();
            dgViewEmpTemp.Invalidate();
        }

        private void dgViewEmpTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                bsEmp.Position -= 1;
                parentForm.btEditEmp_Click(sender, e);
            }
        }
    }
}
