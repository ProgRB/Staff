using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using Kadr;
using LibraryKadr;
using Oracle.DataAccess.Client;
namespace Kadr.Shtat
{
    public partial class ReplEmpAdd : Form
    {
        DataSet ds = new DataSet();
        public object this[string ColumnName]
        {
            get
            {
                return ds.Tables["repl_data"].Rows[0][ColumnName];
            }
            set
            {
                ds.Tables["REPL_DATA"].Rows[0][ColumnName] = value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repl_emp_id">Номер замещения уникальный</param>
        /// <param name="repl_transfer"> Перевод того, кого будут замещать</param>
        /// <param name="CombRepl">Признак что поставить совмещение</param>
        public ReplEmpAdd(object repl_emp_id, object repl_transfer, bool CombRepl)
            : this(repl_emp_id, repl_transfer, null, CombRepl)
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repl_emp_id">Уникальный номер зам</param>
        /// <param name="repl_transfer"> Кого будут замещать</param>
        /// <param name="transfer_id"> Кто будет замещать (совмещать)</param>
        /// <param name="CombRepl"> Признак совмещения</param>
        public ReplEmpAdd(object repl_emp_id,object repl_transfer,object transfer_id,bool CombRepl)
        {           
            InitializeComponent();

            repl_start.Validating += new CancelEventHandler(Library.ValidatingDate);
            repl_end.Validating += new CancelEventHandler(Library.ValidatingDate);
            REPL_PERCENT.KeyPress += new KeyPressEventHandler(Library.ValidatingNumeric);
            date_order.Validating += new CancelEventHandler(Library.ValidatingDate);

            new OracleDataAdapter(string.Format("select * from {0}.type_repl", DataSourceScheme.SchemeName), Connect.CurConnect).Fill(ds,"type_repl");
            type_repl.DataSource = ds.Tables["type_repl"];
            type_repl.DisplayMember = "repl_name";
            type_repl.ValueMember = "type_repl_id";
            OracleDataAdapter a = new OracleDataAdapter(string.Format(Queries.GetQuery(@"Table\select_MAIN_and_REPL_data.sql"), DataSourceScheme.SchemeName), Connect.CurConnect);
            a.SelectCommand.BindByName = true;
            a.SelectCommand.Parameters.Add("p_repl_emp_id",OracleDbType.Decimal,repl_emp_id, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_transfer_id", OracleDbType.Decimal, transfer_id, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_replacing_transfer_id", OracleDbType.Decimal, repl_transfer, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("p_sign_combine", OracleDbType.Decimal, CombRepl?1:0, ParameterDirection.Input);
            a.SelectCommand.Parameters.Add("c", OracleDbType.RefCursor,ParameterDirection.Output);
            a.Fill(ds,"repl_data");
            if (repl_emp_id==null)
            {
                if (ds.Tables["repl_data"].Rows.Count == 0)
                {
                    ds.Tables["repl_data"].Rows.Add(ds.Tables["repl_data"].NewRow());
                    this["sign_combine"] = (CombRepl ? 1 : 0);
                }
                if (repl_transfer!=null)
                    this["replacing_transfer_id"] = repl_transfer;
            }
            per_num.Text = ds.Tables["repl_data"].Rows[0]["per_num"].ToString();
            fio.Text = ds.Tables["repl_data"].Rows[0]["fio"].ToString();
            pos_name.Text = ds.Tables["repl_data"].Rows[0]["pos_name"].ToString();

            whos_per_num.Text = ds.Tables["repl_data"].Rows[0]["per_num2"].ToString();
            whos_fio.Text = ds.Tables["repl_data"].Rows[0]["fio2"].ToString();
            whos_pos_name.Text = ds.Tables["repl_data"].Rows[0]["pos_name2"].ToString();

            repl_start.DataBindings.Add("Text", ds.Tables["repl_data"], "repl_start", false, DataSourceUpdateMode.OnPropertyChanged);
            repl_start.DataBindings["Text"].Parse += new ConvertEventHandler(ReplEmpParseDateBinding);
            repl_end.DataBindings.Add("Text", ds.Tables["repl_data"], "repl_end", false, DataSourceUpdateMode.OnPropertyChanged);
            repl_end.DataBindings["Text"].Parse += new ConvertEventHandler(ReplEmpParseDateBinding);
            repl_order.DataBindings.Add("Text", ds.Tables["repl_data"], "repl_order", false, DataSourceUpdateMode.OnPropertyChanged);
            date_order.DataBindings.Add("Text", ds.Tables["repl_data"], "date_order", false, DataSourceUpdateMode.OnPropertyChanged);
            date_order.DataBindings["Text"].Parse += new ConvertEventHandler(ReplEmpParseDateBinding);
            REPL_PERCENT.DataBindings.Add("Text", ds.Tables["repl_data"], "repl_percent", false, DataSourceUpdateMode.OnPropertyChanged);
            sign_longtm.DataBindings.Add("Checked", ds.Tables["repl_data"], "sign_longtime", false, DataSourceUpdateMode.OnPropertyChanged);
            repl_sal_add_repl_emp.DataBindings.Add("Text", ds.Tables["repl_data"], "repl_sal", false, DataSourceUpdateMode.OnPropertyChanged);
            repl_sal_add_repl_emp.DataBindings["Text"].Parse += new ConvertEventHandler(ReplEmpAdd_ParseNumber);
            radio_bt_addcombempShtat.DataBindings.Add("Checked", ds.Tables["repl_data"], "sign_combine", false, DataSourceUpdateMode.OnPropertyChanged);
            type_repl.DataBindings.Add("SelectedValue", ds.Tables["repl_data"], "type_repl_id", false, DataSourceUpdateMode.OnPropertyChanged);
            cbSignLockRepl.DataBindings.Add("Checked", ds.Tables["repl_data"], "SIGN_LOCK_REPL", false, DataSourceUpdateMode.OnPropertyChanged);
            radio_bt_addcombempShtat.EnableByRules(false);
            radio_bt_addreplempShtat.EnableByRules(false);
            cbSignLockRepl.EnableByRules(false);
        }

        void ReplEmpAdd_ParseNumber(object sender, ConvertEventArgs e)
        {
            if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString().Trim()))
                e.Value = DBNull.Value;
        }

