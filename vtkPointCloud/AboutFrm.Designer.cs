namespace vtkPointCloud
{
    partial class AboutFrm
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
            this.aboutTxtBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // aboutTxtBox
            // 
            this.aboutTxtBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.aboutTxtBox.Location = new System.Drawing.Point(41, 34);
            this.aboutTxtBox.Multiline = true;
            this.aboutTxtBox.Name = "aboutTxtBox";
            this.aboutTxtBox.ReadOnly = true;
            this.aboutTxtBox.Size = new System.Drawing.Size(467, 152);
            this.aboutTxtBox.TabIndex = 0;
            this.aboutTxtBox.TabStop = false;
            // 
            // AboutFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 212);
            this.Controls.Add(this.aboutTxtBox);
            this.Name = "AboutFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox aboutTxtBox;
    }
}