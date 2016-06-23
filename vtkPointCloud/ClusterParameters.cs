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
        public bool isFirst = true;
        MainForm mf;
        public ClusterParameters()
        {
            InitializeComponent();
            textThrehold.Focus();
            
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
            mf = (MainForm)this.Owner;
            this.Visible = false;
            mf.getClusterFromList(threhold, point);
            mf.centers = Tools.getClusterCenter(mf.dbb.clusterAmount, mf.rawData);//计算质心
            //mf.ShowPointsFromFile(mf.centers, 3);//不同颜色显示核心点与野点  这一步对聚类进行分组
            mf.circles = Tools.getCircles(mf.hulls, mf.clusterSum);//计算外接圆
            mf.showCircle(mf.circles,1);
            mf.isShowLegend(2);
            if (isFirst) {
                isFirst = false;
                this.ComfirmResult.Enabled = true;
            }
            //this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            foreach(Point3D p in mf.rawData){
                p.clusterId = 0;
            }
            if (!isFirst) {
                if (MessageBox.Show("确认取消聚类结果吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    return;
            }
            mf.isShowLegend(0);
            mf.ShowPointsFromFile(mf.rawData, 1);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void ComfirmResult_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认聚类结果吗,确认将删除野点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            mf = (MainForm)this.Owner;
            Tools.removeErrorPointFromClustering(mf.rawData);
            mf.isShowLegend(0);
            mf.isShowLegend(4);
            mf.showCircle(mf.circles, 2);
            mf.dealwithMCCandMCE();
            this.Visible = false;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
