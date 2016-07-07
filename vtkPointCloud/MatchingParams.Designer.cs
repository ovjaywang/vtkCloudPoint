namespace vtkPointCloud
{
    partial class MatchingParams
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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SureMatchingParams = new System.Windows.Forms.Button();
            this.CancleMatchingParams = new System.Windows.Forms.Button();
            this.AdjustMatchDistance = new System.Windows.Forms.Button();
            this.ExportMatchedPtsCheckBox = new System.Windows.Forms.CheckBox();
            this.IsShowUnmatchedPtsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(12, 91);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(136, 38);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "110";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(12, 21);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(266, 54);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "请输入匹配阈值        阈值内视为配对";
            // 
            // SureMatchingParams
            // 
            this.SureMatchingParams.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureMatchingParams.Location = new System.Drawing.Point(12, 272);
            this.SureMatchingParams.Name = "SureMatchingParams";
            this.SureMatchingParams.Size = new System.Drawing.Size(124, 48);
            this.SureMatchingParams.TabIndex = 2;
            this.SureMatchingParams.Text = "确认";
            this.SureMatchingParams.UseVisualStyleBackColor = true;
            this.SureMatchingParams.Click += new System.EventHandler(this.SureMatchingParams_Click);
            // 
            // CancleMatchingParams
            // 
            this.CancleMatchingParams.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancleMatchingParams.Location = new System.Drawing.Point(154, 272);
            this.CancleMatchingParams.Name = "CancleMatchingParams";
            this.CancleMatchingParams.Size = new System.Drawing.Size(121, 48);
            this.CancleMatchingParams.TabIndex = 3;
            this.CancleMatchingParams.Text = "取消";
            this.CancleMatchingParams.UseVisualStyleBackColor = true;
            this.CancleMatchingParams.Click += new System.EventHandler(this.CancleMatchingParams_Click);
            // 
            // AdjustMatchDistance
            // 
            this.AdjustMatchDistance.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AdjustMatchDistance.Location = new System.Drawing.Point(154, 91);
            this.AdjustMatchDistance.Name = "AdjustMatchDistance";
            this.AdjustMatchDistance.Size = new System.Drawing.Size(124, 40);
            this.AdjustMatchDistance.TabIndex = 4;
            this.AdjustMatchDistance.Text = "调整";
            this.AdjustMatchDistance.UseVisualStyleBackColor = true;
            this.AdjustMatchDistance.Click += new System.EventHandler(this.AdjustMatchDistance_Click);
            // 
            // ExportMatchedPtsCheckBox
            // 
            this.ExportMatchedPtsCheckBox.AutoSize = true;
            this.ExportMatchedPtsCheckBox.Checked = true;
            this.ExportMatchedPtsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportMatchedPtsCheckBox.Font = new System.Drawing.Font("宋体", 16.2F);
            this.ExportMatchedPtsCheckBox.Location = new System.Drawing.Point(12, 223);
            this.ExportMatchedPtsCheckBox.Name = "ExportMatchedPtsCheckBox";
            this.ExportMatchedPtsCheckBox.Size = new System.Drawing.Size(202, 32);
            this.ExportMatchedPtsCheckBox.TabIndex = 5;
            this.ExportMatchedPtsCheckBox.Text = "输出匹配文件";
            this.ExportMatchedPtsCheckBox.UseVisualStyleBackColor = true;
            // 
            // IsShowUnmatchedPtsCheckBox
            // 
            this.IsShowUnmatchedPtsCheckBox.AutoSize = true;
            this.IsShowUnmatchedPtsCheckBox.Checked = true;
            this.IsShowUnmatchedPtsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsShowUnmatchedPtsCheckBox.Font = new System.Drawing.Font("宋体", 16.2F);
            this.IsShowUnmatchedPtsCheckBox.Location = new System.Drawing.Point(12, 161);
            this.IsShowUnmatchedPtsCheckBox.Name = "IsShowUnmatchedPtsCheckBox";
            this.IsShowUnmatchedPtsCheckBox.Size = new System.Drawing.Size(202, 32);
            this.IsShowUnmatchedPtsCheckBox.TabIndex = 6;
            this.IsShowUnmatchedPtsCheckBox.Text = "显示未匹配点";
            this.IsShowUnmatchedPtsCheckBox.UseVisualStyleBackColor = true;
            this.IsShowUnmatchedPtsCheckBox.CheckedChanged += new System.EventHandler(this.IsShowUnmatchedPtsCheckBox_CheckedChanged);
            // 
            // MatchingParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 357);
            this.Controls.Add(this.IsShowUnmatchedPtsCheckBox);
            this.Controls.Add(this.ExportMatchedPtsCheckBox);
            this.Controls.Add(this.AdjustMatchDistance);
            this.Controls.Add(this.CancleMatchingParams);
            this.Controls.Add(this.SureMatchingParams);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MatchingParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "匹配阈值确认";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button SureMatchingParams;
        private System.Windows.Forms.Button CancleMatchingParams;
        private System.Windows.Forms.Button AdjustMatchDistance;
        private System.Windows.Forms.CheckBox ExportMatchedPtsCheckBox;
        private System.Windows.Forms.CheckBox IsShowUnmatchedPtsCheckBox;
    }
}