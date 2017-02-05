using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;

namespace Kadr.Shtat
{
    public partial class DropStaff_id : Form
    {
        public SelectSubdivFromTree frm;
        string filter, deleting_staffs_id;
        public DropStaff_id(string del_staffs_id)
        {
            filter = "";
            deleting_staffs_id = del_staffs_id;
            InitializeComponent();
        }
        
        private void btChoose_Click(object sender, EventArgs e)
        {
            FindPosCode frmFindPos = new FindPosCode(Connect.CurConnect);
            frmFindPos.ShowDialog();
            if (frmFindPos.pos_code!="-1") code_pos.Text = frmFindPos.pos_code;
        }

        private void DropStaff_id_Load(object sender, EventArgs e)
        {
           if (deleting_staffs_id!=null)
               this.InitField(deleting_staffs_id);
           GridResult.Columns.Clear();
           GridResult.Columns.Add(new DataGridViewCheckBoxColumn());
           GridResult.Columns[0].Name="check";
           GridResult.Columns[0].HeaderText = "Отметка об удалении";
           GridResult.Columns.Add("subdiv_name","Подразделение");
           GridResult.Columns.Add("code_pos", "Код проф");
           GridResult.Columns.Add("pos_name", "Профессия");
           GridResult.Columns.Add("code_degree", "Катег.");
           GridResult.Columns.Add("date_begin", "Введено");
           //GridResult.Columns.Add("date_end", "Исключить"); 
           GridResult.Columns.Add("fio", "ФИО");
           GridResult.Columns.Add("staffs_id", "");
           GridResult.Columns.Add("staff_sign", "");
           GridResult.Columns.Add("fl", "");
           GridResult.Columns["staffs_id"].Visible = false;
           GridResult.Columns["staff_sign"].Visible = false;
           GridResult.Columns["fl"].Visible = false;
           for (int i = 1; i < GridResult.ColumnCount; i++)
               GridResult.Columns[i].ReadOnly = true;
           btFind_Click(null, null);

        }

