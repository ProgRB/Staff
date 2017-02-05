using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using System.ComponentModel;
using LibraryKadr;
using Oracle.DataAccess.Types;
using Staff;
using EntityGenerator;

namespace WpfControlLibrary
{    
   /* public static class Subdiv
    {
        private static DataTable _dtSubdiv = new DataTable();

        public static DataTable DTSubdiv
        {
            get { return _dtSubdiv; }
        }

        static Subdiv()
        {
            GetSubdiv();
            _dtSubdiv.PrimaryKey = new DataColumn[] {_dtSubdiv.Columns["CODE_SUBDIV"]};
        }

        public static void GetSubdiv()
        {
            _dtSubdiv.Clear();
            OracleDataAdapter daSubdiv = new OracleDataAdapter(
            string.Format("SELECT * FROM {0}.SUBDIV_ROLES WHERE CODE_SUBDIV < '700' and " +
                "upper(app_name)=upper(:p_app_name) and PARENT_ID = 0 ORDER BY SUBDIV_NAME",
                Connect.Schema), Connect.CurConnect);
            daSubdiv.SelectCommand.Parameters.Add("p_app_name", OracleDbType.Varchar2).Value = "PREMIUM";
            daSubdiv.Fill(_dtSubdiv);
        }
    }   */    
    
    /*public static class Degree
    { 
        private static DataTable _dtDegree = new DataTable();

        public static DataTable DTDegree
        {
            get { return _dtDegree; }
        }

        static Degree()
        {
            new OracleDataAdapter(string.Format(
                "select DEGREE_ID, CODE_DEGREE, DEGREE_NAME from {0}.DEGREE order by CODE_DEGREE",
                Connect.SchemaApstaff), Connect.CurConnect).Fill(_dtDegree);
            _dtDegree.Columns.Add("DISP_DEGREE").Expression = "CODE_DEGREE+' ('+DEGREE_NAME+')'";            
        }
    }*/

    public static class Tariff_Grid_Salary
    {
        private static DataTable _dtTariff_Grid_Salary = new DataTable();

        public static DataTable DTTariff_Grid_Salary
        {
            get { return _dtTariff_Grid_Salary; }
        }

        private static OracleDataAdapter _daTariff_Grid_Salary = new OracleDataAdapter();

        static Tariff_Grid_Salary()
        {
            _daTariff_Grid_Salary.SelectCommand = new OracleCommand(
                string.Format(Queries.GetQuery("SelectTariff_Grid_Salary_For_Date.sql"),
                Connect.Schema), Connect.CurConnect);
            _daTariff_Grid_Salary.SelectCommand.BindByName = true;
            //_daTariff_Grid_Salary.SelectCommand.Parameters.Add("p_date_transfer", OracleDbType.Date);
            UpdateSet(DateTime.Today);
        }

        public static void UpdateSet(DateTime dateTariff)
        {
            //_daTariff_Grid_Salary.SelectCommand.Parameters["p_date_transfer"].Value = dateTariff;
            _dtTariff_Grid_Salary.Clear();
            _daTariff_Grid_Salary.Fill(_dtTariff_Grid_Salary);
        }

        static List<EntityGenerator.TariffGridSalary> _listTariffSalary;
        public static List<EntityGenerator.TariffGridSalary> TariffGridSalaryList
        {
            get
            {
                if (_listTariffSalary == null)
                    _listTariffSalary = _dtTariff_Grid_Salary.ConvertToEntityList<TariffGridSalary>();
                return _listTariffSalary;
            }
        }
    }

    public static class Base_Tariff
    {
        private static DataTable _dtBase_Tariff = new DataTable();

        public static DataTable DTBase_Tariff
        {
            get { return _dtBase_Tariff; }
        }

        private static OracleDataAdapter _daBase_Tariff = new OracleDataAdapter();

