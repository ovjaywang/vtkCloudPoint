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
        public double distanceMax = 0.0;
        public double distanceMin = 0.0;
        public SureDistanceFilter()
        {
            InitializeComponent();
        }

        private void Refilter_Click(object sender, EventArgs e)
        {
            MainForm mf = (MainForm)this.Owner;
            if (!(double.TryParse(this.ThresholdMaxTxtBox.Text, out distanceMax)))
            {
                MessageBox.Show("最大阈值输入数据有误，请重新输入");
                return;
            }
            else if (!(double.TryParse(this.ThresholdMinTxtBox.Text, out distanceMin)))
            {
                MessageBox.Show("最小阈值输入数据有误，请重新输入");
                return;
            }
            else if (distanceMax < distanceMin)
            {
                MessageBox.Show("最小值不能比最大值大");
                return;
            }
            mf.testParamsTrans(this.checkBox1.Checked,this.distanceMax, this.distanceMin);
        }
        private void OKBtn_Click(object sender, EventArgs e){
            MainForm mf = (MainForm)this.Owner;
            mf.cleanDataByDistance();
            this.DialogResult = DialogResult.OK;
            this.Close();     
        }

    }
}
