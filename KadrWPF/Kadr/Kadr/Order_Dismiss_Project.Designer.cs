namespace Kadr
{
    partial class Order_Dismiss_Project
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
            this.gbDismiss = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cbReason_dismiss = new System.Windows.Forms.ComboBox();
            this.cbBase_doc = new System.Windows.Forms.ComboBox();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btPreview = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dpDate_Transfer = new System.Windows.Forms.DateTimePicker();
            this.gbDismiss.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDismiss
            // 
            this.gbDismiss.Controls.Add(this.dpDate_Transfer);
            this.gbDismiss.Controls.Add(this.label1);
            this.gbDismiss.Controls.Add(this.label26);
            this.gbDismiss.Controls.Add(this.label21);
            this.gbDismiss.Controls.Add(this.cbReason_dismiss);
            this.gbDismiss.Controls.Add(this.cbBase_doc);
            this.gbDismiss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbDismiss.Location = new System.Drawing.Point(0, 0);
            this.gbDismiss.Name = "gbDismiss";
            this.gbDismiss.Size = new System.Drawing.Size(496, 153);
            this.gbDismiss.TabIndex = 27;
            this.gbDismiss.TabStop = false;
            this.gbDismiss.Text = "Данные для проекта приказа об увольнении";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label26.Location = new System.Drawing.Point(14, 86);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(64, 15);
            this.label26.TabIndex = 68;
            this.label26.Text = "Причина";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(14, 57);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(80, 15);
            this.label21.TabIndex = 68;
            this.label21.Text = "Основание";
            // 
            // cbReason_dismiss
            // 
            this.cbReason_dismiss.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbReason_dismiss.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbReason_dismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbReason_dismiss.FormattingEnabled = true;
            this.cbReason_dismiss.Location = new System.Drawing.Point(100, 83);
            this.cbReason_dismiss.Name = "cbReason_dismiss";
            this.cbReason_dismiss.Size = new System.Drawing.Size(378, 23);
            this.cbReason_dismiss.TabIndex = 4;
            // 
            // cbBase_doc
            // 
            this.cbBase_doc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbBase_doc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbBase_doc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBase_doc.FormattingEnabled = true;
            this.cbBase_doc.Location = new System.Drawing.Point(100, 54);
            this.cbBase_doc.Name = "cbBase_doc";
            this.cbBase_doc.Size = new System.Drawing.Size(378, 23);
            this.cbBase_doc.TabIndex = 2;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btPreview);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 115);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(496, 38);
            this.pnButton.TabIndex = 28;
            // 
            // btPreview
            // 
            this.btPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btPreview.Location = new System.Drawing.Point(298, 6);
            this.btPreview.Name = "btPreview";
            this.btPreview.Size = new System.Drawing.Size(87, 23);
            this.btPreview.TabIndex = 2;
            this.btPreview.Text = "Просмотр";
            this.btPreview.UseVisualStyleBackColor = true;
            this.btPreview.Click += new System.EventHandler(this.btPreview_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(391, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 3;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 15);
            this.label1.TabIndex = 69;
            this.label1.Text = "Дата увольнения";
            // 
            // dpDate_Transfer
            // 
            this.dpDate_Transfer.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpDate_Transfer.Location = new System.Drawing.Point(143, 25);
            this.dpDate_Transfer.Name = "dpDate_Transfer";
            this.dpDate_Transfer.Size = new System.Drawing.Size(100, 21);
            this.dpDate_Transfer.TabIndex = 71;
            // 
            // Order_Dismiss_Project
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(496, 153);
            this.Controls.Add(this.pnButton);
            this.Controls.Add(this.gbDismiss);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Order_Dismiss_Project";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order_Dismiss_Project";
            this.gbDismiss.ResumeLayout(false);
            this.gbDismiss.PerformLayout();
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDismiss;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbReason_dismiss;
        private System.Windows.Forms.ComboBox cbBase_doc;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btPreview;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dpDate_Transfer;
    }
}