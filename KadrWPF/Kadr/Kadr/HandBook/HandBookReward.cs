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
    public partial class HandBookReward : Form
    {
        REWARD_NAME_seq reward_name;
        TYPE_REWARD_seq type_reward;
        public HandBookReward(FormMain parentForm)
        {
            InitializeComponent();
            reward_name = new REWARD_NAME_seq(Connect.CurConnect);
            reward_name.Fill(string.Format("order by {0}", REWARD_NAME_seq.ColumnsName.REWARD_NAME));
            type_reward = new TYPE_REWARD_seq(Connect.CurConnect);
            type_reward.Fill(string.Format("order by {0}", TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_NAME));
            dgView.AddBindingSource(reward_name, new LinkArgument(type_reward, TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_NAME));
            tbName.AddBindingSource(reward_name, REWARD_NAME_seq.ColumnsName.REWARD_NAME);
            cbType_Reward.AddBindingSource(reward_name, REWARD_NAME_seq.ColumnsName.TYPE_REWARD_ID,
                new LinkArgument(type_reward, TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_NAME));
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            reward_name.AddNew();
            ((CurrencyManager)BindingContext[reward_name]).Position = ((CurrencyManager)BindingContext[reward_name]).Count;
            cbType_Reward.Enabled = true;
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
            cbType_Reward.Enabled = true;
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
                reward_name.Save();
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
            if (tbName.Text == "")
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbName.Focus();
                return;
            }
            if (cbType_Reward.SelectedValue == null)
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbType_Reward.Focus();
                return;
            }
            dgView.Enabled = true;
            btAdd.Enabled = true;
            btEdit.Enabled = true;
            btDelete.Enabled = true;
            btExit.Text = "Выход";
            btSave.Enabled = false;
            tbName.Enabled = false;
            cbType_Reward.Enabled = false;
            reward_name.Save();
            Connect.Commit();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                Close();
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
                cbType_Reward.Enabled = false;
                reward_name.RollBack();
                Connect.Rollback();
                dgView.Invalidate();
                ((CurrencyManager)BindingContext[reward_name]).Refresh();
                this.Invalidate();
            }
        }

        private void HandBookInstit_Activated(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
            {
                this.btEdit.Enabled = false;
                this.btDelete.Enabled = false;
            }
        }

        private void HandBookInstit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btSave.Enabled == true)
            {
                DialogResult dialog = MessageBox.Show("Вы не сохранили изменения данных. Сохранить изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dialog == DialogResult.Yes)
                {
                    if (tbName.Text != "")
                    {
                        reward_name.Save();
                        Connect.Commit();
                    }
                    else
                    {
                        MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else if (dialog == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    tbName.Focus();
                }
            }
        }
    }
}
