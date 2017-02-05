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
using LibraryKadr;

namespace Kadr
{
    public partial class Rewards : Form
    {
        DataGridView dgViewRewards;
        VisiblePanel pnVisible;

        // Объявление таблиц
        REWARD_seq reward;
        REWARD_NAME_seq reward_name;
        TYPE_REWARD_seq type_reward;
        // Объявление строки образование, табельного номера и флага добавления данных
        REWARD_obj r_reward;
        string per_num;
        bool f_addreward;

        // Создание формы
        public Rewards(string _per_num, bool _f_addreward, int pos, REWARD_seq _reward,
             REWARD_NAME_seq _reward_name, BASE_DOC_seq _base_doc, DataGridView _dgView, VisiblePanel _pnVisible)
        {
            InitializeComponent();
            dgViewRewards = _dgView;
            pnVisible = _pnVisible;
            f_addreward = _f_addreward;
            reward = _reward;
            reward_name = _reward_name;
            per_num = _per_num;
            type_reward = new TYPE_REWARD_seq(Connect.CurConnect);

            // Проверка нужно ли добавлять новую запись
            if (f_addreward)
            {
                r_reward = reward.AddNew();
                r_reward.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[reward]).Position = ((CurrencyManager)BindingContext[reward]).Count - 1;
            }
            else
            {
                BindingContext[reward].Position = pos;
                r_reward = (REWARD_obj)((CurrencyManager)BindingContext[reward]).Current;
            }            
            cbReward_Name.SelectedIndexChanged += new System.EventHandler(cbReward_Name_SelectedIndexChanged);
            // Привязка компонентов 
            cbReward_Name.AddBindingSource(reward, REWARD_NAME_seq.ColumnsName.REWARD_NAME_ID, 
                new LinkArgument(reward_name, REWARD_NAME_seq.ColumnsName.REWARD_NAME));
            tbRew_Doc_Name.AddBindingSource(reward, REWARD_seq.ColumnsName.REW_DOC_NAME);
            tbNum_Reward.AddBindingSource(reward, REWARD_seq.ColumnsName.NUM_REWARD);
            chGov_Sign.AddBindingSource(reward, REWARD_seq.ColumnsName.GOV_SIGN);
            deDate_Reward.AddBindingSource(reward, REWARD_seq.ColumnsName.DATE_REWARD);
            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }  
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            reward.Save();
            Connect.Commit();
            f_addreward = false;
            Library.VisiblePanel(dgViewRewards, pnVisible);
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Rewards_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addreward)
            {
                reward.CancelNew(((CurrencyManager)BindingContext[reward]).Position);
            }
            else if (reward.IsDataChanged())
            {
                reward.RollBack();
            }
            dgViewRewards.Invalidate();
        }

        private void tbRew_Doc_Name_Validating(object sender, CancelEventArgs e)
        {
            tbRew_Doc_Name.Text = tbRew_Doc_Name.Text.ToUpper();
        }

        private void cbReward_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(cbReward_Name.SelectedValue);
                if (((REWARD_NAME_obj)((CurrencyManager)BindingContext[reward_name]).Current).REWARD_NAME_ID != null
                    && cbReward_Name.SelectedValue != null)
                {
                    REWARD_NAME_seq rew = new REWARD_NAME_seq(Connect.CurConnect);
                    rew.Fill(string.Format("where {0} = {1}", REWARD_NAME_seq.ColumnsName.REWARD_NAME_ID,
                        cbReward_Name.SelectedValue));
                    type_reward.Clear();
                    type_reward.Fill(string.Format("where {0} = {1}", TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_ID,
                        ((REWARD_NAME_obj)((CurrencyManager)BindingContext[rew]).Current).TYPE_REWARD_ID));
                    if (type_reward.Count != 0)
                    {
                        tbType_Reward.Text = ((TYPE_REWARD_obj)((CurrencyManager)BindingContext[type_reward]).Current).TYPE_REWARD_NAME;
                    }
                }
            }
            catch { }
        }

    }
}
