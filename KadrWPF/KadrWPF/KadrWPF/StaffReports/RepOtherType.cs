using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oracle.DataAccess.Client;
using LibraryKadr;
using Staff;
using WExcel = Microsoft.Office.Interop.Excel;

namespace StaffReports
{
    public partial class RepOtherType : Form
    {
        //string per_nums;
        //string order;
        string filterOnSubdiv;
        bool _flagArchive;
        int ord = 1;
        public RepOtherType(/*string str, string strOrder, */string _filterOnSubdiv, bool flagArchive)
        {
            InitializeComponent();
            //per_nums = str;
            //order = strOrder;
            filterOnSubdiv = _filterOnSubdiv;
            _flagArchive = flagArchive;
           
        }
        //string[] masAtrib = { "per_num", "FIO", "fullFIO", "code_subdiv", "subdiv_name", "code_pos", "pos_name", "date_hire", "tr_num_order", "tr_date_order", "workTime", "date_end_contr", "code_degree", "classific", "type_tariff_grid_name", "salary", "percent13", "emp_birth_date", "year_birth", "emp_sex", "name_state", "INN", "ser_med_polus", "num_med_polus", "insurance_num", "name_doc", "seria_passport", "num_passport", "when_given", "who_given", "te_name", "instit_name", "name_spec", "qual_name", "year_graduating", "seria_diploma", "num_diploma", "comm_name", "country_birth", "region_birth", "city_birth", "distr_birth", "locality_birth", "name_city", "name_district", "locality_name", "name_street", "reg_house", "reg_bulk", "reg_flat", "reg_code_street", "hab_city", "hab_district", "hab_locality", "hab_street", "hab_house", "hab_bulk", "hab_flat", "hab_code_street", "reg_phone", "date_trnsfr", "reason_dis", "date_transfer", "allStag" };
        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            List<FieldRepOther> list = new List<FieldRepOther>();
            for (int i = 0; i < listViewAtrib.Items.Count; i++)
            {
                if (listViewAtrib.Items[i].SubItems.Count==6)
                {
                    if (listViewAtrib.Items[i].Checked)                
                    {
                        list.Add(new FieldRepOther(listViewAtrib.Items[i].SubItems[0].Text,
                        listViewAtrib.Items[i].SubItems[1].Text ,
                        listViewAtrib.Items[i].SubItems[2].Text,
                        listViewAtrib.Items[i].SubItems[3].Text,
                        listViewAtrib.Items[i].SubItems[4].Text,
                        listViewAtrib.Items[i].SubItems[5].Text));
                    }
                }
                else 
                {
                    if (listViewAtrib.Items[i].Checked)                
                    {
                        list.Add(new FieldRepOther(listViewAtrib.Items[i].SubItems[0].Text,
                        listViewAtrib.Items[i].SubItems[1].Text ,
                        listViewAtrib.Items[i].SubItems[2].Text,
                        listViewAtrib.Items[i].SubItems[3].Text,
                        listViewAtrib.Items[i].SubItems[4].Text,""));
                    }
                }

            }
            if (list.Count > 0)
            {
                string sel_fields = "";
                int i = 1; 
                list.Sort((r1, r2) => Convert.ToInt32(r1.Order).CompareTo(Convert.ToInt32(r2.Order)));
                DataTable table1 = new DataTable();
                for (int j = 0; j <= list.Count; j++ )
                {
                    DataColumn col = new DataColumn();
                    col.DataType = typeof(string);
                    table1.Columns.Add(col);
                }
                DataRow row = table1.NewRow();
                row[0] = "№ п/п";
                List<int> columnWidth = new List<int>();
                List<string> ListFormat = new List<string>();
                columnWidth.Add(5);
                foreach (FieldRepOther item in list)
                {
                    if (item.Field_name.ToString() == "fullFIO")
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            DataColumn col = new DataColumn();
                            col.DataType = typeof(string);
                            table1.Columns.Add(col);
                        }
                        sel_fields += "emp_last_name, emp_first_name, emp_middle_name, ";
                        row[i++] = "Фамилия";
                        row[i++] = "Имя";
                        row[i++] = "Отчество";
                        columnWidth.Add(19);
                        columnWidth.Add(14);
                        columnWidth.Add(19);
                        ListFormat.Add("");
                        ListFormat.Add("");
                        ListFormat.Add("");
                    }
                    else
                    {
                        sel_fields += item.Field_name.ToString() + ", ";
                        row[i++] = item.Name_exc_field.ToString();
                        columnWidth.Add(Convert.ToInt32(item.Width));
                        ListFormat.Add(item.Format);
                    }                    
                }

