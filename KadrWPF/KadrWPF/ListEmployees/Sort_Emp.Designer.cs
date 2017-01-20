namespace Kadr
{
    partial class Sort_Emp
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
            this.gbFind = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btSortListEmp = new System.Windows.Forms.Button();
            this.btDate_DismissSorter = new System.Windows.Forms.RadioButton();
            this.btDate_HireSorter = new System.Windows.Forms.RadioButton();
            this.btDay_BirthSorter = new System.Windows.Forms.RadioButton();
            this.btBirth_DateSorter = new System.Windows.Forms.RadioButton();
            this.btSubdivFIOSorter = new System.Windows.Forms.RadioButton();
            this.btFIOSorter = new System.Windows.Forms.RadioButton();
            this.btSubdivPer_NumSorter = new System.Windows.Forms.RadioButton();
            this.btPer_NumSorter = new System.Windows.Forms.RadioButton();
            this.btSubdivSorter = new System.Windows.Forms.RadioButton();
            this.btDown = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.gbFind.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFind
            // 
            this.gbFind.Controls.Add(this.groupBox1);
            this.gbFind.Controls.Add(this.btDate_DismissSorter);
            this.gbFind.Controls.Add(this.btDate_HireSorter);
            this.gbFind.Controls.Add(this.btDay_BirthSorter);
            this.gbFind.Controls.Add(this.btBirth_DateSorter);
            this.gbFind.Controls.Add(this.btSubdivFIOSorter);
            this.gbFind.Controls.Add(this.btFIOSorter);
            this.gbFind.Controls.Add(this.btSubdivPer_NumSorter);
            this.gbFind.Controls.Add(this.btPer_NumSorter);
            this.gbFind.Controls.Add(this.btSubdivSorter);
            this.gbFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.gbFind.Location = new System.Drawing.Point(0, 0);
            this.gbFind.Name = "gbFind";
            this.gbFind.Size = new System.Drawing.Size(383, 295);
            this.gbFind.TabIndex = 0;
            this.gbFind.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btDown);
            this.groupBox1.Controls.Add(this.btUp);
            this.groupBox1.Controls.Add(this.btSortListEmp);
            this.groupBox1.Location = new System.Drawing.Point(0, 245);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 50);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // btSortListEmp
            // 
            this.btSortListEmp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSortListEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btSortListEmp.Location = new System.Drawing.Point(261, 18);
            this.btSortListEmp.Name = "btSortListEmp";
            this.btSortListEmp.Size = new System.Drawing.Size(108, 23);
            this.btSortListEmp.TabIndex = 0;
            this.btSortListEmp.Text = "Применить";
            this.btSortListEmp.UseVisualStyleBackColor = true;
            this.btSortListEmp.Click += new System.EventHandler(this.btFind_Click);
            // 
            // btDate_DismissSorter
            // 
            this.btDate_DismissSorter.AutoSize = true;
            this.btDate_DismissSorter.Location = new System.Drawing.Point(21, 220);
            this.btDate_DismissSorter.Name = "btDate_DismissSorter";
            this.btDate_DismissSorter.Size = new System.Drawing.Size(161, 19);
            this.btDate_DismissSorter.TabIndex = 9;
            this.btDate_DismissSorter.TabStop = true;
            this.btDate_DismissSorter.Text = "По дате увольнения";
            this.btDate_DismissSorter.UseVisualStyleBackColor = true;
            // 
            // btDate_HireSorter
            // 
            this.btDate_HireSorter.AutoSize = true;
            this.btDate_HireSorter.Location = new System.Drawing.Point(21, 195);
            this.btDate_HireSorter.Name = "btDate_HireSorter";
            this.btDate_HireSorter.Size = new System.Drawing.Size(203, 19);
            this.btDate_HireSorter.TabIndex = 8;
            this.btDate_HireSorter.TabStop = true;
            this.btDate_HireSorter.Text = "По дате приема на работу";
            this.btDate_HireSorter.UseVisualStyleBackColor = true;
            // 
            // btDay_BirthSorter
            // 
            this.btDay_BirthSorter.AutoSize = true;
            this.btDay_BirthSorter.Location = new System.Drawing.Point(21, 170);
            this.btDay_BirthSorter.Name = "btDay_BirthSorter";
            this.btDay_BirthSorter.Size = new System.Drawing.Size(143, 19);
            this.btDay_BirthSorter.TabIndex = 7;
            this.btDay_BirthSorter.TabStop = true;
            this.btDay_BirthSorter.Text = "По дню рождения";
            this.btDay_BirthSorter.UseVisualStyleBackColor = true;
            // 
            // btBirth_DateSorter
            // 
            this.btBirth_DateSorter.AutoSize = true;
            this.btBirth_DateSorter.Location = new System.Drawing.Point(21, 145);
            this.btBirth_DateSorter.Name = "btBirth_DateSorter";
            this.btBirth_DateSorter.Size = new System.Drawing.Size(149, 19);
            this.btBirth_DateSorter.TabIndex = 6;
            this.btBirth_DateSorter.TabStop = true;
            this.btBirth_DateSorter.Text = "По дате рождения";
            this.btBirth_DateSorter.UseVisualStyleBackColor = true;
            // 
            // btSubdivFIOSorter
            // 
            this.btSubdivFIOSorter.AutoSize = true;
            this.btSubdivFIOSorter.Location = new System.Drawing.Point(21, 120);
            this.btSubdivFIOSorter.Name = "btSubdivFIOSorter";
            this.btSubdivFIOSorter.Size = new System.Drawing.Size(348, 19);
            this.btSubdivFIOSorter.TabIndex = 5;
            this.btSubdivFIOSorter.TabStop = true;
            this.btSubdivFIOSorter.Text = "По подразделению и фамилии, имени, отчеству";
            this.btSubdivFIOSorter.UseVisualStyleBackColor = true;
            // 
            // btFIOSorter
            // 
            this.btFIOSorter.AutoSize = true;
            this.btFIOSorter.Location = new System.Drawing.Point(21, 95);
            this.btFIOSorter.Name = "btFIOSorter";
            this.btFIOSorter.Size = new System.Drawing.Size(227, 19);
            this.btFIOSorter.TabIndex = 4;
            this.btFIOSorter.TabStop = true;
            this.btFIOSorter.Text = "По фамилии, имени, отчеству";
            this.btFIOSorter.UseVisualStyleBackColor = true;
            // 
            // btSubdivPer_NumSorter
            // 
            this.btSubdivPer_NumSorter.AutoSize = true;
            this.btSubdivPer_NumSorter.Location = new System.Drawing.Point(21, 70);
            this.btSubdivPer_NumSorter.Name = "btSubdivPer_NumSorter";
            this.btSubdivPer_NumSorter.Size = new System.Drawing.Size(300, 19);
            this.btSubdivPer_NumSorter.TabIndex = 3;
            this.btSubdivPer_NumSorter.TabStop = true;
            this.btSubdivPer_NumSorter.Text = "По подразделению и табельному номеру";
            this.btSubdivPer_NumSorter.UseVisualStyleBackColor = true;
            // 
            // btPer_NumSorter
            // 
            this.btPer_NumSorter.AutoSize = true;
            this.btPer_NumSorter.Location = new System.Drawing.Point(21, 45);
            this.btPer_NumSorter.Name = "btPer_NumSorter";
            this.btPer_NumSorter.Size = new System.Drawing.Size(179, 19);
            this.btPer_NumSorter.TabIndex = 2;
            this.btPer_NumSorter.TabStop = true;
            this.btPer_NumSorter.Text = "По табельному номеру";
            this.btPer_NumSorter.UseVisualStyleBackColor = true;
            // 
            // btSubdivSorter
            // 
            this.btSubdivSorter.AutoSize = true;
            this.btSubdivSorter.Location = new System.Drawing.Point(21, 20);
            this.btSubdivSorter.Name = "btSubdivSorter";
            this.btSubdivSorter.Size = new System.Drawing.Size(152, 19);
            this.btSubdivSorter.TabIndex = 1;
            this.btSubdivSorter.TabStop = true;
            this.btSubdivSorter.Text = "По подразделению";
            this.btSubdivSorter.UseVisualStyleBackColor = true;
            // 
            // btDown
            // 
            this.btDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btDown.Image = global::KadrWPF.Properties.Resources.Arrow_Down_2424;
            this.btDown.Location = new System.Drawing.Point(61, 12);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(34, 34);
            this.btDown.TabIndex = 2;
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btUp
            // 
            this.btUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.btUp.Image = global::KadrWPF.Properties.Resources.Arrow_Up_2424;
            this.btUp.Location = new System.Drawing.Point(21, 12);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(34, 34);
            this.btUp.TabIndex = 1;
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // Sort_Emp
            // 
            this.Controls.Add(this.gbFind);
            this.Name = "Sort_Emp";
            this.Size = new System.Drawing.Size(383, 295);
            this.gbFind.ResumeLayout(false);
            this.gbFind.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFind;
        private System.Windows.Forms.Button btSortListEmp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton btDate_DismissSorter;
        private System.Windows.Forms.RadioButton btDate_HireSorter;
        private System.Windows.Forms.RadioButton btDay_BirthSorter;
        private System.Windows.Forms.RadioButton btBirth_DateSorter;
        private System.Windows.Forms.RadioButton btSubdivFIOSorter;
        private System.Windows.Forms.RadioButton btFIOSorter;
        private System.Windows.Forms.RadioButton btSubdivPer_NumSorter;
        private System.Windows.Forms.RadioButton btPer_NumSorter;
        private System.Windows.Forms.RadioButton btSubdivSorter;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btDown;
    }
}