using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;


namespace Kadr
{
    public partial class spis_nolog : Form
    {
        public spis_nolog(OracleConnection _conn2,OracleDataTable _okl2)
        {
            InitializeComponent();
            dataGridView1.DataSource = _okl2;
            dataGridView1.Columns["change_date"].HeaderText = "Дата изменения";
            dataGridView1.Columns["change_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["change_date"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns["tax_code"].HeaderText = "Шифр налога";
            dataGridView1.Columns["tax_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["tax_code"].SortMode = DataGridViewColumnSortMode.NotSortable;
            // лишние поля скрываю
            dataGridView1.Columns["salary"].Visible = false;
            dataGridView1.Columns["classific"].Visible = false;
            dataGridView1.Columns["harmful_addition"].Visible = false;
            dataGridView1.Columns["comb_addition"].Visible = false;
            dataGridView1.Columns["code_tariff_grid"].Visible = false;
            dataGridView1.Columns["account_data_id"].Visible = false;  
       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
