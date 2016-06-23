namespace vtkPointCloud
{
    partial class ClusterParameters
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
            this.textThrehold = new System.Windows.Forms.TextBox();
            this.textPoint = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.ComfirmResult = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(12, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(296, 37);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "点半径阈值（建议0.07）";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(12, 115);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(264, 37);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "阈值半径内点数(建议7)";
            // 
            // textThrehold
            // 
            this.textThrehold.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textThrehold.Location = new System.Drawing.Point(337, 34);
            this.textThrehold.Name = "textThrehold";
            this.textThrehold.Size = new System.Drawing.Size(129, 34);
            this.textThrehold.TabIndex = 2;
            this.textThrehold.Text = "0.07";
            // 
            // textPoint
            // 
            this.textPoint.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textPoint.Location = new System.Drawing.Point(337, 118);
            this.textPoint.Name = "textPoint";
            this.textPoint.Size = new System.Drawing.Size(129, 34);
            this.textPoint.TabIndex = 3;
            this.textPoint.Text = "7";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(102, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 44);
            this.button1.TabIndex = 4;
            this.button1.Text = "聚类";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(265, 191);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 44);
            this.button2.TabIndex = 5;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox3.Location = new System.Drawing.Point(38, 256);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(383, 101);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "注意：只计算左侧勾选点的聚类，DBScan算法将一个点在半径阈值内满足设定点数的标记为核心点，反之标记为误差点。";
            // 
            // ComfirmResult
            // 
            this.ComfirmResult.Enabled = false;
            this.ComfirmResult.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComfirmResult.Location = new System.Drawing.Point(166, 377);
            this.ComfirmResult.Name = "ComfirmResult";
            this.ComfirmResult.Size = new System.Drawing.Size(121, 44);
            this.ComfirmResult.TabIndex = 7;
            this.ComfirmResult.Text = "确认结果";
            this.ComfirmResult.UseVisualStyleBackColor = true;
            this.ComfirmResult.Click += new System.EventHandler(this.ComfirmResult_Click);
            // 
            // ClusterParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 441);
            this.Controls.Add(this.ComfirmResult);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textPoint);
            this.Controls.Add(this.textThrehold);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClusterParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拓展聚类";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textThrehold;
        private System.Windows.Forms.TextBox textPoint;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button ComfirmResult;
    }
}