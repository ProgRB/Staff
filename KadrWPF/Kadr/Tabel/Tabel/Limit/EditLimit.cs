using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Tabel
{
    public partial class EditLimit : Form
    {
        LIMIT_ON_SUBDIV_seq limit;
        DataTable dtLimitOnDegree, dtDegree;
        public EditLimit(bool _flagAdd, LIMIT_ON_SUBDIV_seq _limit)
        {
            InitializeComponent();
            limit = _limit;          
            deBegin_Limit.AddBindingSource(limit, LIMIT_ON_SUBDIV_seq.ColumnsName.LIMIT_BEGIN);
            deEnd_Limit.AddBindingSource(limit, LIMIT_ON_SUBDIV_seq.ColumnsName.LIMIT_END);
            deLimit_Date_Doc.AddBindingSource(limit, LIMIT_ON_SUBDIV_seq.ColumnsName.LIMIT_DATE_DOC);
            tbLimit_Number_Doc.AddBindingSource(limit, LIMIT_ON_SUBDIV_seq.ColumnsName.LIMIT_NUMBER_DOC);
            
            dtLimitOnDegree = new DataTable();
            //dgLimitOnDegree.AutoGenerateColumns = false;
            //dgLimitOnDegree.DataSource = dtLimitOnDegree;
            RefLimitOnDegree();

            dtDegree = new DataTable();
            new OracleDataAdapter(
                string.Format("select DEGREE_ID, CODE_DEGREE from {0}.DEGREE order by CODE_DEGREE", 
                DataSourceScheme.SchemeName),
                Connect.CurConnect).Fill(dtDegree);

            DataGridViewComboBoxColumn c3 = new DataGridViewComboBoxColumn();
            c3.Name = "DEGREE_ID";
            c3.HeaderText = "Категория";
            c3.AutoComplete = true;
            c3.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            dgLimitOnDegree.Columns.Add(c3);
            ((DataGridViewComboBoxColumn)dgLimitOnDegree.Columns["DEGREE_ID"]).DataSource = dtDegree;
            ((DataGridViewComboBoxColumn)dgLimitOnDegree.Columns["DEGREE_ID"]).ValueMember = "DEGREE_ID";
            ((DataGridViewComboBoxColumn)dgLimitOnDegree.Columns["DEGREE_ID"]).DisplayMember = "CODE_DEGREE";
            dgLimitOnDegree.Columns["DEGREE_ID"].DataPropertyName = "DEGREE_ID";

            DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
            c4.Name = "LIMIT_HOURS_PLAN";
            c4.HeaderText = "Количество часов по плану";
            c4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgLimitOnDegree.Columns.Add(c4);
            dgLimitOnDegree.Columns["LIMIT_HOURS_PLAN"].DataPropertyName = "LIMIT_HOURS_PLAN";
            if (GrantedRoles.GetGrantedRole("TABLE_ADD_LIMIT"))
            {
                tsbAddLOD.Enabled = true;
                tsbDeleteLOD.Enabled = true;
                btSaveLOD.Enabled = true;
                btSaveOrderLimit.Enabled = true;
            }
            else
            {
                tsbAddLOD.Enabled = false;
                tsbDeleteLOD.Enabled = false;
                btSaveLOD.Enabled = false;
                btSaveOrderLimit.Enabled = false;
            }
        }

        void RefLimitOnDegree()
        {
            dtLimitOnDegree.Clear();
            OracleDataAdapter adap =
                new OracleDataAdapter(
                string.Format(Queries.GetQuery("Table/LimitOnDegree.sql"), DataSourceScheme.SchemeName),
                Connect.CurConnect);
            adap.SelectCommand.BindByName = true;
            adap.SelectCommand.Parameters.Add("p_LIMIT_ON_SUBDIV_ID", 
                ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).LIMIT_ON_SUBDIV_ID);
            adap.Fill(dtLimitOnDegree);
            dgLimitOnDegree.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = dtLimitOnDegree;
            dgLimitOnDegree.DataSource = bs;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (deBegin_Limit.Date == null)
            {
                MessageBox.Show("Пустая дата начала лимита!",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deBegin_Limit.Focus();
                return;
            }
            if (deEnd_Limit.Date == null)
            {
                MessageBox.Show("Пустая дата окончания лимита!",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deEnd_Limit.Focus();
                return;
            }
            limit.Save();
            Connect.Commit();
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbAddLOD_Click(object sender, EventArgs e)
        {
            limit.Save();
            Connect.Commit();
            dtLimitOnDegree.Rows.Add();
        }

        private void tsbDeleteLOD_Click(object sender, EventArgs e)
        {
            dgLimitOnDegree.Rows.Remove(dgLimitOnDegree.CurrentRow);
        }

        private void btSaveLOD_Click(object sender, EventArgs e)
        {
            DataTable dtChanges = dtLimitOnDegree.GetChanges();
            if (dtChanges != null)
            {
                LIMIT_ON_DEGREE_seq limit_on_degree = new LIMIT_ON_DEGREE_seq(Connect.CurConnect);
                for (int i = 0; i < dtChanges.Rows.Count; ++i)
                {
                    /// Если строка была добавлена
                    if (dtChanges.Rows[i].RowState == DataRowState.Added)
                    {
                        limit_on_degree.AddNew();
                        ((CurrencyManager)BindingContext[limit_on_degree]).Position =
                            ((CurrencyManager)BindingContext[limit_on_degree]).Count - 1;
                        ((LIMIT_ON_DEGREE_obj)((CurrencyManager)BindingContext[limit_on_degree]).Current).LIMIT_ON_SUBDIV_ID =
                            ((LIMIT_ON_SUBDIV_obj)((CurrencyManager)BindingContext[limit]).Current).LIMIT_ON_SUBDIV_ID;
                        ((LIMIT_ON_DEGREE_obj)((CurrencyManager)BindingContext[limit_on_degree]).Current).DEGREE_ID =
                            Convert.ToDecimal(dtChanges.Rows[i]["DEGREE_ID"]);
                        ((LIMIT_ON_DEGREE_obj)((CurrencyManager)BindingContext[limit_on_degree]).Current).LIMIT_HOURS_PLAN =
                            Convert.ToDecimal(dtChanges.Rows[i]["LIMIT_HOURS_PLAN"]);
                        limit_on_degree.Save();
                    }
                    else
                        /// Если строка была модифицирована
                        if (dtChanges.Rows[i].RowState == DataRowState.Modified)
                        {
                            limit_on_degree.Fill("where LIMIT_ON_DEGREE_ID = " +
                                dtChanges.Rows[i]["LIMIT_ON_DEGREE_ID"].ToString());
                            ((CurrencyManager)BindingContext[limit_on_degree]).Position =
                                ((CurrencyManager)BindingContext[limit_on_degree]).Count - 1;
                            ((LIMIT_ON_DEGREE_obj)((CurrencyManager)BindingContext[limit_on_degree]).Current).DEGREE_ID =
                                Convert.ToDecimal(dtChanges.Rows[i]["DEGREE_ID"]);
                            ((LIMIT_ON_DEGREE_obj)((CurrencyManager)BindingContext[limit_on_degree]).Current).LIMIT_HOURS_PLAN =
                                Convert.ToDecimal(dtChanges.Rows[i]["LIMIT_HOURS_PLAN"]);
                            limit_on_degree.Save();
                        }
                        else
                            /// Если строка была удалена
                            if (dtChanges.Rows[i].RowState == DataRowState.Deleted)
                            {
                                limit_on_degree.Fill("where LIMIT_ON_DEGREE_ID = " +
                                    dtChanges.Rows[i]["LIMIT_ON_DEGREE_ID", DataRowVersion.Original].ToString());
                                ((CurrencyManager)BindingContext[limit_on_degree]).Position =
                                    ((CurrencyManager)BindingContext[limit_on_degree]).Count - 1;                                
                                limit_on_degree.Remove(
                                    (LIMIT_ON_DEGREE_obj)((CurrencyManager)BindingContext[limit_on_degree]).Current);
                                limit_on_degree.Save();
                            }
                }
                Connect.Commit();
                RefLimitOnDegree();
            }
        }

        private void deBegin_Limit_Validating(object sender, CancelEventArgs e)
        {
            if (deBegin_Limit.Date > deEnd_Limit.Date)
            {
                MessageBox.Show("Дата начала лимита больше даты окончания лимита!" +
                    "Дата окончания лимита будет перезаписана.",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deEnd_Limit.Date = deBegin_Limit.Date;
            }
        }

        private void deEnd_Limit_Validating(object sender, CancelEventArgs e)
        {
            if (deBegin_Limit.Date > deEnd_Limit.Date)
            {
                MessageBox.Show("Дата окончания лимита меньше даты начала лимита!" +
                    "Дата начала лимита будет перезаписана.",
                    "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deBegin_Limit.Date = deEnd_Limit.Date;
            }
        }
    }
}
