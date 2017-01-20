using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibraryKadr;
using Oracle.DataAccess.Client;
using System.Data;

namespace WpfControlLibrary.Table
{
    /// <summary>
    /// Interaction logic for List_Subdiv_Table_Viewer.xaml
    /// </summary>
    public partial class List_Subdiv_Table_Viewer : Window
    {
        OracleDataTable dtSubdiv, dtSubdivFT;
        public List_Subdiv_Table_Viewer()
        {
            InitializeComponent();
            dtSubdiv = new OracleDataTable("", Connect.CurConnect);
            dtSubdiv.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SubdivFT.sql"), Connect.Schema);
            dtSubdiv.Fill();
            dgSubdiv.DataContext = dtSubdiv.DefaultView;
            dtSubdivFT = new OracleDataTable("", Connect.CurConnect);
            dtSubdivFT.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("Table/SubdivFTEdit.sql"), Connect.Schema);
            dtSubdivFT.Fill();
            dgSubdivFT.DataContext = dtSubdivFT.DefaultView;
        }

        private void btAddSubdiv_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubdiv.SelectedCells.Count > 0)
            {
                DataRow selRow = dtSubdiv.Select("SUBDIV_ID = " +
                    ((DataRowView)dgSubdiv.SelectedCells[0].Item)["SUBDIV_ID"].ToString()).First();
                dtSubdivFT.Rows.Add(selRow.ItemArray);
                dtSubdiv.Rows.Remove(selRow);
                dgSubdiv.Focus();
            }
        }

        private void btDeleteSubdiv_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubdivFT.SelectedCells.Count > 0)
            {
                DataRow selRow = dtSubdivFT.Select("SUBDIV_ID = " +
                    ((DataRowView)dgSubdivFT.SelectedCells[0].Item)["SUBDIV_ID"].ToString()).First();
                dtSubdiv.Rows.Add(selRow.ItemArray);
                dtSubdivFT.Rows[dtSubdivFT.Rows.IndexOf(selRow)].Delete();
                dgSubdivFT.Focus();
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand com = new OracleCommand(string.Format(
                "INSERT INTO {0}.SUBDIV_FOR_TABLE(SUBDIV_FOR_TABLE_ID, SUBDIV_ID) values({0}.SUBDIV_FOR_TABLE_ID_seq.nextval,:p_SUBDIV_ID)", 
                Connect.Schema), Connect.CurConnect);
            com.BindByName = true;
            com.Parameters.Add("p_SUBDIV_ID", OracleDbType.Decimal);
            OracleCommand comDel = new OracleCommand(string.Format(
                "delete from {0}.SUBDIV_FOR_TABLE where SUBDIV_ID = :p_SUBDIV_ID", 
                Connect.Schema), Connect.CurConnect);
            comDel.BindByName = true;
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
            this.DialogResult = true;
            this.Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void dgSubdiv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btAddSubdiv_Click(null, null);
        }

        private void dgSubdivFT_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btDeleteSubdiv_Click(null, null);
        }
    }
}
