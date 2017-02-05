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

namespace Tabel
{
    public partial class EditTime_Zone : Form
    {
        TIME_ZONE_seq time_zone;
        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_flagAdd">Флаг добавления данных</param>
        /// <param name="_time_zone">Таблица временных зон</param>
        /// <param name="_pos">Позиция в таблице временных зон</param>
        public EditTime_Zone(bool _flagAdd, TIME_ZONE_seq _time_zone, int _pos)
        {
            InitializeComponent();
            time_zone = _time_zone;
            /// Если добавление данных
            if (_flagAdd)
            {
                /// Добавляем новую запись
                time_zone.AddNew();
                /// Встаем на новую запись
                ((CurrencyManager)BindingContext[time_zone]).Position = ((CurrencyManager)BindingContext[time_zone]).Count;
            }
            else
            {
                /// Устанавливаем нужную позицию в таблице
                ((CurrencyManager)BindingContext[time_zone]).Position = _pos;
            }
            tbTime_Zone_Name.AddBindingSource(time_zone, TIME_ZONE_seq.ColumnsName.TIME_ZONE_NAME);
        }

        /// <summary>
        /// Сохраняем данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            /// Если не пусто, то сохраняем
            if (tbTime_Zone_Name.Text.Trim() != "")
            {
                time_zone.Save();
                Connect.Commit();
                Close();
            }
            else
            {
                MessageBox.Show("Вы не ввели наименование временной зоны!", "АРМ \"Учет рабочего времени\"",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        /// <summary>
        /// Закрываем форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
