using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;

namespace ARM_PROP
{
    public partial class AddForm : Form
    {
        OTHER_VIOLATOR_seq other_violator;
        LIST_VIOLATOR_seq list_violator;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq type_violation;
        TYPE_EXACT_seq type_exact;
        public decimal other_violator_id;

        /// <summary>
        /// Конструктор формы добавления нарушителя
        /// </summary>
        /// <param name="_connection">Строка подключения</param>        
        public AddForm(decimal _other_violator_id)
        {
            InitializeComponent();
            other_violator_id = _other_violator_id;

            other_violator = new OTHER_VIOLATOR_seq(Connect.CurConnect);
            other_violator.Fill();
            list_violator = new LIST_VIOLATOR_seq(Connect.CurConnect);
            list_violator.Fill();
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill();
            type_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            type_violation.Fill();
            type_exact = new TYPE_EXACT_seq(Connect.CurConnect);
            type_exact.Fill();

            tbFirst_name.AddBindingSource(other_violator, OTHER_VIOLATOR_seq.ColumnsName.OT_FIRST_NAME);
            tbLast_name.AddBindingSource(other_violator, OTHER_VIOLATOR_seq.ColumnsName.OT_LAST_NAME);
            tbMiddle_name.AddBindingSource(other_violator, OTHER_VIOLATOR_seq.ColumnsName.OT_MIDDLE_NAME);
            dgvNaruh.AddBindingSource(other_violator, new LinkArgument(other_violator, OTHER_VIOLATOR_seq.ColumnsName.OTHER_VIOLATOR_ID));
        }

        /// <summary>
        /// Событие выполняемые при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddForm_Load(object sender, EventArgs e)
        {
            tbFirst_name.ReadOnly = true;
            tbLast_name.ReadOnly = true;
            tbMiddle_name.ReadOnly = true;
        }

        /// <summary>
        /// Событие нажатия кнопки добавления нарушителя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_Click(object sender, EventArgs e)
        {
            other_violator.AddNew();
            //((OTHER_VIOLATOR_obj)((CurrencyManager)BindingContext[other_violator]).Current).OTHER_VIOLATOR_ID = 
            //    ((OTHER_VIOLATOR_obj)((CurrencyManager)BindingContext[other_violator]).Current).OTHER_VIOLATOR_ID;
            ((CurrencyManager)BindingContext[other_violator]).Position = ((CurrencyManager)BindingContext[other_violator]).Count;
            tbFirst_name.ReadOnly = false;
            tbLast_name.ReadOnly = false;
            tbMiddle_name.ReadOnly = false;
        }

        /// <summary>
        /// Событие нажатия кнопки выход из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        /// <summary>
        /// Событие нажатия кнопки выбора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {            
            other_violator.Save();
            Connect.Commit();
            other_violator_id = Convert.ToDecimal(dgvNaruh.CurrentRow.Cells["other_violator_id"].Value);
            Close();
        }

    }
}
