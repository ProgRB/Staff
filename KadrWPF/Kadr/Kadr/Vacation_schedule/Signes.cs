using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;
namespace Kadr.Vacation_schedule
{
    public partial class Signes : Form
    {
        private string Code_docum;
        decimal? subdiv_id;
        private int RowCnt = 0;
        private DataSet ds = new DataSet();
        private Signes(decimal? Subdiv_id, string DocumentName )
        {
            InitializeComponent();
            subdiv_id = Subdiv_id;
            Code_docum = DocumentName;
            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"GO/SelectDocSignes.sql"), Staff.DataSourceScheme.SchemeName), Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p_code_doc", OracleDbType.Varchar2, Code_docum, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal, subdiv_id, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor, ParameterDirection.Output);
            a.TableMappings.Add("Table", "Signes");
            a.Fill(ds);
            grid_sign.RowHeadersDefaultCellStyle.ForeColor = Color.Gray;
            grid_sign.AutoGenerateColumns = false;
            grid_sign.DataSource = ds.Tables["Signes"];
            DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
            c.Name = "fl";
            c.HeaderText = "";
            c.DataPropertyName = "fl";
            grid_sign.Columns.Add(c);
            grid_sign.Columns.Add(new MDataGridViewTextBoxColumn("pos_name_sign","Должность","pos_name_sign"));
            grid_sign.Columns.Add(new MDataGridViewTextBoxColumn("EMP_NAME", "ФИО", "EMP_NAME"));
            grid_sign.Columns.Add(new MDataGridViewTextBoxColumn("DEFAULT_NUMBER", "№ п/п", "DEFAULT_NUMBER"));
            
