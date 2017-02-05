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
    public partial class PostG_Study : Form
    {
        DataGridView dgViewPostG_Study;
        VisiblePanel pnVisible;

        // Объявление таблиц
        POSTG_STUDY_seq postg_study;

        // Объявление строки, табельного номера и флага добавления данных
        POSTG_STUDY_obj r_postg_study;
        string per_num;
        bool f_addpostg_study;

        // Создание формы
        public PostG_Study(string _per_num, bool _f_addpostg_study, int pos, POSTG_STUDY_seq _postg_study,
            INSTIT_seq _instit, TYPE_POSTG_STUDY_seq _type_postg_study, DataGridView _dgView, VisiblePanel _pnVisible)
        {
            InitializeComponent();
            dgViewPostG_Study = _dgView;
            pnVisible = _pnVisible;
            f_addpostg_study = _f_addpostg_study;
            postg_study = _postg_study;
            per_num = _per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addpostg_study)
            {
                r_postg_study = postg_study.AddNew();
                r_postg_study.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[postg_study]).Position = ((CurrencyManager)BindingContext[postg_study]).Count - 1;
            }
            else
            {
                BindingContext[postg_study].Position = pos;
                r_postg_study = (POSTG_STUDY_obj)((CurrencyManager)BindingContext[postg_study]).Current;
            }

            // Привязка компонентов
            cbInstit_ID.AddBindingSource(postg_study, INSTIT_seq.ColumnsName.INSTIT_ID, new LinkArgument(_instit, INSTIT_seq.ColumnsName.INSTIT_NAME));
            cbType_PostG_Study_ID.AddBindingSource(postg_study, TYPE_POSTG_STUDY_seq.ColumnsName.TYPE_POSTG_STUDY_ID, new LinkArgument(_type_postg_study, TYPE_POSTG_STUDY_seq.ColumnsName.TYPE_POSTG_STUDY_NAME));
            tbDoc_Num.AddBindingSource(postg_study, POSTG_STUDY_seq.ColumnsName.PGS_DOC_NUM);
            tbName_Doc.AddBindingSource(postg_study, POSTG_STUDY_seq.ColumnsName.PGS_DOC_NAME);
            //mbGive_Date.AddBindingSource(postg_study, POSTG_STUDY_seq.ColumnsName.GIVE_DATE);
            deGive_Date.AddBindingSource(postg_study, POSTG_STUDY_seq.ColumnsName.GIVE_DATE);
            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            postg_study.Save();
            Connect.Commit();
            f_addpostg_study = false;
            Library.VisiblePanel(dgViewPostG_Study, pnVisible);
            Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void PostG_Study_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addpostg_study)
            {
                postg_study.CancelNew(((CurrencyManager)BindingContext[postg_study]).Position);
            }
            else if (postg_study.IsDataChanged())
            {
                postg_study.RollBack();
            }
            dgViewPostG_Study.Invalidate();
        }

        
    }
}
