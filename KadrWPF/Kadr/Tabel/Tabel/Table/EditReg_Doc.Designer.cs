namespace Tabel
{
    partial class EditReg_Doc
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
            this.btExit = new Elegant.Ui.Button();
            this.btSave = new Elegant.Ui.Button();
            this.gbReg_Doc = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.deDoc_End = new EditorsLibrary.DateEditor();
            this.deDoc_Begin = new EditorsLibrary.DateEditor();
            this.deDoc_Date = new EditorsLibrary.DateEditor();
            this.cbDoc_List_Name = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbDoc_Location = new System.Windows.Forms.TextBox();
            this.tbDoc_Number = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.mbDoc_End = new System.Windows.Forms.MaskedTextBox();
            this.mbDoc_Begin = new System.Windows.Forms.MaskedTextBox();
            this.label13 = new System.Windows.Forms.Label();
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
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 166);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 37);
            this.panel1.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "104a1f6b-e965-433e-a619-14d3b2109d43";
            this.btExit.Location = new System.Drawing.Point(474, 8);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Id = "31b31c00-2df5-4d9b-a21d-3e4f2ba77fe8";
            this.btSave.Location = new System.Drawing.Point(360, 8);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(104, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // gbReg_Doc
            // 
            this.gbReg_Doc.Controls.Add(this.label1);
            this.gbReg_Doc.Controls.Add(this.deDoc_End);
            this.gbReg_Doc.Controls.Add(this.deDoc_Begin);
            this.gbReg_Doc.Controls.Add(this.deDoc_Date);
            this.gbReg_Doc.Controls.Add(this.cbDoc_List_Name);
            this.gbReg_Doc.Controls.Add(this.label14);
            this.gbReg_Doc.Controls.Add(this.tbDoc_Location);
            this.gbReg_Doc.Controls.Add(this.tbDoc_Number);
            this.gbReg_Doc.Controls.Add(this.label15);
            this.gbReg_Doc.Controls.Add(this.label17);
            this.gbReg_Doc.Controls.Add(this.label16);
            this.gbReg_Doc.Controls.Add(this.mbDoc_End);
            this.gbReg_Doc.Controls.Add(this.mbDoc_Begin);
            this.gbReg_Doc.Controls.Add(this.label13);
            this.gbReg_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbReg_Doc.Location = new System.Drawing.Point(0, 0);
            this.gbReg_Doc.Name = "gbReg_Doc";
            this.gbReg_Doc.Size = new System.Drawing.Size(566, 166);
            this.gbReg_Doc.TabIndex = 1;
            this.gbReg_Doc.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 35);
            this.label1.TabIndex = 10;
            this.label1.Text = "Место- нахождение";
            // 
            // deDoc_End
            // 
            this.deDoc_End.AutoSize = true;
            this.deDoc_End.BackColor = System.Drawing.Color.White;
            this.deDoc_End.Date = null;
            this.deDoc_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDoc_End.Location = new System.Drawing.Point(419, 81);
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
            this.deDoc_Begin.Location = new System.Drawing.Point(126, 81);
            this.deDoc_Begin.Name = "deDoc_Begin";
            this.deDoc_Begin.ReadOnly = false;
            this.deDoc_Begin.Size = new System.Drawing.Size(77, 21);
            this.deDoc_Begin.TabIndex = 3;
            this.deDoc_Begin.TextDate = null;
            this.deDoc_Begin.Leave += new System.EventHandler(this.deDoc_Begin_Leave);
            // 
            // deDoc_Date
            // 
            this.deDoc_Date.AutoSize = true;
            this.deDoc_Date.BackColor = System.Drawing.Color.White;
            this.deDoc_Date.Date = null;
            this.deDoc_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDoc_Date.Location = new System.Drawing.Point(472, 19);
            this.deDoc_Date.Name = "deDoc_Date";
            this.deDoc_Date.ReadOnly = false;
            this.deDoc_Date.Size = new System.Drawing.Size(77, 21);
            this.deDoc_Date.TabIndex = 1;
            this.deDoc_Date.TextDate = null;
            // 
            // cbDoc_List_Name
            // 
            this.cbDoc_List_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDoc_List_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDoc_List_Name.FormattingEnabled = true;
            this.cbDoc_List_Name.Location = new System.Drawing.Point(126, 46);
            this.cbDoc_List_Name.Name = "cbDoc_List_Name";
            this.cbDoc_List_Name.Size = new System.Drawing.Size(422, 23);
            this.cbDoc_List_Name.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(14, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 34);
            this.label14.TabIndex = 26;
            this.label14.Text = "Дата и время начала";
            // 
            // tbDoc_Location
            // 
            this.tbDoc_Location.BackColor = System.Drawing.Color.White;
            this.tbDoc_Location.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDoc_Location.Location = new System.Drawing.Point(126, 116);
            this.tbDoc_Location.Multiline = true;
            this.tbDoc_Location.Name = "tbDoc_Location";
            this.tbDoc_Location.Size = new System.Drawing.Size(423, 39);
            this.tbDoc_Location.TabIndex = 7;
            // 
            // tbDoc_Number
            // 
            this.tbDoc_Number.BackColor = System.Drawing.Color.White;
            this.tbDoc_Number.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDoc_Number.Location = new System.Drawing.Point(126, 19);
            this.tbDoc_Number.Name = "tbDoc_Number";
            this.tbDoc_Number.Size = new System.Drawing.Size(128, 21);
            this.tbDoc_Number.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(14, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 15);
            this.label15.TabIndex = 25;
            this.label15.Text = "Тип документа";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(14, 22);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(95, 15);
            this.label17.TabIndex = 23;
            this.label17.Text = "№ документа";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(350, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(116, 15);
            this.label16.TabIndex = 24;
            this.label16.Text = "Дата документа";
            // 
            // mbDoc_End
            // 
            this.mbDoc_End.BackColor = System.Drawing.Color.White;
            this.mbDoc_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDoc_End.Location = new System.Drawing.Point(502, 81);
            this.mbDoc_End.Mask = "00:00";
            this.mbDoc_End.Name = "mbDoc_End";
            this.mbDoc_End.Size = new System.Drawing.Size(46, 21);
            this.mbDoc_End.TabIndex = 6;
            this.mbDoc_End.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mbDoc_End.ValidatingType = typeof(System.DateTime);
            this.mbDoc_End.Leave += new System.EventHandler(this.mbDoc_End_Leave);
            // 
            // mbDoc_Begin
            // 
            this.mbDoc_Begin.BackColor = System.Drawing.Color.White;
            this.mbDoc_Begin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDoc_Begin.Location = new System.Drawing.Point(208, 81);
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
            this.label13.Location = new System.Drawing.Point(313, 78);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(99, 34);
            this.label13.TabIndex = 27;
            this.label13.Text = "Дата и время окончания";
            // 
            // EditReg_Doc
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(566, 203);
            this.Controls.Add(this.gbReg_Doc);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EditReg_Doc";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Оправдательные документы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditReg_Doc_FormClosing);
            this.panel1.ResumeLayout(false);
            this.gbReg_Doc.ResumeLayout(false);
            this.gbReg_Doc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox gbReg_Doc;
        private System.Windows.Forms.ComboBox cbDoc_List_Name;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbDoc_Number;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btSave;
        private EditorsLibrary.DateEditor deDoc_End;
        private EditorsLibrary.DateEditor deDoc_Begin;
        private EditorsLibrary.DateEditor deDoc_Date;
        private System.Windows.Forms.MaskedTextBox mbDoc_Begin;
        private System.Windows.Forms.MaskedTextBox mbDoc_End;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDoc_Location;
    }
}