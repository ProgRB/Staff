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
    public partial class Know_Lang : Form
    {
        DataGridView dgViewKnow_Lang;
        VisiblePanel pnVisible;

        // Объявление таблиц
        KNOW_LANG_seq know_lang;

        // Объявление строки, табельного номера и флага добавления данных
        KNOW_LANG_obj r_know_lang;
        string per_num;
        bool f_addknow_lang;

        // Создание формы
        public Know_Lang(string _per_num, bool _f_addknow_lang, int pos, KNOW_LANG_seq _know_lang,
            LANG_seq _lang, LEVEL_KNOW_seq _level_know, DataGridView _dgView, VisiblePanel _pnVisible, bool flagArchive)
        {
            InitializeComponent();
            dgViewKnow_Lang = _dgView;
            pnVisible = _pnVisible;
            f_addknow_lang = _f_addknow_lang;
            know_lang = _know_lang;
            per_num = _per_num;
            // Проверка нужно ли добавлять новую запись
            if (f_addknow_lang)
            {
                r_know_lang = know_lang.AddNew();
                r_know_lang.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[know_lang]).Position = ((CurrencyManager)BindingContext[know_lang]).Count - 1;
            }
            else
            {
                BindingContext[know_lang].Position = pos;
                r_know_lang = (KNOW_LANG_obj)((CurrencyManager)BindingContext[know_lang]).Current;
            }
            
            // Привязка компонентов
            cbLang_ID.AddBindingSource(know_lang, LANG_seq.ColumnsName.LANG_ID, new LinkArgument(_lang, LANG_seq.ColumnsName.LANGUAGE));
            cbLevel_ID.AddBindingSource(know_lang, LEVEL_KNOW_seq.ColumnsName.LEVEL_ID, new LinkArgument(_level_know, LEVEL_KNOW_seq.ColumnsName.LEV));
            tbLEVEL_IN_EF.AddBindingSource(know_lang, KNOW_LANG_seq.ColumnsName.LEVEL_IN_EF);
            if (flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbLang_ID.Text == "")
            {
                MessageBox.Show("Вы не выбрали наименование языка!", "АРМ 'Кадры'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            know_lang.Save();
            Connect.Commit();
            f_addknow_lang = false;
            Library.VisiblePanel(dgViewKnow_Lang, pnVisible);
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Know_Lang_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addknow_lang)
            {
                know_lang.CancelNew(((CurrencyManager)BindingContext[know_lang]).Position);
            }
            else if (know_lang.IsDataChanged())
            {
                know_lang.RollBack();
            }
            dgViewKnow_Lang.Invalidate();
        }
    }
}
