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
namespace Kadr.Vacation_schedule
{
    public partial class GetPeriod : Form
    {
        
        public DateTime DateBegin
        {
            get
            {
                return vacPeriodForm1.Model.DateBegin.Value.Date;
            }
        }
        public DateTime DateEnd
        {
            get
            {
                return vacPeriodForm1.Model.DateEnd.Value.Date;
            }
        }

        /// <summary>
        /// Выбранные айдишники 
        /// </summary>
        public List<decimal> SelectedVacIDs
        {
            get
            {
                return vacPeriodForm1.Model.VacSource.Where(r => r.Row.Field<bool>("FL")).Select(r => r.Row.Field<decimal>("VAC_SCHED_ID")).ToList();
            }
        }

        /// <summary>
        /// Айдишник подразделения
        /// </summary>
        public decimal? SubdivID
        {
            get
            {
                return vacPeriodForm1.Model.SubdivID;
            }
        }

        public string CodeSubdiv
        {
            get
            {
                return vacPeriodForm1.Model.CodeSubdiv;
            }
        }
        
        public GetPeriod(DateTime DateBeg, DateTime DateEn) : this(DateBeg, DateEn, false,false) 
        {
            this.vacPeriodForm1.Model.IsNeedSelectedVacs = true;
        }
        public GetPeriod(DateTime DateBeg, DateTime DateEn,bool CheckAll) : this(DateBeg, DateEn, CheckAll, false) { }
        public GetPeriod(DateTime DateBeg, DateTime DateEn,bool CheckAll,bool OnlyFact):this(DateBeg, DateEn, CheckAll,OnlyFact,false){}

        public GetPeriod(DateTime DateBeg, DateTime DateEn,bool CheckAll,bool OnlyFact, bool lockDates)
        {
            InitializeComponent();
            vacPeriodForm1.Model = new PeriodVacModel() { DateBegin = DateBeg, DateEnd = DateEn, SubdivID = FilterVS.subdiv_id };
            vacPeriodForm1.ControlClosed += new EventHandler(vacPeriodForm1_ControlClosed);
            vacPeriodForm1.Model.CheckAll(CheckAll);

            if (lockDates)
                vacPeriodForm1.Model.IsPeriodEnabled = false;
            this.DialogResult = DialogResult.Cancel;
            this.vacPeriodForm1.Model.IsOnlyActual = OnlyFact?1m:0m;
            this.vacPeriodForm1.Model.UpdateVacs();
        }

        void vacPeriodForm1_ControlClosed(object sender, EventArgs e)
        {
            this.DialogResult = vacPeriodForm1.DialogResult == true ? DialogResult.OK : DialogResult.Cancel;
            this.Close();
        }
    }

}




