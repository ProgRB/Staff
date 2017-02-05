using LibraryKadr;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VacationSchedule
{
    /// <summary>
    /// Interaction logic for RecalcPeriods.xaml
    /// </summary>
    public partial class RecalcPeriods : Window
    {
        private RecalcPeriodViewModel _model;

        public RecalcPeriods(decimal? transferID, decimal? vacGroupTypeID)
        {
            _model = new RecalcPeriodViewModel();
            InitializeComponent();
            Model.TransferID = transferID;
            Model.VacGroupTypeID = vacGroupTypeID;
            Model.RefreshData();
            DataContext = Model;
        }

        RecalcPeriodViewModel Model
        {
            get
            {
                return _model;
            }
        }

        private void CheckAll_Checked(object sender, RoutedEventArgs e)
        {
            Model.SetCheckAll((sender as CheckBox).IsChecked);
        }

        private void Save_Executed(object sender, RoutedEventArgs e)
        {
            Exception ex = Model.Save();
            if (ex != null)
                MessageBox.Show(Window.GetWindow(this), ex.GetFormattedException(), "Ошибка сохранения данных");
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void Close_Executed(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public partial class RecalcPeriodViewModel : NotificationObject
    {
        DataView dataView;
        DataSet ds;
        OracleDataAdapter odaPeriods;
        private decimal? _transferID;
        private decimal? _vacGroupTypeID;

        public RecalcPeriodViewModel()
        {
            ds = new DataSet();
            odaPeriods = new OracleDataAdapter(Queries.GetQueryWithSchema(@"go\ReCalcPeriods.sql"), Connect.CurConnect);
            odaPeriods.SelectCommand.BindByName = true;
            odaPeriods.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, TransferID, ParameterDirection.Input);
            odaPeriods.SelectCommand.Parameters.Add("p_vac_group_type_id", OracleDbType.Decimal, TransferID, ParameterDirection.Input);
            odaPeriods.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaPeriods.TableMappings.Add("Table", "data");
            ds.Tables.Add("data");
            ds.Tables["DATA"].Columns.Add("FL", typeof(bool));

            odaPeriods.UpdateCommand = new OracleCommand(string.Format("begin {0}.VAC_UPDATE_PERIOD(:p_vac_consist_id,:p_new_begin,:p_new_end); end;", Connect.Schema), Connect.CurConnect);
            odaPeriods.UpdateCommand.BindByName = true;
            odaPeriods.UpdateCommand.Parameters.Add("p_vac_consist_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaPeriods.UpdateCommand.Parameters.Add("p_new_begin", OracleDbType.Date, null, ParameterDirection.Input);
            odaPeriods.UpdateCommand.Parameters.Add("p_new_end", OracleDbType.Date, null, ParameterDirection.Input);
        }

        /// <summary>
        /// Источник данных список периодов использования оптусков
        /// </summary>
        public List<DataRowView> PeriodSource
        {
            get
            {
                if (dataView != null)
                    return dataView.OfType<DataRowView>().ToList();
                else
                    return new List<DataRowView>();
            }
        }

        /// <summary>
        /// Перевод сотрудника для расчетов
        /// </summary>
        public decimal? TransferID
        {
            get
            {
                return _transferID;
            }
            set
            {
                _transferID = value;
                RaisePropertyChanged(() => TransferID);
            }
        }

        /// <summary>
        /// Группа отпусков айдишник
        /// </summary>
        public decimal? VacGroupTypeID
        {
            get
            {
                return _vacGroupTypeID;
            }
            set
            {
                _vacGroupTypeID = value;
                RaisePropertyChanged(() => VacGroupTypeID);
            }
        }


        /// <summary>
        /// Обновление данных
        /// </summary>
        internal void RefreshData()
        {
            if (ds.Tables.Contains("data"))
                ds.Tables["data"].Rows.Clear();
            odaPeriods.SelectCommand.Parameters["p_transfer_id"].Value = TransferID;
            odaPeriods.SelectCommand.Parameters["p_vac_group_type_id"].Value = VacGroupTypeID;
            try
            {
                odaPeriods.Fill(ds);
                if (dataView == null)
                    dataView = new DataView(ds.Tables["data"], "", "", DataViewRowState.CurrentRows);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetFormattedException(), "Ошибка получения данных");
            }
            RaisePropertyChanged(() => PeriodSource);
        }

        internal Exception Save()
        {
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                foreach (DataRowView r in PeriodSource)
                    if (!string.IsNullOrEmpty(r["ERROR_MESSAGE"].ToString()))
                    {
                        odaPeriods.UpdateCommand.Parameters["p_vac_consist_id"].Value = r["vac_consist_id"];
                        odaPeriods.UpdateCommand.Parameters["p_new_begin"].Value = r["new_begin"];
                        odaPeriods.UpdateCommand.Parameters["p_new_end"].Value = r["new_end"];
                        odaPeriods.UpdateCommand.ExecuteNonQuery();
                    }
                tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                return ex;
            }
        }

        internal void SetCheckAll(bool? isChecked)
        {
            foreach (var p in PeriodSource)
                p["FL"] = isChecked;
        }
    }
}
