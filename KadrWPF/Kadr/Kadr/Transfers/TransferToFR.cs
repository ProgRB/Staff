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
using LibraryKadr;
using System.Globalization;
using System.Data.Odbc;


namespace Kadr
{
    public partial class TransferToFR : Form
    {
        CHAR_WORK_seq char_work;
        TARIFF_GRID_seq tariff_grid;
        DEGREE_seq degree;
        TRANSFER_seq transfer, transferPrev;
        TRANSFER_obj r_transfer;
        ACCOUNT_DATA_seq account, accountPrev;
        ACCOUNT_DATA_obj r_account;
        EMP_seq emp;
        //SUBDIV_seq subdiv;
        //POSITION_seq position;
        BASE_DOC_seq base_doc;
        REASON_DISMISS_seq reason;
        Form formOrder;
        ListEmp listEmp;
        /// <summary>
        /// Логические переменные определяют следующие параметры:
        /// flagSave - признак сохранения данных в базе данных
        /// flagOrder - признак формирования номера приказа об увольнении или переводе через отдел кадров
        /// flagEmp - признак определяет можно ли просматривать приказ о приеме
        /// </summary>
        public bool flagDismiss, flagSave, flagOrder, flagEmp;
        public string numOrderPer;

        /// <summary>
        /// Конструктор формы перевода
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_emp">Таблица работников</param>
        /// <param name="_transfer">Таблица текущей записи в переводах сотрудника</param>
        /// <param name="_transferPrev">Таблица предыдущей записи в переводах сотрудника</param>
        /// <param name="_flagDismiss">Признак определяет что выполняется в форме - если истина, 
        /// то увольнение, если ложь, то перевод</param>
        /// <param name="_flagEdit">Признак отключает функцию редактирования данных если это не текущая запись в переводах 
        /// сотрудника</param>
        /// <param name="_flagProsm">Признак отключает функцию просмотра приказа если это первая(приемная) запись в переводах 
        /// сотрудника</param>
        /// <param name="_listEmp">Форма из которой была вызвана данная форма</param>
        public TransferToFR(EMP_seq _emp, TRANSFER_seq _transfer, TRANSFER_seq _transferPrev,
            ACCOUNT_DATA_seq _account, ACCOUNT_DATA_seq _accountPrev, bool _flagDismiss, bool _flagEdit, bool _flagProsm, 
            ListEmp _listEmp)
        {
            InitializeComponent();
            emp = _emp;
            transfer = _transfer;
            account = _account;
            transferPrev = _transferPrev;
            accountPrev = _accountPrev;
            flagDismiss = _flagDismiss;            
            listEmp = _listEmp; 
            r_transfer = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
            if (flagDismiss)
            {
                account.Fill(string.Format("where {0} = {1}", ACCOUNT_DATA_seq.ColumnsName.TRANSFER_ID,
                    r_transfer.FROM_POSITION));
            }
            if (account.Count != 0)
                r_account = (ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current;
            else
                r_account = null;

            char_work = new CHAR_WORK_seq(Connect.CurConnect);
            char_work.Fill(string.Format("order by {0}", CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME.ToString()));
            tariff_grid = new TARIFF_GRID_seq(Connect.CurConnect);
            tariff_grid.Fill(string.Format("order by {0}", TARIFF_GRID_seq.ColumnsName.CODE_TARIFF_GRID.ToString()));
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME.ToString()));

            cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID, new LinkArgument(FormMain.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            tbCode_Subdiv.Text = FormMain.subdiv.Where(s => s.SUBDIV_ID.ToString() == cbSubdiv_Name.SelectedValue.ToString()).FirstOrDefault().CODE_SUBDIV.ToString();
            cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID, new LinkArgument(FormMain.position, POSITION_seq.ColumnsName.POS_NAME));
            tbCode_Pos.Text = FormMain.position.Where(s => s.POS_ID.ToString() == cbPos_Name.SelectedValue.ToString()).FirstOrDefault().CODE_POS.ToString();
            cbDegree_Name.AddBindingSource(transfer, DEGREE_seq.ColumnsName.DEGREE_ID, new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            tbCode_Degree.Text = degree.Where(s => s.DEGREE_ID.ToString() == cbDegree_Name.SelectedValue.ToString()).FirstOrDefault().CODE_DEGREE.ToString();

            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);

            DisableControl.DisableAll(this, false, Color.White);

