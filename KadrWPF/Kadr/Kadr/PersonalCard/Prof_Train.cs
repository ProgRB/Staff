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
    public partial class Prof_Train : Form
    {
        DataGridView dgViewProf_Train;
        VisiblePanel pnVisible;

        // Объявление таблиц
        PROF_TRAIN_seq prof_train;

        // Объявление строки, табельного номера и флага добавления данных
        PROF_TRAIN_obj r_prof_train;
        string per_num;
        bool f_addprof_train;

        // Создание формы
        public Prof_Train(string _per_num, bool _f_addprof_train, int pos, PROF_TRAIN_seq _prof_train, 
             BASE_DOC_seq _base_doc, DataGridView _dgView, VisiblePanel _pnVisible)
        {
            InitializeComponent();
            dgViewProf_Train = _dgView;
            pnVisible = _pnVisible;
            f_addprof_train = _f_addprof_train;
            prof_train = _prof_train;
            per_num = _per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addprof_train)
            {
                r_prof_train = prof_train.AddNew();
                r_prof_train.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[prof_train]).Position = ((CurrencyManager)BindingContext[prof_train]).Count - 1;
            }
            else
            {
                BindingContext[prof_train].Position = pos;
                r_prof_train = (PROF_TRAIN_obj)((CurrencyManager)BindingContext[prof_train]).Current;
            }

            // Привязка компонентов
            //cbProf_ID.AddBindingSource(prof_train, PROF_seq.ColumnsName.PROF_ID, new LinkArgument(_prof, PROF_seq.ColumnsName.PROFESSION));
            cbBase_Doc_ID.AddBindingSource(prof_train, BASE_DOC_seq.ColumnsName.BASE_DOC_ID, new LinkArgument(_base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
            tbNum_Doc.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_NUM_DOC);
            tbName_Doc.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_NAME_DOC);
            //mbDate_Start.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_DATE_START);
            //mbDate_End.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_DATE_END);
            //mbDate_Doc.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_DATE_DOC);
            deDate_Start.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_DATE_START);
            deDate_End.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_DATE_END);
            deDate_Doc.AddBindingSource(prof_train, PROF_TRAIN_seq.ColumnsName.PF_DATE_DOC);
            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            prof_train.Save();
            Connect.Commit();
            f_addprof_train = false;
            Library.VisiblePanel(dgViewProf_Train, pnVisible);
            Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Prof_Train_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addprof_train)
            {
                prof_train.CancelNew(((CurrencyManager)BindingContext[prof_train]).Position);
            }
            else if (prof_train.IsDataChanged())
            {
                prof_train.RollBack();
            }
            dgViewProf_Train.Invalidate();
        }
    }
}
