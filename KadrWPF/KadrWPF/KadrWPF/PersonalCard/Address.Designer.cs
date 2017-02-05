namespace Kadr
{
    partial class Address
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
            this.cbRegion = new System.Windows.Forms.ComboBox();
            this.cbStreet = new System.Windows.Forms.ComboBox();
            this.label54 = new System.Windows.Forms.Label();
            this.mbDate_Reg = new System.Windows.Forms.MaskedTextBox();
            this.cbLocality = new System.Windows.Forms.ComboBox();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.lbPhone = new System.Windows.Forms.Label();
            this.cbCity = new System.Windows.Forms.ComboBox();
            this.lbDate = new System.Windows.Forms.Label();
            this.tbPost_Code = new System.Windows.Forms.TextBox();
            this.tbFlat = new System.Windows.Forms.TextBox();
            this.cbDistrict = new System.Windows.Forms.ComboBox();
            this.label61 = new System.Windows.Forms.Label();
            this.tbBulk = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.tbHouse = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.btStreet = new System.Windows.Forms.Button();
            this.btLocality = new System.Windows.Forms.Button();
            this.btRegion = new System.Windows.Forms.Button();
            this.btCity = new System.Windows.Forms.Button();
            this.btDistrict = new System.Windows.Forms.Button();
            this.deDate_Reg = new LibraryKadr.DateEditor();
            this.SuspendLayout();
            // 
            // cbRegion
            // 
            this.cbRegion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbRegion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbRegion.FormattingEnabled = true;
            this.cbRegion.Location = new System.Drawing.Point(114, 5);
            this.cbRegion.Name = "cbRegion";
            this.cbRegion.Size = new System.Drawing.Size(221, 23);
            this.cbRegion.TabIndex = 0;
            // 
            // cbStreet
            // 
            this.cbStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbStreet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbStreet.FormattingEnabled = true;
            this.cbStreet.Location = new System.Drawing.Point(114, 138);
            this.cbStreet.Name = "cbStreet";
            this.cbStreet.Size = new System.Drawing.Size(221, 23);
            this.cbStreet.TabIndex = 4;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = System.Drawing.Color.Transparent;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label54.Location = new System.Drawing.Point(23, 8);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(54, 15);
            this.label54.TabIndex = 126;
            this.label54.Text = "Регион";
            // 
            // mbDate_Reg
            // 
            this.mbDate_Reg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbDate_Reg.Location = new System.Drawing.Point(114, 237);
            this.mbDate_Reg.Mask = "00/00/0000";
            this.mbDate_Reg.Name = "mbDate_Reg";
            this.mbDate_Reg.Size = new System.Drawing.Size(85, 21);
            this.mbDate_Reg.TabIndex = 9;
            this.mbDate_Reg.ValidatingType = typeof(System.DateTime);
            this.mbDate_Reg.Visible = false;
            // 
            // cbLocality
            // 
            this.cbLocality.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbLocality.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbLocality.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbLocality.FormattingEnabled = true;
            this.cbLocality.Location = new System.Drawing.Point(114, 105);
            this.cbLocality.Name = "cbLocality";
            this.cbLocality.Size = new System.Drawing.Size(221, 23);
            this.cbLocality.TabIndex = 3;
            // 
            // tbPhone
            // 
            this.tbPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPhone.Location = new System.Drawing.Point(274, 237);
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(84, 21);
            this.tbPhone.TabIndex = 10;
            // 
            // lbPhone
            // 
            this.lbPhone.AutoSize = true;
            this.lbPhone.BackColor = System.Drawing.Color.Transparent;
            this.lbPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPhone.Location = new System.Drawing.Point(208, 240);
            this.lbPhone.Name = "lbPhone";
            this.lbPhone.Size = new System.Drawing.Size(67, 15);
            this.lbPhone.TabIndex = 124;
            this.lbPhone.Text = "Телефон";
            // 
            // cbCity
            // 
            this.cbCity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbCity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCity.FormattingEnabled = true;
            this.cbCity.Location = new System.Drawing.Point(114, 72);
            this.cbCity.Name = "cbCity";
            this.cbCity.Size = new System.Drawing.Size(221, 23);
            this.cbCity.TabIndex = 2;
            // 
            // lbDate
            // 
            this.lbDate.BackColor = System.Drawing.Color.Transparent;
            this.lbDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDate.Location = new System.Drawing.Point(23, 232);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(85, 37);
            this.lbDate.TabIndex = 123;
            this.lbDate.Text = "Дата прописки";
            // 
            // tbPost_Code
            // 
            this.tbPost_Code.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPost_Code.Location = new System.Drawing.Point(274, 205);
            this.tbPost_Code.Name = "tbPost_Code";
            this.tbPost_Code.Size = new System.Drawing.Size(84, 21);
            this.tbPost_Code.TabIndex = 8;
            // 
            // tbFlat
            // 
            this.tbFlat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFlat.Location = new System.Drawing.Point(114, 205);
            this.tbFlat.Name = "tbFlat";
            this.tbFlat.Size = new System.Drawing.Size(85, 21);
            this.tbFlat.TabIndex = 7;
            // 
            // cbDistrict
            // 
            this.cbDistrict.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbDistrict.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDistrict.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDistrict.FormattingEnabled = true;
            this.cbDistrict.Location = new System.Drawing.Point(114, 38);
            this.cbDistrict.Name = "cbDistrict";
            this.cbDistrict.Size = new System.Drawing.Size(221, 23);
            this.cbDistrict.TabIndex = 1;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.BackColor = System.Drawing.Color.Transparent;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label61.Location = new System.Drawing.Point(23, 177);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(35, 15);
            this.label61.TabIndex = 112;
            this.label61.Text = "Дом";
            // 
            // tbBulk
            // 
            this.tbBulk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBulk.Location = new System.Drawing.Point(274, 172);
            this.tbBulk.Name = "tbBulk";
            this.tbBulk.Size = new System.Drawing.Size(84, 21);
            this.tbBulk.TabIndex = 6;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.BackColor = System.Drawing.Color.Transparent;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label59.Location = new System.Drawing.Point(23, 108);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(46, 15);
            this.label59.TabIndex = 113;
            this.label59.Text = "Пункт";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label62.Location = new System.Drawing.Point(208, 176);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(53, 15);
            this.label62.TabIndex = 117;
            this.label62.Text = "Корпус";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.BackColor = System.Drawing.Color.Transparent;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label60.Location = new System.Drawing.Point(23, 141);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(48, 15);
            this.label60.TabIndex = 111;
            this.label60.Text = "Улица";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.BackColor = System.Drawing.Color.Transparent;
            this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label63.Location = new System.Drawing.Point(23, 208);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(72, 15);
            this.label63.TabIndex = 118;
            this.label63.Text = "Квартира";
            // 
            // tbHouse
            // 
            this.tbHouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbHouse.Location = new System.Drawing.Point(114, 172);
            this.tbHouse.Name = "tbHouse";
            this.tbHouse.Size = new System.Drawing.Size(85, 21);
            this.tbHouse.TabIndex = 5;
            this.tbHouse.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbHouse_KeyPress);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.BackColor = System.Drawing.Color.Transparent;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label58.Location = new System.Drawing.Point(23, 41);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(48, 15);
            this.label58.TabIndex = 115;
            this.label58.Text = "Район";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.BackColor = System.Drawing.Color.Transparent;
            this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label64.Location = new System.Drawing.Point(208, 208);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(55, 15);
            this.label64.TabIndex = 116;
            this.label64.Text = "Индекс";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.BackColor = System.Drawing.Color.Transparent;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label57.Location = new System.Drawing.Point(23, 75);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(47, 15);
            this.label57.TabIndex = 114;
            this.label57.Text = "Город";
            // 
            // btStreet
            // 
            this.btStreet.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btStreet.Image = global::KadrWPF.Properties.Resources.ICO150;
            this.btStreet.Location = new System.Drawing.Point(336, 138);
            this.btStreet.Name = "btStreet";
            this.btStreet.Size = new System.Drawing.Size(23, 23);
            this.btStreet.TabIndex = 15;
            this.btStreet.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btStreet.UseVisualStyleBackColor = true;
            this.btStreet.Click += new System.EventHandler(this.btStreet_Click);
            // 
            // btLocality
            // 
            this.btLocality.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btLocality.Image = global::KadrWPF.Properties.Resources.ICO150;
            this.btLocality.Location = new System.Drawing.Point(336, 105);
            this.btLocality.Name = "btLocality";
            this.btLocality.Size = new System.Drawing.Size(23, 23);
            this.btLocality.TabIndex = 14;
            this.btLocality.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btLocality.UseVisualStyleBackColor = true;
            this.btLocality.Click += new System.EventHandler(this.btLocality_Click);
            // 
            // btRegion
            // 
            this.btRegion.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btRegion.Image = global::KadrWPF.Properties.Resources.ICO150;
            this.btRegion.Location = new System.Drawing.Point(336, 5);
            this.btRegion.Name = "btRegion";
            this.btRegion.Size = new System.Drawing.Size(23, 23);
            this.btRegion.TabIndex = 11;
            this.btRegion.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btRegion.UseVisualStyleBackColor = true;
            this.btRegion.Click += new System.EventHandler(this.btRegion_Click);
            // 
            // btCity
            // 
            this.btCity.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btCity.Image = global::KadrWPF.Properties.Resources.ICO150;
            this.btCity.Location = new System.Drawing.Point(336, 72);
            this.btCity.Name = "btCity";
            this.btCity.Size = new System.Drawing.Size(23, 23);
            this.btCity.TabIndex = 13;
            this.btCity.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btCity.UseVisualStyleBackColor = true;
            this.btCity.Click += new System.EventHandler(this.btCity_Click);
            // 
            // btDistrict
            // 
            this.btDistrict.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btDistrict.Image = global::KadrWPF.Properties.Resources.ICO150;
            this.btDistrict.Location = new System.Drawing.Point(336, 38);
            this.btDistrict.Name = "btDistrict";
            this.btDistrict.Size = new System.Drawing.Size(23, 23);
            this.btDistrict.TabIndex = 12;
            this.btDistrict.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btDistrict.UseVisualStyleBackColor = true;
            this.btDistrict.Click += new System.EventHandler(this.btDistrict_Click);
            // 
            // deDate_Reg
            // 
            this.deDate_Reg.AutoSize = true;
            this.deDate_Reg.BackColor = System.Drawing.SystemColors.Control;
            this.deDate_Reg.Date = null;
            this.deDate_Reg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deDate_Reg.Location = new System.Drawing.Point(114, 237);
            this.deDate_Reg.Margin = new System.Windows.Forms.Padding(5);
            this.deDate_Reg.Name = "deDate_Reg";
            this.deDate_Reg.ReadOnly = false;
            this.deDate_Reg.Size = new System.Drawing.Size(85, 24);
            this.deDate_Reg.TabIndex = 9;
            this.deDate_Reg.TextDate = null;
            // 
            // Address
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 483);
            this.Controls.Add(this.deDate_Reg);
            this.Controls.Add(this.btStreet);
            this.Controls.Add(this.cbRegion);
            this.Controls.Add(this.btLocality);
            this.Controls.Add(this.btRegion);
            this.Controls.Add(this.btCity);
            this.Controls.Add(this.cbStreet);
            this.Controls.Add(this.btDistrict);
            this.Controls.Add(this.label54);
            this.Controls.Add(this.mbDate_Reg);
            this.Controls.Add(this.cbLocality);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.lbPhone);
            this.Controls.Add(this.cbCity);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.tbPost_Code);
            this.Controls.Add(this.tbFlat);
            this.Controls.Add(this.cbDistrict);
            this.Controls.Add(this.label61);
            this.Controls.Add(this.tbBulk);
            this.Controls.Add(this.label59);
            this.Controls.Add(this.label62);
            this.Controls.Add(this.label60);
            this.Controls.Add(this.label63);
            this.Controls.Add(this.tbHouse);
            this.Controls.Add(this.label58);
            this.Controls.Add(this.label64);
            this.Controls.Add(this.label57);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Address";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Address";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btStreet;
        public System.Windows.Forms.ComboBox cbRegion;
        public System.Windows.Forms.Button btLocality;
        public System.Windows.Forms.Button btRegion;
        public System.Windows.Forms.Button btCity;
        public System.Windows.Forms.ComboBox cbStreet;
        public System.Windows.Forms.Button btDistrict;
        private System.Windows.Forms.Label label54;
        public System.Windows.Forms.ComboBox cbLocality;
        public System.Windows.Forms.ComboBox cbCity;
        public System.Windows.Forms.ComboBox cbDistrict;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label57;
        public System.Windows.Forms.MaskedTextBox mbDate_Reg;
        public System.Windows.Forms.TextBox tbPhone;
        public System.Windows.Forms.Label lbPhone;
        public System.Windows.Forms.Label lbDate;
        public System.Windows.Forms.TextBox tbPost_Code;
        public System.Windows.Forms.TextBox tbFlat;
        public System.Windows.Forms.Label label61;
        public System.Windows.Forms.TextBox tbBulk;
        public System.Windows.Forms.Label label62;
        public System.Windows.Forms.Label label63;
        public System.Windows.Forms.TextBox tbHouse;
        public System.Windows.Forms.Label label64;
        public LibraryKadr.DateEditor deDate_Reg;



    }
}