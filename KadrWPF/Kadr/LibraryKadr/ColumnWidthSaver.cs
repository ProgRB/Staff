using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibraryKadr
{
    //Класс, который работает с длиной колонок
    public class ColumnWidthSaver
    {
        /// <summary>
        /// Добавлено 10.04.2010 Бурцевым Михаилом.
        /// Изменение пути Application.StartupPath на C:\Program Files\Staff,
        /// чтобы настройки для программы хранились на машине пользователя
        /// </summary> 
        //static string pathToProgramFiles = @"C:\Program Files\Staff"; 
        
        // 11.02.2015 static string pathToProgramFiles = Application.CommonAppDataPath;
        static string pathToProgramFiles = Application.LocalUserAppDataPath;
        static Dictionary<string, DataGridWidthNew> mapOfDataGrids;
        /// <summary>
        /// Временная заглушка для пользователей, для перехода на новое заполнение настроек столбцов
        /// </summary>
        static ColumnWidthSaver()
        {
            if (File.Exists(pathToProgramFiles + @"\WidthOfColumn.bin"))
            {
                try
                {
                    FileStream file = File.Open(pathToProgramFiles + @"\WidthOfColumn.bin", FileMode.Open);
                    if (file.Length == 0)
                    {
                        file.Close();
                        File.Delete(pathToProgramFiles + @"\WidthOfColumn.bin");
                    }
                    else
                    {
                        BinaryFormatter formater = new BinaryFormatter();
                        ArrayList listOfGrids = (ArrayList)formater.Deserialize(file);
                        mapOfDataGrids = new Dictionary<string, DataGridWidthNew>();
                        //Перебираем все значения списка
                        for (int i = 0; i < listOfGrids.Count; i++)
                        {
                            DataGridWidth dataGridWidth = (DataGridWidth)listOfGrids[i];
                            DataGridWidthNew dataGridWidthnew = new DataGridWidthNew(dataGridWidth.Name);
                            foreach (KeyValuePair<string, int> p in dataGridWidth.Columns)
                            {
                                if (dataGridWidthnew.Columns.ContainsKey(p.Key))
                                    dataGridWidthnew.Columns[p.Key] = p.Value;
                                else
                                    dataGridWidthnew.Columns.Add(p.Key.ToUpper(), p.Value);
                            }
                            if (!mapOfDataGrids.ContainsKey(dataGridWidth.Name))
                                mapOfDataGrids.Add(dataGridWidth.Name, dataGridWidthnew);
                            else
                                mapOfDataGrids[dataGridWidth.Name] = dataGridWidthnew;
                        }
                        file.Close();
                        file = new FileStream(pathToProgramFiles + @"\WidthOfColumnNew.bin", FileMode.Create, FileAccess.Write);
                        new BinaryFormatter().Serialize(file, mapOfDataGrids);
                        file.Flush();
                        file.Close();
                        File.Delete(pathToProgramFiles + @"\WidthOfColumn.bin");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (mapOfDataGrids == null)
                        mapOfDataGrids = new Dictionary<string, DataGridWidthNew>();
                }
            }
            else
            {
                /* 11.02.2015 - Добавил копирование файла из Application.CommonAppDataPath в Application.LocalUserAppDataPath,
                 чтобы настройки хранились в папке конкретного пользователя, потому что возникают ошибки с доступом на AllUsers*/
                if (File.Exists(Application.CommonAppDataPath + @"\WidthOfColumnNew.bin") &&
                    !File.Exists(pathToProgramFiles + @"\WidthOfColumnNew.bin"))
                { 
                    File.Copy(Application.CommonAppDataPath + @"\WidthOfColumnNew.bin", pathToProgramFiles + @"\WidthOfColumnNew.bin");
                }
                if (File.Exists(pathToProgramFiles + @"\WidthOfColumnNew.bin"))
                {
                    try
                    {
                        FileStream f = new FileStream(pathToProgramFiles + @"\WidthOfColumnNew.bin", FileMode.Open, FileAccess.Read);
                        mapOfDataGrids = (Dictionary<string, DataGridWidthNew>)new BinaryFormatter().Deserialize(f);
                        f.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (mapOfDataGrids == null)
                            mapOfDataGrids = new Dictionary<string, DataGridWidthNew>();
                    }
                }
                else
                    mapOfDataGrids = new Dictionary<string, DataGridWidthNew>();
            }
        }

        //Функция, которая заполняет длину колонок таблицы (DataGridView)
        public static void FillWidthOfColumn(DataGridView dataGridView)
        {
            if (mapOfDataGrids.ContainsKey(dataGridView.Name))
            {
                DataGridWidthNew d = mapOfDataGrids[dataGridView.Name];
                int k;
                for (int i = 0; i < dataGridView.Columns.Count; ++i)
                    if (dataGridView.Columns[i].Visible && d.Columns.TryGetValue(dataGridView.Columns[i].Name, out k))
                    {
                        dataGridView.Columns[i].Width = k;
                    }
            }
        }
        //Функция которая сохраняет длину колонок таблицы (DataGridView)
        public static void SaveWidthOfAllColumns(DataGridView dataGridView)
        {
            //Если файл существует
            if (File.Exists(pathToProgramFiles + @"\WidthOfColumnNew.bin"))
            {
                try
                {
                    //Встречается ли данная таблица в списке
                    if (mapOfDataGrids.ContainsKey(dataGridView.Name))
                    {
                        //Тогда меняем значение всех столбцов
                        Dictionary<string, int> columns = mapOfDataGrids[dataGridView.Name].Columns;
                        int k;
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                            if (columns.TryGetValue(dataGridView.Columns[j].Name, out k))
                                columns[dataGridView.Columns[j].Name] = dataGridView.Columns[j].Width;
                            else
                                columns.Add(dataGridView.Columns[j].Name, dataGridView.Columns[j].Width);
                    }
                    else
                    {
                        DataGridWidthNew dataGridWidth = FillDataGridWidth(dataGridView);
                        mapOfDataGrids.Add(dataGridView.Name, dataGridWidth);
                    }
                    //Осуществляем сериализацию
                    FileStream fileStream = File.Open(pathToProgramFiles + @"\WidthOfColumnNew.bin", FileMode.Create);
                    new BinaryFormatter().Serialize(fileStream, mapOfDataGrids);
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else //Если файла нет
            {
                BinaryFormatter formater = new BinaryFormatter();
                //Заполняем список
                mapOfDataGrids.Add(dataGridView.Name, FillDataGridWidth(dataGridView));
                FileStream fileStream = File.Open(pathToProgramFiles + @"\WidthOfColumnNew.bin", FileMode.Create);
                formater.Serialize(fileStream, mapOfDataGrids);
                fileStream.Close();
            }
        }
        private static DataGridWidthNew FillDataGridWidth(DataGridView dataGridView)
        {
            DataGridWidthNew dataGridWidth = new DataGridWidthNew(dataGridView.Name);
            for (int i = 0; i < dataGridView.Columns.Count; i++)
                dataGridWidth.Columns.Add(dataGridView.Columns[i].Name, dataGridView.Columns[i].Width);
            return dataGridWidth;
        }

        public static void SaveWidthOfColumn(object sender, DataGridViewColumnEventArgs e)
        {
            //Если файл существует
            if (File.Exists(pathToProgramFiles + @"\WidthOfColumnNew.bin"))
            {
                try
                {
                    //Встречается ли данная таблица в списке
                    if (mapOfDataGrids.ContainsKey((sender as DataGridView).Name))
                    {
                        //Тогда меняем значение всех столбцов
                        Dictionary<string, int> columns = mapOfDataGrids[(sender as DataGridView).Name].Columns;
                        int k;
                        if (columns.TryGetValue(e.Column.Name, out k))
                            if (k==e.Column.Width) return;
                            else columns[e.Column.Name] = e.Column.Width;
                        else
                            columns.Add(e.Column.Name, e.Column.Width);
                    }
                    else
                    {
                        DataGridWidthNew dataGridWidth = FillDataGridWidth(sender as DataGridView);
                        mapOfDataGrids.Add((sender as DataGridView).Name, dataGridWidth);
                    }
                    //Осуществляем сериализацию
                    FileStream fileStream = File.Open(pathToProgramFiles + @"\WidthOfColumnNew.bin", FileMode.Create);
                    new BinaryFormatter().Serialize(fileStream, mapOfDataGrids);
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else //Если файла нет
            {
                BinaryFormatter formater = new BinaryFormatter();
                //Заполняем список
                mapOfDataGrids.Add((sender as DataGridView).Name, FillDataGridWidth(sender as DataGridView));
                FileStream fileStream = File.Open(pathToProgramFiles + @"\WidthOfColumnNew.bin", FileMode.Create);
                formater.Serialize(fileStream, mapOfDataGrids);
                fileStream.Close();
            }
        }
    }
    [Serializable]
    public class DataGridWidth
    {
        private Dictionary<string, int> m_Columns = new Dictionary<string, int>();
        private string m_Names;
        public DataGridWidth(string name)
        {
            m_Names = name;
        }
        public string Name { get { return m_Names; } }
        public Dictionary<string, int> Columns
        {
            get
            {
                return m_Columns;
            }
            set
            {
                m_Columns = value;
            }
        }
    }
    [Serializable]
    public class DataGridWidthNew
    {
        private Dictionary<string, int> m_Columns = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        private string m_Names;
        public DataGridWidthNew(string name)
        {
            m_Names = name;
        }
        public string Name { get { return m_Names; } }
        public Dictionary<string, int> Columns
        {
            get
            {
                return m_Columns;
            }
            set
            {
                m_Columns = value;
            }
        }
    }
}
