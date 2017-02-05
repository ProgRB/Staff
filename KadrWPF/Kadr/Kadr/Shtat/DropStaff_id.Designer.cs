using Staff;
using LibraryKadr;
using Oracle.DataAccess.Client;
namespace Kadr.Shtat
{
    partial class DropStaff_id
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btFind = new System.Windows.Forms.Button();
            this.btChoosePos = new System.Windows.Forms.Button();
            this.label4 = new Elegant.Ui.Label();
            this.per_num = new System.Windows.Forms.MaskedTextBox();
            this.code_pos = new System.Windows.Forms.MaskedTextBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.GridResult = new System.Windows.Forms.DataGridView();
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.viewStructBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewStructBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(7, 6);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GridResult);
            this.splitContainer1.Size = new System.Drawing.Size(785, 483);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btFind);
            this.groupBox1.Controls.Add(this.btChoosePos);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.per_num);
            this.groupBox1.Controls.Add(this.code_pos);
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Критерии поиска";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Шифр профессии";
            // 
            // btFind
            // 
            this.btFind.ForeColor = System.Drawing.Color.Blue;
            this.btFind.Location = new System.Drawing.Point(158, 67);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(105, 22);
            this.btFind.TabIndex = 15;
            this.btFind.Text = "Поиск";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // btChoosePos
            // 
            this.btChoosePos.ForeColor = System.Drawing.Color.Blue;
            this.btChoosePos.Location = new System.Drawing.Point(244, 13);
            this.btChoosePos.Name = "btChoosePos";
            this.btChoosePos.Size = new System.Drawing.Size(92, 22);
            this.btChoosePos.TabIndex = 0;
            this.btChoosePos.TabStop = false;
            this.btChoosePos.Text = "Выбрать";
            this.btChoosePos.UseVisualStyleBackColor = true;
            this.btChoosePos.Click += new System.EventHandler(this.btChoose_Click);
            // 
            // label4
            // 
            this.label4.Id = "9488afc3-4ade-4064-875b-ad99ae821b70";
            this.label4.Location = new System.Drawing.Point(18, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "Табельный номер";
            this.label4.UseVisualThemeForBackground = false;
            this.label4.UseVisualThemeForForeground = false;
            // 
            // per_num
            // 
            this.per_num.Location = new System.Drawing.Point(158, 38);
            this.per_num.Margin = new System.Windows.Forms.Padding(4);
            this.per_num.Mask = "000000000";
            this.per_num.Name = "per_num";
            this.per_num.Size = new System.Drawing.Size(79, 22);
            this.per_num.TabIndex = 14;
            // 
            // code_pos
            // 
            this.code_pos.Location = new System.Drawing.Point(158, 13);
            this.code_pos.Margin = new System.Windows.Forms.Padding(4);
            this.code_pos.Mask = "000000000";
            this.code_pos.Name = "code_pos";
            this.code_pos.Size = new System.Drawing.Size(79, 22);
            this.code_pos.TabIndex = 13;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(688, 496);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(105, 22);
            this.btCancel.TabIndex = 18;
            this.btCancel.Text = "Выход";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Location = new System.Drawing.Point(556, 496);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(111, 22);
            this.btOk.TabIndex = 17;
            this.btOk.Text = "Исключить";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // GridResult
            // 
            this.GridResult.AllowUserToAddRows = false;
            this.GridResult.AllowUserToDeleteRows = false;
            this.GridResult.AllowUserToResizeRows = false;
            this.GridResult.BackgroundColor = System.Drawing.Color.White;
            this.GridResult.ColumnHeadersHeight = 30;
            this.GridResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridResult.Location = new System.Drawing.Point(0, 0);
            this.GridResult.Margin = new System.Windows.Forms.Padding(4);
            this.GridResult.MultiSelect = false;
            this.GridResult.Name = "GridResult";
            this.GridResult.RowHeadersVisible = false;
            this.GridResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridResult.Size = new System.Drawing.Size(785, 377);
            this.GridResult.TabIndex = 16;
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // DropStaff_id
            // 
            this.AcceptButton = this.btFind;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(801, 523);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DropStaff_id";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Введите критерии поиска и выберите удаляемую единицу";
            this.Load += new System.EventHandler(this.DropStaff_id_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewStructBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource viewStructBindingSource;
        private System.Windows.Forms.DataGridView GridResult;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        public void InitField(string staffs_id)
        {
            OracleCommand cmd = new OracleCommand(string.Format("select code_subdiv,subdiv_name,degree_name,code_pos,staffs.subdiv_id from {0}.staffs left join {0}.subdiv on (staffs.subdiv_id=subdiv.subdiv_id) left join {0}.degree on (staffs.degree_id=degree.degree_id) left join {0}.position on (staffs.pos_id=position.pos_id) where staffs_id={1}", DataSourceScheme.SchemeName, staffs_id), Connect.CurConnect);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                code_pos.Text = reader["code_pos"].ToString();
                ShtatFilter.subdiv_id = reader["subdiv_id"].ToString();
            }
        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btChoosePos;
        private Elegant.Ui.Label label4;
        private System.Windows.Forms.MaskedTextBox per_num;
        private System.Windows.Forms.MaskedTextBox code_pos;
    }
}