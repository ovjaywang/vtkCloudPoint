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
    public partial class MCC_MCE : Form
    {
        public int ptsRb = 0;
        public double Threshold = 0.0;
        public MCC_MCE()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(this.threshold_txtbox.Text, out this.Threshold))
            {
                MessageBox.Show("输入的不是浮点数，请重新输入");
                return;
            }
            if (this.MCCrb.Checked) ptsRb = 2;
            else if (this.MCErb.Checked) ptsRb = 1;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void MCCrb_CheckedChanged(object sender, EventArgs e)
        {
            if (MCCrb.Checked)
            {
                this.threshold_txtbox.Text = "0.09";
            }
            else {
                this.threshold_txtbox.Text = "1.5";
            }
        }
    }
}
