using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace vtkPointCloud
{
    public partial class ShowSourceClustering : Form
    {
        public int rotationRb = 0;
        public bool noTrans = true;
        public bool isXTrans = false;
        public bool isYTrans = false;
        public double ss = 1;
        public double xsift = 0;
        public double ysift = 0;
        public ShowSourceClustering()
        {
            InitializeComponent();
        }

        private void OriginalDirection_CheckedChanged(object sender, EventArgs e)
        {
            if (this.OriginalDirection.Checked) {
                this.xaxialsymmetry.Checked = false;
                this.yaxialsymmetry.Checked = false;
            }
        }

        private void xaxialsymmetry_CheckedChanged(object sender, EventArgs e)
        {
            if (this.xaxialsymmetry.Checked) {
                this.OriginalDirection.Checked = false;
            }
        }

        private void yaxialsymmetry_CheckedChanged(object sender, EventArgs e)
        {
            if (this.yaxialsymmetry.Checked) {
                this.OriginalDirection.Checked = false;
            }
        }

        private void SureLocation_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AdjustLocation_Click(object sender, EventArgs e)
        {
            
            if (!((double.TryParse(this.xmove.Text, out this.xsift) && double.TryParse(this.ymove.Text, out this.ysift)
                && double.TryParse(this.scale.Text, out this.ss))))
            {
                MessageBox.Show("输入数据有误，请重新输入");
                return;
            }
            this.DialogResult = DialogResult.Yes;
            if (noRotationRb.Checked) rotationRb = 0;
            else if (Rotate90Rb.Checked) rotationRb = 1;
            else if (Rotate180Rb.Checked) rotationRb = 2;
            else if (Rotate270Rb.Checked) rotationRb = 3;

            if (xaxialsymmetry.Checked) {
                isXTrans = true;
                noTrans = false;
            }
            if (yaxialsymmetry.Checked) {
                isYTrans = true;
                noTrans = false;
            }
            this.Close();
        }

        private void CancelClustering_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
