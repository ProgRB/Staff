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
    public partial class filter : Form
    {
        BindingSource bs2;
        FormMain form_spr;
        public filter(FormMain _spr, BindingSource _bs)
        {
            InitializeComponent();
            form_spr = _spr;
            bs2 = _bs;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string str_filter = "";

        private void button1_Click(object sender, EventArgs e)
        {

            str_filter = "";

            //табельный
            if (tB_per_num.Text != "")
            {
                str_filter += "per_num = '" + tB_per_num.Text.PadLeft(5,'0') + "'";
            }
            //подразделение
            if (tB_code_subdiv.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and code_subdiv = '" + tB_code_subdiv.Text.PadLeft(3, '0') + "'" : "" + "code_subdiv = '" + tB_code_subdiv.Text.PadLeft(3, '0') + "'";
            }
            //фамилия
            if (tB_emp_last_name.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and emp_last_name = upper('" + tB_emp_last_name.Text + "')" : "emp_last_name = upper('" + tB_emp_last_name.Text + "')";
                //str_sort += str_sort.Length > 0 ? ", emp_last_name" : "emp_last_name";
            }
            //имя
            if (tB_emp_first_name.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and emp_first_name = upper('" + tB_emp_first_name.Text + "')" : "emp_first_name = upper('" + tB_emp_first_name.Text + "')";
                //str_sort += str_sort.Length > 0 ? ", emp_first_name" : "emp_first_name";
            }
            //отчество
            if (tB_emp_middle_name.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and emp_middle_name = upper('" + tB_emp_middle_name.Text + "')" : "emp_middle_name = upper('" + tB_emp_middle_name.Text + "')";
                //str_sort += str_sort.Length > 0 ? ", emp_middle_name" : "emp_middle_name";
            }
            //пол мужской
            if (cB__pol_m.Checked == true )
            {
                str_filter += str_filter.Length > 0 ? " and emp_sex = 'М'" : " emp_sex = 'М' ";
            }
            //пол женский
            if (cB_pol_j.Checked == true)
            {
                str_filter += str_filter.Length > 0 ? " and emp_sex = 'Ж'" : " emp_sex = 'Ж' ";
            }
            //совместители
            if (cB_sovm.Checked == true)
            {
                str_filter += str_filter.Length > 0 ? " and sign_comb = 1" : " sign_comb = 1";
            }

            ////декретные
            //if (cB_sovm.Checked == true)
            //{
            //    str_filter += str_filter.Length > 0 ? " and sign_comb is not null" : " sign_comb is not null ";
            //}

            //профсоюз
            /*if (ch_Sign_ProfUnion.Checked == true)
            {
                str_filter += str_filter.Length > 0 ? " and SIGN_PROFUNION = 1" : " SIGN_PROFUNION = 1 ";
            }*/
            if (chSign_ProfUnion.CheckState != CheckState.Indeterminate)
            {                
                str_filter += string.Format(@"{0} 
                    {1}.GET_SIGN_PROFUNION(V.WORKER_ID, sysdate) = {2} ",
                    (str_filter.Length != 0 ? " and" : ""), Connect.Schema, (chSign_ProfUnion.Checked ? 1 : 0));
            }

            //пенсионеры
            if (cB_pens.Checked == true)
            {
                str_filter += str_filter.Length > 0 ? " and RETIRER_SIGN = 1" : " RETIRER_SIGN = 1 ";
            }

            // по дате рождения
           
                //dat_po = (DateTime)dateEditor2.Date.Value;
                //dat_s = (DateTime)dateEditor1.Date; 


            if (dateEditor1.Date != null && dateEditor2.Date != null)
            {
                if (dateEditor1.Date > dateEditor2.Date)
                {
                    MessageBox.Show("Неверно задан диапозон даты рождения.\nПроверьте правильность введенных данных и попробуйте заново.", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                str_filter += str_filter.Length > 0 ?
                    " and EMP_BIRTH_DATE between to_date('" + dateEditor1.Date.Value.ToShortDateString() + "','dd.MM.yyyy') and to_date('" + dateEditor2.Date.Value.ToShortDateString() + "','dd.MM.yyyy') "
                  :
                    " EMP_BIRTH_DATE between to_date('" + dateEditor1.Date.Value.ToShortDateString() + "','dd.MM.yyyy') and to_date('" + dateEditor2.Date.Value.ToShortDateString() + "','dd.MM.yyyy') ";

            }
            else if (dateEditor1.Date != null)
            {
                str_filter += str_filter.Length > 0 ?
                    " and EMP_BIRTH_DATE >= to_date('" + dateEditor1.Date.Value.ToShortDateString() + "','dd.MM.yyyy') "
                    :
                    " EMP_BIRTH_DATE >= to_date('" + dateEditor1.Date.Value.ToShortDateString() + "','dd.MM.yyyy') ";

            }
            else if (dateEditor2.Date != null)
            {
                str_filter += str_filter.Length > 0 ?
                    " and EMP_BIRTH_DATE <= to_date('" + dateEditor2.Date.Value.ToShortDateString() + "','dd.MM.yyyy') "
                    :
                    " EMP_BIRTH_DATE <= to_date('" + dateEditor2.Date.Value.ToShortDateString() + "','dd.MM.yyyy') ";

            }


            // Фильтрация по возрасту сотрудника
            if (tB_ot.Text != "" && tB_do.Text != "")
            {
                if (Convert.ToInt32(tB_ot.Text) > Convert.ToInt32(tB_do.Text))
                {
                    MessageBox.Show("Неверно задан возраст сотрудников.\nПроверьте правильность введенных данных и попробуйте заново.", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                str_filter += str_filter.Length > 0 ?
                    " and EMP_BIRTH_DATE between to_date('"+DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_do.Text))+"','dd.MM.yyyy') "+
                    " and to_date('"+DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_ot.Text))+"'dd.MM.yyyy') "
                  : 
                    " EMP_BIRTH_DATE between to_date('"+DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_do.Text))+"','dd.MM.yyyy') "+
                    " and to_date('"+DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_ot.Text))+"','dd.MM.yyyy') ";

            }
            else if (tB_ot.Text != "")
            {
                str_filter += str_filter.Length > 0 ?
                    " and EMP_BIRTH_DATE <= to_date('"+DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_ot.Text))+"','dd.MM.yyyy') "
                    : 
                    " EMP_BIRTH_DATE <=  to_date('"+DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_ot.Text))+"','dd.MM.yyyy') ";

            }
            else if (tB_do.Text != "")
            {
                str_filter += str_filter.Length > 0 ?
                    " and EMP_BIRTH_DATE >= to_date('" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_do.Text)) + "','dd.MM.yyyy') "
                    :
                    " EMP_BIRTH_DATE >=  to_date('" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tB_do.Text)) + "','dd.MM.yyyy') ";

            }
            //Разряд
            if (tbClassific.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and CLASSIFIC = " + tbClassific.Text : "CLASSIFIC = " + tbClassific.Text;
            }
            //Категория
            if (tbCode_degree.Text != "")
            {
                str_filter += str_filter.Length > 0 ? " and CODE_DEGREE = " + tbCode_degree.Text : "CODE_DEGREE = " + tbCode_degree.Text;
            }

            if (str_filter.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для фильтра!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }            

            OracleDataTable dt1 = new OracleDataTable("", Connect.CurConnect);
            dt1.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/spr.sql"), 
                DataSourceScheme.SchemeName, " V where " + str_filter);
            dt1.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = DateTime.Now;
            //dt1.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            dt1.Fill();            
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("В базе данных не найдена введенная информация!", "АРМ Бухгалтера-расчетчика", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
               bs2.DataSource = dt1;
               this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleDataTable dt1 = new OracleDataTable("", Connect.CurConnect);
            dt1.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/spr.sql"), 
                DataSourceScheme.SchemeName, "");
            dt1.SelectCommand.Parameters.Add("p_date", OracleDbType.Date).Value = DateTime.Now;
            //dt1.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value = Connect.UserId.ToUpper();
            dt1.Fill();
            bs2.DataSource = dt1;
            this.Close();
        }
    }
}
