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
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(124, 160);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(165, 38);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "110";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(55, 12);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(303, 129);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "请输入阈值，阈值范围内视为两者匹配。其中红色点为已匹配点，并用白线连接。绿色点为物方真值未匹配点，蓝色为质心未匹配点。";
            // 
            // SureMatchingParams
            // 
            this.SureMatchingParams.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureMatchingParams.Location = new System.Drawing.Point(55, 217);
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
            this.CancleMatchingParams.Location = new System.Drawing.Point(237, 217);
            this.CancleMatchingParams.Name = "CancleMatchingParams";
            this.CancleMatchingParams.Size = new System.Drawing.Size(121, 48);
            this.CancleMatchingParams.TabIndex = 3;
            this.CancleMatchingParams.Text = "取消";
            this.CancleMatchingParams.UseVisualStyleBackColor = true;
            this.CancleMatchingParams.Click += new System.EventHandler(this.CancleMatchingParams_Click);
            // 
            // MatchingParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 317);
            this.Controls.Add(this.CancleMatchingParams);
            this.Controls.Add(this.SureMatchingParams);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MatchingParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "匹配阈值确认";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button SureMatchingParams;
        private System.Windows.Forms.Button CancleMatchingParams;
    }
}