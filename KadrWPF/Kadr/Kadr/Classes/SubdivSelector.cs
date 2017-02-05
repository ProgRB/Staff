using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Kadr.Shtat;
using Staff;
using Oracle.DataAccess.Client;
using LibraryKadr;
using System.Drawing.Drawing2D;
using System.Windows.Threading;

using sw = System.Windows;

namespace Kadr.Classes
{
    public partial class SubdivSelector : UserControl, INotifyPropertyChanged
    {
        private string _byRule = null;
        private decimal? _subdiv_id = null;
        private DataTable tb = new DataTable();

        Timer tm = new Timer();

        public SubdivSelector()
        {
            InitializeComponent();
            tm.Interval = 800;
            tm.Enabled = true;
            tm.Tick += tm_Tick;
            this.Load+=new EventHandler(SubdivSelector_Load);
            if (!this.DesignMode)
                SetWpfComboBox();
        }

        void tm_Tick(object sender, EventArgs e)
        {
            tm.Stop();
            OnSubdivChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Общий источник данных подразделений
        /// </summary>
        private DataView dv = null;

        private bool _isPlantSelectable=true;
        public bool IsAllPlantSelectable
        {
            get
            {
                return _isPlantSelectable;
            }
            set
            {
                _isPlantSelectable = value;
                if (SubdivSource != null)
                    SubdivSource.RowFilter = _isPlantSelectable ? "" : "subdiv_id<>0";
            }
        }

        /// <summary>
        /// Происходит при выборе подразделения
        /// </summary>
        public event EventHandler SubdivChanged;

        protected virtual void OnSubdivChanged(object sender,EventArgs e)
        {
            if (SubdivChanged != null)
                SubdivChanged(this, e);
        }
           
        private void SubdivSelector_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                LoadSubdivs(ByRule);
                if (_subdiv_id == null && SubdivSource != null && SubdivSource.Count > 0)
                    subdiv_id = (decimal?)SubdivSource[0]["SUBDIV_ID"];
            }
            Loaded = true;
        }

