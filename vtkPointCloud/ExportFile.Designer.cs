namespace vtkPointCloud
{
    partial class ExportFile
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.out1btn = new System.Windows.Forms.Button();
            this.outpath1txt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.out2btn = new System.Windows.Forms.Button();
            this.outpath2txt = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.out3btn = new System.Windows.Forms.Button();
            this.outpath3txt = new System.Windows.Forms.TextBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SureBtn = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Location = new System.Drawing.Point(13, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1169, 138);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出聚类结果";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.out1btn);
            this.panel2.Controls.Add(this.outpath1txt);
            this.panel2.Location = new System.Drawing.Point(6, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1157, 69);
            this.panel2.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.checkBox1.Location = new System.Drawing.Point(22, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 24);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "保存聚类文件";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // out1btn
            // 
            this.out1btn.Font = new System.Drawing.Font("宋体", 12F);
            this.out1btn.Location = new System.Drawing.Point(209, 12);
            this.out1btn.Name = "out1btn";
            this.out1btn.Size = new System.Drawing.Size(114, 40);
            this.out1btn.TabIndex = 11;
            this.out1btn.Text = "选择路径";
            this.out1btn.UseVisualStyleBackColor = true;
            this.out1btn.Click += new System.EventHandler(this.out1btn_Click);
            // 
            // outpath1txt
            // 
            this.outpath1txt.Font = new System.Drawing.Font("宋体", 12F);
            this.outpath1txt.Location = new System.Drawing.Point(355, 22);
            this.outpath1txt.Name = "outpath1txt";
            this.outpath1txt.ReadOnly = true;
            this.outpath1txt.Size = new System.Drawing.Size(787, 30);
            this.outpath1txt.TabIndex = 10;
            this.outpath1txt.Text = "未选择路径";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(13, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1169, 138);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出质心结果";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.out2btn);
            this.panel1.Controls.Add(this.outpath2txt);
            this.panel1.Location = new System.Drawing.Point(6, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1157, 69);
            this.panel1.TabIndex = 16;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Font = new System.Drawing.Font("宋体", 12F);
            this.checkBox2.Location = new System.Drawing.Point(22, 20);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(151, 24);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "保存质心文件";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // out2btn
            // 
            this.out2btn.Font = new System.Drawing.Font("宋体", 12F);
            this.out2btn.Location = new System.Drawing.Point(209, 11);
            this.out2btn.Name = "out2btn";
            this.out2btn.Size = new System.Drawing.Size(114, 40);
            this.out2btn.TabIndex = 11;
            this.out2btn.Text = "选择路径";
            this.out2btn.UseVisualStyleBackColor = true;
            this.out2btn.Click += new System.EventHandler(this.out2btn_Click);
            // 
            // outpath2txt
            // 
            this.outpath2txt.Font = new System.Drawing.Font("宋体", 12F);
            this.outpath2txt.Location = new System.Drawing.Point(355, 21);
            this.outpath2txt.Name = "outpath2txt";
            this.outpath2txt.ReadOnly = true;
            this.outpath2txt.Size = new System.Drawing.Size(787, 30);
            this.outpath2txt.TabIndex = 10;
            this.outpath2txt.Text = "未选择路径";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel3);
            this.groupBox3.Location = new System.Drawing.Point(12, 312);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1169, 138);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "输出匹配结果";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBox3);
            this.panel3.Controls.Add(this.out3btn);
            this.panel3.Controls.Add(this.outpath3txt);
            this.panel3.Location = new System.Drawing.Point(6, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1157, 69);
            this.panel3.TabIndex = 16;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Font = new System.Drawing.Font("宋体", 12F);
            this.checkBox3.Location = new System.Drawing.Point(23, 18);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(151, 24);
            this.checkBox3.TabIndex = 9;
            this.checkBox3.Text = "保存匹配文件";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // out3btn
            // 
            this.out3btn.Font = new System.Drawing.Font("宋体", 12F);
            this.out3btn.Location = new System.Drawing.Point(210, 9);
            this.out3btn.Name = "out3btn";
            this.out3btn.Size = new System.Drawing.Size(114, 40);
            this.out3btn.TabIndex = 11;
            this.out3btn.Text = "选择路径";
            this.out3btn.UseVisualStyleBackColor = true;
            this.out3btn.Click += new System.EventHandler(this.out3btn_Click);
            // 
            // outpath3txt
            // 
            this.outpath3txt.Font = new System.Drawing.Font("宋体", 12F);
            this.outpath3txt.Location = new System.Drawing.Point(356, 19);
            this.outpath3txt.Name = "outpath3txt";
            this.outpath3txt.ReadOnly = true;
            this.outpath3txt.Size = new System.Drawing.Size(787, 30);
            this.outpath3txt.TabIndex = 10;
            this.outpath3txt.Text = "未选择路径";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("宋体", 15F);
            this.CancelBtn.Location = new System.Drawing.Point(648, 456);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(114, 55);
            this.CancelBtn.TabIndex = 23;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SureBtn
            // 
            this.SureBtn.Font = new System.Drawing.Font("宋体", 15F);
            this.SureBtn.Location = new System.Drawing.Point(328, 456);
            this.SureBtn.Name = "SureBtn";
            this.SureBtn.Size = new System.Drawing.Size(114, 55);
            this.SureBtn.TabIndex = 22;
            this.SureBtn.Text = "确认";
            this.SureBtn.UseVisualStyleBackColor = true;
            this.SureBtn.Click += new System.EventHandler(this.SureBtn_Click);
            // 
            // ExportFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 527);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SureBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "ExportFile";
            this.Text = "导出数据";
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button out1btn;
        private System.Windows.Forms.TextBox outpath1txt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button out2btn;
        private System.Windows.Forms.TextBox outpath2txt;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button out3btn;
        private System.Windows.Forms.TextBox outpath3txt;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SureBtn;
    }
}