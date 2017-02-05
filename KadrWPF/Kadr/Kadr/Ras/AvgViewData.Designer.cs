namespace Kadr.Ras
{
    partial class AvgViewData
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cnt_months = new System.Windows.Forms.ComboBox();
            this.sign_comb = new System.Windows.Forms.CheckBox();
            this.date_calc = new System.Windows.Forms.DateTimePicker();
            this.per_num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.type_avg = new System.Windows.Forms.ComboBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbtShortPrintAVGCalend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtFullPrintAVGCalend = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(251)))), ((int)(((byte)(215)))));
            this.panel1.Controls.Add(this.cnt_months);
            this.panel1.Controls.Add(this.sign_comb);
            this.panel1.Controls.Add(this.date_calc);
            this.panel1.Controls.Add(this.per_num);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.type_avg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(754, 69);
            this.panel1.TabIndex = 1;
            // 
            // cnt_months
            // 
            this.cnt_months.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cnt_months.FormattingEnabled = true;
            this.cnt_months.Items.AddRange(new object[] {
            "3",
            "12"});
            this.cnt_months.Location = new System.Drawing.Point(469, 36);
            this.cnt_months.Name = "cnt_months";
            this.cnt_months.Size = new System.Drawing.Size(67, 21);
            this.cnt_months.TabIndex = 0;
            this.cnt_months.KeyUp += new System.Windows.Forms.KeyEventHandler(this.per_num_KeyUp);
            // 
            // sign_comb
            // 
            this.sign_comb.AutoSize = true;
            this.sign_comb.Location = new System.Drawing.Point(542, 14);
            this.sign_comb.Name = "sign_comb";
            this.sign_comb.Size = new System.Drawing.Size(92, 17);
            this.sign_comb.TabIndex = 5;
            this.sign_comb.Text = "Совмещение";
            this.sign_comb.UseVisualStyleBackColor = true;
            this.sign_comb.KeyUp += new System.Windows.Forms.KeyEventHandler(this.per_num_KeyUp);
            // 
            // date_calc
            // 
            this.date_calc.CustomFormat = "MMMM yyyy";
            this.date_calc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_calc.Location = new System.Drawing.Point(137, 37);
            this.date_calc.Name = "date_calc";
            this.date_calc.Size = new System.Drawing.Size(134, 20);
            this.date_calc.TabIndex = 3;
            this.date_calc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.per_num_KeyUp);
            // 
            // per_num
            // 
            this.per_num.Location = new System.Drawing.Point(469, 12);
            this.per_num.Name = "per_num";
            this.per_num.Size = new System.Drawing.Size(67, 20);
            this.per_num.TabIndex = 2;
            this.per_num.KeyUp += new System.Windows.Forms.KeyEventHandler(this.per_num_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(286, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(177, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Количество месяцев для расчета";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Месяц расчета";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(423, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Таб. №";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выберите тип расчета";
            // 
            // type_avg
            // 
            this.type_avg.FormattingEnabled = true;
            this.type_avg.Location = new System.Drawing.Point(137, 11);
            this.type_avg.Name = "type_avg";
            this.type_avg.Size = new System.Drawing.Size(223, 21);
            this.type_avg.TabIndex = 0;
            this.type_avg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.per_num_KeyUp);
            // 
            // tabs
            // 
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 94);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(754, 341);
            this.tabs.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btRefresh,
            this.toolStripSeparator1,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 69);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(754, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btRefresh
            // 
            this.btRefresh.Image = global::Kadr.Properties.Resources.RefreshDocViewHS;
            this.btRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(141, 22);
            this.btRefresh.Text = "Обновить результаты";
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtShortPrintAVGCalend,
            this.toolStripMenuItem1,
            this.tsbtFullPrintAVGCalend});
            this.toolStripDropDownButton1.Image = global::Kadr.Properties.Resources.Print_Small;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(73, 22);
            this.toolStripDropDownButton1.Text = "Печать";
            // 
            // tsbtShortPrintAVGCalend
            // 
            this.tsbtShortPrintAVGCalend.Name = "tsbtShortPrintAVGCalend";
            this.tsbtShortPrintAVGCalend.Size = new System.Drawing.Size(328, 22);
            this.tsbtShortPrintAVGCalend.Text = "Сокращенная печать рассчетов за (3 и 12 мес.)";
            this.tsbtShortPrintAVGCalend.ToolTipText = "Печать сокращенных рассчетов средней ЗП";
            this.tsbtShortPrintAVGCalend.Click += new System.EventHandler(this.tsbtShortPrintAVGCalend_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(325, 6);
            // 
            // tsbtFullPrintAVGCalend
            // 
            this.tsbtFullPrintAVGCalend.Name = "tsbtFullPrintAVGCalend";
            this.tsbtFullPrintAVGCalend.Size = new System.Drawing.Size(328, 22);
            this.tsbtFullPrintAVGCalend.Text = "Полная печать";
            this.tsbtFullPrintAVGCalend.Click += new System.EventHandler(this.btPrint_Click);
            // 
            // AvgViewData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 435);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "AvgViewData";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр рассчетов справок на средний";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox type_avg;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.DateTimePicker date_calc;
        private System.Windows.Forms.TextBox per_num;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox sign_comb;
        private System.Windows.Forms.ComboBox cnt_months;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tsbtShortPrintAVGCalend;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsbtFullPrintAVGCalend;
    }
}