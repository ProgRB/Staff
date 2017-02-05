namespace Kadr
{
    partial class FR_EmpFilter
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
            this.pnButton = new System.Windows.Forms.Panel();
            this.btFilter = new System.Windows.Forms.Button();
            this.btNoFilter = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.pnButton.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btFilter);
            this.pnButton.Controls.Add(this.btNoFilter);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 160);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(557, 40);
            this.pnButton.TabIndex = 1;
            // 
            // btFilter
            // 
            this.btFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btFilter.Location = new System.Drawing.Point(150, 9);
            this.btFilter.Name = "btFilter";
            this.btFilter.Size = new System.Drawing.Size(149, 23);
            this.btFilter.TabIndex = 0;
            this.btFilter.Text = "Применить фильтр";
            this.btFilter.UseVisualStyleBackColor = true;
            this.btFilter.Click += new System.EventHandler(this.btFilter_Click);
            // 
            // btNoFilter
            // 
            this.btNoFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btNoFilter.CausesValidation = false;
            this.btNoFilter.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btNoFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btNoFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btNoFilter.Location = new System.Drawing.Point(305, 9);
            this.btNoFilter.Name = "btNoFilter";
            this.btNoFilter.Size = new System.Drawing.Size(149, 23);
            this.btNoFilter.TabIndex = 1;
            this.btNoFilter.Text = "Отменить фильтр";
            this.btNoFilter.UseVisualStyleBackColor = true;
            this.btNoFilter.Click += new System.EventHandler(this.btNoFilter_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(460, 9);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
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
            this.groupBox2.Size = new System.Drawing.Size(557, 160);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // cbPos_Name
            // 
            this.cbPos_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbPos_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPos_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPos_Name.FormattingEnabled = true;
            this.cbPos_Name.Location = new System.Drawing.Point(123, 127);
            this.cbPos_Name.Name = "cbPos_Name";
            this.cbPos_Name.Size = new System.Drawing.Size(410, 23);
            this.cbPos_Name.TabIndex = 4;
            // 
            // cbSubdiv_Name
            // 
            this.cbSubdiv_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbSubdiv_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSubdiv_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSubdiv_Name.FormattingEnabled = true;
            this.cbSubdiv_Name.Location = new System.Drawing.Point(123, 98);
            this.cbSubdiv_Name.Name = "cbSubdiv_Name";
            this.cbSubdiv_Name.Size = new System.Drawing.Size(410, 23);
            this.cbSubdiv_Name.TabIndex = 3;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(20, 47);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(35, 15);
            this.label57.TabIndex = 53;
            this.label57.Text = "Имя";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(20, 20);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(69, 15);
            this.label56.TabIndex = 45;
            this.label56.Text = "Фамилия";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(20, 74);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(71, 15);
            this.label55.TabIndex = 46;
            this.label55.Text = "Отчество";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(20, 101);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(94, 15);
            this.label60.TabIndex = 52;
            this.label60.Text = "Организация";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(20, 130);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(82, 15);
            this.label53.TabIndex = 49;
            this.label53.Text = "Должность";
            // 
            // tbFR_Middle_Name
            // 
            this.tbFR_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFR_Middle_Name.Location = new System.Drawing.Point(123, 71);
            this.tbFR_Middle_Name.Name = "tbFR_Middle_Name";
            this.tbFR_Middle_Name.Size = new System.Drawing.Size(202, 21);
            this.tbFR_Middle_Name.TabIndex = 2;
            // 
            // tbFR_First_Name
            // 
            this.tbFR_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFR_First_Name.Location = new System.Drawing.Point(123, 44);
            this.tbFR_First_Name.Name = "tbFR_First_Name";
            this.tbFR_First_Name.Size = new System.Drawing.Size(202, 21);
            this.tbFR_First_Name.TabIndex = 1;
            // 
            // tbFR_Last_Name
            // 
            this.tbFR_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFR_Last_Name.Location = new System.Drawing.Point(123, 17);
            this.tbFR_Last_Name.Name = "tbFR_Last_Name";
            this.tbFR_Last_Name.Size = new System.Drawing.Size(202, 21);
            this.tbFR_Last_Name.TabIndex = 0;
            // 
            // FR_EmpFilter
            // 
            this.AcceptButton = this.btFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 200);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pnButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FR_EmpFilter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Фильтрация данных";
            this.pnButton.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbPos_Name;
        private System.Windows.Forms.ComboBox cbSubdiv_Name;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox tbFR_Middle_Name;
        private System.Windows.Forms.TextBox tbFR_First_Name;
        private System.Windows.Forms.TextBox tbFR_Last_Name;
        private System.Windows.Forms.Button btFilter;
        private System.Windows.Forms.Button btNoFilter;
    }
}