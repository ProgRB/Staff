using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryKadr
{
    class ColumnParameter
    {
        string _nameColumn;
        int _width;
        string _datatype;
        string _alignment;
        string _title;
        //Microsoft.Office.Interop.Excel.XlBordersIndex[] _borders;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="nameColumn">Имя столбца (пример:"C:C")</param>
        /// <param name="width">Ширина столбца</param>
        /// <param name="datatype">Тип данных</param>        
        /// <param name="title">Заголовок столбца</param>
        public ColumnParameter(string nameColumn, int width, string datatype, string title)
        {
            _nameColumn = nameColumn;
            _width = width;
            _datatype = datatype;            
            _title = title;
        }
        /// <summary>
        /// Наименование столбца
        /// </summary>
        public string NameColumn
        {
            get
            {
                return _nameColumn;
            }
        }
        /// <summary>
        /// Ширина столбца
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
        }
        /// <summary>
        /// Тип данных
        /// </summary>
        public string DataType
        {
            get
            {
                return _datatype;
            }
        }
        /// <summary>
        /// Заголовок столбца
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
        }
    }
}
