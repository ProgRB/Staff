using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using LibraryKadr;
using System.IO;
using Staff;
using Oracle.DataAccess.Client;

namespace Kadr
{
    public partial class ListEmpConditionWork : Form
    {
        static OracleDataAdapter _daEmp, _daTransfer;
        static DataSet _ds;
        FormMain formMain;
        public ListEmpConditionWork(FormMain _parent)
        {
            InitializeComponent();
            formMain = _parent;
            _ds.Tables["EMP"].Rows.Clear();
            _ds.Tables["TRANS"].Rows.Clear();
            _daEmp.Fill(_ds.Tables["EMP"]);
            _daTransfer.Fill(_ds.Tables["TRANS"]);
            _ds.Tables["EMP"].DefaultView.RowFilter = "";

            DataTable _dt = HBConditionWork.DS.Tables["COND"].DefaultView.ToTable();
            _dt.Rows.Add(new object[] { null, ' ' });

            dgEmp.DataSource = _ds.Tables["EMP"].DefaultView;
            dgTransfer.DataSource = _ds.Tables["TRANS"].DefaultView;
            dgEmp.CellFormatting += new DataGridViewCellFormattingEventHandler(dgEmp_CellFormatting);
            RefGridEmp();
            RefGridTransfer();
            DataGridViewComboBoxColumn c3 = new DataGridViewComboBoxColumn();
            c3.Name = "SUBCLASS_NUMBER";
            c3.HeaderText = "№ подкласса";
            c3.AutoComplete = true;            
            c3.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c3.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c3.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            dgTransfer.Columns.Insert(0, c3);
            ((DataGridViewComboBoxColumn)dgTransfer.Columns["SUBCLASS_NUMBER"]).DataSource = _dt;
            ((DataGridViewComboBoxColumn)dgTransfer.Columns["SUBCLASS_NUMBER"]).ValueMember = "CONDITIONS_OF_WORK_ID";
            ((DataGridViewComboBoxColumn)dgTransfer.Columns["SUBCLASS_NUMBER"]).DisplayMember = "SUBCLASS_NUMBER";
            dgTransfer.Columns["SUBCLASS_NUMBER"].DataPropertyName = "CONDITIONS_OF_WORK_ID";

            MDataGridViewCalendarColumn c2 = new MDataGridViewCalendarColumn();
            c2.Name = "CONDITIONS_DATE_BEGIN";
            c2.HeaderText = "Дата утверждения отчета о проведении СОУТ";
            c2.SortMode = DataGridViewColumnSortMode.NotSortable;
            c2.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c2.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns.Insert(1, c2);
            dgTransfer.Columns["CONDITIONS_DATE_BEGIN"].DataPropertyName = "CONDITIONS_DATE_BEGIN";

            MDataGridViewCalendarColumn c4 = new MDataGridViewCalendarColumn();
            c4.Name = "CONDITIONS_DATE_END";
            c4.HeaderText = "Окончание действия даты утверждения";
            c4.SortMode = DataGridViewColumnSortMode.NotSortable;
            c4.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c4.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns.Insert(2, c4);
            dgTransfer.Columns["CONDITIONS_DATE_END"].DataPropertyName = "CONDITIONS_DATE_END";

            pnCondition.EnableByRules();
        }

        static ListEmpConditionWork()
        {
            _ds = new DataSet();
            _ds.Tables.Add("EMP");
            _ds.Tables.Add("TRANS");

            _daEmp = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectEmpConditionWork.sql"),
                DataSourceScheme.SchemeName, ""), Connect.CurConnect);

