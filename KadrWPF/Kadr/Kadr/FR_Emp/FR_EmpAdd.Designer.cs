namespace Kadr
{
    partial class FR_EmpAdd
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.deFR_Date_Start = new EditorsLibrary.DateEditor();
            this.label21 = new System.Windows.Forms.Label();
            this.mbFR_Date_Start = new System.Windows.Forms.MaskedTextBox();
            this.gbPhoto = new System.Windows.Forms.GroupBox();
            this.btEditPhotoFR_Emp = new System.Windows.Forms.Button();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.cbPos_Name = new System.Windows.Forms.ComboBox();
            this.cbSubdiv_Name = new System.Windows.Forms.ComboBox();
            this.label57 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.tbFR_Middle_Name = new System.Windows.Forms.TextBox();
            this.tbFR_First_Name = new System.Windows.Forms.TextBox();
            this.tbFR_Last_Name = new System.Windows.Forms.TextBox();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btSaveFR_Emp = new System.Windows.Forms.Button();
            this.ofdAddPhoto = new System.Windows.Forms.OpenFileDialog();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.groupBox2.SuspendLayout();
            this.gbPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.deFR_Date_Start);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.mbFR_Date_Start);
            this.groupBox2.Controls.Add(this.gbPhoto);
            this.groupBox2.Controls.Add(this.cbPos_Name);
            this.groupBox2.Controls.Add(this.cbSubdiv_Name);
            this.groupBox2.Controls.Add(this.label57);
            this.groupBox2.Controls.Add(this.label56);
            this.groupBox2.Controls.Add(this.label55);
            this.groupBox2.Controls.Add(this.label60);
            this.groupBox2.Controls.Add(this.label53);
            this.groupBox2.Controls.Add(this.tbFR_Middle_Name);
            this.groupBox2.Controls.Add(this.tbFR_First_Name);
            this.groupBox2.Controls.Add(this.tbFR_Last_Name);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(556, 227);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // deFR_Date_Start
            // 
            this.deFR_Date_Start.AutoSize = true;
            this.deFR_Date_Start.BackColor = System.Drawing.SystemColors.Control;
            this.deFR_Date_Start.Date = null;
            this.deFR_Date_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deFR_Date_Start.Location = new System.Drawing.Point(124, 127);
            this.deFR_Date_Start.Margin = new System.Windows.Forms.Padding(5);
            this.deFR_Date_Start.Name = "deFR_Date_Start";
            this.deFR_Date_Start.ReadOnly = false;
            this.deFR_Date_Start.Size = new System.Drawing.Size(73, 24);
            this.deFR_Date_Start.TabIndex = 3;
            this.deFR_Date_Start.TextDate = null;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(21, 130);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(92, 15);
            this.label21.TabIndex = 55;
            this.label21.Text = "Дата начала";
            // 
            // mbFR_Date_Start
            // 
            this.mbFR_Date_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbFR_Date_Start.Location = new System.Drawing.Point(96, 127);
            this.mbFR_Date_Start.Mask = "00/00/0000";
            this.mbFR_Date_Start.Name = "mbFR_Date_Start";
            this.mbFR_Date_Start.Size = new System.Drawing.Size(73, 21);
            this.mbFR_Date_Start.TabIndex = 54;
            this.mbFR_Date_Start.ValidatingType = typeof(System.DateTime);
            this.mbFR_Date_Start.Visible = false;
            // 
            // gbPhoto
            // 
            this.gbPhoto.Controls.Add(this.btEditPhotoFR_Emp);
            this.gbPhoto.Controls.Add(this.pbPhoto);
            this.gbPhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbPhoto.Location = new System.Drawing.Point(376, 0);
            this.gbPhoto.Name = "gbPhoto";
            this.gbPhoto.Size = new System.Drawing.Size(174, 156);
            this.gbPhoto.TabIndex = 6;
            this.gbPhoto.TabStop = false;
            this.gbPhoto.Text = "Фото";
            // 
            // btEditPhotoFR_Emp
            // 
            this.btEditPhotoFR_Emp.BackgroundImage = global::Kadr.Properties.Resources.Prepare_Large;
            this.btEditPhotoFR_Emp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEditPhotoFR_Emp.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btEditPhotoFR_Emp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEditPhotoFR_Emp.Location = new System.Drawing.Point(5, 119);
            this.btEditPhotoFR_Emp.Name = "btEditPhotoFR_Emp";
            this.btEditPhotoFR_Emp.Size = new System.Drawing.Size(27, 26);
            this.btEditPhotoFR_Emp.TabIndex = 0;
            this.btEditPhotoFR_Emp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btEditPhotoFR_Emp.UseVisualStyleBackColor = true;
            this.btEditPhotoFR_Emp.Click += new System.EventHandler(this.btEditPhoto_Click);
            // 
            // pbPhoto
            // 
            this.pbPhoto.Location = new System.Drawing.Point(38, 15);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(120, 130);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPhoto.TabIndex = 0;
            this.pbPhoto.TabStop = false;
            // 
            // cbPos_Name
            // 
            this.cbPos_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbPos_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPos_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPos_Name.FormattingEnabled = true;
            this.cbPos_Name.Location = new System.Drawing.Point(124, 194);
            this.cbPos_Name.Name = "cbPos_Name";
            this.cbPos_Name.Size = new System.Drawing.Size(410, 23);
            this.cbPos_Name.TabIndex = 5;
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(124, 162);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(410, 23);
            this.cbSubdiv_Name.TabIndex = 4;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(21, 62);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(35, 15);
            this.label57.TabIndex = 53;
            this.label57.Text = "Имя";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(21, 28);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(69, 15);
            this.label56.TabIndex = 45;
            this.label56.Text = "Фамилия";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(21, 95);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(71, 15);
            this.label55.TabIndex = 46;
            this.label55.Text = "Отчество";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(21, 165);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(94, 15);
            this.label60.TabIndex = 52;
            this.label60.Text = "Организация";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(21, 197);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(82, 15);
            this.label53.TabIndex = 49;
            this.label53.Text = "Должность";
            // 
            // tbFR_Middle_Name
            // 
            this.tbFR_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFR_Middle_Name.Location = new System.Drawing.Point(124, 92);
            this.tbFR_Middle_Name.Name = "tbFR_Middle_Name";
            this.tbFR_Middle_Name.Size = new System.Drawing.Size(240, 21);
            this.tbFR_Middle_Name.TabIndex = 2;
            // 
            // tbFR_First_Name
            // 
            this.tbFR_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFR_First_Name.Location = new System.Drawing.Point(124, 59);
            this.tbFR_First_Name.Name = "tbFR_First_Name";
            this.tbFR_First_Name.Size = new System.Drawing.Size(240, 21);
            this.tbFR_First_Name.TabIndex = 1;
            // 
            // tbFR_Last_Name
            // 
            this.tbFR_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFR_Last_Name.Location = new System.Drawing.Point(124, 25);
            this.tbFR_Last_Name.Name = "tbFR_Last_Name";
            this.tbFR_Last_Name.Size = new System.Drawing.Size(240, 21);
            this.tbFR_Last_Name.TabIndex = 0;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btSaveFR_Emp);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 227);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(556, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(459, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSaveFR_Emp
            // 
            this.btSaveFR_Emp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveFR_Emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSaveFR_Emp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSaveFR_Emp.Location = new System.Drawing.Point(366, 9);
            this.btSaveFR_Emp.Name = "btSaveFR_Emp";
            this.btSaveFR_Emp.Size = new System.Drawing.Size(87, 23);
            this.btSaveFR_Emp.TabIndex = 0;
            this.btSaveFR_Emp.Text = "Сохранить";
            this.btSaveFR_Emp.UseVisualStyleBackColor = true;
            this.btSaveFR_Emp.Click += new System.EventHandler(this.btSave_Click);
            // 
            // ofdAddPhoto
            // 
            this.ofdAddPhoto.Filter = "Фото сотрудника|*.jpg";
            this.ofdAddPhoto.InitialDirectory = "C:\\";
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // FR_EmpAdd
            // 
            this.AcceptButton = this.btSaveFR_Emp;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(556, 267);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FR_EmpAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FR_EmpAdd";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbPhoto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbPos_Name;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox tbFR_First_Name;
        private System.Windows.Forms.TextBox tbFR_Last_Name;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btSaveFR_Emp;
        private System.Windows.Forms.TextBox tbFR_Middle_Name;
        private System.Windows.Forms.GroupBox gbPhoto;
        public System.Windows.Forms.Button btEditPhotoFR_Emp;
        private System.Windows.Forms.PictureBox pbPhoto;
        private System.Windows.Forms.OpenFileDialog ofdAddPhoto;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.MaskedTextBox mbFR_Date_Start;
        private EditorsLibrary.DateEditor deFR_Date_Start;
    }
}