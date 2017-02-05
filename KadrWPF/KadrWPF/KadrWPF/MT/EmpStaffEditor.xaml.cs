using EntityGenerator;
using KadrWPF;
using LibraryKadr;
using LibrarySalary.Helpers;
using Oracle.DataAccess.Client;
using Salary;
using Salary.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ManningTable
{
    /// <summary>
    /// Interaction logic for EmpStaffEditor.xaml
    /// </summary>
    public partial class EmpStaffEditor : Window
    {
        public EmpStaffEditor(decimal staffID)
        {
            Model = new EmpStaffsViewModel(staffID);
            InitializeComponent();
            DataContext = Model;
        }

        public EmpStaffsViewModel Model
        {
            get;set;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model.HasChanges;
        }

        private void Save_CanExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Exception ex;
            if ((ex = Model.Save()) != null)
                MessageBox.Show(ex.GetFormattedException(), "Ошибка сохранения данных");
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ChooseEmp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model.CurrentEmpStaff!=null;
        }

        private void ChooseEmp_CanExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EmpFinder f = new EmpFinder();
            f.EmpFilter.SubdivID = Model.Staff.SubdivID;
            f.Owner = Window.GetWindow(this);
            if (f.ShowDialog() == true)
            {
                Model.CurrentEmpStaff.TransferID = f.TransferID;
            }
        }

        private void Add_CanExcute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command);
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.AddNewEmpStaff();
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ControlRoles.GetState(e.Command) && Model.CurrentEmpStaff != null;
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Model.DeleteCurrentEmpStaff();
        }
    }

    public partial class EmpStaffsViewModel : NotificationObject
    {
        OracleDataAdapter odaEmp_Staff;
        DataSet ds;
        public EmpStaffsViewModel(decimal? staff)
        {
            ds = new DataSet();

            odaEmp_Staff = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectEmpStaffData.sql"), Connect.CurConnect);
            odaEmp_Staff.SelectCommand.BindByName = true;
            odaEmp_Staff.SelectCommand.Parameters.Add("p_staff_id", OracleDbType.Decimal, staff, ParameterDirection.Input);
            odaEmp_Staff.SelectCommand.Parameters.Add("r1", OracleDbType.RefCursor, ParameterDirection.Output);
            odaEmp_Staff.SelectCommand.Parameters.Add("r2", OracleDbType.RefCursor, ParameterDirection.Output);
            odaEmp_Staff.SelectCommand.Parameters.Add("r3", OracleDbType.RefCursor, ParameterDirection.Output);
            odaEmp_Staff.TableMappings.Add("Table", "EMP_STAFF");
            odaEmp_Staff.TableMappings.Add("Table1", "EMP_TRANSFER_DATA");
            odaEmp_Staff.TableMappings.Add("Table2", "STAFF");

            odaEmp_Staff.InsertCommand = new OracleCommand(string.Format(@"BEGIN {0}.EMP_STAFF_UPDATE(p_EMP_STAFF_ID=>:p_EMP_STAFF_ID,p_TRANSFER_ID=>:p_TRANSFER_ID,p_STAFF_ID=>:p_STAFF_ID,p_WORK_CF=>:p_WORK_CF,p_DATE_START_WORK=>:p_DATE_START_WORK,p_DATE_END_WORK=>:p_DATE_END_WORK,p_EMP_STAFF_GROUP_ID=>:p_EMP_STAFF_GROUP_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaEmp_Staff.InsertCommand.BindByName = true;
            odaEmp_Staff.InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_EMP_STAFF_ID", OracleDbType.Decimal, 0, "EMP_STAFF_ID").Direction = ParameterDirection.InputOutput;
            odaEmp_Staff.InsertCommand.Parameters["p_EMP_STAFF_ID"].DbType = DbType.Decimal;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID").Direction = ParameterDirection.Input;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_WORK_CF", OracleDbType.Decimal, 0, "WORK_CF").Direction = ParameterDirection.Input;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_DATE_START_WORK", OracleDbType.Date, 0, "DATE_START_WORK").Direction = ParameterDirection.Input;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_DATE_END_WORK", OracleDbType.Date, 0, "DATE_END_WORK").Direction = ParameterDirection.Input;
            odaEmp_Staff.InsertCommand.Parameters.Add("p_EMP_STAFF_GROUP_ID", OracleDbType.Decimal, 0, "EMP_STAFF_GROUP_ID").Direction = ParameterDirection.Input;

            odaEmp_Staff.UpdateCommand = new OracleCommand(string.Format(@"BEGIN {0}.EMP_STAFF_UPDATE(p_EMP_STAFF_ID=>:p_EMP_STAFF_ID,p_TRANSFER_ID=>:p_TRANSFER_ID,p_STAFF_ID=>:p_STAFF_ID,p_WORK_CF=>:p_WORK_CF,p_DATE_START_WORK=>:p_DATE_START_WORK,p_DATE_END_WORK=>:p_DATE_END_WORK,p_EMP_STAFF_GROUP_ID=>:p_EMP_STAFF_GROUP_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaEmp_Staff.UpdateCommand.BindByName = true;
            odaEmp_Staff.UpdateCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_EMP_STAFF_ID", OracleDbType.Decimal, 0, "EMP_STAFF_ID").Direction = ParameterDirection.InputOutput;
            odaEmp_Staff.UpdateCommand.Parameters["p_EMP_STAFF_ID"].DbType = DbType.Decimal;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID").Direction = ParameterDirection.Input;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_STAFF_ID", OracleDbType.Decimal, 0, "STAFF_ID").Direction = ParameterDirection.Input;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_WORK_CF", OracleDbType.Decimal, 0, "WORK_CF").Direction = ParameterDirection.Input;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_DATE_START_WORK", OracleDbType.Date, 0, "DATE_START_WORK").Direction = ParameterDirection.Input;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_DATE_END_WORK", OracleDbType.Date, 0, "DATE_END_WORK").Direction = ParameterDirection.Input;
            odaEmp_Staff.UpdateCommand.Parameters.Add("p_EMP_STAFF_GROUP_ID", OracleDbType.Decimal, 0, "EMP_STAFF_GROUP_ID").Direction = ParameterDirection.Input;

            odaEmp_Staff.DeleteCommand = new OracleCommand(string.Format(@"BEGIN {0}.EMP_STAFF_DELETE(:p_EMP_STAFF_ID);end;", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect);
            odaEmp_Staff.DeleteCommand.BindByName = true;
            odaEmp_Staff.DeleteCommand.Parameters.Add("p_EMP_STAFF_ID", OracleDbType.Decimal, 0, "EMP_STAFF_ID").Direction = ParameterDirection.InputOutput;
            odaEmp_Staff.AcceptChangesDuringUpdate = false;

            odaEmp_Staff.Fill(ds);
        }

        EntityGenerator.Staff _staff;

        /// <summary>
        /// Штатная единицы на которые привязана расстановка
        /// </summary>
        public EntityGenerator.Staff Staff
        {
            get
            {
                if (ds.Tables["STAFF"].Rows.Count == 0)
                    throw new Exception("Упс... Данные штатной единицы не найдены. Возможно она была удалена другим пользователем. Обновите данные на экране");
                if (_staff == null)
                    _staff = new EntityGenerator.Staff() { DataRow = ds.Tables["STAFF"].Rows[0] };
                return _staff;
            }
        }

        EntityRelationList<EmpStaffModel> _empStaffSource;

        /// <summary>
        /// Источник данных список штатная расстановка
        /// </summary>
        public EntityRelationList<EmpStaffModel> EmpStaffSource
        {
            get
            {
                if (_empStaffSource == null)
                {
                    _empStaffSource = new EntityRelationList<EmpStaffModel>(ds.Tables["EMP_STAFF"].ConvertToEntityList<EmpStaffModel>());
                    _empStaffSource.RelatedEntity = Staff;
                }
                return _empStaffSource;
            }
        }

        /// <summary>
        /// Иерархическая структура единиц
        /// </summary>
        public List<EmpStaffModel> EmpStaffHierarhicalSource
        {
            get
            {
                var p = EmpStaffSource.Where(r => r.EmpStaffRelID == null).OrderBy(r=>r.DateStartWork).ToList();
                return p;
            }
        }

        EmpStaffModel _currentEmpStaff;
        /// <summary>
        /// Текущий выбранный сотрудник
        /// </summary>
        public EmpStaffModel CurrentEmpStaff
        {
            get
            {
                return _currentEmpStaff;
            }
            set
            {
                _currentEmpStaff = value;
                RaisePropertyChanged(() => CurrentEmpStaff);
            }
        }

        public bool HasChanges
        {
            get
            {
                return ds.HasChanges();
            }
        }

        /// <summary>
        /// Сохранение данных по таблице с использованием транзакции
        /// </summary>
        /// <param name="currentTransaction">Текущая транзакция или не использовать ее</param>
        /// <returns></returns>
        public Exception Save(OracleTransaction currentTransaction=null)
        {
            OracleTransaction tr = currentTransaction ?? Connect.CurConnect.BeginTransaction();
            try
            {
                foreach (var item in EmpStaffSource)
                    if (item.EntityState == DataRowState.Added && item.EmpStaffID != null)
                    {
                        item.EmpStaffID = null;
                        item.StaffID = Staff.StaffID;
                    }
                odaEmp_Staff.Update(ds.Tables["EMP_STAFF"]);
                if (currentTransaction!=null) tr.Commit();
                return null;
            }
            catch (Exception ex)
            {
                if (currentTransaction!=null) tr.Rollback();
                return ex;
            }
        }

        /// <summary>
        /// Добавление новозой записи
        /// </summary>
        public void AddNewEmpStaff(EmpStaffModel parentModel = null)
        {
            _empStaffSource.Add(new EmpStaffModel() { DataRow = ds.Tables["EMP_STAFF"].Rows.Add(),
                DateStartWork = DateTime.Today,
                EmpStaffID = Math.Min(_empStaffSource.Min(r => r.EmpStaffID) ?? 0, -1m),
                StaffID = Staff.StaffID,
                WorkCf = 1,
                EmpStaffRelID = parentModel?.EmpStaffID
            }
            );
            RaisePropertyChanged(() => EmpStaffHierarhicalSource);
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        public void DeleteCurrentEmpStaff()
        {
            if (CurrentEmpStaff != null)
            {
                var p = CurrentEmpStaff;
                _empStaffSource.Remove(p);
                p.DataRow.Delete();
                RaisePropertyChanged(() => EmpStaffHierarhicalSource);
            }
        }
    }

    /// <summary>
    /// Модель представления данных штатной расстановки сотрудника
    /// </summary>
    public partial class EmpStaffModel : EmpStaff
    {
        EmpTransferData _empData;
        OracleDataAdapter odaEmpTransferStaff;

        public EmpStaffModel():base()
        {
            odaEmpTransferStaff = new OracleDataAdapter(Queries.GetQueryWithSchema(@"MT/SelectEmpTransferData.sql"), Connect.CurConnect);
            odaEmpTransferStaff.SelectCommand.BindByName = true;
            odaEmpTransferStaff.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, null, ParameterDirection.Input);
            odaEmpTransferStaff.SelectCommand.Parameters.Add("r1", OracleDbType.RefCursor, ParameterDirection.Output);
        }

        /// <summary>
        /// Перевод сотрудника
        /// </summary>
        public new decimal? TransferID
        {
            get
            {
                return base.TransferID;
            }
            set
            {
                if (base.TransferID != value)
                {
                    base.TransferID = value;
                    UpdateEmpsData(value);
                    _empData = null;
                    RaisePropertyChanged(() => EmpTransferData);
                }
            }
        }

        /// <summary>
        /// Данные сотрудника общие
        /// </summary>
        public EmpTransferData EmpTransferData
        {
            get
            {
                if (_empData == null)
                {
                    _empData = this.GetParentEntity<EmpTransferData>("TRANSFER_ID");
                }
                return _empData;
            }
        }

        /// <summary>
        /// Обновление данных в таблице при измееннении перевода
        /// </summary>
        /// <param name="newTransferID"></param>
        private void UpdateEmpsData(decimal? newTransferID)
        {
            if (DataSet.Tables.Contains("EMP_TRANSFER_DATA"))
            {
                odaEmpTransferStaff.SelectCommand.Parameters["p_transfer_id"].Value = newTransferID;
                DataTable dt = new DataTable();
                odaEmpTransferStaff.Fill(dt);
                DataSet.Tables["EMP_TRANSFER_DATA"].Merge(dt, "TRANSFER_ID");
                DataSet.Tables["EMP_TRANSFER_DATA"].AcceptChanges();
            }
            else
                throw new Exception("Для обновления данных сотрудников требуется представление таблица EMP_TRANSFER_DATA в DATA_SET");
        }

        /// <summary>
        /// Дочерние элементы текущего расположения
        /// </summary>
        public List<EmpStaffModel> ChildEmpStaffs
        {
            get
            {
                var p = this.DataSet.Tables["EMP_STAFF"].ConvertToEntityList<EmpStaffModel>().Where(r => r.EmpStaffRelID == this.EmpStaffID && r.EmpStaffID!=this.EmpStaffID).ToList();
                return p;
            }
        }
    }
}
