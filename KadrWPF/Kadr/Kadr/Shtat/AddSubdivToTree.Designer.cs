namespace Kadr.Shtat
{
    partial class AddSubdivToTree
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
            this.label1 = new Elegant.Ui.Label();
            this.label2 = new Elegant.Ui.Label();
            this.label3 = new Elegant.Ui.Label();
            this.label4 = new Elegant.Ui.Label();
            this.type_work = new System.Windows.Forms.ComboBox();
            this.type_subdiv = new System.Windows.Forms.ComboBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.code_subdiv = new System.Windows.Forms.MaskedTextBox();
            this.labelCode = new Elegant.Ui.Label();
            this.subdiv_name = new System.Windows.Forms.TextBox();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Id = "96fcf926-bae7-434e-9aa9-3d19bbae9ed4";
            this.label1.Location = new System.Drawing.Point(66, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Код подразделения";
            this.label1.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.UseVisualThemeForBackground = false;
            this.label1.UseVisualThemeForForeground = false;
            // 
            // label2
            // 
            this.label2.Id = "f21e8b6c-dbeb-41c4-a74e-804854d1ba3c";
            this.label2.Location = new System.Drawing.Point(20, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Название подразделения";
            this.label2.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.UseVisualThemeForBackground = false;
            this.label2.UseVisualThemeForForeground = false;
            // 
            // label3
            // 
            this.label3.Id = "c0ddf1ed-257d-434d-9a48-782efc077a75";
            this.label3.Location = new System.Drawing.Point(143, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Тип работ";
            this.label3.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.UseVisualThemeForBackground = false;
            this.label3.UseVisualThemeForForeground = false;
            // 
            // label4
            // 
            this.label4.Id = "581be72d-b651-4651-b40e-6dafce50a2db";
            this.label4.Location = new System.Drawing.Point(71, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Тип подразделения";
            this.label4.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.UseVisualThemeForBackground = false;
            this.label4.UseVisualThemeForForeground = false;
            // 
            // type_work
            // 
            this.type_work.FormattingEnabled = true;
            this.type_work.Location = new System.Drawing.Point(226, 70);
            this.type_work.Name = "type_work";
            this.type_work.Size = new System.Drawing.Size(327, 24);
            this.type_work.TabIndex = 3;
            // 
            // type_subdiv
            // 
            this.type_subdiv.FormattingEnabled = true;
            this.type_subdiv.Location = new System.Drawing.Point(226, 96);
            this.type_subdiv.Name = "type_subdiv";
            this.type_subdiv.Size = new System.Drawing.Size(327, 24);
            this.type_subdiv.TabIndex = 4;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Location = new System.Drawing.Point(342, 133);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(103, 24);
            this.btOk.TabIndex = 5;
            this.btOk.Text = "Добавить";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(462, 133);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(101, 24);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Выход";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.code_subdiv);
            this.groupBox1.Controls.Add(this.labelCode);
            this.groupBox1.Controls.Add(this.subdiv_name);
            this.groupBox1.Controls.Add(this.type_subdiv);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.type_work);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(10, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(559, 126);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Данные подразделения:";
            // 
            // code_subdiv
            // 
            this.code_subdiv.HidePromptOnLeave = true;
            this.code_subdiv.Location = new System.Drawing.Point(360, 21);
            this.code_subdiv.Mask = "00000000000000000";
            this.code_subdiv.Name = "code_subdiv";
            this.code_subdiv.Size = new System.Drawing.Size(72, 22);
            this.code_subdiv.TabIndex = 1;
            this.code_subdiv.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // labelCode
            // 
            this.labelCode.ForeColor = System.Drawing.Color.SeaGreen;
            this.labelCode.Id = "e5260aeb-17d3-4c8b-aab2-48d6cf68934c";
            this.labelCode.Location = new System.Drawing.Point(226, 24);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(131, 16);
            this.labelCode.TabIndex = 0;
            this.labelCode.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCode.UseVisualThemeForForeground = false;
            // 
            // subdiv_name
            // 
            this.subdiv_name.Location = new System.Drawing.Point(226, 46);
            this.subdiv_name.Name = "subdiv_name";
            this.subdiv_name.Size = new System.Drawing.Size(327, 22);
            this.subdiv_name.TabIndex = 2;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // AddSubdivToTree
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(578, 160);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddSubdivToTree";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление подразделения";
            this.Load += new System.EventHandler(this.AddSubdivToTree_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.Label label1;
        private Elegant.Ui.Label label2;
        private Elegant.Ui.Label label3;
        private Elegant.Ui.Label label4;
        private System.Windows.Forms.ComboBox type_work;
        private System.Windows.Forms.ComboBox type_subdiv;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox subdiv_name;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private Elegant.Ui.Label labelCode;
        private System.Windows.Forms.MaskedTextBox code_subdiv;
    }
}