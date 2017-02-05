namespace LibraryKadr
{
    partial class Calendar
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.CurYear = new System.Windows.Forms.NumericUpDown();
            this.Title = new System.Windows.Forms.GroupBox();
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.canvas = new System.Windows.Forms.GroupBox();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Mashtab = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMashtab = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CurYear)).BeginInit();
            this.Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mashtab)).BeginInit();
            this.SuspendLayout();
            // 
            // CurYear
            // 
            this.CurYear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CurYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CurYear.Location = new System.Drawing.Point(116, 0);
            this.CurYear.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.CurYear.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.CurYear.Name = "CurYear";
            this.CurYear.Size = new System.Drawing.Size(75, 21);
            this.CurYear.TabIndex = 0;
            this.CurYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CurYear.Value = new decimal(new int[] {
            2011,
            0,
            0,
            0});
            this.CurYear.ValueChanged += new System.EventHandler(this.CurYear_ValueChanged);
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.Controls.Add(this.vScroll);
            this.Title.Controls.Add(this.canvas);
            this.Title.Controls.Add(this.hScroll);
            this.Title.Controls.Add(this.label1);
            this.Title.Controls.Add(this.Mashtab);
            this.Title.Controls.Add(this.CurYear);
            this.Title.Controls.Add(this.label3);
            this.Title.Controls.Add(this.labelMashtab);
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Title.Location = new System.Drawing.Point(3, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(839, 634);
            this.Title.TabIndex = 1;
            this.Title.TabStop = false;
            this.Title.Text = "Календарь на ";
            this.Title.Enter += new System.EventHandler(this.Title_Enter);
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScroll.Location = new System.Drawing.Point(822, 61);
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size(14, 556);
            this.vScroll.TabIndex = 3;
            this.vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // canvas
            // 
            this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas.AutoSize = true;
            this.canvas.BackColor = System.Drawing.Color.WhiteSmoke;
            this.canvas.Location = new System.Drawing.Point(6, 61);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(813, 553);
            this.canvas.TabIndex = 2;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point(6, 617);
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size(830, 14);
            this.hScroll.TabIndex = 4;
            this.hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(189, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "год";
            // 
            // Mashtab
            // 
            this.Mashtab.AutoSize = false;
            this.Mashtab.BackColor = System.Drawing.Color.Lavender;
            this.Mashtab.LargeChange = 4;
            this.Mashtab.Location = new System.Drawing.Point(116, 21);
            this.Mashtab.Maximum = 100;
            this.Mashtab.Name = "Mashtab";
            this.Mashtab.Size = new System.Drawing.Size(192, 34);
            this.Mashtab.SmallChange = 2;
            this.Mashtab.TabIndex = 5;
            this.Mashtab.TickFrequency = 5;
            this.Mashtab.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Mashtab.Value = 50;
            this.Mashtab.Scroll += new System.EventHandler(this.Mashtab_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(34, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "МАСШТАБ:";
            // 
            // labelMashtab
            // 
            this.labelMashtab.AutoSize = true;
            this.labelMashtab.BackColor = System.Drawing.Color.AliceBlue;
            this.labelMashtab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMashtab.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMashtab.Location = new System.Drawing.Point(308, 29);
            this.labelMashtab.Name = "labelMashtab";
            this.labelMashtab.Size = new System.Drawing.Size(25, 18);
            this.labelMashtab.TabIndex = 1;
            this.labelMashtab.Text = "x0";
            this.labelMashtab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Calendar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Title);
            this.Name = "Calendar";
            this.Size = new System.Drawing.Size(845, 637);
            ((System.ComponentModel.ISupportInitialize)(this.CurYear)).EndInit();
            this.Title.ResumeLayout(false);
            this.Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mashtab)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown CurYear;
        private System.Windows.Forms.GroupBox Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox canvas;
        private System.Windows.Forms.VScrollBar vScroll;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.TrackBar Mashtab;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMashtab;


    }
}
