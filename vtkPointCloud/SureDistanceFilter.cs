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
    public partial class SureDistanceFilter : Form
    {
        public double distanceFilterThrehold = 0.0;
        public SureDistanceFilter()
        {
            InitializeComponent();
        }

        private void Refilter_Click(object sender, EventArgs e)
        {
            MainForm mf = (MainForm)this.Owner;
            mf.distanceFilterThrehold = Convert.ToDouble(this.distanceThreholdtxtBox.Text);
            mf.testParamsTrans();
        }

    }
}
