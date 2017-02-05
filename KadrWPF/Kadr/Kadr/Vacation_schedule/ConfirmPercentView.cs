using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using LibraryKadr;

namespace Kadr.Vacation_schedule
{
    public partial class ConfirmPercentView : Form, INotifyPropertyChanged
    {
        DataTable ds;
        OracleDataAdapter odaLoadPercent;
        public ConfirmPercentView(int Year)
        {
            ds = new DataTable();
            odaLoadPercent = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO\SelectConfirmVacPercent.sql"), Connect.Schema), Connect.CurConnect);
            odaLoadPercent.SelectCommand.BindByName = true;
            odaLoadPercent.SelectCommand.Parameters.Add("p_date", OracleDbType.Date, null, ParameterDirection.Input);
            InitializeComponent();
            dgSubdivConfirm.AutoGenerateColumns = false;
            ColumnWidthSaver.FillWidthOfColumn(dgSubdivConfirm);
            dgSubdivConfirm.ColumnWidthChanged += ColumnWidthSaver.SaveWidthOfColumn;
            tsYearCurrent.ValueChanged += new EventHandler(tsbtRefresh_Click);
            tsYearCurrent.Value = Year;
            dgSubdivConfirm.DataBindings.Add("DataSource", this, "SubdivView");
        }

        DataView _subview;
        public DataView SubdivView
        {
            get
            {
                return _subview;
            }
            set
            {

                _subview = value;
                OnPropertyChanged("SubdivView");
            }
        }

        private void UpdateData( DateTime pdate)
        {
            ds.Clear();
            odaLoadPercent.SelectCommand.Parameters["p_date"].Value = pdate;
            try
            {
                odaLoadPercent.Fill(ds);
                if (_subview == null)
                    _subview = new DataView(ds, "", "CODE_SUBDIV", DataViewRowState.CurrentRows);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Library.GetMessageException(ex), "Ошибка получения данных");
            }
        }
        private void tsbtRefresh_Click(object sender, EventArgs e)
        {
            UpdateData(new DateTime((int)tsYearCurrent.Value, 1, 1));
        }
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    
        public event PropertyChangedEventHandler  PropertyChanged;

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