            base_doc = new BASE_DOC_seq(Connect.CurConnect);
            base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME.ToString()));
            reason = new REASON_DISMISS_seq(Connect.CurConnect);
            reason.Fill(string.Format("order by {0}", REASON_DISMISS_seq.ColumnsName.REASON_NAME.ToString()));

            tbPer_Num.AddBindingSource(emp, EMP_seq.ColumnsName.PER_NUM);   
            cbBase_doc.AddBindingSource(transfer, BASE_DOC_seq.ColumnsName.BASE_DOC_ID, new LinkArgument(base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
            cbReason_dismiss.AddBindingSource(transfer, REASON_DISMISS_seq.ColumnsName.REASON_ID, new LinkArgument(reason, REASON_DISMISS_seq.ColumnsName.REASON_NAME));
            tbTr_Num_Dismiss.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_NUM_ORDER);
            deTransferD.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_TRANSFER);
            deTr_Date_Dismiss.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_DATE_ORDER);

            cbChan_SignDismiss.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.CHAN_SIGN);
            if (r_transfer.TR_DATE_ORDER != null && r_transfer.TR_NUM_ORDER != "" && cbBase_doc.SelectedValue != null &&
            cbReason_dismiss.SelectedValue != null && r_transfer.DATE_TRANSFER != null)
            {
                btPreview.Enabled = true;
            }
            else
            {
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN = false;                    
            }

            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER == null)
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER = DateTime.Now;
            btEdit.Enabled = true;
            btExit.Enabled = true;         

            if (!_flagProsm)
            { 
                btPreview.Visible = false; 
                btEdit.Location = new Point(337, 6);
                btSave.Location = new Point(470, 6);
            }
            if (!_flagEdit)
            {
                btEdit.Visible = false;
                btSave.Visible = false;
            }
            if (FormMain.flagArchive)
            {
                btEdit.Enabled = false;
            }
            if (r_transfer.FROM_POSITION == null)
            {
                btPreview.Visible = false;
                btEdit.Visible = false;
                btSave.Visible = false;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEdit_Click(object sender, EventArgs e)
        {  
            DisableControl.Disable(gbDismiss, true, Color.White);
            tbTr_Num_Dismiss.Enabled = false;
            btExit.Text = "Отмена";
            btEdit.Enabled = false;
            btPreview.Enabled = false;
            btSave.Enabled = true;
            tbPer_Num.Enabled = false;
        }

        /// <summary>
        /// Событие нажатия кнопки сохранения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            ((CurrencyManager)BindingContext[transfer]).EndCurrentEdit();          
            if (tbTr_Num_Dismiss.Text == "" && cbChan_SignDismiss.Checked)
            {
                MessageBox.Show("Вы не присвоили значение номеру приказа об увольнении!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbTr_Num_Dismiss.Focus();
                return;
            }
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER == null)
            {
                MessageBox.Show("Вы не указали дату приказа об увольнении!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deTr_Date_Dismiss.Focus();
                return;
            }
            if (cbBase_doc.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали основание!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbBase_doc.Focus();
                return;
            }
            if (cbReason_dismiss.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали причину увольнения!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbReason_dismiss.Focus();
                return;
            }
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER == null)
            {
                MessageBox.Show("Вы не указали дату увольнения!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deTransferD.Focus();
                return;
            }
            DisableControl.Disable(gbDismiss, false, Color.White);
            if (!((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN)
            { 
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_NUM_ORDER == "")
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_NUM_ORDER = 
                        Library.NumberDoc(Staff.DataSourceScheme.SchemeName, string.Format("sknuvol_{0}_seq", DateTime.Now.Year.ToString()),
                            "nextval", Connect.CurConnect).ToString();
            }
            if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_DISMISS == null)
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_DISMISS = DateTime.Now;
            /// Устанавливаем признак текущей работы
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = false;

            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.AddHours(DateTime.Now.Hour);
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.AddMinutes(DateTime.Now.Minute);
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.AddSeconds(DateTime.Now.Second);
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER = ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER.Value.AddMilliseconds(DateTime.Now.Millisecond);
            
            /// Проводим необходимые действия, чтобы выставить признак текущей работы у прошлого перевода.
            TRANSFER_seq transferPrev = new TRANSFER_seq(Connect.CurConnect);
            transferPrev.Fill(string.Format("where {0} = {1}", TRANSFER_seq.ColumnsName.TRANSFER_ID,
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).FROM_POSITION));
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SIGN_CUR_WORK = false;
            transferPrev.Save();
            /// Сохраняем все изменения
            transfer.Save();
            ((CurrencyManager)BindingContext[transfer]).Refresh();

            EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
            FR_EMP_seq frEmp = new FR_EMP_seq(Connect.CurConnect);
            frEmp.Fill(string.Format("where {0} = {1}", FR_EMP_seq.ColumnsName.PERCO_SYNC_ID, r_empNew.PERCO_SYNC_ID));
            if (frEmp.Count == 0)
            {
                frEmp.AddNew();
                ((FR_EMP_obj)(((CurrencyManager)BindingContext[frEmp]).Current)).PERCO_SYNC_ID = r_empNew.PERCO_SYNC_ID;
            }
            ((FR_EMP_obj)(((CurrencyManager)BindingContext[frEmp]).Current)).FR_LAST_NAME = r_empNew.EMP_LAST_NAME;
            ((FR_EMP_obj)(((CurrencyManager)BindingContext[frEmp]).Current)).FR_FIRST_NAME = r_empNew.EMP_FIRST_NAME;
            ((FR_EMP_obj)(((CurrencyManager)BindingContext[frEmp]).Current)).FR_MIDDLE_NAME = r_empNew.EMP_MIDDLE_NAME;
            ((FR_EMP_obj)(((CurrencyManager)BindingContext[frEmp]).Current)).SUBDIV_ID = 0;
            ((FR_EMP_obj)(((CurrencyManager)BindingContext[frEmp]).Current)).POS_ID = 0;

            frEmp.Save();
            FormMain.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(), "",
                r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME, "", ""));

            TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;

            OdbcConnection odbcCon = new OdbcConnection("");
            odbcCon.ConnectionString = string.Format(
                @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                ParVal.Vals["PODOT"]);
            odbcCon.Open();
            OdbcCommand _rezult = new OdbcCommand("", odbcCon);
            _rezult.CommandText = string.Format("update podot set dat_uv = {0} where tnom = '{1}'",
                "{^" + r_transferNew.DATE_TRANSFER.Value.Year + "-" + r_transferNew.DATE_TRANSFER.Value.Month +
                "-" + r_transferNew.DATE_TRANSFER.Value.Day + "}", r_empNew.PER_NUM);
            if (!r_transferNew.SIGN_COMB)
            {
                _rezult.ExecuteNonQuery();
            }
            odbcCon.Close();
            /*Обновление ALSPR*/
            odbcCon.ConnectionString = string.Format(
                @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                ParVal.Vals["ALSPR"]);
            odbcCon.Open();
            _rezult.CommandText = string.Format("update ALSPR set duv = {0} where tn = '{1}' and p_rab = '{2}'",
                "{^" + r_transferNew.DATE_TRANSFER.Value.Year + "-" + r_transferNew.DATE_TRANSFER.Value.Month +
                "-" + r_transferNew.DATE_TRANSFER.Value.Day + "}", r_empNew.PER_NUM,
                r_transferNew.SIGN_COMB ? "2" : "");
            _rezult.ExecuteNonQuery();
            odbcCon.Close();
            /// При увольнении работника нужно обновить старую запись в SPR, установив значения 
            /// в полях FIO, DAT_UV
            odbcCon.ConnectionString = string.Format(
                @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                ParVal.Vals["SPR"]);
            odbcCon.Open();
            _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_UV = {3} " +
                "where podr = '{4}' and tnom = '{0}' and p_rab = '{1}'",
                r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "",
                string.Format("{0} {1} {2}", r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME.Substring(0, 1),
                    r_empNew.EMP_MIDDLE_NAME.Substring(0, 1)).Trim().PadRight(21, ' ') + "*",
                "{^" + r_transferNew.DATE_TRANSFER.Value.Year.ToString() + "-" +
                    r_transferNew.DATE_TRANSFER.Value.Month.ToString().PadLeft(2, '0') + "-" +
                    r_transferNew.DATE_TRANSFER.Value.Day.ToString().PadLeft(2, '0') + "}",
                tbCode_Subdiv.Text
                );
            _rezult.ExecuteNonQuery();
            odbcCon.Close();

            Connect.Commit();
            btExit.Text = "Выход";
            flagSave = true;
            btSave.Enabled = false;
            btEdit.Enabled = true;
            btExit.Enabled = true;
            btPreview.Enabled = true;
        }

        /// <summary>
        /// Событие нажатия кнопки просмотра приказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPreview_Click(object sender, EventArgs e)
        {
            if (!flagDismiss)
            {
                string formsalary, formsalaryPrev, pos_namePrev;
                string subdivPrev, posPrev, degreePrev, podpis;
                string dayTrDateOrder, monthTrDateOrder, yearTrDateOrder;
                EMP_obj r_emp = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                ACCOUNT_DATA_obj r_account_data;
                if (account.Count != 0)
                    r_account_data = (ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current;
                else
                    r_account_data = null;
                ACCOUNT_DATA_obj r_account_dataPrev;
                if (accountPrev.Count != 0)
                    r_account_dataPrev = (ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[accountPrev]).Current;
                else
                    r_account_dataPrev = null;
                TRANSFER_obj r_transfer = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                TRANSFER_obj r_transferPrev = (TRANSFER_obj)((CurrencyManager)BindingContext[transferPrev]).Current;
                if (tbCode_Degree.Text.Replace("0", "") == "1" || tbCode_Degree.Text.Replace("0", "") == "2")
                { formsalary = "Сдельная"; }
                else
                { formsalary = "Повременная"; }
                degreePrev = degree.Where(i => i.DEGREE_ID == r_transferPrev.DEGREE_ID).FirstOrDefault().CODE_DEGREE;
                subdivPrev = FormMain.subdiv.Where(i => i.SUBDIV_ID == r_transferPrev.SUBDIV_ID).FirstOrDefault().CODE_SUBDIV;
                posPrev = FormMain.position.Where(i => i.POS_ID == r_transferPrev.POS_ID).FirstOrDefault().CODE_POS;
                if (degreePrev.Replace("0", "") == "1" || degreePrev.Replace("0", "") == "2")
                { formsalaryPrev = "Сдельная"; }
                else
                { formsalaryPrev = "Повременная"; }
                if (r_transfer.TR_DATE_ORDER != null)
                {
                    dayTrDateOrder = r_transfer.TR_DATE_ORDER.Value.Day.ToString();
                    monthTrDateOrder = Library.MyMonthName(r_transfer.TR_DATE_ORDER.Value);
                    yearTrDateOrder = r_transfer.TR_DATE_ORDER.Value.Year.ToString();
                }
                else
                { dayTrDateOrder = monthTrDateOrder = yearTrDateOrder = ""; }
                OracleDataTable dataTable = new OracleDataTable(string.Format(Queries.GetQuery("SelectDirector.sql"),
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_LAST_NAME, EMP_seq.ColumnsName.EMP_FIRST_NAME,
                    EMP_seq.ColumnsName.EMP_MIDDLE_NAME, EMP_seq.ColumnsName.PER_NUM,
                    "director", DIRECTOR_seq.ColumnsName.DIR_CODE_POS, 23830), Connect.CurConnect);
                dataTable.Fill();
                if (dataTable.Rows.Count != 0)
                {
                    podpis = dataTable.Rows[0][0].ToString() + " " + dataTable.Rows[0][1].ToString() + "." + dataTable.Rows[0][2].ToString() + ".";
                }
                else
                {
                    podpis = "";
                } 
                List<string> slova = Slova(cbPos_Name.Text, ' ');
                List<string> arrayPos = ArraySlov(slova, 20, 38);
                pos_namePrev = FormMain.position.Where(i => i.POS_ID == r_transferPrev.POS_ID).FirstOrDefault().POS_NAME;
                slova = Slova(pos_namePrev, ' ');
                List<string> arrayPosPrev = ArraySlov(slova, 20, 38);
                CellParameter[] cellParameters = new CellParameter[] { 
                new CellParameter(7, 7, dayTrDateOrder, null), new CellParameter(7, 12, monthTrDateOrder, null), 
                new CellParameter(7, 23, yearTrDateOrder, null), new CellParameter(7,33, r_transfer.TR_NUM_ORDER, null),
                new CellParameter(23, 54, r_transfer.DATE_TRANSFER != null ? r_transfer.DATE_TRANSFER.Value.ToShortDateString() : "", null), 
                new CellParameter(26, 54, r_transfer.DATE_END_CONTR != null ? r_transfer.DATE_END_CONTR.Value.ToShortDateString() : "", null), 
                new CellParameter(30, 56, r_emp.PER_NUM, null), new CellParameter(32, 4, r_emp.EMP_LAST_NAME, null), 
                new CellParameter(32, 25, r_emp.EMP_FIRST_NAME, null), new CellParameter(32, 45, r_emp.EMP_MIDDLE_NAME, null),
                new CellParameter(34, 9, subdivPrev, null), new CellParameter(34, 41, tbCode_Subdiv.Text, null),
                new CellParameter(37, 14, arrayPosPrev[0].Trim(), null), new CellParameter(40, 1, arrayPosPrev[1].Trim(), null),
                new CellParameter(42, 1, arrayPosPrev[2], null),
                new CellParameter(37, 47, arrayPos[0].Trim(), null), new CellParameter(40, 34, arrayPos[1].Trim(), null),
                new CellParameter(42, 34, arrayPos[2], null),
                new CellParameter(44, 8, r_account_dataPrev != null ? r_account_dataPrev.CLASSIFIC != null ? r_account_dataPrev.CLASSIFIC.Value.ToString() : "" : "", null),
                new CellParameter(44, 41, r_account_data != null ? r_account_data.CLASSIFIC != null ? r_account_data.CLASSIFIC.Value.ToString() : "" : "", null),
                new CellParameter(46, 13, r_account_dataPrev != null ? r_account_dataPrev.SALARY.Value.ToString() : "", null),
                new CellParameter(46, 46, r_account_data != null ? r_account_data.SALARY.Value.ToString() : "", null),
                new CellParameter(48, 25, posPrev, null),
                new CellParameter(48, 58, tbCode_Pos.Text, null), new CellParameter(50, 14, formsalaryPrev, null),
                new CellParameter(50, 47, formsalary, null), new CellParameter(52, 11, degreePrev, null),
                new CellParameter(52, 44, tbCode_Degree.Text, null), new CellParameter(63, 48, podpis, null)                
                };
                Excel.PrintR1C1("Transfer.xlt", cellParameters);
            }
            else
            {
                string dayTrDateOrder, monthTrDateOrder, yearTrDateOrder, dayDismiss, monthDismiss, yearDismiss;
                string reason_article = "";
                string reason_order = "";
                string podpis;

                CellParameter parameterPodpis;
                /// Изменения от 22.06.2010
                /// Утвердили форму приказа об увольнении
                //if (tbCode_Degree.Text == "04")
                //{
                //    OracleDataTable dataTable = new OracleDataTable(string.Format(Queries.GetQuery("SelectDirector.sql"),
                //        Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_LAST_NAME, EMP_seq.ColumnsName.EMP_FIRST_NAME,
                //        EMP_seq.ColumnsName.EMP_MIDDLE_NAME, EMP_seq.ColumnsName.PER_NUM,
                //        "director", DIRECTOR_seq.ColumnsName.DIR_CODE_POS, 23830), connection);
                //    dataTable.Fill();
                //    if (dataTable.Rows.Count != 0)
                //    {
                //        podpis = dataTable.Rows[0][0].ToString() + " " + dataTable.Rows[0][1].ToString() + "." + dataTable.Rows[0][2].ToString() + ".";
                //    }
                //    else
                //    {
                //        podpis = "";
                //    }
                //    parameterPodpis = new CellParameter(38, 48, podpis, null);
                //}
                //else
                //{
                //    OracleDataTable dataTable = new OracleDataTable(string.Format(Queries.GetQuery("SelectDirector.sql"),
                //        Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_LAST_NAME, EMP_seq.ColumnsName.EMP_FIRST_NAME,
                //        EMP_seq.ColumnsName.EMP_MIDDLE_NAME, EMP_seq.ColumnsName.PER_NUM,
                //        "director", DIRECTOR_seq.ColumnsName.DIR_CODE_POS, 25880), connection);
                //    dataTable.Fill();
                //    if (dataTable.Rows.Count != 0)
                //    {
                //        podpis = dataTable.Rows[0][0].ToString() + " " + dataTable.Rows[0][1].ToString() + "." + dataTable.Rows[0][2].ToString() + ".";
                //    }
                //    else
                //    {
                //        podpis = "";
                //    }
                //    parameterPodpis = new CellParameter(37, 41, podpis, null);
                //}
                OracleDataTable dataTable = new OracleDataTable(string.Format(Queries.GetQuery("SelectDirector.sql"),
                    Staff.DataSourceScheme.SchemeName, "emp", EMP_seq.ColumnsName.EMP_LAST_NAME, EMP_seq.ColumnsName.EMP_FIRST_NAME,
                    EMP_seq.ColumnsName.EMP_MIDDLE_NAME, EMP_seq.ColumnsName.PER_NUM,
                    "director", DIRECTOR_seq.ColumnsName.DIR_CODE_POS, 23830), Connect.CurConnect);
                dataTable.Fill();
                if (dataTable.Rows.Count != 0)
                {
                    podpis = dataTable.Rows[0][0].ToString() + " " + dataTable.Rows[0][1].ToString() + "." + dataTable.Rows[0][2].ToString() + ".";
                }
                else
                {
                    podpis = "";
                }
                parameterPodpis = new CellParameter(38, 48, podpis, null);

                EMP_obj r_emp = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transfer = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                ACCOUNT_DATA_obj r_account_data;
                if (account.Count != 0)
                    r_account_data = (ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current;
                else
                    r_account_data = null;
                if (r_transfer.TR_DATE_ORDER != null)
                {
                    dayTrDateOrder = r_transfer.TR_DATE_ORDER.Value.Day.ToString();
                    monthTrDateOrder = Library.MyMonthName(r_transfer.TR_DATE_ORDER.Value);
                    yearTrDateOrder = r_transfer.TR_DATE_ORDER.Value.Year.ToString();
                }
                else
                { dayTrDateOrder = monthTrDateOrder = yearTrDateOrder = ""; }
                if (r_transfer.DATE_TRANSFER != null)
                {
                    dayDismiss = r_transfer.DATE_TRANSFER.Value.Day.ToString();
                    monthDismiss = Library.MyMonthName(r_transfer.DATE_TRANSFER.Value);
                    yearDismiss = r_transfer.DATE_TRANSFER.Value.Year.ToString();
                }
                else
                { dayDismiss = monthDismiss = yearDismiss = ""; }
                if (r_transfer.REASON_ID != null)
                {
                    reason_article = reason.Where(i => i.REASON_ID == r_transfer.REASON_ID).FirstOrDefault().REASON_ARTICLE;
                    reason_order = reason.Where(i => i.REASON_ID == r_transfer.REASON_ID).FirstOrDefault().REASON_ORDER;
                }
                string prefix = r_transfer.CHAN_SIGN ? "/К" : "/У";
                List<string> slova = Slova(reason_order, ' ');
                List<string> arrayPos = ArraySlov(slova, 65, 65);
                CellParameter[] cellParameters = new CellParameter[] { 
                new CellParameter(7, 7, dayTrDateOrder, null), new CellParameter(7, 12, monthTrDateOrder, null), 
                new CellParameter(7, 23, yearTrDateOrder, null), 
                new CellParameter(7,33, r_transfer.TR_NUM_ORDER + prefix, null),
                new CellParameter(14, 54, r_transfer.CONTR_EMP, null), 
                new CellParameter(15, 54, r_transfer.DATE_CONTR != null ? r_transfer.DATE_CONTR.Value.ToShortDateString() : "", null), 
                new CellParameter(16, 11, dayDismiss, null), new CellParameter(16, 16, monthDismiss, null), 
                new CellParameter(16, 26, yearDismiss, null),                
                new CellParameter(17, 56, r_emp.PER_NUM, null), new CellParameter(18, 1, r_emp.EMP_LAST_NAME + " " + 
                    r_emp.EMP_FIRST_NAME + " " + r_emp.EMP_MIDDLE_NAME, null),
                new CellParameter(20, 20, tbCode_Subdiv.Text, null), new CellParameter(21, 1, cbSubdiv_Name.Text, null), 
                new CellParameter(23, 1, cbPos_Name.Text, null), 
                new CellParameter(25, 8, r_account_data != null ? r_account_data.CLASSIFIC != null ? r_account_data.CLASSIFIC.Value.ToString() : "" : "", null), 
                new CellParameter(26, 11, cbDegree_Name.Text, null), new CellParameter(26, 61, tbCode_Degree.Text, null), 
                new CellParameter(29, 25, cbBase_doc.Text, null), new CellParameter(30, 1, reason_article, null), 
                new CellParameter(31, 1, arrayPos[0].Trim(), null), new CellParameter(32, 1, arrayPos[1].Trim(), null), 
                new CellParameter(33, 1, arrayPos[2].Trim(), null), 
                new CellParameter(43, 39, dayTrDateOrder, null), 
                new CellParameter(43, 44, monthTrDateOrder, null),
                new CellParameter(43, 55, yearTrDateOrder, null), parameterPodpis
                };

                /// Изменения от 22.06.2010
                /// Утвердили форму приказа об увольнении
                //if (tbCode_Degree.Text == "04")
                //{                    
                //    Excel.PrintR1C1("DismissS.xlt", cellParameters);
                //}
                //else
                //{
                //    Excel.PrintR1C1("DismissR.xlt", cellParameters);
                //}

                Excel.PrintR1C1("Dismiss.xlt", cellParameters);
            }
        }

        /// <summary>
        /// Событие нажатия кнопки закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                Close();
            }
            else
            {
                emp.RollBack();
                emp.ResetBindings();
                transfer.Clear();
                transfer.Add(r_transfer);
                Connect.Rollback();
                btExit.Text = "Выход";
                DisableControl.DisableAll(this, false, Color.White);
                btEdit.Enabled = true;
                btExit.Enabled = true;
                if (r_transfer.TR_DATE_ORDER != null && tbTr_Num_Dismiss.Text != "" && cbBase_doc.SelectedValue != null &&
                cbReason_dismiss.SelectedValue != null && r_transfer.DATE_TRANSFER != null)
                {
                    btPreview.Enabled = true;
                }
            }
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

        RadioButton rbOtdel;
        RadioButton rbKanc;
        Button btOk;
        /// <summary>
        /// Событие получения фокуса текстбоксом номера приказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTr_Num_Order_Enter(object sender, EventArgs e)
        {
            //CreateForm();
            //btOk.Name = "sknper";
            //btOk.Click += new EventHandler(btOk_Click);
            //formOrder.Text = "Выбор приказа о переводе";
            //formOrder.ShowDialog();
        }

        /// <summary>
        /// Событие нажатия кнопки выбора типа приказа о переводе (отдел кадров или канцелярия)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btOk_Click(object sender, EventArgs e)
        {
            //if (rbOtdel.Checked)
            //{
            //    NumberOrder();                  
            //    tbTr_Num_Order.Text = numOrderPer;
            //    tbTr_Num_Order.ReadOnly = true;
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;
            //    deTr_Date_Order.Text = DateTime.Now.ToShortDateString();
            //}
            //else
            //{
            //    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN = true;
            //    tbTr_Num_Order.ReadOnly = false;
            //    tbTr_Num_Order.Text = "";
            // }
            //deTr_Date_Order.Focus();
            //formOrder.Close();
        }

        /// <summary>
        /// Событие нажатия кнопки выбора типа приказа об увольнении (отдел кадров или канцелярия)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btOkDismiss_Click(object sender, EventArgs e)
        {
            if (rbOtdel.Checked)
            {
                NumberOrder();
                tbTr_Num_Dismiss.Text = numOrderPer;
                tbTr_Num_Dismiss.ReadOnly = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;
                deTr_Date_Dismiss.Text = DateTime.Now.ToShortDateString();
            }
            else
            {
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN = true;
                tbTr_Num_Dismiss.ReadOnly = false;
                tbTr_Num_Dismiss.Text = "";
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER = null;
                deTr_Date_Dismiss.Text = "";
            }
            deTr_Date_Dismiss.Focus();
            formOrder.Close();
        }

        /// <summary>
        /// Метод получает номер приказа
        /// </summary>
        void NumberOrder()
        {
            if (!flagOrder)
            {
                numOrderPer = (Library.NumberDoc(Staff.DataSourceScheme.SchemeName, string.Format("{0}_{1}_seq", btOk.Name, DateTime.Now.Year.ToString()), "nextval", Connect.CurConnect)).ToString();
                flagOrder = true;
            }
            ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN = false;
        }

        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Transfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!flagSave && flagOrder)
            {
                DropSequanceTransfer();
            }
            if (flagSave)
            {
                listEmp.PositionChange(listEmp.dgEmp, e);
            }
        }

        /// <summary>
        /// Функция разбивает строку на слова по указанному разделителю
        /// </summary>
        /// <param name="_string">Строка, которую нужно разбить</param>
        /// <param name="_char">Символ, который служит разделителем</param>
        /// <returns>Список слов</returns>
        List<string> Slova(string _string, char _char)
        {
            List<string> slova = new List<string>();
            StringBuilder symbol = new StringBuilder();
            for ( int i = 0; i < _string.Length; i++)
            {
                if (_string[i] != _char)
                {
                    symbol.Append(_string[i]);
                }
                else
                {
                    slova.Add(symbol.ToString());
                    symbol = new StringBuilder();
                }
            }
            slova.Add(symbol.ToString());
            return slova;
        }

        /// <summary>
        /// Функция формирует 3 строки из массива слов
        /// </summary>
        /// <param name="slova">Список слов</param>
        /// <param name="len1">Длина первой строки</param>
        /// <param name="len2">Длина второй строки</param>
        /// <returns>Возвращает лист из 3 строк</returns>
        List<string> ArraySlov(List<string> slova, int len1, int len2)
        {
            List<string> arraySlov = new List<string>();
            arraySlov.Add(slova[0]);
            arraySlov.Add("");
            arraySlov.Add("");
            bool flag1 = true, flag2 = true; 
            for (int i = 1; i < slova.Count; i++)
            {
                if (arraySlov[0].Trim().Length + slova[i].Length + 1 <= len1 && flag1)
                {
                    arraySlov[0] += " " + slova[i];                    
                }
                else if (arraySlov[1].Trim().Length + slova[i].Length + 1 <= len2 && flag2)
                {
                    flag1 = false;
                    arraySlov[1] += " " + slova[i];
                }
                else
                {
                    flag2 = false;
                    arraySlov[2] += " " + slova[i];
                }
            }
            return arraySlov;
        }

        /// <summary>
        /// Метод удаляет секванс по текущему номеру
        /// </summary>
        void DropSequanceTransfer()
        {
            string name_seq = string.Format("{0}_{1}_seq", btOk.Name, DateTime.Now.Year.ToString());
            int num = Library.NumberDoc(Staff.DataSourceScheme.SchemeName, name_seq, "currval", Connect.CurConnect);
            Library.DropSequance(Staff.DataSourceScheme.SchemeName, name_seq, Connect.CurConnect);
            Library.CreateSequance(Staff.DataSourceScheme.SchemeName, name_seq, num, Connect.CurConnect);
        }

        /// <summary>
        /// Метод создает форму для выбора типа приказа: отдел кадров или канцелярия
        /// </summary>
        void CreateForm()
        {
            formOrder = new Form();
            formOrder.StartPosition = FormStartPosition.CenterScreen;
            formOrder.Size = new Size(220, 145);
            formOrder.ControlBox = false;
            formOrder.ShowInTaskbar = false;
            formOrder.FormBorderStyle = FormBorderStyle.FixedDialog;
            GroupBox gb1 = new GroupBox();
            gb1.Dock = DockStyle.Top;
            gb1.Height = 70;
            formOrder.Controls.Add(gb1);
            rbOtdel = new RadioButton();
            rbKanc = new RadioButton();
            btOk = new Button();
            gb1.Controls.Add(rbOtdel);
            gb1.Controls.Add(rbKanc);
            formOrder.Controls.Add(btOk);
            Font font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            rbOtdel.Text = "Через отдел кадров";
            rbOtdel.Font = font;
            rbOtdel.Location = new Point(30, 13);
            rbOtdel.AutoSize = true;
            rbOtdel.Checked = true;
            rbKanc.Text = "Через канцелярию";
            rbKanc.Font = font;
            rbKanc.AutoSize = true;
            rbKanc.Location = new Point(30, 39);
            btOk.Text = "OK";
            btOk.Font = font;
            btOk.Location = new Point(69, 80);
            btOk.Size = new Size(82, 23);
            Color color = Color.FromArgb(0, 70, 213);
            btOk.ForeColor = color;            
        }

        /// <summary>
        /// Событие получения фокуса текстбоксом номера приказа об увольнении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTr_Num_Dismiss_Enter(object sender, EventArgs e)
        {
            //CreateForm();
            //btOk.Name = "sknuvol";
            //btOk.Click += new EventHandler(btOkDismiss_Click);
            //formOrder.Text = "Выбор приказа об увольнении";
            //formOrder.ShowDialog();
        }

        /// <summary>
        /// Проверка ввода символа. Если введена точка или запятая, возвращаем разделитель 
        /// в зависимости от языковых настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Convert.ToInt32(e.KeyChar) == 46 || Convert.ToInt32(e.KeyChar) == 44)
            //{
            //    e.KeyChar = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            //}
        }

        private void cbChan_SignDismiss_CheckedChanged(object sender, EventArgs e)
        {
            if (cbChan_SignDismiss.Checked)
            {
                tbTr_Num_Dismiss.Enabled = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = null;
            }
            else
            {
                tbTr_Num_Dismiss.Enabled = false;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = false;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;                
            }
            ((CurrencyManager)BindingContext[transfer]).Refresh();
        }

        private void cbChan_Sign_Click(object sender, EventArgs e)
        {
            //if (cbChan_Sign.Checked)
            //{
            //    tbTr_Num_Order.Enabled = true;
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = true;
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = null;
            //}
            //else
            //{
            //    tbTr_Num_Order.Enabled = false;
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = false;
            //    ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;
            //}
            //((CurrencyManager)BindingContext[transfer]).Refresh();
        }

        private void cbChan_SignDismiss_CheckedChanged_1(object sender, EventArgs e)
        {
            if (cbChan_SignDismiss.Checked)
            {
                tbTr_Num_Dismiss.Enabled = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = null;
            }
            else
            {
                tbTr_Num_Dismiss.Enabled = false;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = false;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;
            }
            ((CurrencyManager)BindingContext[transfer]).Refresh();
        }    
    }
}
