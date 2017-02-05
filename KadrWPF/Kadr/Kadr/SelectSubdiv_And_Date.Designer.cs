namespace Kadr
{
    partial class SelectSubdiv_And_Date
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
            this.dpDate = new System.Windows.Forms.DateTimePicker();
            this.lbCaption_Date = new System.Windows.Forms.Label();
            this.ssSubdivForRep = new Kadr.Classes.SubdivSelector();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btCancel = new Elegant.Ui.Button();
            this.btSelect = new Elegant.Ui.Button();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.gbReg_Doc.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbReg_Doc
            // 
            this.gbReg_Doc.Controls.Add(this.dpDate);
            this.gbReg_Doc.Controls.Add(this.lbCaption_Date);
            this.gbReg_Doc.Controls.Add(this.ssSubdivForRep);
            this.gbReg_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReg_Doc.Location = new System.Drawing.Point(0, 0);
            this.gbReg_Doc.Name = "gbReg_Doc";
            this.gbReg_Doc.Size = new System.Drawing.Size(505, 88);
            this.gbReg_Doc.TabIndex = 0;
            this.gbReg_Doc.TabStop = false;
            // 
            // dpDate
            // 
            this.dpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpDate.Location = new System.Drawing.Point(375, 54);
            this.dpDate.Name = "dpDate";
            this.dpDate.Size = new System.Drawing.Size(108, 20);
            this.dpDate.TabIndex = 31;
            // 
            // lbCaption_Date
            // 
            this.lbCaption_Date.AutoSize = true;
            this.lbCaption_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbCaption_Date.Location = new System.Drawing.Point(22, 56);
            this.lbCaption_Date.Name = "lbCaption_Date";
            this.lbCaption_Date.Size = new System.Drawing.Size(329, 15);
            this.lbCaption_Date.TabIndex = 30;
            this.lbCaption_Date.Text = "Стоимость тарифного коэффициента считать на дату";
            this.lbCaption_Date.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ssSubdivForRep
            // 
            this.ssSubdivForRep.BackColor = System.Drawing.Color.Transparent;
            this.ssSubdivForRep.ByRule = "KADR";
            this.ssSubdivForRep.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ssSubdivForRep.IsAllPlantSelectable = true;
            this.ssSubdivForRep.Location = new System.Drawing.Point(22, 19);
            this.ssSubdivForRep.Name = "ssSubdivForRep";
            this.ssSubdivForRep.Size = new System.Drawing.Size(461, 21);
            this.ssSubdivForRep.subdiv_id = null;
            this.ssSubdivForRep.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(505, 37);
            this.panel1.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.Id = "0a74c381-0bac-4521-a60e-acc324aabb57";
            this.btCancel.Location = new System.Drawing.Point(408, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Отмена";
            // 
            // btSelect
            // 
            this.btSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSelect.Id = "43d431e9-7653-45f4-a759-d49bea521d17";
            this.btSelect.Location = new System.Drawing.Point(296, 8);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(104, 23);
            this.btSelect.TabIndex = 0;
            this.btSelect.Text = "Продолжить";
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // SelectSubdiv_And_Date
            // 
            this.AcceptButton = this.btSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 125);
            this.Controls.Add(this.gbReg_Doc);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectSubdiv_And_Date";
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
        private Elegant.Ui.Button btCancel;
        private Elegant.Ui.Button btSelect;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private Kadr.Classes.SubdivSelector ssSubdivForRep;
        private System.Windows.Forms.DateTimePicker dpDate;
        public System.Windows.Forms.Label lbCaption_Date;
    }
}