namespace Kadr
{
    partial class PadegException
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Val = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Vin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Rod = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SexWord = new System.Windows.Forms.ComboBox();
            this.rbMiddle = new System.Windows.Forms.RadioButton();
            this.rbName = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.rbFam = new System.Windows.Forms.RadioButton();
            this.rbTheNoun = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GridExcept = new System.Windows.Forms.DataGridView();
            this.btSave = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.btNewExceptWord = new System.Windows.Forms.Button();
            this.btDropFromDict = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridExcept)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Val);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(4, 334);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(779, 143);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Склонение слова";
            // 
            // Val
            // 
            this.Val.Location = new System.Drawing.Point(70, 19);
            this.Val.Name = "Val";
            this.Val.Size = new System.Drawing.Size(298, 22);
            this.Val.TabIndex = 1;
            this.Val.TextChanged += new System.EventHandler(this.Val_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.Vin);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.dat);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.Rod);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(374, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(400, 131);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Формы слова в падежах";
            // 
            // Vin
            // 
            this.Vin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Vin.Location = new System.Drawing.Point(133, 91);
            this.Vin.Name = "Vin";
            this.Vin.Size = new System.Drawing.Size(250, 22);
            this.Vin.TabIndex = 5;
            this.Vin.TextChanged += new System.EventHandler(this.Val_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Винительный:";
            // 
            // dat
            // 
            this.dat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dat.Location = new System.Drawing.Point(133, 59);
            this.dat.Name = "dat";
            this.dat.Size = new System.Drawing.Size(250, 22);
            this.dat.TabIndex = 4;
            this.dat.TextChanged += new System.EventHandler(this.Val_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Дательный:";
            // 
            // Rod
            // 
            this.Rod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Rod.Location = new System.Drawing.Point(133, 28);
            this.Rod.Name = "Rod";
            this.Rod.Size = new System.Drawing.Size(250, 22);
            this.Rod.TabIndex = 3;
            this.Rod.TextChanged += new System.EventHandler(this.Val_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Родительный:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SexWord);
            this.groupBox3.Controls.Add(this.rbMiddle);
            this.groupBox3.Controls.Add(this.rbName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.rbFam);
            this.groupBox3.Controls.Add(this.rbTheNoun);
            this.groupBox3.Location = new System.Drawing.Point(6, 40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(362, 99);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Тип слова";
            // 
            // SexWord
            // 
            this.SexWord.Items.AddRange(new object[] {
            "Ж",
            "М"});
            this.SexWord.Location = new System.Drawing.Point(235, 29);
            this.SexWord.Name = "SexWord";
            this.SexWord.Size = new System.Drawing.Size(43, 24);
            this.SexWord.TabIndex = 2;
            this.SexWord.Text = "Ж";
            this.SexWord.TextChanged += new System.EventHandler(this.Val_TextChanged);
            // 
            // rbMiddle
            // 
            this.rbMiddle.AutoSize = true;
            this.rbMiddle.Location = new System.Drawing.Point(10, 57);
            this.rbMiddle.Name = "rbMiddle";
            this.rbMiddle.Size = new System.Drawing.Size(97, 20);
            this.rbMiddle.TabIndex = 5;
            this.rbMiddle.Text = "Отчество";
            this.rbMiddle.UseVisualStyleBackColor = true;
            this.rbMiddle.CheckedChanged += new System.EventHandler(this.rbFam_CheckedChanged);
            // 
            // rbName
            // 
            this.rbName.AutoSize = true;
            this.rbName.Location = new System.Drawing.Point(10, 36);
            this.rbName.Name = "rbName";
            this.rbName.Size = new System.Drawing.Size(55, 20);
            this.rbName.TabIndex = 5;
            this.rbName.Text = "Имя";
            this.rbName.UseVisualStyleBackColor = true;
            this.rbName.CheckedChanged += new System.EventHandler(this.rbFam_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Пол:";
            // 
            // rbFam
            // 
            this.rbFam.AutoSize = true;
            this.rbFam.Checked = true;
            this.rbFam.Location = new System.Drawing.Point(10, 16);
            this.rbFam.Name = "rbFam";
            this.rbFam.Size = new System.Drawing.Size(92, 20);
            this.rbFam.TabIndex = 5;
            this.rbFam.TabStop = true;
            this.rbFam.Text = "Фамилия";
            this.rbFam.UseVisualStyleBackColor = true;
            this.rbFam.CheckedChanged += new System.EventHandler(this.rbFam_CheckedChanged);
            // 
            // rbTheNoun
            // 
            this.rbTheNoun.AutoSize = true;
            this.rbTheNoun.Location = new System.Drawing.Point(10, 77);
            this.rbTheNoun.Name = "rbTheNoun";
            this.rbTheNoun.Size = new System.Drawing.Size(316, 20);
            this.rbTheNoun.TabIndex = 5;
            this.rbTheNoun.Text = "Существительное или прилагательное";
            this.rbTheNoun.UseVisualStyleBackColor = true;
            this.rbTheNoun.CheckedChanged += new System.EventHandler(this.rbFam_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Слово:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.GridExcept);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(3, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(780, 305);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Словарь исключений";
            // 
            // GridExcept
            // 
            this.GridExcept.AllowUserToAddRows = false;
            this.GridExcept.AllowUserToDeleteRows = false;
            this.GridExcept.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridExcept.BackgroundColor = System.Drawing.Color.White;
            this.GridExcept.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridExcept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridExcept.Location = new System.Drawing.Point(3, 18);
            this.GridExcept.MultiSelect = false;
            this.GridExcept.Name = "GridExcept";
            this.GridExcept.ReadOnly = true;
            this.GridExcept.RowHeadersVisible = false;
            this.GridExcept.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridExcept.Size = new System.Drawing.Size(774, 284);
            this.GridExcept.TabIndex = 0;
            this.GridExcept.SelectionChanged += new System.EventHandler(this.GridExcept_SelectionChanged);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Enabled = false;
            this.btSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSave.ForeColor = System.Drawing.Color.Blue;
            this.btSave.Location = new System.Drawing.Point(573, 311);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(96, 23);
            this.btSave.TabIndex = 7;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClose.ForeColor = System.Drawing.Color.Blue;
            this.btClose.Location = new System.Drawing.Point(675, 311);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(96, 23);
            this.btClose.TabIndex = 8;
            this.btClose.Text = "Выход";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btNewExceptWord
            // 
            this.btNewExceptWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btNewExceptWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btNewExceptWord.ForeColor = System.Drawing.Color.Blue;
            this.btNewExceptWord.Location = new System.Drawing.Point(6, 311);
            this.btNewExceptWord.Name = "btNewExceptWord";
            this.btNewExceptWord.Size = new System.Drawing.Size(196, 23);
            this.btNewExceptWord.TabIndex = 6;
            this.btNewExceptWord.Text = "Добавить новое слово";
            this.btNewExceptWord.UseVisualStyleBackColor = true;
            this.btNewExceptWord.Click += new System.EventHandler(this.btNewExceptWord_Click);
            // 
            // btDropFromDict
            // 
            this.btDropFromDict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDropFromDict.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDropFromDict.ForeColor = System.Drawing.Color.Blue;
            this.btDropFromDict.Location = new System.Drawing.Point(206, 311);
            this.btDropFromDict.Name = "btDropFromDict";
            this.btDropFromDict.Size = new System.Drawing.Size(178, 23);
            this.btDropFromDict.TabIndex = 6;
            this.btDropFromDict.Text = "Удалить исключение";
            this.btDropFromDict.UseVisualStyleBackColor = true;
            this.btDropFromDict.Click += new System.EventHandler(this.btDropFromDict_Click);
            // 
            // PadegException
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(786, 478);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btDropFromDict);
            this.Controls.Add(this.btNewExceptWord);
            this.Controls.Add(this.btSave);
            this.Name = "PadegException";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Словарь исключений склонения слов";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridExcept)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView GridExcept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbFam;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox Val;
        private System.Windows.Forms.RadioButton rbTheNoun;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox Vin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox dat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Rod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btNewExceptWord;
        private System.Windows.Forms.RadioButton rbMiddle;
        private System.Windows.Forms.RadioButton rbName;
        private System.Windows.Forms.ComboBox SexWord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btDropFromDict;
    }
}