        private void btFind_Click(object sender, EventArgs e)
        {
            filter = string.Format("select subdiv_name,code_pos,pos_name,code_degree,to_char(date_begin_staff,'DD.MM.YYYY'),emp_last_name||' '||emp_first_name||' '||emp_middle_name fio, s.staffs_id,staff_sign,   " +
                "case when s.staff_sign=1 and exists(select * from {0}.confirm_staffs cs where cs.staffs_id=s.staffs_id and nvl(sign_confirm,-1)=-1 ) then 1 else 0 end fl "+
                "from  {0}.staffs s left join (select * from {0}.transfer where sign_cur_work=1) t on (t.staffs_id=s.staffs_id) left join {0}.subdiv on (s.subdiv_id=subdiv.subdiv_id) left join {0}.degree on (s.degree_id=degree.degree_id) left join {0}.position p on (s.pos_id=p.pos_id) left join {0}.emp on (emp.per_num=t.per_num)" +
                "where staff_sign!=2 and " + (per_num.Text.Length > 0 ? " per_num=" + per_num.Text + " and " : "") + (FormMain.ShtatFilter.subdiv_id != "0" ? " subdiv_id in (select subdiv_id from {0}.subdiv start with subdiv_id=" + FormMain.ShtatFilter.subdiv_id + " connect by prior subdiv_id=parent_id) and " : "") +
                (FormMain.ShtatFilter.DegreeName != "Все" ? " upper(degree_name)=upper('" + FormMain.ShtatFilter.DegreeName + "') and " : "") +
                " upper(code_pos) like upper('%" + code_pos.Text + "%') and ", DataSourceScheme.SchemeName);
            filter = filter.Substring(0, filter.Length - 4);
            filter+=" order by 3,1 ";
            OracleDataAdapter adapter = new OracleDataAdapter(filter, Connect.CurConnect);
            adapter.SelectCommand.BindByName = true;
            DataTable table = new DataTable();
            adapter.Fill(table);
            GridResult.Rows.Clear();
          
            
            for (int i = 0; i < table.Rows.Count; i++)
            {
                object[] t=new object[] {false};
                GridResult.Rows.Add(new DataGridViewRow());
                GridResult["check",i].Value=false;
                for (int j = 1; j < GridResult.ColumnCount; j++)
                    GridResult[j, i].Value = table.Rows[i][j-1].ToString();
                if (table.Rows[i]["staffs_id"].ToString() == deleting_staffs_id)
                {
                    GridResult.Rows[i].Selected = true;
                    GridResult.FirstDisplayedScrollingRowIndex = i;
                    GridResult["check", i].Value = true;
                }
                if (table.Rows[i]["fl"].ToString()=="1")
                {
                    GridResult.Rows[i].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    GridResult.Rows[i].DefaultCellStyle.SelectionForeColor = Color.LightGreen;
                    GridResult.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromArgb(100,100,150);
                    GridResult["check", i].Value = true;
                    GridResult["check", i].ReadOnly = true;
                }
                else
                if (table.Rows[i]["staff_sign"].ToString() == "1")
                {
                    GridResult.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    GridResult.Rows[i].DefaultCellStyle.SelectionForeColor = Color.LightGreen;
                }
            }
            if (deleting_staffs_id!="-1")            
                deleting_staffs_id = "-1";
            Settings.SetDataGridColumnAlign(ref GridResult);
            Settings.SetDataGridCoumnWidth(ref GridResult);
            Settings.SetDataGridCaption(ref GridResult);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            GridResult.CommitEdit(DataGridViewDataErrorContexts.Commit);
            string del="",isk="";
            int k=0,s=0;
            List<string> l= new List<string>();
            l.Clear();
            for (int i=0;i<GridResult.RowCount;i++)
                if (GridResult["check", i].Value.ToString() == "True" && !GridResult["check", i].ReadOnly)
                {
                    if (GridResult["staff_sign",i].Value.ToString()=="1")
                    {
                        isk+=GridResult["staffs_id",i].Value.ToString()+",";
                        l.Add(GridResult["staffs_id", i].Value.ToString());
                        ++k;
                    }
                    else
                    {
                        del+=GridResult["staffs_id",i].Value.ToString()+",";
                        ++s;
                    }
                }
            if (s+k>0)
            {
                if ((isk != "" || del != "") && MessageBox.Show(this, string.Format("Вы действительно хотите исключить выбранные штатные единицы ({0} шт.)?", s + k), "АРМ Кадры", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    OracleCommand cmd = new OracleCommand("", Connect.CurConnect);
                    cmd.BindByName = true;
                    if (isk != "")
                    {
                        SelectPeriod frmper = new SelectPeriod(false, true,DateTime.Now.ToShortDateString(),"");
                        frmper.caption.Text = "Укажите дату исключения единиц(ы). \n Если исключение временное, то укажите  и дату окончания исключения.";
                        frmper.ShowDialog(this);
                        if (frmper.period_begin != "-1")
                        {
                            if (isk != "")
                            {
                                if (frmper.period_end == "-1")
                                {

                                    
                                    for (int i = 0; i < l.Count; i++)
                                    {
                                        cmd.CommandText = string.Format("insert into {0}.confirm_staffs VALUES({0}.confirm_staffs_seq.nextval,{1},null,null,null,null,-1,to_date('{2}','DD-MM-YYYY'),-1)", DataSourceScheme.SchemeName,l[i],frmper.period_begin);
                                        cmd.ExecuteNonQuery();
                                        cmd.CommandText = string.Format("insert into {0}.AUDIT_TABLE(audit_id,table_name,primary_key,user_change,date_change,type_audit,primary_key_old) values ({0}.audit_id_seq.nextval,'staffs','{1}',(select user from dual),SYSDATE,'DELETE',null)", DataSourceScheme.SchemeName, l[i]);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < l.Count; i++)
                                    {
                                        OracleDataReader r1 = new OracleCommand(string.Format("select {0}.staffs_id_seq.nextval from dual", DataSourceScheme.SchemeName), Connect.CurConnect).ExecuteReader();
                                        r1.Read();
                                        OracleDataReader r = new OracleCommand(string.Format("select * from {0}.staffs where staffs_id={1}", DataSourceScheme.SchemeName, l[i]), Connect.CurConnect).ExecuteReader();
                                        r.Read();
                                        cmd.CommandText = string.Format("insert into {0}.staffs VALUES(" +
                                               "{24},'{1}','{2}','{3}','{4}','{5}',to_char(to_date('{6}','DD.MM.YYYY HH24:MI:SS'),'DD.MM.YYYY'),to_char(to_date('{7}','DD.MM.YYYY HH24:MI:SS'),'DD.MM.YYYY'),to_char(to_date('{8}','DD.MM.YYYY HH24:MI:SS'),'DD.MM.YYYY'),'{9}','{10}','{11}','{12}','{13}','{14}',2,'{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                               DataSourceScheme.SchemeName, r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(),
                                                                r[5].ToString(), r[6].ToString(), frmper.period_begin, frmper.period_end,
                                                                r[9].ToString(), r[10].ToString(), r[11].ToString(), r[12].ToString(),
                                                                r[13].ToString(), "0", r[15].ToString(), r[16].ToString(),
                                                                r[17].ToString(), r[18].ToString(), r[19].ToString(),
                                                                r[20].ToString(), l[i], r[22].ToString(),r[23].ToString(),
                                                                r1[0].ToString()
                                                                );
                                        
                                        cmd.ExecuteNonQuery();
                                        cmd.CommandText = string.Format("insert into {0}.confirm_staffs VALUES({0}.confirm_staffs_seq.nextval,{1},null,null,null,null,-1,to_date('{2}','DD-MM-YYYY'),-1)", DataSourceScheme.SchemeName, r1[0].ToString(), frmper.period_begin);
                                        cmd.ExecuteNonQuery();
                                        cmd.CommandText = string.Format("insert into {0}.AUDIT_TABLE(audit_id,table_name,primary_key,user_change,date_change,type_audit,primary_key_old) values ({0}.audit_id_seq.nextval,'staffs','{1}',(select user from dual),SYSDATE,'DELETE',null)", DataSourceScheme.SchemeName, l[i]);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            if (del!="")
                            {
                                cmd.CommandText = string.Format("delete from {0}.confirm_staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, del.Substring(0, del.Length - 1));
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = string.Format("delete from {0}.audit_table where table_name='STAFFS' and PRIMARY_KEY in ({1})", DataSourceScheme.SchemeName, del.Substring(0, del.Length - 1));
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = string.Format("delete from {0}.staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, del.Substring(0, del.Length - 1));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        if (del != "")
                        {
                            cmd.CommandText = string.Format("delete from {0}.confirm_staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, del.Substring(0, del.Length - 1));
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = string.Format("delete from {0}.audit_table where table_name='STAFFS' and PRIMARY_KEY in ({1})", DataSourceScheme.SchemeName, del.Substring(0, del.Length - 1));
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = string.Format("delete from {0}.staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, del.Substring(0, del.Length - 1));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    btFind_Click(null, null);
                };
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
