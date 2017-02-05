using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;

namespace Kadr
{
    public partial class Per_Num_Book : Form
    {
        OracleConnection connection;
        PER_NUM_BOOK_seq per_num_book;
        EMP_seq emp;
        PersonalCard parentForm;
        public Per_Num_Book(OracleConnection _connection, EMP_seq _emp, PersonalCard _parentForm)
        {
            InitializeComponent();
            connection = _connection;
            emp = _emp;
            parentForm = _parentForm;
            per_num_book = new PER_NUM_BOOK_seq(connection);
            per_num_book.Fill(string.Format("where {0} = {1} order by {2}", PER_NUM_BOOK_seq.ColumnsName.FREE_SIGN, 1, 
                PER_NUM_BOOK_seq.ColumnsName.PER_NUM));
            cbPer_Num.DataSource = per_num_book;
            cbPer_Num.DisplayMember = PER_NUM_BOOK_seq.ColumnsName.PER_NUM.ToString();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            per_num_book.Clear();
            Close();
        }

        private void btPer_Num_Click(object sender, EventArgs e)
        {
            //if (cbPer_Num.Text != "")
            //{
            //    parentForm.tbPer_Num.Text = cbPer_Num.Text;
            //}
            //emp.Save();
            Close();
        }


    }
}
