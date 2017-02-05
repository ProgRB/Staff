namespace Kadr.Vacation_schedule
{
    partial class Reminder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.button1 = new Elegant.Ui.Button();
            this.label1 = new Elegant.Ui.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grid_alarm = new System.Windows.Forms.DataGridView();
            this.btRefresh = new Elegant.Ui.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_alarm)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Id = "25f72a63-f262-4372-aa70-b938f1efd02c";
            this.button1.Location = new System.Drawing.Point(573, 348);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Закрыть";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = false;
            this.label1.Id = "9b2e2d80-278f-4d60-b626-ac40798e7958";
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(665, 59);
            this.label1.TabIndex = 2;
            this.label1.Text = "У следующих работников намечен планируемый отпуск, но не проставлены фактические " +
                "даты отпуска. Требуется перенести отпуск либо оформить записку-расчет:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grid_alarm);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(6, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(675, 274);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Работники";
            // 
            // grid_alarm
            // 
            this.grid_alarm.AllowUserToAddRows = false;
            this.grid_alarm.AllowUserToDeleteRows = false;
            this.grid_alarm.AllowUserToResizeRows = false;
            this.grid_alarm.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.grid_alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.AppWorkspace;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_alarm.DefaultCellStyle = dataGridViewCellStyle1;
            this.grid_alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_alarm.Location = new System.Drawing.Point(3, 17);
            this.grid_alarm.MultiSelect = false;
            this.grid_alarm.Name = "grid_alarm";
            this.grid_alarm.ReadOnly = true;
            this.grid_alarm.RowHeadersVisible = false;
            this.grid_alarm.ShowEditingIcon = false;
            this.grid_alarm.Size = new System.Drawing.Size(669, 254);
            this.grid_alarm.TabIndex = 0;
            this.grid_alarm.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grid_alarm_ColumnWidthChanged);
            this.grid_alarm.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_alarm_CellContentClick);
            // 
            // btRefresh
            // 
            this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRefresh.Id = "c1815a1a-a796-49e4-8f05-d14b5732859d";
            this.btRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRefresh.Location = new System.Drawing.Point(14, 348);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(102, 30);
            this.btRefresh.SmallImages.Images.AddRange(new Elegant.Ui.ControlImage[] {
            new Elegant.Ui.ControlImage("Normal", global::Kadr.Properties.Resources.arrow_refresh_mini)});
            this.btRefresh.TabIndex = 1;
            this.btRefresh.Text = "Обновить";
            this.btRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // Reminder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(686, 380);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaximizeBox = false;
            this.Name = "Reminder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Напоминание";
            this.Deactivate += new System.EventHandler(this.Reminder_Deactivate);
            this.Activated += new System.EventHandler(this.Reminder_Activated);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_alarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private Elegant.Ui.Label label1;
        private Elegant.Ui.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grid_alarm;
        private Elegant.Ui.Button btRefresh;
    }
}