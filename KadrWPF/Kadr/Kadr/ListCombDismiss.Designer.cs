namespace Kadr
{
    partial class ListCombDismiss
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.pnEdu_Button2 = new System.Windows.Forms.Panel();
            this.btExit = new System.Windows.Forms.Button();
            this.btEditComb = new System.Windows.Forms.Button();
            this.dgComb = new System.Windows.Forms.DataGridView();
            this.pnEdu_Button2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgComb)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // pnEdu_Button2
            // 
            this.pnEdu_Button2.Controls.Add(this.btExit);
            this.pnEdu_Button2.Controls.Add(this.btEditComb);
            this.pnEdu_Button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnEdu_Button2.Location = new System.Drawing.Point(0, 394);
            this.pnEdu_Button2.Name = "pnEdu_Button2";
            this.pnEdu_Button2.Size = new System.Drawing.Size(740, 34);
            this.pnEdu_Button2.TabIndex = 27;
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(635, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(83, 23);
            this.btExit.TabIndex = 0;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // btEditComb
            // 
            this.btEditComb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditComb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditComb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btEditComb.Location = new System.Drawing.Point(508, 6);
            this.btEditComb.Name = "btEditComb";
            this.btEditComb.Size = new System.Drawing.Size(121, 23);
            this.btEditComb.TabIndex = 0;
            this.btEditComb.Text = "Редактировать";
            this.btEditComb.UseVisualStyleBackColor = true;
            this.btEditComb.Click += new System.EventHandler(this.btEditComb_Click);
            // 
            // dgComb
            // 
            this.dgComb.AllowUserToAddRows = false;
            this.dgComb.AllowUserToDeleteRows = false;
            this.dgComb.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgComb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgComb.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgComb.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgComb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgComb.Location = new System.Drawing.Point(0, 0);
            this.dgComb.Name = "dgComb";
            this.dgComb.ReadOnly = true;
            this.dgComb.RowHeadersWidth = 24;
            this.dgComb.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgComb.Size = new System.Drawing.Size(740, 394);
            this.dgComb.TabIndex = 28;
            // 
            // ListCombDismiss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(740, 428);
            this.Controls.Add(this.dgComb);
            this.Controls.Add(this.pnEdu_Button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListCombDismiss";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список увольнений по совмещению работника";
            this.pnEdu_Button2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgComb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel pnEdu_Button2;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btEditComb;
        private System.Windows.Forms.DataGridView dgComb;
    }
}