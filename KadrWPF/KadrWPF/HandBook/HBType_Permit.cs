using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using PercoXML;
using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Kadr
{
    public partial class HBType_Permit : System.Windows.Forms.UserControl
    {
        TYPE_PERMIT_seq type_permit;        
        public HBType_Permit()
        {
            InitializeComponent();
            pnButton.EnableByRules();
            btSaveType_Permit.Enabled = false;
            type_permit = new TYPE_PERMIT_seq(Connect.CurConnect);
            type_permit.Fill(string.Format("order by {0}", TYPE_PERMIT_seq.ColumnsName.PERMIT_NAME));
            dgType_Permit.AddBindingSource(type_permit);
            dgType_Permit.Columns["DISPLACE_GRAPH"].DisplayIndex = 2;
            //dgType_Permit.Columns["from_pos_id"].Visible = false;            
            tbPermit_Name.AddBindingSource(type_permit, TYPE_PERMIT_seq.ColumnsName.PERMIT_NAME);
            tbDisplace_Graph.AddBindingSource(type_permit, TYPE_PERMIT_seq.ColumnsName.DISPLACE_GRAPH);
            chFree_Exit.AddBindingSource(type_permit, TYPE_PERMIT_seq.ColumnsName.FREE_EXIT);
            chNot_Registr_Pass.AddBindingSource(type_permit, TYPE_PERMIT_seq.ColumnsName.NOT_REGISTR_PASS);
            chRound_Time.AddBindingSource(type_permit, TYPE_PERMIT_seq.ColumnsName.ROUND_TIME);            
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            type_permit.AddNew();
            ((CurrencyManager)BindingContext[type_permit]).Position = 
                ((CurrencyManager)BindingContext[type_permit]).Count - 1; 
            dgType_Permit.Enabled = false;
            btAddType_Permit.Enabled = false;
            btEditType_Permit.Enabled = false;
            btDeleteType_Permit.Enabled = false;
            btExit.Text = "Отмена";
            btSaveType_Permit.Enabled = true;
            tbDisplace_Graph.Enabled = true; 
            tbPermit_Name.Enabled = true;
            chFree_Exit.Enabled = true;
            chNot_Registr_Pass.Enabled = true;
            chRound_Time.Enabled = true;
            this.Invalidate();
            tbPermit_Name.Focus();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            dgType_Permit.Enabled = false;
            btAddType_Permit.Enabled = false;
            btEditType_Permit.Enabled = false;
            btDeleteType_Permit.Enabled = false;
            btExit.Text = "Отмена";
            btSaveType_Permit.Enabled = true;
            tbDisplace_Graph.Enabled = true;
            tbPermit_Name.Enabled = true;
            chFree_Exit.Enabled = true;
            chNot_Registr_Pass.Enabled = true;
            chRound_Time.Enabled = true;
            tbPermit_Name.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int pos =
                    Convert.ToInt32(((TYPE_PERMIT_obj)((CurrencyManager)BindingContext[type_permit]).Current).TYPE_PERMIT_ID);
                OracleCommand com = new OracleCommand("", Connect.CurConnect);
                com.BindByName = true;
                com.CommandText = string.Format("select count(*) from {0}.PERMIT where TYPE_PERMIT_ID = {1}",
                    DataSourceScheme.SchemeName, pos);
                int count = Convert.ToInt32(com.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Невозможно удалить данную запись!\nСуществуют сотрудники с данной привилегией.",
                        "АСУ \"Кадры\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    return;
                }
                type_permit.Remove(((TYPE_PERMIT_obj)((CurrencyManager)BindingContext[type_permit]).Current));
                type_permit.Save();
                Connect.Commit();                
                if (dgType_Permit.Rows.Count == 0)
                {
                    btEditType_Permit.Enabled = false;
                    btDeleteType_Permit.Enabled = false;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tbPermit_Name.Text != "")
            {
                if (tbDisplace_Graph.Text != "")
                {
                    dgType_Permit.Enabled = true;
                    btAddType_Permit.Enabled = true;
                    btEditType_Permit.Enabled = true;
                    btDeleteType_Permit.Enabled = true;
                    btExit.Text = "Выход";
                    btSaveType_Permit.Enabled = false;
                    tbDisplace_Graph.Enabled = false;
                    tbPermit_Name.Enabled = false;
                    chFree_Exit.Enabled = false;
                    chNot_Registr_Pass.Enabled = false;
                    chRound_Time.Enabled = false;
                    type_permit.Save();
                    Connect.Commit();                
                }
                else
                {
                    MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbDisplace_Graph.Focus();
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbPermit_Name.Focus();
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                //Close();
            }
            else
            {
                dgType_Permit.Enabled = true;
                btAddType_Permit.Enabled = true;
                btEditType_Permit.Enabled = true;
                btDeleteType_Permit.Enabled = true;
                btExit.Text = "Выход";
                btSaveType_Permit.Enabled = false;
                tbDisplace_Graph.Enabled = false;
                tbPermit_Name.Enabled = false;
                chFree_Exit.Enabled = false;
                chNot_Registr_Pass.Enabled = false;
                chRound_Time.Enabled = false;
                type_permit.RollBack();
                Connect.Rollback();
                dgType_Permit.Invalidate();
                ((CurrencyManager)BindingContext[type_permit]).Refresh();
                this.Invalidate();
            }
        }
    }
}
