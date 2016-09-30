namespace vtkPointCloud
{
    partial class Clustering
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
            this.ComfirmResult = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PtsInCellTxtBox = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SureMergeBtn = new System.Windows.Forms.Button();
            this.MergeBtn = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.MergeThrehold = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb_2d = new System.Windows.Forms.RadioButton();
            this.rb_3d = new System.Windows.Forms.RadioButton();
            this.cb_showcentroid = new System.Windows.Forms.CheckBox();
            this.cb_showcore = new System.Windows.Forms.CheckBox();
            this.cb_showerror = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("华文仿宋", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(6, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(196, 37);
            this.textBox1.TabIndex = 9;
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
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "阈值内点数";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textThrehold
            // 
            this.textThrehold.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textThrehold.Location = new System.Drawing.Point(227, 47);
            this.textThrehold.Name = "textThrehold";
            this.textThrehold.Size = new System.Drawing.Size(129, 34);
            this.textThrehold.TabIndex = 0;
            this.textThrehold.Text = "0.07";
            this.textThrehold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textPoint
            // 
            this.textPoint.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textPoint.Location = new System.Drawing.Point(227, 128);
            this.textPoint.Name = "textPoint";
            this.textPoint.Size = new System.Drawing.Size(129, 34);
            this.textPoint.TabIndex = 1;
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
            this.DoClusteringBtn.TabIndex = 8;
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
            // ComfirmResult
            // 
            this.ComfirmResult.Enabled = false;
            this.ComfirmResult.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComfirmResult.Location = new System.Drawing.Point(133, 593);
            this.ComfirmResult.Name = "ComfirmResult";
            this.ComfirmResult.Size = new System.Drawing.Size(121, 44);
            this.ComfirmResult.TabIndex = 15;
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
            this.groupBox1.Controls.Add(this.textThrehold);
            this.groupBox1.Controls.Add(this.CancelBtn);
            this.groupBox1.Controls.Add(this.textPoint);
            this.groupBox1.Controls.Add(this.DoClusteringBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 289);
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
            this.PtsInCellTxtBox.TabIndex = 2;
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
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "分块内点数";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SureMergeBtn);
            this.groupBox2.Controls.Add(this.MergeBtn);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.MergeThrehold);
            this.groupBox2.Location = new System.Drawing.Point(11, 444);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 143);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "融合";
            // 
            // SureMergeBtn
            // 
            this.SureMergeBtn.Enabled = false;
            this.SureMergeBtn.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureMergeBtn.Location = new System.Drawing.Point(228, 76);
            this.SureMergeBtn.Name = "SureMergeBtn";
            this.SureMergeBtn.Size = new System.Drawing.Size(89, 44);
            this.SureMergeBtn.TabIndex = 14;
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
            this.MergeBtn.TabIndex = 13;
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
            this.textBox5.TabIndex = 12;
            this.textBox5.Text = "阈值内点集融合";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MergeThrehold
            // 
            this.MergeThrehold.Font = new System.Drawing.Font("华文仿宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MergeThrehold.Location = new System.Drawing.Point(228, 24);
            this.MergeThrehold.Name = "MergeThrehold";
            this.MergeThrehold.Size = new System.Drawing.Size(129, 34);
            this.MergeThrehold.TabIndex = 8;
            this.MergeThrehold.Text = "0.1";
            this.MergeThrehold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rb_2d);
            this.groupBox3.Controls.Add(this.rb_3d);
            this.groupBox3.Controls.Add(this.cb_showcentroid);
            this.groupBox3.Controls.Add(this.cb_showcore);
            this.groupBox3.Controls.Add(this.cb_showerror);
            this.groupBox3.Location = new System.Drawing.Point(21, 316);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(373, 122);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "显示";
            // 
            // rb_2d
            // 
            this.rb_2d.AutoSize = true;
            this.rb_2d.Enabled = false;
            this.rb_2d.Location = new System.Drawing.Point(204, 86);
            this.rb_2d.Name = "rb_2d";
            this.rb_2d.Size = new System.Drawing.Size(116, 19);
            this.rb_2d.TabIndex = 7;
            this.rb_2d.Text = "2D-Motor_xy";
            this.rb_2d.UseVisualStyleBackColor = true;
            // 
            // rb_3d
            // 
            this.rb_3d.AutoSize = true;
            this.rb_3d.Checked = true;
            this.rb_3d.Enabled = false;
            this.rb_3d.Location = new System.Drawing.Point(25, 86);
            this.rb_3d.Name = "rb_3d";
            this.rb_3d.Size = new System.Drawing.Size(76, 19);
            this.rb_3d.TabIndex = 6;
            this.rb_3d.TabStop = true;
            this.rb_3d.Text = "3D-XYZ";
            this.rb_3d.UseVisualStyleBackColor = true;
            this.rb_3d.CheckedChanged += new System.EventHandler(this.rb_3d_CheckedChanged);
            // 
            // cb_showcentroid
            // 
            this.cb_showcentroid.AutoSize = true;
            this.cb_showcentroid.Checked = true;
            this.cb_showcentroid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_showcentroid.Enabled = false;
            this.cb_showcentroid.Location = new System.Drawing.Point(258, 35);
            this.cb_showcentroid.Name = "cb_showcentroid";
            this.cb_showcentroid.Size = new System.Drawing.Size(89, 19);
            this.cb_showcentroid.TabIndex = 5;
            this.cb_showcentroid.Text = "显示质心";
            this.cb_showcentroid.UseVisualStyleBackColor = true;
            this.cb_showcentroid.CheckedChanged += new System.EventHandler(this.cb_showcentroid_CheckedChanged);
            // 
            // cb_showcore
            // 
            this.cb_showcore.AutoSize = true;
            this.cb_showcore.Checked = true;
            this.cb_showcore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_showcore.Enabled = false;
            this.cb_showcore.Location = new System.Drawing.Point(6, 35);
            this.cb_showcore.Name = "cb_showcore";
            this.cb_showcore.Size = new System.Drawing.Size(104, 19);
            this.cb_showcore.TabIndex = 3;
            this.cb_showcore.Text = "显示核心点";
            this.cb_showcore.UseVisualStyleBackColor = true;
            this.cb_showcore.CheckedChanged += new System.EventHandler(this.cb_showcore_CheckedChanged);
            // 
            // cb_showerror
            // 
            this.cb_showerror.AutoSize = true;
            this.cb_showerror.Checked = true;
            this.cb_showerror.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_showerror.Enabled = false;
            this.cb_showerror.Location = new System.Drawing.Point(135, 35);
            this.cb_showerror.Name = "cb_showerror";
            this.cb_showerror.Size = new System.Drawing.Size(89, 19);
            this.cb_showerror.TabIndex = 4;
            this.cb_showerror.Text = "显示野点";
            this.cb_showerror.UseVisualStyleBackColor = true;
            this.cb_showerror.CheckedChanged += new System.EventHandler(this.cb_showerror_CheckedChanged);
            // 
            // Clustering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 649);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ComfirmResult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Clustering";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拓展聚类";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textThrehold;
        private System.Windows.Forms.TextBox textPoint;
        public System.Windows.Forms.Button DoClusteringBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button ComfirmResult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox MergeThrehold;
        public System.Windows.Forms.Button MergeBtn;
        private System.Windows.Forms.TextBox PtsInCellTxtBox;
        private System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.Button SureMergeBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox cb_showcentroid;
        public System.Windows.Forms.CheckBox cb_showcore;
        public System.Windows.Forms.CheckBox cb_showerror;
        public System.Windows.Forms.RadioButton rb_2d;
        public System.Windows.Forms.RadioButton rb_3d;
    }
}