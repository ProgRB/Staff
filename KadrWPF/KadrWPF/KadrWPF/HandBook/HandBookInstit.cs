using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;

namespace Kadr
{
    public partial class HandBookInstit : System.Windows.Forms.UserControl
    {
        INSTIT_seq instit;
        STUDY_CITY_seq study_city;
        public HandBookInstit()
        {
            InitializeComponent();
            instit = new INSTIT_seq(Connect.CurConnect);
            instit.Fill(string.Format("order by {0}", INSTIT_seq.ColumnsName.INSTIT_NAME));
            study_city = new STUDY_CITY_seq(Connect.CurConnect);
            study_city.Fill(string.Format("order by {0}", STUDY_CITY_seq.ColumnsName.CITY_NAME));
            dgView.AddBindingSource(instit, new LinkArgument(study_city, STUDY_CITY_seq.ColumnsName.CITY_NAME));
            tbName.AddBindingSource(instit, INSTIT_seq.ColumnsName.INSTIT_NAME);
            cbCity_Name.AddBindingSource(instit, STUDY_CITY_seq.ColumnsName.CITY_ID,
                new LinkArgument(study_city, STUDY_CITY_seq.ColumnsName.CITY_NAME));
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            instit.AddNew();
            ((CurrencyManager)BindingContext[instit]).Position = ((CurrencyManager)BindingContext[instit]).Count;
            cbCity_Name.Enabled = true;
            dgView.Enabled = false;
            btAdd.Enabled = false;
            btEdit.Enabled = false;
            btDelete.Enabled = false;
            btExit.Text = "Отмена";
            btSave.Enabled = true;
            tbName.Enabled = true;
            this.Invalidate();                
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            cbCity_Name.Enabled = true;
            dgView.Enabled = false;
            btAdd.Enabled = false;
            btEdit.Enabled = false;
            btDelete.Enabled = false;
            btExit.Text = "Отмена";
            btSave.Enabled = true;
            tbName.Enabled = true;
            tbName.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgView.Rows.Remove(dgView.CurrentRow);
                instit.Save();
                Connect.Commit();
                if (dgView.Rows.Count == 0)
                {
                    btEdit.Enabled = false;
                    btDelete.Enabled = false;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            
            if (tbName.Text != "")
            {
                dgView.Enabled = true;
                btAdd.Enabled = true;
                btEdit.Enabled = true;
                btDelete.Enabled = true;
                btExit.Text = "Выход";
                btSave.Enabled = false;
                tbName.Enabled = false;
                cbCity_Name.Enabled = false;
                instit.Save();
                Connect.Commit();
            }
            else
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbName.Focus();
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {

            }
            else
            {
                dgView.Enabled = true;
                btAdd.Enabled = true;
                btEdit.Enabled = true;
                btDelete.Enabled = true;
                btExit.Text = "Выход";
                btSave.Enabled = false;
                tbName.Enabled = false;
                cbCity_Name.Enabled = false;
                instit.RollBack();
                Connect.Rollback();
                dgView.Invalidate();
                ((CurrencyManager)BindingContext[instit]).Refresh();
                this.Invalidate();
            }
        }

        //private void HandBookInstit_Activated(object sender, EventArgs e)
        //{
        //    if (dgView.Rows.Count == 0)
        //    {
        //        this.btEdit.Enabled = false;
        //        this.btDelete.Enabled = false;
        //    }
        //}

        //private void HandBookInstit_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (btSave.Enabled == true)
        //    {
        //        DialogResult dialog = MessageBox.Show("Вы не сохранили изменения данных. Сохранить изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            if (tbName.Text != "")
        //            {
        //                instit.Save();
        //                Connect.Commit();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                return;
        //            }
        //        }
        //        else if (dialog == DialogResult.Cancel)
        //        {
        //            e.Cancel = true;
        //            tbName.Focus();
        //        }
        //    }
        //}
    }
}
