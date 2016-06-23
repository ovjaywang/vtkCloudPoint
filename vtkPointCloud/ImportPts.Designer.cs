namespace vtkPointCloud
{
    partial class ImportPts
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
            this.PathSeltxt = new System.Windows.Forms.TextBox();
            this.pathSelbtn = new System.Windows.Forms.Button();
            this.clearAllRb = new System.Windows.Forms.RadioButton();
            this.noClearRb = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.xia_rb1 = new System.Windows.Forms.RadioButton();
            this.you_rb1 = new System.Windows.Forms.RadioButton();
            this.zuo_rb1 = new System.Windows.Forms.RadioButton();
            this.shang_rb1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.you_rb2 = new System.Windows.Forms.RadioButton();
            this.zuo_rb2 = new System.Windows.Forms.RadioButton();
            this.xia_rb2 = new System.Windows.Forms.RadioButton();
            this.shang_rb2 = new System.Windows.Forms.RadioButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.x_angle_TxtBox = new System.Windows.Forms.TextBox();
            this.y_angle_TxtBox = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // PathSeltxt
            // 
            this.PathSeltxt.Font = new System.Drawing.Font("宋体", 16F);
            this.PathSeltxt.Location = new System.Drawing.Point(177, 40);
            this.PathSeltxt.Name = "PathSeltxt";
            this.PathSeltxt.ReadOnly = true;
            this.PathSeltxt.Size = new System.Drawing.Size(586, 38);
            this.PathSeltxt.TabIndex = 0;
            this.PathSeltxt.Text = "未选择路径";
            // 
            // pathSelbtn
            // 
            this.pathSelbtn.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pathSelbtn.Location = new System.Drawing.Point(33, 36);
            this.pathSelbtn.Name = "pathSelbtn";
            this.pathSelbtn.Size = new System.Drawing.Size(132, 42);
            this.pathSelbtn.TabIndex = 1;
            this.pathSelbtn.Text = "选择路径";
            this.pathSelbtn.UseVisualStyleBackColor = true;
            this.pathSelbtn.Click += new System.EventHandler(this.pathSelbtn_Click);
            // 
            // clearAllRb
            // 
            this.clearAllRb.AutoSize = true;
            this.clearAllRb.Checked = true;
            this.clearAllRb.Font = new System.Drawing.Font("宋体", 15F);
            this.clearAllRb.Location = new System.Drawing.Point(23, 27);
            this.clearAllRb.Name = "clearAllRb";
            this.clearAllRb.Size = new System.Drawing.Size(208, 29);
            this.clearAllRb.TabIndex = 2;
            this.clearAllRb.TabStop = true;
            this.clearAllRb.Text = "清除完全重复点";
            this.clearAllRb.UseVisualStyleBackColor = true;
            // 
            // noClearRb
            // 
            this.noClearRb.AutoSize = true;
            this.noClearRb.Font = new System.Drawing.Font("宋体", 15F);
            this.noClearRb.Location = new System.Drawing.Point(251, 27);
            this.noClearRb.Name = "noClearRb";
            this.noClearRb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.noClearRb.Size = new System.Drawing.Size(158, 29);
            this.noClearRb.TabIndex = 3;
            this.noClearRb.Text = "不清除数据";
            this.noClearRb.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(489, 479);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 44);
            this.button1.TabIndex = 7;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(637, 479);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 44);
            this.button2.TabIndex = 8;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clearAllRb);
            this.groupBox1.Controls.Add(this.noClearRb);
            this.groupBox1.Location = new System.Drawing.Point(33, 460);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 63);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(98, 48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(100, 48);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(128, 128);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.xia_rb1);
            this.groupBox2.Controls.Add(this.you_rb1);
            this.groupBox2.Controls.Add(this.zuo_rb1);
            this.groupBox2.Controls.Add(this.shang_rb1);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(33, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 252);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "x正方向";
            // 
            // xia_rb1
            // 
            this.xia_rb1.AutoSize = true;
            this.xia_rb1.Location = new System.Drawing.Point(177, 230);
            this.xia_rb1.Name = "xia_rb1";
            this.xia_rb1.Size = new System.Drawing.Size(17, 16);
            this.xia_rb1.TabIndex = 14;
            this.xia_rb1.UseVisualStyleBackColor = true;
            // 
            // you_rb1
            // 
            this.you_rb1.AutoSize = true;
            this.you_rb1.Checked = true;
            this.you_rb1.Location = new System.Drawing.Point(281, 116);
            this.you_rb1.Name = "you_rb1";
            this.you_rb1.Size = new System.Drawing.Size(17, 16);
            this.you_rb1.TabIndex = 13;
            this.you_rb1.TabStop = true;
            this.you_rb1.UseVisualStyleBackColor = true;
            // 
            // zuo_rb1
            // 
            this.zuo_rb1.AutoSize = true;
            this.zuo_rb1.Location = new System.Drawing.Point(58, 116);
            this.zuo_rb1.Name = "zuo_rb1";
            this.zuo_rb1.Size = new System.Drawing.Size(17, 16);
            this.zuo_rb1.TabIndex = 12;
            this.zuo_rb1.UseVisualStyleBackColor = true;
            // 
            // shang_rb1
            // 
            this.shang_rb1.AutoSize = true;
            this.shang_rb1.Location = new System.Drawing.Point(177, 14);
            this.shang_rb1.Name = "shang_rb1";
            this.shang_rb1.Size = new System.Drawing.Size(17, 16);
            this.shang_rb1.TabIndex = 11;
            this.shang_rb1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.you_rb2);
            this.groupBox3.Controls.Add(this.zuo_rb2);
            this.groupBox3.Controls.Add(this.xia_rb2);
            this.groupBox3.Controls.Add(this.shang_rb2);
            this.groupBox3.Controls.Add(this.pictureBox2);
            this.groupBox3.Location = new System.Drawing.Point(410, 202);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(337, 252);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "y正方向";
            // 
            // you_rb2
            // 
            this.you_rb2.AutoSize = true;
            this.you_rb2.Location = new System.Drawing.Point(286, 116);
            this.you_rb2.Name = "you_rb2";
            this.you_rb2.Size = new System.Drawing.Size(17, 16);
            this.you_rb2.TabIndex = 18;
            this.you_rb2.UseVisualStyleBackColor = true;
            // 
            // zuo_rb2
            // 
            this.zuo_rb2.AutoSize = true;
            this.zuo_rb2.Location = new System.Drawing.Point(66, 116);
            this.zuo_rb2.Name = "zuo_rb2";
            this.zuo_rb2.Size = new System.Drawing.Size(17, 16);
            this.zuo_rb2.TabIndex = 17;
            this.zuo_rb2.UseVisualStyleBackColor = true;
            // 
            // xia_rb2
            // 
            this.xia_rb2.AutoSize = true;
            this.xia_rb2.Location = new System.Drawing.Point(173, 230);
            this.xia_rb2.Name = "xia_rb2";
            this.xia_rb2.Size = new System.Drawing.Size(17, 16);
            this.xia_rb2.TabIndex = 16;
            this.xia_rb2.UseVisualStyleBackColor = true;
            // 
            // shang_rb2
            // 
            this.shang_rb2.AutoSize = true;
            this.shang_rb2.Checked = true;
            this.shang_rb2.Location = new System.Drawing.Point(173, 14);
            this.shang_rb2.Name = "shang_rb2";
            this.shang_rb2.Size = new System.Drawing.Size(17, 16);
            this.shang_rb2.TabIndex = 15;
            this.shang_rb2.TabStop = true;
            this.shang_rb2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox2.Location = new System.Drawing.Point(33, 120);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(178, 36);
            this.textBox2.TabIndex = 14;
            this.textBox2.Text = "x电机零位角度";
            // 
            // x_angle_TxtBox
            // 
            this.x_angle_TxtBox.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.x_angle_TxtBox.Location = new System.Drawing.Point(223, 118);
            this.x_angle_TxtBox.Name = "x_angle_TxtBox";
            this.x_angle_TxtBox.Size = new System.Drawing.Size(139, 38);
            this.x_angle_TxtBox.TabIndex = 15;
            this.x_angle_TxtBox.Text = "149";
            // 
            // y_angle_TxtBox
            // 
            this.y_angle_TxtBox.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.y_angle_TxtBox.Location = new System.Drawing.Point(590, 118);
            this.y_angle_TxtBox.Name = "y_angle_TxtBox";
            this.y_angle_TxtBox.Size = new System.Drawing.Size(170, 38);
            this.y_angle_TxtBox.TabIndex = 17;
            this.y_angle_TxtBox.Text = "307";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox3.Location = new System.Drawing.Point(395, 120);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(178, 36);
            this.textBox3.TabIndex = 16;
            this.textBox3.Text = "y电机零位角度";
            // 
            // ImportPts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 554);
            this.Controls.Add(this.y_angle_TxtBox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.x_angle_TxtBox);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pathSelbtn);
            this.Controls.Add(this.PathSeltxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(1200, 800);
            this.Name = "ImportPts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入点云文件夹";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PathSeltxt;
        private System.Windows.Forms.Button pathSelbtn;
        private System.Windows.Forms.RadioButton clearAllRb;
        private System.Windows.Forms.RadioButton noClearRb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton xia_rb1;
        private System.Windows.Forms.RadioButton you_rb1;
        private System.Windows.Forms.RadioButton zuo_rb1;
        private System.Windows.Forms.RadioButton shang_rb1;
        private System.Windows.Forms.RadioButton you_rb2;
        private System.Windows.Forms.RadioButton zuo_rb2;
        private System.Windows.Forms.RadioButton xia_rb2;
        private System.Windows.Forms.RadioButton shang_rb2;
        private System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.TextBox x_angle_TxtBox;
        public System.Windows.Forms.TextBox y_angle_TxtBox;
        private System.Windows.Forms.TextBox textBox3;
    }
}