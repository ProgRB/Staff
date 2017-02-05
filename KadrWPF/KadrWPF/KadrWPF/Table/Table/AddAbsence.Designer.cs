namespace Tabel
{
    partial class AddAbsence
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.gbReg_Doc = new System.Windows.Forms.GroupBox();
            this.deDoc_End = new LibraryKadr.DateEditor();
            this.deDoc_Begin = new LibraryKadr.DateEditor();
            this.label14 = new System.Windows.Forms.Label();
            this.mbDoc_End = new System.Windows.Forms.MaskedTextBox();
            this.mbDoc_Begin = new System.Windows.Forms.MaskedTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbReg_Doc.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 37);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(194, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Location = new System.Drawing.Point(80, 8);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(104, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // gbReg_Doc
            // 
            this.gbReg_Doc.Controls.Add(this.deDoc_End);
            this.gbReg_Doc.Controls.Add(this.deDoc_Begin);
            this.gbReg_Doc.Controls.Add(this.label14);
            this.gbReg_Doc.Controls.Add(this.mbDoc_End);
            this.gbReg_Doc.Controls.Add(this.mbDoc_Begin);
            this.gbReg_Doc.Controls.Add(this.label13);
            this.gbReg_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReg_Doc.Location = new System.Drawing.Point(0, 0);
            this.gbReg_Doc.Name = "gbReg_Doc";
            this.gbReg_Doc.Size = new System.Drawing.Size(286, 101);
            this.gbReg_Doc.TabIndex = 1;
            this.gbReg_Doc.TabStop = false;
            // 
            // deDoc_End
            // 
            this.deDoc_End.AutoSize = true;
            this.deDoc_End.BackColor = System.Drawing.Color.White;
            this.deDoc_End.Date = null;
            this.deDoc_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDoc_End.Location = new System.Drawing.Point(132, 61);
            this.deDoc_End.Name = "deDoc_End";
            this.deDoc_End.ReadOnly = false;
            this.deDoc_End.Size = new System.Drawing.Size(77, 21);
            this.deDoc_End.TabIndex = 5;
            this.deDoc_End.TextDate = null;
            this.deDoc_End.Leave += new System.EventHandler(this.deDoc_End_Leave);
            // 
            // deDoc_Begin
            // 
            this.deDoc_Begin.AutoSize = true;
            this.deDoc_Begin.BackColor = System.Drawing.Color.White;
            this.deDoc_Begin.Date = null;
            this.deDoc_Begin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDoc_Begin.Location = new System.Drawing.Point(132, 19);
            this.deDoc_Begin.Name = "deDoc_Begin";
            this.deDoc_Begin.ReadOnly = false;
            this.deDoc_Begin.Size = new System.Drawing.Size(77, 21);
            this.deDoc_Begin.TabIndex = 3;
            this.deDoc_Begin.TextDate = null;
            this.deDoc_Begin.Leave += new System.EventHandler(this.deDoc_Begin_Leave);
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(20, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 34);
            this.label14.TabIndex = 26;
            this.label14.Text = "Дата и время начала";
            // 
            // mbDoc_End
            // 
            this.mbDoc_End.BackColor = System.Drawing.Color.White;
            this.mbDoc_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDoc_End.Location = new System.Drawing.Point(215, 61);
            this.mbDoc_End.Mask = "00:00";
            this.mbDoc_End.Name = "mbDoc_End";
            this.mbDoc_End.Size = new System.Drawing.Size(46, 21);
            this.mbDoc_End.TabIndex = 6;
            this.mbDoc_End.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mbDoc_End.ValidatingType = typeof(System.DateTime);
            // 
            // mbDoc_Begin
            // 
            this.mbDoc_Begin.BackColor = System.Drawing.Color.White;
            this.mbDoc_Begin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDoc_Begin.Location = new System.Drawing.Point(214, 19);
            this.mbDoc_Begin.Mask = "00:00";
            this.mbDoc_Begin.Name = "mbDoc_Begin";
            this.mbDoc_Begin.Size = new System.Drawing.Size(46, 21);
            this.mbDoc_Begin.TabIndex = 4;
            this.mbDoc_Begin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mbDoc_Begin.ValidatingType = typeof(System.DateTime);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(19, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(99, 34);
            this.label13.TabIndex = 27;
            this.label13.Text = "Дата и время окончания";
            // 
            // AddAbsence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(286, 138);
            this.Controls.Add(this.gbReg_Doc);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddAbsence";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Оправдательные документы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddAbsence_FormClosing);
            this.panel1.ResumeLayout(false);
            this.gbReg_Doc.ResumeLayout(false);
            this.gbReg_Doc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbReg_Doc;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSave;
        private LibraryKadr.DateEditor deDoc_End;
        private LibraryKadr.DateEditor deDoc_Begin;
        private System.Windows.Forms.MaskedTextBox mbDoc_Begin;
        private System.Windows.Forms.MaskedTextBox mbDoc_End;
    }
}