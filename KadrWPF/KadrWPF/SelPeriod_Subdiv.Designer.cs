namespace Kadr
{
    partial class SelPeriod_Subdiv
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
            this.gbReg_Doc = new System.Windows.Forms.GroupBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.lbYear = new System.Windows.Forms.Label();
            this.de2 = new System.Windows.Forms.DateTimePicker();
            this.de1 = new System.Windows.Forms.DateTimePicker();
            this.cbDoc_List = new System.Windows.Forms.ComboBox();
            this.lbDocs = new System.Windows.Forms.Label();
            this.lbDe2 = new System.Windows.Forms.Label();
            this.lbDe1 = new System.Windows.Forms.Label();
            this.ssSubdivForRep = new Kadr.Classes.SubdivSelector();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSelect = new System.Windows.Forms.Button();
            this.gbReg_Doc.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbReg_Doc
            // 
            this.gbReg_Doc.Controls.Add(this.tbYear);
            this.gbReg_Doc.Controls.Add(this.lbYear);
            this.gbReg_Doc.Controls.Add(this.de2);
            this.gbReg_Doc.Controls.Add(this.de1);
            this.gbReg_Doc.Controls.Add(this.cbDoc_List);
            this.gbReg_Doc.Controls.Add(this.lbDocs);
            this.gbReg_Doc.Controls.Add(this.lbDe2);
            this.gbReg_Doc.Controls.Add(this.lbDe1);
            this.gbReg_Doc.Controls.Add(this.ssSubdivForRep);
            this.gbReg_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReg_Doc.Location = new System.Drawing.Point(0, 0);
            this.gbReg_Doc.Name = "gbReg_Doc";
            this.gbReg_Doc.Size = new System.Drawing.Size(506, 148);
            this.gbReg_Doc.TabIndex = 0;
            this.gbReg_Doc.TabStop = false;
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(122, 120);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(58, 20);
            this.tbYear.TabIndex = 36;
            this.tbYear.Visible = false;
            // 
            // lbYear
            // 
            this.lbYear.AutoSize = true;
            this.lbYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbYear.Location = new System.Drawing.Point(93, 121);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(28, 15);
            this.lbYear.TabIndex = 35;
            this.lbYear.Text = "Год";
            this.lbYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbYear.Visible = false;
            // 
            // de2
            // 
            this.de2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.de2.Location = new System.Drawing.Point(406, 54);
            this.de2.Name = "de2";
            this.de2.Size = new System.Drawing.Size(77, 20);
            this.de2.TabIndex = 34;
            this.de2.Visible = false;
            this.de2.Validating += new System.ComponentModel.CancelEventHandler(this.de2_Validating);
            // 
            // de1
            // 
            this.de1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.de1.Location = new System.Drawing.Point(122, 54);
            this.de1.Name = "de1";
            this.de1.Size = new System.Drawing.Size(77, 20);
            this.de1.TabIndex = 33;
            this.de1.Visible = false;
            this.de1.Validating += new System.ComponentModel.CancelEventHandler(this.de1_Validating);
            // 
            // cbDoc_List
            // 
            this.cbDoc_List.FormattingEnabled = true;
            this.cbDoc_List.Location = new System.Drawing.Point(122, 86);
            this.cbDoc_List.Name = "cbDoc_List";
            this.cbDoc_List.Size = new System.Drawing.Size(361, 21);
            this.cbDoc_List.TabIndex = 3;
            this.cbDoc_List.Visible = false;
            // 
            // lbDocs
            // 
            this.lbDocs.AutoSize = true;
            this.lbDocs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDocs.Location = new System.Drawing.Point(30, 89);
            this.lbDocs.Name = "lbDocs";
            this.lbDocs.Size = new System.Drawing.Size(91, 15);
            this.lbDocs.TabIndex = 32;
            this.lbDocs.Text = "Опр. документ";
            this.lbDocs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbDocs.Visible = false;
            // 
            // lbDe2
            // 
            this.lbDe2.AutoSize = true;
            this.lbDe2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDe2.Location = new System.Drawing.Point(284, 55);
            this.lbDe2.Name = "lbDe2";
            this.lbDe2.Size = new System.Drawing.Size(122, 15);
            this.lbDe2.TabIndex = 31;
            this.lbDe2.Text = "Окончание периода";
            this.lbDe2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbDe2.Visible = false;
            // 
            // lbDe1
            // 
            this.lbDe1.AutoSize = true;
            this.lbDe1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDe1.Location = new System.Drawing.Point(19, 55);
            this.lbDe1.Name = "lbDe1";
            this.lbDe1.Size = new System.Drawing.Size(102, 15);
            this.lbDe1.TabIndex = 30;
            this.lbDe1.Text = "Начало периода";
            this.lbDe1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbDe1.Visible = false;
            // 
            // ssSubdivForRep
            // 
            this.ssSubdivForRep.BackColor = System.Drawing.Color.Transparent;
            this.ssSubdivForRep.ByRule = "TABLE";
            this.ssSubdivForRep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ssSubdivForRep.IsAllPlantSelectable = true;
            this.ssSubdivForRep.Location = new System.Drawing.Point(22, 19);
            this.ssSubdivForRep.Name = "ssSubdivForRep";
            this.ssSubdivForRep.Size = new System.Drawing.Size(461, 21);
            this.ssSubdivForRep.subdiv_id = null;
            this.ssSubdivForRep.TabIndex = 0;
            this.ssSubdivForRep.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 148);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 37);
            this.panel1.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.Location = new System.Drawing.Point(409, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Отмена";
            // 
            // btSelect
            // 
            this.btSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSelect.Location = new System.Drawing.Point(297, 8);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(104, 23);
            this.btSelect.TabIndex = 0;
            this.btSelect.Text = "Продолжить";
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // formFrameSkinner1
            // 
            // 
            // SelPeriod_Subdiv
            // 
            this.AcceptButton = this.btSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 185);
            this.Controls.Add(this.gbReg_Doc);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelPeriod_Subdiv";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Параметры формирования отчета";
            this.gbReg_Doc.ResumeLayout(false);
            this.gbReg_Doc.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbReg_Doc;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSelect;
        private Kadr.Classes.SubdivSelector ssSubdivForRep;
        private System.Windows.Forms.ComboBox cbDoc_List;
        public System.Windows.Forms.Label lbDocs;
        private System.Windows.Forms.Label lbDe1;
        private System.Windows.Forms.Label lbDe2;
        private System.Windows.Forms.DateTimePicker de2;
        private System.Windows.Forms.DateTimePicker de1;
        private System.Windows.Forms.TextBox tbYear;
        public System.Windows.Forms.Label lbYear;
    }
}