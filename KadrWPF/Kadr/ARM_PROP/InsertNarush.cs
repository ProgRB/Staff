using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using Staff;
using LibraryKadr;
using Kadr;

namespace ARM_PROP
{
    public partial class InsertNarush : Form
    {
        string per_num, other_violation_id, perco_sync_id, cause_arrest, last_name, first_name,
            middle_name, unit; 
        decimal summ, types_exact_id, type_violation_id;
        decimal kolvo;
        decimal violation_log_id;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq type_violation;
        TYPE_EXACT_seq types_exact;
        LIST_VIOLATOR_seq list_violation;
        EMP_seq emp;
        InsertNarush insertNarush;

        /// <summary>
        /// Конструктор формы редактирования нарушения
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public InsertNarush(string _per_num, string _other_violation_id, 
            string _perco_sync_id, string _last_name, string _first_name,
            string _middle_name, decimal _violation_log_id, string _cause_arrest, decimal _types_exact_id, 
            string _unit, decimal _kolvo, decimal _summ, decimal _type_violation_id)
        {
            InitializeComponent();
            per_num = _per_num;
            other_violation_id = _other_violation_id;
            perco_sync_id = _perco_sync_id;
            last_name = _last_name;
            first_name = _first_name;
            middle_name = _middle_name;
            violation_log_id = _violation_log_id;
            type_violation_id = _type_violation_id;
            unit = _unit;
            kolvo = _kolvo;
            summ = _summ;

            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill(string.Format("where {0} = {1}", VIOLATION_LOG_seq.ColumnsName.VIOLATION_LOG_ID, violation_log_id));
            type_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            type_violation.Fill();
            types_exact = new TYPE_EXACT_seq(Connect.CurConnect);
            types_exact.Fill();
            emp = new EMP_seq(Connect.CurConnect);
            emp.Fill();

            pictureBox1.Image = EmployeePhoto.GetPhoto(per_num);

            tbFirst.Text = last_name;
            tbLast.Text = first_name;
            tbMiddle.Text = middle_name;
            tbPriZader.Text = cause_arrest;
            tbEdinIzm.Text = unit;
            //tbKolvo.Text = kolvo;
            //tbSumm.Text = summ;

            cbPriznak.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_VIOLATION_ID, new LinkArgument(type_violation, TYPE_VIOLATION_seq.ColumnsName.TYPE_VIOLATION_NAME));
            cbVzisk.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_EXACT_ID, new LinkArgument(types_exact, TYPE_EXACT_seq.ColumnsName.TYPE_EXACT_NAME));
            cbPriznak.SelectedValue = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID;
            tbPriZader.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.CAUSE_ARREST);
            mtbDatevzisk.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.EXACT_DATE);
            mtbZader.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.ARREST_DATE);
            mtbPodra.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.INFORM_SUBD_DATE);
            tbMesuri.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TAKE_MEASURES);
            tbNote.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.NOTE);
            tbKolvo.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.QUANTITY_GOODS);
            tbSumm.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TOTAL_STOLEN);
            checkBox1.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.SIGN_GROUP);
            checkBox2.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.SIGN_CRIMINAL);
            chbprizKrag.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.SIGN_STEAL);
            tbEdinIzm.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.UNIT);

            pnButton.EnableByRules();
        }

        /// <summary>
        /// Событие нажатия кнопки сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            violation_log.Save();
            Connect.Commit();
            this.Close();
            //violation_log.Clear();
        }

        /// <summary>
        /// Событие нажатия кнопки отмены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Событие выполняемые при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertNarush_Load(object sender, EventArgs e)
        {
                  
        }

        /// <summary>
        /// Событие выполняемые при изменении взыскания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPriznak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPriznak.Text == "Кража")
            {
                if (radioButton1.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "1";
                }
                if (radioButton2.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "2";
                }
                if (radioButton3.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "3";
                }
                if (radioButton4.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "4";
                }

                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "4")
                {
                    radioButton4.Checked = true;
                }
                 if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "3")
                {
                    radioButton3.Checked = true;
                }
                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "2")
                {
                    radioButton2.Checked = true;
                }
                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "1")
                {
                    radioButton1.Checked = true;
                }
                chbprizKrag.Checked = true;
                ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).SIGN_STEAL = true;
                panel4.Visible = true;
                this.Width = 872; 
            }
            if (cbPriznak.Text != "Кража")
            {
                chbprizKrag.Checked = false;
                ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).SIGN_STEAL = false;
                panel4.Visible = false;
                this.Width = 524;
                tbSumm.Clear();
                tbEdinIzm.Clear();
                tbKolvo.Clear();                
            }
        }

        /// <summary>
        /// Событие выполняемые chekedBox признака кражи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbprizKrag_CheckedChanged(object sender, EventArgs e)
        {
            if (chbprizKrag.Checked == false)
            {
                cbPriznak.Text = "Прочее";
            }
            if (chbprizKrag.Checked == true)
            {
                cbPriznak.Text = "Кража";
            }
        }
    }
}
