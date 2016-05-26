namespace vtkPointCloud
{
    partial class SureCentroidFrm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.xstepTextBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.xshift = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.yshift = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.ystepTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(89, 261);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 62);
            this.button1.TabIndex = 0;
            this.button1.Text = "确认质心位置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 15F);
            this.button2.Location = new System.Drawing.Point(89, 338);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(216, 62);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消棋盘聚类";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 15F);
            this.button3.Location = new System.Drawing.Point(72, 664);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(199, 62);
            this.button3.TabIndex = 2;
            this.button3.Text = "修正质心位置";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // xstepTextBox
            // 
            this.xstepTextBox.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xstepTextBox.Location = new System.Drawing.Point(170, 415);
            this.xstepTextBox.Name = "xstepTextBox";
            this.xstepTextBox.Size = new System.Drawing.Size(184, 38);
            this.xstepTextBox.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(28, 415);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(136, 30);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "x方向间距";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox3.Location = new System.Drawing.Point(28, 547);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(136, 30);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "x方向偏移";
            // 
            // xshift
            // 
            this.xshift.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.xshift.Location = new System.Drawing.Point(170, 547);
            this.xshift.Name = "xshift";
            this.xshift.Size = new System.Drawing.Size(184, 38);
            this.xshift.TabIndex = 6;
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox5.Location = new System.Drawing.Point(28, 610);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(136, 30);
            this.textBox5.TabIndex = 7;
            this.textBox5.Text = "y方向偏移";
            // 
            // yshift
            // 
            this.yshift.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.yshift.Location = new System.Drawing.Point(170, 604);
            this.yshift.Name = "yshift";
            this.yshift.Size = new System.Drawing.Size(184, 38);
            this.yshift.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(44, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(342, 243);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "请确认质心位置与点云重合度较高，算法依据标记的质心与点云距离计算聚类。算法计算速度最快，如果偏差较大则需要多次尝试输入参数。xy方向间距决定质心间距及单个聚类范围" +
    "，xy方向偏移决定质心整体移动";
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox4.Location = new System.Drawing.Point(28, 479);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(136, 30);
            this.textBox4.TabIndex = 10;
            this.textBox4.Text = "y方向间距";
            // 
            // ystepTextBox
            // 
            this.ystepTextBox.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ystepTextBox.Location = new System.Drawing.Point(170, 473);
            this.ystepTextBox.Name = "ystepTextBox";
            this.ystepTextBox.Size = new System.Drawing.Size(184, 38);
            this.ystepTextBox.TabIndex = 11;
            // 
            // SureCentroidFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 761);
            this.Controls.Add(this.ystepTextBox);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.yshift);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.xshift);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.xstepTextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "SureCentroidFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "确认质心";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.TextBox xstepTextBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.TextBox xshift;
        private System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.TextBox yshift;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.TextBox ystepTextBox;
    }
}