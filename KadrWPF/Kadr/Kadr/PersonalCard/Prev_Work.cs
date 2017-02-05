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
    public partial class Prev_Work : Form
    {
        DataGridView dgViewPrev_Work;
        VisiblePanel pnVisible;

        // Объявление таблиц
        PREV_WORK_seq prev_work;

        // Объявление строки, табельного номера и флага добавления данных
        PREV_WORK_obj r_prev_work;
        string per_num;
        bool f_addprev_work;
        int pos;
        DateTime dateHire;

        // Создание формы
        public Prev_Work(string _per_num, bool _f_addprev_work, int _pos, 
            PREV_WORK_seq _prev_work, POSITION_seq _position, DataGridView _dgView, VisiblePanel _pnVisible,
            DateTime _dateHire)
        {
            InitializeComponent();
            dgViewPrev_Work = _dgView;
            pnVisible = _pnVisible;
            f_addprev_work = _f_addprev_work;
            prev_work = _prev_work;
            per_num = _per_num;
            pos = _pos;
            dateHire = _dateHire;

            // Проверка нужно ли добавлять новую запись
            if (f_addprev_work)
            {
                r_prev_work = prev_work.AddNew();
                r_prev_work.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[prev_work]).Position = ((CurrencyManager)BindingContext[prev_work]).Count - 1;
            }
            else
            {
                BindingContext[prev_work].Position = pos;
                r_prev_work = (PREV_WORK_obj)((CurrencyManager)BindingContext[prev_work]).Current;
            }  

            // Привязка компонентов
            tbName_Pos.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.PW_NAME_POS);
            tbFirm.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.PW_FIRM);
            chWork_In_Fact.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.WORK_IN_FACT);
            chMedical_Sign.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.MEDICAL_SIGN);
            //mbDate_Start.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.PW_DATE_START);
            //mbDate_End.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.PW_DATE_END);
            deDate_Start.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.PW_DATE_START);
            deDate_Start.TextChanged += new EventHandler(deDate_Start_TextChanged);
            deDate_End.AddBindingSource(prev_work, PREV_WORK_seq.ColumnsName.PW_DATE_END);
            deDate_End.TextChanged += new EventHandler(deDate_End_TextChanged);
            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        void deDate_Start_TextChanged(object sender, EventArgs e)
        {
            if (deDate_Start.Text.Trim().Length == 10)
            {
                if (dateHire <= deDate_Start.Date)
                {
                    MessageBox.Show("Дата начала периода работы больше даты приема на работу.\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deDate_Start.Focus();
                }
                else
                {
                    Raschet();
                }
            }
        }

        void deDate_End_TextChanged(object sender, EventArgs e)
        {
            if (deDate_End.Text.Trim().Length == 10)
            {
                if (deDate_Start.Date != null && deDate_Start.Date >= deDate_End.Date)
                {
                    MessageBox.Show("Дата начала периода работы больше даты окончания периода работы.\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deDate_End.Focus();
                }
                else
                {
                    if (dateHire <= deDate_End.Date)
                    {
                        MessageBox.Show("Дата окончания периода работы больше даты приема на работу.\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        deDate_End.Focus();
                    }
                    else
                    {
                        Raschet();
                    }
                }
            }
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            if (deDate_Start.TextDate == null || deDate_Start.TextDate.Replace(".","").Trim() == "" || 
                 deDate_End.TextDate == null || deDate_End.TextDate.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Невозможно сохранить данные! Период работы введен не полностью. \nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            prev_work.Save();
            Connect.Commit();
            f_addprev_work = false;
            Library.VisiblePanel(dgViewPrev_Work, pnVisible);
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Prev_Work_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addprev_work)
            {
                prev_work.CancelNew(((CurrencyManager)BindingContext[prev_work]).Position);
            }
            else if (prev_work.IsDataChanged())
            {
                prev_work.RollBack();
            }
            dgViewPrev_Work.Invalidate();
        }

        void Raschet()
        {
            decimal year, month, day;
            year = month = day = 0;
            if (deDate_Start.Date != null && deDate_End.Date != null)
            {
                //Library.CalculationWork_Length((DateTime)deDate_Start.Date, (DateTime)deDate_End.Date, ref year, ref month, ref day);
                Library.CalcStanding((DateTime)deDate_Start.Date, (DateTime)deDate_End.Date, ref year, ref month, ref day);
                tbYear.Text = year.ToString();
                tbMonth.Text = month.ToString();
                tbDay.Text = day.ToString();
            }
        }

        private void Prev_Work_Activated(object sender, EventArgs e)
        {
            Raschet();
        }

        /// <summary>
        /// Перевод наименования фирмы в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFirm_Validating(object sender, CancelEventArgs e)
        {
            tbFirm.Text = tbFirm.Text.ToUpper();
        }

        /// <summary>
        /// Перевод должности в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbName_Pos_Validating(object sender, CancelEventArgs e)
        {
            tbName_Pos.Text = tbName_Pos.Text.ToUpper();
        }

        private void deDate_End_Validating(object sender, CancelEventArgs e)
        {
            if (dateHire <= deDate_End.Date)
            {
                MessageBox.Show("Дата окончания периода работы больше даты приема на работу.\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void deDate_Start_Validating(object sender, CancelEventArgs e)
        {
            if (dateHire <= deDate_Start.Date)
            {
                MessageBox.Show("Дата начала периода работы больше даты приема на работу.\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }
    }
}
