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
    public partial class grid_ras : Form, IDataLinkKadr
    {
        OracleDataTable per, okl, dekr;
        /// <summary>
        /// Применяется при поиске сотрудников, чтобы определить позицию работника в списке
        /// </summary>
        public BindingSource bs = new BindingSource();
        FormMain formMain;
        public grid_ras(FormMain _parent)
        {
            InitializeComponent();
            formMain = _parent;

            RefGridTransfer();

            dgEmp.DoubleClick += new EventHandler(_parent.btUpdateRas_Click);
            dgEmp.ContextMenuStrip.Items.Add(ListLinkKadr.GetMenuItem(this));
        }

        private void RefGridTransfer()
        {
            per = new OracleDataTable("", Connect.CurConnect);
            per.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/SelectEmpTransfer.sql"),
                Connect.Schema);
            //per.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = -1;
            per.SelectCommand.Parameters.Add("p_WORKER_ID", OracleDbType.Decimal).Value = -1;
            per.SelectCommand.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = "";
            per.Fill();
            dgTransfer.DataSource = per;
            dgTransfer.Columns["code_subdiv"].HeaderText = "Подр.";
            dgTransfer.Columns["code_subdiv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["sign_comb"].HeaderText = "Совм.";
            dgTransfer.Columns["sign_comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["sign_comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["pos_name"].HeaderText = "Наименование должности";
            dgTransfer.Columns["pos_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgTransfer.Columns["pos_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["POS_NOTE"].HeaderText = "Примечание к должности";
            dgTransfer.Columns["POS_NOTE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgTransfer.Columns["POS_NOTE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["date_transfer"].HeaderText = "Дата движения";
            dgTransfer.Columns["date_transfer"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["date_transfer"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["type_transfer_name"].HeaderText = "Тип движения";
            dgTransfer.Columns["type_transfer_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["type_transfer_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["code_degree"].HeaderText = "Кат.";
            dgTransfer.Columns["code_degree"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["code_degree"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["contr_emp"].HeaderText = "Трудовой договор";
            dgTransfer.Columns["contr_emp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["contr_emp"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["sign_comb"].HeaderText = "Совм.";
            dgTransfer.Columns["sign_comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["sign_comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["date_contr"].HeaderText = "Дата договора";
            dgTransfer.Columns["date_contr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["date_contr"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["date_end_contr"].HeaderText = "Окончание договора";
            dgTransfer.Columns["date_end_contr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["date_end_contr"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["tr_num_order"].HeaderText = "Номер приказа";
            dgTransfer.Columns["tr_num_order"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["tr_num_order"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["tr_date_order"].HeaderText = "Дата приказа";
            dgTransfer.Columns["tr_date_order"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["tr_date_order"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["chan_sign"].HeaderText = "Признак канц.";
            dgTransfer.Columns["chan_sign"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["chan_sign"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["form_pay"].HeaderText = "Форма оплаты";
            dgTransfer.Columns["form_pay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["form_pay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["char_TRANSFER_name"].HeaderText = "Срок перевода";
            dgTransfer.Columns["char_TRANSFER_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["char_TRANSFER_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["CHAR_WORK_NAME"].HeaderText = "Срок договора";
            dgTransfer.Columns["CHAR_WORK_NAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgTransfer.Columns["CHAR_WORK_NAME"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns["transfer_id"].Visible = false;
            dgTransfer.Columns["from_position"].Visible = false;
            dgTransfer.Columns["type_transfer_id"].Visible = false;
            dgTransfer.ReadOnly = true;

            okl = new OracleDataTable("", Connect.CurConnect);
            okl.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/SelectAccountData.sql"),
                    Connect.Schema);
            okl.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal);
            okl.SelectCommand.Parameters["p_transfer_id"].Value = -1;
            okl.Fill();
            dgAccount_data.DataSource = okl;
            dgAccount_data.Columns["change_date"].HeaderText = "Дата изменения";
            dgAccount_data.Columns["change_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["change_date"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["salary"].HeaderText = "Оклад";
            dgAccount_data.Columns["salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["salary"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["tax_code"].HeaderText = "Шифр налога";
            dgAccount_data.Columns["tax_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["tax_code"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["classific"].HeaderText = "Разряд";
            dgAccount_data.Columns["classific"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["classific"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["harmful_addition"].HeaderText = "Вредность в том числе";
            dgAccount_data.Columns["harmful_addition"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["harmful_addition"].SortMode = DataGridViewColumnSortMode.NotSortable;            
            dgAccount_data.Columns["harmful_addition_add"].HeaderText = "Вредность дополнительно";
            dgAccount_data.Columns["harmful_addition_add"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["harmful_addition_add"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["comb_addition"].HeaderText = "Надбавка за совмещение";
            dgAccount_data.Columns["comb_addition"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["comb_addition"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["code_tariff_grid"].HeaderText = "Шифр тарифной сетки";
            dgAccount_data.Columns["code_tariff_grid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgAccount_data.Columns["code_tariff_grid"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgAccount_data.Columns["account_data_id"].Visible = false;

            dekr = new OracleDataTable("", Connect.CurConnect);
            dekr.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/dekr.txt"), 
                Connect.Schema);
            dekr.SelectCommand.Parameters.Add("p_per_num", OracleDbType.Varchar2);
            dekr.SelectCommand.Parameters.Add("p_date", OracleDbType.Date);
        }

        /// <summary>
        /// Загрузка формы отображения списка сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_ras_Load(object sender, EventArgs e)
        {
            /// Я не знаю зачем Виталий Иванович так сделал, но оставил Load
            /*ЗАПОЛНЕНИЕ В ТОТ МОМЕНТ КОГДА ФОРМА ТОЛЬКО ОТКРЫВЕТСЯ */
            /* ЗАПОЛНЕНИЕ СПРАВОЧНИКА РАБОТАЮЩИХ И УВОЛЕННЫХ В ТЕЧЕНИИ ГОДА*/
            OracleDataTable dt = new OracleDataTable("", Connect.CurConnect);
            dt.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/spr.sql"), 
                Connect.Schema, "");
            dt.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = DateTime.Now;
            //dt.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            dt.Fill();
            bs.DataSource = dt;

            dgEmp.DataSource = bs;
            dgEmp.Columns["code_subdiv"].HeaderText = "Подр.";
            dgEmp.Columns["code_subdiv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["code_subdiv"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["code_subdiv"].Width = 50;
            dgEmp.Columns["per_num"].HeaderText = "Таб.№";
            dgEmp.Columns["per_num"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["per_num"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["per_num"].Width = 50;
            dgEmp.Columns["emp_last_name"].HeaderText = "Фамилия";
            dgEmp.Columns["emp_last_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_last_name"].Width = 110;
            dgEmp.Columns["emp_first_name"].HeaderText = "Имя";
            dgEmp.Columns["emp_first_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_first_name"].Width = 100;
            dgEmp.Columns["emp_middle_name"].HeaderText = "Отчество";
            dgEmp.Columns["emp_middle_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_middle_name"].Width = 120;
            dgEmp.Columns["emp_sex"].HeaderText = "Пол";
            dgEmp.Columns["emp_sex"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["emp_sex"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_sex"].Width = 40;
            dgEmp.Columns["emp_birth_date"].HeaderText = "Дата рождения";
            dgEmp.Columns["emp_birth_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["emp_birth_date"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["emp_birth_date"].Width = 80;
            dgEmp.Columns["comb"].HeaderText = "Сов.";
            dgEmp.Columns["comb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgEmp.Columns["comb"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["comb"].Width = 40;
            dgEmp.Columns["code_pos"].HeaderText = "Шифр";
            dgEmp.Columns["code_pos"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["code_pos"].Width = 55;
            dgEmp.Columns["CODE_DEGREE"].HeaderText = "Кат.";
            dgEmp.Columns["CODE_DEGREE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["CODE_DEGREE"].Width = 40;
            dgEmp.Columns["pos_name"].HeaderText = "Наименование профессии";
            dgEmp.Columns["pos_name"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["pos_name"].Width = 400;
            dgEmp.Columns["POS_NOTE"].HeaderText = "Примечание к профессии";
            dgEmp.Columns["POS_NOTE"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgEmp.Columns["POS_NOTE"].Width = 100;
            dgEmp.Columns["worker_id"].Visible = false;
            dgEmp.Columns["DATE_TRANSFER"].Visible = false;
            dgEmp.Columns["transfer_id"].Visible = false;
            dgEmp.Columns["from_position"].Visible = false;
            dgEmp.Columns["date_hire"].Visible = false;
            dgEmp.Columns["SIGN_PROFUNION"].Visible = false;
            dgEmp.Columns["RETIRER_SIGN"].Visible = false;
            dgEmp.Columns["TYPE_TRANSFER_ID"].Visible = false;
            dgEmp.Columns["SIGN_LEAVE"].Visible = false;
            dgEmp.Columns["SIGN_COMB"].Visible = false;
            dgEmp.ReadOnly = true;

            dgEmp.CellFormatting += new DataGridViewCellFormattingEventHandler(dgEmp_CellFormatting);
        }
       
        public void dgEmp_SelectionChanged(object sender, EventArgs e)
        {
            ///*ЗАПОЛНЕНИЕ ПЕРЕВОДОВ*/
            if (dgEmp.CurrentRow != null)
            {
                per.Clear();
                //per.SelectCommand.Parameters["p_transfer_id"].Value = dgEmp.CurrentRow.Cells["transfer_id"].Value;
                if (chShowAllTransfer.Checked)
                {
                    per.SelectCommand.Parameters["p_WORKER_ID"].Value = null;
                    per.SelectCommand.Parameters["p_PER_NUM"].Value = dgEmp.CurrentRow.Cells["PER_NUM"].Value;
                    per.Fill();
                    /// Определяем в декрете ли сотрудник. Если нет, то обозначаем какая это деятельность
                    if (dgEmp.CurrentRow.Cells["SIGN_LEAVE"].Value.ToString() == "0")
                    {
                        gbTransfer.Text = "Все переводы сотрудника";                        
                    }
                    else
                    {
                        // ОПРЕДЕЛЯЕМ даты декретного отпуска сотрудника
                        dekr.Clear();
                        dekr.SelectCommand.Parameters["p_per_num"].Value = dgEmp.CurrentRow.Cells["per_num"].Value;
                        dekr.SelectCommand.Parameters["p_date"].Value = DateTime.Now;
                        dekr.Fill();
                        if (dekr.Rows.Count > 0)
                        {
                            gbTransfer.Text = "Человек в декретном отпуске с " + dekr.Rows[0]["doc_begin"].ToString() + " по " + dekr.Rows[0]["doc_end"].ToString() + "";
                        }
                        else
                        {
                            gbTransfer.Text = "Все переводы сотрудника";
                        }
                    }
                }
                else
                {
                    per.SelectCommand.Parameters["p_WORKER_ID"].Value = dgEmp.CurrentRow.Cells["WORKER_ID"].Value;
                    per.SelectCommand.Parameters["p_PER_NUM"].Value = dgEmp.CurrentRow.Cells["PER_NUM"].Value;
                    per.Fill();
                    /// Определяем в декрете ли сотрудник. Если нет, то обозначаем какая это деятельность
                    if (dgEmp.CurrentRow.Cells["SIGN_LEAVE"].Value.ToString() == "0")
                    {
                        if (dgEmp.CurrentRow.Cells["comb"].Value.ToString() != "X")
                        {
                            gbTransfer.Text = "Переводы";
                        }
                        else
                        {
                            gbTransfer.Text = "Совмещение";
                        }
                    }
                    else
                    {
                        // ОПРЕДЕЛЯЕМ даты декретного отпуска сотрудника
                        dekr.Clear();
                        dekr.SelectCommand.Parameters["p_per_num"].Value = dgEmp.CurrentRow.Cells["per_num"].Value;
                        dekr.SelectCommand.Parameters["p_date"].Value = DateTime.Now;
                        dekr.Fill();
                        if (dekr.Rows.Count > 0)
                        {
                            gbTransfer.Text = "Человек в декретном отпуске с " + dekr.Rows[0]["doc_begin"].ToString() + " по " + dekr.Rows[0]["doc_end"].ToString() + "";
                        }
                        else
                        {
                            gbTransfer.Text = "Переводы";
                        }
                    }
                }
            }

        }                    

        /// <summary>
        /// Выбор перевода. Обновляем бухг. данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTransfer_SelectionChanged(object sender, EventArgs e)
        {
            if (dgTransfer.CurrentRow != null)
            {
                okl.Clear();
                okl.SelectCommand.Parameters["p_transfer_id"].Value = dgTransfer.CurrentRow.Cells["transfer_id"].Value;
                okl.Fill();
            }
        }

        private void grid_ras_FormClosing(object sender, FormClosingEventArgs e)
        {
            formMain.rgFilterRas.Visible = false;
            formMain.rgUpdateRas.Visible = false;
        }

        private void dgEmp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /// Если сотрудник уволен красим строку в серый цвет
            if (dgEmp["TYPE_TRANSFER_ID", e.RowIndex].Value.ToString() == "3")
            {
                e.CellStyle.BackColor = Color.Gainsboro;
            }
            else
                /// Если сотрудник находится в декретном отпуске красим его в бледно-желтый цвет
                if (dgEmp["SIGN_LEAVE", e.RowIndex].Value.ToString() != "0")
                {
                    e.CellStyle.BackColor = Color.LightGoldenrodYellow;
                }
        }

        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Transfer_id, "ACCOUNT");
        }

        public static void OpenLink(object sender, LinkData e)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(string.Format(@"select subdiv_id, code_subdiv, subdiv_name, transfer_id from {0}.transfer join {0}.subdiv using (subdiv_id) where 
                    transfer_id in (select transfer_id from {0}.transfer where sign_cur_work=1 or type_transfer_id=3 start with transfer_id=:p_transfer_id connect by nocycle PRIOR transfer_id=from_position or prior from_position = transfer_id)", Connect.Schema), Connect.CurConnect);
                cmd.Parameters.Add("p_transfer_id", OracleDbType.Decimal, e.Transfer_id, ParameterDirection.Input);
                cmd.BindByName = true;
                OracleDataReader r = cmd.ExecuteReader();
                r.Read();
                var OpenGridRas = Application.OpenForms.Cast<Form>().Where(t => t.Name == "grid_ras");
                if (OpenGridRas.Count() == 0)
                {
                    FormMain.grid_form = new grid_ras((Application.OpenForms["FormMain"] as FormMain));
                    FormMain.grid_form.MdiParent = Application.OpenForms["FormMain"];
                    (Application.OpenForms["FormMain"] as FormMain).rgDataFill.Visible = true;
                    (Application.OpenForms["FormMain"] as FormMain).rgFilterRas.Visible = true;
                    (Application.OpenForms["FormMain"] as FormMain).rgUpdateRas.Visible = true;
                    FormMain.grid_form.Show();
                    FormMain.grid_form.SetCurrentTransferId(r["TRANSFER_ID"]);
                }
                else
                {
                    FormMain.grid_form = OpenGridRas.First<Form>() as grid_ras;
                    FormMain.grid_form.SetCurrentTransferId(r["TRANSFER_ID"]);
                    FormMain.grid_form.Activate();
                }
                r.Close();
            }
            catch { }
        }

        public void SetCurrentTransferId(object transfer_id)
        {
            DataGridViewRow r = dgEmp.Rows.Cast<DataGridViewRow>().FirstOrDefault(t => (t.DataBoundItem as DataRowView)["TRANSFER_ID"].GetHashCode() == transfer_id.GetHashCode());
            if (r != null)
                dgEmp.CurrentCell = r.Cells[0];
        }

        public LinkData GetDataLink(object sender)
        {
            if (dgEmp.CurrentRow != null)
                return new LinkData(null, (dgEmp.CurrentRow.DataBoundItem as DataRowView).Row.Field<Decimal>("TRANSFER_ID"));
            else
                return null;
        }

        private void chShowAllTransfer_CheckedChanged(object sender, EventArgs e)
        {
            dgEmp_SelectionChanged(sender, e);
        }
    }
}
