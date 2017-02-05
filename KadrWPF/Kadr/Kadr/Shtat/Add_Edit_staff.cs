using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using LibraryKadr;
using Staff;
using Oracle.DataAccess.Client;

namespace Kadr.Shtat
{
    public partial class Add_Edit_staff : Form
    {
        private TypeAdditionShtat type_call;
        public bool Result=false;
        public string staffs_id_edit,
            main_transfer_id = "-1";
        public List<string> List_edit;
        public Add_Edit_staff(TypeAdditionShtat tp ):this(tp,null,null)
        {
        }
        public Add_Edit_staff(TypeAdditionShtat tp,string Edit_staffs):this(tp,Edit_staffs,null)
        {
        }
        public Add_Edit_staff(TypeAdditionShtat tp,string edit_staff, List<string> _List_editable_staffs)
        {
            type_call = tp;//кто посыльщик-редактирование или же добавление кнопки
            staffs_id_edit = edit_staff;//айдишник штатной единицы.ноль- значит не выбрана. 
            List_edit = _List_editable_staffs;
            InitializeComponent();
            DataTable table = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(string.Format("select degree_name,degree_id from {0}.Degree", DataSourceScheme.SchemeName), Connect.CurConnect);
            adapter.Fill(table);
            Degree.DataSource = table;
            Degree.ValueMember = "degree_id";
            Degree.DisplayMember = "degree_name";

            adapter.SelectCommand.CommandText = string.Format("select null tar_name,null as tariff_grid_id from dual union all select '('||code_tariff_grid ||')'||tariff_grid_name tar_name,tariff_grid_id from {0}.tariff_grid", DataSourceScheme.SchemeName);
            DataTable t1 = new DataTable();
            adapter.Fill(t1);
            tariff_grid.DataSource = t1;
            tariff_grid.ValueMember = "tariff_grid_id";
            tariff_grid.DisplayMember = "tar_name";
            GridReplEmp.ColumnWidthChanged+=new DataGridViewColumnEventHandler(ColumnWidthSaver.SaveWidthOfColumn);
        }
        private void Add_staff_Load(object sender, EventArgs e)
        {
            if (type_call==TypeAdditionShtat.EditExists)
            {
                this.Text = "Редактирование штатной единицы";
                this.btSaveShtat.Visible = true;
                if (staffs_id_edit != null) this.InitField(staffs_id_edit, false);
            }
            else
                if (type_call ==  TypeAdditionShtat.NewEmpty)
                {
                    this.Text = "Добавление новой штатной единицы";
                    this.btAddShtat.Visible = true;
                    numberOfPackage.Enabled = true;
                }
                else
                    if (type_call== TypeAdditionShtat.NewByExists)
                    {
                        this.Text = "Добавление новой штатной единицы";
                        this.btAddShtat.Visible = true;
                        numberOfPackage.Enabled = true;
                        if (staffs_id_edit != null) this.InitField(staffs_id_edit, false);
                        main_transfer_id = "-1";
                    }
                    else
                        if (type_call == TypeAdditionShtat.EditGroup)
                        {
                            this.Text = "Редактирование группы единиц(различные поля не доступны для редактирования)";
                            btSaveGroupShtat.Visible = true;
                            numberOfPackage.Enabled = false;
                            numberOfPackage.Value = List_edit.Count;
                            if (List_edit != null) this.InitGroupField(List_edit);
                            main_transfer_id = "-1";
                        }
            if (type_call == TypeAdditionShtat.TransferToNew)
            {
                this.Text = "Перевод на новую штатную единицу";
                this.btExcludeAndSaveShtat.Visible = true;
                SetTemp.Visible = true;
                numberOfPackage.Enabled = true;
                if (staffs_id_edit != null) this.InitField(staffs_id_edit,false);
            }
            DateEndVacant.Enabled = vacant_sign.Checked;
            panel_main_commands.EnableByRules(false);
            if (type_call !=  TypeAdditionShtat.EditExists)
            {
                tpMainEmpShtat.Enabled = false;
                tpReplEmpShtat.Enabled = false;
            }
            else
            {
                tpMainEmpShtat.EnableByRules(true);
                tpReplEmpShtat.EnableByRules(true);
            }

        }

