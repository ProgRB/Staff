namespace Kadr
{
    partial class ListEmpConditionWork
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListEmpConditionWork));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmGrid_ras = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gbTransfer = new System.Windows.Forms.GroupBox();
            this.dgTransfer = new System.Windows.Forms.DataGridView();
            this.pnCondition = new System.Windows.Forms.Panel();
            this.btCancelCondition = new System.Windows.Forms.Button();
            this.btSaveCondition = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btFilter_Clear = new System.Windows.Forms.Button();
            this.btFilter_Apply = new System.Windows.Forms.Button();
            this.tbEmp_Middle_Name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbEmp_First_Name = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbEmp_Last_Name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPer_Num = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCode_Subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgEmp = new System.Windows.Forms.DataGridView();
            this.gbTransfer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTransfer)).BeginInit();
            this.pnCondition.SuspendLayout();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmp)).BeginInit();
            this.SuspendLayout();
            // 
            // cmGrid_ras
            // 
            this.cmGrid_ras.Name = "cmGrid_ras";
            this.cmGrid_ras.Size = new System.Drawing.Size(61, 4);
            // 
            // gbTransfer
            // 
            this.gbTransfer.Controls.Add(this.dgTransfer);
            this.gbTransfer.Controls.Add(this.pnCondition);
            this.gbTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbTransfer.Location = new System.Drawing.Point(0, 284);
            this.gbTransfer.Name = "gbTransfer";
            this.gbTransfer.Size = new System.Drawing.Size(933, 204);
            this.gbTransfer.TabIndex = 9;
            this.gbTransfer.TabStop = false;
            this.gbTransfer.Text = "Переводы";
            // 
            // dgTransfer
            // 
            this.dgTransfer.AllowUserToAddRows = false;
            this.dgTransfer.AllowUserToDeleteRows = false;
            this.dgTransfer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgTransfer.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgTransfer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTransfer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTransfer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgTransfer.Location = new System.Drawing.Point(30, 18);
            this.dgTransfer.Name = "dgTransfer";
            this.dgTransfer.RowHeadersWidth = 24;
            this.dgTransfer.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgTransfer.Size = new System.Drawing.Size(900, 183);
            this.dgTransfer.TabIndex = 58;
            // 
            // pnCondition
            // 
            this.pnCondition.BackColor = System.Drawing.SystemColors.Control;
            this.pnCondition.Controls.Add(this.btCancelCondition);
            this.pnCondition.Controls.Add(this.btSaveCondition);
            this.pnCondition.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnCondition.Location = new System.Drawing.Point(3, 18);
            this.pnCondition.Name = "pnCondition";
            this.pnCondition.Size = new System.Drawing.Size(27, 183);
            this.pnCondition.TabIndex = 57;
            // 
            // btCancelCondition
            // 
            this.btCancelCondition.BackgroundImage = global::KadrWPF.Properties.Resources.UndoSmall;
            this.btCancelCondition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btCancelCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.btCancelCondition.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btCancelCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancelCondition.Location = new System.Drawing.Point(0, 26);
            this.btCancelCondition.Name = "btCancelCondition";
            this.btCancelCondition.Size = new System.Drawing.Size(27, 26);
            this.btCancelCondition.TabIndex = 7;
            this.btCancelCondition.UseVisualStyleBackColor = true;
            this.btCancelCondition.Click += new System.EventHandler(this.btCancelCondition_Click);
            // 
            // btSaveCondition
            // 
            this.btSaveCondition.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btSaveCondition.BackgroundImage")));
            this.btSaveCondition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btSaveCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.btSaveCondition.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btSaveCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSaveCondition.Location = new System.Drawing.Point(0, 0);
            this.btSaveCondition.Name = "btSaveCondition";
            this.btSaveCondition.Size = new System.Drawing.Size(27, 26);
            this.btSaveCondition.TabIndex = 6;
            this.btSaveCondition.UseVisualStyleBackColor = true;
            this.btSaveCondition.Click += new System.EventHandler(this.btSaveCondition_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btFilter_Clear);
            this.gbFilter.Controls.Add(this.btFilter_Apply);
            this.gbFilter.Controls.Add(this.tbEmp_Middle_Name);
            this.gbFilter.Controls.Add(this.label5);
            this.gbFilter.Controls.Add(this.tbEmp_First_Name);
            this.gbFilter.Controls.Add(this.label4);
            this.gbFilter.Controls.Add(this.tbEmp_Last_Name);
            this.gbFilter.Controls.Add(this.label3);
            this.gbFilter.Controls.Add(this.tbPer_Num);
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.tbCode_Subdiv);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFilter.Location = new System.Drawing.Point(0, 0);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(933, 63);
            this.gbFilter.TabIndex = 11;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Фильтрация сотрудников";
            // 
            // btFilter_Clear
            // 
            this.btFilter_Clear.BackgroundImage = global::KadrWPF.Properties.Resources.filter_delete;
            this.btFilter_Clear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btFilter_Clear.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btFilter_Clear.Location = new System.Drawing.Point(867, 21);
            this.btFilter_Clear.Name = "btFilter_Clear";
            this.btFilter_Clear.Size = new System.Drawing.Size(36, 36);
            this.btFilter_Clear.TabIndex = 11;
            this.btFilter_Clear.UseVisualStyleBackColor = true;
            this.btFilter_Clear.Click += new System.EventHandler(this.btFilter_Clear_Click);
            // 
            // btFilter_Apply
            // 
            this.btFilter_Apply.BackgroundImage = global::KadrWPF.Properties.Resources.filter_add;
            this.btFilter_Apply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btFilter_Apply.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btFilter_Apply.Location = new System.Drawing.Point(825, 20);
            this.btFilter_Apply.Name = "btFilter_Apply";
            this.btFilter_Apply.Size = new System.Drawing.Size(36, 36);
            this.btFilter_Apply.TabIndex = 10;
            this.btFilter_Apply.UseVisualStyleBackColor = true;
            this.btFilter_Apply.Click += new System.EventHandler(this.btFilter_Apply_Click);
            // 
            // tbEmp_Middle_Name
            // 
            this.tbEmp_Middle_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Middle_Name.Location = new System.Drawing.Point(682, 27);
            this.tbEmp_Middle_Name.Name = "tbEmp_Middle_Name";
            this.tbEmp_Middle_Name.Size = new System.Drawing.Size(128, 22);
            this.tbEmp_Middle_Name.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(597, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Отчество";
            // 
            // tbEmp_First_Name
            // 
            this.tbEmp_First_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_First_Name.Location = new System.Drawing.Point(475, 27);
            this.tbEmp_First_Name.Name = "tbEmp_First_Name";
            this.tbEmp_First_Name.Size = new System.Drawing.Size(113, 22);
            this.tbEmp_First_Name.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(436, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Имя";
            // 
            // tbEmp_Last_Name
            // 
            this.tbEmp_Last_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbEmp_Last_Name.Location = new System.Drawing.Point(307, 27);
            this.tbEmp_Last_Name.Name = "tbEmp_Last_Name";
            this.tbEmp_Last_Name.Size = new System.Drawing.Size(123, 22);
            this.tbEmp_Last_Name.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(232, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Фамилия";
            // 
            // tbPer_Num
            // 
            this.tbPer_Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPer_Num.Location = new System.Drawing.Point(166, 27);
            this.tbPer_Num.Name = "tbPer_Num";
            this.tbPer_Num.Size = new System.Drawing.Size(55, 22);
            this.tbPer_Num.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Таб.№";
            // 
            // tbCode_Subdiv
            // 
            this.tbCode_Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbCode_Subdiv.Location = new System.Drawing.Point(61, 27);
            this.tbCode_Subdiv.Name = "tbCode_Subdiv";
            this.tbCode_Subdiv.Size = new System.Drawing.Size(40, 22);
            this.tbCode_Subdiv.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Подр.";
            // 
            // dgEmp
            // 
            this.dgEmp.AllowUserToAddRows = false;
            this.dgEmp.AllowUserToDeleteRows = false;
            this.dgEmp.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmp.ContextMenuStrip = this.cmGrid_ras;
            this.dgEmp.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgEmp.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEmp.Location = new System.Drawing.Point(0, 63);
            this.dgEmp.Name = "dgEmp";
            this.dgEmp.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmp.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgEmp.RowHeadersWidth = 24;
            this.dgEmp.Size = new System.Drawing.Size(933, 221);
            this.dgEmp.TabIndex = 12;
            this.dgEmp.SelectionChanged += new System.EventHandler(this.dgEmp_SelectionChanged);
            // 
            // ListEmpConditionWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 488);
            this.Controls.Add(this.dgEmp);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbTransfer);
            this.Name = "ListEmpConditionWork";
            this.Text = "Список сотрудников АРМ Бухгалтера";
            this.gbTransfer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTransfer)).EndInit();
            this.pnCondition.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmGrid_ras;
        private System.Windows.Forms.GroupBox gbTransfer;
        private System.Windows.Forms.Panel pnCondition;
        private System.Windows.Forms.Button btSaveCondition;
        private System.Windows.Forms.DataGridView dgTransfer;
        private System.Windows.Forms.Button btCancelCondition;
        public System.Windows.Forms.DataGridView dgEmp;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.TextBox tbEmp_Middle_Name;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbEmp_First_Name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbEmp_Last_Name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPer_Num;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCode_Subdiv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btFilter_Clear;
        private System.Windows.Forms.Button btFilter_Apply;
    }
}