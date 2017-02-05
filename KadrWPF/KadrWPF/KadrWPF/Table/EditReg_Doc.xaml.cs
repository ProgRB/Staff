using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using System.Data;
using LibraryKadr;
using Oracle.DataAccess.Client;
using System.Globalization;
using System.ComponentModel;
using Salary;
using EntityGenerator;

namespace WpfControlLibrary.Table
{
    /// <summary>
    /// Interaction logic for EditReg_Doc.xaml
    /// </summary>
    public partial class EditRegDoc : Window
    {
        private RegDocModel _model;
        public EditRegDoc(decimal _transfer_id, decimal? _reg_doc_id, bool sign_waybill)
        {
            _model = new RegDocModel(_reg_doc_id, _transfer_id, sign_waybill);
            InitializeComponent();
            DataContext = Model;
            dtTimeBegin.TextChanged += MaskedTextBox_TextChanged;
            dtTimeEnd.TextChanged += MaskedTextBox_TextChanged_1;
            this.Loaded += new RoutedEventHandler(EditRegDoc_Loaded);
        }

        void EditRegDoc_Loaded(object sender, RoutedEventArgs e)
        {
            Model.IsActive = true;
        }

        /// <summary>
        /// Модель данных
        /// </summary>
        public RegDocModel Model
        {
            get
            {
                return _model;
            }
        }

        /// <summary>
        /// Проверка на возможность выполенения команды
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Model != null && string.IsNullOrEmpty(Model.Error);
        }

