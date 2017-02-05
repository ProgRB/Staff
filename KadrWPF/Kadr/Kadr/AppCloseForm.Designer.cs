namespace Kadr
{
    partial class AppCloseForm
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
            this.btCloseNow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label_time_block = new System.Windows.Forms.Label();
            this.label_remain_time = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCloseNow
            // 
            this.btCloseNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCloseNow.Location = new System.Drawing.Point(253, 49);
            this.btCloseNow.Name = "btCloseNow";
            this.btCloseNow.Size = new System.Drawing.Size(81, 52);
            this.btCloseNow.TabIndex = 0;
            this.btCloseNow.Text = "Закрыть программу сейчас";
            this.btCloseNow.UseVisualStyleBackColor = true;
            this.btCloseNow.Click += new System.EventHandler(this.btCloseNow_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label_time_block);
            this.groupBox1.Controls.Add(this.label_remain_time);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 157);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(173, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "сек.";
            // 
            // label_time_block
            // 
            this.label_time_block.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_time_block.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_time_block.ForeColor = System.Drawing.Color.Red;
            this.label_time_block.Location = new System.Drawing.Point(28, 58);
            this.label_time_block.Name = "label_time_block";
            this.label_time_block.Size = new System.Drawing.Size(192, 20);
            this.label_time_block.TabIndex = 0;
            this.label_time_block.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_remain_time
            // 
            this.label_remain_time.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_remain_time.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_remain_time.ForeColor = System.Drawing.Color.Red;
            this.label_remain_time.Location = new System.Drawing.Point(114, 117);
            this.label_remain_time.Name = "label_remain_time";
            this.label_remain_time.Size = new System.Drawing.Size(51, 20);
            this.label_remain_time.TabIndex = 0;
            this.label_remain_time.Text = "60";
            this.label_remain_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 138);
            this.label1.TabIndex = 0;
            this.label1.Text = "Администратор АСУ \"КАДРЫ\" вызвал блокировку  программы до \r\nСохраните важные данн" +
    "ые. \r\nПрограмма будет автоматически закрыта через ";
            // 
            // AppCloseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 162);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btCloseNow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppCloseForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Закрытие приложения";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCloseNow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_remain_time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_time_block;
    }
}