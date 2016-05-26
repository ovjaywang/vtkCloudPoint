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
    public partial class MatchingParams : Form
    {
        public MatchingParams()
        {
            InitializeComponent();
        }
        public double matchingParams;
        private void SureMatchingParams_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(this.textBox1.Text, out this.matchingParams))
            {
                MessageBox.Show("输入的不是整数，请重新输入");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancleMatchingParams_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