        /// <summary>
        /// Сохранение данных по документу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exception ex = Model.Save();
            if (ex == null)
            {
                this.DialogResult = true;
                Close();
            }
            else
                MessageBox.Show(ex.GetFormattedException(), "АРМ \"Учет рабочего времени\":Ошибка сохранения");
        }

        private void DateBegin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Model.DocBegin != null && Math.Abs(Model.DocBegin.Value.Subtract(DateTime.Now).Days) > 366)
            {
                if (MessageBox.Show(this, "Значение введенной даты начала документа отличается на год от текущей даты!" +
                        "\nВы хотите продолжить работу?",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                {
                    e.Handled = true;
                }
            }
        }

        private void DateEnd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Model.DocEnd != null && Math.Abs(Model.DocEnd.Value.Subtract(DateTime.Now).Days) > 366)
            {
                if (MessageBox.Show(this, "Значение введенной даты окончания документа отличается на год от текущей даты!" +
                        "\nВы хотите продолжить работу?",
                        "АРМ \"Учет рабочего времени\"", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                {
                    e.Handled = true;
                }
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void MaskedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Model.IsActive)
            {
                string s = (string)(sender as Xceed.Wpf.Toolkit.MaskedTextBox).Text.TrimEnd('_');
                if ((s.LastOrDefault() == '0' || s != Model.TimeDocEnd) && s.Length > 5)
                    // это короче жучок ебаный и при вводе цифры 0 не срабаывает обновление значение , поэтому приходится вручную обновлять
                    Model.TimeDocBegin = s;
            }
        }

        private void MaskedTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (Model.IsActive)
            {
                string s = (string)(sender as Xceed.Wpf.Toolkit.MaskedTextBox).Text.TrimEnd('_');
                if ((s.LastOrDefault() == '0' || s != Model.TimeDocEnd) && s.Length>5)
                    // это короче жучок ебаный и при вводе цифры 0 не срабаывает обновление значение , поэтому приходится вручную обновлять
                    Model.TimeDocEnd = s;
            }
        }
    }

    /// <summary>
    /// Модель представления документа опрадательного
    /// </summary>
    public class RegDocModel : RegDoc, IDataErrorInfo
    {
        DataSet ds;
        /// <summary>
        /// Адаптер выбирающий даты входа-выхода за день
        /// </summary>
        OracleDataTable dtSelTimeGraph;
        /// <summary>
        /// Адаптер выбора последнего выхода за день
        /// </summary>
        OracleDataTable dtSelPassEvent;
        private bool _signWaybill;

        public RegDocModel(decimal? _regDocID, decimal? transfer_id, bool signWaybill)
        {
            this.AdapterConnection = Connect.CurConnect;
            DataAdapter = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table\SelectRegDocData.sql"), Connect.Schema, Connect.SchemaSalary), AdapterConnection);
            DataAdapter.SelectCommand.BindByName = true;
            DataAdapter.SelectCommand.Parameters.Add("p_reg_doc_id", OracleDbType.Decimal, _regDocID, ParameterDirection.Input);
            DataAdapter.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, transfer_id, ParameterDirection.Input);
            DataAdapter.SelectCommand.Parameters.Add("c1", OracleDbType.RefCursor, ParameterDirection.Output);
            DataAdapter.SelectCommand.Parameters.Add("c2", OracleDbType.RefCursor, ParameterDirection.Output);
            DataAdapter.SelectCommand.Parameters.Add("c3", OracleDbType.RefCursor, ParameterDirection.Output);

            DataAdapter.TableMappings.Add("Table", "REG_DOC");
            DataAdapter.TableMappings.Add("Table1", "DOC_LIST");
            DataAdapter.TableMappings.Add("Table2", "TRANSFER");
            ds = new DataSet();
            DataAdapter.Fill(ds);
            ///если айдишник не задан, значит это добавление нового документа
            if (_regDocID == null)
            {
                DataRow r = ds.Tables["REG_DOC"].NewRow();
                r["TRANSFER_ID"] = transfer_id;
                r["PER_NUM"] = ds.Tables["TRANSFER"].Rows[0].Field2<string>("PER_NUM");
                r["DOC_DATE"] = DateTime.Now;
                ds.Tables["REG_DOC"].Rows.Add(r);
            }
            this.DataRow = ds.Tables["REG_DOC"].Rows[0];
            SignWaybill = signWaybill;
            OnSetDocBegin(DocBegin);
            OnSetDocEnd(DocEnd);


            #region Создание адаптеров для данных

            DataAdapter.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.REG_DOC_UPDATE(p_REG_DOC_ID=>:p_REG_DOC_ID,p_PER_NUM=>:p_PER_NUM,p_DOC_LIST_ID=>:p_DOC_LIST_ID,
                            p_DOC_DATE=>:p_DOC_DATE,p_DOC_BEGIN=>:p_DOC_BEGIN,p_DOC_END=>:p_DOC_END,p_ABSENCE_ID=>:p_ABSENCE_ID,p_TRANSFER_ID=>:p_TRANSFER_ID,
                            p_DOC_LOCATION=>:p_DOC_LOCATION,p_DOC_NUMBER=>:p_DOC_NUMBER);end;", Connect.Schema), AdapterConnection);
            DataAdapter.InsertCommand.BindByName = true;
            DataAdapter.InsertCommand.Parameters.Add("p_REG_DOC_ID", OracleDbType.Decimal, 0, "REG_DOC_ID").Direction = ParameterDirection.InputOutput;
            DataAdapter.InsertCommand.Parameters["p_REG_DOC_ID"].DbType = DbType.Decimal;
            DataAdapter.InsertCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_DOC_LIST_ID", OracleDbType.Decimal, 0, "DOC_LIST_ID").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_DOC_DATE", OracleDbType.Date, 0, "DOC_DATE").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_DOC_BEGIN", OracleDbType.Date, 0, "DOC_BEGIN").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_DOC_END", OracleDbType.Date, 0, "DOC_END").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_ABSENCE_ID", OracleDbType.Decimal, 0, "ABSENCE_ID").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_DOC_LOCATION", OracleDbType.Varchar2, 0, "DOC_LOCATION").Direction = ParameterDirection.Input;
            DataAdapter.InsertCommand.Parameters.Add("p_DOC_NUMBER", OracleDbType.Varchar2, 0, "DOC_NUMBER").Direction = ParameterDirection.Input; 
            
            DataAdapter.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.REG_DOC_UPDATE(p_REG_DOC_ID=>:p_REG_DOC_ID,p_PER_NUM=>:p_PER_NUM,p_DOC_LIST_ID=>:p_DOC_LIST_ID,
                            p_DOC_DATE=>:p_DOC_DATE,p_DOC_BEGIN=>:p_DOC_BEGIN,p_DOC_END=>:p_DOC_END,p_ABSENCE_ID=>:p_ABSENCE_ID,p_TRANSFER_ID=>:p_TRANSFER_ID,
                            p_DOC_LOCATION=>:p_DOC_LOCATION, p_DOC_NUMBER=>:p_DOC_NUMBER);end;", Connect.Schema), AdapterConnection);
            DataAdapter.UpdateCommand.BindByName = true;
            DataAdapter.UpdateCommand.Parameters.Add("p_REG_DOC_ID", OracleDbType.Decimal, 0, "REG_DOC_ID").Direction = ParameterDirection.InputOutput;
            DataAdapter.UpdateCommand.Parameters["p_REG_DOC_ID"].DbType = DbType.Decimal;
            DataAdapter.UpdateCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2, 0, "PER_NUM").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_DOC_LIST_ID", OracleDbType.Decimal, 0, "DOC_LIST_ID").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_DOC_DATE", OracleDbType.Date, 0, "DOC_DATE").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_DOC_BEGIN", OracleDbType.Date, 0, "DOC_BEGIN").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_DOC_END", OracleDbType.Date, 0, "DOC_END").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_ABSENCE_ID", OracleDbType.Decimal, 0, "ABSENCE_ID").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_DOC_LOCATION", OracleDbType.Varchar2, 0, "DOC_LOCATION").Direction = ParameterDirection.Input;
            DataAdapter.UpdateCommand.Parameters.Add("p_DOC_NUMBER", OracleDbType.Varchar2, 0, "DOC_NUMBER").Direction = ParameterDirection.Input;

            DataAdapter.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.REG_DOC_DELETE(p_REG_DOC_ID=>:p_REG_DOC_ID);end;", Connect.Schema), AdapterConnection);
            DataAdapter.DeleteCommand.BindByName = true;
            DataAdapter.DeleteCommand.Parameters.Add("p_REG_DOC_ID", OracleDbType.Decimal, 0, "REG_DOC_ID").Direction = ParameterDirection.InputOutput;


            dtSelTimeGraph = new OracleDataTable(string.Format(Queries.GetQuery("Table/SelectTimeBeginReg_Doc.sql"), Connect.Schema), Connect.CurConnect);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_date", OracleDbType.Date);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_doc_begin", OracleDbType.Date, DateTime.Today, ParameterDirection.Input);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_doc_end", OracleDbType.Date, DateTime.Today, ParameterDirection.Input);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2, PerNum, ParameterDirection.Input);
            dtSelTimeGraph.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, TransferID, ParameterDirection.Input);

            dtSelPassEvent = new OracleDataTable("", Connect.CurConnect);
            if (!SignWaybill)
            {
                dtSelPassEvent.SelectCommand.CommandText = string.Format(
                    @"select max(event_time) MAX_EVENT, min(event_time) MIN_EVENT,
                        MAX(CASE WHEN WHERE_FROM = 2 THEN EVENT_TIME END) MAX_EXIT
                    from {0}.EMP_PASS_EVENT EP where EP.per_num = :p_per_num and 
                        trunc(EP.event_time) = trunc(:p_date) ",
                    "perco");
            }
            else
            {
                dtSelPassEvent.SelectCommand.CommandText = string.Format(
                    @"select to_char(max(END_WORK),'HH24:MI') MAX_EVENT_TIME,  
                        to_char(max(BEGIN_WORK),'HH24:MI') MIN_EVENT_TIME, max(END_WORK) MAX_EVENT,
                        MAX(END_WORK) MAX_EXIT
                    from {0}.V_PUTL_NEW where PER_NUM = :p_per_num and :p_date = trunc(END_WORK)",
                    Connect.Schema);
            }
            dtSelPassEvent.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dtSelPassEvent.SelectCommand.Parameters.Add("p_date", OracleDbType.Date);

            #endregion
        }

        /// <summary>
        /// Источник данных список документов доступных
        /// </summary>
        public IEnumerable<DocList> DocListSource
        {
            get
            {
                return ds.Tables["DOC_LIST"].Rows.OfType<DataRow>().Select(r => new DocList() { DataRow = r }).OrderBy(r => r.DocName);
            }
        }

        /// <summary>
        /// Процедура автоматической уставновки начала-окончания по документу
        /// </summary>
        public void AutoSetBeginDocumPeriod(DateTime proposedDate)
        {
            if (DocList == null)
                return;
            if (DocList.PayTypeID == 106 || DocList.PayTypeID == 306)
            {
                dtSelTimeGraph.Clear();
                dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = proposedDate.Date;
                dtSelTimeGraph.Fill(); //выбираем даты выходов за день начала документа
                if (dtSelTimeGraph.Rows.Count > 0)
                {
                    DocBegin = dtSelTimeGraph.Rows[0].Field2<DateTime?>("LAST_TIME_END");
                }

                dtSelPassEvent.Clear();
                dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = PerNum;
                dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = proposedDate.Date;
                dtSelPassEvent.Fill();
                if (dtSelPassEvent.Rows.Count > 0 && DocEnd==null)
                {
                    DocEnd = dtSelPassEvent.Rows[0].Field2<DateTime?>("MAX_EVENT");
                }
            }
            else
                if (DocList.PayTypeID == 124 || DocList.PayTypeID == 121 || DocList.PayTypeID == 304)
                {
                    dtSelPassEvent.Clear();
                    dtSelPassEvent.SelectCommand.Parameters["p_per_num"].Value = PerNum;
                    dtSelPassEvent.SelectCommand.Parameters["p_date"].Value = proposedDate.Date;
                    dtSelPassEvent.Fill();
                    if (dtSelPassEvent.Rows.Count > 0)
                    {
                        if (DocBegin==null)
                            DocBegin = dtSelPassEvent.Rows[0].Field2<DateTime?>("MIN_EVENT");
                        if (DocEnd == null)
                            DocEnd = dtSelPassEvent.Rows[0].Field2<DateTime?>("MAX_EVENT");
                    }
                }
                else
                    if (DocList.SignAllDay == 1)
                    {
                        dtSelTimeGraph.Clear();
                        dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = proposedDate.Date;
                        dtSelTimeGraph.Fill();
                        if (dtSelTimeGraph.Rows.Count > 0 && DocBegin == null)
                        {
                            DocBegin = dtSelTimeGraph.Rows[0].Field2<DateTime?>("FIRST_TIME_BEGIN");
                        }
                    }
            //if (DocBegin == null) // если же дата не поставилась, ставим тут дату которую предложили проверить
            //    DocBegin = proposedDate;
            RaisePropertyChanged(() => TimeDocBegin);
            RaisePropertyChanged(() => DateDocBegin);
            RaisePropertyChanged(() => TimeDocEnd);
            RaisePropertyChanged(() => DateDocEnd);
        }

        /// <summary>
        /// Автоматическое определение окончания документа
        /// </summary>
        public void AutoSetEndDocumPeriod(DateTime proposedDate)
        {
            if (DocList == null)
                return;
            if (DocEnd == null && DocList.SignAllDay == 1)
            {
                dtSelTimeGraph.Clear();
                dtSelTimeGraph.SelectCommand.Parameters["p_date"].Value = proposedDate;
                dtSelTimeGraph.Fill();
                if (dtSelTimeGraph.Rows.Count > 0)
                {
                    //DocEnd = dtSelTimeGraph.Rows[0].Field2<DateTime?>("MAX_TIME_END");
                    DocEnd = dtSelTimeGraph.Rows[0].Field2<DateTime?>("LAST_TIME_END");
                }
            }
            //if (DocEnd == null)
            //    DocEnd = proposedDate;
            RaisePropertyChanged(() => TimeDocEnd);
            RaisePropertyChanged(() => DateDocEnd);
        }

        #region IDataError implementation
        /// <summary>
        /// Ошибка по конкретному полю модели
        /// </summary>
        /// <param name="columName"></param>
        /// <returns></returns>
        public new string this[string columName]
        {
            get
            {
                string s = base[columName];
                if (!string.IsNullOrEmpty(s))
                    return s;
                if ((columName == "DocBegin" || columName == "DocEnd") && DocBegin > DocEnd)
                    return "Дата начала документа должна быть меньше даты окончания документа";
                DateTime t;
                TimeSpan tm;
                if (columName == "DateDocBegin")
                {
                    if (string.IsNullOrWhiteSpace(_dateDocBegin))
                        return "Не введена дата начала документа";
                    if (!DateTime.TryParseExact(_dateDocBegin, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out t))
                        return "Не верно введена дата начала документа";
                }
                if (columName == "TimeDocBegin")
                {
                    if (string.IsNullOrWhiteSpace(_timeDocBegin))
                        return "Не введено время начала документа";
                    if (!TimeSpan.TryParseExact(_timeDocBegin, @"hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out tm))
                        return "Не верно введено время начала документа";
                }

                if (columName == "DateDocEnd")
                {
                    if (string.IsNullOrWhiteSpace(_dateDocEnd))
                        return "Не введена дата окончания документа";
                    if (!DateTime.TryParseExact(_dateDocEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out t))
                        return "Не верно введена дата окончания документа";
                }
                if (columName == "TimeDocEnd")
                {
                    if (string.IsNullOrWhiteSpace(_timeDocEnd))
                        return "Не введено время окончания документа";
                    if (!TimeSpan.TryParseExact(_timeDocEnd, @"hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out tm))
                        return "Не верно введено время окончания документа";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Ошибка по всей модели
        /// </summary>
        public new string Error
        {
            get
            {
                if (!string.IsNullOrEmpty(base.Error))
                    return base.Error;
                if (DocBegin > DocEnd)
                    return "Дата начала документа должна быть меньше даты окончания документа";

                if (string.IsNullOrWhiteSpace(_dateDocBegin))
                    return "Не введена дата начала документа";
                DateTime t;
                TimeSpan tm;
                if (!DateTime.TryParseExact(_dateDocBegin, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out t))
                    return "Не верно введена дата начала документа";
                if (string.IsNullOrWhiteSpace(_timeDocBegin))
                    return "Не введено время начала документа";
                if (!TimeSpan.TryParseExact(_timeDocBegin, @"hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out tm))
                    return "Не верно введено время начала документа";

                if (string.IsNullOrWhiteSpace(_dateDocEnd))
                    return "Не введена дата окончания документа";
                if (!DateTime.TryParseExact(_dateDocEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out t))
                    return "Не верно введена дата окончания документа";
                if (string.IsNullOrWhiteSpace(_timeDocEnd))
                    return "Не введено время окончания документа";
                if (!TimeSpan.TryParseExact(_timeDocEnd, @"hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out tm))
                    return "Не верно введено время окончания документа";
                if (DocList!=null && (DocBegin>DocList.DocEndValid || DocEnd<DocList.DocBeginValid))
                    return "Документ не попадает в период действия типа документа!";
                return string.Empty;
            }
        }

        #endregion

        #region Дополнительные поля для класса
        string _dateDocBegin, _timeDocBegin, _dateDocEnd, _timeDocEnd;

        public new DateTime? DocBegin
        {
            get
            {
                return base.DocBegin;
            }
            set
            {
                base.DocBegin = value;
                OnSetDocBegin(value);
            }
        }

        public new DateTime? DocEnd
        {
            get
            {
                return base.DocEnd;
            }
            set
            {
                base.DocEnd = value;
                OnSetDocEnd(value);
            }
        }


        /// <summary>
        /// Чисто дата начала документа (без времени)
        /// </summary>
        public string DateDocBegin
        { 
            get
            {
                return this.DocBegin.HasValue? this.DocBegin.Value.Date.ToString("dd-MM-yyyy"): string.Empty;
            }
            set
            {

                if (string.IsNullOrWhiteSpace(value)) // если никакая дата не была введена, то это получается пустая дата
                {
                    DocBegin = null;
                }
                else
                {
                    DateTime d;
                    DateTime? dvalue = null;
                    if (DateTime.TryParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                        dvalue = d;
                    if (dvalue != null) // если  же это все таки дата нормальная введена, то...
                    {
                        if (this.DocBegin == null)
                        {
                            AutoSetBeginDocumPeriod(dvalue.Value.Date); // тогда если дата ее не введена, то ставим и начало и окончание автоматически
                        }
                        else
                        {
                            DocBegin = dvalue.Value.Date + DocBegin.Value.TimeOfDay; // устанавливаем только дату, время оставляем тоже самое
                        }
                    }
                }
                _dateDocBegin = value;
                RaisePropertyChanged(() => Error);
                RaisePropertyChanged(() => DateDocBegin);
            }
        }

        /// <summary>
        /// Чисто время начала документа
        /// </summary>
        public string TimeDocBegin
        {
            get
            {
                return _timeDocBegin;
            }
            set
            {
                string realValue = value == "00:00:" || value == "00:00:0" ? "00:00:00" :
                    value!=null && value.Length>5? value.PadRight(8, '0') : value; //эту хуйню пришлось поставить, потому что значение 00:00:00 не вызывает ValueChanged MaskedTextBox  я не знаю почему, остается значение 00:00:0

                if (string.IsNullOrWhiteSpace(realValue)) // если никакая дата не была введена, то это получается пустая дата
                {
                    if (DocBegin != null)
                        DocBegin = DocBegin.Value.Date;
                }
                else
                {
                    TimeSpan d;
                    TimeSpan? dvalue = null;
                    if (TimeSpan.TryParseExact(realValue.PadRight(8, '0'), @"hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out d))
                        dvalue = d;
                    if (dvalue != null) // если  же это все таки время нормальное введено, то...
                    {
                        if (this._dateDocBegin == null)
                        {
                            DocBegin = DateTime.Today + dvalue.Value; // тогда если дата ее не введена, то ставим сегодняшний день
                        }
                        else
                        {
                            DocBegin = Convert.ToDateTime(_dateDocBegin).Date + dvalue.Value; // устанавливаем только дату, время оставляем тоже самое
                        }
                    }
                }
                _timeDocBegin = realValue;
                RaisePropertyChanged(() => Error);
                RaisePropertyChanged(() => TimeDocBegin);
            }
        }

        /// <summary>
        /// Чисто дата окончания документа (без времени)
        /// </summary>
        public string DateDocEnd
        {
            get
            {
                return this.DocEnd.HasValue ? this.DocEnd.Value.Date.ToString("dd-MM-yyyy") : string.Empty;
            }
            set
            {

                if (string.IsNullOrWhiteSpace(value)) // если никакая дата не была введена, то это получается пустая дата
                {
                    DocEnd = null;
                }
                else
                {
                    DateTime d;
                    DateTime? dvalue = null;
                    if (DateTime.TryParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                        dvalue = d;
                    if (dvalue != null) // если  же это все таки дата нормальная введена, то...
                    {
                        if (this.DocEnd == null)
                        {
                            AutoSetEndDocumPeriod(dvalue.Value.Date); // тогда если дата ее не введена, то ставим и начало и окончание автоматически
                        }
                        else
                        {
                            DocEnd = dvalue.Value.Date + DocEnd.Value.TimeOfDay; // устанавливаем только дату, время оставляем тоже самое
                        }
                    }
                }
                _dateDocEnd = value;
                RaisePropertyChanged(() => Error);
                RaisePropertyChanged(() => DateDocEnd);
            }
        }

        /// <summary>
        /// Чисто время окончания документа
        /// </summary>
        public string TimeDocEnd
        {
            get
            {
                return _timeDocEnd;
            }
            set
            {
                string realValue = value == "00:00:" || value == "00:00:0" ? "00:00:00" :
                    value != null && value.Length > 5 ? value.PadRight(8, '0') : value; //эту хуйню пришлось поставить, потому что значение 00:00:00 не вызывает ValueChanged MaskedTextBox  я не знаю почему, остается значение 00:00:0

                if (string.IsNullOrWhiteSpace(realValue)) // если никакая дата не была введена, то это получается пустая дата
                {
                    if (DocEnd != null)
                        DocEnd = DocEnd.Value.Date;
                }
                else
                {
                    TimeSpan d;
                    TimeSpan? dvalue = null;
                    if (TimeSpan.TryParseExact(realValue.PadRight(8, '0'), @"hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out d))
                        dvalue = d;
                    if (dvalue != null) // если  же это все таки время нормальное введено, то...
                    {
                        if (this._dateDocEnd == null)
                        {
                            DocEnd = DateTime.Today + dvalue.Value; // тогда если дата ее не введена, то ставим сегодняшний день
                        }
                        else
                        {
                            DocEnd = Convert.ToDateTime(_dateDocEnd).Date + dvalue.Value; // устанавливаем только дату, время оставляем тоже самое
                        }
                    }
                }
                _timeDocEnd = realValue;
                RaisePropertyChanged(() => Error);
                RaisePropertyChanged(() => TimeDocEnd);
            }
        }

        /// <summary>
        /// Признак учета сотрудника по путевым листам
        /// </summary>
        public bool SignWaybill
        {
            get
            {
                return _signWaybill;
            }
            private set
            {
                _signWaybill = value;
                RaisePropertyChanged(() => SignWaybill);
            }
        }

        /// <summary>
        /// Активна ли в данный момент модель
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }
        #endregion

        private void OnSetDocBegin(DateTime? value)
        {
            if (value != null)
            {
                _dateDocBegin = value.Value.ToString("dd-MM-yyyy");
                _timeDocBegin = value.Value.ToString("HH:mm:ss");
            }
            else
                _dateDocBegin = _timeDocBegin = string.Empty;
        }
        private void OnSetDocEnd(DateTime? value)
        {
            if (value != null)
            {
                _dateDocEnd = value.Value.ToString("dd-MM-yyyy");
                _timeDocEnd = value.Value.ToString("HH:mm:ss");
            }
            else _dateDocEnd = _timeDocEnd = string.Empty;
        }

        public Exception Save()
        {
            if (DocList.PayTypeID == 302 && (DocEnd.Value - DocBegin.Value).TotalMinutes < 30 && MessageBox.Show("Данный документ менее 30 минут!" +
                            "\nВ отгулах будет убрано 30 минут. Продолжить сохранение?",
                            "АРМ \"Учет рабочего времени\"", MessageBoxButton.YesNo , MessageBoxImage.Exclamation)== MessageBoxResult.No)
                return new Exception("Прервано пользователем");
            else return base.Save();
        }
    }

    /// <summary>
    /// Таблица документов в табеле
    /// </summary>
    [Table(Name = "REG_DOC")]
    public partial class RegDoc : RowEntityBase
    {

        #region Class Members

        /// <summary>
        /// Уникальный идентификатор регистрационного документа;
        /// </summary>
        [Column(Name = "REG_DOC_ID")]
        public Decimal? RegDocID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => RegDocID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => RegDocID, value);
            }
        }

        /// <summary>
        /// Местонахождение;
        /// </summary>
        [Column(Name = "DOC_LOCATION")]
        public String DocLocation
        {
            get
            {
                return this.GetDataRowField<String>(() => DocLocation);
            }
            set
            {
                UpdateDataRow<String>(() => DocLocation, value);
            }
        }


        [Column(Name = "TRANSFER_ID", CanBeNull=false)]
        public Decimal? TransferID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => TransferID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => TransferID, value);
            }
        }


        [Column(Name = "ABSENCE_ID")]
        public Decimal? AbsenceID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => AbsenceID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => AbsenceID, value);
            }
        }

        /// <summary>
        /// Дата окончания; Дата окончания действия документа;
        /// </summary>
        [Column(Name = "DOC_END", CanBeNull=false)]
        public DateTime? DocEnd
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DocEnd);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DocEnd, value);
            }
        }

        /// <summary>
        /// Дата начала; Дата начала действия документа;
        /// </summary>
        [Column(Name = "DOC_BEGIN", CanBeNull=false)]
        public DateTime? DocBegin
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DocBegin);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DocBegin, value);
            }
        }

        /// <summary>
        /// Номер документа;
        /// </summary>
        [Column(Name = "DOC_NUMBER")]
        public String DocNumber
        {
            get
            {
                return this.GetDataRowField<String>(() => DocNumber);
            }
            set
            {
                UpdateDataRow<String>(() => DocNumber, value);
            }
        }

        /// <summary>
        /// Дата документа;
        /// </summary>
        [Column(Name = "DOC_DATE", CanBeNull=false)]
        public DateTime? DocDate
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DocDate);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DocDate, value);
            }
        }

        /// <summary>
        /// Уникальный идентификатор документа;
        /// </summary>
        [Column(Name = "DOC_LIST_ID", CanBeNull=false)]
        public Decimal? DocListID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => DocListID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => DocListID, value);
            }
        }

        /// <summary>
        /// Табельный номер;
        /// </summary>
        [Column(Name = "PER_NUM")]
        public String PerNum
        {
            get
            {
                return this.GetDataRowField<String>(() => PerNum);
            }
            set
            {
                UpdateDataRow<String>(() => PerNum, value);
            }
        }
        #endregion

        /// <summary>
        /// Тип документа
        /// </summary>
        public DocList DocList
        {
            get
            {
                if (this.DocListID == null)
                    return null;
                else
                    return this.DataSet.Tables["DOC_LIST"].Rows.OfType<DataRow>().Where(r => r.Field2<Decimal?>("DOC_LIST_ID") == DocListID).Select(r => new DocList() { DataRow = r }).FirstOrDefault();
            }
        }


    }

    /// <summary>
    /// Таблица типов документов табеля
    /// </summary>
    [Table(Name = "DOC_LIST")]
    public partial class DocList : RowEntityBase
    {
        #region Class Members

        /// <summary>
        /// Уникальный идентификатор документа;
        /// </summary>
        [Column(Name = "DOC_LIST_ID")]
        public Decimal? DocListID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => DocListID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => DocListID, value);
            }
        }

        /// <summary>
        /// Дата окончания действия типа документа;
        /// </summary>
        [Column(Name = "DOC_END_VALID")]
        public DateTime? DocEndValid
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DocEndValid);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DocEndValid, value);
            }
        }

        /// <summary>
        /// Дата начала действия типа документа;
        /// </summary>
        [Column(Name = "DOC_BEGIN_VALID")]
        public DateTime? DocBeginValid
        {
            get
            {
                return this.GetDataRowField<DateTime?>(() => DocBeginValid);
            }
            set
            {
                UpdateDataRow<DateTime?>(() => DocBeginValid, value);
            }
        }

        /// <summary>
        /// Признак целого дня; (Если признак = 1, то в этот день не может быть больше видов оплат кроме данного)
        /// </summary>
        [Column(Name = "SIGN_ALL_DAY")]
        public Decimal? SignAllDay
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => SignAllDay);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => SignAllDay, value);
            }
        }

        /// <summary>
        /// Признак добавления дня отгула;
        /// </summary>
        [Column(Name = "ADD_HOLIDAY")]
        public Decimal? AddHoliday
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => AddHoliday);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => AddHoliday, value);
            }
        }

        /// <summary>
        /// Признак расчета;
        /// </summary>
        [Column(Name = "ISCALC")]
        public Decimal? Iscalc
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => Iscalc);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => Iscalc, value);
            }
        }

        /// <summary>
        /// Вид оплат;
        /// </summary>
        [Column(Name = "PAY_TYPE_ID")]
        public Decimal? PayTypeID
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => PayTypeID);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => PayTypeID, value);
            }
        }

        /// <summary>
        /// Тип документов (оправдательный документ или сверхурочный);
        /// </summary>
        [Column(Name = "DOC_TYPE")]
        public Decimal? DocType
        {
            get
            {
                return this.GetDataRowField<Decimal?>(() => DocType);
            }
            set
            {
                UpdateDataRow<Decimal?>(() => DocType, value);
            }
        }

        /// <summary>
        /// Обозначение документа;
        /// </summary>
        [Column(Name = "DOC_NOTE")]
        public String DocNote
        {
            get
            {
                return this.GetDataRowField<String>(() => DocNote);
            }
            set
            {
                UpdateDataRow<String>(() => DocNote, value);
            }
        }

        /// <summary>
        /// Наименование документа;
        /// </summary>
        [Column(Name = "DOC_NAME")]
        public String DocName
        {
            get
            {
                return this.GetDataRowField<String>(() => DocName);
            }
            set
            {
                UpdateDataRow<String>(() => DocName, value);
            }
        }
        #endregion
    }
}
