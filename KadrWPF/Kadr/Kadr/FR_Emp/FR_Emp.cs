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
    public partial class FR_Emp : Form
    {
        public static SUBDIV_seq subdivFR;
        public static POSITION_seq positionFR;
        FormMain parentForm;
        public BindingSource bsFR_Emp, bsFR_EmpDismiss;
        public OracleDataTable dtFR_Emp, dtFR_EmpDismiss;
        string _textQueryFR;
        /// <summary>
        /// Конструктор формы работы со сторонними сотрудниками.
        /// </summary>
        /// <param name="_connection">Строка подключения.</param>
        /// <param name="_parentForm">Родительсткая форма.</param>
        /// <param name="_dtFR_Emp">Таблица сторонних работников.</param>
        /// <param name="_dtFR_EmpDismiss">Таблица уволенных сторонних сотрудников.</param>
        public FR_Emp(FormMain _parentForm, OracleDataTable _dtFR_Emp, OracleDataTable _dtFR_EmpDismiss, string textQueryFR)
        {
            InitializeComponent();
            pnButton.EnableByRules();
            parentForm = _parentForm;
            dtFR_Emp = _dtFR_Emp;
            _textQueryFR = textQueryFR;
            dtFR_Emp.Fill();
            dtFR_EmpDismiss = _dtFR_EmpDismiss;
            dtFR_EmpDismiss.Fill();
            /// Создаем таблицу подразделений. 
            /// Заполняем ее только сторонними подразделениями (признак равен 6).
            subdivFR = new SUBDIV_seq(Connect.CurConnect);
            subdivFR.Fill("where type_subdiv_id = 6");
            /// Создаем таблицу должностей. 
            /// Заполняем ее всеми должностями.
            positionFR = new POSITION_seq(Connect.CurConnect);
            positionFR.Fill();
            
            bsFR_Emp = new BindingSource();
            bsFR_Emp.DataSource = dtFR_Emp;
            dgFR_Emp.DataSource = bsFR_Emp;
            bsFR_EmpDismiss = new BindingSource();
            bsFR_EmpDismiss.DataSource = dtFR_EmpDismiss;
            dgFR_EmpDismiss.DataSource = bsFR_EmpDismiss;
            RefreshDataGrid(dgFR_Emp);
            RefreshDataGrid(dgFR_EmpDismiss);
            dgFR_Emp.DoubleClick += new EventHandler(btEditFR_Emp_Click);
            dgFR_Emp.KeyPress += new KeyPressEventHandler(dgFR_Emp_KeyPress);
            /// Так как первой показана вкладка работающих сторонних сотрудников, скрываем дату увольнения.
            dgFR_Emp.Columns["FR_DATE_END"].Visible = false;
        }

        void dgFR_Emp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                btEditFR_Emp_Click(sender, e);
            }
            else
            {
                if (e.KeyChar >= Convert.ToChar(1040) && e.KeyChar <= Convert.ToChar(1103))
                {
                    string str_find = string.Format(" where FR_DATE_END is null and upper(em.FR_LAST_NAME) like '{0}%' ", 
                        e.KeyChar.ToString().ToUpper());
                    string sort = " order by fr_last_name,fr_first_name,fr_middle_name";
                    /// Формируем строку запроса сторонних сотрудников. Создаем и заполняем таблицу.
                    string sql = string.Format(Queries.GetQuery("SelectListFR_Emp.sql"),
                        Connect.Schema, str_find.ToString(), sort.ToString());
                    OracleDataTable oracleTable = new OracleDataTable(sql, Connect.CurConnect);
                    oracleTable.Fill();
                    /// Проверяем наличие данных.
                    if (oracleTable.Rows.Count == 0)
                    {
                        return;
                    }
                    /// Запоминаем нужную нам позицию - первая в списке.
                    string perco_sync_id = oracleTable.Rows[0][0].ToString();
                    /// Ищем позицию начала сортировки.
                    int posOrder = _textQueryFR.IndexOf(" order by ");
                    /// Обновляем строку запроса новой сортировкой, согласно заполненным полям поиска.
                    _textQueryFR = _textQueryFR.Substring(0, posOrder) + sort.ToString();
                    /// Очищаем таблицу, заполняем строку запроса и заполняем таблицу отсортированными данными.
                    dtFR_Emp.Clear();
                    dtFR_Emp.SelectCommand.CommandText = _textQueryFR;
                    dtFR_Emp.Fill();
                    /// Ищем нужную нам позицию в новом списке.
                    for (int i = 0; i < dgFR_Emp.Rows.Count; i++)
                    {
                        if (dgFR_Emp["perco_sync_id", i].Value.ToString() == perco_sync_id)
                        {
                            bsFR_Emp.Position = dgFR_Emp["perco_sync_id", i].RowIndex;
                            break;
                        }
                    }
                    dgFR_Emp.Focus();
                }
            }
        }

        private void btAddFR_Emp_Click(object sender, EventArgs e)
        {
            FR_EmpAdd fr_EmpAdd = new FR_EmpAdd(subdivFR, positionFR, true, 0);
            fr_EmpAdd.Text = "Добавление данных о новом стороннем сотруднике";
            fr_EmpAdd.ShowDialog();
            dtFR_Emp.Clear();
            dtFR_Emp.Fill();
        }
        
        public void btEditFR_Emp_Click(object sender, EventArgs e)
        {
            if (dgFR_Emp.RowCount != 0)
            {
                int pos = bsFR_Emp.Position;
                int id = Convert.ToInt32(dgFR_Emp.Rows[pos].Cells["perco_sync_id"].Value);
                FR_EmpAdd fr_EmpAdd = new FR_EmpAdd(subdivFR, positionFR, false, id);
                fr_EmpAdd.Text = "Редактирование данных о стороннем сотруднике";
                fr_EmpAdd.ShowDialog();
                dtFR_Emp.Clear();
                dtFR_Emp.Fill();
                bsFR_Emp.Position = pos;
            }
        }

        private void btDismissFR_Emp_Click(object sender, EventArgs e)
        {
            if (dgFR_Emp.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите уволить сотрудника?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgFR_Emp.Rows[bsFR_Emp.Position].Cells["perco_sync_id"].Value);
                    FR_EMP_seq fr_Emp = new FR_EMP_seq(Connect.CurConnect);
                    fr_Emp.Fill(string.Format("where {0} = {1}", FR_EMP_seq.ColumnsName.PERCO_SYNC_ID, id));
                    /// Если увольнение в перко прошло успешно увольняем его в нашей базе.
                    if (FormMain.employees.DismissEmployee(id.ToString(), " ", DateTime.Now.AddDays(-1)))
                    {
                        ((FR_EMP_obj)((CurrencyManager)BindingContext[fr_Emp]).Current).FR_DATE_END = DateTime.Now;
                        fr_Emp.Save();
                        Connect.Commit();
                        dtFR_Emp.Clear();
                        dtFR_EmpDismiss.Clear();
                        dtFR_Emp.Fill();
                        dtFR_EmpDismiss.Fill();
                    }
                    else
                    {
                        MessageBox.Show("Увольнение в Перко не прошло.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    
                }
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод обновляет грид сторонних сотрудников
        /// </summary>
        public void RefreshDataGrid(DataGridView _dg)
        {
            _dg.Columns["FR_LAST_NAME"].HeaderText = "Фамилия";
            _dg.Columns["FR_FIRST_NAME"].HeaderText = "Имя";
            _dg.Columns["FR_MIDDLE_NAME"].HeaderText = "Отчество";            
            _dg.Columns["SUBDIV_NAME"].HeaderText = "Подразделение";
            _dg.Columns["POS_NAME"].HeaderText = "Должность";
            _dg.Columns["FR_DATE_START"].HeaderText = "Дата начала работы";
            _dg.Columns["FR_DATE_END"].HeaderText = "Дата уволения";
            _dg.Columns["PERCO_SYNC_ID"].Visible = false;
        }

        private void tcFR_Emp_SelectedIndexChanged(object sender, EventArgs e)
        {
            /// Если выбрана вкладка работающих сторонних сотрудников, делаем невидимой поле Дата увольнения.
            if (tcFR_Emp.SelectedIndex == 0)
                dgFR_Emp.Columns["FR_DATE_END"].Visible = false;
            else
                dgFR_Emp.Columns["FR_DATE_END"].Visible = true;
        }

        private void FR_Emp_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.rgFilterFR_Emp.Visible = false;
        }
    }
}
