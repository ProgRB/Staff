using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;

namespace Kadr
{
    public partial class spr_tariff : Form
    {
        //BindingSource bs = new BindingSource();
        public spr_tariff()
        {
            InitializeComponent();

            OracleDataTable tr = new OracleDataTable("", Connect.CurConnect);
            tr.SelectCommand.CommandText = string.Format(Queries.GetQuery("ras/tariff_grid.txt"), 
                DataSourceScheme.SchemeName);
            tr.Fill();
            //bs.DataSource = tr;
            dataGridView1.DataSource = tr;
            dataGridView1.Columns["code_tariff_grid"].HeaderText = "Шифр тарифной сетки";
            dataGridView1.Columns["code_tariff_grid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["code_tariff_grid"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns["tariff_grid_name"].Visible = false;
            dataGridView1.Columns["TARIFF_GRID_id"].Visible = false;
            dataGridView1.Columns["TYPE_TARIFF_GRID_NAME"].Visible = false;
            dataGridView1.ReadOnly = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string stroka = dataGridView1.CurrentRow.Cells["tariff_grid_name"].Value.ToString();
            if (stroka == "")
            {
                label24.Text = "Нет описания";
            }
            else
            {
                label24.Text = dataGridView1.CurrentRow.Cells["tariff_grid_name"].Value.ToString();
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells["TYPE_TARIFF_GRID_NAME"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
