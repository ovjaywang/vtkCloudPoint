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
            this.distanceThreholdtxtBox = new System.Windows.Forms.TextBox();
            this.Refilter = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 18F);
            this.textBox1.Location = new System.Drawing.Point(26, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(163, 42);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "距离阈值";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // distanceThreholdtxtBox
            // 
            this.distanceThreholdtxtBox.Font = new System.Drawing.Font("宋体", 18F);
            this.distanceThreholdtxtBox.Location = new System.Drawing.Point(195, 35);
            this.distanceThreholdtxtBox.Name = "distanceThreholdtxtBox";
            this.distanceThreholdtxtBox.Size = new System.Drawing.Size(177, 42);
            this.distanceThreholdtxtBox.TabIndex = 1;
            // 
            // Refilter
            // 
            this.Refilter.Font = new System.Drawing.Font("宋体", 18F);
            this.Refilter.Location = new System.Drawing.Point(419, 32);
            this.Refilter.Name = "Refilter";
            this.Refilter.Size = new System.Drawing.Size(165, 42);
            this.Refilter.TabIndex = 2;
            this.Refilter.Text = "重新过滤";
            this.Refilter.UseVisualStyleBackColor = true;
            this.Refilter.Click += new System.EventHandler(this.Refilter_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 18F);
            this.checkBox1.Location = new System.Drawing.Point(24, 161);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(215, 34);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "显示被过滤点";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.Font = new System.Drawing.Font("宋体", 18F);
            this.OKBtn.Location = new System.Drawing.Point(225, 30);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(124, 50);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "确认";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("宋体", 15F);
            this.checkBox2.Location = new System.Drawing.Point(6, 46);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(184, 29);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "导出过滤文件";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OKBtn);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Location = new System.Drawing.Point(270, 115);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 106);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "确认过滤";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.distanceThreholdtxtBox);
            this.groupBox2.Controls.Add(this.Refilter);
            this.groupBox2.Location = new System.Drawing.Point(24, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 97);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输入过滤阈值";
            // 
            // SureDistanceFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 248);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SureDistanceFilter";
            this.Text = "距离阈值确认";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox distanceThreholdtxtBox;
        private System.Windows.Forms.Button Refilter;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}