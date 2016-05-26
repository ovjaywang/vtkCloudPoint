namespace vtkPointCloud
{
    partial class ShowSourceClustering
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
            this.Rotate270Rb = new System.Windows.Forms.RadioButton();
            this.Rotate180Rb = new System.Windows.Forms.RadioButton();
            this.Rotate90Rb = new System.Windows.Forms.RadioButton();
            this.noRotationRb = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.yaxialsymmetry = new System.Windows.Forms.CheckBox();
            this.xaxialsymmetry = new System.Windows.Forms.CheckBox();
            this.OriginalDirection = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.scale = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.xmove = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.ymove = new System.Windows.Forms.TextBox();
            this.SureLocation = new System.Windows.Forms.Button();
            this.AdjustLocation = new System.Windows.Forms.Button();
            this.CancelClustering = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Rotate270Rb);
            this.groupBox1.Controls.Add(this.Rotate180Rb);
            this.groupBox1.Controls.Add(this.Rotate90Rb);
            this.groupBox1.Controls.Add(this.noRotationRb);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(738, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "旋转";
            // 
            // Rotate270Rb
            // 
            this.Rotate270Rb.AutoSize = true;
            this.Rotate270Rb.Font = new System.Drawing.Font("宋体", 14F);
            this.Rotate270Rb.Location = new System.Drawing.Point(533, 41);
            this.Rotate270Rb.Name = "Rotate270Rb";
            this.Rotate270Rb.Size = new System.Drawing.Size(211, 28);
            this.Rotate270Rb.TabIndex = 3;
            this.Rotate270Rb.TabStop = true;
            this.Rotate270Rb.Text = "顺时针旋转270°";
            this.Rotate270Rb.UseVisualStyleBackColor = true;
            // 
            // Rotate180Rb
            // 
            this.Rotate180Rb.AutoSize = true;
            this.Rotate180Rb.Font = new System.Drawing.Font("宋体", 14F);
            this.Rotate180Rb.Location = new System.Drawing.Point(331, 41);
            this.Rotate180Rb.Name = "Rotate180Rb";
            this.Rotate180Rb.Size = new System.Drawing.Size(211, 28);
            this.Rotate180Rb.TabIndex = 2;
            this.Rotate180Rb.TabStop = true;
            this.Rotate180Rb.Text = "顺时针旋转180°";
            this.Rotate180Rb.UseVisualStyleBackColor = true;
            // 
            // Rotate90Rb
            // 
            this.Rotate90Rb.AutoSize = true;
            this.Rotate90Rb.Font = new System.Drawing.Font("宋体", 14F);
            this.Rotate90Rb.Location = new System.Drawing.Point(135, 41);
            this.Rotate90Rb.Name = "Rotate90Rb";
            this.Rotate90Rb.Size = new System.Drawing.Size(199, 28);
            this.Rotate90Rb.TabIndex = 1;
            this.Rotate90Rb.TabStop = true;
            this.Rotate90Rb.Text = "顺时针旋转90°";
            this.Rotate90Rb.UseVisualStyleBackColor = true;
            // 
            // noRotationRb
            // 
            this.noRotationRb.AutoSize = true;
            this.noRotationRb.Checked = true;
            this.noRotationRb.Font = new System.Drawing.Font("宋体", 14F);
            this.noRotationRb.Location = new System.Drawing.Point(26, 41);
            this.noRotationRb.Name = "noRotationRb";
            this.noRotationRb.Size = new System.Drawing.Size(103, 28);
            this.noRotationRb.TabIndex = 0;
            this.noRotationRb.TabStop = true;
            this.noRotationRb.Text = "不旋转";
            this.noRotationRb.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.yaxialsymmetry);
            this.groupBox2.Controls.Add(this.xaxialsymmetry);
            this.groupBox2.Controls.Add(this.OriginalDirection);
            this.groupBox2.Location = new System.Drawing.Point(15, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 86);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "坐标变换";
            // 
            // yaxialsymmetry
            // 
            this.yaxialsymmetry.AutoSize = true;
            this.yaxialsymmetry.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.yaxialsymmetry.Location = new System.Drawing.Point(316, 39);
            this.yaxialsymmetry.Name = "yaxialsymmetry";
            this.yaxialsymmetry.Size = new System.Drawing.Size(116, 28);
            this.yaxialsymmetry.TabIndex = 2;
            this.yaxialsymmetry.Text = "y轴对称";
            this.yaxialsymmetry.UseVisualStyleBackColor = true;
            this.yaxialsymmetry.CheckedChanged += new System.EventHandler(this.yaxialsymmetry_CheckedChanged);
            // 
            // xaxialsymmetry
            // 
            this.xaxialsymmetry.AutoSize = true;
            this.xaxialsymmetry.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xaxialsymmetry.Location = new System.Drawing.Point(184, 39);
            this.xaxialsymmetry.Name = "xaxialsymmetry";
            this.xaxialsymmetry.Size = new System.Drawing.Size(116, 28);
            this.xaxialsymmetry.TabIndex = 1;
            this.xaxialsymmetry.Text = "x轴对称";
            this.xaxialsymmetry.UseVisualStyleBackColor = true;
            this.xaxialsymmetry.CheckedChanged += new System.EventHandler(this.xaxialsymmetry_CheckedChanged);
            // 
            // OriginalDirection
            // 
            this.OriginalDirection.AutoSize = true;
            this.OriginalDirection.Checked = true;
            this.OriginalDirection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OriginalDirection.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OriginalDirection.Location = new System.Drawing.Point(26, 39);
            this.OriginalDirection.Name = "OriginalDirection";
            this.OriginalDirection.Size = new System.Drawing.Size(152, 28);
            this.OriginalDirection.TabIndex = 0;
            this.OriginalDirection.Text = "原坐标方位";
            this.OriginalDirection.UseVisualStyleBackColor = true;
            this.OriginalDirection.CheckedChanged += new System.EventHandler(this.OriginalDirection_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(504, 161);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 36);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "比例";
            // 
            // scale
            // 
            this.scale.Font = new System.Drawing.Font("宋体", 15F);
            this.scale.Location = new System.Drawing.Point(610, 161);
            this.scale.Name = "scale";
            this.scale.Size = new System.Drawing.Size(100, 36);
            this.scale.TabIndex = 3;
            this.scale.Text = "1";
            this.scale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox3.Location = new System.Drawing.Point(44, 36);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(134, 36);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = "x方向平移";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // xmove
            // 
            this.xmove.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xmove.Location = new System.Drawing.Point(210, 36);
            this.xmove.Name = "xmove";
            this.xmove.Size = new System.Drawing.Size(100, 36);
            this.xmove.TabIndex = 5;
            this.xmove.Text = "0";
            this.xmove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox5.Location = new System.Drawing.Point(371, 36);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(129, 36);
            this.textBox5.TabIndex = 6;
            this.textBox5.Text = "y方向平移";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ymove
            // 
            this.ymove.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ymove.Location = new System.Drawing.Point(530, 36);
            this.ymove.Name = "ymove";
            this.ymove.Size = new System.Drawing.Size(100, 36);
            this.ymove.TabIndex = 7;
            this.ymove.Text = "0";
            this.ymove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SureLocation
            // 
            this.SureLocation.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureLocation.Location = new System.Drawing.Point(15, 338);
            this.SureLocation.Name = "SureLocation";
            this.SureLocation.Size = new System.Drawing.Size(256, 47);
            this.SureLocation.TabIndex = 8;
            this.SureLocation.Text = "确认物方数据位置";
            this.SureLocation.UseVisualStyleBackColor = true;
            this.SureLocation.Click += new System.EventHandler(this.SureLocation_Click);
            // 
            // AdjustLocation
            // 
            this.AdjustLocation.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AdjustLocation.Location = new System.Drawing.Point(299, 338);
            this.AdjustLocation.Name = "AdjustLocation";
            this.AdjustLocation.Size = new System.Drawing.Size(148, 47);
            this.AdjustLocation.TabIndex = 9;
            this.AdjustLocation.Text = "调整位置";
            this.AdjustLocation.UseVisualStyleBackColor = true;
            this.AdjustLocation.Click += new System.EventHandler(this.AdjustLocation_Click);
            // 
            // CancelClustering
            // 
            this.CancelClustering.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelClustering.Location = new System.Drawing.Point(463, 338);
            this.CancelClustering.Name = "CancelClustering";
            this.CancelClustering.Size = new System.Drawing.Size(196, 47);
            this.CancelClustering.TabIndex = 10;
            this.CancelClustering.Text = "取消模式聚类";
            this.CancelClustering.UseVisualStyleBackColor = true;
            this.CancelClustering.Click += new System.EventHandler(this.CancelClustering_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.xmove);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Controls.Add(this.ymove);
            this.groupBox3.Location = new System.Drawing.Point(15, 233);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(684, 99);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "平移";
            // 
            // ShowSourceClustering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 412);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CancelClustering);
            this.Controls.Add(this.AdjustLocation);
            this.Controls.Add(this.SureLocation);
            this.Controls.Add(this.scale);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ShowSourceClustering";
            this.Text = "确认/调整真值方位";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton noRotationRb;
        public System.Windows.Forms.RadioButton Rotate270Rb;
        public System.Windows.Forms.RadioButton Rotate180Rb;
        public System.Windows.Forms.RadioButton Rotate90Rb;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox yaxialsymmetry;
        public System.Windows.Forms.CheckBox xaxialsymmetry;
        public System.Windows.Forms.CheckBox OriginalDirection;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.TextBox scale;
        private System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.TextBox xmove;
        private System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.TextBox ymove;
        private System.Windows.Forms.Button SureLocation;
        private System.Windows.Forms.Button AdjustLocation;
        private System.Windows.Forms.Button CancelClustering;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}