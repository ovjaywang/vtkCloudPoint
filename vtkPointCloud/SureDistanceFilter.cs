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
            if (!((double.TryParse(this.distanceThreholdtxtBox.Text, out distanceFilterThrehold))))
            {
                MessageBox.Show("输入数据有误，请重新输入");
                return;
            }
           mf.testParamsTrans(checkBox1.Checked, this.distanceFilterThrehold);
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            MainForm mf = (MainForm)this.Owner;
            mf.cleanDataByDistance();
            this.DialogResult = DialogResult.OK;
            this.Close();     
        }

    }
}
