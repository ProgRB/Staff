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
    public partial class Access_Gr_Subdiv : Form
    {
        int gr_work_id;
        public Access_Gr_Subdiv(int _gr_work_id)
        {
            InitializeComponent();
            gr_work_id = _gr_work_id;            

            /// Создание списка подразделений
            OracleDataTable dtSubdiv = new OracleDataTable("", Connect.CurConnect);
            dtSubdiv.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Access_GR_Subdiv.sql"),
                DataSourceScheme.SchemeName);
            dtSubdiv.SelectCommand.Parameters.Add("p_gr_work_id", gr_work_id);
            dtSubdiv.Fill();
            /// Создание списка доступных подразделений для данного графика
            OracleDataTable dtAccessSubdiv = new OracleDataTable("", Connect.CurConnect);
            dtAccessSubdiv.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/Access_SubdivGR.sql"),
                DataSourceScheme.SchemeName);
            dtAccessSubdiv.SelectCommand.Parameters.Add("p_gr_work_id", gr_work_id);
            dtAccessSubdiv.Fill();
            foreach (DataRow row in dtSubdiv.Rows)
            {
                lvSubdiv.Items.Add(new ListViewItem(new string[] 
                    {row[0].ToString(), row[1].ToString(), row[2].ToString()}));
            }
            foreach (DataRow row in dtAccessSubdiv.Rows)
            {
                lvAccessSubdiv.Items.Add(new ListViewItem(new string[] 
                    { row[0].ToString(), row[1].ToString(), row[2].ToString() }));
            }  
        }

        private void btAddSubdiv_Click(object sender, EventArgs e)
        {
            if (lvSubdiv.SelectedItems != null)
            {
                foreach (ListViewItem item in lvSubdiv.SelectedItems)
                {
                    lvSubdiv.Items.Remove(item);
                    lvAccessSubdiv.Items.Add(item);
                    lvAccessSubdiv.Items[lvAccessSubdiv.Items.IndexOf(item)].Selected = false;
                }
            }
        }

        private void btDeleteSubdiv_Click(object sender, EventArgs e)
        {
            if (lvAccessSubdiv.SelectedItems != null)
            {
                foreach (ListViewItem item in lvAccessSubdiv.SelectedItems)
                {
                    lvAccessSubdiv.Items.Remove(item);
                    //lvSubdiv.Items.Insert(Convert.ToInt32(item.SubItems[1].Text) - 1, item);
                    lvSubdiv.Items.Insert(lvSubdiv.Items.Count, item);
                    lvSubdiv.Items[lvSubdiv.Items.IndexOf(item)].Selected = false;
                }
            }
        }

        private void btAddSubdivAll_Click(object sender, EventArgs e)
        {
            if (lvSubdiv.Items != null)
            {
                foreach (ListViewItem item in lvSubdiv.Items)
                {
                    lvSubdiv.Items.Remove(item);
                    lvAccessSubdiv.Items.Add(item);
                    lvAccessSubdiv.Items[lvAccessSubdiv.Items.IndexOf(item)].Selected = false;
                }
            }
        }

        private void btDeleteSubdivAll_Click(object sender, EventArgs e)
        {
            if (lvAccessSubdiv.Items != null)
            {
                foreach (ListViewItem item in lvAccessSubdiv.Items)
                {
                    lvAccessSubdiv.Items.Remove(item);
                    //lvSubdiv.Items.Insert(Convert.ToInt32(item.SubItems[1].Text) - 1, item);
                    lvSubdiv.Items.Insert(lvSubdiv.Items.Count, item);
                    lvSubdiv.Items[lvSubdiv.Items.IndexOf(item)].Selected = false;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.BindByName = true;
            com.CommandText = string.Format("delete from {0}.ACCESS_GR_WORK where GR_WORK_ID = :p_gr_work_id",
                DataSourceScheme.SchemeName);
            com.Parameters.Add("p_gr_work_id", gr_work_id);
            com.ExecuteNonQuery();
            com = new OracleCommand("", Connect.CurConnect);
            com.CommandText = "commit";
            com.ExecuteNonQuery();

            ACCESS_GR_WORK_seq access_gr_work = new ACCESS_GR_WORK_seq(Connect.CurConnect);
            foreach (ListViewItem item in lvAccessSubdiv.Items)
            {
                access_gr_work.AddNew();
                ((CurrencyManager)BindingContext[access_gr_work]).Position =
                    ((CurrencyManager)BindingContext[access_gr_work]).Count;
                ((ACCESS_GR_WORK_obj)((CurrencyManager)BindingContext[access_gr_work]).Current).GR_WORK_ID = 
                    gr_work_id;
                ((ACCESS_GR_WORK_obj)((CurrencyManager)BindingContext[access_gr_work]).Current).SUBDIV_ID =
                    Convert.ToInt32(item.SubItems[2].Text);                
            }
            access_gr_work.Save();
            Connect.Commit();
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
