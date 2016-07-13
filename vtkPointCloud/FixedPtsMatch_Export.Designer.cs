namespace vtkPointCloud
{
    partial class FixedPtsMatch_Export
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
            this.pathSelbtn = new System.Windows.Forms.Button();
            this.PathSeltxt = new System.Windows.Forms.TextBox();
            this.pathSel2Btn = new System.Windows.Forms.Button();
            this.PathSeltxt2 = new System.Windows.Forms.TextBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SureBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pathSelbtn
            // 
            this.pathSelbtn.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pathSelbtn.Location = new System.Drawing.Point(30, 46);
            this.pathSelbtn.Name = "pathSelbtn";
            this.pathSelbtn.Size = new System.Drawing.Size(397, 42);
            this.pathSelbtn.TabIndex = 4;
            this.pathSelbtn.Text = "选择与固定点对应真值点文件路径";
            this.pathSelbtn.UseVisualStyleBackColor = true;
            this.pathSelbtn.Click += new System.EventHandler(this.pathSelbtn_Click);
            // 
            // PathSeltxt
            // 
            this.PathSeltxt.Font = new System.Drawing.Font("宋体", 16F);
            this.PathSeltxt.Location = new System.Drawing.Point(445, 50);
            this.PathSeltxt.Name = "PathSeltxt";
            this.PathSeltxt.ReadOnly = true;
            this.PathSeltxt.Size = new System.Drawing.Size(726, 38);
            this.PathSeltxt.TabIndex = 3;
            this.PathSeltxt.Text = "未选择路径";
            // 
            // pathSel2Btn
            // 
            this.pathSel2Btn.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pathSel2Btn.Location = new System.Drawing.Point(30, 142);
            this.pathSel2Btn.Name = "pathSel2Btn";
            this.pathSel2Btn.Size = new System.Drawing.Size(397, 42);
            this.pathSel2Btn.TabIndex = 6;
            this.pathSel2Btn.Text = "选择固定点点名匹配输出路径";
            this.pathSel2Btn.UseVisualStyleBackColor = true;
            this.pathSel2Btn.Click += new System.EventHandler(this.pathSel2Btn_Click);
            // 
            // PathSeltxt2
            // 
            this.PathSeltxt2.Font = new System.Drawing.Font("宋体", 16F);
            this.PathSeltxt2.Location = new System.Drawing.Point(445, 142);
            this.PathSeltxt2.Name = "PathSeltxt2";
            this.PathSeltxt2.ReadOnly = true;
            this.PathSeltxt2.Size = new System.Drawing.Size(726, 38);
            this.PathSeltxt2.TabIndex = 5;
            this.PathSeltxt2.Text = "未选择路径";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelBtn.Location = new System.Drawing.Point(702, 222);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(132, 42);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SureBtn
            // 
            this.SureBtn.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SureBtn.Location = new System.Drawing.Point(239, 222);
            this.SureBtn.Name = "SureBtn";
            this.SureBtn.Size = new System.Drawing.Size(132, 42);
            this.SureBtn.TabIndex = 8;
            this.SureBtn.Text = "确认";
            this.SureBtn.UseVisualStyleBackColor = true;
            this.SureBtn.Click += new System.EventHandler(this.SureBtn_Click);
            // 
            // FixedPtsMatch_Export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 286);
            this.Controls.Add(this.SureBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.pathSel2Btn);
            this.Controls.Add(this.PathSeltxt2);
            this.Controls.Add(this.pathSelbtn);
            this.Controls.Add(this.PathSeltxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FixedPtsMatch_Export";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入真值并输出匹配";
            this.Load += new System.EventHandler(this.FixedPtsMatch_Export_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pathSelbtn;
        private System.Windows.Forms.TextBox PathSeltxt;
        private System.Windows.Forms.Button pathSel2Btn;
        private System.Windows.Forms.TextBox PathSeltxt2;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SureBtn;
    }
}