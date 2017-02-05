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

namespace Kadr
{
    public partial class HBPosition : Form
    {
        FormMain parentForm;
        POSITION_seq position;
        POSITION_seq fromPosition;
        bool flagAdd = false;
        public HBPosition(FormMain _parentForm)
        {
            InitializeComponent();
            parentForm = _parentForm;
            position = new POSITION_seq(Connect.CurConnect);
            position.Fill(string.Format("order by {0}", POSITION_seq.ColumnsName.CODE_POS));
            fromPosition = new POSITION_seq(Connect.CurConnect);
            fromPosition.Fill(string.Format("order by {0}", POSITION_seq.ColumnsName.POS_NAME));
            dgPosition.AddBindingSource(position);
            dgPosition.Columns["from_pos_id"].Visible = false;
            tbCode_Pos.AddBindingSource(position, POSITION_seq.ColumnsName.CODE_POS);
            tbName_Pos.AddBindingSource(position, POSITION_seq.ColumnsName.POS_NAME);
            //mbPos_Date_Start.AddBindingSource(position, POSITION_seq.ColumnsName.POS_DATE_START);
            //mbPos_Date_End.AddBindingSource(position, POSITION_seq.ColumnsName.POS_DATE_END);
            dePos_Date_Start.AddBindingSource(position, POSITION_seq.ColumnsName.POS_DATE_START);
            dePos_Date_End.AddBindingSource(position, POSITION_seq.ColumnsName.POS_DATE_END);
            chPos_Actual_Sign.AddBindingSource(position, POSITION_seq.ColumnsName.POS_ACTUAL_SIGN);
            cbFrom_Pos_ID.AddBindingSource(position, POSITION_seq.ColumnsName.FROM_POS_ID, new LinkArgument(fromPosition,
                POSITION_seq.ColumnsName.POS_NAME));
            chPos_Chief_Or_Deputy_Sign.AddBindingSource(position, POSITION_seq.ColumnsName.POS_CHIEF_OR_DEPUTY_SIGN);
            pnButton.EnableByRules();
            btSavePosition.Enabled = false;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            flagAdd = true;
            position.AddNew();
            ((CurrencyManager)BindingContext[position]).Position = ((CurrencyManager)BindingContext[position]).Count - 1; 
            dgPosition.Enabled = false;
            btAddPosition.Enabled = false;
            btEditPosition.Enabled = false;
            btDeletePosition.Enabled = false;
            btExit.Text = "Отмена";
            btSavePosition.Enabled = true;
            tbCode_Pos.Enabled = true; 
            tbName_Pos.Enabled = true;
            dePos_Date_Start.Enabled = true;
            dePos_Date_End.Enabled = true;
            chPos_Actual_Sign.Enabled = true;
            chPos_Chief_Or_Deputy_Sign.Enabled = true;
            cbFrom_Pos_ID.Enabled = true;
            this.Invalidate();
            tbCode_Pos.Focus();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            flagAdd = false;
            dgPosition.Enabled = false;
            btAddPosition.Enabled = false;
            btEditPosition.Enabled = false;
            btDeletePosition.Enabled = false;
            btExit.Text = "Отмена";
            btSavePosition.Enabled = true;
            tbCode_Pos.Enabled = true;
            tbName_Pos.Enabled = true;
            dePos_Date_Start.Enabled = true;
            dePos_Date_End.Enabled = true;
            chPos_Actual_Sign.Enabled = true;
            chPos_Chief_Or_Deputy_Sign.Enabled = true;
            cbFrom_Pos_ID.Enabled = true;
            tbCode_Pos.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string pos = ((POSITION_obj)((CurrencyManager)BindingContext[position]).Current).POS_ID.ToString();
                dgPosition.Rows.Remove(dgPosition.CurrentRow);                
                if (Authorization.positions.DeletePosition(pos))
                {
                    position.Save();
                    Connect.Commit();
                }
                if (dgPosition.Rows.Count == 0)
                {
                    btEditPosition.Enabled = false;
                    btDeletePosition.Enabled = false;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tbCode_Pos.Text != "")
            {
                if (tbName_Pos.Text != "")
                {
                    dgPosition.Enabled = true;
                    btAddPosition.Enabled = true;
                    btEditPosition.Enabled = true;
                    btDeletePosition.Enabled = true;
                    btExit.Text = "Выход";
                    btSavePosition.Enabled = false;
                    tbCode_Pos.Enabled = false;
                    tbName_Pos.Enabled = false;
                    dePos_Date_Start.Enabled = false;
                    dePos_Date_End.Enabled = false;
                    chPos_Actual_Sign.Enabled = false;
                    chPos_Chief_Or_Deputy_Sign.Enabled = false;
                    cbFrom_Pos_ID.Enabled = false;
                    if (flagAdd)
                    {
                        if (Authorization.positions.InsertPosition(new Position(
                            ((POSITION_obj)((CurrencyManager)BindingContext[position]).Current).POS_ID.ToString(),
                            ((POSITION_obj)((CurrencyManager)BindingContext[position]).Current).POS_NAME)))
                        {
                            position.Save();
                            Connect.Commit();
                        }
                    }
                    else
                    {
                        if (Authorization.positions.UpdatePosition(new Position(
                            ((POSITION_obj)((CurrencyManager)BindingContext[position]).Current).POS_ID.ToString(),
                            ((POSITION_obj)((CurrencyManager)BindingContext[position]).Current).POS_NAME)))
                        {
                            position.Save();
                            Connect.Commit();
                        }
                    }                    
                }
                else
                {
                    MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbName_Pos.Focus();
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbCode_Pos.Focus();
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                Close();
            }
            else
            {
                dgPosition.Enabled = true;
                btAddPosition.Enabled = true;
                btEditPosition.Enabled = true;
                btDeletePosition.Enabled = true;
                btExit.Text = "Выход";
                btSavePosition.Enabled = false;
                tbCode_Pos.Enabled = false;
                tbName_Pos.Enabled = false;
                dePos_Date_Start.Enabled = false;
                dePos_Date_End.Enabled = false;
                chPos_Actual_Sign.Enabled = false;
                chPos_Chief_Or_Deputy_Sign.Enabled = false;
                cbFrom_Pos_ID.Enabled = false;
                position.RollBack();
                Connect.Rollback();
                dgPosition.Invalidate();
                ((CurrencyManager)BindingContext[position]).Refresh();
                this.Invalidate();
            }
        }

        private void HBPosition_Activated(object sender, EventArgs e)
        {
            if (dgPosition.Rows.Count == 0)
            {
                this.btEditPosition.Enabled = false;
                this.btDeletePosition.Enabled = false;
            }
        }
    }
}
