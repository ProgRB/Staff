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
    public partial class HandBookSpec : System.Windows.Forms.UserControl
    {
        SPECIALITY_seq speciality;
        TYPE_PER_DOC_seq type_per_doc;
        REASON_DISMISS_seq reason_dismiss;
        TYPE_EDU_seq type_edu;
        string handBook;
        public HandBookSpec(string _handBook)
        {
            InitializeComponent();
            handBook = _handBook;
            Design();
            if (!GrantedRoles.GetGrantedRole("STAFF_PERSONNEL"))
            {
                btAdd.Enabled = false;
                btDelete.Enabled = false;
                btEdit.Enabled = false;
                btSave.Enabled = false;
            }
        }

        void Design()
        {
            switch (handBook)
            {
                case "speciality":
                    {
                        this.Text = "Справочник специальностей";
                        speciality = new SPECIALITY_seq(Connect.CurConnect);
                        speciality.Fill(string.Format("order by {0}", SPECIALITY_seq.ColumnsName.NAME_SPEC));
                        dgView.AddBindingSource(speciality, null);   
                        lbName1.Text = "Наименование специальности";                     
                        tbName1.AddBindingSource(speciality, SPECIALITY_seq.ColumnsName.NAME_SPEC);
                        lbName2.Text = "Код специальности";
                        tbName2.AddBindingSource(speciality, SPECIALITY_seq.ColumnsName.CODE_SPEC);
                        chEdu_Sign.AddBindingSource(speciality, SPECIALITY_seq.ColumnsName.EDU_SIGN);                     
                        break;
                    }
                case "type_per_doc":
                    {
                        this.Text = "Справочник документов личности";
                        type_per_doc = new TYPE_PER_DOC_seq(Connect.CurConnect);
                        type_per_doc.Fill(string.Format("order by {0}", TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
                        dgView.AddBindingSource(type_per_doc, null);                        
                        lbName1.Text = "Наименование документа";
                        tbName1.AddBindingSource(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.NAME_DOC);
                        lbName2.Text = "Шаблон серии";
                        tbName2.AddBindingSource(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.TEMPL_SER);                        
                        lbName3.Text = "Шаблон номера";
                        tbName3.AddBindingSource(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.TEMPL_NUM);
                        tbName3.Visible = true;
                        lbName3.Visible = true;
                        gbNote.Visible = true;                        
                        break;
                    }
                case "reason_dismiss":
                    {
                        this.Text = "Справочник причин увольнения";
                        reason_dismiss = new REASON_DISMISS_seq(Connect.CurConnect);
                        reason_dismiss.Fill(string.Format("order by {0}", 
                            REASON_DISMISS_seq.ColumnsName.REASON_NAME));
                        dgView.AddBindingSource(reason_dismiss, null);
                        dgView.Columns["SIGN_GOOD_REASON"].DisplayIndex = dgView.Columns.Count - 1;
                        lbName1.Text = "Наименование вида увольнения";
                        tbName1.AddBindingSource(reason_dismiss, REASON_DISMISS_seq.ColumnsName.REASON_NAME);
                        lbName2.Text = "Статья";
                        tbName2.Size = tbName1.Size;
                        tbName2.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        tbName2.AddBindingSource(reason_dismiss, REASON_DISMISS_seq.ColumnsName.REASON_ARTICLE);
                        lbName3.Text = "Наименование приказа";
                        lbName3.Location = new Point(11, 204);
                        tbName3.Size = tbName1.Size;
                        tbName3.Location = new Point(10, 222);
                        tbName3.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        tbName3.AddBindingSource(reason_dismiss, REASON_DISMISS_seq.ColumnsName.REASON_ORDER);
                        chEdu_Sign.AddBindingSource(reason_dismiss, REASON_DISMISS_seq.ColumnsName.SIGN_GOOD_REASON);
                        lbEdu_Sign.Text = "Признак уважительной причины увольнения";
                        lbEdu_Sign.Location = new Point(30, 288);
                        chEdu_Sign.Location = new Point(10, 290);
                        tbName3.Visible = true;
                        lbName3.Visible = true;
                        lbEdu_Sign.Visible = true;
                        chEdu_Sign.Visible = true;
                        break;
                    }
                case "type_edu":
                    {
                        this.Text = "Справочник видов образования";
                        type_edu = new TYPE_EDU_seq(Connect.CurConnect);
                        type_edu.Fill("order by TE_NAME");
                        dgView.AddBindingSource(type_edu, null);
                        lbName1.Text = "Наименование вида образования";
                        tbName1.AddBindingSource(type_edu, TYPE_EDU_seq.ColumnsName.TE_NAME);
                        lbName2.Text = "Группа приоритета для отчетов о кол-ве уволенных";
                        tbName2.AddBindingSource(type_edu, TYPE_EDU_seq.ColumnsName.TE_PRIORITY);
                        lbName3.Location = new Point(11, 174);
                        lbName3.Text = "Приоритет";
                        tbName3.AddBindingSource(type_edu, TYPE_EDU_seq.ColumnsName.TYPE_EDU_PRIOR);
                        tbName3.Location = new Point(10, 192);
                        tbName3.Visible = true;
                        lbName3.Visible = true;
                        break;
                    }
                default:
                    break;
            }
        }
        private void btAdd_Click(object sender, EventArgs e)
        {
            switch (handBook)
            {
                case "speciality":
                    {
                        speciality.AddNew();
                        ((CurrencyManager)BindingContext[speciality]).Position = ((CurrencyManager)BindingContext[speciality]).Count;
                        break;
                    }
                case "type_per_doc":
                    {
                        type_per_doc.AddNew();
                        ((CurrencyManager)BindingContext[type_per_doc]).Position = ((CurrencyManager)BindingContext[type_per_doc]).Count;
                        break;
                    }
                case "reason_dismiss":
                    {
                        reason_dismiss.AddNew();
                        ((CurrencyManager)BindingContext[reason_dismiss]).Position = ((CurrencyManager)BindingContext[reason_dismiss]).Count;
                        break;
                    }
            }
            chEdu_Sign.Enabled = true;
            tbName2.Enabled = true;
            tbName3.Enabled = true;
            dgView.Enabled = false;
            btAdd.Enabled = false;
            btEdit.Enabled = false;
            btDelete.Enabled = false;
            btExit.Text = "Отмена";
            btSave.Enabled = true;
            tbName1.Enabled = true;
            this.Invalidate();     
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            chEdu_Sign.Enabled = true;
            tbName2.Enabled = true;
            tbName3.Enabled = true;
            dgView.Enabled = false;
            btAdd.Enabled = false;
            btEdit.Enabled = false;
            btDelete.Enabled = false;
            btExit.Text = "Отмена";
            btSave.Enabled = true;
            tbName1.Enabled = true;
            tbName1.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgView.Rows.Remove(dgView.CurrentRow);
                switch (handBook)
                {
                    case "speciality":
                        {
                            speciality.Save();
                            break;
                        }
                    case "type_per_doc":
                        {
                            type_per_doc.Save();                            
                            break;
                        }
                    case "reason_dismiss":
                        {
                            reason_dismiss.Save();                            
                            break;
                        }
                }
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
            if (tbName1.Text != "")
            {
                dgView.Enabled = true;
                btAdd.Enabled = true;
                btEdit.Enabled = true;
                btDelete.Enabled = true;
                btExit.Text = "Выход";
                btSave.Enabled = false;
                tbName1.Enabled = false;
                chEdu_Sign.Enabled = false;
                tbName2.Enabled = false;
                tbName3.Enabled = false;
                switch (handBook)
                {
                    case "speciality":
                        {
                            speciality.Save();
                            break;
                        }
                    case "type_per_doc":
                        {
                            type_per_doc.Save();
                            break;
                        }
                    case "reason_dismiss":
                        {
                            reason_dismiss.Save();
                            break;
                        }
                }
                Connect.Commit();
            }
            else
            {
                MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbName1.Focus();
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
                dgView.Enabled = true;
                btAdd.Enabled = true;
                btEdit.Enabled = true;
                btDelete.Enabled = true;
                btExit.Text = "Выход";
                btSave.Enabled = false;
                tbName1.Enabled = false;
                chEdu_Sign.Enabled = false;
                tbName2.Enabled = false;
                tbName3.Enabled = false;
                switch (handBook)
                {
                    case "speciality":
                        {
                            speciality.RollBack();
                            ((CurrencyManager)BindingContext[speciality]).Refresh();
                            break;
                        }
                    case "type_per_doc":
                        {
                            type_per_doc.RollBack();
                            ((CurrencyManager)BindingContext[type_per_doc]).Refresh();
                            break;
                        }
                    case "reason_dismiss":
                        {
                            reason_dismiss.RollBack();
                            ((CurrencyManager)BindingContext[reason_dismiss]).Refresh();
                            break;
                        }
                }
                Connect.Rollback();
                dgView.Invalidate();
                this.Invalidate();
            }
        }

        //private void HandBookSpec_Activated(object sender, EventArgs e)
        //{
        //    if (dgView.Rows.Count == 0)
        //    {
        //        this.btEdit.Enabled = false;
        //        this.btDelete.Enabled = false;
        //    }
        //}

        //private void HandBookSpec_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (btSave.Enabled == true)
        //    {
        //        DialogResult dialog = MessageBox.Show("Вы не сохранили изменения данных. Сохранить изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            if (tbName1.Text != "")
        //            {
        //                switch (handBook)
        //                {
        //                    case "speciality":
        //                        {
        //                            speciality.Save();
        //                            break;
        //                        }
        //                    case "type_per_doc":
        //                        {
        //                            type_per_doc.Save();
        //                            break;
        //                        }
        //                    case "reason_dismiss":
        //                        {
        //                            reason_dismiss.Save();
        //                            break;
        //                        }
        //                }
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
        //            tbName1.Focus();
        //        }
        //    }
        //}
    }
}