                table1.Rows.Add(row);
                sel_fields = sel_fields.Remove(sel_fields.Length - 2, 2);
                string sql = string.Format(Queries.GetQuery("SelectRepOtherType.sql"),
                    Connect.Schema, sel_fields, filterOnSubdiv.Trim().Length != 0 ? 
                        string.Format(" tr.subdiv_id = {0} and ", filterOnSubdiv) : "");
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Connect.CurConnect);
                adapter.SelectCommand.BindByName = true;
                adapter.SelectCommand.Parameters.Add("p_user_name", OracleDbType.Varchar2).Value =
                    Connect.UserId.ToUpper();
                DataTable table = new DataTable();
                adapter.Fill(table);
                //CellParameter[] cellParameters = new CellParameter[] { new CellParameter(10, 1, "sdfsdaf", null) };
                Excel.PrintRepOtherType("RepOtherType.xlt", "A1", new DataTable[] { table1, table }, columnWidth.ToArray(), ListFormat.ToArray());
                //Excel.PrintR1C1("RepOtherType.xlt", cellParameters);
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни одного реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Cursor = Cursors.Default;
                return;
            }     
            this.Cursor = Cursors.Default;        }

        void checkedListBoxAtrib_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //MessageBox.Show(checkedListBoxAtrib.SelectedItem.ToString());
            if (checkedListBoxAtrib.GetItemChecked(checkedListBoxAtrib.SelectedIndex) == true)

                checkedListBoxAtrib.SelectedItem = "(" + mass.Count.ToString() + ") " + checkedListBoxAtrib.SelectedItem.ToString();

            else

                checkedListBoxAtrib.SelectedItem = checkedListBoxAtrib.SelectedItem.ToString();
                //checkedListBoxAtrib.SelectedItem = "(" + mass.Count.ToString()+ ") " + checkedListBoxAtrib.SelectedItem.ToString();            
        }
        List<int> mass = new List<int>();

        private void checkedListBoxAtrib_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBoxAtrib.GetItemChecked(checkedListBoxAtrib.SelectedIndex) == true)

                mass.Add(checkedListBoxAtrib.SelectedIndex);

            else

                mass.Remove(checkedListBoxAtrib.SelectedIndex);
 
        }

        private void listViewAtrib_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAtrib.SelectedItems.Count != 0 && listViewAtrib.SelectedItems[0].Checked)
            {
                upOrder.Enabled = true;
                downOrder.Enabled = true;
            }
            else
            {
                upOrder.Enabled = false;
                downOrder.Enabled = false;
            }
            
            //if (listViewAtrib.CheckedItems == true) ;
            //MessageBox.Show(listViewAtrib.CheckedItems[0].ToString());

                //if (checkedListBoxAtrib.GetItemChecked(checkedListBoxAtrib.SelectedIndex) == true)

                //    mass.Add(checkedListBoxAtrib.SelectedIndex);

                //else

                //    mass.Remove(checkedListBoxAtrib.SelectedIndex);
        }

        private void listViewAtrib_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int p;            
                if (e.Item.Checked)
                {
                    e.Item.SubItems[0].Text = ord++.ToString();

                }
                else
                {                    
                    --ord;
                    
                    p = Convert.ToInt32(e.Item.SubItems[0].Text);
                    for (int i = 0; i < listViewAtrib.Items.Count; i++)
                    {
                        if (listViewAtrib.Items[i].Checked/*(e.Item.Checked*/ && (/*Convert.ToInt32(e.Item.SubItems[i].Text)*/Convert.ToInt32(listViewAtrib.Items[i].SubItems[0].Text) > p/*Convert.ToInt32(e.Item.SubItems[0].Text)*/))
                        {
                            listViewAtrib.Items[i].SubItems[0].Text = (Convert.ToInt32(listViewAtrib.Items[i].SubItems[0].Text) - 1).ToString();
                            
                        }
                    }
                    e.Item.SubItems[0].Text = "";
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {   
            int i = -1;
            string p;
            if (listViewAtrib.SelectedItems.Count > 0 && listViewAtrib.CheckedItems.Count > 0)
            if (listViewAtrib.SelectedItems[0].Checked && (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) < listViewAtrib.CheckedItems.Count))
            {
                p = (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) + 1).ToString();
                do
                {
                    i++;
                    if (listViewAtrib.Items[i].SubItems[0].Text == p)
                    {
                        listViewAtrib.Items[i].SubItems[0].Text = (Convert.ToInt32(/*listViewAtrib.SelectedItems[0].SubItems[0].Text)*/p) - 1).ToString();
                        break;
                    }
                }
                while (listViewAtrib.Items[i].SubItems[0].Text != p);
                listViewAtrib.SelectedItems[0].SubItems[0].Text = (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) + 1).ToString();
                //do
                //    if (listViewAtrib.SelectedItems[i].SubItems[0].Text == p)
                //        listViewAtrib.SelectedItems[0].SubItems[0].Text = (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) - 1).ToString();
                //while (listViewAtrib.SelectedItems[i].SubItems[0].Text != p);
            }
            listViewAtrib.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i = -1;
            string p;
            if (listViewAtrib.SelectedItems.Count > 0 && listViewAtrib.CheckedItems.Count > 0)
            if (listViewAtrib.SelectedItems[0].Checked && ((Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text)-1) != 0))
            {
                p = (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) - 1).ToString();
                do
                {
                    i++;
                    if (listViewAtrib.Items[i].SubItems[0].Text == p)
                    {
                        listViewAtrib.Items[i].SubItems[0].Text = (Convert.ToInt32(/*listViewAtrib.SelectedItems[0].SubItems[0].Text*/p) + 1).ToString();
                        break;
                    }
                }
                while (listViewAtrib.Items[i].SubItems[0].Text != p);
                listViewAtrib.SelectedItems[0].SubItems[0].Text = (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) - 1).ToString();
                //do
                //    if (listViewAtrib.SelectedItems[i].SubItems[0].Text == p)
                //        listViewAtrib.SelectedItems[0].SubItems[0].Text = (Convert.ToInt32(listViewAtrib.SelectedItems[0].SubItems[0].Text) - 1).ToString();
                //while (listViewAtrib.SelectedItems[i].SubItems[0].Text != p);
            }
            listViewAtrib.Focus();
        }

        private void listViewAtrib_Click(object sender, EventArgs e)
        {
            //listViewAtrib.SelectedItems[0].Checked = listViewAtrib.SelectedItems[0].Checked
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewAtrib.Items.Count; i++)            
                listViewAtrib.Items[i].Checked = false;        
        }

        private void RepOtherType_Load(object sender, EventArgs e)
        {

        }
       
    }

    public class FieldRepOther
    {
        private string _order;
        private string _field;
        private string _fieldname;
        private string _width;
        private string _name_exc_field;
        private string _format;


        public FieldRepOther(string order, string field, string field_name, string width, string name_exc_field, 
            string format)
        {
            _order = order;
            _field = field;
            _fieldname = field_name;
            _width = width;
            _name_exc_field = name_exc_field;
            _format = format;

        }
        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }
        public string Field
        {
            get { return _field; }
            set { _field = value; }
        }
        public string Field_name
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public string Name_exc_field
        {
            get { return _name_exc_field; }
            set { _name_exc_field = value; }
        }
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}
