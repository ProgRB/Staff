using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;

namespace Kadr
{
    public partial class find_spr : Form
    {
        grid_ras grid_form;
        BindingSource bs2;
        FormMain form_spr;
        string str_sort = "";
        string str_filter = "";
        public find_spr(FormMain _spr,BindingSource _bs,grid_ras _grid_form )
        {
            InitializeComponent();
            form_spr = _spr;
            bs2 = _bs;
            grid_form = _grid_form;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public static string f_per_num, f_emp_last_name, f_emp_middle_name, f_emp_first_name,
                             f_code_subdiv, f_subdiv_name;
        decimal findTrans;
        private void button2_Click(object sender, EventArgs e)
        {
            str_filter = "";
            str_sort = "";

            f_per_num = tB_per_num.Text;
            f_emp_last_name = tB_emp_last_name.Text;
            f_emp_middle_name = tB_emp_middle_name.Text;
            f_emp_first_name = tB_emp_first_name.Text;
            f_code_subdiv = tB_code_subdiv.Text;

            if (tB_per_num.Text != "")
            {
                //str_filter += "per_num = " + f_per_num;
                str_filter += str_filter.Length > 0 ? " and " + "'" + f_per_num.PadLeft(5,'0') + "'" : "per_num = " + "'" + f_per_num.PadLeft(5,'0') + "'";

                //str_sort += "per_num";
            }
            if (tB_code_subdiv.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and code_subdiv = " + "'" + f_code_subdiv.PadLeft(3, '0') + "'" : "code_subdiv = " + "'" + f_code_subdiv.PadLeft(3, '0') + "'";
                //str_sort += str_sort.Length > 0 ? ", code_subdiv" : "code_subdiv";
            }
            if (tB_emp_last_name.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and emp_last_name like " + "upper('" + f_emp_last_name + "%')" : "" + "emp_last_name like " + "upper('" + f_emp_last_name + "%')";
                str_sort += str_sort.Length > 0 ? ", emp_last_name" : "emp_last_name";
            }
            if (tB_emp_first_name.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and emp_first_name like " + "upper('" + f_emp_first_name + "%')" : "" + "emp_first_name like " + "upper('" + f_emp_first_name + "%')";
                str_sort += str_sort.Length > 0 ? ", emp_first_name" : "emp_first_name";
            }
            if (tB_emp_middle_name.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and emp_middle_name like " + "upper('" + f_emp_middle_name + "%')" : "" + "emp_middle_name like " + "upper('" + f_emp_middle_name + "%')";
                str_sort += str_sort.Length > 0 ? ", emp_middle_name" : "emp_middle_name";
            }


            if (str_filter.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OracleDataTable dt1 = new OracleDataTable("", Connect.CurConnect);
            dt1.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/spr.sql"),
                DataSourceScheme.SchemeName, " where " + str_filter);
            dt1.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = DateTime.Now;
            //dt1.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            dt1.Fill();  

            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("В базе данных не найдена введенная информация!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            findTrans = Convert.ToDecimal(dt1.Rows[0]["transfer_id"]);




            foreach (DataGridViewRow row in grid_form.dgEmp.Rows)
            {
                if (Convert.ToDecimal(row.Cells["transfer_id"].Value) == findTrans)
                {
                    bs2.Position = row.Index;
                    break;
                }
                
            }

            //if (grid_form.dgEmp.CurrentRow.Cells["per_num"].Value.ToString() != or_per_num)
            //{
            //    MessageBox.Show("В базе данных не найдена введенная информация!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    this.Close();
            //}
            

            this.Close();
        }
    }
}
