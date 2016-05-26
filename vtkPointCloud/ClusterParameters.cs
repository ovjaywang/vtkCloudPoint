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
    public partial class ClusterParameters : Form
    {
        public double threhold;
        public int point;
        public ClusterParameters()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(this.textPoint.Text, out this.point))
            {
                MessageBox.Show("输入的不是整数，请重新输入");
                return;
            }
            if (!double.TryParse(this.textThrehold.Text,out this.threhold)) {
                MessageBox.Show("输入的不是浮点数，请重新输入");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
