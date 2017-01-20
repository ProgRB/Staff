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
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class AddAbsence : Form
    {
        ABSENCE_seq absence;
        string per_num;
        /// <summary>
        /// Конструктор формы оправдательного документа
        /// </summary>
        /// <param name="_per_num">Табельный номер</param>
        public AddAbsence(string _per_num)
        {
            InitializeComponent();
            per_num = _per_num;
            absence = new ABSENCE_seq(Connect.CurConnect);
            absence.AddNew();
            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).PER_NUM = per_num;
            ((ABSENCE_obj)((CurrencyManager)BindingContext[absence]).Current).TYPE_ABSENCE = 1;
            deDoc_Begin.AddBindingSource(absence, ABSENCE_seq.ColumnsName.ABS_TIME_BEGIN);
            deDoc_End.AddBindingSource(absence, ABSENCE_seq.ColumnsName.ABS_TIME_END);   
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            deDoc_End.Focus();
            btSave.Focus();            
            /// Проверяем ввод данных
            if (deDoc_Begin.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату начала действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_Begin.Focus();
                return;
            }
            if (deDoc_End.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату окончания действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_End.Focus();
                return;
            }
            if (deDoc_End.Date.Value < deDoc_Begin.Date.Value)
            {
                MessageBox.Show("Вы ввели неверную дату окончания действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDoc_End.Focus();
                return;
            }
            /// Строка содержит время начала действия документа
            string str = mbDoc_Begin.Text.Trim(); 
            /// Временная переменная для дат
            DateTime? db = ((ABSENCE_obj)(((CurrencyManager)BindingContext[absence]).Current)).ABS_TIME_BEGIN;
            try
            {
                /// Добавляем в дату начала время начала
                ((ABSENCE_obj)(((CurrencyManager)BindingContext[absence]).Current)).ABS_TIME_BEGIN =
                    new DateTime(db.Value.Year, db.Value.Month, db.Value.Day, Convert.ToInt32(str.Substring(0, 2)),
                        Convert.ToInt32(str.Substring(3, 2)), 0);
            }
            catch
            {
                MessageBox.Show("Вы ввели неверное время начала действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbDoc_Begin.Focus();
                return;
            }
            /// Строка содержит время окончания действия документа
            str = mbDoc_End.Text.Trim();
            db = ((ABSENCE_obj)(((CurrencyManager)BindingContext[absence]).Current)).ABS_TIME_END;
            try
            {
                /// Добавляем в дату окончания время окончания
                ((ABSENCE_obj)(((CurrencyManager)BindingContext[absence]).Current)).ABS_TIME_END =
                    new DateTime(db.Value.Year, db.Value.Month, db.Value.Day, Convert.ToInt32(str.Substring(0, 2)),
                        Convert.ToInt32(str.Substring(3, 2)), 0);
            }
            catch
            {
                MessageBox.Show("Вы ввели неверное время окончания действия документа!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mbDoc_End.Focus();
                return;
            }               
            absence.Save();
            Connect.Commit();   
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void deDoc_Begin_Leave(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(deDoc_Begin.Date);
            TimeSpan ti = DateTime.Now.Subtract(DateTime.Now.AddDays(-366));            
            if (DateTime.Now > date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
            {
                if (MessageBox.Show("Значение введенной даты начала документа больше на год текущей даты!" +
                    "\nВы хотите продолжить работу?",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    deDoc_Begin.Focus();
                    return;
                }
            }
            if (DateTime.Now < date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
            {
                if (MessageBox.Show("Значение введенной даты начала документа больше на год текущей даты!" +
                    "\nВы хотите продолжить работу?",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                { 
                    deDoc_Begin.Focus();
                    return;
                }
            }
        }

        private void deDoc_End_Leave(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(deDoc_End.Date);
            TimeSpan ti = DateTime.Now.Subtract(DateTime.Now.AddDays(-366));
            if (DateTime.Now > date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
            {
                if (MessageBox.Show("Значение введенной даты окончания документа больше на год " +
                    "текущей даты!\nВы хотите продолжить работу?",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    deDoc_End.Focus();
                    return;
                }
            }
            if (DateTime.Now < date && (Math.Abs(DateTime.Now.Subtract(date).Days)) > ti.Days)
            {
                if (MessageBox.Show("Значение введенной даты окончания документа больше на год " +
                    "текущей даты!\nВы хотите продолжить работу?",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                { 
                    deDoc_End.Focus();
                    return;
                }
            }               
        }

        private void AddAbsence_FormClosing(object sender, FormClosingEventArgs e)
        {
            Connect.Rollback();
        }
    }
}