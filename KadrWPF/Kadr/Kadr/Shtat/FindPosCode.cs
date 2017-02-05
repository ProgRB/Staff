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
    public partial class FindPosCode : Form
    {
        public OracleConnection connect;
        public string pos_code="-1",pos_name;
        public FindPosCode(OracleConnection cnt)
        {
            connect = cnt;
            pos_code = "-1";
            pos_name = "Профессия не выбрана";
            InitializeComponent();
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.btOk_Click);
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            if (Posname.Text == "")
            {
                MessageBox.Show("Вы не ввели критерия поиска!");
                return;
            }
            string filter = string.Format("select code_pos, pos_name " +
                " from {0}.position where upper(pos_name) like upper('%{1}%') and pos_actual_sign=1 order by pos_name", DataSourceScheme.SchemeName, Posname.Text.Trim()); 
            OracleDataAdapter adapter = new OracleDataAdapter(filter , connect);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            Settings.SetDataGridCoumnWidth(ref dataGridView1);
            Settings.SetDataGridCaption(ref dataGridView1);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                pos_code = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                pos_name = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                this.Close();
            }
            else pos_code = "-1";            
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
   }
}
