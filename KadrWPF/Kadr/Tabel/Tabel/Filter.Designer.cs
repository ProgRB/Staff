namespace Tabel
{
    partial class Filter
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
            this.formFrameSkinner = new Elegant.Ui.FormFrameSkinner();
            this.btExit = new Elegant.Ui.Button();
            this.btFilter = new Elegant.Ui.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.label3 = new Elegant.Ui.Label();
            this.label1 = new Elegant.Ui.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.tbMonth = new System.Windows.Forms.TextBox();
            this.label2 = new Elegant.Ui.Label();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.label4 = new Elegant.Ui.Label();
            this.panel1.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner
            // 
            this.formFrameSkinner.Form = this;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "b06b79e2-5498-4ca7-b984-14c920fbb117";
            this.btExit.Location = new System.Drawing.Point(414, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btFilter
            // 
            this.btFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFilter.Id = "c3c7c666-c095-4a85-ad80-e11290096a35";
            this.btFilter.Location = new System.Drawing.Point(251, 9);
            this.btFilter.Name = "btFilter";
            this.btFilter.Size = new System.Drawing.Size(146, 23);
            this.btFilter.TabIndex = 0;
            this.btFilter.Text = "Применить фильтр";
            this.btFilter.Click += new System.EventHandler(this.btFilter_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btFilter);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(507, 42);
            this.panel1.TabIndex = 1;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.label3);
            this.gbFilter.Controls.Add(this.label4);
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Controls.Add(this.tbYear);
            this.gbFilter.Controls.Add(this.tbMonth);
            this.gbFilter.Controls.Add(this.tbCode_Subdiv);
            this.gbFilter.Controls.Add(this.cbSubdiv_Name);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilter.Location = new System.Drawing.Point(0, 0);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(507, 78);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Id = "b21fa254-393b-4d45-9f9b-bf8560e90b34";
            this.label3.Location = new System.Drawing.Point(17, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Наименование";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Id = "c8fb4977-601e-4f33-b297-9f57a2ef445a";
            this.label1.Location = new System.Drawing.Point(17, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 22);
            this.label1.TabIndex = 12;
            this.label1.Text = "Шифр подразделения";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(170, 18);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(53, 21);
            this.tbCode_Subdiv.TabIndex = 0;
            this.tbCode_Subdiv.Validating += new System.ComponentModel.CancelEventHandler(this.tbCode_Subdiv_Validating);
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Items.AddRange(new object[] {
            "1 отдел",
            "10 отдел",
            "106 отдел",
            "109 отдел",
            "11 отдел",
            "11 цех",
            "110 отдел",
            "111 отдел",
            "114 отдел",
            "116 цех",
            "119 отдел",
            "12 отдел",
            "12 цех",
            "13 отдел",
            "132 отдел",
            "139 отдел",
            "15 отдел",
            "16 отдел",
            "16 цех",
            "17 отдел",
            "17 цех",
            "18 цех",
            "180 отдел",
            "19 отдел",
            "20 отдел",
            "21 отдел",
            "22 отдел",
            "23 отдел",
            "24 цех",
            "27 отдел",
            "28 цех",
            "3 отдел",
            "33 отдел",
            "36 цех",
            "39 отдел",
            "40 цех",
            "41 отдел",
            "43 отдел",
            "45 отдел",
            "49 цех",
            "5 отдел",
            "50 цех",
            "51 цех",
            "53 цех",
            "54 цех",
            "55 цех",
            "6 отдел",
            "60 цех",
            "61 цех",
            "62 отдел",
            "7 отдел",
            "78 отдел",
            "9 отдел",
            "95 цех",
            "ЗАС-133",
            "ЗШЗ-123",
            "ИЗ-30",
            "МСЗ",
            "МСЗ-146",
            "Пуско-наладка системы. Новосибирск.",
            "Котокель\t\t"});
            this.cbSubdiv_Name.Location = new System.Drawing.Point(170, 45);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(319, 23);
            this.cbSubdiv_Name.TabIndex = 3;
            // 
            // tbMonth
            // 
            this.tbMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbMonth.Location = new System.Drawing.Point(310, 18);
            this.tbMonth.Name = "tbMonth";
            this.tbMonth.Size = new System.Drawing.Size(53, 21);
            this.tbMonth.TabIndex = 1;
            this.tbMonth.Validating += new System.ComponentModel.CancelEventHandler(this.tbMonth_Validating);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Id = "ccbe831e-8058-4938-bac2-36d8bb21f7c9";
            this.label2.Location = new System.Drawing.Point(260, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "Месяц";
            // 
            // tbYear
            // 
            this.tbYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbYear.Location = new System.Drawing.Point(436, 18);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(53, 21);
            this.tbYear.TabIndex = 2;
            this.tbYear.Validating += new System.ComponentModel.CancelEventHandler(this.tbYear_Validating);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Id = "6bcf531c-1a0d-4a7f-a6af-2fe1ca237a6f";
            this.label4.Location = new System.Drawing.Point(403, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 22);
            this.label4.TabIndex = 12;
            this.label4.Text = "Год";
            // 
            // Filter
            // 
            this.AcceptButton = this.btFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(507, 120);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Filter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Фильтр";
            this.panel1.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btFilter;
        private System.Windows.Forms.GroupBox gbFilter;
        private Elegant.Ui.Label label1;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Label label3;
        private Elegant.Ui.Label label2;
        private System.Windows.Forms.TextBox tbMonth;
        private Elegant.Ui.Label label4;
        private System.Windows.Forms.TextBox tbYear;
    }
}