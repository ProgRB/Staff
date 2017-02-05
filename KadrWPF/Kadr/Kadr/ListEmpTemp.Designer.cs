namespace Kadr
{
    partial class ListEmpTemp
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgViewEmpTemp = new System.Windows.Forms.DataGridView();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewEmpTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // dgViewEmpTemp
            // 
            this.dgViewEmpTemp.AllowUserToAddRows = false;
            this.dgViewEmpTemp.AllowUserToDeleteRows = false;
            this.dgViewEmpTemp.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewEmpTemp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewEmpTemp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewEmpTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewEmpTemp.Location = new System.Drawing.Point(0, 0);
            this.dgViewEmpTemp.Name = "dgViewEmpTemp";
            this.dgViewEmpTemp.ReadOnly = true;
            this.dgViewEmpTemp.RowHeadersWidth = 24;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewEmpTemp.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgViewEmpTemp.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgViewEmpTemp.Size = new System.Drawing.Size(702, 470);
            this.dgViewEmpTemp.TabIndex = 1;
            this.dgViewEmpTemp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgViewEmpTemp_KeyPress);
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // ListEmpTemp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 470);
            this.Controls.Add(this.dgViewEmpTemp);
            this.Name = "ListEmpTemp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListEmpTemp_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewEmpTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgViewEmpTemp;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;


    }
}