   /**********************************************************************************************************/
        private void InitEmpMainPage(string transfer_id)
        {
            if (type_call == TypeAdditionShtat.NewByExists)
                return;
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("new/GetMainEmpShtat.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_transfer_id", transfer_id);
            OracleDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                MainPer_num.Text = r["per_num"].ToString();
                MainEmpPhoto.Image = Staff.EmployeePhoto.GetPhoto(r["per_num"].ToString());
                MainFam.Text = r["emp_last_name"].ToString();
                MainName.Text = r["emp_first_name"].ToString();
                MainMiddleName.Text = r["emp_middle_name"].ToString();
                MainDate_hire.Text = r["Date_hire"].ToString();
                MainSalary.Text = r["salary"].ToString();
                MainComb_AddSht.Text = r["comb_addition"].ToString();
                MainHarmf_ad.Text = r["harmful_addition"].ToString();
                MainSecret_add.Text = r["Secret_addition"].ToString();
                MainDopSoglSht.Text = r["date_add_agree"].ToString();
            }
            else
            {
                MainPer_num.Text = "";
                MainEmpPhoto.Image = null;
                MainFam.Text = "";
                MainName.Text = "";
                MainMiddleName.Text = "";
                MainDate_hire.Text = "";
                MainSalary.Text = "";
                MainComb_AddSht.Text = "";
                MainHarmf_ad.Text = "";
                MainSecret_add.Text = "";
                MainDopSoglSht.Text = "";
            }

        }
        private void InitReplEmpPage(string transfer_id)
        {
            if (type_call == TypeAdditionShtat.NewByExists)
                return;
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("new/GetMainEmpShtat.sql"), DataSourceScheme.SchemeName),Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_transfer_id",transfer_id);
            OracleDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                Repl_per_num.Text = r["per_num"].ToString();
                ReplEmpPhoto.Image = Staff.EmployeePhoto.GetPhoto(r["per_num"].ToString());
                ReplFam.Text = r["emp_last_name"].ToString();
                Repl_name.Text = r["emp_first_name"].ToString();
                Repl_Middle_name.Text = r["emp_middle_name"].ToString();
                Repl_date_hire.Text = r["Date_hire"].ToString();
                Repl_salary.Text = r["salary"].ToString();
                Repl_comb_add.Text = r["comb_addition"].ToString();
                Repl_harm_add.Text = r["harmful_addition"].ToString();
                Repl_secret_add.Text = r["Secret_addition"].ToString();
                Repl_dop_sogl.Text = r["date_add_agree"].ToString();
            }
            else
            {
                Repl_per_num.Text = "";
                ReplEmpPhoto.Image = null;
                ReplFam.Text ="";
                Repl_name.Text ="";
                Repl_Middle_name.Text ="";
                Repl_date_hire.Text ="";
                Repl_salary.Text = "";
                Repl_comb_add.Text = "";
                Repl_harm_add.Text = "";
                Repl_secret_add.Text = "";
                Repl_dop_sogl.Text = "";
            }
        }
        
