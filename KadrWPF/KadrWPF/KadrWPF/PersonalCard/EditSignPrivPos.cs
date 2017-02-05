using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Kadr
{
    public partial class EditSignPrivPos : Form
    {
        decimal transfer_id;        
        OracleDataTable dtTransfer, dtPriv_Pos;
        OracleCommand ocUpdateSignPrivPos;
        public EditSignPrivPos(decimal _transfer_id)
        {
            InitializeComponent();
            transfer_id = _transfer_id;
            dtPriv_Pos = new OracleDataTable("", Connect.CurConnect);
            dtPriv_Pos.SelectCommand.CommandText = string.Format(
                "select PP.PRIVILEGED_POSITION_ID, PP.SPECIAL_CONDITIONS, PP.KPS " +
                "from {0}.PRIVILEGED_POSITION PP " +
                "where PP.SUBDIV_ID = :p_subdiv_id and PP.POS_ID = :p_pos_id ",
                Connect.Schema);
            dtPriv_Pos.SelectCommand.Parameters.Add("p_subdiv_id", OracleDbType.Decimal);
            dtPriv_Pos.SelectCommand.Parameters.Add("p_pos_id", OracleDbType.Decimal);
            dtTransfer = new OracleDataTable("", Connect.CurConnect);
            dtTransfer.SelectCommand.CommandText = string.Format(
                Queries.GetQuery("TransferForEditPrivPos.sql"), Connect.Schema);
            dtTransfer.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal).Value = transfer_id;
            dtTransfer.Fill();
            dgTransfer.AutoGenerateColumns = false;
            dgTransfer.DataSource = dtTransfer;
            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.Name = "CODE_SUBDIV";
            c1.HeaderText = "Подр.";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;            
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["CODE_SUBDIV"].DataPropertyName = "CODE_SUBDIV";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "POS_NAME";
            c1.HeaderText = "Наименование должности";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["POS_NAME"].DataPropertyName = "POS_NAME";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "DATE_TRANSFER";
            c1.HeaderText = "Дата движения";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["DATE_TRANSFER"].DataPropertyName = "DATE_TRANSFER";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "TYPE_TR";
            c1.HeaderText = "Тип перевода";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["TYPE_TR"].DataPropertyName = "TYPE_TR";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "SIGN_COMB";
            c1.HeaderText = "Совм.";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["SIGN_COMB"].DataPropertyName = "SIGN_COMB";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "SPECIAL_CONDITIONS";
            c1.HeaderText = "Особые условия";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["SPECIAL_CONDITIONS"].DataPropertyName = "SPECIAL_CONDITIONS";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "KPS";
            c1.HeaderText = "КПС";
            c1.ReadOnly = true;
            c1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            c1.DefaultCellStyle.Font = new Font(Font.FontFamily, 9);
            c1.HeaderCell.Style.Font = new Font(Font.FontFamily, 9, FontStyle.Bold);
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["KPS"].DataPropertyName = "KPS";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "SUBDIV_ID";
            c1.Visible = false;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["SUBDIV_ID"].DataPropertyName = "SUBDIV_ID";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "POS_ID";
            c1.Visible = false;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["POS_ID"].DataPropertyName = "POS_ID";
            c1 = new DataGridViewTextBoxColumn();
            c1.Name = "ACCOUNT_DATA_ID";
            c1.Visible = false;
            dgTransfer.Columns.Add(c1);
            dgTransfer.Columns["ACCOUNT_DATA_ID"].DataPropertyName = "ACCOUNT_DATA_ID";

            dgTransfer.SelectionChanged += new EventHandler(dgTransfer_SelectionChanged);

            ocUpdateSignPrivPos = new OracleCommand("", Connect.CurConnect);
            ocUpdateSignPrivPos.BindByName = true;
            ocUpdateSignPrivPos.CommandText = string.Format(
                "begin {0}.ACCOUNT_DATA_update_PRIV(:p_sign_priv_pos,:p_account_data_id); end; ", 
                Connect.Schema);
            ocUpdateSignPrivPos.Parameters.Add("p_sign_priv_pos", OracleDbType.Decimal);
            ocUpdateSignPrivPos.Parameters.Add("p_account_data_id", OracleDbType.Decimal);

            pnButton.EnableByRules();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (dgTransfer.CurrentRow != null)
            {
                dtPriv_Pos.Clear();
                dtPriv_Pos.SelectCommand.Parameters["p_subdiv_id"].Value =
                    dgTransfer.CurrentRow.Cells["SUBDIV_ID"].Value;
                dtPriv_Pos.SelectCommand.Parameters["p_pos_id"].Value =
                    dgTransfer.CurrentRow.Cells["POS_ID"].Value;
                dtPriv_Pos.Fill();
                cbKPS.DataSource = dtPriv_Pos;
                cbKPS.DisplayMember = "KPS";
                cbKPS.ValueMember = "PRIVILEGED_POSITION_ID";              
                tbSpec_Con.DataBindings.Add("Text", dtPriv_Pos, "SPECIAL_CONDITIONS");
                tbSpec_Con.Enabled = true;
                cbKPS.Enabled = true;
                btEditPriv_Pos.Enabled = false;
                btSavePriv_Pos.Enabled = true;
                btCancelPriv_Pos.Enabled = true;
                dgTransfer.Enabled = false;
                cbKPS.Focus();                
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            ocUpdateSignPrivPos.Parameters["p_sign_priv_pos"].Value = cbKPS.SelectedValue;
            ocUpdateSignPrivPos.Parameters["p_account_data_id"].Value =
                dgTransfer.CurrentRow.Cells["ACCOUNT_DATA_ID"].Value;
            ocUpdateSignPrivPos.ExecuteNonQuery();
            Connect.Commit();
            btEditPriv_Pos.Enabled = true;
            btSavePriv_Pos.Enabled = false;
            btCancelPriv_Pos.Enabled = false;
            dgTransfer.Enabled = true;
            tbSpec_Con.Enabled = false;
            cbKPS.Enabled = false;
            tbSpec_Con.DataBindings.Clear();
            cbKPS.DataSource = null;
            dtTransfer.Clear();
            dtTransfer.Fill();            
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            dgTransfer.Enabled = true;
            btEditPriv_Pos.Enabled = true;
            btSavePriv_Pos.Enabled = false;
            btCancelPriv_Pos.Enabled = false;
            tbSpec_Con.Enabled = false;
            cbKPS.Enabled = false;
            tbSpec_Con.DataBindings.Clear();
            cbKPS.DataSource = null;
            dtTransfer.Clear();
            dtTransfer.Fill();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgTransfer_SelectionChanged(object sender, EventArgs e)
        {
            if (dgTransfer.CurrentRow != null)
            {
                tbSpec_Con.Text = dgTransfer.CurrentRow.Cells["SPECIAL_CONDITIONS"].Value.ToString();
                cbKPS.Text = dgTransfer.CurrentRow.Cells["KPS"].Value.ToString();
            }
        }

        private void btViewPrivPos_Click(object sender, EventArgs e)
        {
            //HBPrivileged_Position priv_pos = new HBPrivileged_Position();            
            //priv_pos.ShowDialog();
        }
    }
}
