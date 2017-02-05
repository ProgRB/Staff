namespace Kadr.Shtat
{
    partial class FindStaffEd
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
            this.Position = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Subdiv = new System.Windows.Forms.ComboBox();
            this.label4 = new Elegant.Ui.Label();
            this.code_subdiv = new System.Windows.Forms.TextBox();
            this.label1 = new Elegant.Ui.Label();
            this.Degree = new Elegant.Ui.ComboBox();
            this.label2 = new Elegant.Ui.Label();
            this.Per_num = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new Elegant.Ui.Label();
            this.btFind = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.DG = new System.Windows.Forms.DataGridView();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DG)).BeginInit();
            this.SuspendLayout();
            // 
            // Position
            // 
            this.Position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Position.Location = new System.Drawing.Point(164, 41);
            this.Position.Margin = new System.Windows.Forms.Padding(4);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(640, 22);
            this.Position.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.btFind);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btOk);
            this.splitContainer1.Panel2.Controls.Add(this.DG);
            this.splitContainer1.Size = new System.Drawing.Size(820, 484);
            this.splitContainer1.SplitterDistance = 158;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Subdiv);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.code_subdiv);
            this.groupBox1.Controls.Add(this.Position);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Degree);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Per_num);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(811, 119);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Критерии поиска";
            // 
            // Subdiv
            // 
            this.Subdiv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Subdiv.FormattingEnabled = true;
            this.Subdiv.Location = new System.Drawing.Point(255, 14);
            this.Subdiv.Name = "Subdiv";
            this.Subdiv.Size = new System.Drawing.Size(549, 21);
            this.Subdiv.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Id = "004ab663-409d-41aa-9857-9fb3ff1f11b6";
            this.label4.Location = new System.Drawing.Point(29, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Табельный номер";
            this.label4.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            this.label4.UseVisualThemeForBackground = false;
            this.label4.UseVisualThemeForForeground = false;
            // 
            // code_subdiv
            // 
            this.code_subdiv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.code_subdiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.code_subdiv.Location = new System.Drawing.Point(164, 14);
            this.code_subdiv.Margin = new System.Windows.Forms.Padding(4);
            this.code_subdiv.Name = "code_subdiv";
            this.code_subdiv.Size = new System.Drawing.Size(84, 22);
            this.code_subdiv.TabIndex = 2;
            this.code_subdiv.TextChanged += new System.EventHandler(this.code_subdiv_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Id = "ffdae602-a300-4189-a5e7-092b3d3f910a";
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Подразделение";
            this.label1.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            this.label1.UseVisualThemeForBackground = false;
            this.label1.UseVisualThemeForForeground = false;
            // 
            // Degree
            // 
            this.Degree.BackColor = System.Drawing.Color.White;
            this.Degree.DrawMode = System.Windows.Forms.DrawMode.Normal;
            this.Degree.DroppedDown = false;
            this.Degree.Editable = false;
            this.Degree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Degree.FormatInfo = null;
            this.Degree.FormatString = "";
            this.Degree.FormattingEnabled = false;
            this.Degree.Id = "e74b8994-d69d-4373-a196-ccc79930eb35";
            this.Degree.LabelText = "";
            this.Degree.Location = new System.Drawing.Point(164, 66);
            this.Degree.Name = "Degree";
            this.Degree.Size = new System.Drawing.Size(293, 24);
            this.Degree.Sorted = false;
            this.Degree.TabIndex = 12;
            this.Degree.TextEditorWidth = 274;
            this.Degree.UseVisualThemeForBackground = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Id = "b5e1a527-d882-4801-b735-1946d7a2a03a";
            this.label2.Location = new System.Drawing.Point(66, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Категория";
            this.label2.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            this.label2.UseVisualThemeForBackground = false;
            this.label2.UseVisualThemeForForeground = false;
            // 
            // Per_num
            // 
            this.Per_num.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Per_num.Location = new System.Drawing.Point(164, 93);
            this.Per_num.Margin = new System.Windows.Forms.Padding(4);
            this.Per_num.Mask = "0000000000";
            this.Per_num.Name = "Per_num";
            this.Per_num.PromptChar = ' ';
            this.Per_num.Size = new System.Drawing.Size(84, 22);
            this.Per_num.TabIndex = 11;
            this.Per_num.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Id = "1b71546c-f298-4253-bc18-ab29ddd408c8";
            this.label3.Location = new System.Drawing.Point(66, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Профессия";
            this.label3.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            this.label3.UseVisualThemeForBackground = false;
            this.label3.UseVisualThemeForForeground = false;
            // 
            // btFind
            // 
            this.btFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFind.ForeColor = System.Drawing.Color.Blue;
            this.btFind.Location = new System.Drawing.Point(700, 130);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(100, 25);
            this.btFind.TabIndex = 14;
            this.btFind.Text = "Поиск";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(700, 293);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(89, 25);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Выход";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Location = new System.Drawing.Point(595, 293);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(100, 25);
            this.btOk.TabIndex = 3;
            this.btOk.Text = "Выбрать";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // DG
            // 
            this.DG.AllowUserToAddRows = false;
            this.DG.AllowUserToDeleteRows = false;
            this.DG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DG.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DG.BackgroundColor = System.Drawing.Color.White;
            this.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DG.Location = new System.Drawing.Point(4, 5);
            this.DG.Margin = new System.Windows.Forms.Padding(4);
            this.DG.MultiSelect = false;
            this.DG.Name = "DG";
            this.DG.ReadOnly = true;
            this.DG.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DG.Size = new System.Drawing.Size(815, 288);
            this.DG.TabIndex = 0;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // FindStaffEd
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(820, 484);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FindStaffEd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Введите критерии поиска и выберите штатную единицу";
            this.Load += new System.EventHandler(this.FindStaffEd_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Position;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Elegant.Ui.Label label4;
        private Elegant.Ui.Label label3;
        private Elegant.Ui.Label label2;
        private Elegant.Ui.Label label1;
        private System.Windows.Forms.DataGridView DG;
        private System.Windows.Forms.MaskedTextBox Per_num;
        private Elegant.Ui.ComboBox Degree;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.ComboBox Subdiv;
        private System.Windows.Forms.TextBox code_subdiv;
    }
}