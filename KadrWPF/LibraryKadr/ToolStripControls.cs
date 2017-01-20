using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace LibraryKadr
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripDateTimePicker: ToolStripControlHost
    {
        private DateTimePicker dt;
        public ToolStripDateTimePicker()
            : base(new FlowLayoutPanel())
        {
            base.Control.BackColor = System.Drawing.Color.Transparent;
            base.Control.Font = this.Font;
            dt = new DateTimePicker();
            dt.ValueChanged += new EventHandler(OnValueChanged);
            base.Control.Controls.Add(dt);
        }
        public event EventHandler ValueChanged;
        public DateTimePicker DateTimeControl
        {
            get
            {
                return dt;
            }
        }
        public DateTime Value
        {
            get { return DateTimeControl.Value; }
            set { DateTimeControl.Value = value; }
        }
        private void OnValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
       /* protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            try
            {
                ((DateTimePicker)((FlowLayoutPanel)control).Controls[0]).ValueChanged += new EventHandler(OnValueChanged);
            }
            catch
            { 
            }
        }
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            try
            {
                ((DateTimePicker)((FlowLayoutPanel)control).Controls[0]).ValueChanged -= new EventHandler(OnValueChanged);
            }
            catch { }
        }*/
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripCheckBox : ToolStripControlHost
    {
        public ToolStripCheckBox()
            : base(new CheckBox())
        {
            base.Control.BackColor = System.Drawing.Color.Transparent;
        }
        public CheckBox CheckBoxControl
        {
            get { return Control as CheckBox; }
        }

        public bool Checked
        {
            get { return CheckBoxControl.Checked; }
            set { CheckBoxControl.Checked = value; }
        }
        public event EventHandler CheckedChanged;

        public void OnCheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
            {
                CheckedChanged(this, e);
            }
        }

        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            (control as CheckBox).CheckedChanged += OnCheckedChanged;
        }

        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            (control as CheckBox).CheckedChanged -= OnCheckedChanged;
        }
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripNumbericUpDown : ToolStripControlHost
    {
        private NumericUpDown dt;
        public ToolStripNumbericUpDown()
            : base(new FlowLayoutPanel())
        {
            base.Control.BackColor = System.Drawing.Color.Transparent;
            base.Control.Font = this.Font;
            dt = new NumericUpDown();
            dt.ValueChanged += new EventHandler(OnValueChanged);
            base.Control.Controls.Add(dt);
        }
        public event EventHandler ValueChanged;
        public NumericUpDown NumericDownControl
        {
            get
            {
                return dt;
            }
        }
        public Decimal Value
        {
            get { return NumericDownControl.Value; }
            set { NumericDownControl.Value = value; }
        }
        public Decimal Minimun
        {
            get
            {
                return NumericDownControl.Minimum;
            }
            set
            {
                NumericDownControl.Minimum = value;
            }
        }
        public Decimal Maximum
        {
            get
            {
                return NumericDownControl.Maximum;
            }
            set
            {
                NumericDownControl.Maximum = value;
            }
        }
        private void OnValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
    }
}
