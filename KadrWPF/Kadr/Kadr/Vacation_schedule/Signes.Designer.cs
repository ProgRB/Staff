namespace Kadr.Vacation_schedule
{
    partial class Signes
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
            this.formFrameSkinner1 = new Elegant.Ui.FormFrameSkinner();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grid_sign = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btAddSign = new System.Windows.Forms.Button();
            this.btDeleteSign = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSaveSignes = new System.Windows.Forms.Button();
            this.screenTip1 = new Elegant.Ui.ScreenTip();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sign)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFrameSkinner1
            // 
            this.formFrameSkinner1.Form = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grid_sign);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 216);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // grid_sign
            // 
            this.grid_sign.AllowUserToAddRows = false;
            this.grid_sign.AllowUserToDeleteRows = false;
            this.grid_sign.AllowUserToResizeRows = false;
            this.grid_sign.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.grid_sign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_sign.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_sign.Location = new System.Drawing.Point(39, 18);
            this.grid_sign.MultiSelect = false;
            this.grid_sign.Name = "grid_sign";
            this.grid_sign.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid_sign.ShowCellErrors = false;
            this.grid_sign.ShowEditingIcon = false;
            this.grid_sign.ShowRowErrors = false;
            this.grid_sign.Size = new System.Drawing.Size(504, 195);
            this.grid_sign.TabIndex = 2;
            this.grid_sign.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grid_sign_DataError);
            this.grid_sign.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grid_sign_ColumnWidthChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btAddSign);
            this.panel1.Controls.Add(this.btDeleteSign);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(36, 195);
            this.panel1.TabIndex = 3;
            // 
            // btAddSign
            // 
            this.btAddSign.ForeColor = System.Drawing.Color.Blue;
            this.btAddSign.Image = global::Kadr.Properties.Resources.TrackChanges_Small;
            this.btAddSign.Location = new System.Drawing.Point(3, 10);
            this.btAddSign.Name = "btAddSign";
            this.screenTip1.GetScreenTip(this.btAddSign).Caption = "Подсказка";
            this.screenTip1.GetScreenTip(this.btAddSign).Text = "Добавить новую подпись";
            this.btAddSign.Size = new System.Drawing.Size(30, 26);
            this.btAddSign.TabIndex = 3;
            this.btAddSign.UseVisualStyleBackColor = true;
            this.btAddSign.Click += new System.EventHandler(this.btAddSign_Click);
            // 
            // btDeleteSign
            // 
            this.btDeleteSign.ForeColor = System.Drawing.Color.Blue;
            this.btDeleteSign.Image = global::Kadr.Properties.Resources.Remove_Small;
            this.btDeleteSign.Location = new System.Drawing.Point(3, 45);
            this.btDeleteSign.Name = "btDeleteSign";
            this.screenTip1.GetScreenTip(this.btDeleteSign).Caption = "Подсказка";
            this.screenTip1.GetScreenTip(this.btDeleteSign).Text = "Удалить выбранную подпись из списка";
            this.btDeleteSign.Size = new System.Drawing.Size(30, 29);
            this.btDeleteSign.TabIndex = 3;
            this.btDeleteSign.UseVisualStyleBackColor = true;
            this.btDeleteSign.Click += new System.EventHandler(this.btDeleteSign_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.ForeColor = System.Drawing.Color.Blue;
            this.btOk.Image = global::Kadr.Properties.Resources.next_gray3232;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOk.Location = new System.Drawing.Point(360, 221);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(88, 23);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "Далее";
            this.btOk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(457, 221);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(86, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btSaveSignes
            // 
            this.btSaveSignes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSaveSignes.ForeColor = System.Drawing.Color.Blue;
            this.btSaveSignes.Location = new System.Drawing.Point(12, 221);
            this.btSaveSignes.Name = "btSaveSignes";
            this.btSaveSignes.Size = new System.Drawing.Size(172, 23);
            this.btSaveSignes.TabIndex = 2;
            this.btSaveSignes.Text = "Сохранить подписи";
            this.btSaveSignes.UseVisualStyleBackColor = true;
            this.btSaveSignes.Click += new System.EventHandler(this.btSaveSignes_Click);
            // 
            // Signes
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(553, 247);
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSaveSignes);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Signes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ввод должностей и ФИО на месте подписи документа";
            this.Shown += new System.EventHandler(this.Signes_Shown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_sign)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elegant.Ui.FormFrameSkinner formFrameSkinner1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.DataGridView grid_sign;
        private System.Windows.Forms.Button btSaveSignes;
        private System.Windows.Forms.Button btAddSign;
        private Elegant.Ui.ScreenTip screenTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btDeleteSign;
    }
}