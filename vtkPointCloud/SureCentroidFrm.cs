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
    public partial class SureCentroidFrm : Form
    {
        public double step_x;
        public double step_y;
        public double xs;
        public double ys;
        public SureCentroidFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!((double.TryParse(this.xstepTextBox.Text, out this.step_x)
                && double.TryParse(this.ystepTextBox.Text, out this.step_y)
                && double.TryParse(this.xshift.Text, out this.xs)
                && double.TryParse(this.yshift.Text, out this.ys))))
            {
                MessageBox.Show("输入数据有误，请重新输入");
                return;
            }
            this.DialogResult = DialogResult.Yes;
        }
    }
}