/************************************************ОСНОВНЫЕ КНОПКИ***********************************************/
        private void btOkAddShtat_Click(object sender, EventArgs e)
        {
            string sc=DataSourceScheme.SchemeName;
            if (SS1.subdiv_id == "0"&& code_pos.Text.Trim()=="")
            {
                MessageBox.Show("Заполните поля основных данных!");
                return;
            }
               OracleCommand cmd = new OracleCommand();
               string datebegintext = (!Date_Begin.MaskFull ? "" : Date_Begin.Text),
                      dateendtext = (!Date_End.MaskFull ? "" : Date_End.Text);
               cmd.Connection = Connect.CurConnect;
               for (int i = 0; i < numberOfPackage.Value; i++) //добавляем тогда в таблицы столько штатных единиц, сколько указано в поле.
               {
                   cmd.CommandText = string.Format("select {0}.staffs_id_seq.nextval from dual", DataSourceScheme.SchemeName);
                   OracleDataReader reader = cmd.ExecuteReader();
                   reader.Read();
                   DMLCommands.InsertInto("staffs", DataSourceScheme.SchemeName, Connect.CurConnect, new SP[]{
                        new SP("staffs_id",reader[0].ToString()),
                        new SP("pos_id",string.Format("(select max(pos_id) from {0}.position where upper(code_pos)=upper('{1}'))",sc,code_pos.Text),TypeColValue.Query),
                        new SP("subdiv_id",SS1.subdiv_id),
                        new SP("degree_id",Degree.SelectedValue),
                        new SP("date_begin_staff",datebegintext),
                        new SP("date_end_staff",dateendtext),
                        new SP("vacant_sign",(vacant_sign.Checked?"1":"0")),
                        new SP("date_end_vacant",(!DateEndVacant.MaskFull?"":DateEndVacant.Text)),
                        new SP("harmful_addition",harmful_add.Text),
                        new SP("comment_to_pos",comments_pos.Text),
                        new SP("type_person",(type_personal.Text == type_personal.Items[0].ToString() ? "0" : (type_personal.Text == type_personal.Items[1].ToString() ? "1" : "2"))),
                        new SP("order_id",string.Format("(select order_id from {0}.orders where order_name='{1}')",DataSourceScheme.SchemeName,order.Text),TypeColValue.Query),
                        new SP("staff_sign","1"),//тут при утверж. должно быть 0
                        new SP("type_staff",(dateendtext==""?"0":"1")),
                        new SP("tarif_grid_id",tariff_grid.SelectedValue),
                        new SP("tar_by_schema",tar_by_schema.Value),
                        new SP("classific",classific.Text),
                        new SP("add_exp_area",secret_add.Text)
                   });
                   //надо добавить в таблицу аудит что было редактирование.
                   cmd.CommandText = string.Format("insert into {0}.AUDIT_TABLE(audit_id,table_name,primary_key,user_change,date_change,type_audit,primary_key_old) values ({0}.audit_id_seq.nextval,'staffs','{1}',(select user from dual),SYSDATE,'INSERT',null)", DataSourceScheme.SchemeName, reader[0].ToString());
                   cmd.ExecuteNonQuery();
                   Connect.Commit();
               }
               Result = true;
               this.Close();
               if (numberOfPackage.Value > 1) MessageBox.Show("Штатные единицы успешно добавлены!"); else MessageBox.Show("Штатная единица успешно добавлена!");           
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (code_pos.Text.Trim() == "")
            {
                MessageBox.Show("Не выбран код профессии", "АРМ Штатное расписание");
                return;
            }
            OracleCommand cmd = new OracleCommand("", Connect.CurConnect);
            cmd.BindByName = true;
            string datebegintext = (!Date_Begin.MaskFull ? "" : Date_Begin.Text),
                   dateendtext = (!Date_End.MaskFull ? "" : Date_End.Text);
            //иначе старую обновляем без создания новой
            DMLCommands.Update("staffs", DataSourceScheme.SchemeName, Connect.CurConnect, new SP[]{
                new SP("pos_id",string.Format("(select max(pos_id) from {0}.position where upper(code_pos)=upper('{1}') and pos_actual_sign=1)",DataSourceScheme.SchemeName,code_pos.Text),TypeColValue.Query),
	            new SP("subdiv_id",SS1.subdiv_id),
                new SP("degree_id",Degree.SelectedValue),
	            new SP("vacant_sign",(vacant_sign.Checked ? "1" : "0")),
                new SP("date_end_vacant",(!DateEndVacant.MaskFull? "" : DateEndVacant.Text)),
                new SP("date_begin_staff",datebegintext),
                new SP("date_end_staff",dateendtext),	
                new SP("harmful_addition",harmful_add.Text),
                new SP("type_person",(type_personal.Text == type_personal.Items[0].ToString() ? "0" : (type_personal.Text == type_personal.Items[1].ToString() ? "1" : "2"))),
                new SP("comment_to_pos",comments_pos.Text),
                new SP("order_id",string.Format("(select max(order_id) from {0}.orders where order_name='{1}')",DataSourceScheme.SchemeName,order.Text),TypeColValue.Query),
                new SP("type_staff",(dateendtext.Length > 0 ? "1" : "0")),
                new SP("tarif_grid_id",tariff_grid.SelectedValue),
                new SP("tar_by_schema",tar_by_schema.Value),
                new SP("classific",classific.Text),
                new SP("add_exp_area",secret_add.Text)
            }," where staffs_id="+staffs_id_edit);

            if (main_transfer_id != "-1")
            {
                string s = Library.NVL(new OracleCommand(string.Format("select account_data_id from {0}.account_data where transfer_id={1} order by change_date ", DataSourceScheme.SchemeName, main_transfer_id), Connect.CurConnect).ExecuteScalar(), "");
                if (s != "")
                {
                    cmd.CommandText = string.Format("update {0}.account_data set comb_addition=to_number('{1}'),date_add_agree='{2}' where account_data_id={3}", DataSourceScheme.SchemeName, MainComb_AddSht.Text,(MainDopSoglSht.MaskFull?MainDopSoglSht.Text:""), s);
                    cmd.ExecuteNonQuery();
                }
            }
            Connect.Commit();
            Result = true;
            this.Close();
            //MessageBox.Show("Штатная единица успешно изменена!");
            
        }
        private void ExcludeAndSave_Click(object sender, EventArgs e)
        {
            if (SetTemp.Checked && !Date_Begin.MaskFull)
            {
                MessageBox.Show(this, "Уставлен флаг 'ВВЕСТИ ВРЕМЕННО' и не указана дата начала действия изменений!\nУкажите дату начала изменений или уберите отметку 'ВВЕСТИ ВРЕМЕННО'", "АРМ Кадры");
                return;
            }
            if (SetTemp.Checked && !Date_End.MaskFull)
            {
                MessageBox.Show(this, "Уставлен флаг 'ВВЕСТИ ВРЕМЕННО' и не указана дата окончания действия изменений!\nУкажите дату окончания изменений или уберите отметку 'ВВЕСТИ ВРЕМЕННО'", "АРМ Кадры");
                return;
            }
            string datebegintext = (!Date_Begin.MaskFull ? "" : Date_Begin.Text),
                   dateendtext = (!Date_End.MaskFull ? "" : Date_End.Text);
            string new_staffs_id = new OracleCommand(string.Format("select {0}.staffs_id_seq.nextval from dual", DataSourceScheme.SchemeName), Connect.CurConnect).ExecuteScalar().ToString();
            OracleCommand cmd = new OracleCommand("", Connect.CurConnect);
            if (!SetTemp.Checked)
            {
                cmd.CommandText = string.Format("insert into {0}.confirm_staffs VALUES({0}.confirm_staffs_seq.nextval,{1},null,null,null,null,-1,to_date('{2}','DD-MM-YYYY'),-1)", DataSourceScheme.SchemeName, staffs_id_edit, datebegintext);
                cmd.ExecuteNonQuery();
            }
            DMLCommands.InsertInto("staffs", DataSourceScheme.SchemeName, Connect.CurConnect, new SP[]{
                    new SP("staffs_id",new_staffs_id),
                    new SP("pos_id",string.Format("select pos_id from {0}.position where code_pos='{1}'",DataSourceScheme.SchemeName,code_pos.Text),TypeColValue.Query),
                    new SP("subdiv_id",SS1.subdiv_id),
                    new SP("degree_id",Degree.SelectedValue.ToString()),
                    new SP("vacant_sign",(vacant_sign.Checked ? "1" : "0")),
                    new SP("date_end_vacant",(DateEndVacant.Text.Replace('.',' ').Trim()==""?"":DateEndVacant.Text)),
                    new SP("date_begin_staff",datebegintext),
                    new SP("date_end_staff",dateendtext),
                    new SP("make_from_id",staffs_id_edit),
                    new SP("harmful_addition",harmful_add.Text),
                    new SP("type_person",(type_personal.Text == type_personal.Items[0].ToString() ? "0" : (type_personal.Text == type_personal.Items[1].ToString() ? "1" : "2"))),
                    new SP("comment_to_pos",comments_pos.Text),
                    new SP("order_id",string.Format("(select order_id from {0}.orders where order_name='{1}')",DataSourceScheme.SchemeName,order.Text),TypeColValue.Query),
                    new SP("staff_sign","0"),
                    new SP("type_staff",(dateendtext.Length > 0 ? "1" : "0")),
                    new SP("staff_parent_id",string.Format("(select staff_parent_id from {0}.staffs where staffs_id='{1}')",DataSourceScheme.SchemeName,staffs_id_edit)),
                    new SP("tariff_grid_id",string.Format("(select tariff_grid_id from {0}.tariff_grid where '('||code_tariff_grid||')'||tariff_grid_name='{1}')",DataSourceScheme.SchemeName,tariff_grid.Text)),
                    new SP("tar_by_schema",tar_by_schema.Text),
                    new SP("classific",classific.Text),
                    new SP("temp_over_id",(SetTemp.Checked ? staffs_id_edit : "null")),
                    new SP("add_exp_area",secret_add.Text)});
            /*cmd.CommandText = string.Format("update {0}.transfer set staffs_id={1} where staffs_id={2}", DataSourceScheme.SchemeName, new_staffs_id, staffs_id_edit);
            cmd.ExecuteNonQuery();*/
            //надо добавить в таблицу аудит что было редактирование.
            cmd.CommandText = string.Format("insert into {0}.AUDIT_TABLE(audit_id,table_name,primary_key,user_change,date_change,type_audit,primary_key_old) values ({0}.audit_id_seq.nextval,'staffs','{1}',(select user from dual),SYSDATE,'INSERT','{2}')", DataSourceScheme.SchemeName, new_staffs_id, staffs_id_edit);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            Result = true;
            MessageBox.Show(this, "Изменения проведены успешно", "АРМ Кадры");
        }
        private void btSaveGroup_Click(object sender, EventArgs e)
        {
            if (List_edit == null || List_edit.Count == 0)
            {
                MessageBox.Show("Вы не выбрали для редактирования штатную единицу");
                return;
            }
            string datebegintext = (Date_Begin.Text.Replace('.', ' ').Trim() == "" ? "" : Date_Begin.Text),
                   dateendtext = (Date_End.Text.Replace('.', ' ').Trim() == "" ? "" : Date_End.Text);
            OracleCommand cmd = new OracleCommand("", Connect.CurConnect);
            cmd.BindByName = true;
            //старую обновляем без создания новой
            for (int i = 0; i < List_edit.Count; i++)
            {
                DMLCommands.Update("staffs", DataSourceScheme.SchemeName, Connect.CurConnect, new SP[]{
                new SP("pos_id",string.Format("(select pos_id from {0}.position where upper(code_pos)=upper('{1}') and pos_actual_sign=1)",DataSourceScheme.SchemeName,code_pos.Text),TypeColValue.Query),
	            new SP("subdiv_id",SS1.subdiv_id),
                new SP("degree_id",Degree.SelectedValue),
	            new SP("vacant_sign",(vacant_sign.Checked ? "1" : "0")),
                new SP("date_end_vacant",(!DateEndVacant.MaskFull ? "" : DateEndVacant.Text)),
                new SP("date_begin_staff",datebegintext),
                new SP("date_end_staff",dateendtext),	
                new SP("harmful_addition",harmful_add.Text),
                new SP("type_person",(type_personal.Text == type_personal.Items[0].ToString() ? "0" : (type_personal.Text == type_personal.Items[1].ToString() ? "1" : "2"))),
                new SP("comment_to_pos",comments_pos.Text),
                new SP("order_id",string.Format("(select order_id from {0}.orders where order_name='{1}')",DataSourceScheme.SchemeName,order.Text),TypeColValue.Query),
                new SP("type_staff",(dateendtext.Length > 0 ? "1" : "0")),
                new SP("tarif_grid_id",tariff_grid.SelectedValue),
                new SP("tar_by_schema",tar_by_schema.Text),
                new SP("classific",classific.Text),
                new SP("add_exp_area",secret_add.Text)
            }," where staffs_id="+List_edit[i]);
            }
            this.Close();
            Result = true;
            MessageBox.Show("Штатные единицы успешно изменены(" + List_edit.Count.ToString() + " шт.)!");
        }

        private void btFindStaffOne_Click(object sender, EventArgs e)
        {
            FindStaffEd frmFind = new FindStaffEd(Connect.CurConnect);
            frmFind.ShowDialog();
            if (frmFind.staffs_id != "-1")
            {
                staffs_id_edit = frmFind.staffs_id.ToString();
                this.InitField(staffs_id_edit,true);
            }

        }
        private void bt_find_pos_Click(object sender, EventArgs e)
        {
            FindPosCode frmFindPos = new FindPosCode(Connect.CurConnect);
            frmFindPos.ShowDialog(this);
            if (frmFindPos.pos_code != "-1")
            {
                code_pos.Text = frmFindPos.pos_code;
                label_pos_name.Text = frmFindPos.pos_name;
            }
            else label_pos_name.Text = "Профессия не выбрана";
            code_pos.Focus();

        }
        
        private void vacant_sign_CheckedChanged(object sender, EventArgs e)
        {
                DateEndVacant.Enabled = vacant_sign.Checked;
        }
        private void codePos_Validating(object sender, CancelEventArgs e)
        {
            if (code_pos.Text != "")
            {
                OracleCommand cmd = new OracleCommand(string.Format("select code_pos,pos_name from {0}.position where code_pos='{1}' and pos_actual_sign=1", DataSourceScheme.SchemeName, code_pos.Text.Trim()), Connect.CurConnect);
                cmd.BindByName = true;
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    label_pos_name.Text = reader["pos_name"].ToString();
                else
                {
                    label_pos_name.Text = "Профессия не выбрана";
                    MessageBox.Show("Профессии с заданным кодом не существует!");
                    code_pos.Focus();
                }
            }
        }
        private void type_personal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 8)
                e.SuppressKeyPress = true;
            else type_personal.Text = "";
        }

        private void tariff_grid_TextChanged(object sender, EventArgs e)
        {
            OracleDataReader r = new OracleCommand(string.Format("select tar_classif from {0}.descr_tariff_grid where " +
                "tariff_grid_id=(select tariff_grid_id from {0}.tariff_grid where '('||code_tariff_grid||')'||tariff_grid_name='{1}') " +
                "and tar_date=(select max(tar_date) from {0}.descr_tariff_grid where  tariff_grid_id=(select tariff_grid_id from {0}.tariff_grid " +
                "where '('||code_tariff_grid||')'||tariff_grid_name='{1}'))", DataSourceScheme.SchemeName, tariff_grid.Text), Connect.CurConnect).ExecuteReader();
            classific.Items.Clear();
            while (r.Read())
            {
                classific.Items.Add(r[0].ToString());
            }
        }        

       
        private void order_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar!=8 && e.KeyChar > 48 && e.KeyChar < 58 && order.Text.Length == 3 && order.Text.Substring(0, 2) == "26")
            {
                string s, s1 = SS1.CodeSubdiv;
                s1=s1.Substring(0,(s1.IndexOf('/')>-1?s1.IndexOf('/'):s1.Length));
                s ="0000" + s1;
                order.Text +=e.KeyChar+ s.Substring(s.Length - 4, 4);
                e.Handled = false;
            }
        }

        private void btClearReplEmp_Click(object sender, EventArgs e)
        {
            if (GridReplEmp.SelectedRows.Count > 0 && MessageBox.Show("Вы действительно хотите отправить данную запись в архив?","АРМ Штатное расписание",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                DataGridViewRow r = GridReplEmp.SelectedRows[0];
                OracleCommand cmd = new OracleCommand(string.Format("UPDATE {0}.repl_emp set repl_actual_sign=0 where repl_emp_id={1}  ", DataSourceScheme.SchemeName, r.Cells["repl_emp_id"].Value.ToString()), Connect.CurConnect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                FillGridReplEmp(staffs_id_edit);
                ToolTip t = new ToolTip();
                t.IsBalloon = true;
                t.ToolTipIcon = ToolTipIcon.Info;
                t.Show("Запись отправлена в архив.\nИзменения сохранены", tpMainEmpShtat, 0, 0, 5000);
                Result = true;
            }
        }

        private void btSetReplEmp_Click(object sender, EventArgs e)
        {
            ReplEmpAdd rea = new ReplEmpAdd(null,main_transfer_id,true);
            if (rea.ShowDialog(this)== DialogResult.OK)
            {
                FillGridReplEmp(staffs_id_edit);
                ToolTip t = new ToolTip();
                t.IsBalloon = true;
                t.ToolTipIcon = ToolTipIcon.Info;
                t.Show("Добавлен новый замещающий сотрудник.\n Изменения сохранены", tpMainEmpShtat, 0, 0, 5000);
                Result = true;
            }
        }

        private void GridReplEmp_SelectionChanged(object sender, EventArgs e)
        {
            if (GridReplEmp.SelectedRows.Count > 0)
                InitReplEmpPage(GridReplEmp.SelectedRows[0].Cells["transfer_id"].Value.ToString());
            else
                InitReplEmpPage("-1");
        }

        private void btEditReplEmp_Click(object sender, EventArgs e)
        {
            if (GridReplEmp.CurrentRow!=null)
            {
                ReplEmpAdd rea = new ReplEmpAdd(GridReplEmp.CurrentRow.Cells["repl_emp_id"].Value.ToString(),main_transfer_id, true);
                if (rea.ShowDialog(this)== DialogResult.OK)
                {
                    FillGridReplEmp(staffs_id_edit);
                    ToolTip t = new ToolTip();
                    t.IsBalloon = true;
                    t.ToolTipIcon = ToolTipIcon.Info;
                    t.Show("Изменения сохранены", tpMainEmpShtat, 0, 0, 5000);
                    Result = true;
                }
            }
        }

        private void order_Validating(object sender, CancelEventArgs e)
        {
            if (order.Text.Length > 0)
            {
                OracleCommand cmd = new OracleCommand(string.Format("select * from {0}.orders where order_name={1}", DataSourceScheme.SchemeName, order.Text), Connect.CurConnect);
                OracleDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    order.Text = "";
                    MessageBox.Show("Не верный номер заказа");
                    order.Focus();
                }
            }
        }

        private void classific_SelectedValueChanged(object sender, EventArgs e)
        {
            OracleDataReader r = new OracleCommand(string.Format("select tar_sal from {0}.descr_tariff_grid dt join {0}.tariff_grid tg on (tg.tariff_grid_id=dt.tariff_grid_id) " +
                " where '('||code_tariff_grid||')'||tariff_grid_name = '{1}' and tar_classif='{2}' order by tar_date desc", DataSourceScheme.SchemeName, tariff_grid.Text, classific.Text), Connect.CurConnect).ExecuteReader();
            if (r.Read())
                tar_by_schema.Text = r[0].ToString(); 
        }

        private void btEditMainEmp_Click(object sender, EventArgs e)
        {
            FindEmpSingle rea = new FindEmpSingle();
            if (rea.ShowDialog(this)==DialogResult.OK && rea.transfer_id != main_transfer_id)
            {
                OracleCommand cmd = new OracleCommand(string.Format("update {0}.transfer set staffs_id=null where transfer_id='{1}'", DataSourceScheme.SchemeName, main_transfer_id), Connect.CurConnect);
                cmd.ExecuteNonQuery();
                main_transfer_id = rea.transfer_id.ToString();
                cmd.CommandText = string.Format("update {0}.transfer set staffs_id={1} where transfer_id='{2}'", DataSourceScheme.SchemeName, staffs_id_edit, main_transfer_id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                InitEmpMainPage(main_transfer_id);
                Result = true;
                ToolTip t = new ToolTip();
                t.IsBalloon = true;
                t.ToolTipIcon = ToolTipIcon.Info;
                t.Show("Назначен новый работник.\n Изменения сохранены", tpMainEmpShtat, 0, 0, 5000);
            }
        }

        private void btRemoveMainEmp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите убрать работника с данной штатной единицы?", "АРМ Штатное расписание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OracleCommand cmd = new OracleCommand(string.Format("update {0}.transfer set staffs_id=null where transfer_id={1}", DataSourceScheme.SchemeName, main_transfer_id), Connect.CurConnect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                InitEmpMainPage(main_transfer_id = "-1");
                ToolTip t = new ToolTip();
                t.IsBalloon = true;
                t.ToolTipIcon = ToolTipIcon.Info;
                t.Show("Изменения сохранены", tpMainEmpShtat, 0, 0, 5000);
                Result = true;
            }
        }

        /// <summary>
        /// устанавливает в  поля формы значения свойств данной штатной единицы 
        /// </summary>
        /// 
        public void BlockAllFields()
        {
            panel_data.Enabled = false;
        }
        public void InitField(string staffs_id, bool Locked)
        {
            //ЗАПОЛНЯЕМ ПОЛЯ ПО ШТАТНОЙ ЕДИНИЦЕ!
            string _staff_sign;
            OracleCommand cmd = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("new/select_about_staff_when_edit_load.sql"), DataSourceScheme.SchemeName, staffs_id), Connect.CurConnect);
            OracleDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return;
            Degree.Text = reader["degree_name"].ToString();
            code_pos.Text = reader["code_pos"].ToString();
            vacant_sign.Checked = (reader["vacant_sign"].ToString() == "1" ? true : false);
            Date_Begin.Text = reader["date_begin_staff"].ToString();
            Date_End.Text = reader["date_end_staff"].ToString();
            comments_pos.Text = reader["comment_to_pos"].ToString();
            harmful_add.Text = (reader["harmful_addition"].ToString() == "0" ? "" : reader["harmful_addition"].ToString());
            _staff_sign = reader["staff_sign"].ToString();
            DateEndVacant.Text = reader["date_end_vacant"].ToString();
            SS1.subdiv_id = reader["subdiv_id"].ToString();
            if (reader["t_g"].ToString() != "()") tariff_grid.Text = reader["t_g"].ToString();
            classific.Text = reader["classific"].ToString();
            tar_by_schema.Text = reader["tar_by_schema"].ToString();
            secret_add.Text = (reader["add_exp_area"].ToString() == "0" ? "" : reader["add_exp_area"].ToString());
            label_pos_name.Text = reader["pos_name"].ToString();
            order.Text = reader["order_name"].ToString();
            type_personal.Text = (reader["type_person"].ToString() == "0" ? "АУП" : (reader["type_person"].ToString() == "1" ? "МОП" : "ПТП"));
            if (_staff_sign == "1" && Locked)
            {
                bt_find_pos.Enabled = Degree.Enabled = SS1.Enabled = code_pos.Enabled = numberOfPackage.Enabled =
                   gbDateStaffs.Enabled = gbAccounData.Enabled = SetTemp.Enabled = false;
            }
            else if (_staff_sign == "2" && Locked)
            {
                bt_find_pos.Enabled = gbMainData.Enabled =
                    gbDateStaffs.Enabled = gbVacant_.Enabled = gbAccounData.Enabled = false;
            }
            //*****************     ищем работника по шт единице     *******************************************************************
            cmd.CommandText = string.Format("select tr.transfer_id from {0}.transfer tr where sign_cur_work=1 and staffs_id='{1}'", DataSourceScheme.SchemeName, staffs_id);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                InitEmpMainPage(main_transfer_id = (reader["transfer_id"].ToString() == "" ? "-1" : reader["transfer_id"].ToString()));
                //tpMainEmpShtat.EnableByRules(connect);
            }
            FillGridReplEmp(staffs_id);
            //tpReplEmpShtat.EnableByRules(connect);
        }
        private void FillGridReplEmp(string staffs_id)
        {
            //ищем замещающих. 
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery(@"new\FillCombineReplEmp.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
            cmd.Parameters.Add("repl_tr_id", main_transfer_id);
            DataTable t = new DataTable();
            new OracleDataAdapter(cmd).Fill(t);
            GridReplEmp.DataSource = t;
            GridReplEmp.Columns["repl_emp_id"].Visible = false;
            GridReplEmp.Columns["transfer_id"].Visible = false;
            Settings.SetDataGridCaption(ref GridReplEmp);
            ColumnWidthSaver.FillWidthOfColumn(GridReplEmp);
        }
        public void InitGroupField(List<string> _le)
        {
            if (_le.Count == 0) return;
            string in_staffs = "", allStaffsSign;
            for (int i = 0; i < _le.Count; i++)
                in_staffs += _le[i] + ",";
            in_staffs = in_staffs.Substring(0, in_staffs.Length - 1);
            OracleDataReader r = new OracleCommand(string.Format("select DISTINCT staff_sign from {0}.staffs where staffs_id in ({1})", DataSourceScheme.SchemeName, in_staffs), Connect.CurConnect).ExecuteReader();
            r.Read();
            allStaffsSign = r[0].ToString();
            if (r.Read() || allStaffsSign == "2"/*или 1 тоже думаю*/)
            {
                BlockAllFields();
                return;
            }

            OracleCommand cmd = new OracleCommand(string.Format(LibraryKadr.Queries.GetQuery("new/GetEqualsFieldsOfStaffs.sql"), DataSourceScheme.SchemeName, _le[0], in_staffs), Connect.CurConnect);
            r = cmd.ExecuteReader();
            if (!r.Read()) return;
            if (r["subdiv_id"].ToString() == "-1")
            { /*code_subdiv.Enabled=Subdiv.Enabled=false;*/}
            else
            {
                SS1.subdiv_id = r["subdiv_id"].ToString();
            }
            if (r["pos_id"].ToString() == "-1")
            {
                /* codePos.Enabled = false;
                 bt_find_pos.Enabled = false;*/
            }
            else
            {
                OracleDataReader r1 = new OracleCommand(string.Format("select code_pos,pos_name from {0}.position where pos_id={1}", DataSourceScheme.SchemeName, r["pos_id"].ToString()), Connect.CurConnect).ExecuteReader();
                r1.Read();
                code_pos.Text = r1[0].ToString();
                label_pos_name.Text = r1[1].ToString();
            }

            if (r["degree_id"].ToString() == "-1") /*BoxCategory.Enabled = false*/ ;
            else Degree.Text = r["degree_id"].ToString();
            if (r["VACANT_SIGN"].ToString() == "-1") /*vacant_sign.Enabled = false*/ ;
            else vacant_sign.Checked = (r["VACANT_SIGN"].ToString() == "1" ? true : false);
            if (r["DATE_END_VACANT"].ToString() == "01.01.1000") /*DateEndVacant.Enabled = false*/ ;
            else DateEndVacant.Text = r["DATE_END_VACANT"].ToString();
            if (r["DATE_BEGIN_STAFF"].ToString() == "01.01.1000") /*DateBegin.Enabled = false*/ ;
            else Date_Begin.Text = r["DATE_BEGIN_STAFF"].ToString();
            if (r["DATE_END_STAFF"].ToString() == "01.01.1000") /*DateEnd.Enabled = false*/ ;
            else Date_End.Text = r["DATE_END_STAFF"].ToString();
            if (r["HARMFUL_ADDITION"].ToString() == "-1") /*harmful_add.Enabled = false*/ ;
            else harmful_add.Text = r["HARMFUL_ADDITION"].ToString();
            if (r["TYPE_PERSON"].ToString() == "-1") /*type_personal.Enabled = false*/ ;
            else type_personal.Text = (r["type_person"].ToString() == "0" ? "АУП" : (r["type_person"].ToString() == "1" ? "МОП" : "ПТП"));
            if (r["COMMENT_TO_POS"].ToString() == "-1") /*comments_pos.Enabled = false*/ ;
            else comments_pos.Text = r["COMMENT_TO_POS"].ToString();
            if (r["ORDER_ID"].ToString() == "-1") /*order.Enabled = false*/ ;
            else order.Text = r["ORDER_ID"].ToString();

            /*if (r["COMB_ADDITION"].ToString() == "-1") /*CombAdd.Enabled = false;
            else CombAdd.Text = r["COMB_ADDITION"].ToString();*/
            if (r["TARIF_GRID_ID"].ToString() == "-1") /*tariff_grid.Enabled = false*/ ;
            else tariff_grid.Text = r["TARIF_GRID_ID"].ToString();
            if (r["TAR_BY_SCHEMA"].ToString() == "-1") /*TarSchema.Enabled = false*/ ;
            else tar_by_schema.Text = r["TAR_BY_SCHEMA"].ToString();
            if (r["CLASSIFIC"].ToString() == "-1") /*classific.Enabled = false*/ ;
            else classific.Text = r["CLASSIFIC"].ToString();
            /*if (r["PERSONAL_TAR"].ToString() == "-1") /*personal_tar.Enabled = false;
            else personal_tar.Text = r["PERSONAL_TAR"].ToString();*/
            if (r["ADD_EXP_AREA"].ToString() == "-1") /*add_exp_area.Enabled = false*/ ;
            else secret_add.Text = r["ADD_EXP_AREA"].ToString();
        }
    }
    public enum TypeAdditionShtat
    {
        NewEmpty=0,
        EditExists=1,
        NewByExists=2,
        EditGroup=3,
        TransferToNew=4
    }
}
