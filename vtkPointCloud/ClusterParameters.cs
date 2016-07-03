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
        public int ptsIncell;
        public bool isFirst = true;
        MainForm mf;
        public ClusterParameters()
        {
            InitializeComponent();
            textThrehold.Focus();
        }
        private void ComfirmResult_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认聚类结果吗,确认将删除野点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            mf = (MainForm)this.Owner;
            Tools.removeErrorPointFromClustering(mf.rawData);
            mf.isShowLegend(0);
            mf.isShowLegend(4);
            mf.showCircle(mf.circles, 2,mf.rawData);
            mf.dealwithMCCandMCE();
            this.Visible = false;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
        private void CancelBtn_Click(object sender, EventArgs e) {
            if (MessageBox.Show("确认聚类结果吗,确认将删除野点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            mf = (MainForm)this.Owner;
            Tools.removeErrorPointFromClustering(mf.rawData);
            mf.isShowLegend(0);
            mf.isShowLegend(4);
            mf.showCircle(mf.circles, 2, mf.rawData);
            mf.dealwithMCCandMCE();
            this.Visible = false;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
        private void DoClusteringBtn_Click(object sender, EventArgs e) {

            if (!int.TryParse(this.textPoint.Text, out this.point))
            {
                MessageBox.Show("输入的不是整数，请重新输入");
                return;
            }
            if (!double.TryParse(this.textThrehold.Text, out this.threhold))
            {
                MessageBox.Show("输入的不是浮点数，请重新输入");
                return;
            }
            if (!int.TryParse(this.PtsInCellTxtBox.Text, out this.ptsIncell))
            {
                MessageBox.Show("输入的不是整数，请重新输入");
                return;
            }
            mf = (MainForm)this.Owner;
            this.Visible = false;
            mf.getClusterFromList(threhold, point,ptsIncell);
            mf.isShowLegend(2);
            if (isFirst)
            {
                isFirst = false;
                this.ComfirmResult.Enabled = true;
            }


        }
        private void NoCellClustering(){
            if (!int.TryParse(this.textPoint.Text, out this.point))
            {
                MessageBox.Show("输入的不是整数，请重新输入");
                return;
            }
            if (!double.TryParse(this.textThrehold.Text, out this.threhold))
            {
                MessageBox.Show("输入的不是浮点数，请重新输入");
                return;
            }
            mf = (MainForm)this.Owner;
            this.Visible = false;
            mf.getClusterFromList(threhold, point);

            mf.centers = new List<Point3D>();
            //mf.grouping = new List<Point3D>[mf.dbb.clusterAmount];//将聚类写进分组的数组
            mf.clusList = new List<ClusObj>();
            for (int j = 0; j < mf.dbb.clusterAmount; j++)
            {
                mf.clusList.Add(new ClusObj());//初始化每个List
            }
            Tools.getClusterCenter(mf.dbb.clusterAmount, mf.rawData, mf.centers, mf.clusList, null);//计算质心 计算分组
            mf.ShowPointsFromFile(mf.centers, 3);//不同颜色显示核心点与野点  这一步对聚类进行分组 计算外接多边形
            //mf.circles = Tools.getCircles(mf.hulls, mf.clusterSum);//计算外接圆
            mf.circles = Tools.getCircles(mf.clusList, mf.clusterSum);//计算外接圆
            mf.showCircle(mf.circles, 1, mf.rawData);
            mf.isShowLegend(2);
            if (isFirst)
            {
                isFirst = false;
                this.ComfirmResult.Enabled = true;
            }
            mf.centers = new List<Point3D>();
            //mf.grouping = new List<Point3D>[mf.dbb.clusterAmount];//将聚类写进分组的数组
            mf.clusList = new List<ClusObj>();
            for (int j = 0; j < mf.dbb.clusterAmount; j++)
            {
                mf.clusList.Add(new ClusObj());//初始化每个List
            }
            Tools.getClusterCenter(mf.dbb.clusterAmount, mf.rawData, mf.centers, mf.clusList, null);//计算质心 计算分组
            mf.ShowPointsFromFile(mf.centers, 3);//不同颜色显示核心点与野点  这一步对聚类进行分组 计算外接多边形
            mf.circles = Tools.getCircles(mf.clusList, mf.clusterSum);//计算外接圆
            mf.showCircle(mf.circles, 1, mf.rawData);
        }
        private void MergeBtn_Click(object sender, EventArgs e)
        {

        }

        private void SureMergeBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
