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
        public double mergeThrehold;
        public bool isFirst = true;
        public bool isMerge = false;
        public List<ClusObj> tmpClusList;
        public List<Point3D> tmpCenters;
        public List<Point2D> tmpCircles;
        MainForm mf;
        public ClusterParameters()
        {
            InitializeComponent();
        }
        private void ComfirmResult_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认聚类结果吗,确认将删除野点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            mf = (MainForm)this.Owner;
            Tools.removeErrorPointFromClustering(mf.rawData);
            mf.isShowLegend(0);
            mf.isShowLegend(4);
            mf.showCircle(mf.circles, 2,mf.rawData,mf.centers);
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
            mf.showCircle(mf.circles, 2, mf.rawData,mf.centers);
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
            if (isFirst)
            {
                isFirst = false;
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
            mf.getClusterFromList(threhold, point, ptsIncell);

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
            mf.circles = Tools.getCircles(mf.clusList);//计算外接圆
            mf.showCircle(mf.circles, 1, mf.rawData,mf.centers);
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
            mf.circles = Tools.getCircles(mf.clusList);//计算外接圆
            mf.showCircle(mf.circles, 1, mf.rawData,mf.centers);
        }
        private void MergeBtn_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(this.MergeThrehold.Text, out this.mergeThrehold))
            {
                MessageBox.Show("输入的不是浮点数，请重新输入");
                return;
            }
            mf = (MainForm)this.Owner;
            if (!isMerge) {
                isMerge = true;
            }

            //tmpCenters = new List<Point3D>();

            //tmpCenters.AddRange(mf.centers);
            foreach (Point3D p in mf.centers)
            {
                p.isTraversal = false;
            }
            tmpClusList = null;
            tmpClusList = new List<ClusObj>();
            tmpClusList.AddRange(mf.clusList);
            int fff = 0;
            foreach (ClusObj oo in mf.clusList)//clusList被改写了 fuck
            {
                fff += oo.li.Count;
            }
            Console.WriteLine("fff的值为" + fff);
            Dictionary<int,int> dick = Tools.IntegratingClusID(mf.centers, mergeThrehold);//计算需要融合的ID
            tmpCenters = null;
            tmpCenters = Tools.refreshCensAndClusByDictionary(dick, tmpClusList);//重新分配ID号 计算分配后ID数
            fff = 0;
            foreach (ClusObj oo in tmpClusList)
            {
                fff += oo.li.Count;
            }
            Console.WriteLine("fff的新值为" + fff);
            //mf.centers = null;
            //mf.centers = tmpCenters;
            //重新洗牌！！
            tmpCircles = null;
            tmpCircles = new List<Point2D>();
            tmpCircles = Tools.getCircles(tmpClusList);//再次计算外接圆
            mf.showCircle(tmpCircles, 1, mf.clusForMerge, tmpCenters);
            //mf.showCircle(circles, 1, );//显示圆
        }

        private void SureMergeBtn_Click(object sender, EventArgs e)
        {
            if (isMerge) this.ComfirmResult.Enabled = true;
            else {
                DialogResult r = MessageBox.Show("确定不使用进行分块融合吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r != DialogResult.OK)
                {
                    this.ComfirmResult.Enabled = true;
                }
                else
                {
                    return;
                }
            }
            mf = (MainForm)this.Owner;
            mf.centers = null;
            mf.centers = this.tmpCenters;
            mf.clusList = null;
            mf.clusList = this.tmpClusList;
        }
    }
}
