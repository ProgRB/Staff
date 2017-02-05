using System;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Globalization;
namespace LibraryKadr
{

    #region Колонка c выпадающими календариками

    public class MDataGridViewCalendarColumn : DataGridViewColumn
    {
        public MDataGridViewCalendarColumn()
            : base(new CalendarCell())
        {
            this.DateFormat = "dd.MM.yyyy";
        }

        public MDataGridViewCalendarColumn(string name, string caption, string dataPropertyName)
            : base(new CalendarCell())
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }

        public string DateFormat
        {
            get;
            set;
        }

    }

    public class CalendarCell : DataGridViewTextBoxCell
    {

        public CalendarCell()
            : base()
        {
            // Use the short date format.
            this.Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CalendarEditingControl ctl =
                DataGridView.EditingControl as CalendarEditingControl;
            // Use the default row value when Value property is null.
            ctl.DateFormat = ((MDataGridViewCalendarColumn)this.OwningColumn).DateFormat;
            if (this.Value == null || this.Value == DBNull.Value)
            {
                ctl.Value = DateTime.Now;
                ctl.Checked = false;
            }
            else
            {
                ctl.Value = (DateTime)this.Value;
                ctl.Checked = true;
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(CalendarEditingControl);
            }
        }

        /*public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                //return typeof(DateTime?);
            }
        }*/

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                if (this.OwningColumn != null && this.OwningColumn.IsDataBound)
                    return DBNull.Value;
                else
                    return null;
            }
        }

        class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
        {
            private bool check_state = false;
            DataGridView dataGridView;
            private bool valueChanged = false;
            int rowIndex;

            public CalendarEditingControl()
            {
                this.Format = DateTimePickerFormat.Custom;
            }

            public string DateFormat
            {
                get;
                set;
            }
            public new bool Checked
            {
                get
                {
                    return base.Checked;
                }
                set
                {
                    check_state = value;
                    base.Checked = value;
                }
            }
            // Implements the IDataGridViewEditingControl
            // .EditingControlDataGridView property.
            public DataGridView EditingControlDataGridView
            {
                get
                {
                    return dataGridView;
                }
                set
                {
                    dataGridView = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
            // property.
            public object EditingControlFormattedValue
            {
                get
                {
                    if (this.Checked) return this.Value.ToString();
                    else return "";
                }
                set
                {
                    /*if (value is String || value is DBNull)
                    {*/
                    try
                    {
                        // This will throw an exception of the string is 
                        // null, empty, or not in the format of a date.
                        this.Value = DateTime.Parse((String)value);
                        this.Checked = true;
                    }
                    catch
                    {
                        // In the case of an exception, just use the 
                        // default value so we're not left with a null
                        // value.
                        this.Value = DateTime.Now;
                        this.Checked = false;
                    }
                    //}
                }
            }

            // Implements the 
            // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
            public object GetEditingControlFormattedValue(
                DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            // Implements the 
            // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
            public void ApplyCellStyleToEditingControl(
                DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
                this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
                this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
            }

            // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
            // property.
            public int EditingControlRowIndex
            {
                get
                {
                    return rowIndex;
                }
                set
                {
                    rowIndex = value;
                }
            }

            // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
            // method.
            public bool EditingControlWantsInputKey(
                Keys key, bool dataGridViewWantsInputKey)
            {
                // Let the DateTimePicker handle the keys listed.
                switch (key & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Right:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageDown:
                    case Keys.PageUp:
                    case Keys.Enter:
                        return true;
                    default:
                        return !dataGridViewWantsInputKey;
                }
            }

            // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
            // method.
            public void PrepareEditingControlForEdit(bool selectAll)
            {
                this.ShowCheckBox = true;
                this.CustomFormat = this.DateFormat;
            }

            // Implements the IDataGridViewEditingControl
            // .RepositionEditingControlOnValueChange property.
            public bool RepositionEditingControlOnValueChange
            {
                get
                {
                    return false;
                }
            }


            // Implements the IDataGridViewEditingControl
            // .EditingControlValueChanged property.
            public bool EditingControlValueChanged
            {
                get
                {
                    return valueChanged;
                }
                set
                {
                    valueChanged = value;
                }
            }

            // Implements the IDataGridViewEditingControl
            // .EditingPanelCursor property.
            public Cursor EditingPanelCursor
            {
                get
                {
                    return base.Cursor;
                }
            }

            protected override void OnValueChanged(EventArgs eventargs)
            {
                // Notify the DataGridView that the contents of the cell
                // have changed.
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(eventargs);
            }
            protected override void OnLostFocus(EventArgs e)
            {
                if (this.check_state != base.Checked)
                {
                    valueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                }
                base.OnLostFocus(e);
            }

            protected override void OnValidating(CancelEventArgs e)
            {
                if (this.check_state != base.Checked)
                {
                    valueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                }
                base.OnValidating(e);
            }

        }
    }

    #endregion

    #region Колонка с маскЭдит в ячейке. Маска задается колонке Mask="маска";

    public class MDataGridViewMaskedTextColumn : DataGridViewColumn
    {
        private string cellMask = "";
        public MDataGridViewMaskedTextColumn()
            : base(new MaskedTextCell(""))
        {
        }
        public MDataGridViewMaskedTextColumn(string name, string caption, string dataPropertyName, string mask)
            : base(new MaskedTextCell(mask))
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
            this.CellMask = mask;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(MaskedTextCell)))
                {
                    throw new InvalidCastException("Must be a MaskedCell");
                }
                base.CellTemplate = value;
            }
        }
        public string CellMask
        {
            get { return cellMask; }
            set { cellMask = value; }
        }


    }

    public class MaskedTextCell : DataGridViewTextBoxCell
    {
        public MaskedTextCell()
            : base()
        { }
        public MaskedTextCell(string mask)
            : base()
        {
            Mask = mask;
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            MaskedEditingControl ctl =
                DataGridView.EditingControl as MaskedEditingControl;
            // Use the default row value when Value property is null.
            ctl.Mask = ((MDataGridViewMaskedTextColumn)this.OwningColumn).CellMask;
            if (((MDataGridViewMaskedTextColumn)this.OwningColumn).IsDataBound)
            {
                ctl.ValueType = this.OwningColumn.ValueType;
            }
            else
                ctl.ValueType = this.ValueType;
            if (this.Value == null)
            {
                ctl.Text = "";
            }
            else
            {
                ctl.Text = this.Value.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(MaskedEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                return base.ValueType;
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return base.DefaultNewRowValue;
            }
        }

        public string Mask
        {
            get;
            set;
        }

        class MaskedEditingControl : MaskedTextBox, IDataGridViewEditingControl
        {
            private bool valueChanged = false;

            public MaskedEditingControl()
            {
            }

            public Type ValueType
            {
                get;
                set;
            }

            public DataGridView EditingControlDataGridView
            {
                get;
                set;
            }

            public object EditingControlFormattedValue
            {
                get
                {
                    this.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                    if (this.Text.Trim() == "")
                        return "";
                    this.TextMaskFormat = MaskFormat.IncludeLiterals;
                    return this.Text;
                }
                set
                {
                    this.Text = value.ToString();
                }
            }

            public int EditingControlRowIndex
            {
                get;
                set;
            }

            public bool EditingControlValueChanged
            {
                get { return valueChanged; }
                set { valueChanged = value; }
            }

            public Cursor EditingPanelCursor
            {
                get { return base.Cursor; }
            }

            public bool RepositionEditingControlOnValueChange
            {
                get { return false; }
            }

            public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dgStyle)
            {
                this.Font = dgStyle.Font;
                this.ForeColor = dgStyle.ForeColor;
                this.BackColor = dgStyle.BackColor;
            }

            public bool EditingControlWantsInputKey(Keys key, bool DataGridViewWantsInputKey)
            {
                switch (key)
                {
                    case Keys.Left:
                    case Keys.Right: return true;
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Enter:
                    case Keys.Escape: return false;
                    default: return !DataGridViewWantsInputKey;
                }
            }

            public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            public void PrepareEditingControlForEdit(bool SelectAll)
            {
                /*ничего не буду делать*/
            }

            protected override void OnTextChanged(EventArgs e)
            {
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnTextChanged(e);
            }

            protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
            {
                try
                {
                    if (this.Text != "")
                        TypeDescriptor.GetConverter(ValueType).ConvertFrom(this.Text);

                }
                catch
                {
                    ToolTip t = new ToolTip();
                    t.IsBalloon = false;
                    t.BackColor = Color.Red;
                    e.Cancel = true;
                    t.Show("Данные введены не верно", this, 0, this.Height, 2000);
                    return;
                }
                base.OnValidating(e);

            }

        }
    }

    #endregion


    #region Колонка с КомбоВсплаывающими таблицами выбора

    /// <summary>
    /// Колонка с выпадающей таблицей
    /// </summary>
    public class MDataGridViewComboGridColumn : DataGridViewColumn
    {
        private Dictionary<string, bool> col_visible = new Dictionary<string, bool>(new MyComparerString());

        public MDataGridViewComboGridColumn()
            : base(new ComboGridCell())
        {
        }

        public MDataGridViewComboGridColumn(string name, string caption, string dataPropertyName, string valueMember, string displayMember, ref object dataSource)
            : base(new ComboGridCell())
        {
            this.Name = name;
            this.HeaderText = caption;
            this.ValueMember = ValueMember;
            this.DisplayMember = displayMember;
            this.DataSource = DataSource;
        }

        public Dictionary<string, bool> ColumnsVisible
        {
            get { return col_visible; }
            set { col_visible = value; }
        }

        public object DataSource
        {
            get;
            set;
        }

        public string ValueMember
        {
            get;
            set;
        }

        public string DisplayMember
        {
            get;
            set;
        }

        public int? DropDownWidth
        {
            get;
            set;
        }

        public int? DropDownHeight
        {
            get;
            set;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(ComboGridCell)))
                {
                    throw new InvalidCastException("Must be a DataPopupCell");
                }
                base.CellTemplate = value;
            }
        }

        public class ComboGridCell : DataGridViewTextBoxCell
        {
            public override Type EditType
            {
                get
                {
                    return typeof(ComboGridEditingControl);
                }
            }

            protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
            {
                if (value == null || value.ToString() == "" || ((MDataGridViewComboGridColumn)this.OwningColumn).ValueMember == "")
                    return null;
                DataRow[] rs = ((DataTable)((MDataGridViewComboGridColumn)this.OwningColumn).DataSource).Select(((MDataGridViewComboGridColumn)this.OwningColumn).ValueMember + "=" + value);
                if (rs.Rank > 0)
                {
                    return rs[0][((MDataGridViewComboGridColumn)this.OwningColumn).DisplayMember];
                }
                else
                    return null;
            }

            public override object DefaultNewRowValue
            {
                get
                {
                    return base.DefaultNewRowValue;
                }
            }

            public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            {
                // Set the value of the editing control to the current cell value.
                base.InitializeEditingControl(rowIndex, initialFormattedValue,
                    dataGridViewCellStyle);
                ComboGridEditingControl ctl =
                    DataGridView.EditingControl as ComboGridEditingControl;
                ctl.DataDispalayed = ((MDataGridViewComboGridColumn)this.OwningColumn).DataSource;
                ctl.DisplayMember = ((MDataGridViewComboGridColumn)this.OwningColumn).DisplayMember;
                ctl.ValueMember = ((MDataGridViewComboGridColumn)this.OwningColumn).ValueMember;
                ctl.DropDownWidth = ((MDataGridViewComboGridColumn)this.OwningColumn).DropDownWidth ?? this.OwningColumn.Width;
                ctl.DropDownHeight = ((MDataGridViewComboGridColumn)this.OwningColumn).DropDownHeight ?? 0;
                ctl.ColumnsVisible = ((MDataGridViewComboGridColumn)this.OwningColumn).ColumnsVisible;
                try
                {
                    if (this.Value != null)
                    {
                        ctl.SelectedValue = this.Value.ToString();
                    }
                }
                catch { }
            }
        }

        public class ComboGridEditingControl : ComboBoxGrid, IDataGridViewEditingControl
        {
            DataGridView dataGridView;
            private bool valueChanged = false;
            int rowIndex;

            public ComboGridEditingControl()
            {
            }
            public DataGridView EditingControlDataGridView
            {
                get
                {
                    return dataGridView;
                }
                set
                {
                    dataGridView = value;
                }
            }


            public object EditingControlFormattedValue
            {
                get
                {
                    if (this.SelectedValue == null)
                        return "";
                    else
                        return this.SelectedValue.ToString();
                }
                set
                {
                    if (value != null)
                        this.SelectedItem = value.ToString();
                }
            }

            public object GetEditingControlFormattedValue(
                DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            // Implements the 
            // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
            public void ApplyCellStyleToEditingControl(
                DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
                this.ForeColor = dataGridViewCellStyle.ForeColor;
                this.BackColor = dataGridViewCellStyle.BackColor;
            }

            public int EditingControlRowIndex
            {
                get
                {
                    return rowIndex;
                }
                set
                {
                    rowIndex = value;
                }
            }

            public bool EditingControlWantsInputKey(
                Keys key, bool dataGridViewWantsInputKey)
            {
                // Let the DateTimePicker handle the keys listed.
                switch (key & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Right:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageDown:
                    case Keys.PageUp:
                    case Keys.LButton
                    :
                        return true;
                    default:
                        return !dataGridViewWantsInputKey;
                }
            }

            public void PrepareEditingControlForEdit(bool selectAll)
            {
                // No preparation needs to be done.
            }

            public bool RepositionEditingControlOnValueChange
            {
                get
                {
                    return false;
                }
            }

            public bool EditingControlValueChanged
            {
                get
                {
                    return valueChanged;
                }
                set
                {
                    valueChanged = value;
                }
            }

            public Cursor EditingPanelCursor
            {
                get
                {
                    return base.Cursor;
                }
            }

            protected override void OnSelectedValueChanged(EventArgs e)
            {
                // Notify the DataGridView that the contents of the cell
                // have changed.
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnSelectedValueChanged(e);
            }
        }
    }
    #endregion

    #region  Колонка текстовая с перегрузкой Конструктора
    public class MDataGridViewTextBoxColumn : DataGridViewTextBoxColumn
    {
        public MDataGridViewTextBoxColumn()
            : base()
        {
        }
        public MDataGridViewTextBoxColumn(string name, string caption, string dataPropertyName)
            : base()
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
        }
        public MDataGridViewTextBoxColumn(string name, string caption, string dataPropertyName, bool IsReadOnly)
            : base()
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
            this.ReadOnly = IsReadOnly;
        }
    }
    #endregion

    #region  Колонка c выпадающим списком с перегрузкой Конструктора
    public class MDataGridViewComboBoxColumn : DataGridViewComboBoxColumn
    {
        public MDataGridViewComboBoxColumn()
            : base()
        {
        }
        public MDataGridViewComboBoxColumn(string name, string caption, string dataPropertyName, string valueMember, string displayMember, object dataSource)
            : base()
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
            this.ValueMember = valueMember;
            this.DisplayMember = displayMember;
            this.DataSource = dataSource;
        }
    }
    #endregion

    #region  Колонка c checkbox с перегрузкой Конструктора
    public class MDataGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public MDataGridViewCheckBoxColumn()
            : base()
        {
        }
        public MDataGridViewCheckBoxColumn(string name, string caption, string dataPropertyName)
            : base()
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
        }
    }
    #endregion


    #region Колонка с NumericUpDown
    public class MDataGridViewNumericColumn : DataGridViewColumn
    {
        private int _decimalPlaces = 0;
        private decimal _mx_val = Decimal.MaxValue, _mn_val = Decimal.MinValue;
        public int DecimalPlaces
        {
            get
            {
                return _decimalPlaces;
            }
            set
            {
                _decimalPlaces = value;
            }
        }
        public MDataGridViewNumericColumn()
            : base(new MNumericCell())
        {
        }
        public MDataGridViewNumericColumn(string name, string caption, string dataPropertyName)
            : base(new MNumericCell())
        {
            this.Name = name;
            this.HeaderText = caption;
            this.DataPropertyName = dataPropertyName;
        }

        public decimal MaximumValue
        {
            get { return _mx_val; }
            set { _mx_val = value; }
        }

        public decimal MinimumValue
        {
            get { return _mn_val; }
            set { _mn_val = value; }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(MNumericCell)))
                {
                    throw new InvalidCastException("Must be a MNumericCell");
                }
                base.CellTemplate = value;
            }
        }

    }

    public class MNumericCell : DataGridViewTextBoxCell
    {
        public MNumericCell()
            : base()
        { }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            NumericEditingControl ctl =
                DataGridView.EditingControl as NumericEditingControl;
            // Use the default row value when Value property is null.
            ctl.DecimalPlaces = ((MDataGridViewNumericColumn)this.OwningColumn).DecimalPlaces;
            ctl.MaximumValue = ((MDataGridViewNumericColumn)this.OwningColumn).MaximumValue;
            ctl.MinimumValue = ((MDataGridViewNumericColumn)this.OwningColumn).MinimumValue;
            decimal a;
            if (decimal.TryParse(this.FormattedValue.ToString(), out a))
                ctl.Value = a;
            else
                ctl.Value = null;
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that Cell uses.
                return typeof(NumericEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that Cell contains.
                return base.ValueType;
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return DBNull.Value;
            }
        }

        class NumericEditingControl : MNumericUpDown, IDataGridViewEditingControl
        {
            private bool valueChanged = false;

            public NumericEditingControl()
            {

            }

            public DataGridView EditingControlDataGridView
            {
                get;
                set;
            }

            public object EditingControlFormattedValue
            {
                get
                {
                    if (this.Value.HasValue)
                        return this.Value.ToString();
                    else return DBNull.Value.ToString();
                }
                set
                {
                    if (value != DBNull.Value)
                        this.Value = decimal.Parse(value.ToString());
                    else
                        this.Value = null;
                }
            }

            public int EditingControlRowIndex
            {
                get;
                set;
            }

            public bool EditingControlValueChanged
            {
                get { return valueChanged; }
                set { valueChanged = value; }
            }

            public Cursor EditingPanelCursor
            {
                get { return base.Cursor; }
            }

            public bool RepositionEditingControlOnValueChange
            {
                get { return false; }
            }

            public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dgStyle)
            {
                this.Font = dgStyle.Font;
                this.ForeColor = dgStyle.ForeColor;
                this.BackColor = dgStyle.BackColor;
            }

            public bool EditingControlWantsInputKey(Keys key, bool DataGridViewWantsInputKey)
            {
                switch (key)
                {
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down: return true;
                    case Keys.Enter:
                    case Keys.Escape: return false;
                    default: return !DataGridViewWantsInputKey;
                }
            }

            public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            public void PrepareEditingControlForEdit(bool SelectAll)
            {
                /*ничего не буду делать*/
            }

            protected override void OnValueChanged(EventArgs e)
            {
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(e);
            }

        }

        [DefaultBindingProperty("Value"), DefaultEvent("ValueChanged")]
        public class MNumericUpDown : NumericUpDown
        {
            private int decimalPlaces;
            private decimal _max_val, _min_val;
            private decimal? curVal = null;
            private bool currentValueChanged = false;
            //private new bool UserEdit=false;

            [Bindable(true)]
            public new decimal? Value
            {
                get
                {
                    return curVal;
                }
                set
                {
                    if (value != null)
                        value = CheckConstraints(value.Value);
                    if (this.curVal != value)
                    {
                        if (value.HasValue)
                            this.curVal = Decimal.Parse(GetNumberText(value.Value));
                        else curVal = null;
                        OnValueChanged(EventArgs.Empty);
                        currentValueChanged = true;
                        if (!UserEdit) UpdateEditText();
                    }
                }
            }

            public new int DecimalPlaces
            {
                get
                {
                    return decimalPlaces;
                }
                set
                {
                    if (value >= 0 && value <= 99)
                    {
                        decimalPlaces = value;
                        UpdateEditText();
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("DecimalPlaces", "Недопустимое количество знаков после запятой");
                    }
                }
            }

            private string GetNumberText(decimal num)
            {
                string result;
                if (this.Hexadecimal)
                {
                    result = ((long)num).ToString("X", CultureInfo.InvariantCulture);
                }
                else
                {
                    result = num.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                }
                return result;
            }

            private decimal CheckConstraints(decimal a)
            {
                if (a > _max_val) return this._max_val;
                else if (a < this._min_val) return this._min_val;
                else return a;
            }
            protected override void UpdateEditText()
            {
                currentValueChanged = false;
                if (this.Value.HasValue)
                    this.Text = this.GetNumberText(this.curVal.Value);
                else
                    this.Text = "";
            }
            protected new void ParseEditText()
            {
                try
                {
                    if (string.IsNullOrEmpty(this.Text))
                        this.Value = null;
                    else
                        if ((this.Text.Length != 1 || !(this.Text == "-")))
                        {
                            if (this.Hexadecimal)
                            {
                                this.Value = Convert.ToDecimal(Convert.ToInt32(this.Text, 16));
                            }
                            else
                            {
                                this.Value = decimal.Parse(this.Text, CultureInfo.CurrentCulture);
                            }
                        }
                }
                catch
                {
                }
                finally
                {
                    UserEdit = false;
                }

            }
            protected override void OnTextBoxTextChanged(object source, EventArgs e)
            {
                if (UserEdit)
                {
                    ParseEditText();
                }
            }
            protected override void OnTextBoxKeyDown(object source, KeyEventArgs e)
            {
                base.OnTextBoxKeyDown(source, e);
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    UserEdit = true;
            }
            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                UserEdit = true;
                if (e.KeyChar == (char)Keys.Enter)
                {
                    UserEdit = false;
                    ParseEditText();
                    UpdateEditText();
                }
                base.OnKeyPress(e);
            }
            protected override void OnValidating(CancelEventArgs e)
            {
                if (UserEdit)
                {
                    ParseEditText();
                    UpdateEditText();
                }
                base.OnValidating(e);
            }

            [DefaultValue(((double)Decimal.MaxValue))]
            public decimal MaximumValue
            {
                get { return _max_val; }
                set
                {
                    _max_val = value;
                    if (this.curVal != null)
                        this.Value = CheckConstraints(curVal.Value);
                }

            }

            [DefaultValue(((double)Decimal.MinValue))]
            public decimal MinimumValue
            {
                get { return _min_val; }
                set
                {
                    _min_val = value;
                    if (this.curVal != null)
                        this.Value = CheckConstraints(curVal.Value);
                }

            }
            public new event EventHandler ValueChanged;

            protected override void OnValueChanged(EventArgs e)
            {
                if (this.ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }
            public MNumericUpDown()
                : base()
            {
                base.Minimum = decimal.MinValue;
                base.Maximum = decimal.MaxValue;
            }

            public override void UpButton()
            {
                base.UpButton();
                decimal a = this.curVal ?? 0;
                try
                {
                    a += this.Increment;
                }
                catch (OverflowException)
                {
                    a = decimal.MaxValue;
                }
                this.Value = a;
            }
            public override void DownButton()
            {
                base.DownButton();
                decimal a = this.curVal ?? 0;
                try
                {
                    a -= this.Increment;
                }
                catch (OverflowException)
                {
                    a = decimal.MinValue;
                }
                this.Value = a;
            }

        }
    }
    #endregion


   [Serializable]
    public class MyComparerString : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.ToUpper() == y.ToUpper();
        }
        public int GetHashCode(string s)
        {
            return s.ToUpper().GetHashCode();
        }
    }
   [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
   public class ComboBoxGrid : ComboBox
   {
       DataGridView d;
       private bool isshowing = false;
       ToolStripControlHost host;
       ToolStripDropDown t = new ToolStripDropDown();
       private object dataSource;
       private Dictionary<string, bool> columnVis = new Dictionary<string, bool>(new MyComparerString());
       public ComboBoxGrid()
       {
           d = new DataGridView();
           d.BackgroundColor = Color.White;
           d.AllowUserToAddRows = false;
           d.AllowUserToDeleteRows = false;
           d.AutoGenerateColumns = true;
           d.RowHeadersVisible = false;
           d.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
           d.ReadOnly = true;
           d.AllowUserToResizeRows = false;
           host = new ToolStripControlHost(d);
           host.AutoSize = false;
           d.ScrollBars = ScrollBars.Both;
           d.MultiSelect = false;
           d.CellMouseClick -= new DataGridViewCellMouseEventHandler(ValueGridSelected);
           d.CellMouseClick += new DataGridViewCellMouseEventHandler(ValueGridSelected);
           t.Items.Add(host);
           t.AutoClose = false;
       }
       /// <summary>
       /// Аналог DataSource
       /// </summary>
       public object DataDispalayed
       {
           set
           {
               dataSource = value;
               this.DataSource = value;
           }
       }
       public DataGridView DataGridView
       {
           get
           {
               return host.Control as DataGridView;
           }
           set
           {
               d = value;
               d.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
               d.AllowUserToResizeRows = false;
               d.ScrollBars = ScrollBars.Both;
               d.MultiSelect = false;
               d.CellMouseClick -= new DataGridViewCellMouseEventHandler(ValueGridSelected);
               d.CellMouseClick += new DataGridViewCellMouseEventHandler(ValueGridSelected);
           }
       }

       public bool IsStripShowing
       {
           get { return isshowing; }
           set { isshowing = value; }
       }

       public Dictionary<string, bool> ColumnsVisible
       {
           get { return columnVis; }
           set { columnVis = value; }
       }

       private void ShowDropDown(string find_string)
       {
           d.Rows.Clear();
           d.Columns.Clear();
           DataTable dt;
           if (find_string != "" && (this.DisplayMember ?? "") != "")
           {
               dt = ((DataTable)dataSource).Clone();
               foreach (DataRow r in ((DataTable)dataSource).Select(this.DisplayMember + " LIKE '" + find_string.ToUpper() + "*'"))
                   dt.ImportRow(r);
           }
           else dt = (DataTable)dataSource;
           for (int i = 0; i < dt.Columns.Count; ++i)
           {
               d.Columns.Add(dt.Columns[i].ColumnName, dt.Columns[i].Caption);
               if (ColumnsVisible.ContainsKey(dt.Columns[i].ColumnName))
                   d.Columns[i].Visible = columnVis[dt.Columns[i].ColumnName];
           }
           for (int i = 0; i < dt.Rows.Count; ++i)
           {
               d.Rows.Add(dt.Rows[i].ItemArray);
               if ((this.ValueMember ?? "") != "" && dt.Rows[i][this.ValueMember].Equals(this.SelectedValue))
               {
                   d.Rows[i].Selected = true;
                   d.FirstDisplayedScrollingRowIndex = i;
               }
           }
           host.Height = this.DropDownHeight;
           host.Width = this.DropDownWidth;
           t.AutoClose = false;
       }

       private const int CBN_DROPDOWN = 0x0201;
       private static int HIWORD(int n)
       {
           return (n >> 16) & 0xffff;
       }
       protected override void WndProc(ref Message m)
       {
           /*if (m.Msg == (WM_REFLECT + WM_COMMAND))
           {*/
           if (m.Msg == CBN_DROPDOWN)
           {
               ShowDropDown("");
               t.Show(this, 0, this.Height);
               return;
           }
           //}
           base.WndProc(ref m);
       }

       protected override void OnKeyUp(KeyEventArgs e)
       {
           base.OnKeyUp(e);
           ShowDropDown(this.Text);
       }

       protected override void OnLostFocus(EventArgs e)
       {
           if (!(this.Focused || t.Focused))
           {
               t.AutoClose = true;
               t.Close();
           }
           base.OnLostFocus(e);
       }

       private void ValueGridSelected(object sender, DataGridViewCellMouseEventArgs e)
       {
           if ((this.ValueMember ?? "") != "" && e.ColumnIndex > -1 && e.RowIndex > -1)
           {
               try
               {
                   this.SelectedValue = d[this.ValueMember, e.RowIndex].Value;
                   t.AutoClose = true;
                   t.Hide();
               }
               catch (Exception ex)
               {
                   new ToolTip().Show("Невозможно выбрать данную строку, уточните ошибку у разработчиков.\n" + ex.Message, this, 0, 0, 3000);
               }
           }
       }
   }
   
}