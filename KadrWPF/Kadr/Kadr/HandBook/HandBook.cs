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

using System.Reflection;
using LibraryKadr;
namespace Kadr
{
    public partial class HandBook : Form
    {
        //BindingSource bsspr = new BindingSource();
        public Type type_seq;
        object handbook;
        /// <summary>
        /// Конструктор формы справочника
        /// </summary>
        /// <param name="_formmain">Родительская форма</param>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_type_seq">Тип таблицы</param>
        public HandBook(FormMain _formmain, Type _type_seq)
        {        
            InitializeComponent();
            type_seq = _type_seq;
            Handbook(type_seq, 0, null);
            this.MdiParent = _formmain;
            this.Show();
        }

        /// <summary>
        /// Конструктор формы справочника
        /// </summary>
        /// <param name="_formmain">Родительская форма</param>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_type_seq">Тип таблицы</param>
        /// <param name="_widthform">Ширина формы</param>
        /// <param name="_widthcolumn">Ширина колонки</param>
        /// <param name="_names">Имя колонок</param>
        public HandBook(FormMain _formmain, Type _type_seq, int _widthform, int _widthcolumn, params string[] _names)
        {
            InitializeComponent(); 
            type_seq = _type_seq;
            int i = this.splitContainer.Panel2.Width;
            this.Width = _widthform;
            Handbook(type_seq, _widthcolumn, _names);
            this.splitContainer.SplitterDistance = _widthform - i;
            this.MdiParent = _formmain;
            this.Show();
        }

        /// <summary>
        /// Свойство возвращает биндингсоурс
        /// </summary>
        //public BindingSource BindingSource
        //{
        //    get { return bsspr; }
        //}

        /// <summary>
        /// Кнопка закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            if (btExit.Text == "Выход")
            {
                Close();
            }
            else
            {
                dgView.Enabled = true;
                btAdd.Enabled = true;
                btEdit.Enabled = true;
                btDelete.Enabled = true;
                btExit.Text = "Выход";
                btSave.Enabled = false;
                tbName.Enabled = false;
                foreach (Control contr in groupBox1.Controls)
                {
                    if (contr is TextBox_BMW)
                        ((TextBox_BMW)contr).Enabled = false;
                    if (contr is ComboBox_BMW)
                        ((ComboBox_BMW)contr).Enabled = false;
                }
                type_seq.InvokeMember("RollBack", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                Connect.Rollback();
                dgView.Invalidate();
                type_seq.InvokeMember("ResetBindings", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                this.Invalidate();
                //Width_Column(dgView, _widthcolumn);
            }
        }
        
        /// <summary>
        /// Событие активации формы и отключения кнопок при отсутствии данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSpr_Activated(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
            {
                this.btEdit.Enabled = false;
                this.btDelete.Enabled = false;
            }
        }

