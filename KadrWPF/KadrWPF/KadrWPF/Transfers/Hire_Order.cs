using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using LibraryKadr;
using System.Globalization;
using System.Data.Odbc;
using WpfControlLibrary;
using PercoXML;

namespace Kadr
{
    public partial class Hire_Order : Form
    {
        //CHAR_WORK_seq char_work;
        //CHAR_TRANSFER_seq char_transfer;
        TYPE_TERM_TRANSFER_seq type_Term_Transfer;
        TARIFF_GRID_seq tariff_grid;
        DEGREE_seq degree;
        FORM_OPERATION_seq form_operation;
        FORM_PAY_seq form_pay;
        TRANSFER_seq transfer;
        //TRANSFER_obj r_transfer;
        ACCOUNT_DATA_seq account_data;
        ACCOUNT_DATA_obj r_account_data;
        POSITION_seq positionLocal;
        SUBDIV_seq subdivLocal;           
        EMP_seq emp;
        /// <summary>
        /// Признак добавления данных в перко.
        /// </summary>
        bool flagAdd = false;
        /// <summary>
        /// Код подразделения. Заполняется при входе в форму. Служит для обновления SPR.
        /// </summary>
        string code_subdiv;
        /// <summary>
        /// Конструктор формы приказа о приеме
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_emp">Таблица данных сотрудника</param>
        /// <param name="_transfer">Таблица перевода сотрудника</param>
        /// <param name="ciphers">Массив строк для формирования приказа</param>
        public Hire_Order(EMP_seq _emp, TRANSFER_seq _transfer)
        {
            InitializeComponent();
            emp = _emp;
            transfer = _transfer;
            //char_work = new CHAR_WORK_seq(Connect.CurConnect);
            //char_work.Fill(string.Format("order by {0}", CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME.ToString()));
            //char_transfer = new CHAR_TRANSFER_seq(Connect.CurConnect);
            //char_transfer.Fill("ORDER BY CHAR_TRANSFER_NAME");
            type_Term_Transfer = new TYPE_TERM_TRANSFER_seq(Connect.CurConnect);
            type_Term_Transfer.Fill("ORDER BY TYPE_TERM_TRANSFER_ID");

            tariff_grid = new TARIFF_GRID_seq(Connect.CurConnect);
            tariff_grid.Fill(string.Format("order by {0}", TARIFF_GRID_seq.ColumnsName.CODE_TARIFF_GRID.ToString()));
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME.ToString()));
            form_operation = new FORM_OPERATION_seq(Connect.CurConnect);
            form_operation.Fill(string.Format("order by {0}", FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            form_pay = new FORM_PAY_seq(Connect.CurConnect);
            form_pay.Fill("order by FORM_PAY");

            account_data = new ACCOUNT_DATA_seq(Connect.CurConnect);            
            account_data.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID));
            /// Если по данному переводу отсутствуют бухгалтерские данные, то добавляем новую строку.
            if (account_data.Count == 0)
            {
                r_account_data = account_data.AddNew();
                r_account_data.TRANSFER_ID = ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID;
            }
            else
            {
                r_account_data = (ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current;
            }

            positionLocal = new POSITION_seq(Connect.CurConnect);
            positionLocal.Clear();
            positionLocal.Fill(string.Format("where POS_ACTUAL_SIGN = 1 " +
                "union " +
                "SELECT case POS_CHIEF_OR_DEPUTY_SIGN when 1 then 'True' else 'False' end as \"POS_CHIEF_OR_DEPUTY_SIGN\",POS_ID,CODE_POS,POS_NAME, " +
                "case POS_ACTUAL_SIGN when 1 then 'True' else 'False' end as \"POS_ACTUAL_SIGN\", " +
                "POS_DATE_START,POS_DATE_END,FROM_POS_ID " +
                "FROM {0}.POSITION tab1 where POS_ID = {1} " +
                "order by POS_NAME", Connect.Schema, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).POS_ID));
            subdivLocal = new SUBDIV_seq(Connect.CurConnect);
            subdivLocal.Clear();
            subdivLocal.Fill(string.Format(
                @"where SUB_ACTUAL_SIGN = 1 and WORK_TYPE_ID != 7 
                union 
                select TYPE_SUBDIV_ID,SUBDIV_ID,CODE_SUBDIV,SUBDIV_NAME, case SUB_ACTUAL_SIGN when 1 then 'True' else 'False' end as SUB_ACTUAL_SIGN,
                    WORK_TYPE_ID,SERVICE_ID,SUB_DATE_START,SUB_DATE_END,PARENT_ID,FROM_SUBDIV,GR_WORK_ID from {0}.SUBDIV tab1 where SUBDIV_ID = {1} 
                order by SUBDIV_NAME", Connect.Schema, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SUBDIV_ID));
            /// Привязка компонентов
            cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID, 
                new LinkArgument(subdivLocal, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID,
                new LinkArgument(positionLocal, POSITION_seq.ColumnsName.POS_NAME));

            //cbChar_Work_ID.AddBindingSource(transfer, CHAR_WORK_seq.ColumnsName.CHAR_WORK_ID, 
            //    new LinkArgument(char_work,CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME));
            cbChar_Work_ID.DataSource = type_Term_Transfer;
            cbChar_Work_ID.DisplayMember = "TYPE_TERM_TRANSFER_NAME";
            cbChar_Work_ID.ValueMember = "TYPE_TERM_TRANSFER_ID";
            cbChar_Work_ID.DataBindings.Add("SelectedValue", transfer, "CHAR_WORK_ID", true, DataSourceUpdateMode.OnPropertyChanged, "");

            cbDegree_Name.AddBindingSource(transfer, DEGREE_seq.ColumnsName.DEGREE_ID, 
                new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            cbSign_Comb.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.SIGN_COMB);
            tbPer_Num.AddBindingSource(emp, EMP_seq.ColumnsName.PER_NUM);   
            tbNum_Order.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_NUM_ORDER);            
            tbContr_Empl.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.CONTR_EMP);
            cbCode_Tariff_Grid.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.TARIFF_GRID_ID, 
                new LinkArgument(tariff_grid, TARIFF_GRID_seq.ColumnsName.CODE_TARIFF_GRID));
            tbSalary.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.SALARY);
            tbClassific.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.CLASSIFIC);
            tbSign_Add.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.SIGN_ADD);
            tbProff_Addition.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.PROF_ADDITION);
            tbHarmfull_Addition.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.HARMFUL_ADDITION);
            tbHarmfull_Addition_Add.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.HARMFUL_ADDITION_ADD);
            tbComb_Addition.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.COMB_ADDITION);
            tbSecret_Addition.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.SECRET_ADDITION);
            tbProbation_Period.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.PROBA_PERIOD);
            deDate_Hire.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_TRANSFER);
            deDate_End_Contr.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_END_CONTR);
            deDate_Order.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_DATE_ORDER);
            deDate_Contr.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_CONTR);
            deDate_Add.AddBindingSource(account_data, ACCOUNT_DATA_seq.ColumnsName.DATE_ADD);
            cbForm_Operation.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.FORM_OPERATION_ID,
                new LinkArgument(form_operation, FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            cbForm_Pay.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.FORM_PAY,
                new LinkArgument(form_pay, FORM_PAY_seq.ColumnsName.NAME_FORM_PAY));
            tbHarmful_Vac.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.HARMFUL_VAC);
            tbPos_Note.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.POS_NOTE);
            if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).PRIVILEGED_POSITION_ID != null)
                chSign_Priv_Pos.Checked = true;

            /// Настройка компонентов.
            DisableControl.DisableAll(this, false, Color.White);
            btEdit.Enabled = true;
            btExit.Enabled = true;
            /// Если все необходимые поля для формирования приказа заполнены, открываем доступ на его просмотр
            /// и устанавливаем флаг добавления данных в false.
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER != null &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_NUM_ORDER != null &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CONTR_EMP != null &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAR_WORK_ID != null &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DEGREE_ID != null &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER != null &&
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR != null)
            {
                btPreview.Enabled = true;
                flagAdd = false;
            }
            else
                flagAdd = true;
            /// Если дата приказа пустая, записываем туда текущую дату.
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER == null)
            {
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;             
            }
            /// Если дата договора пустая, записываем туда текущую дату.
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CONTR_EMP == null ||
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CONTR_EMP == "")
            {
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR = DateTime.Now;
            }

            /// Прочие необходимые действия.
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);
            cbForm_Operation.SelectedIndexChanged += new EventHandler(cbForm_Operation_SelectedIndexChanged);

            tbSalary.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbProff_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbHarmfull_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbComb_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbSecret_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);

            /// Устанавливаем коды для подразделения и должности.
            /// Сделано потому что, если индекс в комбобоксе не меняется, событие при котором данный код 
            /// устанавливается не происходит. Здесь мы принудительно это делаем.
            tbCode_Subdiv.Text = subdivLocal.Where(s => s.SUBDIV_ID ==
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SUBDIV_ID).FirstOrDefault().CODE_SUBDIV.ToString();
            tbCode_Pos.Text = positionLocal.Where(s => s.POS_ID.ToString() == cbPos_Name.SelectedValue.ToString()).FirstOrDefault().CODE_POS.ToString();
            tbCode_Degree.Text = degree.Where(s => s.DEGREE_ID.ToString() == cbDegree_Name.SelectedValue.ToString()).FirstOrDefault().CODE_DEGREE.ToString();
            tbCode_Form_Operation.Text =
                form_operation.Where(s => s.FORM_OPERATION_ID.ToString() == cbForm_Operation.SelectedValue.ToString()).FirstOrDefault().CODE_FORM_OPERATION.ToString();

            code_subdiv = tbCode_Subdiv.Text;
            
            // Выбираем признак УТК
            OracleCommand _ocTransfer_QM = new OracleCommand(string.Format(
                @"SELECT TRANSFER_QM_ID FROM {0}.TRANSFER_QM WHERE TRANSFER_ID = :p_TRANSFER_ID",
                Connect.Schema), Connect.CurConnect);
            _ocTransfer_QM.BindByName = true;
            _ocTransfer_QM.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
            if (_ocTransfer_QM.ExecuteReader().Read())
                chTransfer_QM.Checked = true;

            if (Connect.UserId.ToUpper() == "BMW12714")
            {
                btInsert_Into_Perco.Visible = true;
                btInsert_Into_Perco.Enabled = true;
            }
            if (GrantedRoles.GetGrantedRole("STAFF_GROUP_HIRE"))
            {
                btEmp_Contract.Visible = true;
                btEmp_Contract.Enabled = true;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки изменения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEdit_Click(object sender, EventArgs e)
        {
            DisableControl.DisableAll(this, true, Color.White);
            btExit.Text = "Отмена";
            btEdit.Enabled = false;
            btPreview.Enabled = false;
            tbPer_Num.Enabled = false;
            tbNum_Order.Enabled = false;
            tbContr_Empl.Enabled = false;
            chSign_Priv_Pos.Enabled = false;
        }

        /// <summary>
        /// Событие нажатия кнопки сохранения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            /*Проверяем есть ли подключение к сетевым дискам*/
            OdbcConnection odbcTestCon = new OdbcConnection("");
            odbcTestCon.ConnectionString = string.Format(
                @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                ParVal.Vals["SPR"]);
            try
            {                
                odbcTestCon.Open();
                odbcTestCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nНевозможно подключиться к файлу " + ParVal.Vals["SPR"] +
                    "!\nПроверьте подключение к сети, наличие сетевых дисков и повторите попытку.");
                return;
            }
            /// Останавливаем редактирование переводов.
            /// Проводим различные проверки на ввод данных.
            ((CurrencyManager)BindingContext[transfer]).EndCurrentEdit();
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER == null)
            {
                MessageBox.Show("Вы не указали дату приема!" + "\n" + "Повторите ввод.","АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDate_Hire.Focus();
                return;
            }
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAR_WORK_ID == null)
            {
                MessageBox.Show("Вы не выбрали Срок договора!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbChar_Work_ID.Focus();
                return;
            }
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DEGREE_ID == null)
            {
                MessageBox.Show("Вы не выбрали категорию!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbDegree_Name.Focus();
                return;
            }
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FORM_OPERATION_ID == null)
            {
                MessageBox.Show("Вы не выбрали вид производства!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbForm_Operation.Focus();
                return;
            }
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FORM_PAY == null)
            {
                MessageBox.Show("Вы не выбрали форму оплаты труда!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbForm_Pay.Focus();
                return;
            }
            else
            {
                OracleCommand ocSignError = new OracleCommand(string.Format(
                    @"select COUNT(*) from {0}.FORM_PAY_ON_DEGREE FP
                        where FP.DEGREE_ID = :p_DEGREE_ID and FP.FORM_PAY_ID = :p_FORM_PAY_ID",
                    Connect.Schema), Connect.CurConnect);
                ocSignError.Parameters.Add("p_DEGREE_ID", OracleDbType.Decimal).Value = cbDegree_Name.SelectedValue;
                ocSignError.Parameters.Add("p_FORM_PAY_ID", OracleDbType.Decimal).Value = cbForm_Pay.SelectedValue;
                if (Convert.ToInt16(ocSignError.ExecuteScalar()) == 0)
                {
                    MessageBox.Show("Неверное сочетание Формы оплаты и Категории!\nПовторите ввод.", "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbForm_Pay.Focus();
                    return;
                }
            }
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER == null)
            {
                MessageBox.Show("Вы не указали дату приказа!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDate_Order.Focus();
                return;
            }
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR == null)
            {
                MessageBox.Show("Вы не указали дату договора!" + "\n" + "Повторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deDate_Contr.Focus();
                return;
            }
            if (tbHarmful_Vac.Text == "")
            {
                MessageBox.Show("Необходимо ввести количество дней дополнительного отпуска!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbHarmful_Vac.Focus();
                return;
            }
            /// Устанавливаем признак приема на работу.
            ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).HIRE_SIGN = true;
            /// Если вставляем запись о приема устанавливаем признак текущей работы
            if (flagAdd)
            {
                OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format(
                    "select gr_work_id from {0}.SUBDIV where subdiv_id = :p_SUBDIV_ID",
                    Connect.Schema);
                com.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal).Value = cbSubdiv_Name.SelectedValue;
                OracleDataReader reader = com.ExecuteReader();
                if (reader.Read())
                {
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).GR_WORK_ID =
                        Convert.ToDecimal(reader[0] != DBNull.Value ? reader[0] : null);
                }
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_CUR_WORK = true;

            }
            /// Если принимаем совместителя, то необходимо получить дату приема по основному месту. 
            /// Если таковой нет, то ставим дату приема равную дате этого приема.
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_COMB)
            {
                /// Создаем таблицу переводов и заполняем ее текущей работой.
                TRANSFER_seq transferCur = new TRANSFER_seq(Connect.CurConnect);
                transferCur.Fill(string.Format(" where {0} = '{1}' and {2} = 1 ",
                    TRANSFER_seq.ColumnsName.PER_NUM, 
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM, 
                    TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
                /// Если данные по текущей работе отсутствуют, то заполняем дату приема новым значением.
                /// Если данные есть, то заполняем поля имеющимися данными.
                if (transferCur.Count == 0)
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE = 
                        Convert.ToDateTime(deDate_Hire.Text);
                else
                {
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE = 
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferCur]).Current)).DATE_HIRE;
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SOURCE_ID = 
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferCur]).Current)).SOURCE_ID;
                }
            }
            /// Изменения от 2010.10.20
            /// Для приема на основную работу добавлена та же проверка на наличие 
            /// текущей работы как для приема совместителя
            else
            {
                ///// Создаем таблицу переводов и заполняем ее текущей работой.
                //TRANSFER_seq transferCur = new TRANSFER_seq(connection);
                //transferCur.Fill(string.Format(" where {0} = '{1}' and {2} = 1 ",
                //    TRANSFER_seq.ColumnsName.PER_NUM,
                //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM, 
                //    TRANSFER_seq.ColumnsName.SIGN_CUR_WORK));
                ///// Если данные по текущей работе отсутствуют, то заполняем дату приема новым значением.
                ///// Если данные есть, то заполняем поля имеющимися данными.
                //if (transferCur.Count == 0)
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE = 
                        Convert.ToDateTime(deDate_Hire.Text);
                //else
                //{
                //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE = 
                //        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferCur]).Current)).DATE_HIRE;
                //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SOURCE_ID = 
                //        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferCur]).Current)).SOURCE_ID;
                //}
            }
            /// Если дата формирования приказа пустая, заполняем ее текущей датой.
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DF_BOOK_ORDER == null)
            {
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DF_BOOK_ORDER = DateTime.Now;
            }

            /// Изменения от 27.07.2010
            /// Номер приказа о приеме и номер договора формируются при нажатии на кнопку сохранения.
            /// Раньше формировались при создании формы.
            if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_NUM_ORDER == null ||
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_NUM_ORDER == "")
            {
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_NUM_ORDER = 
                    (Library.NumberDoc(Staff.DataSourceScheme.SchemeName, 
                    string.Format("sknprik_{0}_seq", DateTime.Now.Year.ToString()),
                    "nextval", Connect.CurConnect)).ToString();
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CONTR_EMP = 
                    (Library.NumberDoc(Staff.DataSourceScheme.SchemeName, 
                    string.Format("skkon_{0}_seq", DateTime.Now.Year.ToString()),
                    "nextval", Connect.CurConnect)).ToString();
            }            
            /// Сохраняем данные в базе.
            transfer.Save();
            if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CHANGE_DATE == null)
            {
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CHANGE_DATE =
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER;
            }
            if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_SERVANT 
                == null)
            {
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_SERVANT =
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE;
            }
            account_data.Save();
            Library.UpdateGR_Work(
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM.ToString(),
                Convert.ToInt32(((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID),
                Convert.ToInt32(((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).GR_WORK_ID),
                (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER,
                1);
            /// Обновляем отображение переводов.
            ((CurrencyManager)BindingContext[transfer]).Refresh();
            ((CurrencyManager)BindingContext[emp]).Refresh();

            /// Объекты для изменения данных в различных таблицах
            OdbcConnection odbcCon;
            OdbcCommand _rezult;

            /// Если добавляем нового работника, то в перко осуществляем вставку.
            /// Если редактируем данные, то в перко обновляем имеющиеся данные.
            if (flagAdd)
            {                  
                EMP_obj r_empNew;
                TRANSFER_obj r_transferNew;
                if (((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PERCO_SYNC_ID == null)
                {
                    OracleCommand command = new OracleCommand();
                    command.Connection = Connect.CurConnect;
                    command.CommandText = string.Format(
                        "select {0}.perco_sync_id_seq.nextval from dual", Staff.DataSourceScheme.SchemeName);
                    decimal perco_sync_id = Convert.ToDecimal(command.ExecuteScalar());
                    ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PERCO_SYNC_ID = perco_sync_id;
                    emp.Save();
                    Connect.Commit();
                    r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                    r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                    DictionaryPerco.employees.InsertEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), 
                        r_empNew.PER_NUM, r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                        r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()) { DateBegin = r_transferNew.DATE_HIRE.ToString() });
                }
                else
                {
                    DictionaryPerco.employees.RecoveryEmployee(
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PERCO_SYNC_ID.ToString(),
                        ((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PER_NUM);
                    r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                    r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                    DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                        r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                        r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()) { DateBegin = r_transferNew.DATE_HIRE.ToString() });
                }
                flagAdd = false;
                /*Добавляем новую запись в SPR*/
                odbcCon = new OdbcConnection("");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["SPR"]);
                //MessageBox.Show("Строка подключения создана!");
                odbcCon.Open();
                //MessageBox.Show("Подключено!");
                _rezult = new OdbcCommand("", odbcCon);
                DateTime datePost;
                if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_ADD != null)
                {
                    datePost = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_ADD;
                }
                else
                {
                    datePost = new DateTime(1, 1, 1);
                }
                DateTime date_servant;
                if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_SERVANT != null)
                {
                    date_servant = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_SERVANT;
                }
                else
                {
                    date_servant = (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE;
                }
                /// Проверяем наличие записей в SPR по сотруднику в данном подразделении
                _rezult.CommandText = string.Format(
                    "select count(*) from SPR where podr = '{0}' and tnom = '{1}' and p_rab = '{2}'",
                    tbCode_Subdiv.Text, r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "");
                int countRow = Convert.ToInt32(_rezult.ExecuteScalar());
                /// Если записей нет, то вставляем новую запись. Если записи есть, то обновляем существующую запись.
                if (countRow == 0)
                {
                    _rezult.CommandText = string.Format(
                        "insert into SPR(PODR,TNOM,FIO,DAT_POST,KT,SHPR,POL,DAT_ROG,DAT_KOR, " +
                        "FAM,NAM,OTC,P_RAB,RAZ,OKL," +
                        "PRF,PRNAD, NAL,ALIM,NAD_VRED,BIR,SC_OT,PR13,DUB_NAL,KOL_IG14,KOL_IG15, " +
                        "KOL_IG16,KOL_IG17,KOL_IG18,KOL_IG19,KOL_IG20,KOL_IG21,POST_INDEX,OKL_N, " +
                        "VSET, INN_STRAH,KOD_DOC,SER_DOC,NOM_DOC,STATE,CITY,PUNKT,STREET,HOUSE,KORPUS,FLAT,P_PI, " +
                        "PUNKT_ROG,DISTR_ROG,REGION_ROG,STRANA_ROG,KEM_VID,PHONE,NALOG_INN,SER_MEDP,NOM_MEDP,ZAKAZ, " +
                        "DAT_VIS,DAT_UV,DAT_PI,DAT_DOK,DAT_PR,DAT_PRIEM,DAT_PRUZ)" +
                        "values('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8}, " +
                        "'{9}','{10}','{11}','{12}', {14}, {15}, " +
                        /*19*/
                        "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, " + "'{16}'," +
                        /*22*/
                        "'','','','','','','','','','','','','','','','','','','','','','', " +
                        "{13},ctod(''),ctod(''),ctod(''),ctod(''),ctod(''),ctod(''))",
                        tbCode_Subdiv.Text, r_empNew.PER_NUM,
                        string.Format("{0} {1} {2}", r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME.Substring(0, 1),
                            r_empNew.EMP_MIDDLE_NAME.Substring(0, 1)),
                        "{^" + datePost.Year.ToString() + "-" +
                            datePost.Month.ToString().PadLeft(2, '0') + "-" +
                            datePost.Day.ToString().PadLeft(2, '0') + "}",
                        tbCode_Degree.Text, tbCode_Pos.Text, r_empNew.EMP_SEX == "М" ? "1" : "2",
                        "{^" + r_empNew.EMP_BIRTH_DATE.Value.Year.ToString() + "-" +
                            r_empNew.EMP_BIRTH_DATE.Value.Month.ToString().PadLeft(2, '0') + "-" +
                            r_empNew.EMP_BIRTH_DATE.Value.Day.ToString().PadLeft(2, '0') + "}",
                        "date()",
                        r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                        r_transferNew.SIGN_COMB ? "2" : "",
                        "{^" + date_servant.Year.ToString() + "-" +
                            date_servant.Month.ToString().PadLeft(2, '0') + "-" +
                            date_servant.Day.ToString().PadLeft(2, '0') + "}",
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CLASSIFIC == null ? 
                        0 : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CLASSIFIC,
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).SALARY == null ? "0"
                        : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).SALARY.Value.ToString().Replace(",", "."),
                        cbCode_Tariff_Grid.Text);
                }
                else
                {
                    _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_POST = {3}, KT = '{4}', " +
                        "SHPR = {5}, POL = '{6}', DAT_ROG = {7}, DAT_KOR = {8}, FAM = '{9}', " +
                        "NAM = '{10}', OTC = '{11}', DAT_VIS = {13}, DAT_UV = {14}, RAZ = {15}, OKL = {16}, VSET = '{17}' " +
                        "where podr = '{12}' and tnom = '{0}' and p_rab = '{1}'",
                        r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "",
                        string.Format("{0} {1} {2}", r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME.Substring(0, 1),
                            r_empNew.EMP_MIDDLE_NAME.Substring(0, 1)),
                        "{^" + datePost.Year.ToString() + "-" +
                            datePost.Month.ToString().PadLeft(2, '0') + "-" +
                            datePost.Day.ToString().PadLeft(2, '0') + "}",
                        tbCode_Degree.Text, tbCode_Pos.Text, r_empNew.EMP_SEX == "М" ? "1" : "2",
                        "{^" + r_empNew.EMP_BIRTH_DATE.Value.Year.ToString() + "-" +
                            r_empNew.EMP_BIRTH_DATE.Value.Month.ToString().PadLeft(2, '0') + "-" +
                            r_empNew.EMP_BIRTH_DATE.Value.Day.ToString().PadLeft(2, '0') + "}",
                        "date()",
                        r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                        tbCode_Subdiv.Text,
                        "{^" + date_servant.Year.ToString() + "-" +
                            date_servant.Month.ToString().PadLeft(2, '0') + "-" +
                            date_servant.Day.ToString().PadLeft(2, '0') + "}", "ctod('')",
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CLASSIFIC == null ? 
                        0 : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CLASSIFIC,
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).SALARY == null ? "0"
                            : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).SALARY.Value.ToString().Replace(",", "."),
                        cbCode_Tariff_Grid.Text);
                }                
                //MessageBox.Show("Строка создана!");
                _rezult.ExecuteNonQuery();
                //MessageBox.Show("Выполнена!");
                odbcCon.Close();
            }
            else
            {
                EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                    r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                    r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()) { DateBegin = r_transferNew.DATE_HIRE.ToString() });

                //MessageBox.Show("Строка подключения создана1!");
                odbcCon = new OdbcConnection("");
                //MessageBox.Show("Строка подключения создана2!");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["SPR"]);
                //MessageBox.Show("Строка подключения создана!");
                odbcCon.Open();
                //MessageBox.Show("Подключено!");
                _rezult = new OdbcCommand("", odbcCon);
                DateTime datePost;
                if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_ADD != null)
                {
                    datePost = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_ADD;
                }
                else
                {
                    datePost = new DateTime(1, 1, 1);
                }
                DateTime date_servant;
                if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_SERVANT != null)
                {
                    date_servant = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).DATE_SERVANT;
                }
                else
                {
                    date_servant = (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE;
                }
                //MessageBox.Show("Дата создана!");
                _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_POST = {3}, KT = '{4}', " +
                    "SHPR = {5}, POL = '{6}', DAT_ROG = {7}, DAT_KOR = {8}, FAM = '{9}', " +
                    "NAM = '{10}', OTC = '{11}', PODR = '{13}', DAT_VIS = {14}, RAZ = {15}, OKL = {16}, VSET = '{17}' " +
                    "where podr = '{12}' and tnom = '{0}' and p_rab = '{1}'",
                    r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "",
                    string.Format("{0} {1} {2}", r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME.Substring(0, 1),
                        r_empNew.EMP_MIDDLE_NAME == "" ? " " : r_empNew.EMP_MIDDLE_NAME.Substring(0, 1)),
                    "{^" + datePost.Year.ToString() + "-" +
                        datePost.Month.ToString().PadLeft(2, '0') + "-" +
                        datePost.Day.ToString().PadLeft(2, '0') + "}",
                    tbCode_Degree.Text, tbCode_Pos.Text, r_empNew.EMP_SEX == "М" ? "1" : "2",
                    "{^" + r_empNew.EMP_BIRTH_DATE.Value.Year.ToString() + "-" +
                        r_empNew.EMP_BIRTH_DATE.Value.Month.ToString().PadLeft(2, '0') + "-" +
                        r_empNew.EMP_BIRTH_DATE.Value.Day.ToString().PadLeft(2, '0') + "}",
                    "date()",
                    r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                    code_subdiv, tbCode_Subdiv.Text,
                    "{^" + date_servant.Year.ToString() + "-" +
                        date_servant.Month.ToString().PadLeft(2, '0') + "-" +
                        date_servant.Day.ToString().PadLeft(2, '0') + "}",
                    ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CLASSIFIC == null ? 
                    0 : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).CLASSIFIC,
                    ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).SALARY == null ? "0"
                        : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).SALARY.Value.ToString().Replace(",", "."),
                    cbCode_Tariff_Grid.Text);
                //MessageBox.Show("Строка создана!");
                _rezult.ExecuteNonQuery();
                //MessageBox.Show("Выполнена!");
                odbcCon.Close();

            }
            Connect.Commit();

            // Обновляем признак УТК
            OracleCommand _ocUpdateTransfer_QM = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.TRANSFER_QM_UPDATE(:p_TRANSFER_ID, :p_PROJECT_TRANSFER_ID, :p_PROJECT_STATEMENT_ID, :p_SIGN_TRANSFER_QM);
                END;",
                Connect.Schema), Connect.CurConnect);
            _ocUpdateTransfer_QM.BindByName = true;
            _ocUpdateTransfer_QM.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
            _ocUpdateTransfer_QM.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal);
            _ocUpdateTransfer_QM.Parameters.Add("p_PROJECT_STATEMENT_ID", OracleDbType.Decimal);
            _ocUpdateTransfer_QM.Parameters.Add("p_SIGN_TRANSFER_QM", OracleDbType.Decimal).Value =
                Convert.ToInt16(chTransfer_QM.Checked);
            OracleTransaction transact = Connect.CurConnect.BeginTransaction();
            try
            {
                _ocUpdateTransfer_QM.Transaction = transact;
                _ocUpdateTransfer_QM.ExecuteNonQuery();
                transact.Commit();
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show("Ошибка обновления признака УТК!\n" +
                    ex.Message, "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //

            /* Оставил этот код в комментариях, потому что для нового работника не должно быть 
             записей в обновляемых таблицах. В то время как можно испортить данные у старых работников
             при редактировании приказа о приеме. */
            #region Обновление таблиц
            //odbcCon = new OdbcConnection("");
            //odbcCon.ConnectionString = string.Format(
            //    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
            //    ParVal.Vals["PODOT"]);
            //odbcCon.Open();
            //_rezult = new OdbcCommand("", odbcCon);
            //_rezult.CommandText = string.Format("update podot set podr = '{0}' where tnom = '{1}'",
            //    tbCode_Subdiv.Text, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM);
            //_rezult.ExecuteNonQuery();
            //odbcCon.Close();
            ///*Обновление SUD, UD_SUD*/
            //odbcCon.ConnectionString = string.Format(
            //    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
            //    ParVal.Vals["SUD"]);
            //odbcCon.Open();
            //_rezult.CommandText = string.Format("update SUD set podr = '{0}' where tnom = '{1}' and p_rab = '{2}'",
            //    tbCode_Subdiv.Text, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM,
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_COMB ? "2" : "");
            //_rezult.ExecuteNonQuery();
            //_rezult.CommandText = string.Format("select pnsud from SUD where tnom = '{0}' and p_rab = '{1}'",
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM,
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_COMB ? "2" : "");
            //OdbcCommand updUd_Sud = new OdbcCommand("", odbcCon);
            //OdbcDataReader _reader = _rezult.ExecuteReader();
            //while (_reader.Read())
            //{
            //    updUd_Sud.CommandText = string.Format("update UD_SUD set podr = '{0}' where tnom = '{1}' and pnsud = '{2}'",
            //       tbCode_Subdiv.Text, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM,
            //       _reader[0].ToString().Trim());
            //    updUd_Sud.ExecuteNonQuery();
            //}
            //odbcCon.Close();
            ///*Обновление SPR_287*/
            //odbcCon.ConnectionString = string.Format(
            //    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
            //    ParVal.Vals["SPR_287"]);
            //odbcCon.Open();
            //_rezult.CommandText = string.Format("update SPR_287 set podr = '{0}' where tnom = '{1}' and p_rab = '{2}'",
            //    tbCode_Subdiv.Text, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM,
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_COMB ? "2" : "");
            //_rezult.ExecuteNonQuery();
            //odbcCon.Close();
            ///*Обновление ALSPR*/
            //odbcCon.ConnectionString = string.Format(
            //    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
            //    ParVal.Vals["ALSPR"]);
            //odbcCon.Open();
            //_rezult.CommandText = string.Format("update ALSPR set sc = '{0}' where tn = '{1}' and p_rab = '{2}'",
            //    tbCode_Subdiv.Text, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM,
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_COMB ? "2" : "");
            //_rezult.ExecuteNonQuery();
            //odbcCon.Close();
            #endregion

            /// Настройка компонентов.
            btExit.Text = "Выход";
            DisableControl.DisableAll(this, false, Color.White);
            btEdit.Enabled = true;
            btExit.Enabled = true;
            btPreview.Enabled = true;
        }

        /// <summary>
        /// Событие нажатия кнопки формирования приказа о приеме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPreview_Click(object sender, EventArgs e)
        {
            /// Различные переменные.
            string pos_name, pos_name1, full_pos_name;
            full_pos_name = cbPos_Name.Text + " " + tbPos_Note.Text.Trim();
            /// Если длина профессии больше 50, то разбиваем ее на части.
            if (full_pos_name.Length > 50)
            {
                pos_name = full_pos_name.Substring(0, 50);
                pos_name1 = full_pos_name.Substring(50, full_pos_name.Length - 50);
            }
            else
            {
                pos_name = full_pos_name.Substring(0, full_pos_name.Length);
                pos_name1 = "";            
            }
                     
            EMP_obj r_emp = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
            /// Создаем таблицу и заполняем ее должностным лицом.
            /*OracleDataTable dataTable = new OracleDataTable(string.Format(Queries.GetQuery("SelectDirector.sql"),
                Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_LAST_NAME, EMP_seq.ColumnsName.EMP_FIRST_NAME, 
                EMP_seq.ColumnsName.EMP_MIDDLE_NAME, EMP_seq.ColumnsName.PER_NUM,
                "director", DIRECTOR_seq.ColumnsName.DIR_CODE_POS, 29950), Connect.CurConnect);
            dataTable.Fill();
            /// Если данные найдены, то формируем строку подписи должностного лица.
            if (dataTable.Rows.Count != 0)
            {
                podpis = dataTable.Rows[0][0].ToString() + " " + dataTable.Rows[0][1].ToString() + "." + dataTable.Rows[0][2].ToString() + ".";
            }
            else
            {
                podpis = "";
            }*/
            string[][] s_pos = new string[][]{};
            if (Signes.Show(0, "Hire_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == true)
            {
                /// Различные переменные.
                string dayTrDateOrder, monthTrDateOrder, yearTrDateOrder, dateTransfer, dateAdd, dayDateContr, monthDateContr, yearDateContr;
                /// Проверяем пуста ли дата приказа о приеме и заполняем переменные.
                if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER != null)
                {
                    dayTrDateOrder =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER.Value.Day.ToString();
                    monthTrDateOrder =
                        Library.MyMonthName(((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER.Value);
                    yearTrDateOrder =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER.Value.Year.ToString();
                }
                else
                {
                    dayTrDateOrder = monthTrDateOrder = yearTrDateOrder = "";
                }
                /// Проверяем пуста ли дата договора и заполняем переменные.
                if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR != null)
                {
                    dayDateContr =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR.Value.Day.ToString();
                    monthDateContr =
                        Library.MyMonthName(((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR.Value);
                    yearDateContr =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_CONTR.Value.Year.ToString();
                }
                else
                {
                    dayDateContr = monthDateContr = yearDateContr = "";
                }
                /// Проверяем пуста ли дата перевода.
                if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER != null)
                {
                    dateTransfer =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER.Value.ToShortDateString();
                }
                else
                {
                    dateTransfer = "";
                }
                /// Если работника принимают на определенный период, то добавляем строку перевода данными.
                if ((((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_END_CONTR != null) &&
                    (dateTransfer != ""))
                {
                    dateTransfer += " по " +
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_END_CONTR.Value.ToShortDateString();
                }
                /// Проверка на совмещение.
                if (((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SIGN_COMB)
                {
                    dateTransfer += " совместитель";
                }
                /// Проверяем дату надбавки.
                if (r_account_data.DATE_ADD != null)
                {
                    dateAdd = r_account_data.DATE_ADD.Value.ToShortDateString();
                }
                else
                {
                    dateAdd = "";
                }

                /// Формируем массив параметров.
                CellParameter[] cellParameters = new CellParameter[] { 
                    new CellParameter(7, 7, dayTrDateOrder, null), new CellParameter(7, 12, monthTrDateOrder, null), 
                    new CellParameter(7, 23, yearTrDateOrder, null), new CellParameter(7, 33, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_NUM_ORDER, null),                
                    new CellParameter(20, 56, r_emp.PER_NUM, null), new CellParameter(22, 4 , r_emp.EMP_LAST_NAME, null), 
                    new CellParameter(22, 25, r_emp.EMP_FIRST_NAME, null), new CellParameter(22, 45 , r_emp.EMP_MIDDLE_NAME, null),
                    new CellParameter(23, 21, dateTransfer, null), new CellParameter(25, 3, cbSubdiv_Name.Text, null),
                    new CellParameter(27, 14, pos_name, null), new CellParameter(29, 14, pos_name1, null),
                    new CellParameter(31, 16, tbCode_Pos.Text, null), new CellParameter(33, 8, tbClassific.Text, null),
                    new CellParameter(33, 31, cbCode_Tariff_Grid.Text, null), new CellParameter(33, 58, tbSalary.Text, null),
                    new CellParameter(35, 10, (string.IsNullOrEmpty(tbComb_Addition.Text.Trim()) ?  "" : string.Format("совмещение - {0}; ",tbComb_Addition.Text)) +
                        (string.IsNullOrEmpty(tbHarmfull_Addition_Add.Text.Trim()) ? "" : string.Format("вредность доп. - {0}; ", tbHarmfull_Addition_Add.Text)).
                            Trim().TrimEnd(new char[] { ';' }), null),
                    new CellParameter(37, 14, cbForm_Pay.Text, null), new CellParameter(39, 11, cbDegree_Name.Text, null),
                    new CellParameter(39, 61, tbCode_Degree.Text, null), new CellParameter(41, 41, tbSign_Add.Text, null),
                    new CellParameter(42, 44, dateAdd, null), new CellParameter(44, 24, tbProbation_Period.Text, null),
                    new CellParameter(48, 26, dayDateContr, null), new CellParameter(48, 31, monthDateContr, null), 
                    new CellParameter(48, 42, yearDateContr, null), new CellParameter(48, 52, ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CONTR_EMP, null),
                    new CellParameter(51, 1, s_pos[0][0], null), new CellParameter(51, 45, s_pos[0][1], null)
                };
                /// Отображаем отчет.
                Excel.PrintR1C1(!GrantedRoles.GetGrantedRole("DBA"),"Hire.xlt", cellParameters);
            }
        }

        /// <summary>
        /// Событие нажатия кнопки закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            /// Если не выполняется редактирование данных, то просто закрываем форму.
            /// Если выполняется редактирование данных, то откатываем внесенные изменения.
            if (btExit.Text == "Выход")
            {
                Close();
            }
            else
            {
                emp.RollBack();
                emp.ResetBindings();
                transfer.RollBack();
                transfer.ResetBindings();
                Connect.Rollback();
                btExit.Text = "Выход";
                DisableControl.DisableAll(this, false, Color.White);
                btEdit.Enabled = true;
                btExit.Enabled = true;
                btPreview.Enabled = true;
            }
        }

        /// <summary>
        /// Событие изменения индекса категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbDegree_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDegree_Name.SelectedValue != null)
            {
                tbCode_Degree.Text = Library.CodeBySelectedValue(Connect.CurConnect, DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(),
                    Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.DEGREE_ID.ToString(), cbDegree_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Проверка введенного шифра категории и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Degree_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Degree, cbDegree_Name, 2, Connect.CurConnect, e, DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "degree", DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(), tbCode_Degree.Text);
        }   

        /// <summary>
        /// Событие изменения индекса подразделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbPos_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPos_Name.SelectedValue != null)
            {
                tbCode_Pos.Text = Library.CodeBySelectedValue(Connect.CurConnect, POSITION_seq.ColumnsName.CODE_POS.ToString(), Staff.DataSourceScheme.SchemeName,
                    "position", POSITION_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Проверка введенного шифра должности и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pos, cbPos_Name, 5, Connect.CurConnect, e, POSITION_seq.ColumnsName.POS_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "position", POSITION_seq.ColumnsName.CODE_POS.ToString(), tbCode_Pos.Text);
        }

        private void cbForm_Operation_Leave(object sender, EventArgs e)
        {
            if (cbForm_Operation.Text == "")
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).FORM_OPERATION_ID = null;
        }

        private void tbCode_Form_Operation_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Form_Operation, cbForm_Operation, 1, Connect.CurConnect, e,
                FORM_OPERATION_seq.ColumnsName.FORM_OPERATION_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "form_operation",
                FORM_OPERATION_seq.ColumnsName.CODE_FORM_OPERATION.ToString(), tbCode_Form_Operation.Text);
        }

        void cbForm_Operation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbForm_Operation.SelectedValue != null)
            {
                tbCode_Form_Operation.Text =
                    Library.CodeBySelectedValue(Connect.CurConnect, FORM_OPERATION_seq.ColumnsName.CODE_FORM_OPERATION.ToString(),
                    Staff.DataSourceScheme.SchemeName, "form_operation",
                    FORM_OPERATION_seq.ColumnsName.FORM_OPERATION_ID.ToString(), cbForm_Operation.SelectedValue.ToString());
            }
        }

        private void cbCode_Tariff_Grid_Leave(object sender, EventArgs e)
        {
            if (cbCode_Tariff_Grid.Text == "")
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account_data]).Current).TARIFF_GRID_ID = null;
        }

        private void btInsert_Into_Perco_Click(object sender, EventArgs e)
        {
            if (((EMP_obj)((CurrencyManager)BindingContext[emp]).Current).PERCO_SYNC_ID != null)
            {
                EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                if (DictionaryPerco.employees.InsertEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(),
                    r_empNew.PER_NUM, r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                    r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()) { DateBegin = r_transferNew.DATE_HIRE.ToString() }))
                    MessageBox.Show("Запись по сотруднику добавлена в систему Перко.", "АСУ \"Кадры\"", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Ошибка добавления записи.", "АСУ \"Кадры\"", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);      
            }
        }

        private void btEmp_Contract_Click(object sender, EventArgs e)
        {
            System.Data.DataTable _dt = new System.Data.DataTable();
            if (Signes.Show(0, "Hire_Order", "Выберите должностное лицо для подписи", 1, ref _dt) == true)
            {
                DataSet _dsReport = new DataSet();
                _dsReport.Tables.Add("SIGNES");
                _dsReport.Tables.Add("ORDER_EMP");
                _dsReport.Tables.Add("ORDER_EMP_COND");
                _dsReport.Tables.Add("APPROVAL");
                int _sign_Dir_Sugnature = 0;
                string _fio_Signature = "";
                new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO, 0 DEFAULT_NUMBER from dual where 1 = 2",
                    Connect.CurConnect).Fill(_dsReport.Tables["SIGNES"]);
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    _dsReport.Tables["SIGNES"].Rows.Add(new object[] { 
                            _dt.Rows[i][0].ToString(), _dt.Rows[i][1].ToString(), _dt.Rows[i][2].ToString() });
                    // Пожелания Эльканова Р.Д. - он хочет чтобы ФИО подписанта выходила в договоре и допнике. 
                    // Притом в формате Фамилия И.О., поэтому проводим такие махинации, меняя местами ИО и фамилию
                    if (_dt.Rows[i][1].ToString().IndexOf(" ") > 0)
                        _fio_Signature = _dt.Rows[i][1].ToString().Substring(_dt.Rows[i][1].ToString().LastIndexOf(" ")) + " " +
                            _dt.Rows[i][1].ToString().Substring(0, _dt.Rows[i][1].ToString().LastIndexOf(" "));
                    else
                        _fio_Signature = _dt.Rows[i][1].ToString();
                    if (_dt.Rows[i][2].ToString() == "1")
                        _sign_Dir_Sugnature = 1;
                }                
                OracleCommand _ocProj_Trans_ID = new OracleCommand(string.Format(
                    @"SELECT (select PROJECT_TRANSFER_ID from {0}.PROJECT_TRANSFER PT WHERE PT.TRANSFER_ID = :p_TRANSFER_ID) FROM DUAL",
                    Connect.Schema), Connect.CurConnect);
                _ocProj_Trans_ID.BindByName = true;
                _ocProj_Trans_ID.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID;
                object _proj_trans_ID = _ocProj_Trans_ID.ExecuteScalar();
                if (_proj_trans_ID != null)
                {
                    // Доп.соглашение
                    OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepEmp_Contract.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daOrder_Hire.SelectCommand.BindByName = true;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value = _proj_trans_ID;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Decimal).Value =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM;
                    _daOrder_Hire.Fill(_dsReport.Tables["ORDER_EMP"]);
                    if (_dsReport.Tables["ORDER_EMP"].Rows.Count > 0)
                    {
                        // Заполняем поле SIGN_DIR_SIGNATURE
                        _dsReport.Tables["ORDER_EMP"].Rows[0]["SIGN_DIR_SIGNATURE"] = _sign_Dir_Sugnature;
                        _dsReport.Tables["ORDER_EMP"].Rows[0]["FIO_SIGNATURE"] = _fio_Signature;
                        // Условия труда для допника
                        _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepWorking_Conditions.sql"),
                            Connect.Schema), Connect.CurConnect);
                        _daOrder_Hire.SelectCommand.BindByName = true;
                        _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value = _proj_trans_ID;
                        _daOrder_Hire.Fill(_dsReport.Tables["ORDER_EMP_COND"]);
                        // Согласование
                        _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("TP/RepApprovalForContract.sql"),
                            Connect.Schema), Connect.CurConnect);
                        _daOrder_Hire.SelectCommand.BindByName = true;
                        _daOrder_Hire.SelectCommand.Parameters.Add("p_PROJECT_TRANSFER_ID", OracleDbType.Decimal).Value = _proj_trans_ID;
                        _daOrder_Hire.Fill(_dsReport.Tables["APPROVAL"]);
                        // Вывод отчета на экран
                        if (MessageBox.Show("Вывести отчет на формате А3?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            ReportViewerWindow reportContract = new ReportViewerWindow(
                                "Трудовой договор", "Reports/Emp_ContractA3.rdlc", _dsReport,
                                new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                            );
                            reportContract.Show();
                        }
                        else
                        {
                            ReportViewerWindow reportContract = new ReportViewerWindow(
                                "Трудовой договор", "Reports/Emp_Contract.rdlc", _dsReport,
                                new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                            );
                            reportContract.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данных по трудовому договору нет!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    }
                    else
                    {
                        MessageBox.Show("Данных по трудовому договору нет!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
