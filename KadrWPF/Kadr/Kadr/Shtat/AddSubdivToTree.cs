using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using Oracle.DataAccess.Client;
namespace Kadr.Shtat
{
    public partial class AddSubdivToTree : Form
    {
        public OracleConnection connect;
        public string who_sender, subdiv_id;
        public AddSubdivToTree(OracleConnection cnt, string sndr, string subdiv)
        {
            subdiv_id = subdiv;
            connect = cnt;
            who_sender = sndr;
            InitializeComponent();
        }

        private void AddSubdivToTree_Load(object sender, EventArgs e)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select type_name from {0}.work_type", DataSourceScheme.SchemeName), connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            type_work.Items.Clear();
            
            for (int i = 0; i < table.Rows.Count; i++)
                type_work.Items.Add(table.Rows[i][0].ToString());
            adapter.SelectCommand.CommandText = string.Format("select type_subdiv_name from {0}.type_subdiv", DataSourceScheme.SchemeName);
            table.Reset();
            adapter.Fill(table);
            type_subdiv.Items.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
                type_subdiv.Items.Add(table.Rows[i][0].ToString());
            adapter.SelectCommand.CommandText = string.Format("select code_subdiv from {0}.subdiv where subdiv_id={1}", DataSourceScheme.SchemeName, subdiv_id);
            OracleDataReader reader = adapter.SelectCommand.ExecuteReader();
            if (reader.Read()&& subdiv_id!="0"&&who_sender!="edit")
                labelCode.Text = reader["code_subdiv"].ToString()+"/";
            code_subdiv.Text = LibraryKadr.Library.NVL(
                new OracleCommand(string.Format("select max(to_number(code_sub1))+1 from  (select  case " +
            "when instr(substr(code_subdiv,LENGTH('{1}')+2),'/')>0 then substr(substr(code_subdiv,LENGTH('{1}')+2),1,instr(substr(code_subdiv,LENGTH('{1}')+2),'/')) " +
            "else          substr(code_subdiv,LENGTH('{1}')+2) end code_sub1 from {0}.subdiv where concat('{1}','/')=substr(code_subdiv,1,LENGTH('{1}')+1 ))",
            DataSourceScheme.SchemeName, reader[0].ToString()), connect).ExecuteScalar(), "");
            if (who_sender == "edit")
            {
                adapter.SelectCommand.CommandText = string.Format("select subdiv_name,type_name,type_subdiv_name,code_subdiv from {0}.subdiv left join {0}.work_type on (subdiv.work_type_id=work_type.work_type_id) left join {0}.type_subdiv on (subdiv.type_subdiv_id=type_subdiv.type_subdiv_id) where subdiv_id={1}", DataSourceScheme.SchemeName, subdiv_id);
                table.Reset();
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    subdiv_name.Text = table.Rows[0][0].ToString();
                    type_work.Text = table.Rows[0][1].ToString();
                    type_subdiv.Text = table.Rows[0][2].ToString();
                    string s = table.Rows[0][3].ToString();
                    if (subdiv_id != "0")
                    {
                        if (s.LastIndexOf('/') > -1)
                        {
                            labelCode.Text = s.Substring(0, s.LastIndexOf('/') + 1);
                            code_subdiv.Text = s.Substring(s.LastIndexOf('/') + 1, s.Length - s.LastIndexOf('/') - 1);
                        }
                        else code_subdiv.Text = s;
                    }
                }
                label2.Text = "Название подразделения";
                btOk.Text = "Сохранить";
                this.Text = "Редактирование подразделения";
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (subdiv_name.Text == "")
            {
                MessageBox.Show("Вы не ввели название подразделения!");
                return;
            }
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connect;
            if (who_sender == "edit")
                cmd.CommandText = string.Format("update {0}.subdiv set code_subdiv='{1}', subdiv_name='{2}',sub_actual_sign=1,work_type_id=(select work_type_id from {0}.work_type where type_name='{3}'),type_subdiv_id=(select type_subdiv_id from {0}.type_subdiv where type_subdiv_name='{4}') where subdiv_id={5}", DataSourceScheme.SchemeName, labelCode.Text+code_subdiv.Text, subdiv_name.Text, type_work.Text, type_subdiv.Text, subdiv_id);
            else
                cmd.CommandText = string.Format("insert into {0}.subdiv(subdiv_id,code_subdiv,subdiv_name,sub_actual_sign,work_type_id,type_subdiv_id,parent_id)" +
                                                "values ({0}.subdiv_id_seq.nextval,'{1}','{2}',1,(select work_type_id from {0}.work_type where type_name='{3}'),(select type_subdiv_id from {0}.type_subdiv where type_subdiv_name='{4}'),{5})", DataSourceScheme.SchemeName, labelCode.Text+code_subdiv.Text, subdiv_name.Text, type_work.Text, type_subdiv.Text, subdiv_id);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Подразделение с таким кодом уже существует!");
                return;
            }
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
