namespace Tabel
{
    partial class EditTime_Interval
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
            this.cbType_Interval = new System.Windows.Forms.ComboBox();
            this.mbTime_End = new System.Windows.Forms.MaskedTextBox();
            this.mbTime_Begin = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btExit = new Elegant.Ui.Button();
            this.btSave = new Elegant.Ui.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner
            // 
            this.formFrameSkinner.Form = this;
            // 
            // cbType_Interval
            // 
            this.cbType_Interval.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbType_Interval.FormattingEnabled = true;
            this.cbType_Interval.Location = new System.Drawing.Point(129, 79);
            this.cbType_Interval.Name = "cbType_Interval";
            this.cbType_Interval.Size = new System.Drawing.Size(207, 23);
            this.cbType_Interval.TabIndex = 2;
            // 
            // mbTime_End
            // 
            this.mbTime_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbTime_End.Location = new System.Drawing.Point(288, 50);
            this.mbTime_End.Mask = "00:00";
            this.mbTime_End.Name = "mbTime_End";
            this.mbTime_End.Size = new System.Drawing.Size(48, 21);
            this.mbTime_End.TabIndex = 1;
            this.mbTime_End.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mbTime_End.ValidatingType = typeof(System.DateTime);
            this.mbTime_End.Validating += new System.ComponentModel.CancelEventHandler(this.mbTime_End_Validating);
            // 
            // mbTime_Begin
            // 
            this.mbTime_Begin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mbTime_Begin.Location = new System.Drawing.Point(288, 20);
            this.mbTime_Begin.Mask = "00:00";
            this.mbTime_Begin.Name = "mbTime_Begin";
            this.mbTime_Begin.Size = new System.Drawing.Size(48, 21);
            this.mbTime_Begin.TabIndex = 0;
            this.mbTime_Begin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mbTime_Begin.ValidatingType = typeof(System.DateTime);
            this.mbTime_Begin.Validating += new System.ComponentModel.CancelEventHandler(this.mbTime_Begin_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Время начала интервала";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(16, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Тип интервала";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(16, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Время окончания интервала";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btExit);
            this.panel2.Controls.Add(this.btSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 113);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(355, 38);
            this.panel2.TabIndex = 1;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Id = "504b47e6-49aa-461e-b1ed-3a196b49442e";
            this.btExit.Location = new System.Drawing.Point(261, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 1;
            this.btExit.Text = "Выход";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.Id = "b52e56ec-58b3-4869-b719-ea56f3b390cc";
            this.btSave.Location = new System.Drawing.Point(142, 6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(104, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Сохранить";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbType_Interval);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.mbTime_End);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.mbTime_Begin);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // EditTime_Interval
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 151);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditTime_Interval";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Временной интервал";
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mbTime_End;
        private System.Windows.Forms.MaskedTextBox mbTime_Begin;
        private System.Windows.Forms.ComboBox cbType_Interval;
        private System.Windows.Forms.Panel panel2;
        private Elegant.Ui.Button btExit;
        private Elegant.Ui.Button btSave;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}