        static Base_Tariff()
        {
            _daBase_Tariff.SelectCommand = new OracleCommand(string.Format(
                @"select B.BASE_TARIFF_ID, B.BDATE, LEAD(B.BDATE-1/86400,1,DATE '3000-01-01') OVER(ORDER BY B.BDATE) EDATE, B.TARIFF
                FROM {0}.BASE_TARIFF B ", Connect.Schema), Connect.CurConnect);
            _daBase_Tariff.SelectCommand.BindByName = true;
            UpdateSet(DateTime.Today);
        }

        public static void UpdateSet(DateTime dateTariff)
        {
            //_daTariff_Grid_Salary.SelectCommand.Parameters["p_date_transfer"].Value = dateTariff;
            _dtBase_Tariff.Clear();
            _daBase_Tariff.Fill(_dtBase_Tariff);
        }
    }

    public class AppDataSet
    {
        static DataSet ds;
        static Dictionary<string, OracleDataAdapter> dic_set = new Dictionary<string, OracleDataAdapter>(StringComparer.CurrentCultureIgnoreCase);
        public static SUBDIV_seq subdiv;
        public static POSITION_seq position;
        public static SUBDIV_seq allSubdiv;
        public static POSITION_seq allPosition;
        public static SUBDIV_seq subdivFR;

        static AppDataSet()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                ds = new DataSet();
                dic_set.Add("POSITION", new OracleDataAdapter(string.Format(
                    @"select POS_ID, CODE_POS, POS_NAME, UPPER(POS_NAME) POS_NAME_UPPER, NVL(POS_ACTUAL_SIGN,0) POS_ACTUAL_SIGN, 
                        DECODE(POS_ACTUAL_SIGN,1,'','X') POS_ACTUAL, POS_DATE_START, POS_DATE_END 
                    from {0}.POSITION order by POS_ACTUAL_SIGN desc, CODE_POS, POS_NAME", 
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("DEGREE", new OracleDataAdapter(string.Format(
                    "select DEGREE_ID, CODE_DEGREE, DEGREE_NAME from {0}.DEGREE order by CODE_DEGREE",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("FORM_OPERATION", new OracleDataAdapter(string.Format(
                    "select FORM_OPERATION_ID, NAME_FORM_OPERATION, CODE_FORM_OPERATION from {0}.FORM_OPERATION order by TO_NUMBER(CODE_FORM_OPERATION)",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("SUBDIV", new OracleDataAdapter(string.Format(
                    @"select code_subdiv, subdiv_name, subdiv_id, SUB_ACTUAL_SIGN, type_subdiv_id, parent_id, SUB_DATE_START, SUB_DATE_END
                    from {0}.subdiv join {0}.TYPE_SUBDIV USING(TYPE_SUBDIV_ID)
                    where PARENT_ID = 0 and (SIGN_SUBDIV_PLANT = 1 or CODE_SUBDIV = '108') ORDER BY SUB_ACTUAL_SIGN desc, CODE_SUBDIV", 
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("SOURCE_EMPLOYABILITY", new OracleDataAdapter(string.Format(
                    "select SOURCE_EMPLOYABILITY_ID, SOURCE_EMPLOYABILITY_NAME from {0}.SOURCE_EMPLOYABILITY order by SOURCE_EMPLOYABILITY_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("SPECIALITY", new OracleDataAdapter(string.Format(
                    "SELECT SPEC_ID, CODE_SPEC, NAME_SPEC, EDU_SIGN FROM {0}.SPECIALITY ORDER BY NAME_SPEC",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("INSTIT", new OracleDataAdapter(string.Format(
                    "SELECT INSTIT_ID, INSTIT_NAME, CITY_ID FROM {0}.INSTIT ORDER BY INSTIT_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("TYPE_STUDY", new OracleDataAdapter(string.Format(
                    "SELECT TYPE_STUDY_ID, TS_NAME FROM {0}.TYPE_STUDY ORDER BY TS_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("TYPE_EDU", new OracleDataAdapter(string.Format(
                    "SELECT TYPE_EDU_ID, TE_NAME, TE_PRIORITY, TYPE_EDU_PRIOR FROM {0}.TYPE_EDU ORDER BY TE_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("QUAL", new OracleDataAdapter(string.Format(
                    "SELECT QUAL_ID, QUAL_NAME FROM {0}.QUAL ORDER BY QUAL_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("GROUP_SPEC", new OracleDataAdapter(string.Format(
                    "SELECT GR_SPEC_ID, GS_NAME FROM {0}.GROUP_SPEC ORDER BY GS_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("SOURCE_COMPLECT", new OracleDataAdapter(string.Format(
                    "SELECT SOURCE_ID, SOURCE_NAME FROM {0}.SOURCE_COMPLECT ORDER BY SOURCE_NAME", 
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("TYPE_PER_DOC", new OracleDataAdapter(string.Format(
                    "select TYPE_PER_DOC_ID, NAME_DOC, TEMPL_SER, TEMPL_NUM from {0}.TYPE_PER_DOC order by NAME_DOC",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("MAR_STATE", new OracleDataAdapter(string.Format(
                    "select MAR_STATE_ID, NAME_STATE from {0}.MAR_STATE order by NAME_STATE",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("TARIFF_GRID", new OracleDataAdapter(string.Format(
                    @"select TARIFF_GRID_ID, CODE_TARIFF_GRID, TARIFF_GRID_NAME from {0}.TARIFF_GRID 
                    order by TO_NUMBER(REGEXP_SUBSTR(CODE_TARIFF_GRID,'[0-9]*')),NVL(REGEXP_SUBSTR(CODE_TARIFF_GRID,'[^[0-9]]*'),'1')",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("FORM_PAY", new OracleDataAdapter(string.Format(
                    "select FORM_PAY, NAME_FORM_PAY from {0}.FORM_PAY order by NAME_FORM_PAY",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("CONDITIONS_OF_WORK", new OracleDataAdapter(string.Format(
                    "select CONDITIONS_OF_WORK_ID, SUBCLASS_NUMBER, SIGN_CALC_PREF_WORK_EXP from {0}.CONDITIONS_OF_WORK order by SUBCLASS_NUMBER",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("PRIVILEGED_POSITION", new OracleDataAdapter(string.Format(
                    @"select PRIVILEGED_POSITION_ID, SUBDIV_ID, POS_ID, SPECIAL_CONDITIONS, KPS 
                    from {0}.PRIVILEGED_POSITION order by SUBDIV_ID, POS_ID, SPECIAL_CONDITIONS, KPS",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("SUBDIV_ALL", new OracleDataAdapter(string.Format(
                    @"select * from {0}.SUBDIV where nvl(PARENT_ID,0) = 0 and SUBDIV_ID != 0 order by CODE_SUBDIV, SUBDIV_NAME", Connect.Schema),
                    Connect.CurConnect));
                dic_set.Add("ACCESS_SUBDIV", new OracleDataAdapter(string.Format(@"select code_subdiv, subdiv_name, subdiv_id, app_name, sub_actual_sign, parent_id, sub_level from {0}.SUBDIV_ROLES_ALL where sub_level<3 and subdiv_id!=201 order by code_subdiv, sub_actual_sign desc", Connect.SchemaApstaff, Connect.SchemaSalary), Connect.CurConnect));
                foreach (string s in dic_set.Keys)
                {
                    try
                    {
                        UpdateSet(s);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Я НЕ ЗАГРУЗИЛ ТАБЛИЦУ {s}. Ошибка такая " + ex.Message);
                    }
                }
                Tables["POSITION"].Columns.Add("DISP_POSITION").Expression = "CODE_POS+' ('+POS_NAME+')'+IIF(POS_ACTUAL_SIGN=0,' <не актуально>','')";
                Tables["DEGREE"].Columns.Add("DISP_DEGREE").Expression = "CODE_DEGREE+' ('+DEGREE_NAME+')'";
                Tables["SUBDIV"].Columns.Add("DISP_SUBDIV").Expression = "CODE_SUBDIV+' ('+SUBDIV_NAME+')'+IIF(SUB_ACTUAL_SIGN=0,' <не актуально>','')";
                Tables["FORM_OPERATION"].Columns.Add("DISP_FORM_OPERATION").Expression = "CODE_FORM_OPERATION+' ('+NAME_FORM_OPERATION+')'";
                Tables["SUBDIV_ALL"].PrimaryKey = new DataColumn[] { Tables["SUBDIV_ALL"].Columns["SUBDIV_ID"] };
                Tables["SUBDIV_ALL"].Columns.Add("DISP_SUBDIV").Expression = "CODE_SUBDIV+' ('+SUBDIV_NAME+')'+IIF(SUB_ACTUAL_SIGN=0,' <не актуально>','')";

                LoadDataBase();
            }
        }

        /// <summary>
        /// Создание и загрузка таблиц подразделение и должности (используется в старых формах кадров)
        /// </summary>
        static void LoadDataBase()
        {
            try
            {
                subdiv = new SUBDIV_seq(Connect.CurConnect);
                subdiv.Fill("where SUB_ACTUAL_SIGN = 1 and WORK_TYPE_ID != 7 order by SUBDIV_NAME");
                position = new POSITION_seq(Connect.CurConnect);
                position.Fill("where POS_ACTUAL_SIGN = 1 order by POS_NAME");
                allSubdiv = new SUBDIV_seq(Connect.CurConnect);
                allSubdiv.Fill("where WORK_TYPE_ID != 7 order by SUBDIV_NAME");
                allPosition = new POSITION_seq(Connect.CurConnect);
                allPosition.Fill("order by POS_NAME");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Data: " + ex.Data + "Message: " + ex.Message + "TargetSite: " + ex.TargetSite);
            }
        }

        public static void UpdateSet(string TableName)
        {
            if (dic_set.ContainsKey(TableName))
            {
                dic_set[TableName].Fill(ds, TableName);
                if (ds.Tables[TableName].PrimaryKey.Length == 0 && ds.Tables[TableName].Columns.Contains(TableName + "_ID"))
                    ds.Tables[TableName].PrimaryKey = new DataColumn[] { ds.Tables[TableName].Columns[TableName + "_ID"] };
            }
        }

        public static DataTableCollection Tables
        {
            get
            {
                return ds.Tables;
            }
        }
    }

    public class ProjectDataSet
    {
        static DataSet ds;
        static Dictionary<string, OracleDataAdapter> dic_set = new Dictionary<string, OracleDataAdapter>(StringComparer.CurrentCultureIgnoreCase);
        static ProjectDataSet()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                ds = new DataSet();
                dic_set.Add("WORKING_TIME", new OracleDataAdapter(string.Format(
                    "select WORKING_TIME_ID, WORKING_TIME_NAME, WORKING_TIME_PATTERN, SIGN_NUMBER_OF_HOURS from {0}.WORKING_TIME order by WORKING_TIME_ID",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("TYPE_TERM_TRANSFER", new OracleDataAdapter(string.Format(
                    @"select TYPE_TERM_TRANSFER_ID, TYPE_TERM_TRANSFER_NAME, 
                        NVL(SIGN_BASE_TEMP_TRANSFER,0) SIGN_BASE_TEMP_TRANSFER, NVL(SIGN_REPL_EMP,0) SIGN_REPL_EMP 
                    from {0}.TYPE_TERM_TRANSFER order by TYPE_TERM_TRANSFER_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("BASE_TERM_TRANSFER", new OracleDataAdapter(string.Format(
                    "select BASE_TERM_TRANSFER_ID, BASE_TERM_TRANSFER_NAME from {0}.BASE_TERM_TRANSFER order by BASE_TERM_TRANSFER_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("PROJECT_PLAN_APPROVAL", new OracleDataAdapter(string.Format(
                    @"select PROJECT_PLAN_APPROVAL_ID, ROLE_NAME, NOTE_ROLE_NAME, PROJECT_PLAN_APPROVAL_ID_PRIOR, NOTE_ROLE_APPROVAL,
                        RESET_TO_PPA_ID, TYPE_TRANSFER_ID
                    from {0}.PROJECT_PLAN_APPROVAL order by TYPE_TRANSFER_ID, PROJECT_PLAN_APPROVAL_ID",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("TYPE_APPROVAL", new OracleDataAdapter(string.Format(
                    @"select TYPE_APPROVAL_ID, TYPE_APPROVAL_NAME, SIGN_APPROVAL_NOTE, SIGN_CONTINUE_APPROVAL, SIGN_REWORK, SIGN_CANCELLATION
                    from {0}.TYPE_APPROVAL order by TYPE_APPROVAL_ID",
                    Connect.Schema), Connect.CurConnect));
                dic_set.Add("BASE_DOC", new OracleDataAdapter(string.Format(
                    @"select BASE_DOC_ID, BASE_DOC_NAME from {0}.BASE_DOC order by BASE_DOC_NAME",
                    Connect.Schema), Connect.CurConnect));
                foreach (string s in dic_set.Keys)
                {
                    UpdateSet(s);
                }

                Tables["PROJECT_PLAN_APPROVAL"].DefaultView.ApplyDefaultSort = true;

                // Таблицы без PrimaryKey (чтобы не было ошибок при добавлении в них записей)
                dic_set.Add("TYPE_CONDITION", new OracleDataAdapter(string.Format(
                    @"select TYPE_CONDITION_ID, TYPE_CONDITION_NAME,PARENT_ID,SIGN_MAIN_TYPE 
                    from {0}.TYPE_CONDITION order by SIGN_MAIN_TYPE DESC, TYPE_CONDITION_NAME",
                    Connect.Schema), Connect.CurConnect));
                dic_set["TYPE_CONDITION"].Fill(ds, "TYPE_CONDITION");
            }
        }

        public static void UpdateSet(string TableName)
        {
            if (dic_set.ContainsKey(TableName))
            {
                dic_set[TableName].Fill(ds, TableName);
                if (ds.Tables[TableName].PrimaryKey.Length == 0 && ds.Tables[TableName].Columns.Contains(TableName + "_ID"))
                    ds.Tables[TableName].PrimaryKey = new DataColumn[] { ds.Tables[TableName].Columns[TableName + "_ID"] };
            }
        }

        public static DataTableCollection Tables
        {
            get
            {
                return ds.Tables;
            }
        }
    }

    /// <summary>
    /// Отдельный набор данных для штатного расписания - мелкосправочники и тд
    /// </summary>
    public class ManningDataSet
    {
        static DataSet ds;
        static Dictionary<string, OracleDataAdapter> dic_set = new Dictionary<string, OracleDataAdapter>(StringComparer.CurrentCultureIgnoreCase);
        static ManningDataSet()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                ds = new DataSet();
                dic_set.Add("ORDERS", new OracleDataAdapter("select * from APSTAFF.ORDERS order by order_name", Connect.CurConnect));
                dic_set.Add("TYPE_STAFF_ADDITION", new OracleDataAdapter("select * from APSTAFF.TYPE_STAFF_ADDITION", Connect.CurConnect));
                dic_set.Add("VAC_GROUP_TYPE", new OracleDataAdapter("select * from APSTAFF.VAC_GROUP_TYPE", Connect.CurConnect));
                dic_set.Add("WORKING_TIME", new OracleDataAdapter("select * from APSTAFF.WORKING_TIME", Connect.CurConnect));
                dic_set.Add("BASE_TARIFF", new OracleDataAdapter("select * from APSTAFF.V_BASE_TARIFF", Connect.CurConnect));
                dic_set.Add("PROF_STANDART", new OracleDataAdapter("select * from APSTAFF.PROF_STANDART order by CODE_PROF", Connect.CurConnect));
                foreach (string s in dic_set.Keys)
                {
                    UpdateSet(s);
                }
            }
        }

        public static void UpdateSet(string TableName)
        {
            try
            {
                if (dic_set.ContainsKey(TableName))
                {
                    dic_set[TableName].Fill(ds, TableName);
                    if (ds.Tables[TableName].PrimaryKey.Length == 0 && ds.Tables[TableName].Columns.Contains(TableName + "_ID"))
                        ds.Tables[TableName].PrimaryKey = new DataColumn[] { ds.Tables[TableName].Columns[TableName + "_ID"] };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения данных таблицы {TableName}. Обратитесь к разработчику ПО"+ex.Message);
            }
        }

        public static DataTableCollection Tables
        {
            get
            {
                return ds.Tables;
            }
        }
    }

}
