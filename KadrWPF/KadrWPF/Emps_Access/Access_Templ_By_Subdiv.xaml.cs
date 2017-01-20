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
using System.Data;
using Oracle.DataAccess.Client;

namespace WpfControlLibrary.Emps_Access
{
    /// <summary>
    /// Interaction logic for Access_Templ_By_Subdiv.xaml
    /// </summary>
    public partial class Access_Templ_By_Subdiv : Window
    {
        DataTable dtSubdiv, dtSubdivFT;
        OracleDataAdapter _daSubdiv;
        object _id_SHABLON_MAIN;
        public Access_Templ_By_Subdiv(DataTable dtAccess_Templ_Subdiv, object id_SHABLON_MAIN)
        {
            InitializeComponent();
            _id_SHABLON_MAIN = id_SHABLON_MAIN;
            dtSubdiv = new DataTable();
            _daSubdiv = new OracleDataAdapter(string.Format(
                Queries.GetQuery("PO/SelectSubdiv_For_Access_Template.sql"), Connect.Schema), Connect.CurConnect);;
            _daSubdiv.SelectCommand.Parameters.Add("p_ID_SHABLON_MAIN", OracleDbType.Decimal).Value = id_SHABLON_MAIN;
            _daSubdiv.Fill(dtSubdiv);
            dgSubdiv.DataContext = dtSubdiv.DefaultView;

            dtSubdivFT = dtAccess_Templ_Subdiv;
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
                @"BEGIN
                    {0}.PERCO_PKG.ACCESS_TEMPL_BY_SUBDIV_UPDATE(:ACCESS_TEMPL_BY_SUBDIV_ID,:ID_SHABLON_MAIN,:SUBDIV_ID);
                END;", 
                Connect.Schema), Connect.CurConnect);
            com.BindByName = true;
            com.Parameters.Add("ACCESS_TEMPL_BY_SUBDIV_ID", OracleDbType.Decimal, 0, "ACCESS_TEMPL_BY_SUBDIV_ID");
            com.Parameters.Add("ID_SHABLON_MAIN", OracleDbType.Decimal, 0, "ID_SHABLON_MAIN");
            com.Parameters.Add("SUBDIV_ID", OracleDbType.Decimal, 0, "SUBDIV_ID");
            OracleCommand comDel = new OracleCommand(string.Format(
                @"BEGIN
                    {0}.PERCO_PKG.ACCESS_TEMPL_BY_SUBDIV_DELETE(:ACCESS_TEMPL_BY_SUBDIV_ID);
                END;", 
                Connect.Schema), Connect.CurConnect);
            comDel.BindByName = true;
            comDel.Parameters.Add("ACCESS_TEMPL_BY_SUBDIV_ID", OracleDbType.Decimal, 0, "ACCESS_TEMPL_BY_SUBDIV_ID");
            for (int i = 0; i < dtSubdivFT.Rows.Count; i++)
            {
                if (dtSubdivFT.Rows[i].RowState == DataRowState.Added)
                {
                    com.Parameters["ID_SHABLON_MAIN"].Value = _id_SHABLON_MAIN;
                    com.Parameters["SUBDIV_ID"].Value = dtSubdivFT.Rows[i]["SUBDIV_ID"];
                    com.ExecuteNonQuery();
                }
                else
                    if (dtSubdivFT.Rows[i].RowState == DataRowState.Deleted)
                    {
                        comDel.Parameters["ACCESS_TEMPL_BY_SUBDIV_ID"].Value = dtSubdivFT.Rows[i]["ACCESS_TEMPL_BY_SUBDIV_ID", DataRowVersion.Original];
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
