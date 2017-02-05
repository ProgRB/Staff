namespace Kadr.Shtat
{
    partial class ReportFilter
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
            this.btNext = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.current_subdiv = new Kadr.Classes.SubdivSelector();
            this.cur_date = new System.Windows.Forms.DateTimePicker();
            this.cbNameDegree = new System.Windows.Forms.ComboBox();
            this.cbCodeDegree = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btNext.ForeColor = System.Drawing.Color.Blue;
            this.btNext.Image = global::Kadr.Properties.Resources.next_gray3232;
            this.btNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btNext.Location = new System.Drawing.Point(231, 6);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(85, 23);
            this.btNext.TabIndex = 5;
            this.btNext.Text = "Далее";
            this.btNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btNext.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.current_subdiv);
            this.groupBox1.Controls.Add(this.cur_date);
            this.groupBox1.Controls.Add(this.cbNameDegree);
            this.groupBox1.Controls.Add(this.cbCodeDegree);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 129);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор категории и подразделения";
            // 
            // current_subdiv
            // 
            this.current_subdiv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.current_subdiv.BackColor = System.Drawing.Color.Transparent;
            this.current_subdiv.ByRule = "";
            this.current_subdiv.Location = new System.Drawing.Point(8, 41);
            this.current_subdiv.Name = "current_subdiv";
            this.current_subdiv.Size = new System.Drawing.Size(395, 21);
            this.current_subdiv.TabIndex = 4;
            // 
            // cur_date
            // 
            this.cur_date.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cur_date.Location = new System.Drawing.Point(95, 97);
            this.cur_date.Name = "cur_date";
            this.cur_date.Size = new System.Drawing.Size(308, 21);
            this.cur_date.TabIndex = 3;
            // 
            // cbNameDegree
            // 
            this.cbNameDegree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNameDegree.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbNameDegree.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbNameDegree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNameDegree.FormattingEnabled = true;
            this.cbNameDegree.Location = new System.Drawing.Point(167, 68);
            this.cbNameDegree.Name = "cbNameDegree";
            this.cbNameDegree.Size = new System.Drawing.Size(236, 23);
            this.cbNameDegree.TabIndex = 2;
            // 
            // cbCodeDegree
            // 
            this.cbCodeDegree.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCodeDegree.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCodeDegree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCodeDegree.FormattingEnabled = true;
            this.cbCodeDegree.Location = new System.Drawing.Point(107, 68);
            this.cbCodeDegree.Name = "cbCodeDegree";
            this.cbCodeDegree.Size = new System.Drawing.Size(57, 23);
            this.cbCodeDegree.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "На дату";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Категория";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btNext);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 35);
            this.panel1.TabIndex = 3;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCancel.Location = new System.Drawing.Point(328, 6);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(76, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // ReportFilter
            // 
            this.AcceptButton = this.btNext;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(415, 164);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportFilter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчеты";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportFilter_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.DateTimePicker cur_date;
        private System.Windows.Forms.ComboBox cbNameDegree;
        private System.Windows.Forms.ComboBox cbCodeDegree;
        private System.Windows.Forms.Label label2;
        private Kadr.Classes.SubdivSelector current_subdiv;
    }
}