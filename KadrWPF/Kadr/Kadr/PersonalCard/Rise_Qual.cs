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
    public partial class Rise_Qual : Form
    {
        DataGridView dgViewRise_Qual;
        VisiblePanel pnVisible;

        // Объявление таблиц
        RISE_QUAL_seq rise_qual;

        // Объявление строки, табельного номера и флага добавления данных
        RISE_QUAL_obj r_rise_qual;
        string per_num;
        bool f_addrise_qual;

        /// <summary>
        /// Создание формы
        /// </summary>
        /// <param name="_per_num"></param>
        /// <param name="_f_addrise_qual"></param>
        /// <param name="pos"></param>
        /// <param name="_rise_qual"></param>
        /// <param name="_base_doc"></param>
        /// <param name="_instit"></param>
        /// <param name="_type_rise_qual"></param>
        /// <param name="_dgView"></param>
        /// <param name="_pnVisible"></param>
        /// <param name="fl_rise_qual">Параметр определяет с чем мы хотим работать: 
        /// с повышением квалификации (fl_rise_qual=true) или с обязательным обучением (fl_rise_qual=false)</param>
        public Rise_Qual(string _per_num, bool _f_addrise_qual, int pos, RISE_QUAL_seq _rise_qual, 
            BASE_DOC_seq _base_doc, INSTIT_seq _instit, TYPE_RISE_QUAL_seq _type_rise_qual, DataGridView _dgView, 
            VisiblePanel _pnVisible, bool fl_rise_qual)
        {
            InitializeComponent();
            dgViewRise_Qual = _dgView;
            pnVisible = _pnVisible;
            f_addrise_qual = _f_addrise_qual;
            rise_qual = _rise_qual;
            per_num = _per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addrise_qual)
            {
                r_rise_qual = rise_qual.AddNew();
                r_rise_qual.PER_NUM = per_num;
                if (fl_rise_qual)
                    r_rise_qual.SIGN_RISE_QUAL = true;
                else
                    r_rise_qual.SIGN_RISE_QUAL = false;
                ((CurrencyManager)BindingContext[rise_qual]).Position = ((CurrencyManager)BindingContext[rise_qual]).Count - 1;
            }
            else
            {
                BindingContext[rise_qual].Position = pos;
                r_rise_qual = (RISE_QUAL_obj)((CurrencyManager)BindingContext[rise_qual]).Current;
            }

            // Привязка компонентов
            cbInstit_ID.AddBindingSource(rise_qual, INSTIT_seq.ColumnsName.INSTIT_ID, new LinkArgument(_instit, INSTIT_seq.ColumnsName.INSTIT_NAME));
            cbBase_Doc_ID.AddBindingSource(rise_qual, BASE_DOC_seq.ColumnsName.BASE_DOC_ID, new LinkArgument(_base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
            cbType_Rise_Qual_ID.AddBindingSource(rise_qual, TYPE_RISE_QUAL_seq.ColumnsName.TYPE_RISE_QUAL_ID, new LinkArgument(_type_rise_qual, TYPE_RISE_QUAL_seq.ColumnsName.TYPE_RISE_QUAL_NAME));
            tbNum_Doc.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_NUM_DOC);
            tbName_Doc.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_NAME_DOC);
            tbTheme.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_THEME);
            deDate_Start.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_DATE_START);
            deDate_End.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_DATE_END);
            deRQ_WORKING_OFF.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_WORKING_OFF);

            // В зависимости от типа вызывающей формы, работаем с Повышением квалификации или Обязательным образованием
            if (fl_rise_qual)
            {                
                deDate_Doc.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_DATE_DOC);
                // Доработка от 30.10.2015 - добавляю новые поля
                deRQ_DATE_BASE_DOC.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_DATE_BASE_DOC);
                tbRQ_NUM_BASE_DOC.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_NUM_BASE_DOC);
            }
            else
            {
                lbDate_end_rise_qual.Visible = true;
                deDate_end_rise_qual.Visible = true;
                lbType_rise_qual_id.Text = "Вид обучения";
                lbDate_begin_rise_qual.Text = "Дата выдачи документа";
                deDate_Doc.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_DATE_ISSUE_DOC);
                deDate_end_rise_qual.AddBindingSource(rise_qual, RISE_QUAL_seq.ColumnsName.RQ_DATE_EXPIRY);
                // Доработка от 30.10.2015 - добавляю новые поля
                deRQ_DATE_BASE_DOC.Visible = false;
                tbRQ_NUM_BASE_DOC.Visible = false;
                lbRQ_DATE_BASE_DOC.Visible = false;
                lbRQ_NUM_BASE_DOC.Visible = false;
                cbBase_Doc_ID.Width = cbInstit_ID.Width;
            }

            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            rise_qual.Save();
            Connect.Commit();
            f_addrise_qual = false;
            Library.VisiblePanel(dgViewRise_Qual, pnVisible);
            Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
              
        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Rise_Qual_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addrise_qual)
            {
                rise_qual.CancelNew(((CurrencyManager)BindingContext[rise_qual]).Position);
            }
            else if (rise_qual.IsDataChanged())
            {
                rise_qual.RollBack();
            }
            dgViewRise_Qual.Invalidate();
        }
    }
}
