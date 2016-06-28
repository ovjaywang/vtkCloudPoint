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
        private bool isFirst = true;
        private bool isFix = false;
        private bool isClose = true;
        MainForm mf;
        public SureDistanceFilter(bool isFixedPoint)
        {
            InitializeComponent();
            this.isFix = isFixedPoint;
        }

        private void Refilter_Click(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
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
            if (!isFix)
            {//判断是否是固定点
                Tools.FilterByDistance_ScanPoint(mf.rawData, this.distanceMax, this.distanceMin);
                mf.ShowPointsFromFile(mf.rawData, 6+(this.checkBox1.Checked?0:1));
            }
            else
            { 
                Tools.FilterByDistance_FixedPoint(mf.grouping, this.distanceMax, this.distanceMin);
                mf.showFixPointData(2 + (this.checkBox1.Checked ? 0 : 2));//显示剔野之后数据
            }
            if (isFirst) {
                mf.isShowLegend(1);
                isFirst = false;
            }
        }
        private void OKBtn_Click(object sender, EventArgs e){
            if (!isFirst)
            {
                //if (MessageBox.Show("确认不使用Distance的值过滤吗?", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //{
                //    this.DialogResult = DialogResult.Cancel;
                //    this.isClose = false;
                //    this.Close();
                //}
                //else
                //{
                //    return;
                //}
                mf = (MainForm)this.Owner;
                mf.isShowLegend(0);
                if (!isFix)
                {//判断是否固定点
                    mf.ExcludePtsByDistance(this.checkBox2.Checked);
                }
                else
                {
                    mf.RejectPtsByDistanceFromFixed(this.checkBox2.Checked);
                    mf.centers = Tools.getFixedPtsCentroid(mf.grouping, this.checkBox2.Checked);
                    mf.showFixPointData(3);
                    mf.isShowLegend(5);
                }
                isClose = false;
                this.DialogResult = DialogResult.OK;
                this.Close();  

            }
            else {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (!isFix)//不是固定点
                mf.ShowPointsFromFile(mf.rawData, 6 + ((this.checkBox1.Checked) ? 0 : 1));
            else {
                mf.showFixPointData(2 + ((this.checkBox1.Checked) ? 0 : 2));
            }    
        }

        private void SureDistanceFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && isClose)
            {
                DialogResult r = MessageBox.Show("确定不使用距离阈值过滤吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