        /// <summary>
        /// Загрузился ли это гребаный селектор
        /// </summary>
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                _loaded = value;
            }
        }

        /// <summary>
        /// А здесь ставим комбобокс из впф на форму
        /// </summary>
        private void SetWpfComboBox()
        {
            sw.Controls.Grid gg = new sw.Controls.Grid();
            //gg.ColumnDefinitions = new sw.Controls.ColumnDefinitionCollection();
            gg.ColumnDefinitions.Add(new sw.Controls.ColumnDefinition() { Width = new sw.GridLength(50) });
            gg.ColumnDefinitions.Add(new sw.Controls.ColumnDefinition());
            gg.DataContext = this;

            System.Windows.Controls.ComboBox cb1 = new System.Windows.Controls.ComboBox();
            cb1.SetBinding(sw.Controls.ComboBox.ItemsSourceProperty, "SubdivSource");
            cb1.SetBinding(System.Windows.Controls.ComboBox.SelectedValueProperty, "subdiv_id");
            cb1.DisplayMemberPath = "CODE_SUBDIV";
            cb1.SelectedValuePath="SUBDIV_ID";
            //cb1.SetValue(System.Windows.Controls.TextSearch.TextPathProperty, "CODE_SUBDIV"); // пусть показывает и код подразделения через пробел
            cb1.IsEditable = true;
            gg.Children.Add(cb1);

            System.Windows.Controls.ComboBox cb = new System.Windows.Controls.ComboBox();
            cb.SetBinding(sw.Controls.ComboBox.ItemsSourceProperty, "SubdivSource");
            cb.SetBinding(System.Windows.Controls.ComboBox.SelectedValueProperty, "subdiv_id");
            cb.SelectedValuePath = "SUBDIV_ID";
            cb.IsEditable = true;
            cb.SetValue(System.Windows.Controls.TextSearch.TextPathProperty, "SUBDIV_NAME"); // пусть показывает имя подразделения через пробел
            cb.Margin = new sw.Thickness(2, 0, 0, 0);
            cb.SetBinding(sw.Controls.ComboBox.ToolTipProperty, "SubdivName");// прибиндим для подсказки название подразделения
            sw.Controls.Grid.SetColumn(cb, 1);
            gg.Children.Add(cb);

            System.Windows.Controls.Grid.SetIsSharedSizeScope(cb, true);
            
            System.Windows.DataTemplate tp = new System.Windows.DataTemplate();

            var gridFactory = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.Grid));
            var column1 = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.ColumnDefinition));
            column1.SetValue(sw.Controls.ColumnDefinition.SharedSizeGroupProperty, "cl1");
            var column2 = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.ColumnDefinition));
            column2.SetValue(sw.Controls.ColumnDefinition.SharedSizeGroupProperty, "cl2");
            gridFactory.AppendChild(column1);
            gridFactory.AppendChild(column2);

            var tb1Factory= new sw.FrameworkElementFactory(typeof(sw.Controls.TextBlock));
            tb1Factory.SetValue(sw.Controls.TextBlock.MarginProperty, new sw.Thickness(5,2,5,2));
            tb1Factory.SetValue(sw.Controls.Grid.ColumnProperty, 0);
            tb1Factory.SetBinding(sw.Controls.TextBlock.TextProperty, new sw.Data.Binding("CODE_SUBDIV"));

            var tb2Factory= new sw.FrameworkElementFactory(typeof(sw.Controls.TextBlock));
            tb2Factory.SetValue(sw.Controls.TextBlock.MarginProperty, new sw.Thickness(5,2,5,2));
            tb2Factory.SetValue(sw.Controls.Grid.ColumnProperty, 1);
            tb2Factory.SetBinding(sw.Controls.TextBlock.TextProperty, new sw.Data.Binding("SUBDIV_NAME"));

            gridFactory.AppendChild(tb1Factory);
            gridFactory.AppendChild(tb2Factory);

            tp.VisualTree = gridFactory;

            cb.ItemTemplate = tp;

            elementHost1.Child = gg;
        }

        /// <summary>
        /// Айдишник подразделения
        /// </summary>
        public decimal? subdiv_id
        {
            get
            {
                return _subdiv_id;
            }
            set
            {
                LoadSubdivs(ByRule); // загружаем подразделения если они еще не загружены
                if (_subdiv_id != value)
                {
                    _subdiv_id = value;
                    if (_subdiv_id != null && this.Loaded)
                    {
                        tm.Stop();
                        tm.Start();
                    }
                        //OnSubdivChanged(this, EventArgs.Empty);
                    OnPropertyChanged("subdiv_id");
                    OnPropertyChanged("CodeSubdiv");
                    OnPropertyChanged("SubdivName");
                }
            }
        }


        /// <summary>
        /// Источник данных для подразделений
        /// </summary>
        public DataView SubdivSource
        {
            get
            {
                return dv;
            }
            set
            {
                dv = value;
                OnPropertyChanged("SubdivSource");
            }
        }

        /// <summary>
        /// Наименование выбранного подразделения
        /// </summary>
        [Browsable(false)]
        public string SubdivName
        {
            get
            {
                if (_subdiv_id == null || this.DesignMode) return string.Empty;
                else
                {
                    if (CurrentSubdiv != null)
                        return CurrentSubdiv["SUBDIV_NAME"].ToString();
                    else
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Код выбранного подразделения
        /// </summary>
        [Browsable(false)]
        public string CodeSubdiv
        {
            get
            {
                if (_subdiv_id == null || this.DesignMode) return string.Empty;
                else
                {
                    if (CurrentSubdiv != null)
                        return CurrentSubdiv["CODE_SUBDIV"].ToString();
                    else
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Текущая строчка подразделения
        /// </summary>
        private DataRowView CurrentSubdiv
        {
            get
            {
                if (SubdivSource != null)
                {
                    return SubdivSource.OfType<DataRowView>().Where(r => r.Row.Field2<decimal?>("SUBDIV_ID") == _subdiv_id).FirstOrDefault();
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Поле означает какую роль требуется использовать для фильтрации подрзаделений
        /// </summary>
        public string ByRule
        {
            get
            {
                return _byRule;
            }
            set
            {
                if (_byRule != value)
                {
                    LoadSubdivs(value);
                    _byRule = value;
                    OnPropertyChanged("SubdivSource");
                }                
                OnPropertyChanged("ByRule");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private bool _loaded = false;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Загружаем данные под доступу в подрзаделения
        /// </summary>
        private void LoadSubdivs(string accessName)
        {
            if (!this.DesignMode && (dv==null || accessName!=_byRule))
            {
                try
                {
                    tb.Rows.Clear();
                    if (string.IsNullOrEmpty(accessName))
                    {
                        OracleDataAdapter a = new OracleDataAdapter(string.Format(@"select distinct subdiv_id, subdiv_name, code_subdiv, parent_id, level as sub_level from {0}.subdiv 
                        where sub_actual_sign=1 and code_subdiv<'700' and code_subdiv is not null
                        start with parent_id is null
                        connect by prior subdiv_id=parent_id
                        order by level, code_subdiv", Connect.Schema), Connect.CurConnect);
                        a.Fill(tb);
                    }
                    else
                    {
                        OracleDataAdapter a = new OracleDataAdapter(string.Format(@"select subdiv_id, 
                             case when subdiv_id=0 then 'У-УАЗ' else subdiv_name end subdiv_name, 
                             case when subdiv_id=0 then '000' else code_subdiv end code_subdiv, 
                             parent_id, sub_level 
                        from {0}.subdiv_roles where APP_NAME=UPPER(:p_app_name) and sub_level<3", Connect.Schema), Connect.CurConnect);
                        a.SelectCommand.Parameters.Add("p_app_name", accessName);
                        a.Fill(tb);
                        if (!tb.Columns.Contains("CODENAME"))
                            tb.Columns.Add("CODENAME").Expression = "CODE_SUBDIV+' '+SUBDIV_NAME";
                    }
                    SubdivSource = new DataView(tb, _isPlantSelectable ? "" : "subdiv_id<>0", "code_subdiv", DataViewRowState.CurrentRows);
                }
                catch
                { }
                if (SubdivSource.Count > 0)
                    _subdiv_id = (decimal?)SubdivSource[0]["SUBDIV_ID"];
            }
        }
    }


    //public class DropDownSubdivBox : ComboBox
    //{
    //    public DropDownSubdivBox()
    //        : base()
    //    { }
    //    protected override void WndProc(ref Message m)
    //    {
    //        if (m.Msg != 0x020A || this.DroppedDown)
    //            base.WndProc(ref m);
    //    }
    //}

    //public class ResizableToolStripDropDown : ToolStripDropDown 
    //{
    //    public ResizableToolStripDropDown()
    //        : base()
    //    {
    //        this.AutoSize = false;
    //    }
    //    private Rectangle BottomGripBounds
    //    {
    //        get
    //        {
    //            Rectangle r = ClientRectangle;
    //            r.Y = r.Bottom - 4;
    //            r.Height = 4;
    //            return r;
    //        }
    //    }
    //    private Rectangle BottomRightGripBounds
    //    {
    //        get
    //        {
    //            Rectangle r = BottomGripBounds;
    //            r.X = r.Width - 4;
    //            r.Width = 4;
    //            return r;
    //        }
    //    }
    //    protected override void WndProc(ref Message m)
    //    {
    //        if (m.Msg == NativeMethods.WM_NCHITTEST)
    //        {
    //            int x = NativeMethods.LOWORD(m.LParam), y = NativeMethods.HIWORD(m.LParam);
    //            Point p = PointToClient(new Point(x, y));
    //            if (BottomRightGripBounds.Contains(p))
    //            {
    //                m.Result = (IntPtr)NativeMethods.HTBOTTOMRIGHT;
    //                return;
    //            }
    //            if (BottomGripBounds.Contains(p))
    //            {
    //                m.Result = (IntPtr)NativeMethods.HTBOTTOM;
    //                return;
    //            }
    //        }
    //        base.WndProc(ref m);
    //    }
    //    internal class NativeMethods
    //    {
    //        internal const int WM_NCHITTEST = 0x0084,
    //                         HTBOTTOM = 15,
    //                         HTBOTTOMRIGHT = 17;
    //        internal static int HIWORD(int n)
    //        {
    //            return (n >> 16) & 0xffff;
    //        }

    //        internal static int HIWORD(IntPtr n)
    //        {
    //            return HIWORD(unchecked((int)(long)n));
    //        }

    //        internal static int LOWORD(int n)
    //        {
    //            return n & 0xffff;
    //        }

    //        internal static int LOWORD(IntPtr n)
    //        {
    //            return LOWORD(unchecked((int)(long)n));
    //        }

    //    }
    //}
    //public class MyToolStripRender : ToolStripRenderer
    //{
    //    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    //    {
    //        base.OnRenderToolStripBorder(e);
    //        ControlPaint.DrawFocusRectangle(e.Graphics, e.AffectedBounds, SystemColors.ControlDarkDark, SystemColors.ControlDarkDark);
    //    }
    //}
}