        void ReplEmpParseDateBinding(object sender, ConvertEventArgs e)
        {
            if (sender!=null && (sender as Binding).BindableComponent is MaskedTextBox && !((sender as Binding).BindableComponent as MaskedTextBox).MaskFull)
                e.Value = DBNull.Value;
        }
        
        private void btFindEmp_Click(object sender, EventArgs e)
        {
            FindEmpSingle frmFind = new FindEmpSingle();
            frmFind.ShowDialog(this);
            if (frmFind.transfer_id !=null)
            {
                per_num.Text = frmFind.per_num;
                fio.Text = frmFind.fio;
                pos_name.Text = frmFind.pos_name;
                ds.Tables["repl_data"].Rows[0]["transfer_id"] = frmFind.transfer_id;
            }
        }
        private void btFindReplacingEmp_Click(object sender, EventArgs e)
        {
            FindEmpSingle frmFind = new FindEmpSingle();
            frmFind.ShowDialog(this);
            if (frmFind.transfer_id != null)
            {
                whos_per_num.Text = frmFind.per_num;
                whos_fio.Text = frmFind.fio;
                whos_pos_name.Text = frmFind.pos_name;
                ds.Tables["repl_data"].Rows[0]["replacing_transfer_id"] = frmFind.transfer_id;
            }
        }
        private void btOk_Click(object sender, EventArgs e)
        {
            
            OracleCommand cmd = new OracleCommand(string.Format(@"begin {0}.REPL_EMP_UPDATE( :p_REPL_EMP_ID, :p_TRANSFER_ID, :p_REPL_SAL, :p_REPL_START, :p_REPL_END, :p_REPL_ACTUAL_SIGN,
                        :p_REPL_ORDER, :p_DATE_ORDER, :p_SIGN_LONGTIME, :p_TYPE_REPL_ID, :p_SIGN_COMBINE, :p_REPLACING_TRANSFER_ID, :p_REPL_PERCENT); end;", Connect.Schema), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_REPL_EMP_ID", OracleDbType.Decimal, this["REPL_EMP_ID"], ParameterDirection.InputOutput).DbType = DbType.Decimal;
            cmd.Parameters.Add("p_TRANSFER_ID", OracleDbType.Decimal, this["TRANSFER_ID"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPL_SAL", OracleDbType.Decimal, this["REPL_SAL"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPL_START", OracleDbType.Date, this["REPL_START"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPL_END", OracleDbType.Date, this["REPL_END"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPL_ACTUAL_SIGN", OracleDbType.Decimal, this["REPL_ACTUAL_SIGN"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPL_ORDER", OracleDbType.Varchar2, this["REPL_ORDER"], ParameterDirection.Input);
            cmd.Parameters.Add("p_DATE_ORDER", OracleDbType.Date, this["DATE_ORDER"], ParameterDirection.Input);
            cmd.Parameters.Add("p_SIGN_LONGTIME", OracleDbType.Decimal, this["SIGN_LONGTIME"], ParameterDirection.Input);
            cmd.Parameters.Add("p_TYPE_REPL_ID", OracleDbType.Decimal, this["TYPE_REPL_ID"], ParameterDirection.Input);
            cmd.Parameters.Add("p_SIGN_COMBINE", OracleDbType.Decimal, this["SIGN_COMBINE"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPLACING_TRANSFER_ID", OracleDbType.Decimal, this["REPLACING_TRANSFER_ID"], ParameterDirection.Input);
            cmd.Parameters.Add("p_REPL_PERCENT", OracleDbType.Decimal, this["REPL_PERCENT"], ParameterDirection.Input);
            bool fl = false; // признак неудачной обработки документа. если сохранился то фолс
            OracleTransaction tr = Connect.CurConnect.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                this["REPL_EMP_ID"] = cmd.Parameters["P_repl_emp_id"].Value;
                tr.Commit();
                ds.AcceptChanges();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                MessageBox.Show(Library.GetMessageException(ex));
                fl = true;
            }
            if (!fl)
                try
                {
                    if (GrantedRoles.GetGrantedRole("ACCOUNTANT_EDIT"))
                    {
                        cmd = new OracleCommand(string.Format(@"begin {0}.REPL_EMP_CLOSE(:p_repl_emp_id, :p_sign);end;", Connect.Schema), Connect.CurConnect);
                        cmd.Parameters.Add("p_repl_emp_id", OracleDbType.Decimal, this["REPL_EMP_ID"], ParameterDirection.Input);
                        cmd.Parameters.Add("p_sign", OracleDbType.Decimal, this["SIGN_LOCK_REPL"], ParameterDirection.Input);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Library.GetMessageException(ex));
                    fl = true;
                }
            if (!fl) 
                this.DialogResult = DialogResult.OK;           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            group_combine.Enabled = radio_bt_addcombempShtat.Checked;
            group_repl.Enabled = radio_bt_addreplempShtat.Checked;
        }

        private void cbSignLockRepl_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = groupBox2.Enabled = !cbSignLockRepl.Checked;
        }

        public static bool CanOpenLink(object sender, LinkData e)
        {
            return LinkKadr.CanExecuteByAccessSubdiv(e.Worker_ID, "Table");
        }

        public static void OpenStaticView(object sender, LinkData e)
        {
            ReplEmpForm f = new ReplEmpForm(e.Transfer_id, null);
            f.ShowDialog();
        }
    }
}
