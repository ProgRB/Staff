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
using Kadr.Vacation_schedule;


namespace Kadr
{
    public partial class Transfer : Form
    {
        //CHAR_TRANSFER_seq char_transfer;
        //CHAR_WORK_seq char_work;
        TYPE_TERM_TRANSFER_seq type_Term_Transfer, type_Term_Transfer2;
        TARIFF_GRID_seq tariff_grid;
        DEGREE_seq degree;
        FORM_OPERATION_seq form_operation;
        FORM_PAY_seq form_pay;
        TRANSFER_seq transfer, transferPrev;
        TRANSFER_obj r_transfer;
        ACCOUNT_DATA_seq account, accountPrev;
        ACCOUNT_DATA_obj r_account;
        EMP_seq emp;
        //SUBDIV_seq subdiv;
        //POSITION_seq position;
        BASE_DOC_seq base_doc;
        REASON_DISMISS_seq reason;
        MAT_RESP_PERSON_seq mat_resp_person;
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
        /// Признак добавления перевода
        /// </summary>
        public static bool flagAdd;
        /// <summary>
        /// Код подразделения. Заполняется при входе в форму. Служит для обновления SPR.
        /// </summary>
        string code_subdiv;
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
        public Transfer(EMP_seq _emp, TRANSFER_seq _transfer, TRANSFER_seq _transferPrev,
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
                account.Fill(string.Format("where TRANSFER_ID = {0}", r_transfer.FROM_POSITION));
            }
            if (account.Count != 0)
                r_account = (ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current;
            else
                r_account = null;

            OracleCommand cmd_aud = new OracleCommand(string.Format(
                "begin APSTAFF.TABLE_AUDIT_EX_INSERT(:p_table_id, :p_type_kard); end;", Connect.Schema), Connect.CurConnect);
            cmd_aud.BindByName = true;
            cmd_aud.Parameters.Add("p_table_id", OracleDbType.Decimal, r_transfer.TRANSFER_ID, ParameterDirection.Input);
            cmd_aud.Parameters.Add("p_type_kard", OracleDbType.Varchar2, "TRANS", ParameterDirection.Input);
            cmd_aud.ExecuteNonQuery();

            //char_work = new CHAR_WORK_seq(Connect.CurConnect);
            //char_work.Fill(string.Format("order by {0}", CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME.ToString()));
            //char_transfer = new CHAR_TRANSFER_seq(Connect.CurConnect);
            //char_transfer.Fill("ORDER BY CHAR_TRANSFER_NAME");
            type_Term_Transfer = new TYPE_TERM_TRANSFER_seq(Connect.CurConnect);
            type_Term_Transfer.Fill("ORDER BY TYPE_TERM_TRANSFER_ID");
            type_Term_Transfer2 = new TYPE_TERM_TRANSFER_seq(Connect.CurConnect);
            type_Term_Transfer2.Fill("ORDER BY TYPE_TERM_TRANSFER_ID");
            tariff_grid = new TARIFF_GRID_seq(Connect.CurConnect);
            tariff_grid.Fill(string.Format("order by {0}", TARIFF_GRID_seq.ColumnsName.CODE_TARIFF_GRID.ToString()));
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME.ToString()));
            form_operation = new FORM_OPERATION_seq(Connect.CurConnect);
            form_operation.Fill(string.Format("order by {0}", FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            form_pay = new FORM_PAY_seq(Connect.CurConnect);
            form_pay.Fill("order by FORM_PAY");
            /*!!!Перезаполняем должности!!!*/
            FormMain.position.Clear();
            FormMain.position.Fill(string.Format("where POS_ACTUAL_SIGN = 1 " +
                "union " +
                "SELECT case POS_CHIEF_OR_DEPUTY_SIGN when 1 then 'True' else 'False' end as \"POS_CHIEF_OR_DEPUTY_SIGN\",POS_ID,CODE_POS,POS_NAME, " + 
                "case POS_ACTUAL_SIGN when 1 then 'True' else 'False' end as \"POS_ACTUAL_SIGN\", " +
                "POS_DATE_START,POS_DATE_END,FROM_POS_ID " + 
                "FROM {0}.POSITION tab1 where POS_ID = {1} " +
                "order by POS_NAME", Connect.Schema, r_transfer.POS_ID));
            FormMain.subdiv.Clear();
            FormMain.subdiv.Fill(string.Format(
                @"where SUB_ACTUAL_SIGN = 1 and WORK_TYPE_ID != 7 
                union 
                select TYPE_SUBDIV_ID,SUBDIV_ID,CODE_SUBDIV,SUBDIV_NAME, case SUB_ACTUAL_SIGN when 1 then 'True' else 'False' end as SUB_ACTUAL_SIGN,
                    WORK_TYPE_ID,SERVICE_ID,SUB_DATE_START,SUB_DATE_END,PARENT_ID,FROM_SUBDIV,GR_WORK_ID from {0}.SUBDIV tab1 where SUBDIV_ID = {1} 
                order by SUBDIV_NAME", Connect.Schema, r_transfer.SUBDIV_ID));                
            // Привязка компонентов
            cbSubdiv_Name.AddBindingSource(transfer, SUBDIV_seq.ColumnsName.SUBDIV_ID, new LinkArgument(FormMain.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            tbCode_Subdiv.Text = FormMain.subdiv.Where(s => s.SUBDIV_ID.ToString() == cbSubdiv_Name.SelectedValue.ToString()).FirstOrDefault().CODE_SUBDIV.ToString();
            cbPos_Name.AddBindingSource(transfer, POSITION_seq.ColumnsName.POS_ID, new LinkArgument(FormMain.position, POSITION_seq.ColumnsName.POS_NAME));
            tbCode_Pos.Text = FormMain.position.Where(s => s.POS_ID.ToString() == cbPos_Name.SelectedValue.ToString()).FirstOrDefault().CODE_POS.ToString();
            
            //cbChar_Work_ID.AddBindingSource(transfer, CHAR_WORK_seq.ColumnsName.CHAR_WORK_ID, new LinkArgument(char_work, CHAR_WORK_seq.ColumnsName.CHAR_WORK_NAME));
            //cbChar_Transfer.AddBindingSource(transfer, CHAR_TRANSFER_seq.ColumnsName.CHAR_TRANSFER_ID, new LinkArgument(char_transfer, CHAR_TRANSFER_seq.ColumnsName.CHAR_TRANSFER_NAME));
            cbChar_Work_ID.DataSource = type_Term_Transfer;
            cbChar_Work_ID.DisplayMember = "TYPE_TERM_TRANSFER_NAME";
            cbChar_Work_ID.ValueMember = "TYPE_TERM_TRANSFER_ID";
            cbChar_Work_ID.DataBindings.Add("SelectedValue", transfer, "CHAR_WORK_ID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            cbChar_Transfer.DataSource = type_Term_Transfer2;
            cbChar_Transfer.DisplayMember = "TYPE_TERM_TRANSFER_NAME";
            cbChar_Transfer.ValueMember = "TYPE_TERM_TRANSFER_ID";
            cbChar_Transfer.DataBindings.Add("SelectedValue", transfer, "CHAR_TRANSFER_ID", true, DataSourceUpdateMode.OnPropertyChanged, "");

            cbCode_Tariff_Grid.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.TARIFF_GRID_ID,
                new LinkArgument(tariff_grid, TARIFF_GRID_seq.ColumnsName.CODE_TARIFF_GRID));
            cbDegree_Name.AddBindingSource(transfer, DEGREE_seq.ColumnsName.DEGREE_ID, new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            tbCode_Degree.Text = degree.Where(s => s.DEGREE_ID.ToString() == cbDegree_Name.SelectedValue.ToString()).FirstOrDefault().CODE_DEGREE.ToString();
            cbSign_Comb.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.SIGN_COMB);
            tbPer_Num.AddBindingSource(emp, EMP_seq.ColumnsName.PER_NUM);   
            cbForm_Operation.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.FORM_OPERATION_ID, 
                new LinkArgument(form_operation, FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            tbCode_Form_Operation.Text = 
                form_operation.Where(s => s.FORM_OPERATION_ID.ToString() == cbForm_Operation.SelectedValue.ToString()).FirstOrDefault().CODE_FORM_OPERATION.ToString();
            cbForm_Pay.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.FORM_PAY,
                new LinkArgument(form_pay, FORM_PAY_seq.ColumnsName.NAME_FORM_PAY));
            tbSalary.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SALARY);
            tbClassific.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.CLASSIFIC);            
            tbSign_Add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SIGN_ADD);
            tbProff_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.PROF_ADDITION);
            tbHarmfull_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.HARMFUL_ADDITION);
            tbHarmfull_Addition_Add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.HARMFUL_ADDITION_ADD);
            tbComb_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.COMB_ADDITION);
            tbSecret_Addition.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.SECRET_ADDITION);
            tbPercent13.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.PERCENT13);
            tbContr_Empl.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.CONTR_EMP);
            tbHarmful_Vac.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.HARMFUL_VAC);
            tbPos_Note.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.POS_NOTE);
            if (account.Count != 0 &&
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).PRIVILEGED_POSITION_ID != null)
                chSign_Priv_Pos.Checked = true;            

            deDate_End_Contr.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_END_CONTR);
            deDate_Add.AddBindingSource(account, ACCOUNT_DATA_seq.ColumnsName.DATE_ADD);
            deDate_Contr.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_CONTR);

            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);
            cbForm_Operation.SelectedIndexChanged += new EventHandler(cbForm_Operation_SelectedIndexChanged);

            tbSalary.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbProff_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbHarmfull_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbComb_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);
            tbSecret_Addition.KeyPress += new KeyPressEventHandler(Library.InputSeparator);

            DisableControl.DisableAll(this, false, Color.White);

            if (flagDismiss)
            {
                base_doc = new BASE_DOC_seq(Connect.CurConnect);
                base_doc.Fill(string.Format("order by {0}", BASE_DOC_seq.ColumnsName.BASE_DOC_NAME.ToString()));
                reason = new REASON_DISMISS_seq(Connect.CurConnect);
                reason.Fill(string.Format("order by {0}", REASON_DISMISS_seq.ColumnsName.REASON_NAME.ToString()));
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
            }
            else
            {                
                tbTr_Num_Order.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_NUM_ORDER);
                deDate_Transfer.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.DATE_TRANSFER);
                deTr_Date_Order.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.TR_DATE_ORDER);
                try
                {
                    if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY == null)
                    {
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY =
                            ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[accountPrev]).Current).SALARY;
                    }
                    if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC == null)
                    {
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC =
                            ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[accountPrev]).Current).CLASSIFIC;
                    }
                }
                catch { }
                cbChan_Sign.AddBindingSource(transfer, TRANSFER_seq.ColumnsName.CHAN_SIGN);                
                if (r_transfer.DATE_TRANSFER != null && r_transfer.TR_NUM_ORDER != "" && cbChar_Work_ID.SelectedValue != null &&
                cbDegree_Name.SelectedValue != null && r_transfer.TR_DATE_ORDER != null)
                {
                    btPreview.Enabled = true;
                }
                else
                {
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN = false;
                }
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
            //if (FormMain.flagArchive)
            //{
            //    btEdit.Enabled = false;
            //}
            if (r_transfer.FROM_POSITION == null)
            {
                btPreview.Visible = false;
                btEdit.Visible = false;
                btSave.Visible = false;
            }

            if (GrantedRoles.GetGrantedRole("STAFF_PERSONNEL") //&& _flagEdit
                )
            {
                // 08.12.2016 Делаем кнопки видимыми для личного стола для возможности формировать документы по старым переводам
                btPreview.Visible = true;
                btViewAdd_Agreement.Visible = true;
                btViewAdd_Agreement.Enabled = true;
            }

            btMatching_List_Project.EnableByRules();
            if (btMatching_List_Project.Enabled)
                btMatching_List_Project.Visible = true;
            /// Настройка для работы с МОЛ
            mat_resp_person = new MAT_RESP_PERSON_seq(Connect.CurConnect);
            mat_resp_person.Fill(string.Format("where {0} = '{1}'", MAT_RESP_PERSON_seq.ColumnsName.PER_NUM,
                ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM));
            if (mat_resp_person.Count != 0)
            {
                chSign_MRP.Checked = true;
                chSign_MRP2.Checked = true;
            }

            code_subdiv = FormMain.subdiv.Where(s => s.SUBDIV_ID ==
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).SUBDIV_ID).FirstOrDefault().CODE_SUBDIV.ToString();

            // Выбираем признак УТК
            OracleCommand _ocTransfer_QM = new OracleCommand(string.Format(
                @"SELECT TRANSFER_QM_ID FROM {0}.TRANSFER_QM WHERE TRANSFER_ID = :p_TRANSFER_ID",
                Connect.Schema), Connect.CurConnect);
            _ocTransfer_QM.BindByName = true;
            _ocTransfer_QM.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TRANSFER_ID;
            if (_ocTransfer_QM.ExecuteReader().Read())
                chTransfer_QM.Checked = true;
        }
                
        /// <summary>
        /// Событие нажатия кнопки редактирования данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEdit_Click(object sender, EventArgs e)
        {
            if (chSign_MRP.Checked && flagAdd)
            {
                MessageBox.Show("Данный работник является материально-ответственным лицом!", 
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                /// 15.02.2013 - Служебка от отдела 7 чтобы убрать проверку признака МОЛ при переводах
                //if (!flagDismiss)
                //{
                //    MessageBox.Show("Нельзя сформировать приказ для материально-ответственного лица!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //else
                //{
                //    TRANSFER_seq transferCount = new TRANSFER_seq(Connect.CurConnect);
                //    transferCount.Fill(string.Format("where per_num = {0} and sign_cur_work = 1",
                //        ((EMP_obj)(((CurrencyManager)BindingContext[emp]).Current)).PER_NUM));
                //    if (transferCount.Count == 1)
                //    {
                //        MessageBox.Show("Нельзя сформировать приказ для материально-ответственного лица!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}
            }
            if (!flagDismiss)
            {
                DisableControl.DisableAll(this, true, Color.White);
                DisableControl.Disable(gbDismiss, false, Color.White);
                tbTr_Num_Order.Enabled = false;                
            }
            else
            {
                DisableControl.Disable(gbDismiss, true, Color.White);
                tbTr_Num_Dismiss.Enabled = false;
            }
            chSign_MRP.Enabled = false;
            chSign_MRP2.Enabled = false;
            btExit.Text = "Отмена";
            btEdit.Enabled = false;
            btPreview.Enabled = false;
            btSave.Enabled = true;
            tbPer_Num.Enabled = false;
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
            ((CurrencyManager)BindingContext[transfer]).EndCurrentEdit();
            ((CurrencyManager)BindingContext[account]).EndCurrentEdit();
            if (!flagDismiss)
            {
                if (cbSubdiv_Name.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали подразделение!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbChar_Work_ID.Focus();
                    return;
                }
                if (cbPos_Name.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали должность!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbChar_Work_ID.Focus();
                    return;
                }
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER == null)
                {
                    MessageBox.Show("Вы не указали дату перевода!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deDate_Transfer.Focus();
                    return;
                }
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER <= 
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE)
                {
                    MessageBox.Show("Вы указали дату перевода меньше даты приема!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deDate_Transfer.Focus();
                    return;
                }
                if (cbChar_Work_ID.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали Срок договора!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbChar_Work_ID.Focus();
                    return;
                }
                if (cbDegree_Name.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали категорию!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbDegree_Name.Focus();
                    return;
                }
                if (cbForm_Operation.SelectedValue == null)
                {
                    MessageBox.Show("Вы не выбрали вид производства!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbForm_Operation.Focus();
                    return;
                }
                if (cbForm_Pay.SelectedValue == null)
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
                if (tbTr_Num_Order.Text == "" && cbChan_Sign.Checked)
                {
                    MessageBox.Show("Вы не присвоили значение номеру приказа о переводе!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbTr_Num_Order.Focus();
                    return;
                }
                if (tbHarmful_Vac.Text == "")
                {
                    MessageBox.Show("Необходимо ввести количество дней дополнительного отпуска!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbHarmful_Vac.Focus();
                    return;
                }
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER == null)
                {
                    MessageBox.Show("Вы не указали дату приказа!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deTr_Date_Order.Focus();
                    return;
                }                
                if (!((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN)
                {
                    if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_NUM_ORDER == "")                        
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_NUM_ORDER =
                            Library.NumberDoc(Staff.DataSourceScheme.SchemeName, string.Format("sknper_{0}_seq", DateTime.Now.Year.ToString()),
                                "nextval", Connect.CurConnect).ToString();
                }
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_ORDER == null)
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_ORDER = DateTime.Now;
                /// Устанавливаем признак текущей работы
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = true;
            }
            else
            {                
                if (tbTr_Num_Dismiss.Text == "" && cbChan_SignDismiss.Checked)
                {
                    MessageBox.Show("Вы не присвоили значение номеру приказа об увольнении!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbTr_Num_Dismiss.Focus();
                    return;
                }
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_DATE_ORDER == null)
                {
                    MessageBox.Show("Вы не указали дату приказа об увольнении!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deTr_Date_Order.Focus();
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
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER <=
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_HIRE)
                {
                    MessageBox.Show("Вы указали дату увольнения меньше даты приема!\nПовторите ввод.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deTransferD.Focus();
                    return;
                }                
                if (!((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN)
                { 
                    if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_NUM_ORDER == "")
                        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).TR_NUM_ORDER = 
                            Library.NumberDoc(Staff.DataSourceScheme.SchemeName, string.Format("sknuvol_{0}_seq", DateTime.Now.Year.ToString()),
                                "nextval", Connect.CurConnect).ToString();
                }
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_DISMISS == null)
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_DISMISS = DateTime.Now;
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_ORDER == null)
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DF_BOOK_ORDER = DateTime.Now;
                /// Устанавливаем признак текущей работы
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SIGN_CUR_WORK = false;
            }

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
            Connect.Commit();
            /// Сохраняем все изменения
            transfer.Save();            
            Library.UpdateGR_Work(
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).PER_NUM.ToString(),
                Convert.ToInt32(((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID),
                Convert.ToInt32(((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).GR_WORK_ID),
                (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_TRANSFER,
                1);
            ((CurrencyManager)BindingContext[transfer]).Refresh();
            //if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CHANGE_DATE == null)
            //{
            //    ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CHANGE_DATE =
            //        ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).DATE_TRANSFER;
            //}
            /*Если у человека перевод, а не увольнение, то обновляем дату изменения*/
            if (!flagDismiss)
            {
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CHANGE_DATE = DateTime.Now;
                /* Если добавляем перевод, если стоит льготная профессия и если должности не равны, 
                 * то обнуляем льготную профессию*/
                if (flagAdd && ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).PRIVILEGED_POSITION_ID != null
                    && ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).POS_ID !=
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).POS_ID)
                {
                    ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).PRIVILEGED_POSITION_ID = null;
                }
            }
            /* При добавлении нового перевода и когда перевод в другое подразделение, нужно обнулять надбавки*/
            if (flagAdd && ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).SUBDIV_ID !=
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SUBDIV_ID)
            {
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).PROF_ADDITION = null;
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CHIEF_ADDITION = null;
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASS_ADDITION = null;
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SUM_YOUNG_SPEC = null;
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_END_YOUNG_SPEC = null;
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).TRIP_ADDITION = null;
                /* При смене подразделения и профессии нужно обнулять признак расчета гидропроцедур */
                if (((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).POS_ID !=
                    ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).POS_ID)
                {
                    ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).WATER_PROC = false;
                }
                // Обнуляем Группу расчета премии и процент премии за личное клеймо
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).PREMIUM_CALC_GROUP_ID = null;
            }
            account.Save();
            DisableControl.Disable(gbDismiss, false, Color.White);
            DisableControl.Disable(gbAll, false, Color.White);
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
                MessageBox.Show("Ошибка обновления признака УТК!\n"+
                    ex.Message, "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //

            if (!flagDismiss)
            {
                EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                FormMain.employees.UpdateEmployee(new PercoXML.Employee(r_empNew.PERCO_SYNC_ID.ToString(),
                    r_empNew.PER_NUM,
                    r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME, r_empNew.EMP_MIDDLE_NAME,
                    r_transferNew.SUBDIV_ID.ToString(), r_transferNew.POS_ID.ToString()));
                /*При переводе работника необходимо обновить данные в таблицах PODOT, SUD, UD_SUD */
                /*Обновление PODOT*/
                OdbcConnection odbcCon = new OdbcConnection("");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["PODOT"]);
                odbcCon.Open();
                OdbcCommand _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format("update podot set podr = '{0}' where tnom = '{1}'",
                    tbCode_Subdiv.Text, r_empNew.PER_NUM);
                try
                {   
                    /* Обновляем данные в подотчете только для основных работников! */
                    if (!r_transferNew.SIGN_COMB)
                    {
                        _rezult.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы PODOT!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                odbcCon.Close();
                /*Обновление SUD, UD_SUD*/
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["SUD"]);
                odbcCon.Open();
                _rezult.CommandText = string.Format("update SUD set podr = '{0}' where tnom = '{1}' and p_rab = '{2}'",
                    tbCode_Subdiv.Text, r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "");
                try
                {
                    _rezult.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы SUD!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                _rezult.CommandText = string.Format("select pnsud from SUD where tnom = '{0}' and p_rab = '{1}'",
                    r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "");
                OdbcCommand updUd_Sud = new OdbcCommand("", odbcCon);
                OdbcDataReader reader = _rezult.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        updUd_Sud.CommandText = string.Format("update UD_SUD set podr = '{0}' where tnom = '{1}' and pnsud = '{2}'",
                           tbCode_Subdiv.Text, r_empNew.PER_NUM, reader[0].ToString().Trim());
                        updUd_Sud.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы UD_SUD!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                odbcCon.Close();
                ///*Обновление SPR_287*/
                //odbcCon.ConnectionString = string.Format(
                //    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                //    ParVal.Vals["SPR_287"]);
                //odbcCon.Open();
                //_rezult.CommandText = string.Format("update SPR_287 set podr = '{0}' where tnom = '{1}' and p_rab = '{2}'",
                //    tbCode_Subdiv.Text, r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "");
                //_rezult.ExecuteNonQuery();
                //odbcCon.Close();
                ///*Обновление ALSPR*/
                //odbcCon.ConnectionString = string.Format(
                //    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                //    ParVal.Vals["ALSPR"]);
                //odbcCon.Open();
                //_rezult.CommandText = string.Format("update ALSPR set sc = '{0}' where tn = '{1}' and p_rab = '{2}'",
                //    tbCode_Subdiv.Text, r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "");
                //_rezult.ExecuteNonQuery();
                //odbcCon.Close();
                /*При добавлении нового перевода, когда меняется подразделение нужно в SPR занести новую запись.
                 При изменении перевода, нужно просто обновить поля.*/
                if (flagAdd &&
                    r_transferNew.SUBDIV_ID != ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SUBDIV_ID)
                {
                    /// Выбираем подразделение из какого он перевелся для обновления справочника
                    OracleCommand comSub = new OracleCommand("", Connect.CurConnect);
                    comSub.BindByName = true;
                    comSub.CommandText = string.Format(
                        "select CODE_SUBDIV from {0}.SUBDIV where SUB_ACTUAL_SIGN = 1 and SUBDIV_ID = {1}",
                        Connect.Schema, ((TRANSFER_obj)(((CurrencyManager)BindingContext[transferPrev]).Current)).SUBDIV_ID);
                    OracleDataReader orSub = comSub.ExecuteReader();
                    while (orSub.Read())
                    {
	                    code_subdiv = orSub["CODE_SUBDIV"].ToString();
                    }
                    /// При добавлении нового перевода нужно обновить старую запись, установив значения 
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
                        "{^" + r_transferNew.DATE_TRANSFER.Value.AddDays(-1).Year.ToString() + "-" +
                            r_transferNew.DATE_TRANSFER.Value.AddDays(-1).Month.ToString().PadLeft(2, '0') + "-" +
                            r_transferNew.DATE_TRANSFER.Value.AddDays(-1).Day.ToString().PadLeft(2, '0') + "}",
                        code_subdiv);
                    try
                    {
                        _rezult.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    DateTime datePost = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_ADD;
                    DateTime date_servant;
                    if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_SERVANT != null)
                    {
                        date_servant = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_SERVANT;
                    }
                    else
                    {
                        date_servant = (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE;
                    }
                    /// Проверяем наличие записей в SPR по сотруднику в данном подразделении
                    _rezult.CommandText = string.Format(
                        "select count(*) from SPR where podr = '{0}' and tnom = '{1}' and p_rab = '{2}'",
                        tbCode_Subdiv.Text, r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "");
                    int countRow = 0;
                    try
                    {
                        Convert.ToInt32(_rezult.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при запросе к таблице SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    /// Если записей нет, то вставляем новую запись. Если записи есть, то обновляем существующую запись.
                    if (countRow == 0)
                    {
                        /// Затем нужно вставить новую запись
                        _rezult.CommandText = string.Format(
                            "insert into SPR(PODR,TNOM,FIO,DAT_POST,KT,SHPR,POL,DAT_ROG,DAT_KOR, " +
                            "FAM,NAM,OTC,P_RAB,RAZ,OKL," +
                            "PRF,PRNAD,NAL,ALIM,NAD_VRED,BIR,SC_OT,PR13,DUB_NAL,KOL_IG14,KOL_IG15, " +
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
                            ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC == null
                            ? 0 : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC,
                            ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY == null ? "0"
                            : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY.Value.ToString().Replace(",", "."),
                            cbCode_Tariff_Grid.Text);
                        // 23.07.2015 - убираем обновление данной таблицы, т.к. написано новое ПО по перечислениям
                        #region Обновление SPR_287
                        /*OdbcConnection odbcCon287 = new OdbcConnection("");
                        odbcCon287.ConnectionString = string.Format(
                            @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                            ParVal.Vals["SPR_287"]);
                        odbcCon287.Open();
                        // Создаем таблицу для справочника и заполняем ее данными
                        DataTable dtSPR_287 = new DataTable();
                        try
                        {
                            new OdbcDataAdapter(string.Format(
                                "select * from SPR_287 where PODR = '{0}' and TNOM = '{1}'",
                                code_subdiv, r_empNew.PER_NUM),
                                odbcCon287).Fill(dtSPR_287);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при запросе к таблице SPR_287!" +
                                "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                                "\nСодержание ошибки: \n" + ex.Message,
                                "АСУ \"Кадры\"",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        // Создаем команду удаления строк в таблице банков по новому подразделению, чтобы далее добавить их
                        OdbcCommand del287 = new OdbcCommand(string.Format("delete from SPR_287 where podr = '{0}' and tnom = '{1}'",
                            tbCode_Subdiv.Text, r_empNew.PER_NUM), odbcCon287);
                        try
                        {
                            del287.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при удалении данных из таблицы SPR_287!" +
                                "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                                "\nСодержание ошибки: \n" + ex.Message,
                                "АСУ \"Кадры\"",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        /// Создаем команду вставки строк
                        OdbcCommand ins287 = new OdbcCommand("", odbcCon287);
                        foreach (DataRow row in dtSPR_287.Rows)
                        {
                            DateTime dat_dok = Convert.ToDateTime(row["dat_dok"].ToString());
                            DateTime data_k = Convert.ToDateTime(row["data_k"].ToString());
                            ins287.CommandText = string.Format(
                                "insert into SPR_287(podr,tnom,fam,nam,otc,p_rab,pn,summa,proc,kod_bank, " +
                                "lics,nsber,karts,fam_p,nam_p,otc_p,ser,nom,kod_doc,dat_dok,kem_vid,pgorod, " +
                                "pp, data_k, pruc, proc_av, summa_av, proc_pr) " +
                                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},'{9}', " +
                                "'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}',{19},'{20}', " +
                                "'{21}',{22},{23},{24},{25},{26},{27})",
                                tbCode_Subdiv.Text, row["tnom"], row["fam"], row["nam"], row["otc"], 
                                row["p_rab"], row["pn"], row["summa"].ToString().Replace(",","."), 
                                row["proc"].ToString().Replace(",","."), row["kod_bank"],
                                row["lics"], row["nsber"], row["karts"], row["fam_p"], row["nam_p"],
                                row["otc_p"], row["ser"], row["nom"], row["kod_doc"], 
                                "{^" + dat_dok.Year.ToString() + "-" +
                                dat_dok.Month.ToString().PadLeft(2, '0') + "-" +
                                dat_dok.Day.ToString().PadLeft(2, '0') + "}",
                                row["kem_vid"], row["pgorod"], row["pp"].ToString() == "True" ? ".T." : ".F.", 
                                "{^" + data_k.Year.ToString() + "-" +
                                data_k.Month.ToString().PadLeft(2, '0') + "-" +
                                data_k.Day.ToString().PadLeft(2, '0') + "}",    
                                row["pruc"].ToString() == "True" ? ".T." : ".F.", 
                                row["proc_av"].ToString().Replace(",","."), 
                                row["summa_av"].ToString().Replace(",","."), 
                                row["proc_pr"].ToString().Replace(",","."));
                            try
                            {
                                ins287.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка при добавлении данных в таблицу SPR_287!" +
                                    "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                                    "\nСодержание ошибки: \n" + ex.Message,
                                    "АСУ \"Кадры\"",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        odbcCon287.Close();
                         * */
                        #endregion
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
                            ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC == null ? 0
                            : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC,
                            ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY == null ? "0"
                            : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY.Value.ToString().Replace(",", "."),
                            cbCode_Tariff_Grid.Text);
                    }
                    try
                    {
                        _rezult.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    odbcCon.Close();
                    flagAdd = false;
                }
                else
                {
                    /// При редактировании перевода нужно обновить запись, установив новые значения 
                    /// в полях PODR, FIO, DAT_POST, KT, SHPR, POL, DAT_ROG, DAT_KOR, FAM, NAM, OTC
                    odbcCon.ConnectionString = string.Format(
                        @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                        ParVal.Vals["SPR"]);
                    odbcCon.Open();
                    DateTime datePost = new DateTime();
                    if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_ADD != null)
                    {
                        datePost = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_ADD;
                    }
                    DateTime date_servant;
                    if (((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_SERVANT != null)
                    {
                        date_servant = (DateTime)((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).DATE_SERVANT;
                    }
                    else
                    {
                        date_servant = (DateTime)((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).DATE_HIRE;
                    }
                    _rezult.CommandText = string.Format("update SPR set FIO = '{2}', DAT_POST = {3}, KT = '{4}', " +
                        "SHPR = {5}, POL = '{6}', DAT_ROG = {7}, DAT_KOR = {8}, FAM = '{9}', " +
                        "NAM = '{10}', OTC = '{11}', PODR = '{13}', DAT_VIS = {14}, DAT_UV = {15}, RAZ = {16}, OKL = {17}, VSET = '{18}' " +
                        "where podr = '{12}' and tnom = '{0}' and p_rab = '{1}'",
                        r_empNew.PER_NUM, r_transferNew.SIGN_COMB ? "2" : "",
                        string.Format("{0} {1} {2}", r_empNew.EMP_LAST_NAME, r_empNew.EMP_FIRST_NAME.Substring(0, 1),
                            r_empNew.EMP_MIDDLE_NAME.Substring(0, 1)),
                            datePost == new DateTime() ? "ctod('')" : "{^" + datePost.Year.ToString() + "-" +
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
                        date_servant.Day.ToString().PadLeft(2, '0') + "}", "ctod('')",
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC == null ?
                        0 : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).CLASSIFIC,
                        ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY == null ?
                        "0" : ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).SALARY.Value.ToString().Replace(",", "."),
                        cbCode_Tariff_Grid.Text);
                    try
                    {
                        _rezult.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                            "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                            "\nСодержание ошибки: \n" + ex.Message,
                            "АСУ \"Кадры\"",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    odbcCon.Close();
                }
            }
            else
            {
                EMP_obj r_empNew = (EMP_obj)((CurrencyManager)BindingContext[emp]).Current;
                TRANSFER_obj r_transferNew = (TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current;
                // Для совмещения делаем дополнительную проверку, работает ли сотрудник по основной работе
                if (r_transferNew.SIGN_COMB == true)
                {
                    TRANSFER_seq trans_temp = new TRANSFER_seq(Connect.CurConnect);
                    trans_temp.Fill(string.Format(
                        "WHERE PER_NUM = '{0}' and SIGN_COMB = 0 and SIGN_CUR_WORK = 1",
                        r_transferNew.PER_NUM));
                    // Если основной работы нет, то увольняем его в Перке.
                    if (trans_temp.Count == 0)
                    {
                        FormMain.employees.DismissEmployee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                            DateTime.Today.AddDays(-1));
                            //(DateTime)r_transfer.DATE_TRANSFER.Value.AddDays(-1));
                    }
                }
                else
                {
                    FormMain.employees.DismissEmployee(r_empNew.PERCO_SYNC_ID.ToString(), r_empNew.PER_NUM,
                        DateTime.Today.AddDays(-1));
                        //(DateTime)r_transfer.DATE_TRANSFER.Value.AddDays(-1));
                }
                /*При увольнении работника необходимо обновить данные в таблицах PODOT, ALSPR */
                /*Обновление PODOT*/
                OdbcConnection odbcCon = new OdbcConnection("");
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["PODOT"]);
                odbcCon.Open();
                OdbcCommand _rezult = new OdbcCommand("", odbcCon);
                _rezult.CommandText = string.Format("update podot set dat_uv = {0} where tnom = '{1}'",
                    "{^" + r_transferNew.DATE_TRANSFER.Value.Year + "-" + r_transferNew.DATE_TRANSFER.Value.Month +
                    "-" + r_transferNew.DATE_TRANSFER.Value.Day + "}", r_empNew.PER_NUM);
                try
                {
                    if (!r_transferNew.SIGN_COMB)
                    {
                        _rezult.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы PODOT!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                odbcCon.Close();                
                /*Обновление ALSPR*/
                /* 03.07.2015 - Убираю обновление ALSPR 
                odbcCon.ConnectionString = string.Format(
                    @"DRIVER=Microsoft FoxPro VFP Driver (*.dbf);Exclusive = No;SourceType = DBF;sourceDB={0}",
                    ParVal.Vals["ALSPR"]);
                odbcCon.Open();
                _rezult.CommandText = string.Format("update ALSPR set duv = {0} where tn = '{1}' and p_rab = '{2}'",
                    "{^" + r_transferNew.DATE_TRANSFER.Value.Year + "-" + r_transferNew.DATE_TRANSFER.Value.Month +
                    "-" + r_transferNew.DATE_TRANSFER.Value.Day + "}", r_empNew.PER_NUM,
                    r_transferNew.SIGN_COMB ? "2" : "");
                try
                {
                    _rezult.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы ALSPR!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                odbcCon.Close();*/
                /// При увольнении работника нужно обновить старую запись, установив значения 
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
                try
                {
                    _rezult.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы SPR!" +
                        "\nНеобходимо сообщить об ошибке разработчикам программы!" +
                        "\nСодержание ошибки: \n" + ex.Message,
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                odbcCon.Close();
            }
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
                /*string formsalaryPrev, pos_namePrev;
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
                degreePrev = degree.Where(i => i.DEGREE_ID == r_transferPrev.DEGREE_ID).FirstOrDefault().CODE_DEGREE;
                subdivPrev = FormMain.subdiv.Where(i => i.SUBDIV_ID == r_transferPrev.SUBDIV_ID).FirstOrDefault().CODE_SUBDIV;
                posPrev = FormMain.position.Where(i => i.POS_ID == r_transferPrev.POS_ID).FirstOrDefault().CODE_POS;
                formsalaryPrev = form_pay.Where(i => i.FORM_PAY == r_transferPrev.FORM_PAY).FirstOrDefault().NAME_FORM_PAY;
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
                    new CellParameter(50, 47, cbForm_Pay.Text, null), new CellParameter(52, 11, degreePrev, null),
                    new CellParameter(52, 44, tbCode_Degree.Text, null), new CellParameter(63, 48, podpis, null)                
                };
                Excel.PrintR1C1(false, "Transfer.xlt", cellParameters);*/

                string[][] s_pos = new string[][] { };
                if (Signes.Show(0, "Transfer_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == System.Windows.Forms.DialogResult.OK)
                {
                    DataSet _dsReport = new DataSet();
                    _dsReport.Tables.Add("SIGNES");
                    _dsReport.Tables.Add("ORDER_EMP");
                    new OracleDataAdapter("select '' SIGNES_POS, '' SIGNES_FIO from dual where 1 = 2",
                        Connect.CurConnect).Fill(_dsReport.Tables["SIGNES"]);
                    for (int i = 0; i < s_pos.Count(); i++)
                    {
                        _dsReport.Tables["SIGNES"].Rows.Add(new object[] { s_pos[i][0].ToString(), s_pos[i][1].ToString() });
                    }
                    OracleDataAdapter _daOrder_Hire = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectOrder_Transfer.sql"),
                        Connect.Schema), Connect.CurConnect);
                    _daOrder_Hire.SelectCommand.BindByName = true;
                    _daOrder_Hire.SelectCommand.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID;
                    _daOrder_Hire.Fill(_dsReport.Tables["ORDER_EMP"]);
                    // Разделяем название должности на 3 строки чтобы была красивая печать
                    List<string> slova = Kadr.Transfer.Slova(_dsReport.Tables["ORDER_EMP"].Rows[0]["POS_NAME"].ToString() +
                        " " + _dsReport.Tables["ORDER_EMP"].Rows[0]["POS_NOTE"].ToString(), ' ');
                    List<string> arrayPos = Kadr.Transfer.ArraySlov(slova, 20, 38);
                    _dsReport.Tables["ORDER_EMP"].Rows[0]["POS1"] = arrayPos[0];
                    _dsReport.Tables["ORDER_EMP"].Rows[0]["POS2"] = arrayPos[1];
                    _dsReport.Tables["ORDER_EMP"].Rows[0]["POS3"] = arrayPos[2];
                    slova = Kadr.Transfer.Slova(_dsReport.Tables["ORDER_EMP"].Rows[0]["PREV_POS_NAME"].ToString() +
                        " " + _dsReport.Tables["ORDER_EMP"].Rows[0]["PREV_POS_NOTE"].ToString(), ' ');
                    arrayPos = Kadr.Transfer.ArraySlov(slova, 20, 38);
                    _dsReport.Tables["ORDER_EMP"].Rows[0]["PREV_POS1"] = arrayPos[0];
                    _dsReport.Tables["ORDER_EMP"].Rows[0]["PREV_POS2"] = arrayPos[1];
                    _dsReport.Tables["ORDER_EMP"].Rows[0]["PREV_POS3"] = arrayPos[2];
                    ReportViewerWindow report = new ReportViewerWindow(
                        "Приказ о переводе", "Reports/Order_Transfer.rdlc", _dsReport,
                        new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                    );
                    report.Show();
                }
            }
            else
            {
                string dayTrDateOrder, monthTrDateOrder, yearTrDateOrder, dayDismiss, monthDismiss, yearDismiss;
                string reason_article = "";
                string reason_order = "";
                string podpis;
                string[][] s_pos = new string[][] { };
                if (Kadr.Vacation_schedule.Signes.Show(this, 0, "Dismiss_Order", "Выберите должностное лицо для подписи приказа", 1, ref s_pos) == System.Windows.Forms.DialogResult.OK)
                {

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
                    //CellParameter[] cellParameters = new CellParameter[] { 
                    //    new CellParameter(7, 7, dayTrDateOrder, null), new CellParameter(7, 12, monthTrDateOrder, null), 
                    //    new CellParameter(7, 23, yearTrDateOrder, null), 
                    //    new CellParameter(7,33, r_transfer.TR_NUM_ORDER + prefix, null),
                    //    new CellParameter(14, 54, r_transfer.CONTR_EMP, null), 
                    //    new CellParameter(15, 54, r_transfer.DATE_CONTR != null ? r_transfer.DATE_CONTR.Value.ToShortDateString() : "", null), 
                    //    new CellParameter(16, 11, dayDismiss, null), new CellParameter(16, 16, monthDismiss, null), 
                    //    new CellParameter(16, 26, yearDismiss, null),                
                    //    new CellParameter(17, 56, r_emp.PER_NUM, null), new CellParameter(18, 1, r_emp.EMP_LAST_NAME + " " + 
                    //        r_emp.EMP_FIRST_NAME + " " + r_emp.EMP_MIDDLE_NAME, null),
                    //    new CellParameter(20, 20, tbCode_Subdiv.Text, null), new CellParameter(21, 1, cbSubdiv_Name.Text, null), 
                    //    new CellParameter(23, 1, cbPos_Name.Text, null), 
                    //    new CellParameter(25, 8, r_account_data != null ? r_account_data.CLASSIFIC != null ? r_account_data.CLASSIFIC.Value.ToString() : "" : "", null), 
                    //    new CellParameter(26, 11, cbDegree_Name.Text, null), new CellParameter(26, 61, tbCode_Degree.Text, null), 
                    //    new CellParameter(27, 25, cbBase_doc.Text, null), new CellParameter(28, 1, reason_article, null), 
                    //    new CellParameter(29, 1, arrayPos[0].Trim(), null), new CellParameter(30, 1, arrayPos[1].Trim(), null), 
                    //    new CellParameter(31, 1, arrayPos[2].Trim(), null), 
                    //    new CellParameter(40, 39, dayTrDateOrder, null), 
                    //    new CellParameter(40, 44, monthTrDateOrder, null),
                    //    new CellParameter(40, 55, yearTrDateOrder, null), parameterPodpis
                    //};

                    CellParameter[] cellParameters = new CellParameter[] { };
                    if (arrayPos[2].Trim() != "")
                    {
                        cellParameters = new CellParameter[] { 
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
                            new CellParameter(28, 1, reason_article, null), 
                            new CellParameter(29, 1, arrayPos[0].Trim(), null), new CellParameter(30, 1, arrayPos[1].Trim(), null), 
                            new CellParameter(31, 1, arrayPos[2].Trim(), null), 
                            new CellParameter(32, 1, cbBase_doc.Text, null),
                            new CellParameter(38, 1, s_pos[0][0], null), 
                            new CellParameter(39, 48, s_pos[0][1], null)/*,
                            new CellParameter(41, 39, dayTrDateOrder, null), 
                            new CellParameter(41, 44, monthTrDateOrder, null),
                            new CellParameter(41, 55, yearTrDateOrder, null), */
                        };
                    }
                    else
                    {
                        cellParameters = new CellParameter[] { 
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
                            new CellParameter(30, 1, reason_article, null), 
                            new CellParameter(31, 1, arrayPos[0].Trim(), null), new CellParameter(32, 1, arrayPos[1].Trim(), null), 
                            new CellParameter(33, 1, cbBase_doc.Text, null),
                            new CellParameter(40, 1, s_pos[0][0], null), 
                            new CellParameter(41, 48, s_pos[0][1], null)/*,
                            new CellParameter(41, 39, dayTrDateOrder, null), 
                            new CellParameter(41, 44, monthTrDateOrder, null),
                            new CellParameter(41, 55, yearTrDateOrder, null), */
                        };
                    }
                    Excel.PrintR1C1("Dismiss.xlt", cellParameters);
                }
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
                if (flagDismiss)
                {
                    if (r_transfer.TR_DATE_ORDER != null && tbTr_Num_Dismiss.Text != "" && cbBase_doc.SelectedValue != null &&
                    cbReason_dismiss.SelectedValue != null && r_transfer.DATE_TRANSFER != null)
                    {
                        btPreview.Enabled = true;
                    }
                }
                else
                {
                    if (r_transfer.DATE_TRANSFER != null && tbTr_Num_Order.Text != "" && cbChar_Work_ID.SelectedValue != null &&
                    cbDegree_Name.SelectedValue != null && r_transfer.TR_DATE_ORDER != null)
                    {
                        btPreview.Enabled = true;
                    }
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
        /// Событие нажатия кнопки выбора типа приказа о переводе (отдел кадров или канцелярия)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btOk_Click(object sender, EventArgs e)
        {
            if (rbOtdel.Checked)
            {
                NumberOrder();                  
                tbTr_Num_Order.Text = numOrderPer;
                tbTr_Num_Order.ReadOnly = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;
                deTr_Date_Order.Text = DateTime.Now.ToShortDateString();
            }
            else
            {
                ((TRANSFER_obj)(((CurrencyManager)BindingContext[transfer]).Current)).CHAN_SIGN = true;
                tbTr_Num_Order.ReadOnly = false;
                tbTr_Num_Order.Text = "";
             }
            deTr_Date_Order.Focus();
            formOrder.Close();
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
        public static List<string> Slova(string _string, char _char)
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
        public static List<string> ArraySlov(List<string> slova, int len1, int len2)
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

        private void cbChan_SignDismiss_CheckedChanged(object sender, EventArgs e)
        {
            if (transfer.Count > 0)
            {
                if (cbChan_SignDismiss.Checked)
                {
                    // Предполагаем, что если cbChan_SignDismiss.Enabled, то мы редактируем запись и нам можно обнулить дату приказа
                    if (cbChan_SignDismiss.Enabled)
                    {
                        tbTr_Num_Dismiss.Enabled = true;
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = true;
                        ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = null;
                    }
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

        private void cbChan_Sign_Click(object sender, EventArgs e)
        {
            if (cbChan_Sign.Checked)
            {
                tbTr_Num_Order.Enabled = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = true;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = null;
            }
            else
            {
                tbTr_Num_Order.Enabled = false;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).CHAN_SIGN = false;
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TR_DATE_ORDER = DateTime.Now;
            }
            ((CurrencyManager)BindingContext[transfer]).Refresh();
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
                ((ACCOUNT_DATA_obj)((CurrencyManager)BindingContext[account]).Current).TARIFF_GRID_ID = null;
        }

        private void btViewAdd_Agreement_Click(object sender, EventArgs e)
        {
            string[][] s_pos = new string[][] { };
            if (Signes.Show(0, "Transfer_Order", "Выберите должностное лицо для подписи", 1, ref s_pos, 1) == System.Windows.Forms.DialogResult.OK)
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
                for (int i = 0; i < s_pos.Count(); i++)
                {
                    _dsReport.Tables["SIGNES"].Rows.Add(new object[] { 
                            s_pos[i][0].ToString(), s_pos[i][1].ToString(), s_pos[i][2].ToString() });
                    // Пожелания Эльканова Р.Д. - он хочет чтобы ФИО подписанта выходила в договоре и допнике. 
                    // Притом в формате Фамилия И.О., поэтому проводим такие махинации, меняя местами ИО и фамилию
                    if (s_pos[i][1].ToString().IndexOf(" ") > 0)
                        _fio_Signature = s_pos[i][1].ToString().Substring(s_pos[i][1].ToString().LastIndexOf(" ")) + " " +
                            s_pos[i][1].ToString().Substring(0, s_pos[i][1].ToString().LastIndexOf(" "));
                    else
                        _fio_Signature = s_pos[i][1].ToString();
                    if (s_pos[i][2].ToString() == "1")
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
                        ReportViewerWindow reportContract = new ReportViewerWindow(
                            "Дополнительное соглашение", "Reports/Add_Agreement.rdlc", _dsReport,
                            new List<Microsoft.Reporting.WinForms.ReportParameter>() { }
                        );
                        reportContract.Show();
                    }                
                    else
                    {
                        MessageBox.Show("Данных по доп.соглашению нет!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Данных по доп.соглашению нет!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btMatching_List_Project_Click(object sender, EventArgs e)
        {
            OracleCommand _ocProject_Statement_ID = new OracleCommand(string.Format(
                @"select PROJECT_TRANSFER_ID from {0}.PROJECT_TRANSFER WHERE TRANSFER_ID = :p_TRANSFER_ID",
                Connect.Schema), Connect.CurConnect);
            _ocProject_Statement_ID.BindByName = true;
            _ocProject_Statement_ID.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal).Value =
                ((TRANSFER_obj)((CurrencyManager)BindingContext[transfer]).Current).TRANSFER_ID;
            OracleDataReader _dr = _ocProject_Statement_ID.ExecuteReader();
            if (_dr.Read())
            {
                WpfControlLibrary.Вase_Exchange.Matching_List_Project_View _matching_List = new WpfControlLibrary.Вase_Exchange.Matching_List_Project_View(_dr["PROJECT_TRANSFER_ID"]);
                //_matching_List.Owner = Window.GetWindow(this);
                _matching_List.ShowDialog();
            }
            else
            {
                MessageBox.Show("Данный проект перевода был создан без электронного согласования заявления!",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
