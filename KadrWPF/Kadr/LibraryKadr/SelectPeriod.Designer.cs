namespace LibraryKadr
{
    partial class SelectPeriod
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
            this.btCancel = new Elegant.Ui.Button();
            this.btSelect = new Elegant.Ui.Button();
            this.gbReg_Doc = new System.Windows.Forms.GroupBox();
            this.de1 = new EditorsLibrary.DateEditor();
            this.de2 = new EditorsLibrary.DateEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbReg_Doc.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 37);
            this.panel1.TabIndex = 3;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.Id = "ad183f6b-537e-4c86-9105-6597499ed521";
            this.btCancel.Location = new System.Drawing.Point(151, 8);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Отмена";
            // 
            // btSelect
            // 
            this.btSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSelect.Id = "5fd12b66-a329-42b7-bcf6-da73925c01cd";
            this.btSelect.Location = new System.Drawing.Point(37, 8);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(104, 23);
            this.btSelect.TabIndex = 0;
            this.btSelect.Text = "Продолжить";
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // gbReg_Doc
            // 
            this.gbReg_Doc.Controls.Add(this.de1);
            this.gbReg_Doc.Controls.Add(this.de2);
            this.gbReg_Doc.Controls.Add(this.label1);
            this.gbReg_Doc.Controls.Add(this.label16);
            this.gbReg_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReg_Doc.Location = new System.Drawing.Point(0, 0);
            this.gbReg_Doc.Name = "gbReg_Doc";
            this.gbReg_Doc.Size = new System.Drawing.Size(243, 80);
            this.gbReg_Doc.TabIndex = 4;
            this.gbReg_Doc.TabStop = false;
            // 
            // de1
            // 
            this.de1.AutoSize = true;
            this.de1.BackColor = System.Drawing.Color.White;
            this.de1.Date = null;
            this.de1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.de1.Location = new System.Drawing.Point(159, 20);
            this.de1.Name = "de1";
            this.de1.ReadOnly = false;
            this.de1.Size = new System.Drawing.Size(77, 21);
            this.de1.TabIndex = 1;
            this.de1.TextDate = null;
            this.de1.Validating += new System.ComponentModel.CancelEventHandler(this.de1_Validating);
            // 
            // de2
            // 
            this.de2.AutoSize = true;
            this.de2.BackColor = System.Drawing.Color.White;
            this.de2.Date = null;
            this.de2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.de2.Location = new System.Drawing.Point(159, 47);
            this.de2.Name = "de2";
            this.de2.ReadOnly = false;
            this.de2.Size = new System.Drawing.Size(77, 21);
            this.de2.TabIndex = 1;
            this.de2.TextDate = null;
            this.de2.Validating += new System.ComponentModel.CancelEventHandler(this.de2_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 24;
            this.label1.Text = "Начала периода";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(14, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(139, 15);
            this.label16.TabIndex = 24;
            this.label16.Text = "Окончание периода";
            // 
            // SelectPeriod
            // 
            this.AcceptButton = this.btSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 117);
            this.ControlBox = false;
            this.Controls.Add(this.gbReg_Doc);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SelectPeriod";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор периода";
            this.Activated += new System.EventHandler(this.SelectPeriod_Activated);
            this.panel1.ResumeLayout(false);
            this.gbReg_Doc.ResumeLayout(false);
            this.gbReg_Doc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox gbReg_Doc;
        private EditorsLibrary.DateEditor de1;
        private EditorsLibrary.DateEditor de2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Button btCancel;
        private Elegant.Ui.Button btSelect;
    }
}