            ColumnWidthSaver.FillWidthOfColumn(grid_sign);
        }
        /// <summary>
        /// Форма подписи
        /// </summary>
        /// <param name="con">соединение</param>
        /// <param name="owner">форма-владелец</param>
        /// <param name="Subdiv_id">подразделение документа</param>
        /// <param name="DocumentName">код документа</param>
        /// <param name="GroupBoxCaption">надпись в форме</param>
        /// <param name="NeedRowCount">сколько требуется подписей</param>
        /// <param name="str_signes">массив для возврата подписей в формате {ДОЛЖНОСТЬ,ФИО}</param>
        /// <returns>Возвращает результат диалога формы Ok  или Cancel</returns>
        public static DialogResult Show(IWin32Window owner, decimal? Subdiv_id, string DocumentName, string GroupBoxCaption, int NeedRowCount, ref string[][] str_signes)
        {
            Signes r = new Signes(Subdiv_id, DocumentName);            
            r.Text = GroupBoxCaption;
            r.RowCnt = NeedRowCount;
            r.ShowDialog(owner);
            r.grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            List<string[]> l =new List<string[]>();
            if (r.DialogResult == DialogResult.OK)
                str_signes = r.GetSignes();
            return r.DialogResult;
        }
        
        /// <summary>
        /// Показывает форму выбора подписей
        /// </summary>
        /// <param name="owner">Владеленц окна</param>
        /// <param name="Subdiv_id">Подразделение айдишник</param>
        /// <param name="DocumentName">Код докмента подписей</param>
        /// <param name="GroupBoxCaption">Напись окна</param>
        /// <param name="NeedRowCount">Кол-во подписантов необходимых</param>
        /// <param name="res_table">Выходная таблица подписей</param>
        /// <returns>Возвращает модальный результат</returns>
        public static DialogResult Show(IWin32Window owner, decimal? Subdiv_id, string DocumentName, string GroupBoxCaption, int NeedRowCount, ref DataTable res_table)
        {
            Signes r = new Signes(Subdiv_id, DocumentName);            
            r.Text = GroupBoxCaption;
            r.RowCnt = NeedRowCount;
            r.ShowDialog(owner);
            r.grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            List<string[]> l =new List<string[]>();
            if (r.DialogResult == DialogResult.OK)
            {
                res_table = new DataTable();
                res_table.Columns.Add("POS_NAME", typeof(string));
                res_table.Columns.Add("FIO", typeof(string));
                res_table.Columns.Add("ORDER_NUMBER", typeof(decimal));
                foreach (DataRowView rr in r.t.DefaultView)
                {
                    if (rr.Row["FL"] != DBNull.Value && rr.Row["FL"].ToString() == "1")
                        res_table.Rows.Add(rr["POS_NAME_SIGN"], rr["EMP_NAME"], rr["DEFAULT_NUMBER"]);
                }
            }
            return r.DialogResult;
        }

        /// <summary>
        /// Форма подписи
        /// </summary>
        /// <param name="con">соединение</param>
        /// <param name="owner">форма-владелец</param>
        /// <param name="Subdiv_id">подразделение документа</param>
        /// <param name="DocumentName">код документа</param>
        /// <param name="GroupBoxCaption">надпись в форме</param>
        /// <param name="NeedRowCount">сколько требуется подписей</param>
        /// <param name="str_signes">массив для возврата подписей в формате {ДОЛЖНОСТЬ,ФИО}</param>
        /// <returns>Возвращает результат диалога формы Ok  или Cancel</returns>
        public static DialogResult Show(decimal? Subdiv_id, string DocumentName, string GroupBoxCaption, int NeedRowCount, ref string[][] str_signes)
        {
            Signes r = new Signes(Subdiv_id, DocumentName);
            r.Text = GroupBoxCaption;
            r.RowCnt = NeedRowCount;
            r.ShowDialog();
            r.grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            List<string[]> l = new List<string[]>();
            if (r.DialogResult == DialogResult.OK)
                str_signes = r.GetSignes();
            return r.DialogResult;
        }

        /// <summary>
        /// Форма подписи
        /// </summary>
        /// <param name="con">соединение</param>
        /// <param name="owner">форма-владелец</param>
        /// <param name="Subdiv_id">подразделение документа</param>
        /// <param name="DocumentName">код документа</param>
        /// <param name="GroupBoxCaption">надпись в форме</param>
        /// <param name="NeedRowCount">сколько требуется подписей</param>
        /// <param name="str_signes">массив для возврата подписей в формате {ДОЛЖНОСТЬ,ФИО}</param>
        /// <returns>Возвращает результат диалога формы Ok  или Cancel</returns>
        public static DialogResult Show(decimal? Subdiv_id, string DocumentName, string GroupBoxCaption, int NeedRowCount, ref string[][] str_signes, int NeedDefaultNumber)
        {
            Signes r = new Signes(Subdiv_id, DocumentName);
            r.Text = GroupBoxCaption;
            r.RowCnt = NeedRowCount;
            r.ShowDialog();
            r.grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            List<string[]> l = new List<string[]>();
            if (r.DialogResult == DialogResult.OK)
                str_signes = r.GetSignes2();
            return r.DialogResult;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.Cancel;
        }

        /// <summary>
        /// Сохраняем изменения по подписям.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveSignes_Click(object sender, EventArgs e)
        {
            grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            grid_sign.BindingContext[ds.Tables["Signes"]].EndCurrentEdit();
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            OracleDataAdapter odaSave = new OracleDataAdapter("", Connect.CurConnect);
            odaSave.DeleteCommand = new OracleCommand(string.Format("delete from {0}.sign_doc where sign_doc_id=:p_sign_doc_id", Connect.Schema), Connect.CurConnect);
            odaSave.DeleteCommand.BindByName = true;
            odaSave.DeleteCommand.Parameters.Add("p_sign_doc_id", OracleDbType.Decimal, 0, "SIGN_DOC_ID");

            odaSave.UpdateCommand = new OracleCommand(string.Format("update {0}.sign_doc SET POS_NAME_SIGN=:p_POS_NAME_SIGN, EMP_NAME=:p_EMP_NAME, DEFAULT_NUMBER=:p_DEFAULT_NUMBER where sign_doc_id=:p_sign_doc_id", Connect.Schema), Connect.CurConnect);
            odaSave.UpdateCommand.BindByName = true;
            odaSave.UpdateCommand.Parameters.Add("p_sign_doc_id", OracleDbType.Decimal, 0, "SIGN_DOC_ID");
            odaSave.UpdateCommand.Parameters.Add("p_POS_NAME_SIGN", OracleDbType.Varchar2, 0, "POS_NAME_SIGN");
            odaSave.UpdateCommand.Parameters.Add("p_EMP_NAME", OracleDbType.Varchar2, 0, "EMP_NAME");
            odaSave.UpdateCommand.Parameters.Add("p_DEFAULT_NUMBER", OracleDbType.Decimal, 0, "DEFAULT_NUMBER");

            odaSave.InsertCommand = new OracleCommand(string.Format(@"INSERT INTO {0}.SIGN_DOC (
                                                       SIGN_DOC_ID, POS_NAME_SIGN, EMP_NAME, 
                                                       CODE_DOCUM, DEFAULT_NUMBER, SUBDIV_ID) 
                                                    VALUES ( :p_sign_doc_id,
                                                     :p_POS_NAME_SIGN,
                                                     :p_EMP_NAME,
                                                     :p_CODE_DOCUM,
                                                     :p_DEFAULT_NUMBER,
                                                     :p_SUBDIV_ID)", Connect.Schema), Connect.CurConnect);
            odaSave.InsertCommand.BindByName = true;
            odaSave.InsertCommand.Parameters.Add("p_sign_doc_id", OracleDbType.Decimal, 0, "SIGN_DOC_ID");
            odaSave.InsertCommand.Parameters.Add("p_POS_NAME_SIGN", OracleDbType.Varchar2, 0, "POS_NAME_SIGN");
            odaSave.InsertCommand.Parameters.Add("p_EMP_NAME", OracleDbType.Varchar2, 0, "EMP_NAME");
            odaSave.InsertCommand.Parameters.Add("p_CODE_DOCUM", OracleDbType.Varchar2, Code_docum.Trim(), ParameterDirection.Input);
            odaSave.InsertCommand.Parameters.Add("p_DEFAULT_NUMBER", OracleDbType.Decimal, 0, "DEFAULT_NUMBER");
            odaSave.InsertCommand.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal, subdiv_id, ParameterDirection.Input);
            try
            {
                for (int i = 0; i < ds.Tables["Signes"].Rows.Count; ++i)
                {
                    if (ds.Tables["Signes"].Rows[i].RowState == DataRowState.Added)
                        ds.Tables["Signes"].Rows[i]["sign_doc_id"] = new OracleCommand(string.Format("select {0}.sign_doc_id_seq.nextval from dual", Connect.Schema), Connect.CurConnect).ExecuteScalar();
                }
                odaSave.Update(t);
                tr.Commit();
                new ToolTip().Show("Подписи успешно сохранены!", btSaveSignes, 2000);
            }
            catch (Exception ex)
            {
                tr.Rollback();
                MessageBox.Show("Ошибка:" + ex.Message, "Ошибка сохранения");
            }
        }
        private DataTable t
        {
            get { return ds.Tables["Signes"]; }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            grid_sign.BindingContext[ds.Tables["Signes"]].EndCurrentEdit();
            int k=t.Rows.Cast<DataRow>().Count(er => er.RowState != DataRowState.Deleted && er["fl"].ToString() == "1");
            if (k < RowCnt)
            {
                MessageBox.Show("Требуется выбрать " + RowCnt + " ответственных", "Ошибка");
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void grid_sign_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthSaver.SaveWidthOfColumn(sender,e);
        }

        private void btAddSign_Click(object sender, EventArgs e)
        {
            ds.Tables["Signes"].Rows.Add(ds.Tables["Signes"].NewRow());
            grid_sign["fl", grid_sign.RowCount - 1].Value = false;
            grid_sign.Rows[grid_sign.RowCount - 1].HeaderCell.Value = "";
        }

        private void grid_sign_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            grid_sign[e.ColumnIndex, e.RowIndex].Value = DBNull.Value;
        }

        private void btDeleteSign_Click(object sender, EventArgs e)
        {
            if (grid_sign.CurrentRow!=null && MessageBox.Show("Удалить запись из списка подписей документа?", "АРМ Кадры", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                (grid_sign.CurrentRow.DataBoundItem as DataRowView).Delete();
            }
        }

        private string[][] GetSignes()
        {
            return t.Rows.Cast<DataRow>().Where(er => er.RowState != DataRowState.Deleted && er["fl"].ToString() == "1").OrderBy(m => m["DEFAULT_NUMBER"]).Select(o => new string[] { o["pos_name_sign"].ToString(), o["EMP_NAME"].ToString() }).ToArray();
        }

        private string[][] GetSignes2()
        {
            return t.Rows.Cast<DataRow>().Where(er => er.RowState != DataRowState.Deleted && er["fl"].ToString() == "1").OrderBy(m => m["DEFAULT_NUMBER"]).Select(o => new string[] { o["pos_name_sign"].ToString(), o["EMP_NAME"].ToString(), o["DEFAULT_NUMBER"].ToString() }).ToArray();
        }

        private void Signes_Shown(object sender, EventArgs e)
        {
            grid_sign.CommitEdit(DataGridViewDataErrorContexts.Commit);
            /*if (ds.Tables["Signes"].Rows.Count > 0)
                grid_sign["EMP_NAME", 0].Selected = true;*/
        }


    }
}
