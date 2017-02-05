using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using System.Reflection;
using System.Collections;
using System.Globalization;
using EditorsLibrary;
using Oracle.DataAccess.Client;
using System.Collections.ObjectModel;

namespace LibraryKadr
{
    public class Library
    {
        // Создание визуальной панели
        public static void VisiblePanel(DataGridView _dgView, VisiblePanel _pnVisible)
        {
            if (_dgView.RowCount == 0)
            {
                _pnVisible.Left = _dgView.Left;
                _pnVisible.Top = _dgView.Top + _dgView.ColumnHeadersHeight - 1;                
                _pnVisible.Height = _dgView.Height - _dgView.ColumnHeadersHeight - 17;
                _pnVisible.Width = _dgView.Width;
                int widthcolumns = 0;
                for (int i = 0; i < _dgView.ColumnCount; i++)
                    if (_dgView.Columns[i].Visible)
                        widthcolumns += _dgView.Columns[i].Width;
                if (_pnVisible.Width > widthcolumns)
                    {
                        _pnVisible.Width = widthcolumns + 22;
                        _pnVisible.Height += 17;
                    }
                _pnVisible.Visible = true;
            }
            else
                _pnVisible.Visible = false;
        }

        // Смена формата отображения даты
        public static void FormatDateTimePicker(DateTimePicker _dtPicker)
        {
            _dtPicker.CustomFormat = "dd.MM.yyyy";
        }

        /// <summary>
        /// функция замены пустого пустого объекта на строку
        /// </summary>
        /// <param name="value">Объект</param>
        /// <param name="replace">Строка для замены</param>
        /// <returns></returns>
        public static string NVL(object value, string replace)
        {
            if (value == null) return replace;
            else
                return (value.ToString() == "" ? replace : value.ToString());
        }

        public static string TypeVac(string PayType)
        {
            string[] otp= new string[]{"226","227","501","502","532","531"};
            string[] bol= new string[]{"236","237","238"};
            if (otp.Contains(PayType))
                return "отпуска";
            else
                if (bol.Contains(PayType))
                    return "болезни";
                else return "отсутствия";
        }
        
        // Активация вкладок, при которой добавляется визуальная панель
        public static void ActivateTabPage(TabControl _tabControl, TabPage _tabPage, DataGridView _dgView, VisiblePanel _pnVisible, ref int _page)
        {
            string dgname = "";
            foreach (Control contr in _tabControl.TabPages[_page].Controls)
                if (contr is DataGridView)
                {
                    dgname = ((DataGridView)contr).Name;
                    break;
                }
            if (dgname != "")
            {
                _tabControl.TabPages[_page].Controls[dgname].Controls.Remove(_pnVisible);
            }
            _page = _tabControl.TabPages.IndexOf(_tabPage);
            _dgView.Controls.Add(_pnVisible);
            Library.VisiblePanel(_dgView, _pnVisible);            
        }

        // Получение фокуса ричтекстом и его развертывание
        public static void EnterRichTextBox(RichTextBox _rt)
        {
            _rt.Height = 63;
            _rt.ScrollBars = RichTextBoxScrollBars.None;
        }

        // Потеря фокуса ричтекстом и его свертывание
        public static void LeaveRichTextBox(RichTextBox _rt)
        {
            _rt.Height = 21;
            _rt.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
        }

        public static string MyMonthName(DateTime _date)
        {
            int mon = _date.Month;
            return Month(mon);
        }

        public static string MyMonthName(int _mon)
        {
            return Month(_mon);
        }

        private static string Month(int _mon)
        {
            string month;
            switch (_mon)
            {
                case 1:
                    month = "января";
                    break;
                case 2:
                    month = "февраля";
                    break;
                case 3:
                    month = "марта";
                    break;
                case 4:
                    month = "апреля";
                    break;
                case 5:
                    month = "мая";
                    break;
                case 6:
                    month = "июня";
                    break;
                case 7:
                    month = "июля";
                    break;
                case 8:
                    month = "августа";
                    break;
                case 9:
                    month = "сентября";
                    break;
                case 10:
                    month = "октября";
                    break;
                case 11:
                    month = "ноября";
                    break;
                case 12:
                    month = "декабря";
                    break;
                default:
                    month = "";
                    break;
            }
            return month;
        }

        public static char[] StringToChar(string _st)
        {
            char[] newchar = new char[_st.Length];
            for (int i = 0; i < _st.Length; i++)
                newchar[i] = _st[i];
            return newchar;
        }

