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
    public partial class SelPeriod_Subdiv : Form
    {
        private DateTime _maxDate = DateTime.Now.AddYears(1000);
        private DateTime _minDate = DateTime.Now.AddYears(-1000);
        private DateTime _beginDate;
        private DateTime _endDate;
        private int _subdiv_id = 0;
        private string _code_subdiv = "0";
        private Int16 _year = 2000;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set { _maxDate = value; }
        }

        public DateTime MinDate
        {
            get { return _minDate; }
            set { _minDate = value; }
        }

        public DateTime BeginDate
        {
            get { return _beginDate; }
            set { _beginDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public int Subdiv_ID
        {
            get { return _subdiv_id; }
        }

        public string Code_Subdiv
        {
            get { return _code_subdiv; }
        }

        public Int16 Year
        {
            get { return _year; }
        }
        /// <summary>
        /// Форма выбора подразделения, ввода периода или выбора оправ. документа
        /// При инициализации все компоненты скрыты от пользователя. В зависимости от входных параметров
        /// компоненты становятся видимыми.
        /// </summary>
        public SelPeriod_Subdiv(bool fl_visibleSubdiv, bool fl_visiblePeriod, bool fl_visibleDocs, bool fl_visibleYear)
        {
            InitializeComponent();
            /* Если признак показывать подразделение, то делаем его видимым.
                Иначе поднимаем все компоненты вверх*/ 
            if (fl_visibleSubdiv)
            {
                ssSubdivForRep.Visible = true;
            }
            else
            {
                lbDe1.Top = lbDe1.Top - 32;
                lbDe2.Top = lbDe2.Top - 32;
                de1.Top = de1.Top - 32;
                de2.Top = de2.Top - 32;                
                lbDocs.Top = lbDocs.Top - 32;
                cbDoc_List.Top = cbDoc_List.Top - 32;
                lbYear.Top = lbYear.Top - 32;
                tbYear.Top = tbYear.Top - 32;
                this.Height = this.Height - 32;
            }
            /* Если признак показывать период, то делаем компоненты видимыми.
                Иначе поднимаем оставшиеся компоненты вверх*/ 
            if (fl_visiblePeriod)
            {
                lbDe1.Visible = true;
                lbDe2.Visible = true;
                de1.Visible = true;
                de2.Visible = true;
                BeginDate = de1.Value.Date;
                EndDate = de2.Value.Date.AddDays(1).AddSeconds(-1);
            }
            else
            {                
                lbDocs.Top = lbDocs.Top - 32;
                cbDoc_List.Top = cbDoc_List.Top - 32;
                lbYear.Top = lbYear.Top - 32;
                tbYear.Top = tbYear.Top - 32;
                this.Height = this.Height - 32;
            }
            if (fl_visibleDocs)
            {
                lbDocs.Visible = true;
                cbDoc_List.Visible = true;
            }
            else
            {
                this.Height = this.Height - 32;
                lbYear.Top = lbYear.Top - 32;
                tbYear.Top = tbYear.Top - 32;
            }
            if (fl_visibleYear)
            {
                lbYear.Visible = true;
                tbYear.Visible = true;
            }
            else
            {
                lbYear.Top = lbYear.Top - 32;
                tbYear.Top = tbYear.Top - 32;
                this.Height = this.Height - 32;
            }
        }

        public SelPeriod_Subdiv(bool fl_visibleSubdiv, bool fl_visiblePeriod, bool fl_visibleDocs)
            : this(fl_visibleSubdiv, fl_visiblePeriod, fl_visibleDocs, false)
        { }

        private void de1_Validating(object sender, CancelEventArgs e)
        {
            if (de1.Value != null)
            {
                if (!(de1.Value >= MinDate && de1.Value <= MaxDate))
                {
                    MessageBox.Show("Дата начала периода должна быть в промежутке: \n" +
                        MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    de1.Focus();
                    return;
                }
                BeginDate = (DateTime)de1.Value.Date;
            }
        }

        private void de2_Validating(object sender, CancelEventArgs e)
        {
            if (de2.Value != null)
            {
                if (!(de2.Value >= MinDate && de2.Value <= MaxDate))
                {
                    MessageBox.Show("Дата окончания периода должна быть в промежутке: \n" +
                        MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    de2.Focus();
                    return;
                }
                EndDate = (DateTime)de2.Value.Date.AddDays(1).AddSeconds(-1);
            }
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
            if (de1.Value == null && de1.Visible)
            {
                MessageBox.Show("Дата начала периода должна быть в промежутке: \n" +
                    MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                de1.Focus();
                return;
            }
            if (de2.Value == null && de2.Visible)
            {
                MessageBox.Show("Дата окончания периода должна быть в промежутке: \n" +
                    MinDate.ToShortDateString() + " - " + MaxDate.ToShortDateString(),
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                de2.Focus();
                return;
            }
            if (tbYear.Text.Trim() != "")
            {
                _year = Convert.ToInt16(tbYear.Text.Trim());
            }
            Close();
            this.DialogResult = DialogResult.OK;
        }
    }
}
