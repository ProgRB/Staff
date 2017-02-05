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

namespace Tabel
{
    public partial class EditSubdivFT : Form
    {
        OracleDataTable dtSubdiv, dtSubdivFT;
        public EditSubdivFT()
        {
            InitializeComponent();
            dtSubdiv = new OracleDataTable("", Connect.CurConnect);
            dtSubdiv.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SubdivFT.sql"), Connect.Schema);
            dtSubdiv.Fill();
            dgSubdiv.DataSource = dtSubdiv;
            dgSubdiv.Columns["subdiv_id"].Visible = false;
            dgSubdiv.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgSubdiv.Columns["SUBDIV_NAME"].HeaderText = "Наименование подразделения";
            dtSubdivFT = new OracleDataTable("", Connect.CurConnect);
            dtSubdivFT.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SubdivFTEdit.sql"), Connect.Schema);
            dtSubdivFT.Fill();
            dgSubdivFT.DataSource = dtSubdivFT;
            dgSubdivFT.Columns["subdiv_id"].Visible = false;
            dgSubdivFT.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgSubdivFT.Columns["SUBDIV_NAME"].HeaderText = "Наименование подразделения";
        }

        private void btAddSubdiv_Click(object sender, EventArgs e)
        {
            if (dgSubdiv.CurrentRow != null)
            {
                DataRow selRow = dtSubdiv.Select("SUBDIV_ID = " +
                    dgSubdiv.CurrentRow.Cells["SUBDIV_ID"].Value.ToString()).First();             
                dtSubdivFT.Rows.Add(selRow.ItemArray);
                dtSubdiv.Rows.Remove(selRow);                
                dgSubdiv.Columns["subdiv_id"].Visible = false;
                dgSubdiv.Focus();
            }
        }

        private void btDeleteSubdiv_Click(object sender, EventArgs e)
        {
            if (dgSubdivFT.CurrentRow != null)
            {
                DataRow selRow = dtSubdivFT.Select("SUBDIV_ID = " + 
                    dgSubdivFT.CurrentRow.Cells["SUBDIV_ID"].Value.ToString()).First();
                dtSubdiv.Rows.Add(selRow.ItemArray);
                dtSubdivFT.Rows[dtSubdivFT.Rows.IndexOf(selRow)].Delete();
                dgSubdivFT.Focus();
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format(Queries.GetQuery("Table/UpdateSubdivFT.sql"), Connect.Schema);
            com.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            com.Parameters.Add("p_date_advance", OracleDbType.Date);
            com.Parameters.Add("p_date_salary", OracleDbType.Date);
            com.Parameters.Add("p_sign_processing", OracleDbType.Decimal).Value = 0;
            OracleCommand comDel = new OracleCommand("", Connect.CurConnect);
            comDel.BindByName = true;
            comDel.CommandText = string.Format(
                "delete from {0}.SUBDIV_FOR_TABLE where SUBDIV_ID = :p_SUBDIV_ID", Connect.Schema);
            comDel.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            for (int i = 0; i < dtSubdivFT.Rows.Count; i++)
            {
                if (dtSubdivFT.Rows[i].RowState == DataRowState.Added)
                {
                    com.Parameters["p_SUBDIV_ID"].Value = dtSubdivFT.Rows[i]["SUBDIV_ID"];
                    com.ExecuteNonQuery();
                }
                else
                    if (dtSubdivFT.Rows[i].RowState == DataRowState.Deleted)
                    {
                        comDel.Parameters["p_SUBDIV_ID"].Value = dtSubdivFT.Rows[i]["SUBDIV_ID", DataRowVersion.Original];
                        comDel.ExecuteNonQuery();
                    }
            }
            Connect.Commit();
        }
    }
}
