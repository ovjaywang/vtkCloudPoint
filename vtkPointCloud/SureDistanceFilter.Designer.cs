namespace vtkPointCloud
{
    partial class SureDistanceFilter
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ThresholdMaxTxtBox = new System.Windows.Forms.TextBox();
            this.Refilter = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ThresholdMinTxtBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox_maxD = new System.Windows.Forms.TextBox();
            this.textBox_minD = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb_3d = new System.Windows.Forms.RadioButton();
            this.rb_2d = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox1.Location = new System.Drawing.Point(26, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(163, 30);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "距离最大阈值";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ThresholdMaxTxtBox
            // 
            this.ThresholdMaxTxtBox.Font = new System.Drawing.Font("宋体", 12F);
            this.ThresholdMaxTxtBox.Location = new System.Drawing.Point(195, 47);
            this.ThresholdMaxTxtBox.Name = "ThresholdMaxTxtBox";
            this.ThresholdMaxTxtBox.Size = new System.Drawing.Size(146, 30);
            this.ThresholdMaxTxtBox.TabIndex = 1;
            this.ThresholdMaxTxtBox.Text = "42.12";
            this.ThresholdMaxTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Refilter
            // 
            this.Refilter.Font = new System.Drawing.Font("宋体", 18F);
            this.Refilter.Location = new System.Drawing.Point(32, 237);
            this.Refilter.Name = "Refilter";
            this.Refilter.Size = new System.Drawing.Size(165, 42);
            this.Refilter.TabIndex = 22;
            this.Refilter.Text = "重新过滤";
            this.Refilter.UseVisualStyleBackColor = true;
            this.Refilter.Click += new System.EventHandler(this.Refilter_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 14F);
            this.checkBox1.Location = new System.Drawing.Point(26, 182);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(176, 28);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "显示被过滤点";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Font = new System.Drawing.Font("宋体", 18F);
            this.OKBtn.Location = new System.Drawing.Point(221, 32);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(115, 50);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "确认";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Font = new System.Drawing.Font("宋体", 14F);
            this.checkBox2.Location = new System.Drawing.Point(6, 46);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(176, 28);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "导出过滤文件";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OKBtn);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Location = new System.Drawing.Point(28, 443);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 106);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "确认过滤";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.ThresholdMinTxtBox);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.ThresholdMaxTxtBox);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.Refilter);
            this.groupBox2.Location = new System.Drawing.Point(23, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 344);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输入过滤阈值";
            // 
            // ThresholdMinTxtBox
            // 
            this.ThresholdMinTxtBox.Font = new System.Drawing.Font("宋体", 12F);
            this.ThresholdMinTxtBox.Location = new System.Drawing.Point(195, 105);
            this.ThresholdMinTxtBox.Name = "ThresholdMinTxtBox";
            this.ThresholdMinTxtBox.Size = new System.Drawing.Size(146, 30);
            this.ThresholdMinTxtBox.TabIndex = 2;
            this.ThresholdMinTxtBox.Text = "41.70";
            this.ThresholdMinTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox2.Location = new System.Drawing.Point(28, 105);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(163, 30);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "距离最小阈值";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox3.Location = new System.Drawing.Point(23, 12);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(167, 30);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = "Distance最大值";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox4.Location = new System.Drawing.Point(23, 48);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(167, 30);
            this.textBox4.TabIndex = 9;
            this.textBox4.Text = "Distance最小值";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_maxD
            // 
            this.textBox_maxD.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_maxD.Location = new System.Drawing.Point(226, 12);
            this.textBox_maxD.Name = "textBox_maxD";
            this.textBox_maxD.ReadOnly = true;
            this.textBox_maxD.Size = new System.Drawing.Size(153, 30);
            this.textBox_maxD.TabIndex = 10;
            this.textBox_maxD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_minD
            // 
            this.textBox_minD.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_minD.Location = new System.Drawing.Point(226, 48);
            this.textBox_minD.Name = "textBox_minD";
            this.textBox_minD.ReadOnly = true;
            this.textBox_minD.Size = new System.Drawing.Size(153, 30);
            this.textBox_minD.TabIndex = 11;
            this.textBox_minD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rb_2d);
            this.groupBox3.Controls.Add(this.rb_3d);
            this.groupBox3.Location = new System.Drawing.Point(250, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(80, 137);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            // 
            // rb_3d
            // 
            this.rb_3d.AutoSize = true;
            this.rb_3d.Checked = true;
            this.rb_3d.Location = new System.Drawing.Point(6, 24);
            this.rb_3d.Name = "rb_3d";
            this.rb_3d.Size = new System.Drawing.Size(44, 19);
            this.rb_3d.TabIndex = 0;
            this.rb_3d.TabStop = true;
            this.rb_3d.Text = "3D";
            this.rb_3d.UseVisualStyleBackColor = true;
            // 
            // rb_2d
            // 
            this.rb_2d.AutoSize = true;
            this.rb_2d.Location = new System.Drawing.Point(6, 88);
            this.rb_2d.Name = "rb_2d";
            this.rb_2d.Size = new System.Drawing.Size(44, 19);
            this.rb_2d.TabIndex = 1;
            this.rb_2d.Text = "2D";
            this.rb_2d.UseVisualStyleBackColor = true;
            // 
            // SureDistanceFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 561);
            this.Controls.Add(this.textBox_minD);
            this.Controls.Add(this.textBox_maxD);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SureDistanceFilter";
            this.Text = "距离阈值确认";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SureDistanceFilter_FormClosing);
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

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox ThresholdMaxTxtBox;
        private System.Windows.Forms.Button Refilter;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ThresholdMinTxtBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.TextBox textBox_maxD;
        public System.Windows.Forms.TextBox textBox_minD;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_2d;
        private System.Windows.Forms.RadioButton rb_3d;
    }
}