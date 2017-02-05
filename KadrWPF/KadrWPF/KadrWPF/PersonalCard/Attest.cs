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
    public partial class Attest : Form
    {
        DataGridView dgViewAttest;
        VisiblePanel pnVisible;

        // Объявление таблиц
        ATTEST_seq attest;

        // Объявление строки образование, табельного номера и флага добавления данных
        ATTEST_obj r_attest;
        string per_num;
        bool f_addattest;

        // Создание формы
        public Attest(string _per_num, bool _f_addattest, int pos, ATTEST_seq _attest,
            BASE_DOC_seq _base_doc, DataGridView _dgView, VisiblePanel _pnVisible, bool flagArchive)
        {
            InitializeComponent();
            dgViewAttest = _dgView;
            pnVisible = _pnVisible;
            f_addattest = _f_addattest;
            attest = _attest;
            per_num = _per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addattest)
            {
                r_attest = attest.AddNew();
                r_attest.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[attest]).Position = ((CurrencyManager)BindingContext[attest]).Count - 1;
            }
            else
            {
                BindingContext[attest].Position = pos;
                r_attest = (ATTEST_obj)((CurrencyManager)BindingContext[attest]).Current;
            }

            // Привязка компонентов
            cbBase_Doc_ID.AddBindingSource(attest, BASE_DOC_seq.ColumnsName.BASE_DOC_ID, new LinkArgument(_base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
            tbNum_Protocol.AddBindingSource(attest, ATTEST_seq.ColumnsName.NUM_PROTOCOL);
            tbThema.AddBindingSource(attest, ATTEST_seq.ColumnsName.THEMA);
            tbSolution.AddBindingSource(attest, ATTEST_seq.ColumnsName.SOLUTION);
            tbRecom.AddBindingSource(attest, ATTEST_seq.ColumnsName.RECOM);
            deDate_Protocol.AddBindingSource(attest, ATTEST_seq.ColumnsName.DATE_PROTOCOL);
            deDate_Attest.AddBindingSource(attest, ATTEST_seq.ColumnsName.DATE_ATTEST);
            deDate_Base_Doc.AddBindingSource(attest, ATTEST_seq.ColumnsName.DATE_BASE_DOC);
            tbNum_Base_Doc.AddBindingSource(attest, ATTEST_seq.ColumnsName.NUM_BASE_DOC);
            if (flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            attest.Save();
            Connect.Commit();
            f_addattest = false;
            Library.VisiblePanel(dgViewAttest, pnVisible);
            Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Attest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addattest)
            {
                attest.CancelNew(((CurrencyManager)BindingContext[attest]).Position);
            }
            else if (attest.IsDataChanged())
            {
                attest.RollBack();
            }
            dgViewAttest.Invalidate();
        }
    }
}
