using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace LibraryKadr
{
    public class Settings
    {
        public static void SetDataGridCoumnWidth(ref DataGridView  d)
        {
            try
            {
                StreamReader r = new StreamReader(Application.StartupPath + @"\Settings\ColumnWidth.txt", System.Text.Encoding.GetEncoding(1251));
                string s;
                while (!r.EndOfStream)
                {
                    s = r.ReadLine();
                    if (s.Substring(0, 2) != "//")
                    {
                        int i = s.IndexOf('=');
                        string name = s.Substring(0, i), Value = s.Substring(i + 1, s.Length - i - 1);
                        name.Trim(); Value.Trim();
                        if (d.Columns.Contains(name))
                            d.Columns[name].Width = Convert.ToInt32(Value);
                    }
                }
                r.Close();
            }
            catch
            {
                new ToolTip().Show("Настройки выравнивания столбцов не найдены или не корректны.\n Будут использованы настройки по умолчанию.", Application.OpenForms["FormMain"], 0, 0, 2000);
            }

        }
        public static void SetDataGridCaption(ref DataGridView d)
        {
            try
            {
                StreamReader r = new StreamReader(Application.StartupPath+@"\Settings\DataGridCaptions.txt", System.Text.Encoding.GetEncoding(1251));
                string s;
                while (!r.EndOfStream)
                {
                    s = r.ReadLine();
                    if (s.Substring(0, 2) != "//")
                    {
                        int i = s.IndexOf('=');
                        string name = s.Substring(0, i), Value = s.Substring(i + 1, s.Length - i - 1);

                        if (d.Columns.Contains(name))
                            d.Columns[name].HeaderText = Value;
                    }
                }
                r.Close();
            }
            catch
            {
                new ToolTip().Show("Настройки выравнивания столбцов не найдены или не корректны.\n Будут использованы настройки по умолчанию.", Application.OpenForms["FormMain"], 0, 0, 2000);
            }
        }
        
            public static void SetDataGridColumnAlign(ref DataGridView d)
        {
            try
            {
                StreamReader r = new StreamReader(Application.StartupPath + @"\Settings\TextAlignInColumn.txt", System.Text.Encoding.GetEncoding(1251));
                string s;
                while (!r.EndOfStream)
                {
                    s = r.ReadLine();
                    if (s.Substring(0, 2) != "//")
                    {
                        int i = s.IndexOf('=');
                        string name = s.Substring(0, i), Value = s.Substring(i + 1, s.Length - i - 1);

                        if (d.Columns.Contains(name))
                        {
                            switch (Value)
                            {
                                case "Right": d.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; break;
                                case "Center": d.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; break;
                                default: d.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; break;
                            }
                        }
                    }
                }
                r.Close();
            }
            catch
            {
                new ToolTip().Show("Настройки выравнивания столбцов не найдены или не корректны.\n Будут использованы настройки по умолчанию.",Application.OpenForms["FormMain"],0,0,2000);
            }
        }
    }
}