        /// <summary>
        /// Метод удаляет секванс из базы данных
        /// </summary>
        /// <param name="_schema">Имя схемы</param>
        /// <param name="_name_seq">Имя секванса</param>
        /// <param name="_connection">Строка подключения</param>
        public static void DropSequance(string _schema, string _name_seq, OracleConnection _connection)
        {
            /// Вызываем процедуру для удаления секванса
            string textBlock = string.Format("{0}.dropSequence('{1}')", _schema, _name_seq);
            OracleCommand command = new OracleCommand(textBlock, _connection);
            command.BindByName = true;
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Метод создает секванс в базе данных
        /// </summary>
        /// <param name="_schema">Имя схемы</param>
        /// <param name="_name_seq">Имя секванса</param>
        /// <param name="_number">Начальное значение секванса</param>
        /// <param name="_connection">Строка подключения</param>
        public static void CreateSequance(string _schema, string _name_seq, int _number, OracleConnection _connection)
        {
            /// Вызываем процедуру для создания секванса
            string textBlock = string.Format("{0}.createSequence('{1}', {2})", _schema, _name_seq, _number);
            OracleCommand command = new OracleCommand(textBlock, _connection);
            command.BindByName = true;
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Функция возвращает значение секванса
        /// </summary>
        /// <param name="_schema">Имя схемы</param>
        /// <param name="_name_seq">Имя секванса</param>
        /// <param name="_command">Команда, которую необходимо вызвать для секванса</param>
        /// <param name="_connection">Строка подключения</param>
        /// <returns></returns>
        public static int NumberDoc(string _schema, string _name_seq, string _command, OracleConnection _connection)
        {
            /// Создаем команду, которая проверяет существует ли такой секванс в базе данных.
            OracleCommand sql_seq = new OracleCommand(string.Format("select count(*) as val from all_objects where object_type = upper('sequence') and upper(object_name) = upper('{0}')", _name_seq), _connection);
            OracleDataReader reader = sql_seq.ExecuteReader();
            reader.Read();
            /// Если секванс не найден, то создаем его начиная с 1.
            if (Convert.ToInt32(reader["val"]) == 0)
                CreateSequance(_schema, _name_seq, 1, _connection);
            /// Вызываем нужный метод секванса
            OracleCommand sql = new OracleCommand(string.Format("select {0}.{1}.{2} as value from dual", _schema, _name_seq, _command), _connection);
            reader = sql.ExecuteReader();
            reader.Read();
            return Convert.ToInt32(reader["value"]);
        }

        public static bool TestingInput(MaskedTextBox _ms1, MaskedTextBox _ms2)
        {
            if (_ms1.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату начала периода!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _ms1.Focus();
                return false;
            }
            if (_ms2.Text.Replace(".", "").Trim() == "")
            {
                MessageBox.Show("Вы не ввели дату окончания периода!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _ms2.Focus();
                return false;
            }
            if (Convert.ToDateTime(_ms1.Text) >= Convert.ToDateTime(_ms2.Text))
            {
                MessageBox.Show("Дата начала периода должна быть меньше даты окончания периода!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _ms2.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод выполняет поиск шифра
        /// </summary>
        /// <param name="_tbText">Компонент, содержащий шифр</param>
        /// <param name="_cbCombo">Компонент, позицию которого нужно изменить</param>
        /// <param name="_length">Длина шифра</param>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="e">Параметр определяет отменять событие ввода или нет</param>
        /// <param name="_params">Массив строк для построения запроса</param>
        public static void ValidTextBox(TextBox _tbText, ComboBox _cbCombo,
            int _length, OracleConnection _connection, CancelEventArgs e, params string[] _params)
        {
            if (_tbText.Text.Trim() != "")
            {
                ValidatingInt(_tbText, e);
                if (!e.Cancel)
                {
                    _tbText.Text = _tbText.Text.Trim().PadLeft(_length, '0');
                    _params[4] = _tbText.Text;
                    ValueByCode(_cbCombo, _tbText, _connection, _params);
                }
            }
            else
            {
                _cbCombo.SelectedItem = null;
            }
        }

        /// <summary>
        /// Проверка на ввод целого числа
        /// </summary>
        /// <param name="tbText">Компонент, текст которого нужно проверить</param>
        /// <param name="e">Параметр определяет отменять событие ввода или нет</param>
        public static void ValidatingInt(object tbText, CancelEventArgs e)
        {
            try
            {
                if (((TextBox)tbText).Text.Trim() != "")
                {
                    Convert.ToInt32(((TextBox)tbText).Text);
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show("Введите число!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Проверка нажатия клавиши и замены на ','  а
        /// так же количества знаков после запятой. Только на маскедтекстбокс
        /// </summary>
        public static void ValidatingNumeric(object _tbObject, KeyPressEventArgs e)
        {

            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 44 && e.KeyChar != 46 && e.KeyChar != 8)
                e.KeyChar = Convert.ToChar(0);
            else
            {
                if (e.KeyChar == 46)
                    e.KeyChar = Convert.ToChar(44);
                if (e.KeyChar == 44)
                    if (((MaskedTextBox)_tbObject).Text.IndexOf(e.KeyChar) > -1 || ((MaskedTextBox)_tbObject).Text.Length == 0)
                        e.KeyChar = Convert.ToChar(0);
            }
 
        }
        /// <summary>
        /// Проверка ввода даты
        /// </summary>
        /// <param name="tbText">Компонент, текст которого нужно проверить</param>
        /// <param name="e">Параметр определяет отменять событие ввода или нет</param>
        public static void ValidatingDate(object mbDate, CancelEventArgs e)
        {
            try
            {
                if (((MaskedTextBox)mbDate).Text.Replace(".", "").Trim() != "" && ((MaskedTextBox)mbDate).Text.Replace("-", "").Trim() != "")
                {
                    if (Convert.ToDateTime(((MaskedTextBox)mbDate).Text) < new DateTime(1900, 1, 1) ||
                        Convert.ToDateTime(((MaskedTextBox)mbDate).Text) > new DateTime(3000, 1, 1))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Вы ввели неверную дату!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show("Вы ввели неверную дату!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Метод устанавливает комбобокс в определенную позицию
        /// </summary>
        /// <param name="_comboBox">Комбобокс, позицию которого нужно изменить</param>
        /// <param name="_textBox">Текстбокс, содержащий текст шифра</param>
        /// <param name="_connection">Строка соединения</param>
        /// <param name="_params">Массив строк, определяющий параметры запроса</param>
        public static void ValueByCode(System.Windows.Forms.ComboBox _comboBox, System.Windows.Forms.TextBox _textBox,
            OracleConnection _connection, params string[] _params)
        {
            decimal? pos = ValueBySelectedCode(_connection, _params);
            if (pos != null)
            {
                _comboBox.SelectedValue = pos;
            }
            else
            {
                MessageBox.Show("Введенный шифр не найден!", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _textBox.Focus();
            }
        }

        /// <summary>
        /// Функция возвращает значение идентификатора строки базы данных по введенному шифру
        /// </summary>
        /// <param name="_connection">Строка соединения</param>
        /// <param name="_params">Массив строк, определяющий параметры запроса</param>
        /// <returns>Идентификатор строки базы данных или null</returns>
        public static decimal? ValueBySelectedCode(OracleConnection _connection, params string[] _params)
        {
            string sql = string.Format(Queries.GetQuery("SelectCode.sql"), _params[0], _params[1], 
                _params[2], _params[3],
                _params[4]);
            OracleDataTable oracleTable = new OracleDataTable(sql, _connection);            
            oracleTable.SelectCommand.Parameters.Add("p_id", OracleDbType.Varchar2).Value = _params[4];
            oracleTable.Fill();
            if (oracleTable.Rows.Count != 0)
            {
                return oracleTable.Rows[0].Field<decimal?>(_params[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Метод возвращает код поля по выбранному значению в комбобоксе
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_params">Массив строк, определяющий параметры запроса</param>
        /// <returns>Код выбранного значения в комбобоксе</returns>
        public static string CodeBySelectedValue(OracleConnection _connection, params string[] _params)
        {
            string sql = string.Format(Queries.GetQuery("SelectCode.sql"), _params[0], _params[1], 
                _params[2], _params[3]);
            OracleDataTable oracleTable = new OracleDataTable(sql, _connection);
            oracleTable.SelectCommand.Parameters.Add("p_id", OracleDbType.Decimal).Value = _params[4];
            oracleTable.Fill();
            return oracleTable.Rows[0].Field<string>(_params[0]).ToString();
        }

        /// <summary>
        /// Метод осуществляет расчет стажа
        /// </summary>
        /// <param name="start">Дата начала периода работы</param>
        /// <param name="end">Дата окончания периода работы</param>
        /// <param name="yearCalc">Количество лет стажа</param>
        /// <param name="monthCalc">Количество месяцев стажа</param>
        /// <param name="dayCalc">Количество дней стажа</param>
        public static void CalculationWork_Length(DateTime start, DateTime end, ref int yearCalc, ref int monthCalc, ref int dayCalc)
        {
            /*/// Если дата начала деятельности больше даты окончания, то выходим из расчета
            if (start > end)
            {
                return;
            }
            /// Считаем разницу в днях
            int day = end.Day - start.Day;
            /// Если она меньше 0, то значит между днями нет полного месяца 
            /// (например разница между датой начала 10.06.2008 и датой окончания 05.10.2008)
            if (day < 0)
            {
                /// Вычитаем лишний месяц
                monthCalc -= 1;
                /// Считаем количество дней
                dayCalc += end.Day + 30 - start.Day + 1;
            }
            else
            {
                /// Прибавляем дни
                dayCalc += day + 1;
            }
            /// Если количество дней больше 29, то есть есть лишний месяц, то вычисляем
            if (dayCalc > 29)
            {
                /// Прибавляем к месяцам те, которые хранятся в днях
                monthCalc += dayCalc / 30;
                /// Считаем количество дней, откидывая лишние месяцы
                dayCalc %= 30;
            }
            /// Считаем разницу в месяцах
            int month = end.Month - start.Month;
            /// Проверяем условие
            /// (например разница между датой начала 10.10.2008 и датой окончания 05.03.2009)
            if (month < 1)
            {
                /// Если оно верно, то вычитаем из годов лишний
                yearCalc -= 1;
                /// Считаем количество месяцев
                monthCalc += end.Month + 12 - start.Month;
            }
            else
            {
                /// Прибавляем месяца
                monthCalc += month;
            }
            /// Если количество месяцев больше 11
            if (monthCalc > 11)
            {
                /// Прибавляем к годам те, которые хранятся в месяцах
                yearCalc += monthCalc / 12;
                /// Считаем количество месяцев, откидывая лишние года
                monthCalc %= 12;
            }
            /// Считаем разницу в годах
            int years = end.Year - start.Year;
            /// Прибавляем к годам разницу лет в датах
            yearCalc += years;*/
        }

        /// <summary>
        /// Метод осуществляет расчет стажа (2 вариант)
        /// </summary>
        /// <param name="start">Дата начала периода работы</param>
        /// <param name="end">Дата окончания периода работы</param>
        /// <param name="yearCalc">Количество лет стажа</param>
        /// <param name="monthCalc">Количество месяцев стажа</param>
        /// <param name="dayCalc">Количество дней стажа</param>
        public static void CalcStanding(DateTime start, DateTime end, ref decimal yearCalc, ref decimal monthCalc, ref decimal dayCalc)
        {
            TimeSpan ts = end - start;
            decimal stTemp = ts.Days + 1;
            stTemp = stTemp / 365;
            yearCalc = Math.Truncate(stTemp);
            stTemp = (stTemp - yearCalc) * 12;
            monthCalc = Math.Truncate(stTemp);
            dayCalc = Math.Truncate((stTemp - monthCalc) * 30.42M);
        }

        /// <summary>
        /// Проверка ввода символа. Если введена точка или запятая, возвращаем разделитель 
        /// в зависимости от языковых настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void InputSeparator(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 46 || Convert.ToInt32(e.KeyChar) == 44)
            {
                e.KeyChar = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            }
        }

        /// <summary>
        /// Метод выделяет выбранную строку в гриде и окрашивает ее в определенный цвет
        /// </summary>
        /// <param name="sender">Грид</param>
        /// <param name="e">Строка</param>
        public static void DataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
            ((DataGridView)sender).RowsDefaultCellStyle.SelectionBackColor = Color.LightBlue;
            ((DataGridView)sender).RowsDefaultCellStyle.SelectionForeColor = Color.Black;
        }
        
	    public static void DataGridView_CellsCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.CellValue1 == System.DBNull.Value ^ e.CellValue2 == System.DBNull.Value)
            {
                e.SortResult = (e.CellValue1 == DBNull.Value ? -1 : 1);
                e.Handled = true;
            }
        }

        public static void UpdateGR_Work(string _per_num, int _transfer_id, 
            int _gr_work_id, DateTime _date_transfer, int _num_day)
        {
            OracleCommand com = new OracleCommand("", Connect.CurConnect);
            com.CommandText = string.Format(Queries.GetQuery("Table/UpdateGr_Work.sql"), Connect.Schema);
            com.BindByName = true;
            com.Parameters.Add("PER_NUM", OracleDbType.Varchar2).Value =_per_num;
            com.Parameters.Add("TRANSFER_ID", OracleDbType.Decimal).Value = _transfer_id;
            com.Parameters.Add("GR_WORK_ID", OracleDbType.Decimal).Value = _gr_work_id;
            com.Parameters.Add("GR_WORK_DATE_BEGIN", OracleDbType.Date).Value = _date_transfer;
            com.Parameters.Add("GR_WORK_DAY_NUM", OracleDbType.Decimal).Value = _num_day;            
            com.ExecuteNonQuery();
            Connect.Commit();        
        }

        public static string GetMessageException(Exception ex)
        {
            if (ex is OracleException && (ex as OracleException).Number>19999 && (ex as OracleException).Number<25000)
            {
                OracleException e = (ex as OracleException);
                return e.Message.Substring(0, e.Message.IndexOf("ORA-", 10, StringComparison.CurrentCultureIgnoreCase)); 
            }
            else return ex.Message;
        }

        /// <summary>
        /// Изменение размера изображения с сохранением пропорций
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image ScaleImage(Image source, int width, int height)
        {
            Image dest = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(dest))
            {
                gr.FillRectangle(Brushes.White, 0, 0, width, height);  // Очищаем экран
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                float srcwidth = source.Width;
                float srcheight = source.Height;
                float dstwidth = width;
                float dstheight = height;

                if (srcwidth <= dstwidth && srcheight <= dstheight)  // Исходное изображение меньше целевого
                {
                    int left = (width - source.Width) / 2;
                    int top = (height - source.Height) / 2;
                    gr.DrawImage(source, left, top, source.Width, source.Height);
                }
                else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
                {
                    float cy = srcheight / srcwidth * dstwidth;
                    float top = ((float)dstheight - cy) / 2.0f;
                    if (top < 1.0f) top = 0;
                    gr.DrawImage(source, 0, top, dstwidth, cy);
                }
                else  // Пропорции исходного изображения более узкие
                {
                    float cx = srcwidth / srcheight * dstheight;
                    float left = ((float)dstwidth - cx) / 2.0f;
                    if (left < 1.0f) left = 0;
                    gr.DrawImage(source, left, 0, cx, dstheight);
                }

                return dest;
            }
        }

        public static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Ошибка сохранения файла: \n{0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }
    }

    public class BindingMTB
    {
        private MaskedTextBox _mbDate;
        private object _columnData;
        public BindingMTB(MaskedTextBox mbDate, object columnData)
        {
            _mbDate = mbDate;
            _columnData = columnData;
        }
        public MaskedTextBox MBDate
        {
            get { return _mbDate; }
            set { _mbDate = value; }
        }
        public object ColumnData
        {
            get { return _columnData; }
            set { _columnData = value; }
        }
    }
    public class TableMove
    {
        private string _place;
        private string _inputPlace;
        public TableMove(string place, string inputPlace)
        {
            _place = place;
            _inputPlace = inputPlace;
        }
        public string Place
        {
            get { return _place; }
            set { _place = value; }
        }
        public string InputPlace
        {
            get { return _inputPlace; }
            set { _inputPlace = value; }
        }

        public static List<TableMove> ComboMove()
        {
            List<TableMove> _tableMove = new List<TableMove>();
            _tableMove.Add(new TableMove("cbRegion", "cbDistrict"));
            _tableMove.Add(new TableMove("cbRegion", "cbCity"));
            _tableMove.Add(new TableMove("cbRegion", "cbRegion"));
            _tableMove.Add(new TableMove("cbDistrict", "cbRegion"));
            _tableMove.Add(new TableMove("cbDistrict", "cbDistrict"));
            _tableMove.Add(new TableMove("cbDistrict", "cbLocality"));
            _tableMove.Add(new TableMove("cbDistrict", "cbCity"));
            _tableMove.Add(new TableMove("cbCity", "cbRegion"));
            _tableMove.Add(new TableMove("cbCity", "cbCity"));
            _tableMove.Add(new TableMove("cbCity", "cbDistrict"));
            _tableMove.Add(new TableMove("cbCity", "cbLocality"));
            _tableMove.Add(new TableMove("cbCity", "cbStreet"));
            _tableMove.Add(new TableMove("cbLocality", "cbRegion"));
            _tableMove.Add(new TableMove("cbLocality", "cbDistrict"));
            _tableMove.Add(new TableMove("cbLocality", "cbLocality"));
            _tableMove.Add(new TableMove("cbLocality", "cbCity"));
            _tableMove.Add(new TableMove("cbLocality", "cbStreet"));
            return _tableMove;
        }

        public static List<TableMove> HabitTableMove()
        {
            List<TableMove> _tableMove = new List<TableMove>();
            _tableMove.Add(new TableMove("cbHRegion", "cbHDistrict"));
            _tableMove.Add(new TableMove("cbHRegion", "cbHCity"));
            _tableMove.Add(new TableMove("cbHRegion", "cbHRegion"));
            _tableMove.Add(new TableMove("cbHDistrict", "cbHRegion"));
            _tableMove.Add(new TableMove("cbHDistrict", "cbHDistrict"));
            _tableMove.Add(new TableMove("cbHDistrict", "cbHLocality"));
            _tableMove.Add(new TableMove("cbHDistrict", "cbHCity"));
            _tableMove.Add(new TableMove("cbHCity", "cbHRegion"));
            _tableMove.Add(new TableMove("cbHCity", "cbHCity"));
            _tableMove.Add(new TableMove("cbHCity", "cbHDistrict"));
            _tableMove.Add(new TableMove("cbHCity", "cbHLocality"));
            _tableMove.Add(new TableMove("cbHCity", "cbHStreet"));
            _tableMove.Add(new TableMove("cbHLocality", "cbHRegion"));
            _tableMove.Add(new TableMove("cbHLocality", "cbHDistrict"));
            _tableMove.Add(new TableMove("cbHLocality", "cbHLocality"));
            _tableMove.Add(new TableMove("cbHLocality", "cbHCity"));
            _tableMove.Add(new TableMove("cbHLocality", "cbHStreet"));
            return _tableMove;
        }

        public static List<TableMove> RegistrTableMove()
        {
            List<TableMove> _tableMove = new List<TableMove>();
            _tableMove.Add(new TableMove("cbRRegion", "cbRDistrict"));
            _tableMove.Add(new TableMove("cbRRegion", "cbRCity"));
            _tableMove.Add(new TableMove("cbRRegion", "cbRRegion"));
            _tableMove.Add(new TableMove("cbRDistrict", "cbRRegion"));
            _tableMove.Add(new TableMove("cbRDistrict", "cbRDistrict"));
            _tableMove.Add(new TableMove("cbRDistrict", "cbRLocality"));
            _tableMove.Add(new TableMove("cbRDistrict", "cbRCity"));
            _tableMove.Add(new TableMove("cbRCity", "cbRRegion"));
            _tableMove.Add(new TableMove("cbRCity", "cbRCity"));
            _tableMove.Add(new TableMove("cbRCity", "cbRDistrict"));
            _tableMove.Add(new TableMove("cbRCity", "cbRLocality"));
            _tableMove.Add(new TableMove("cbRCity", "cbRStreet"));
            _tableMove.Add(new TableMove("cbRLocality", "cbRRegion"));
            _tableMove.Add(new TableMove("cbRLocality", "cbRDistrict"));
            _tableMove.Add(new TableMove("cbRLocality", "cbRLocality"));
            _tableMove.Add(new TableMove("cbRLocality", "cbRCity"));
            _tableMove.Add(new TableMove("cbRLocality", "cbRStreet"));
            return _tableMove;
        }

        public static List<TableMove> ButtonMove()
        {
            List<TableMove> _tableMove = new List<TableMove>();
            _tableMove.Add(new TableMove("btRegion", "cbRegion"));
            _tableMove.Add(new TableMove("btDistrict", "cbRegion"));
            _tableMove.Add(new TableMove("btDistrict", "cbDistrict"));
            _tableMove.Add(new TableMove("btDistrict", "cbCity"));
            _tableMove.Add(new TableMove("btCity", "cbRegion"));
            _tableMove.Add(new TableMove("btCity", "cbDistrict"));
            _tableMove.Add(new TableMove("btCity", "cbCity"));
            _tableMove.Add(new TableMove("btLocality", "cbRegion"));
            _tableMove.Add(new TableMove("btLocality", "cbDistrict"));
            _tableMove.Add(new TableMove("btLocality", "cbCity"));
            _tableMove.Add(new TableMove("btLocality", "cbLocality"));
            _tableMove.Add(new TableMove("btStreet", "cbRegion"));
            _tableMove.Add(new TableMove("btStreet", "cbDistrict"));
            _tableMove.Add(new TableMove("btStreet", "cbCity"));
            _tableMove.Add(new TableMove("btStreet", "cbLocality"));
            _tableMove.Add(new TableMove("btStreet", "cbStreet"));
            return _tableMove;
        }

        public static List<TableMove> HabitButtonMove()
        {
            List<TableMove> _tableMove = new List<TableMove>();
            _tableMove.Add(new TableMove("btHRegion", "cbHRegion"));
            _tableMove.Add(new TableMove("btHDistrict", "cbHRegion"));
            _tableMove.Add(new TableMove("btHDistrict", "cbHDistrict"));
            _tableMove.Add(new TableMove("btHDistrict", "cbHCity"));
            _tableMove.Add(new TableMove("btHCity", "cbHRegion"));
            _tableMove.Add(new TableMove("btHCity", "cbHDistrict"));
            _tableMove.Add(new TableMove("btHCity", "cbHCity"));
            _tableMove.Add(new TableMove("btHLocality", "cbHRegion"));
            _tableMove.Add(new TableMove("btHLocality", "cbHDistrict"));
            _tableMove.Add(new TableMove("btHLocality", "cbHCity"));
            _tableMove.Add(new TableMove("btHLocality", "cbHLocality"));
            _tableMove.Add(new TableMove("btHStreet", "cbHRegion"));
            _tableMove.Add(new TableMove("btHStreet", "cbHDistrict"));
            _tableMove.Add(new TableMove("btHStreet", "cbHCity"));
            _tableMove.Add(new TableMove("btHStreet", "cbHLocality"));
            _tableMove.Add(new TableMove("btHStreet", "cbHStreet"));
            return _tableMove;
        }
    }

    public static class DisableControl
    {
        public static void DisableAll(this Control control, bool flag, Color color)
        {
            Disable(control, flag, color);
        }

        public static void Disable(Control control, bool flag, Color color)
        {
            if (control != null)
            {
                if (control.Controls.Count != 0)
                {
                    foreach (Control component in control.Controls)
                    {
                        if (component is TextBox)
                        {
                            ((TextBox)component).Enabled = flag;
                            ((TextBox)component).BackColor = color;
                        }
                        else if (component is MaskedTextBox)
                        {
                            ((MaskedTextBox)component).Enabled = flag;
                            ((MaskedTextBox)component).BackColor = color;
                        }
                        else if (component is DateEditor)
                        {
                            ((DateEditor)component).Enabled = flag;
                            ((MaskedTextBox)component.Controls[0]).Enabled = flag;
                            ((MaskedTextBox)component.Controls[0]).BackColor = color;
                        }
                        else if (component is Button)
                        {
                            ((Button)component).Enabled = flag;
                        }
                        else if (component is CheckBox)
                        {
                            ((CheckBox)component).Enabled = flag;
                        }
                        else if (component is RichTextBox)
                        {
                            ((RichTextBox)component).Enabled = flag;
                            ((RichTextBox)component).BackColor = color;
                        }
                        else if (component is RadioButton)
                        {
                            ((RadioButton)component).Enabled = flag;
                        }
                        else if (component is ComboBox)
                        {
                            if (flag)
                                ((ComboBox)component).DropDownStyle = ComboBoxStyle.DropDown;
                            else
                                ((ComboBox)component).DropDownStyle = ComboBoxStyle.DropDownList;
                            ((ComboBox)component).Enabled = flag;
                            ((ComboBox)component).BackColor = color;
                        }
                        else
                        {
                            Disable(component, flag, color);
                        }
                    }
                }
            }
        }
    }

    public class KadrFunction
    {
        //public static string Pos<TObj,TSeq>(TSeq seq, System.Windows.Forms.ComboBox _comboBox, string nameOfFill)
        //    where TObj : Staff.DataObject
        //    where TSeq : DataSequence<TObj>
        //{            
        //    Type typeObj = typeof(TObj);
        //    Type typeSeq = typeof(TSeq);
        //    PropertyInfo primaryProperty = typeObj.GetProperties().Where(s => ((ColumnAttribute)s.GetCustomAttributes(false).Where(atr => atr is ColumnAttribute).FirstOrDefault()).Primary).FirstOrDefault();
        //    StringBuilder rezult = new StringBuilder();
        //    foreach(TObj obj in seq)
        //    {
        //        if(primaryProperty.GetValue(obj,null).ToString().Trim().ToUpper() == _comboBox.SelectedValue.ToString().ToUpper().Trim())
        //        {
        //            rezult.Append(typeObj.InvokeMember(nameOfFill, BindingFlags.GetProperty| BindingFlags.Default,null,obj,null).ToString());
        //            break;
        //        }
        //    }
        //    return rezult.ToString();
        //}
    }

	public class Pair<T, K>
    {
		public Pair(T a, K b)
        {
            first = a;
            second = b;
        }
        public T first { get; set; }
        public K second { get; set; }
    }

    public class GrantedRoles
    {
        private static Dictionary<string, bool> _grantedRoles;
        static GrantedRoles()
        {
            _grantedRoles = new Dictionary<string, bool>();
            if (Connect.CurConnect != null)
            {
                OracleDataReader drGrantedRoles = new OracleCommand(string.Format(
                    @"select GRANTED_ROLE from user_role_privs union 
                      select  GRANTED_ROLE from role_role_privs"), Connect.CurConnect).ExecuteReader();
                while (drGrantedRoles.Read())
                {
                    _grantedRoles.Add(drGrantedRoles["GRANTED_ROLE"].ToString(), true);
                }
            }
        }
        public static bool GetGrantedRole(string RoleName)
        {
            return _grantedRoles.ContainsKey(RoleName.ToUpper());
        }
        public static int CountGrantedRoles
        {
            get { return _grantedRoles.Count(); }
        }
    }
}
public class OracleDataTable : DataTable
{
    OracleCommand _selectCommand;
    bool _bindbyname = true;
    public bool BindByName
    {
        get
        {
            return _bindbyname;
        }
        set
        {
            _bindbyname = value;
        }
    }
    public OracleDataTable()
        : base()
    {
        _selectCommand.BindByName = _bindbyname;
    }
    public OracleDataTable(string command_text, OracleConnection connt)
        : base()
    {
        _selectCommand = new OracleCommand(command_text, connt);
        _selectCommand.BindByName = _bindbyname;
    }
    public OracleCommand SelectCommand
    {
        get
        {
            return _selectCommand;
        }
        set
        {
            _selectCommand = value;
            _selectCommand.BindByName = _bindbyname;
        }
    }
    public void Fill()
    {
        new OracleDataAdapter(_selectCommand).Fill(this);
    }
}

namespace LibraryKadr
{
    public class ListLinkKadr
    {
        private static List<LinkKadr> c = new List<LinkKadr>();
        public static List<LinkKadr> ListLink
        {
            get
            {
                if (c == null)
                {
                    c = new List<LinkKadr>();
                }
                return c;
            }
            set 
            {
                c = value;
            }
        }
        public static void AddLink(LinkKadr l)
        {
            c.Add(l);
        }
        public static Dictionary<int, IDataLinkKadr> dict_getter = new Dictionary<int,IDataLinkKadr>();

        public static ToolStripItem GetMenuItem(IDataLinkKadr _dataGetter)
        {
            ToolStripMenuItem mm = new ToolStripMenuItem("Ссылки на другие данные");
            mm.DropDownOpening += new EventHandler(mm_DropDownOpening);
            foreach (LinkKadr l in c)
            {
                ToolStripItem i = mm.DropDownItems.Add(l.LinkName, null, l.Execute);
                i.Tag = l;
                dict_getter.Add(i.GetHashCode(), _dataGetter);
            }
            return mm;
        }

        /// <summary>
        /// Возвращаем менюшку для WPF контрола
        /// </summary>
        /// <param name="_dataGetter"></param>
        /// <returns></returns>
        public static System.Windows.Controls.MenuItem GetWPFMenuItem(IDataLinkKadr _dataGetter)
        {
            System.Windows.Controls.MenuItem mm = new System.Windows.Controls.MenuItem();
            mm.Header = "Ссылки на другие данные";
            mm.ContextMenuOpening +=new System.Windows.Controls.ContextMenuEventHandler(mm_ContextMenuOpening);
            foreach (LinkKadr l in c)
            {
                System.Windows.Controls.MenuItem i = new System.Windows.Controls.MenuItem() { Header = l.LinkName, Tag = l };
                i.Click += l.Execute;
                mm.Items.Add(i);
                dict_getter.Add(i.GetHashCode(), _dataGetter);
            }
            return mm;
        }

        static void  mm_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
 	        foreach (System.Windows.Controls.MenuItem m in (sender as System.Windows.Controls.MenuItem).Items)
                m.IsEnabled = (m.Tag as LinkKadr).CanExecute(m);
        } 

        static void mm_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripItem m in (sender as ToolStripMenuItem).DropDownItems)
                m.Enabled = (m.Tag as LinkKadr).CanExecute(m);
        }
    }
    public class LinkKadr
    {
        public delegate void ExecuteLinkHandler(object sender, LinkData e);
        public delegate bool CanExecuteLinkHandler(object sender, LinkData e);
        public LinkKadr(string _LinkName, string _commmandName, ExecuteLinkHandler _execEventHandler, CanExecuteLinkHandler _canExecuteHandler)
        {
            LinkName = _LinkName;
            CommandName = _commmandName;
            ExecuteLink += _execEventHandler;
            CanExecuteLink += _canExecuteHandler;
        }
        public string LinkName
        {
            get;
            set;
        }

        public string CommandName
        {
            get;
            set;
        }

        private event ExecuteLinkHandler ExecuteLink;
        private event CanExecuteLinkHandler CanExecuteLink;
        public void Execute(LinkData e)
        {
            if (ExecuteLink!=null && e!=null)
                ExecuteLink(this, e);
        }
        public LinkData GetLinkData(object sender)
        {
            if (sender != null && ListLinkKadr.dict_getter.ContainsKey(sender.GetHashCode()))
                return  ListLinkKadr.dict_getter[sender.GetHashCode()].GetDataLink(sender);
            else
                return null;
        }
        public void Execute(object sender, EventArgs e)
        {
            Execute(this.GetLinkData(sender));
        }
        public bool CanExecute(object sender)
        {
            if (CanExecuteLink != null)
                return CanExecuteLink(sender, GetLinkData(sender));
            else return false;
        }
        public bool CanExecute(LinkData e)
        {
            if (CanExecuteLink != null && e != null)
                return CanExecuteLink(null, e);
            else return false;
        }

        public static bool CanExecuteByAccessSubdiv(object transfer_id, string app_name)
        {
            try
            {
                OracleCommand a = new OracleCommand(string.Format(Queries.GetQuery("LinkCanExecute.sql"), Connect.Schema), Connect.CurConnect);
                a.BindByName = true;
                a.Parameters.Add("p_transfer_id", OracleDbType.Decimal, transfer_id, ParameterDirection.Input);
                a.Parameters.Add("p_app_name", OracleDbType.Varchar2, app_name, ParameterDirection.Input);
                object c = a.ExecuteScalar();
                if (c != null && c != DBNull.Value)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
    public class LinkData
    {
        public LinkData(string _per_num, decimal? _transfer_id)
        {
            Per_num = _per_num;
            Transfer_id = _transfer_id;
        }
        public string Per_num
        { get; set; }
        public decimal? Transfer_id
        {
            get;
            set;
        }
    }
    public interface IDataLinkKadr
    {
        LinkData GetDataLink(object sender);
    }
}
