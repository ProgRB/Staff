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
    public partial class HBSubdiv : Form
    {
        FormMain parentForm;        
        SUBDIV_seq subdiv;
        SERVICE_seq service;
        WORK_TYPE_seq work_type;
        TYPE_SUBDIV_seq type_subdiv;
        GR_WORK_seq gr_work;
        bool flagAdd = false;
        public HBSubdiv(FormMain _parentForm)
        {
            InitializeComponent();
            parentForm = _parentForm;
            subdiv = new SUBDIV_seq(Connect.CurConnect);
            subdiv.Fill(string.Format("order by {0}", SUBDIV_seq.ColumnsName.CODE_SUBDIV));
            service = new SERVICE_seq(Connect.CurConnect);
            service.Fill(string.Format("order by {0}", SERVICE_seq.ColumnsName.SERVICE_NAME));
            work_type = new WORK_TYPE_seq(Connect.CurConnect);
            work_type.Fill(string.Format("order by {0}", WORK_TYPE_seq.ColumnsName.TYPE_NAME));
            type_subdiv = new TYPE_SUBDIV_seq(Connect.CurConnect);
            type_subdiv.Fill(string.Format("order by {0}", TYPE_SUBDIV_seq.ColumnsName.TYPE_SUBDIV_NAME));
            gr_work = new GR_WORK_seq(Connect.CurConnect);
            gr_work.Fill(string.Format("order by {0}", GR_WORK_seq.ColumnsName.GR_WORK_NAME));
            dgSubdiv.AddBindingSource(subdiv, new LinkArgument(service, SERVICE_seq.ColumnsName.SERVICE_NAME), 
                new LinkArgument(work_type, WORK_TYPE_seq.ColumnsName.TYPE_NAME),
                new LinkArgument(type_subdiv, TYPE_SUBDIV_seq.ColumnsName.TYPE_SUBDIV_NAME),
                new LinkArgument(gr_work, GR_WORK_seq.ColumnsName.GR_WORK_NAME));
            dgSubdiv.Columns["parent_id"].Visible = false;
            dgSubdiv.Columns["from_subdiv"].Visible = false;
            dgSubdiv.Columns["TYPE_SUBDIV_ID"].DisplayIndex = 6; 
            dgSubdiv.Columns["GR_WORK_ID"].DisplayIndex = 7;

            //dgSubdiv.Columns["TYPE_SUBDIV_ID"].HeaderText = "Наименование типа подразделения";
            dgSubdiv.Columns["sub_actual_sign"].HeaderText = "Действующее подразделение";
            tbCode_Subdiv.AddBindingSource(subdiv, SUBDIV_seq.ColumnsName.CODE_SUBDIV);
            tbName_Subdiv.AddBindingSource(subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME);
            deSub_Date_Start.AddBindingSource(subdiv, SUBDIV_seq.ColumnsName.SUB_DATE_START);
            deSub_Date_End.AddBindingSource(subdiv, SUBDIV_seq.ColumnsName.SUB_DATE_END);
            chSub_Actual_Sign.AddBindingSource(subdiv, SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN);
            cbService_ID.AddBindingSource(subdiv, SERVICE_seq.ColumnsName.SERVICE_ID, new LinkArgument(service,
                SERVICE_seq.ColumnsName.SERVICE_NAME));
            cbWork_Type_ID.AddBindingSource(subdiv, WORK_TYPE_seq.ColumnsName.WORK_TYPE_ID,
                new LinkArgument(work_type, WORK_TYPE_seq.ColumnsName.TYPE_NAME));
            cbType_Subdiv.AddBindingSource(subdiv, TYPE_SUBDIV_seq.ColumnsName.TYPE_SUBDIV_ID,
                new LinkArgument(type_subdiv, TYPE_SUBDIV_seq.ColumnsName.TYPE_SUBDIV_NAME));
            cbGr_Work_ID.AddBindingSource(subdiv, GR_WORK_seq.ColumnsName.GR_WORK_ID,
                new LinkArgument(gr_work, GR_WORK_seq.ColumnsName.GR_WORK_NAME));

            pnButton.EnableByRules();
            btSaveSubdiv.Enabled = false;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            flagAdd = true;
            subdiv.AddNew();
            ((CurrencyManager)BindingContext[subdiv]).Position = ((CurrencyManager)BindingContext[subdiv]).Count - 1;
            dgSubdiv.Enabled = false;
            btAddSubdiv.Enabled = false;
            btEditSubdiv.Enabled = false;
            btDeleteSubdiv.Enabled = false;
            btExit.Text = "Отмена";
            btSaveSubdiv.Enabled = true;
            tbCode_Subdiv.Enabled = true;
            tbName_Subdiv.Enabled = true;
            deSub_Date_Start.Enabled = true;
            deSub_Date_End.Enabled = true;
            chSub_Actual_Sign.Enabled = true;
            cbService_ID.Enabled = true;
            cbType_Subdiv.Enabled = true;
            cbWork_Type_ID.Enabled = true;
            cbGr_Work_ID.Enabled = true;
            this.Invalidate();
            tbCode_Subdiv.Focus();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            flagAdd = false;
            dgSubdiv.Enabled = false;
            btAddSubdiv.Enabled = false;
            btEditSubdiv.Enabled = false;
            btDeleteSubdiv.Enabled = false;
            btExit.Text = "Отмена";
            btSaveSubdiv.Enabled = true;
            tbCode_Subdiv.Enabled = true;
            tbName_Subdiv.Enabled = true;
            deSub_Date_Start.Enabled = true;
            deSub_Date_End.Enabled = true;
            chSub_Actual_Sign.Enabled = true;
            cbService_ID.Enabled = true;
            cbType_Subdiv.Enabled = true;
            cbWork_Type_ID.Enabled = true;
            cbGr_Work_ID.Enabled = true;
            tbCode_Subdiv.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sub = ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).SUBDIV_ID.ToString();
                dgSubdiv.Rows.Remove(dgSubdiv.CurrentRow);
                if (Authorization.subdivisions.DeleteSubdiv(sub))
                {
                    subdiv.Save();
                    Connect.Commit();
                }
                if (dgSubdiv.Rows.Count == 0)
                {
                    btEditSubdiv.Enabled = false;
                    btDeleteSubdiv.Enabled = false;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tbCode_Subdiv.Text.ToString() == "")
            {
                MessageBox.Show("Вы не ввели код подразделения!", "АСУ \"Кадры\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbCode_Subdiv.Focus();
                return;
            }
            if (tbName_Subdiv.Text.ToString() == "")
            {
                MessageBox.Show("Вы не ввели наименование подразделения!", "АСУ \"Кадры\"", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbName_Subdiv.Focus();
                return;
            }
            if (cbService_ID.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали службу!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbService_ID.Focus();
                return;
            }
            if (cbWork_Type_ID.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали характер работы!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbWork_Type_ID.Focus();
                return;
            }
            if (cbType_Subdiv.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали тип подразделения!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbType_Subdiv.Focus();
                return;
            }
            if (cbGr_Work_ID.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали график работы подразделения!", "АСУ \"Кадры\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbGr_Work_ID.Focus();
                return;
            }
            dgSubdiv.Enabled = true;
            btAddSubdiv.Enabled = true;
            btEditSubdiv.Enabled = true;
            btDeleteSubdiv.Enabled = true;
            btExit.Text = "Выход";
            btSaveSubdiv.Enabled = false;
            tbCode_Subdiv.Enabled = false;
            tbName_Subdiv.Enabled = false;
            deSub_Date_Start.Enabled = false;
            deSub_Date_End.Enabled = false;
            chSub_Actual_Sign.Enabled = false;
            cbService_ID.Enabled = false;
            cbType_Subdiv.Enabled = false;
            cbWork_Type_ID.Enabled = false;
            cbGr_Work_ID.Enabled = false;
            if (flagAdd)
            {
                if (Authorization.subdivisions.InsertSubDiv(new Subdivision(
                    ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).SUBDIV_ID.ToString(),
                    ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).CODE_SUBDIV,
                    ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).SUBDIV_NAME)))
                {
                    if (((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).PARENT_ID == null)
                        ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).PARENT_ID = 0;
                    subdiv.Save();
                    Connect.Commit();
                }
            }
            else
            {
                if (Authorization.subdivisions.UpdateSubDiv(new Subdivision(
                    ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).SUBDIV_ID.ToString(),
                    ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).CODE_SUBDIV,
                    ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).SUBDIV_NAME)))
                {
                    if (((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).PARENT_ID == null)
                        ((SUBDIV_obj)((CurrencyManager)BindingContext[subdiv]).Current).PARENT_ID = 0;
                    subdiv.Save();
                    Connect.Commit();
                }
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
                dgSubdiv.Enabled = true;
                btAddSubdiv.Enabled = true;
                btEditSubdiv.Enabled = true;
                btDeleteSubdiv.Enabled = true;
                btExit.Text = "Выход";
                btSaveSubdiv.Enabled = false;
                tbCode_Subdiv.Enabled = false;
                tbName_Subdiv.Enabled = false;
                deSub_Date_Start.Enabled = false;
                deSub_Date_End.Enabled = false;
                chSub_Actual_Sign.Enabled = false;
                cbService_ID.Enabled = false;
                cbType_Subdiv.Enabled = false;
                cbWork_Type_ID.Enabled = false;
                cbGr_Work_ID.Enabled = false;
                subdiv.RollBack();
                Connect.Rollback();
                dgSubdiv.Invalidate();
                ((CurrencyManager)BindingContext[subdiv]).Refresh();
                this.Invalidate();
            }
        }

        private void HBSubdiv_Activated(object sender, EventArgs e)
        {
            if (dgSubdiv.Rows.Count == 0)
            {
                this.btEditSubdiv.Enabled = false;
                this.btDeleteSubdiv.Enabled = false;
            }
        }
    }
}