        /// <summary>
        /// Метод создает таблицу по ее типу
        /// </summary>
        /// <param name="type_seq">Тип таблицы</param>
        /// <param name="_widthcolumn">Ширина колонки</param>
        /// <param name="displayNames">Имя колонки</param>
       private void Handbook(Type type_seq, int _widthcolumn, params string[] displayNames)
        {
            handbook = Activator.CreateInstance(type_seq, new object[] { Connect.CurConnect });
            type_seq.InvokeMember("Fill", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
            //bsspr.DataSource = handbook;
            //bdNavigator.BindingSource = bsspr;
            TableAttribute att = (TableAttribute)type_seq.GetCustomAttributes(false).Where(s => s is TableAttribute).FirstOrDefault();
            this.Text = att.Comment;
            if (displayNames == null)
                dgView.AddBindingSource(handbook, new LinkArgument(null, ""));
            else
            {
                List<LinkArgument> linkAgument = new List<LinkArgument>();
                //Получаем список вторичных ключей
                PropertyInfo[] foreignKey = GetForiegnKey(handbook);
                for (int i = 0; i < foreignKey.Count(); i++)
                {
                    PropertyInfo property = foreignKey[i];
                    //Получаем аттрибут колонки
                    ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                    //Получаем ссылосную таблицу
                    FullTableRef tableRef = Staff.DataGridViewSuperStructure.getTableAndField(attribute.RefTable);
                    //Получаем тип последовательности
                    Type typeSeq = GetSequenceType(string.Format("{0}_seq", tableRef.RefTable));
                    Object seqInstance = Activator.CreateInstance(typeSeq, new object[] { Connect.CurConnect });
                    typeSeq.InvokeMember("Fill", BindingFlags.Default | BindingFlags.InvokeMethod, null, seqInstance, null);
                    //bsreftable.DataSource = seqInstance;
                    linkAgument.Add(new LinkArgument(seqInstance, displayNames[i]));
                }
                dgView.AddBindingSource(handbook, linkAgument.ToArray());
            }
            dgView.Invalidate();
            dgView.Columns[0].Visible = false;
            dgView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            PropertyInfo[] properties = DataGridViewSuperStructure.GetObjFromSeq(type_seq).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute columnattr = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                if (!columnattr.FromSequence)
                {
                    lbName.Text = columnattr.Caption;
                    tbName.MaxLength = columnattr.DataLength;
                    tbName.AddBindingSource(handbook, property.Name);
                    break;
                }
            }
            /// Событие добавления данных в справочник
            btAdd.Click += new EventHandler(delegate(object sender1, EventArgs e1)
            {
                dgView.Enabled = false;
                btAdd.Enabled = false;
                btEdit.Enabled = false;
                btDelete.Enabled = false;
                btExit.Text = "Отмена";
                btSave.Enabled = true;
                tbName.Enabled = true;
                foreach (Control contr in groupBox1.Controls)
                {
                    if (contr is TextBox_BMW)
                        ((TextBox_BMW)contr).Enabled = true;
                    if (contr is ComboBox_BMW)
                        ((ComboBox_BMW)contr).Enabled = true;
                }
                tbName.Focus();
                type_seq.InvokeMember("AddNew", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                BindingContext[handbook].Position = dgView.Rows.Count - 1;
                this.Invalidate();                
                Width_Column(dgView,_widthcolumn);
            });
            /// Событие изменения данных в справочнике
            btEdit.Click += new EventHandler(delegate(object sender1, EventArgs e1)
            {
                dgView.Enabled = false;
                btAdd.Enabled = false;
                btEdit.Enabled = false;
                btDelete.Enabled = false;
                btExit.Text = "Отмена";
                btSave.Enabled = true;
                tbName.Enabled = true;
                foreach (Control contr in groupBox1.Controls)
                {
                    if (contr is TextBox_BMW)
                        ((TextBox_BMW)contr).Enabled = true;
                    if (contr is ComboBox_BMW)
                        ((ComboBox_BMW)contr).Enabled = true;
                }
                tbName.Focus();
            });
            /// Событие удаления данных из справочника
            btDelete.Click += new EventHandler(delegate(object sender1, EventArgs e1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgView.Rows.Remove(dgView.CurrentRow);
                    type_seq.InvokeMember("Save", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                    Connect.Commit();
                    Width_Column(dgView, _widthcolumn);
                    if (dgView.Rows.Count == 0)
                    {
                        btEdit.Enabled = false;
                        btDelete.Enabled = false;
                    }
                }
            });
            /// Событие сохранения данных в справочнике
            btSave.Click += new EventHandler(delegate(object sender1, EventArgs e1)
            {
                if (tbName.Text != "")
                {
                    dgView.Enabled = true;
                    btAdd.Enabled = true;
                    btEdit.Enabled = true;
                    btDelete.Enabled = true;
                    btExit.Text = "Выход";
                    btSave.Enabled = false;
                    tbName.Enabled = false;
                    foreach (Control contr in groupBox1.Controls)
                    {
                        if (contr is TextBox_BMW)
                            ((TextBox_BMW)contr).Enabled = false;
                        if (contr is ComboBox_BMW)
                            ((ComboBox_BMW)contr).Enabled = false;
                    }
                    type_seq.InvokeMember("Save", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                    Connect.Commit();
                    Width_Column(dgView,_widthcolumn);
                }
                else
                {
                    MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbName.Focus();
                }
            });
            /// Закрытие формы
            FormClosing += new FormClosingEventHandler(delegate(object sender1, FormClosingEventArgs e1)
            {
                if (btSave.Enabled == true)
                {
                    DialogResult dialog = MessageBox.Show("Вы не сохранили изменения данных. Сохранить изменения?", "АСУ \"Кадры\"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (dialog == DialogResult.Yes)
                    {
                        if (tbName.Text != "")
                        {                            
                            type_seq.InvokeMember("Save", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                            Connect.Commit();
                        }
                        else
                        {
                            MessageBox.Show("Вы не ввели значение реквизита!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        //type_seq.InvokeMember("Save", BindingFlags.Default | BindingFlags.InvokeMethod, null, handbook, null);
                        //connection.Commit();
                    }
                    else if (dialog == DialogResult.Cancel)
                    {
                        e1.Cancel = true;
                        tbName.Focus();
                    }
                }
            });
            /// Изменение ширины панелей таблицы и поля реквизита
            splitContainer.SplitterMoved += new SplitterEventHandler(delegate(object sender1, SplitterEventArgs e1)
            {
                Width_Column(dgView, _widthcolumn);
            });
            /// Изменение размеров формы
            Resize += new EventHandler(delegate(object sender1, EventArgs e1)
            {
                Width_Column(dgView, _widthcolumn);
            });
            Show();
        }

        /// <summary>
        /// Метод меняет ширину колонок датагрида
        /// </summary>
        /// <param name="_dgView">Датагрид, в котором необходимо поменять ширину колонки</param>
        /// <param name="_widthcolumn">Ширина колонки</param>
        public static void Width_Column(DataGridView _dgView,int _widthcolumn)
        {
            if (_widthcolumn == 0)
            {
                if (_dgView.Height < _dgView.RowCount * _dgView.RowTemplate.Height + _dgView.ColumnHeadersHeight)
                {
                    _dgView.Columns[1].Width = _dgView.Width - 44;
                }
                else
                {
                    _dgView.Columns[1].Width = _dgView.Width - 27;
                }
            }
            else
            {
                if (_dgView.Height < _dgView.RowCount * _dgView.RowTemplate.Height + _dgView.ColumnHeadersHeight)
                {
                    _dgView.Columns[1].Width = _dgView.Width - 44 - _widthcolumn;
                }
                else
                {
                    _dgView.Columns[1].Width = _dgView.Width - 27 - _widthcolumn;
                }
            }
        }

        public static Type GetSequenceType(string name)
        {
            Assembly asa = Assembly.GetEntryAssembly();
            Assembly assembly = Assembly.GetAssembly(typeof(EMP_obj));
            //return assembly.GetType(string.Format("{0}.{1}", Staff.DataSourceScheme.SchemeName, name));            
            return assembly.GetType("Staff." + name, true, true);
        }

        /// <summary>
        /// Функция получения вторичных ключей
        /// </summary>
        /// <param name="obj">Объект, ключ которого нужно получить</param>
        /// <returns>Массив свойств</returns>
        public static PropertyInfo[] GetForiegnKey(Object obj)
        {
            Type type_obj = Staff.DataGridViewSuperStructure.GetObjFromSeq(obj.GetType());
            PropertyInfo[] properties = type_obj.GetProperties();
            List<PropertyInfo> rezult = new List<PropertyInfo>();
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute columnAttribute = (ColumnAttribute)property.GetCustomAttributes(false).Where(s => s is ColumnAttribute).FirstOrDefault();
                if (columnAttribute != null && columnAttribute.RefTable != "")
                    rezult.Add(property);
            }
            return rezult.ToArray();
        }
        
    }
}
