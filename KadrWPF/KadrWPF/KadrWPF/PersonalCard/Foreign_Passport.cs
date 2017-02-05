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
    public partial class Foreign_Passport : Form
    {
        DataGridView dgViewFP;

        // Объявление таблиц
        FOREIGN_PASSPORT_seq foreign_passport;

        // Объявление строки образование, табельного номера и флага добавления данных
        FOREIGN_PASSPORT_obj r_fp;
        string per_num;
        bool f_addFP;

        // Создание формы
        public Foreign_Passport(string _per_num, bool _f_addFP, int pos,
            FOREIGN_PASSPORT_seq _fp, DataGridView _dgView, bool flagArchive)
        {
            InitializeComponent();
            dgViewFP = _dgView;
            f_addFP = _f_addFP;
            foreign_passport = _fp;
            per_num = _per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addFP)
            {
                r_fp = foreign_passport.AddNew();
                r_fp.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[foreign_passport]).Position = ((CurrencyManager)BindingContext[foreign_passport]).Count - 1;
            }
            else
            {
                BindingContext[foreign_passport].Position = pos;
                r_fp = (FOREIGN_PASSPORT_obj)((CurrencyManager)BindingContext[foreign_passport]).Current;
            }

            // Привязка компонентов
            tbSeria_FP.AddBindingSource(foreign_passport, FOREIGN_PASSPORT_seq.ColumnsName.SERIA_FP);
            tbNum_FP.AddBindingSource(foreign_passport, FOREIGN_PASSPORT_seq.ColumnsName.NUMBER_FP);
            tbWho_Given_FP.AddBindingSource(foreign_passport, FOREIGN_PASSPORT_seq.ColumnsName.WHO_GIVEN_FP);
            deWhen_Given_FP.AddBindingSource(foreign_passport, FOREIGN_PASSPORT_seq.ColumnsName.WHEN_GIVEN_FP);
            dePeriod_FP.AddBindingSource(foreign_passport, FOREIGN_PASSPORT_seq.ColumnsName.PERIOD_OF_VALIDITY_FP);
            deLease_FP.AddBindingSource(foreign_passport, FOREIGN_PASSPORT_seq.ColumnsName.DATE_LEASE_FP);
            if (flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            foreign_passport.Save();
            Connect.Commit();
            f_addFP = false;
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
            if (f_addFP)
            {
                foreign_passport.CancelNew(((CurrencyManager)BindingContext[foreign_passport]).Position);
            }
            else if (foreign_passport.IsDataChanged())
            {
                foreign_passport.RollBack();
            }
            dgViewFP.Invalidate();
        }
    }
}
