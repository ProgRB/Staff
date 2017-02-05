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
using KadrWPF;
using WpfControlLibrary;

namespace Kadr
{
    public partial class HBPrivileged_Position : System.Windows.Forms.UserControl
    {
        OracleDataTable dtPP;
        PRIVILEGED_POSITION_seq priv_pos;
        public HBPrivileged_Position()
        {
            InitializeComponent();
            dtPP = new OracleDataTable("", Connect.CurConnect);
            dtPP.SelectCommand.CommandText = string.Format(Queries.GetQuery("Privileged_Position.sql"),
                DataSourceScheme.SchemeName);
            dtPP.Fill();
            dgView.DataSource = dtPP;
            foreach (DataGridViewColumn col in dgView.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;                
            }
            dgView.Columns["PRIVILEGED_POSITION_ID"].Visible = false;
            dgView.Columns["CODE_SUBDIV"].HeaderText = "Подр.";
            dgView.Columns["CODE_POS"].HeaderText = "Шифр";
            dgView.Columns["POS_NAME"].HeaderText = "Наименование профессии";
            dgView.Columns["SPECIAL_CONDITIONS"].HeaderText = "Особые условия";
            dgView.Columns["KPS"].HeaderText = "КПС";
            dgView.Columns["NUMBER_LIST"].HeaderText = "№ списка";

            priv_pos = new PRIVILEGED_POSITION_seq(Connect.CurConnect);
                        
            cbSubdiv_Name.AddBindingSource(priv_pos, PRIVILEGED_POSITION_seq.ColumnsName.SUBDIV_ID,
                new LinkArgument(AppDataSet.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            cbPos_Name.AddBindingSource(priv_pos, PRIVILEGED_POSITION_seq.ColumnsName.POS_ID,
                new LinkArgument(AppDataSet.position, POSITION_seq.ColumnsName.POS_NAME));
            tbSpecial_Conditions.AddBindingSource(priv_pos, PRIVILEGED_POSITION_seq.ColumnsName.SPECIAL_CONDITIONS);
            tbKPS.AddBindingSource(priv_pos, PRIVILEGED_POSITION_seq.ColumnsName.KPS);
            tbNumber_List.AddBindingSource(priv_pos, PRIVILEGED_POSITION_seq.ColumnsName.NUMBER_LIST);
            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            pnButton.EnableByRules();
        }

        void ReloadData()
        {
            dtPP.Clear();
            dtPP.Fill();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            priv_pos.Clear();
            priv_pos.AddNew();
            ((CurrencyManager)BindingContext[priv_pos]).Position = ((CurrencyManager)BindingContext[priv_pos]).Count;
            tbCode_Subdiv.Enabled = true;
            tbCode_Pos.Enabled = true;
            cbSubdiv_Name.Enabled = true;
            cbPos_Name.Enabled = true;
            tbSpecial_Conditions.Enabled = true;
            tbKPS.Enabled = true;
            tbNumber_List.Enabled = true;
            dgView.Enabled = false;
            btAddPriv_Pos.Enabled = false;
            btEditPriv_Pos.Enabled = false;
            btDeletePriv_Pos.Enabled = false;
            btExit.Text = "Отмена";
            btSavePriv_Pos.Enabled = true;
            this.Invalidate();
            tbCode_Subdiv.Focus();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (dgView.CurrentRow != null)
            {
                priv_pos.Clear();
                priv_pos.Fill("where PRIVILEGED_POSITION_ID = " +
                    dgView.CurrentRow.Cells["PRIVILEGED_POSITION_ID"].Value.ToString());
                tbCode_Subdiv.Enabled = true;
                tbCode_Pos.Enabled = true;
                cbSubdiv_Name.Enabled = true;
                cbPos_Name.Enabled = true;
                tbSpecial_Conditions.Enabled = true;
                tbKPS.Enabled = true;
                tbNumber_List.Enabled = true;
                dgView.Enabled = false;
                btAddPriv_Pos.Enabled = false;
                btEditPriv_Pos.Enabled = false;
                btDeletePriv_Pos.Enabled = false;
                btExit.Text = "Отмена";
                btSavePriv_Pos.Enabled = true;
                tbCode_Subdiv.Focus();
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (dgView.CurrentRow != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleCommand ocDeletePP = new OracleCommand("", Connect.CurConnect);
                    ocDeletePP.BindByName = true;
                    ocDeletePP.CommandText = string.Format("delete from {0}.PRIVILEGED_POSITION PP " +
                        "where PP.PRIVILEGED_POSITION_ID = :p_priv_pos_id", Connect.Schema);
                    ocDeletePP.Parameters.Add("p_priv_pos_id", OracleDbType.Decimal).Value =
                        dgView.CurrentRow.Cells["PRIVILEGED_POSITION_ID"].Value;
                    ocDeletePP.ExecuteNonQuery();                    
                    Connect.Commit();
                    ReloadData();
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали подразделение!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbSubdiv_Name.Focus();
                return;
            }
            if (cbPos_Name.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали должность!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbPos_Name.Focus();
                return;
            }
            if (tbSpecial_Conditions.Text == "")
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbSpecial_Conditions.Focus();
                return;
            }
            //if (tbKPS.Text == "")
            //{
            //    MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    tbKPS.Focus();
            //    return;
            //}
            dgView.Enabled = true;
            btAddPriv_Pos.Enabled = true;
            btEditPriv_Pos.Enabled = true;
            btDeletePriv_Pos.Enabled = true;
            btExit.Text = "Выход";
            btSavePriv_Pos.Enabled = false;
            tbCode_Subdiv.Enabled = false;
            tbCode_Pos.Enabled = false;
            cbSubdiv_Name.Enabled = false;
            cbPos_Name.Enabled = false;
            tbSpecial_Conditions.Enabled = false;
            tbKPS.Enabled = false;
            tbNumber_List.Enabled = false;
            priv_pos.Save();
            Connect.Commit();
            ReloadData();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                //Close();
            }
            else
            {
                dgView.Enabled = true;
                btAddPriv_Pos.Enabled = true;
                btEditPriv_Pos.Enabled = true;
                btDeletePriv_Pos.Enabled = true;
                btExit.Text = "Выход";
                btSavePriv_Pos.Enabled = false;
                tbCode_Subdiv.Enabled = false;
                tbCode_Pos.Enabled = false;
                cbSubdiv_Name.Enabled = false;
                cbPos_Name.Enabled = false;
                tbSpecial_Conditions.Enabled = false;
                tbKPS.Enabled = false;
                tbNumber_List.Enabled = false;
                priv_pos.Clear();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Проверка введенного шифра должности и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pos, cbPos_Name, 5, Connect.CurConnect, e, POSITION_seq.ColumnsName.POS_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "position", POSITION_seq.ColumnsName.CODE_POS.ToString(), tbCode_Pos.Text);
        }

        /// <summary>
        /// Событие изменения индекса подразделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbPos_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPos_Name.SelectedValue != null)
            {
                tbCode_Pos.Text = Library.CodeBySelectedValue(Connect.CurConnect, POSITION_seq.ColumnsName.CODE_POS.ToString(), Staff.DataSourceScheme.SchemeName,
                    "position", POSITION_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.SelectedValue.ToString());
            }
        }
    }
}
