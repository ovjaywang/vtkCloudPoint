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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MergeThrehold = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.MergeBtn = new System.Windows.Forms.Button();
            this.SureMergeBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(6, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(196, 37);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "点半径阈值";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(6, 128);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(196, 37);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "阈值内点数";
            // 
            // textThrehold
            // 
            this.textThrehold.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textThrehold.Location = new System.Drawing.Point(227, 47);
            this.textThrehold.Name = "textThrehold";
            this.textThrehold.Size = new System.Drawing.Size(129, 34);
            this.textThrehold.TabIndex = 2;
            this.textThrehold.Text = "0.07";
            this.textThrehold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textPoint
            // 
            this.textPoint.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textPoint.Location = new System.Drawing.Point(227, 128);
            this.textPoint.Name = "textPoint";
            this.textPoint.Size = new System.Drawing.Size(129, 34);
            this.textPoint.TabIndex = 3;
            this.textPoint.Text = "7";
            this.textPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(37, 171);
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
            this.button2.Location = new System.Drawing.Point(203, 171);
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
            this.textBox3.Location = new System.Drawing.Point(6, 221);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(369, 101);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "注意：只计算左侧勾选点的聚类，DBScan算法将一个点在半径阈值内满足设定点数的标记为核心点，反之标记为误差点。";
            // 
            // ComfirmResult
            // 
            this.ComfirmResult.Enabled = false;
            this.ComfirmResult.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComfirmResult.Location = new System.Drawing.Point(130, 510);
            this.ComfirmResult.Name = "ComfirmResult";
            this.ComfirmResult.Size = new System.Drawing.Size(121, 44);
            this.ComfirmResult.TabIndex = 7;
            this.ComfirmResult.Text = "确认结果";
            this.ComfirmResult.UseVisualStyleBackColor = true;
            this.ComfirmResult.Click += new System.EventHandler(this.ComfirmResult_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textThrehold);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textPoint);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 333);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "聚类";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SureMergeBtn);
            this.groupBox2.Controls.Add(this.MergeBtn);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.MergeThrehold);
            this.groupBox2.Location = new System.Drawing.Point(11, 351);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 143);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "融合";
            // 
            // MergeThrehold
            // 
            this.MergeThrehold.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MergeThrehold.Location = new System.Drawing.Point(228, 24);
            this.MergeThrehold.Name = "MergeThrehold";
            this.MergeThrehold.Size = new System.Drawing.Size(129, 34);
            this.MergeThrehold.TabIndex = 7;
            this.MergeThrehold.Text = "0";
            this.MergeThrehold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox5.Location = new System.Drawing.Point(7, 24);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(206, 37);
            this.textBox5.TabIndex = 7;
            this.textBox5.Text = "阈值内点集融合";
            // 
            // MergeBtn
            // 
            this.MergeBtn.AutoSize = true;
            this.MergeBtn.Enabled = false;
            this.MergeBtn.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MergeBtn.Location = new System.Drawing.Point(48, 76);
            this.MergeBtn.Name = "MergeBtn";
            this.MergeBtn.Size = new System.Drawing.Size(94, 44);
            this.MergeBtn.TabIndex = 7;
            this.MergeBtn.Text = "融合";
            this.MergeBtn.UseVisualStyleBackColor = true;
            // 
            // SureMergeBtn
            // 
            this.SureMergeBtn.Enabled = false;
            this.SureMergeBtn.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureMergeBtn.Location = new System.Drawing.Point(228, 76);
            this.SureMergeBtn.Name = "SureMergeBtn";
            this.SureMergeBtn.Size = new System.Drawing.Size(89, 44);
            this.SureMergeBtn.TabIndex = 8;
            this.SureMergeBtn.Text = "确认";
            this.SureMergeBtn.UseVisualStyleBackColor = true;
            // 
            // ClusterParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 563);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ComfirmResult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ClusterParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拓展聚类";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox MergeThrehold;
        private System.Windows.Forms.Button SureMergeBtn;
        public System.Windows.Forms.Button MergeBtn;
    }
}