            // Select
            _daTransfer = new OracleDataAdapter(string.Format(Queries.GetQuery("SelectEmpTransferCond.sql"),
                Connect.Schema), Connect.CurConnect);
            _daTransfer.SelectCommand.BindByName = true;
            _daTransfer.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = 0;
            // Update
            _daTransfer.UpdateCommand = new OracleCommand(string.Format(
                @"BEGIN {0}.TRANSFER_COND_OF_WORK_UPDATE(:TRANSFER_ID, :CONDITIONS_OF_WORK_ID, :CONDITIONS_DATE_BEGIN, :CONDITIONS_DATE_END); END;", Connect.Schema),
                Connect.CurConnect);
            _daTransfer.UpdateCommand.BindByName = true;
            _daTransfer.UpdateCommand.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal, 0, "TRANSFER_ID");
            _daTransfer.UpdateCommand.Parameters.Add("CONDITIONS_OF_WORK_ID", OracleDbType.Decimal, 0, "CONDITIONS_OF_WORK_ID");
            _daTransfer.UpdateCommand.Parameters.Add("CONDITIONS_DATE_BEGIN", OracleDbType.Date, 0, "CONDITIONS_DATE_BEGIN");
            _daTransfer.UpdateCommand.Parameters.Add("CONDITIONS_DATE_END", OracleDbType.Date, 0, "CONDITIONS_DATE_END");
        }

        void RefGridEmp()
        {
            dgEmp.Columns["code_subdiv"].HeaderText = "Подр.";
            dgEmp.Columns["code_subdiv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgEmp.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["code_subdiv"].Width = 50;
            dgEmp.Columns["per_num"].HeaderText = "Таб.№";
            dgEmp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgEmp.Columns["per_num"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["per_num"].Width = 50;
            dgEmp.Columns["emp_last_name"].HeaderText = "Фамилия";
            //dgEmp.Columns["emp_last_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_last_name"].Width = 110;
            dgEmp.Columns["emp_first_name"].HeaderText = "Имя";
            //dgEmp.Columns["emp_first_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_first_name"].Width = 100;
            dgEmp.Columns["emp_middle_name"].HeaderText = "Отчество";
            //dgEmp.Columns["emp_middle_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_middle_name"].Width = 120;
            dgEmp.Columns["emp_sex"].HeaderText = "Пол";
            dgEmp.Columns["emp_sex"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgEmp.Columns["emp_sex"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_sex"].Width = 40;
            dgEmp.Columns["emp_birth_date"].HeaderText = "Дата рождения";
            dgEmp.Columns["emp_birth_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgEmp.Columns["emp_birth_date"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_birth_date"].Width = 80;
            dgEmp.Columns["comb"].HeaderText = "Сов.";
            dgEmp.Columns["comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgEmp.Columns["comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["comb"].Width = 40;
            dgEmp.Columns["code_pos"].HeaderText = "Шифр";
            //dgEmp.Columns["code_pos"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["code_pos"].Width = 55;
            dgEmp.Columns["CODE_DEGREE"].HeaderText = "Кат.";
            //dgEmp.Columns["CODE_DEGREE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["CODE_DEGREE"].Width = 40;
            dgEmp.Columns["pos_name"].HeaderText = "Наименование профессии";
            //dgEmp.Columns["pos_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["pos_name"].Width = 400;
            dgEmp.Columns["SUBCLASS_NUMBER"].HeaderText = "№ подкласса";
            //dgEmp.Columns["SUBCLASS_NUMBER"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["WORKER_ID"].Visible = false;
            dgEmp.Columns["transfer_id"].Visible = false;
            dgEmp.Columns["from_position"].Visible = false;
            dgEmp.Columns["date_hire"].Visible = false;
            dgEmp.Columns["TYPE_TRANSFER_ID"].Visible = false;
            dgEmp.Columns["SIGN_COMB"].Visible = false;
            dgEmp.ReadOnly = true;
        }

        private void RefGridTransfer()
        {
            dgTransfer.Columns["code_subdiv"].HeaderText = "Подр.";
            dgTransfer.Columns["code_subdiv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["code_subdiv"].ReadOnly = true;
            dgTransfer.Columns["sign_comb"].HeaderText = "Совм.";
            dgTransfer.Columns["sign_comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["sign_comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["sign_comb"].ReadOnly = true;
            dgTransfer.Columns["pos_name"].HeaderText = "Наименование должности";
            dgTransfer.Columns["pos_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgTransfer.Columns["pos_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["pos_name"].ReadOnly = true;
            dgTransfer.Columns["POS_NOTE"].HeaderText = "Примечание к должности";
            dgTransfer.Columns["POS_NOTE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgTransfer.Columns["POS_NOTE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["POS_NOTE"].ReadOnly = true;
            dgTransfer.Columns["date_transfer"].HeaderText = "Дата движения";
            dgTransfer.Columns["date_transfer"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["date_transfer"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["date_transfer"].ReadOnly = true;
            dgTransfer.Columns["type_transfer_name"].HeaderText = "Тип движения";
            dgTransfer.Columns["type_transfer_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["type_transfer_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["type_transfer_name"].ReadOnly = true;
            dgTransfer.Columns["code_degree"].HeaderText = "Кат.";
            dgTransfer.Columns["code_degree"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["code_degree"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["code_degree"].ReadOnly = true;
            dgTransfer.Columns["contr_emp"].HeaderText = "Трудовой договор";
            dgTransfer.Columns["contr_emp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["contr_emp"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["contr_emp"].ReadOnly = true;
            dgTransfer.Columns["sign_comb"].HeaderText = "Совм.";
            dgTransfer.Columns["sign_comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["sign_comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["sign_comb"].ReadOnly = true;
            dgTransfer.Columns["date_contr"].HeaderText = "Дата договора";
            dgTransfer.Columns["date_contr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["date_contr"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["date_contr"].ReadOnly = true;
            dgTransfer.Columns["date_end_contr"].HeaderText = "Окончание договора";
            dgTransfer.Columns["date_end_contr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["date_end_contr"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["date_end_contr"].ReadOnly = true;
            dgTransfer.Columns["tr_num_order"].HeaderText = "Номер приказа";
            dgTransfer.Columns["tr_num_order"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["tr_num_order"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["tr_num_order"].ReadOnly = true;
            dgTransfer.Columns["tr_date_order"].HeaderText = "Дата приказа";
            dgTransfer.Columns["tr_date_order"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["tr_date_order"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["tr_date_order"].ReadOnly = true;
            dgTransfer.Columns["chan_sign"].HeaderText = "Признак канц.";
            dgTransfer.Columns["chan_sign"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["chan_sign"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["chan_sign"].ReadOnly = true;
            dgTransfer.Columns["form_pay"].HeaderText = "Форма оплаты";
            dgTransfer.Columns["form_pay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["form_pay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["form_pay"].ReadOnly = true;
            dgTransfer.Columns["char_work_name"].HeaderText = "Срок договора";
            dgTransfer.Columns["char_work_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["char_work_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["char_work_name"].ReadOnly = true;
            dgTransfer.Columns["CHAR_TRANSFER_NAME"].HeaderText = "Срок перевода";
            dgTransfer.Columns["CHAR_TRANSFER_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["CHAR_TRANSFER_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["CHAR_TRANSFER_NAME"].ReadOnly = true;
            dgTransfer.Columns["transfer_id"].Visible = false;
            dgTransfer.Columns["from_position"].Visible = false;
            dgTransfer.Columns["type_transfer_id"].Visible = false;
            dgTransfer.Columns["CONDITIONS_OF_WORK_ID"].Visible = false;
            dgTransfer.Columns["CONDITIONS_DATE_BEGIN"].Visible = false;
            dgTransfer.Columns["CONDITIONS_DATE_END"].Visible = false;
        }
               
        public void dgEmp_SelectionChanged(object sender, EventArgs e)
        {
            ///*ЗАПОЛНЕНИЕ ПЕРЕВОДОВ*/
            if (dgEmp.CurrentRow != null)
            {
                _ds.Tables["TRANS"].Clear();
                _daTransfer.SelectCommand.Parameters["p_WORKER_ID"].Value = dgEmp.CurrentRow.Cells["WORKER_ID"].Value;
                _daTransfer.Fill(_ds.Tables["TRANS"]);
                if (dgEmp.CurrentRow.Cells["comb"].Value.ToString() != "X")
                {
                    gbTransfer.Text = "Переводы";
                }
                else
                {
                    gbTransfer.Text = "Совмещение";
                }                
            }

        }          

        private void dgEmp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если сотрудник уволен красим строку в серый цвет
            if (dgEmp["TYPE_TRANSFER_ID", e.RowIndex].Value.ToString() == "3")
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
        }


        private void btSaveCondition_Click(object sender, EventArgs e)
        {
            DataTable _dtChanges = _ds.Tables["TRANS"].GetChanges();
            if (_dtChanges != null)
            {
                for (int i = 0; i < _dtChanges.DefaultView.Count; i++)
                {
                    if (_dtChanges.DefaultView[i]["CONDITIONS_OF_WORK_ID"] != DBNull.Value)
                        if (_dtChanges.DefaultView[i]["CONDITIONS_DATE_BEGIN"] == DBNull.Value)
                        {
                            MessageBox.Show("Пустая дата начала действия подкласса!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        { }
                    else
                    {
                        _dtChanges.DefaultView[i]["CONDITIONS_DATE_BEGIN"] = DBNull.Value;
                        _dtChanges.DefaultView[i]["CONDITIONS_DATE_END"] = DBNull.Value;
                    }
                }
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    _daTransfer.UpdateCommand.Transaction = transact;
                    _daTransfer.Update(_ds.Tables["TRANS"]);
                    transact.Commit();
                    _ds.Tables["TRANS"].Clear();
                    _daTransfer.Fill(_ds.Tables["TRANS"]);
                    MessageBox.Show("Данные сохранены!",
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения!\n\n" + ex.Message,
                        "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    transact.Rollback();
                }
            }
        }

        private void btCancelCondition_Click(object sender, EventArgs e)
        {
            _ds.Tables["TRANS"].RejectChanges();
        }

        private void btFilter_Apply_Click(object sender, EventArgs e)
        {
            string stFilter = "";
            if (tbPer_Num.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "PER_NUM = '" + tbPer_Num.Text.Trim().PadLeft(5, '0') + "'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "EMP_LAST_NAME like '" + tbEmp_Last_Name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbEmp_First_Name.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "EMP_FIRST_NAME like '" + tbEmp_First_Name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbEmp_Middle_Name.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "EMP_MIDDLE_NAME like '" + tbEmp_Middle_Name.Text.Trim().ToUpper() + "%'",
                    stFilter != "" ? "and" : "").Trim();
            }
            if (tbCode_Subdiv.Text != "")
            {
                stFilter = string.Format("{0} {2} {1}", stFilter, "CODE_SUBDIV = '" + tbCode_Subdiv.Text.Trim().PadLeft(3,'0') + "'",
                    stFilter != "" ? "and" : "").Trim();
            }
            _ds.Tables["EMP"].DefaultView.RowFilter = stFilter;
        }

        private void btFilter_Clear_Click(object sender, EventArgs e)
        {
            tbCode_Subdiv.Text = "";
            tbPer_Num.Text = "";
            tbEmp_Last_Name.Text = "";
            tbEmp_First_Name.Text = "";
            tbEmp_Middle_Name.Text = "";
            _ds.Tables["EMP"].DefaultView.RowFilter = "";
        }
    }
}
