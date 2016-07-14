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
    public partial class Clustering : Form
    {
        public double threhold;
        public int point;
        public int ptsIncell;
        public double mergeThrehold;
        public bool isFirst = true;
        public bool isMerge = false;
        public List<ClusObj> tmpClusList;

        public List<Point3D> tmpCenters;
        public List<Point3D> tmpCenters2D;
        public List<Point2D> tmpCircles;
        public List<Point2D> tmpCircles2D;
        MainForm mf;
        public Clustering()
        {
            InitializeComponent();
        }
        private void ComfirmResult_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认聚类结果吗,确认将删除野点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            mf = (MainForm)this.Owner;

            mf.rawData = new List<Point3D>();
            mf.clusForMerge.ForEach(i => mf.rawData.Add((Point3D)i.Clone()));
            MainForm.clusterSum = tmpClusList.Count;
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
            //mf.getClusterFromList(threhold, point,ptsIncell); 
            mf.getClusterFromMotor(threhold, point, ptsIncell);
            this.rb_3d.CheckedChanged -= new System.EventHandler(this.rb_3d_CheckedChanged);
            this.cb_showcore.CheckedChanged -= new System.EventHandler(this.cb_showcore_CheckedChanged);
            this.cb_showerror.CheckedChanged -= new System.EventHandler(this.cb_showerror_CheckedChanged);
            this.cb_showcentroid.CheckedChanged -= new System.EventHandler(this.cb_showcentroid_CheckedChanged);

            this.cb_showcentroid.Checked = true;
            this.cb_showcore.Checked = true;
            this.cb_showerror.Checked = true;
            this.rb_3d.Checked = true;
            this.rb_2d.Checked = false;

            this.rb_3d.CheckedChanged += new System.EventHandler(this.rb_3d_CheckedChanged);
            this.cb_showcore.CheckedChanged += new System.EventHandler(this.cb_showcore_CheckedChanged);
            this.cb_showerror.CheckedChanged += new System.EventHandler(this.cb_showerror_CheckedChanged);
            this.cb_showcentroid.CheckedChanged += new System.EventHandler(this.cb_showcentroid_CheckedChanged);

            if (isFirst)
            {
                isFirst = false;
                this.cb_showcentroid.Enabled = true;
                this.cb_showcore.Enabled = true;
                this.cb_showerror.Enabled = true;
                this.rb_2d.Enabled = true;
                this.rb_3d.Enabled = true;
            }
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

            foreach (Point3D p in mf.centers)
            {
                p.isTraversal = false;
            }
            tmpClusList = new List<ClusObj>();//重新建立分组List
            //a.ForEach(i => b.Add((Point3D)i.Clone()));
            mf.clusList.ForEach(i => tmpClusList.Add((ClusObj)i.Clone()));//把聚类后的初始分组copy过来
            Dictionary<int,int> dick = Tools.IntegratingClusID(mf.centers, mergeThrehold);//根据聚类后初始质心和阈值threhold计算需要融合的ID
            tmpCenters = new List<Point3D>();//缓存质心初始化
            tmpCenters2D = new List<Point3D>();
            Tools.refreshCensAndClusByDictionary(dick, tmpClusList ,ref tmpCenters,ref tmpCenters2D);//重新分配ID号 计算分配后ID数 计算两组质心
            //重新洗牌！！
            tmpCircles = new List<Point2D>();
            tmpCircles2D = new List<Point2D>();
            tmpCircles = Tools.getCircles(tmpClusList,true);//再次计算外接圆
            tmpCircles2D = Tools.getCircles(tmpClusList,false);//再次计算
            //取消事件注册
            this.cb_showcore.CheckedChanged -= new System.EventHandler(this.cb_showcore_CheckedChanged);
            this.cb_showerror.CheckedChanged -= new System.EventHandler(this.cb_showerror_CheckedChanged);
            this.cb_showcentroid.CheckedChanged -= new System.EventHandler(this.cb_showcentroid_CheckedChanged);

            this.cb_showcentroid.Checked = true;
            this.cb_showcore.Checked = true;
            this.cb_showerror.Checked = true;
            //添加事件注册
            this.cb_showcore.CheckedChanged += new System.EventHandler(this.cb_showcore_CheckedChanged);
            this.cb_showerror.CheckedChanged += new System.EventHandler(this.cb_showerror_CheckedChanged);
            this.cb_showcentroid.CheckedChanged += new System.EventHandler(this.cb_showcentroid_CheckedChanged);
            //mf.showCircle(this.rb_3d.Checked?(tmpCircles):(tmpCircles2D), 1, mf.clusForMerge, this.rb_3d.Checked?(tmpCenters):(tmpCenters2D));
            if (rb_3d.Checked)
            {
                mf.showCircle(tmpCircles, 1, mf.clusForMerge, tmpCenters);
            }
            else
            {
                mf.showCircles2D(tmpCircles2D, 1, mf.clusForMerge, tmpCenters2D);
            }
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
            
            mf.centers = new List<Point3D>();
            this.tmpCenters.ForEach(i => mf.centers.Add((Point3D)i.Clone()));//复制当前质心
            mf.clusList = new List<ClusObj>();
            this.tmpClusList.ForEach(i => mf.clusList.Add((ClusObj)i.Clone()));//复制当前分组
            mf.circles = new List<Point2D>();
            this.tmpCircles.ForEach(i => mf.circles.Add((Point2D)i.Clone()));//赋值当前外接圆

        }

        private void cb_showcore_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (this.cb_showcore.Checked)
                mf.addActor(mf.actorC);
            else
                mf.deleteActor(mf.actorC);
        }

        private void cb_showerror_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (this.cb_showerror.Checked)
                mf.addActor(mf.actorB);
            else
                mf.deleteActor(mf.actorB);
        }

        private void cb_showcentroid_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (this.cb_showcentroid.Checked)
                mf.addActor(mf.actorA);
            else
                mf.deleteActor(mf.actorA);
        }

        private void rb_3d_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (!isMerge) {
                if (rb_3d.Checked)
                {
                    mf.showCircle(mf.circles, 1, mf.clusForMerge, mf.centers);
                }
                else
                {
                    mf.showCircles2D(mf.circles2D, 1, mf.clusForMerge, mf.centers2D);
                }
            }
            else
            {
                if (rb_3d.Checked)
                {
                    mf.showCircle(tmpCircles, 1, mf.clusForMerge, tmpCenters);
                }
                else
                {
                    mf.showCircles2D(tmpCircles2D, 1, mf.clusForMerge, tmpCenters2D);
                }

            }
            if (!this.cb_showcentroid.Checked) mf.deleteActor(mf.actorA);
            if (!this.cb_showerror.Checked) mf.deleteActor(mf.actorB);
            if (!this.cb_showcore.Checked) mf.deleteActor(mf.actorC);
        }
    }
}