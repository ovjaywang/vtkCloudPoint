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
            this.DoClusteringBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.ComfirmResult = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PtsInCellTxtBox = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SureMergeBtn = new System.Windows.Forms.Button();
            this.MergeBtn = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.MergeThrehold = new System.Windows.Forms.TextBox();
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
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // DoClusteringBtn
            // 
            this.DoClusteringBtn.AutoSize = true;
            this.DoClusteringBtn.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DoClusteringBtn.Location = new System.Drawing.Point(47, 237);
            this.DoClusteringBtn.Name = "DoClusteringBtn";
            this.DoClusteringBtn.Size = new System.Drawing.Size(94, 44);
            this.DoClusteringBtn.TabIndex = 4;
            this.DoClusteringBtn.Text = "聚类";
            this.DoClusteringBtn.UseVisualStyleBackColor = true;
            this.DoClusteringBtn.Click += new System.EventHandler(this.DoClusteringBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelBtn.Location = new System.Drawing.Point(227, 237);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(89, 44);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox3.Location = new System.Drawing.Point(9, 303);
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
            this.ComfirmResult.Location = new System.Drawing.Point(138, 571);
            this.ComfirmResult.Name = "ComfirmResult";
            this.ComfirmResult.Size = new System.Drawing.Size(121, 44);
            this.ComfirmResult.TabIndex = 7;
            this.ComfirmResult.Text = "确认结果";
            this.ComfirmResult.UseVisualStyleBackColor = true;
            this.ComfirmResult.Click += new System.EventHandler(this.ComfirmResult_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PtsInCellTxtBox);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textThrehold);
            this.groupBox1.Controls.Add(this.CancelBtn);
            this.groupBox1.Controls.Add(this.textPoint);
            this.groupBox1.Controls.Add(this.DoClusteringBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 411);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "聚类";
            // 
            // PtsInCellTxtBox
            // 
            this.PtsInCellTxtBox.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PtsInCellTxtBox.Location = new System.Drawing.Point(227, 197);
            this.PtsInCellTxtBox.Name = "PtsInCellTxtBox";
            this.PtsInCellTxtBox.Size = new System.Drawing.Size(129, 34);
            this.PtsInCellTxtBox.TabIndex = 8;
            this.PtsInCellTxtBox.Text = "200";
            this.PtsInCellTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox4.Location = new System.Drawing.Point(9, 194);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(196, 37);
            this.textBox4.TabIndex = 7;
            this.textBox4.Text = "分块内点数";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SureMergeBtn);
            this.groupBox2.Controls.Add(this.MergeBtn);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.MergeThrehold);
            this.groupBox2.Location = new System.Drawing.Point(11, 422);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 143);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "融合";
            // 
            // SureMergeBtn
            // 
            this.SureMergeBtn.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureMergeBtn.Location = new System.Drawing.Point(228, 76);
            this.SureMergeBtn.Name = "SureMergeBtn";
            this.SureMergeBtn.Size = new System.Drawing.Size(89, 44);
            this.SureMergeBtn.TabIndex = 8;
            this.SureMergeBtn.Text = "确认";
            this.SureMergeBtn.UseVisualStyleBackColor = true;
            this.SureMergeBtn.Click += new System.EventHandler(this.SureMergeBtn_Click);
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
            this.MergeBtn.Click += new System.EventHandler(this.MergeBtn_Click);
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
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MergeThrehold
            // 
            this.MergeThrehold.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MergeThrehold.Location = new System.Drawing.Point(228, 24);
            this.MergeThrehold.Name = "MergeThrehold";
            this.MergeThrehold.Size = new System.Drawing.Size(129, 34);
            this.MergeThrehold.TabIndex = 7;
            this.MergeThrehold.Text = "0.1";
            this.MergeThrehold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ClusterParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 627);
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
        public System.Windows.Forms.Button DoClusteringBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button ComfirmResult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox MergeThrehold;
        public System.Windows.Forms.Button MergeBtn;
        private System.Windows.Forms.TextBox PtsInCellTxtBox;
        private System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.Button SureMergeBtn;
    }
}