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

namespace Kadr.Shtat
{
    public partial class FindEmpSingle : Form
    {
        public FindEmpSingle()
        {
            InitializeComponent();
            grid_resultFindEmpShtat.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
        }
        
        /// <summary>
        /// Null если перевод не выбран или айди перевода
        /// </summary>
        public object transfer_id
        {
            get
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                    return (grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView)["transfer_id"];
                else
                    return null;
            }
        }
        public string last_name
        {
            get
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                    return (grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView)["emp_last_name"].ToString();
                else return null;
            }
        }
        public string first_name
        {
            get
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                    return (grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView)["first_name"].ToString();
                else return null;
            }
        }
        public string per_num
        {
            get
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                    return (grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView)["per_num"].ToString();
                else return null;
            }
        }
        public string middle_name
        {
            get
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                    return (grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView)["middle_name"].ToString();
                else return null;
            }
        }
        public string fio
        {
            get 
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                {
                    DataRowView r = grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView;
                    return string.Format("{0} {1} {2}", r["emp_last_name"], r["emp_first_name"], r["emp_middle_name"]);
                }
                else return null;
            }
        }
        public string pos_name
        {
            get
            {
                if (grid_resultFindEmpShtat.CurrentRow != null)
                    return (grid_resultFindEmpShtat.CurrentRow.DataBoundItem as DataRowView)["pos_name"].ToString();
                else return null;
            }
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            if (Per_num.Text==""&& emp_last_name.Text=="" && emp_first_name.Text=="" && emp_middle_name.Text=="")
            {
                MessageBox.Show("Вы не ввели ни одного критерия поиска!");
                return;
            }
            string filter = string.Format("",DataSourceScheme.SchemeName);
            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"new\FindEmp.sql"),DataSourceScheme.SchemeName),Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p_per_num", Per_num.Text);
            a.SelectCommand.Parameters.Add("p_emp_last_name", emp_last_name.Text);
            a.SelectCommand.Parameters.Add("p_emp_first_name", emp_first_name.Text);
            a.SelectCommand.Parameters.Add("p_emp_middle_name", emp_middle_name.Text);
            DataTable table = new DataTable();
            a.Fill(table);
            grid_resultFindEmpShtat.AutoGenerateColumns = false;
            grid_resultFindEmpShtat.DataSource = table;
            //LibraryKadr.Settings.SetDataGridCaption(ref grid_resultFindEmpShtat);
            ColumnWidthSaver.FillWidthOfColumn(grid_resultFindEmpShtat);
        }

        private void emp_last_name_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btFind_Click(null, null);
        }
    }
}
