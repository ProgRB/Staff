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

namespace Tabel
{
    public partial class Edit_Group_Master : Form
    {
        EMP_GROUP_MASTER_seq emp_GM;
        OracleDataTable dtHistoryGM;
        decimal _transfer_id;
        string _per_num;
        bool flagAdd = false;
        /// <summary>
        /// Редактирование группы мастера
        /// </summary>
        /// <param name="_transfer_id">Идентификатор перевода работника</param>
        public Edit_Group_Master(decimal transfer_id, string per_num)
        {
            InitializeComponent();
            _transfer_id = transfer_id;
            _per_num = per_num;
            dtHistoryGM = new OracleDataTable("", Connect.CurConnect);
            dtHistoryGM.SelectCommand.CommandText = string.Format(Queries.GetQuery("Table/SelectHistoryGM.sql"),
                Connect.Schema);
            dtHistoryGM.SelectCommand.Parameters.Add("p_transfer_id", _transfer_id);
            dtHistoryGM.Fill();
            dgHistoryGM.DataSource = dtHistoryGM;
            dgHistoryGM.Columns["EMP_GROUP_MASTER_ID"].Visible = false;
            foreach (DataGridViewColumn column in dgHistoryGM.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            emp_GM = new EMP_GROUP_MASTER_seq(Connect.CurConnect);
            mbName_GM.AddBindingSource(emp_GM, EMP_GROUP_MASTER_seq.ColumnsName.NAME_GROUP_MASTER);
            deBegin_Group.AddBindingSource(emp_GM, EMP_GROUP_MASTER_seq.ColumnsName.BEGIN_GROUP);
            deEnd_Group.AddBindingSource(emp_GM, EMP_GROUP_MASTER_seq.ColumnsName.END_GROUP);
            EnabledControl(false);
        }

        /// <summary>
        /// Активация контролов
        /// </summary>
        /// <param name="p"></param>
        private void EnabledControl(bool p)
        {
            mbName_GM.Enabled = p;
            deBegin_Group.Enabled = p;
            deEnd_Group.Enabled = p;            
            if (p)
            {
                btAddGM.Enabled = !p;
                btEditGM.Enabled = !p;
                btDeleteGM.Enabled = !p;
                btSave.Enabled = p;
                mbName_GM.BackColor = Color.White;
                deBegin_Group.BackColor = Color.White;
                deEnd_Group.BackColor = Color.White;
            }
            else
            {
                btAddGM.Enabled = !p;
                btEditGM.Enabled = !p;
                btDeleteGM.Enabled = !p;
                btSave.Enabled = p;
                mbName_GM.BackColor = Color.Gainsboro;
                deBegin_Group.BackColor = Color.Gainsboro;
                deEnd_Group.BackColor = Color.Gainsboro;
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Добавление группы мастера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddGM_Click(object sender, EventArgs e)
        {
            emp_GM.Clear();
            emp_GM.AddNew();
            ((EMP_GROUP_MASTER_obj)((CurrencyManager)BindingContext[emp_GM]).Current).TRANSFER_ID = 
                _transfer_id;
            EnabledControl(true);
            mbName_GM.Focus();
            flagAdd = true;
        }

        /// <summary>
        /// Редактирование группы мастера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEditGM_Click(object sender, EventArgs e)
        {
            if (dgHistoryGM.CurrentRow != null)
            {
                emp_GM.Clear();
                emp_GM.Fill("where EMP_GROUP_MASTER_ID = " +
                    dgHistoryGM.CurrentRow.Cells["EMP_GROUP_MASTER_ID"].Value.ToString());
                EnabledControl(true);
                mbName_GM.Focus();
                flagAdd = false;
            }
        }

        /// <summary>
        /// Удаление группы мастера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDeleteGM_Click(object sender, EventArgs e)
        {
            if (dgHistoryGM.CurrentRow != null)
            {
                if (Convert.ToDateTime(dgHistoryGM.CurrentRow.Cells["Дата начала работы"].Value) <
                    Table.dCloseTable && Connect.UserId.ToUpper() != "BMW12714")
                {
                    MessageBox.Show("Запись нельзя удалить!\n" +
                        "Дата начала работы на группе меньше даты закрытия табеля!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Вы действительно хотите удалить запись?",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        OracleCommand ocDeleteGM = new OracleCommand("", Connect.CurConnect);
                        ocDeleteGM.BindByName = true;
                        ocDeleteGM.CommandText = string.Format(
                            "delete from {0}.EMP_GROUP_MASTER where EMP_GROUP_MASTER_ID = :p_EMP_GROUP_MASTER_ID",
                            Connect.Schema);
                        ocDeleteGM.Parameters.Add("p_EMP_GROUP_MASTER_ID", dgHistoryGM.CurrentRow.Cells["EMP_GROUP_MASTER_ID"].Value);
                        ocDeleteGM.ExecuteNonQuery();
                        Connect.Commit();
                        dgHistoryGM.Rows.Remove(dgHistoryGM.CurrentRow);
                    }
                }
            }
        }
        
        /// <summary>
        /// Сохранение группы мастера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            if (mbName_GM.Text.Trim().Replace("0", "") == "")
            {
                MessageBox.Show("Вы не ввели номер группы мастера!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                mbName_GM.Focus();
                return;
            }
            if (deBegin_Group.Date == null)
            {
                MessageBox.Show("Вы не ввели дату начала работы!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                deBegin_Group.Focus();
                return;
            }
            if (deBegin_Group.Date != null && deEnd_Group.Date != null)
            {
                if (deEnd_Group.Date < deBegin_Group.Date)
                {
                    MessageBox.Show("Дата окончания работы не может быть меньше даты начала работы!",
                        "АРМ \"Учет рабочего времени\"",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    deEnd_Group.Focus();
                    return;
                }
            }
            if (flagAdd && deBegin_Group.Date < Table.dCloseTable)
            {
                MessageBox.Show("Дата начала работы на группе меньше даты закрытия табеля!",
                    "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                deBegin_Group.Focus();
                return;
            }
            emp_GM.Save();
            Connect.Commit();
            emp_GM.Clear();
            dtHistoryGM.Clear();
            dtHistoryGM.Fill();
            EnabledControl(false);            
        }

        private void btRefresh_Group_Master_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите это сделать?",
                "АРМ \"Учет рабочего времени\"",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleCommand ocRefresh_Transfer_ID = new OracleCommand("", Connect.CurConnect);
                ocRefresh_Transfer_ID.BindByName = true;
                ocRefresh_Transfer_ID.CommandText = string.Format(
                    @"update {0}.EMP_GROUP_MASTER EG
                    set EG.TRANSFER_ID = 
                         (select T.FROM_POSITION from {0}.TRANSFER T
                            where T.TRANSFER_ID = EG.TRANSFER_ID)
                    where EMP_GROUP_MASTER_ID in 
                        (select EMP_GROUP_MASTER_ID from 
                            (select * from {0}.TRANSFER T join {0}.EMP_GROUP_MASTER EG on T.TRANSFER_ID = EG.TRANSFER_ID
                            where T.PER_NUM = :p_PER_NUM and T.TYPE_TRANSFER_ID = 3))",
                    Connect.Schema);
                ocRefresh_Transfer_ID.Parameters.Add("p_PER_NUM", OracleDbType.Varchar2).Value = _per_num;
                OracleTransaction transact = Connect.CurConnect.BeginTransaction();
                try
                {
                    ocRefresh_Transfer_ID.Transaction = transact;
                    ocRefresh_Transfer_ID.ExecuteNonQuery();
                    transact.Commit();
                }
                catch(Exception ex)
                {
                    transact.Rollback();
                    MessageBox.Show("Ошибка обновления перевода для группы мастера:\n"+ex.Message,
                        "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Connect.Commit();
            }
        }
    }
}
