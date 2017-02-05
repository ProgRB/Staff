namespace Kadr
{
    partial class EditSignPrivPos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.pnButton = new System.Windows.Forms.Panel();
            this.btCancelPriv_Pos = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btViewPrivPos = new System.Windows.Forms.Button();
            this.btEditPriv_Pos = new System.Windows.Forms.Button();
            this.btSavePriv_Pos = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbKPS = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSpec_Con = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgTransfer = new System.Windows.Forms.DataGridView();
            this.pnButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransfer)).BeginInit();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // pnButton
            // 
            this.pnButton.BackColor = System.Drawing.SystemColors.Control;
            this.pnButton.Controls.Add(this.btCancelPriv_Pos);
            this.pnButton.Controls.Add(this.btExit);
            this.pnButton.Controls.Add(this.btViewPrivPos);
            this.pnButton.Controls.Add(this.btEditPriv_Pos);
            this.pnButton.Controls.Add(this.btSavePriv_Pos);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnButton.Location = new System.Drawing.Point(0, 364);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(876, 38);
            this.pnButton.TabIndex = 1;
            // 
            // btCancelPriv_Pos
            // 
            this.btCancelPriv_Pos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelPriv_Pos.Enabled = false;
            this.btCancelPriv_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancelPriv_Pos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btCancelPriv_Pos.Location = new System.Drawing.Point(678, 6);
            this.btCancelPriv_Pos.Name = "btCancelPriv_Pos";
            this.btCancelPriv_Pos.Size = new System.Drawing.Size(87, 23);
            this.btCancelPriv_Pos.TabIndex = 2;
            this.btCancelPriv_Pos.Text = "Отменить";
            this.btCancelPriv_Pos.UseVisualStyleBackColor = true;
            this.btCancelPriv_Pos.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btExit.Location = new System.Drawing.Point(771, 6);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 23);
            this.btExit.TabIndex = 3;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btViewPrivPos
            // 
            this.btViewPrivPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btViewPrivPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btViewPrivPos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btViewPrivPos.Image = global::Kadr.Properties.Resources.Thesaurus_Small;
            this.btViewPrivPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btViewPrivPos.Location = new System.Drawing.Point(15, 6);
            this.btViewPrivPos.Name = "btViewPrivPos";
            this.btViewPrivPos.Size = new System.Drawing.Size(201, 23);
            this.btViewPrivPos.TabIndex = 0;
            this.btViewPrivPos.Text = "Льготные профессии";
            this.btViewPrivPos.UseVisualStyleBackColor = true;
            this.btViewPrivPos.Click += new System.EventHandler(this.btViewPrivPos_Click);
            // 
            // btEditPriv_Pos
            // 
            this.btEditPriv_Pos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditPriv_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btEditPriv_Pos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btEditPriv_Pos.Location = new System.Drawing.Point(452, 6);
            this.btEditPriv_Pos.Name = "btEditPriv_Pos";
            this.btEditPriv_Pos.Size = new System.Drawing.Size(127, 23);
            this.btEditPriv_Pos.TabIndex = 0;
            this.btEditPriv_Pos.Text = "Редактировать";
            this.btEditPriv_Pos.UseVisualStyleBackColor = true;
            this.btEditPriv_Pos.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btSavePriv_Pos
            // 
            this.btSavePriv_Pos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSavePriv_Pos.Enabled = false;
            this.btSavePriv_Pos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSavePriv_Pos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSavePriv_Pos.Location = new System.Drawing.Point(585, 6);
            this.btSavePriv_Pos.Name = "btSavePriv_Pos";
            this.btSavePriv_Pos.Size = new System.Drawing.Size(87, 23);
            this.btSavePriv_Pos.TabIndex = 1;
            this.btSavePriv_Pos.Text = "Сохранить";
            this.btSavePriv_Pos.UseVisualStyleBackColor = true;
            this.btSavePriv_Pos.Click += new System.EventHandler(this.btSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbKPS);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbSpec_Con);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(876, 49);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // cbKPS
            // 
            this.cbKPS.Enabled = false;
            this.cbKPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbKPS.Location = new System.Drawing.Point(689, 17);
            this.cbKPS.Name = "cbKPS";
            this.cbKPS.Size = new System.Drawing.Size(169, 23);
            this.cbKPS.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(319, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(364, 15);
            this.label2.TabIndex = 84;
            this.label2.Text = "Код позиции списка для подразделения и должности";
            // 
            // tbSpec_Con
            // 
            this.tbSpec_Con.BackColor = System.Drawing.Color.White;
            this.tbSpec_Con.Enabled = false;
            this.tbSpec_Con.Location = new System.Drawing.Point(248, 19);
            this.tbSpec_Con.Name = "tbSpec_Con";
            this.tbSpec_Con.ReadOnly = true;
            this.tbSpec_Con.Size = new System.Drawing.Size(43, 20);
            this.tbSpec_Con.TabIndex = 82;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 15);
            this.label1.TabIndex = 83;
            this.label1.Text = "Особые условия (для просмотра)";
            // 
            // dgTransfer
            // 
            this.dgTransfer.AllowUserToAddRows = false;
            this.dgTransfer.AllowUserToDeleteRows = false;
            this.dgTransfer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgTransfer.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgTransfer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTransfer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTransfer.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTransfer.Location = new System.Drawing.Point(0, 0);
            this.dgTransfer.Name = "dgTransfer";
            this.dgTransfer.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTransfer.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgTransfer.RowHeadersWidth = 24;
            this.dgTransfer.Size = new System.Drawing.Size(876, 315);
            this.dgTransfer.TabIndex = 3;
            // 
            // EditSignPrivPos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 402);
            this.Controls.Add(this.dgTransfer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditSignPrivPos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование признака льготной профессии по переводам";
            this.pnButton.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransfer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button btCancelPriv_Pos;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btEditPriv_Pos;
        private System.Windows.Forms.Button btSavePriv_Pos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSpec_Con;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgTransfer;
        private System.Windows.Forms.ComboBox cbKPS;
        private System.Windows.Forms.Button btViewPrivPos;
    }
}