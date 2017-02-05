namespace VacationSchedule
{
    partial class EditVac
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditVac));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbRecalcPeriods = new System.Windows.Forms.CheckBox();
            this.group_plan = new System.Windows.Forms.GroupBox();
            this.gridPlanVac = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtAddPlanVac = new System.Windows.Forms.ToolStripButton();
            this.tsbtDelPlanVac = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.plan_begin = new System.Windows.Forms.MaskedTextBox();
            this.plan_end = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btExit = new System.Windows.Forms.Button();
            this.btSaveVS = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stripEditVs = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.DATE_CONFIRM = new System.Windows.Forms.ToolStripTextBox();
            this.tscbConfirmSign = new LibraryKadr.ToolStripCheckBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.DATE_CLOSE = new System.Windows.Forms.ToolStripTextBox();
            this.tscbCloseSign = new LibraryKadr.ToolStripCheckBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.group_actual = new System.Windows.Forms.GroupBox();
            this.gridActualVac = new System.Windows.Forms.DataGridView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsbtAddActualVac = new System.Windows.Forms.ToolStripButton();
            this.tsbtDelActualVac = new System.Windows.Forms.ToolStripButton();
            this.tsbtPlanVsToFactVS = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.actual_end = new System.Windows.Forms.MaskedTextBox();
            this.actual_begin = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.group_changes = new System.Windows.Forms.GroupBox();
            this.COMMENTS = new System.Windows.Forms.TextBox();
            this.UNUSED_DAYS = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.group_plan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPlanVac)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.stripEditVs.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.group_actual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridActualVac)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.group_changes.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.toolTip1.AutomaticDelay = 50;
            this.toolTip1.AutoPopDelay = 1000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 10;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Подсказка";
            // 
            // cbRecalcPeriods
            // 
            this.cbRecalcPeriods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRecalcPeriods.AutoSize = true;
            this.cbRecalcPeriods.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbRecalcPeriods.Checked = true;
            this.cbRecalcPeriods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRecalcPeriods.Location = new System.Drawing.Point(329, 7);
            this.cbRecalcPeriods.Name = "cbRecalcPeriods";
            this.cbRecalcPeriods.Size = new System.Drawing.Size(233, 17);
            this.cbRecalcPeriods.TabIndex = 24;
            this.cbRecalcPeriods.Text = "Пересчитать периоды после сохранения";
            this.toolTip1.SetToolTip(this.cbRecalcPeriods, "Пересчитать периоды текущего отпуска после его сохранения в базе данных");
            this.cbRecalcPeriods.UseVisualStyleBackColor = true;
            // 
            // group_plan
            // 
            this.group_plan.BackColor = System.Drawing.Color.Bisque;
            this.group_plan.Controls.Add(this.gridPlanVac);
            this.group_plan.Controls.Add(this.toolStrip1);
            this.group_plan.Controls.Add(this.panel3);
            this.group_plan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group_plan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.group_plan.Location = new System.Drawing.Point(0, 0);
            this.group_plan.Name = "group_plan";
            this.group_plan.Padding = new System.Windows.Forms.Padding(5);
            this.group_plan.Size = new System.Drawing.Size(807, 179);
            this.group_plan.TabIndex = 0;
            this.group_plan.TabStop = false;
            this.group_plan.Text = "Плановый отпуск:";
            // 
            // gridPlanVac
            // 
            this.gridPlanVac.AllowUserToAddRows = false;
            this.gridPlanVac.AllowUserToDeleteRows = false;
            this.gridPlanVac.AllowUserToResizeRows = false;
            this.gridPlanVac.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPlanVac.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridPlanVac.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPlanVac.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridPlanVac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPlanVac.Location = new System.Drawing.Point(29, 44);
            this.gridPlanVac.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
            this.gridPlanVac.MultiSelect = false;
            this.gridPlanVac.Name = "gridPlanVac";
            this.gridPlanVac.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridPlanVac.RowHeadersWidth = 15;
            this.gridPlanVac.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridPlanVac.Size = new System.Drawing.Size(773, 130);
            this.gridPlanVac.TabIndex = 10;
            this.gridPlanVac.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridPlanVac_DataError);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtAddPlanVac,
            this.tsbtDelPlanVac});
            this.toolStrip1.Location = new System.Drawing.Point(5, 44);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(24, 130);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtAddPlanVac
            // 
            this.tsbtAddPlanVac.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtAddPlanVac.Image = ((System.Drawing.Image)(resources.GetObject("tsbtAddPlanVac.Image")));
            this.tsbtAddPlanVac.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtAddPlanVac.Name = "tsbtAddPlanVac";
            this.tsbtAddPlanVac.Size = new System.Drawing.Size(21, 20);
            this.tsbtAddPlanVac.Text = "toolStripButton1";
            this.tsbtAddPlanVac.ToolTipText = "Добавить дни к отпуску";
            this.tsbtAddPlanVac.Click += new System.EventHandler(this.tsbtAddPlanVac_Click);
            // 
            // tsbtDelPlanVac
            // 
            this.tsbtDelPlanVac.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtDelPlanVac.Image = ((System.Drawing.Image)(resources.GetObject("tsbtDelPlanVac.Image")));
            this.tsbtDelPlanVac.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtDelPlanVac.Name = "tsbtDelPlanVac";
            this.tsbtDelPlanVac.Size = new System.Drawing.Size(21, 20);
            this.tsbtDelPlanVac.Text = "toolStripButton2";
            this.tsbtDelPlanVac.ToolTipText = "Удалить строку";
            this.tsbtDelPlanVac.Click += new System.EventHandler(this.tsbtDelPlanVac_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.plan_begin);
            this.panel3.Controls.Add(this.plan_end);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 19);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(797, 25);
            this.panel3.TabIndex = 13;
            // 
            // plan_begin
            // 
            this.plan_begin.Location = new System.Drawing.Point(201, 2);
            this.plan_begin.Mask = "00-00-0000";
            this.plan_begin.Name = "plan_begin";
            this.plan_begin.ResetOnSpace = false;
            this.plan_begin.Size = new System.Drawing.Size(90, 21);
            this.plan_begin.TabIndex = 1;
            // 
            // plan_end
            // 
            this.plan_end.Enabled = false;
            this.plan_end.Location = new System.Drawing.Point(323, 2);
            this.plan_end.Mask = "00-00-0000";
            this.plan_end.Name = "plan_end";
            this.plan_end.ResetOnSpace = false;
            this.plan_end.Size = new System.Drawing.Size(91, 21);
            this.plan_end.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "ПО";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Планируемые даты отпуска С";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.CausesValidation = false;
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.Blue;
            this.btExit.Location = new System.Drawing.Point(694, 4);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(99, 23);
            this.btExit.TabIndex = 23;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // btSaveVS
            // 
            this.btSaveVS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveVS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSaveVS.ForeColor = System.Drawing.Color.Blue;
            this.btSaveVS.Location = new System.Drawing.Point(585, 4);
            this.btSaveVS.Name = "btSaveVS";
            this.btSaveVS.Size = new System.Drawing.Size(99, 23);
            this.btSaveVS.TabIndex = 22;
            this.btSaveVS.Text = "Сохранить";
            this.btSaveVS.UseVisualStyleBackColor = true;
            this.btSaveVS.Click += new System.EventHandler(this.btSaveVS_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.cbRecalcPeriods);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSaveVS);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(807, 30);
            this.panel1.TabIndex = 27;
            // 
            // stripEditVs
            // 
            this.stripEditVs.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.stripEditVs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.DATE_CONFIRM,
            this.tscbConfirmSign,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.DATE_CLOSE,
            this.tscbCloseSign});
            this.stripEditVs.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.stripEditVs.Location = new System.Drawing.Point(0, 0);
            this.stripEditVs.Name = "stripEditVs";
            this.stripEditVs.Size = new System.Drawing.Size(807, 25);
            this.stripEditVs.TabIndex = 28;
            this.stripEditVs.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(122, 22);
            this.toolStripLabel1.Text = "Дата утверждения:";
            // 
            // DATE_CONFIRM
            // 
            this.DATE_CONFIRM.AutoSize = false;
            this.DATE_CONFIRM.BackColor = System.Drawing.Color.White;
            this.DATE_CONFIRM.Enabled = false;
            this.DATE_CONFIRM.Name = "DATE_CONFIRM";
            this.DATE_CONFIRM.Size = new System.Drawing.Size(80, 25);
            // 
            // tscbConfirmSign
            // 
            this.tscbConfirmSign.BackColor = System.Drawing.Color.Transparent;
            this.tscbConfirmSign.Checked = false;
            this.tscbConfirmSign.Name = "tscbConfirmSign";
            this.tscbConfirmSign.Size = new System.Drawing.Size(123, 22);
            this.tscbConfirmSign.Text = "Отпуск утвержден";
            this.tscbConfirmSign.ToolTipText = "Признак согласованности отпуска";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(8, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(102, 22);
            this.toolStripLabel2.Text = "Дата закрытия:";
            // 
            // DATE_CLOSE
            // 
            this.DATE_CLOSE.AutoSize = false;
            this.DATE_CLOSE.BackColor = System.Drawing.Color.White;
            this.DATE_CLOSE.Enabled = false;
            this.DATE_CLOSE.Name = "DATE_CLOSE";
            this.DATE_CLOSE.Size = new System.Drawing.Size(80, 25);
            // 
            // tscbCloseSign
            // 
            this.tscbCloseSign.BackColor = System.Drawing.Color.Transparent;
            this.tscbCloseSign.Checked = false;
            this.tscbCloseSign.Name = "tscbCloseSign";
            this.tscbCloseSign.Size = new System.Drawing.Size(103, 22);
            this.tscbCloseSign.Text = "Отпуск закрыт";
            this.tscbCloseSign.ToolTipText = "Признак закрытия отпуска";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.group_plan);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.group_actual);
            this.splitContainer.Size = new System.Drawing.Size(807, 368);
            this.splitContainer.SplitterDistance = 179;
            this.splitContainer.TabIndex = 29;
            // 
            // group_actual
            // 
            this.group_actual.BackColor = System.Drawing.Color.Honeydew;
            this.group_actual.Controls.Add(this.gridActualVac);
            this.group_actual.Controls.Add(this.toolStrip2);
            this.group_actual.Controls.Add(this.panel2);
            this.group_actual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group_actual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.group_actual.Location = new System.Drawing.Point(0, 0);
            this.group_actual.Name = "group_actual";
            this.group_actual.Padding = new System.Windows.Forms.Padding(5);
            this.group_actual.Size = new System.Drawing.Size(807, 185);
            this.group_actual.TabIndex = 4;
            this.group_actual.TabStop = false;
            this.group_actual.Text = "Фактический отпуск:";
            // 
            // gridActualVac
            // 
            this.gridActualVac.AllowUserToAddRows = false;
            this.gridActualVac.AllowUserToDeleteRows = false;
            this.gridActualVac.AllowUserToResizeRows = false;
            this.gridActualVac.BackgroundColor = System.Drawing.Color.White;
            this.gridActualVac.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridActualVac.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridActualVac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridActualVac.Location = new System.Drawing.Point(29, 46);
            this.gridActualVac.MultiSelect = false;
            this.gridActualVac.Name = "gridActualVac";
            this.gridActualVac.RowHeadersWidth = 15;
            this.gridActualVac.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridActualVac.Size = new System.Drawing.Size(773, 134);
            this.gridActualVac.TabIndex = 14;
            this.gridActualVac.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridPlanVac_DataError);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtAddActualVac,
            this.tsbtDelActualVac,
            this.tsbtPlanVsToFactVS});
            this.toolStrip2.Location = new System.Drawing.Point(5, 46);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(24, 134);
            this.toolStrip2.TabIndex = 13;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsbtAddActualVac
            // 
            this.tsbtAddActualVac.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtAddActualVac.Image = ((System.Drawing.Image)(resources.GetObject("tsbtAddActualVac.Image")));
            this.tsbtAddActualVac.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtAddActualVac.Name = "tsbtAddActualVac";
            this.tsbtAddActualVac.Size = new System.Drawing.Size(21, 20);
            this.tsbtAddActualVac.Text = "toolStripButton1";
            this.tsbtAddActualVac.ToolTipText = "Добавить дни к отпуску";
            this.tsbtAddActualVac.Click += new System.EventHandler(this.tsbtAddActualVac_Click);
            // 
            // tsbtDelActualVac
            // 
            this.tsbtDelActualVac.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtDelActualVac.Image = ((System.Drawing.Image)(resources.GetObject("tsbtDelActualVac.Image")));
            this.tsbtDelActualVac.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtDelActualVac.Name = "tsbtDelActualVac";
            this.tsbtDelActualVac.Size = new System.Drawing.Size(21, 20);
            this.tsbtDelActualVac.Text = "toolStripButton2";
            this.tsbtDelActualVac.ToolTipText = "Удалить строку";
            this.tsbtDelActualVac.Click += new System.EventHandler(this.tsbtDelActualVac_Click);
            // 
            // tsbtPlanVsToFactVS
            // 
            this.tsbtPlanVsToFactVS.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsbtPlanVsToFactVS.Image = ((System.Drawing.Image)(resources.GetObject("tsbtPlanVsToFactVS.Image")));
            this.tsbtPlanVsToFactVS.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsbtPlanVsToFactVS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtPlanVsToFactVS.Name = "tsbtPlanVsToFactVS";
            this.tsbtPlanVsToFactVS.Size = new System.Drawing.Size(21, 85);
            this.tsbtPlanVsToFactVS.Text = "ИЗ ПЛАНА";
            this.tsbtPlanVsToFactVS.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            this.tsbtPlanVsToFactVS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbtPlanVsToFactVS.ToolTipText = "Занести фактический отпуск из планового";
            this.tsbtPlanVsToFactVS.Click += new System.EventHandler(this.tsbtPlanVsToFactVS_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.actual_end);
            this.panel2.Controls.Add(this.actual_begin);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(797, 27);
            this.panel2.TabIndex = 12;
            // 
            // actual_end
            // 
            this.actual_end.Enabled = false;
            this.actual_end.Location = new System.Drawing.Point(323, 2);
            this.actual_end.Mask = "00-00-0000";
            this.actual_end.Name = "actual_end";
            this.actual_end.ResetOnSpace = false;
            this.actual_end.Size = new System.Drawing.Size(91, 21);
            this.actual_end.TabIndex = 11;
            // 
            // actual_begin
            // 
            this.actual_begin.Location = new System.Drawing.Point(201, 2);
            this.actual_begin.Mask = "00-00-0000";
            this.actual_begin.Name = "actual_begin";
            this.actual_begin.ResetOnSpace = false;
            this.actual_begin.Size = new System.Drawing.Size(90, 21);
            this.actual_begin.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(295, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "ПО";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Фактические даты отпуска С";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // group_changes
            // 
            this.group_changes.Controls.Add(this.COMMENTS);
            this.group_changes.Controls.Add(this.UNUSED_DAYS);
            this.group_changes.Controls.Add(this.label8);
            this.group_changes.Controls.Add(this.label7);
            this.group_changes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.group_changes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.group_changes.Location = new System.Drawing.Point(0, 393);
            this.group_changes.Name = "group_changes";
            this.group_changes.Size = new System.Drawing.Size(807, 68);
            this.group_changes.TabIndex = 0;
            this.group_changes.TabStop = false;
            this.group_changes.Text = "Изменения отпуска:";
            // 
            // COMMENTS
            // 
            this.COMMENTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.COMMENTS.Location = new System.Drawing.Point(103, 19);
            this.COMMENTS.MaxLength = 100;
            this.COMMENTS.Multiline = true;
            this.COMMENTS.Name = "COMMENTS";
            this.COMMENTS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.COMMENTS.Size = new System.Drawing.Size(420, 45);
            this.COMMENTS.TabIndex = 21;
            // 
            // UNUSED_DAYS
            // 
            this.UNUSED_DAYS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UNUSED_DAYS.Location = new System.Drawing.Point(710, 43);
            this.UNUSED_DAYS.Mask = "0000";
            this.UNUSED_DAYS.Name = "UNUSED_DAYS";
            this.UNUSED_DAYS.Size = new System.Drawing.Size(47, 21);
            this.UNUSED_DAYS.TabIndex = 20;
            this.UNUSED_DAYS.ValidatingType = typeof(int);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(6, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Примечание:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(521, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(184, 48);
            this.label7.TabIndex = 0;
            this.label7.Text = "Количество неиспользованных дней отпуска:\r\n";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EditVac
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(807, 491);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.stripEditVs);
            this.Controls.Add(this.group_changes);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditVac";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.group_plan.ResumeLayout(false);
            this.group_plan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPlanVac)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.stripEditVs.ResumeLayout(false);
            this.stripEditVs.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.group_actual.ResumeLayout(false);
            this.group_actual.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridActualVac)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.group_changes.ResumeLayout(false);
            this.group_changes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btSaveVS;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox group_plan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox plan_end;
        private System.Windows.Forms.MaskedTextBox plan_begin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip stripEditVs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox DATE_CONFIRM;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox DATE_CLOSE;
        private System.Windows.Forms.DataGridView gridPlanVac;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtAddPlanVac;
        private System.Windows.Forms.ToolStripButton tsbtDelPlanVac;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox group_actual;
        private System.Windows.Forms.DataGridView gridActualVac;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbtAddActualVac;
        private System.Windows.Forms.ToolStripButton tsbtDelActualVac;
        private System.Windows.Forms.MaskedTextBox actual_end;
        private System.Windows.Forms.MaskedTextBox actual_begin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox group_changes;
        private System.Windows.Forms.TextBox COMMENTS;
        private System.Windows.Forms.MaskedTextBox UNUSED_DAYS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripButton tsbtPlanVsToFactVS;
        private LibraryKadr.ToolStripCheckBox tscbConfirmSign;
        private LibraryKadr.ToolStripCheckBox tscbCloseSign;
        private System.Windows.Forms.CheckBox cbRecalcPeriods;
    }
}