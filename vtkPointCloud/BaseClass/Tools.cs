using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace vtkPointCloud
{
    class Tools
    {
        //计算点阵边界
        static public double[] calBounds(List<Point3D> lst)
        {
            double x_min = lst[0].X;
            double y_min = lst[0].Y;
            double x_max = lst[0].X;
            double y_max = lst[0].Y;
            foreach (Point3D p in lst)
            {
                if (p.X > x_max) x_max = p.X;
                if (p.X < x_min) x_min = p.X;
                if (p.Y > y_max) y_max = p.Y;
                if (p.Y < y_min) y_min = p.Y;
            }
            return new double[4] { x_min, x_max, y_min, y_max };
        }
        /// <summary>
        /// 截屏事件
        /// </summary>
        static public void Screen(MainForm fm)
        {
            //Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;
            Rectangle rect = fm.ClientRectangle;
            int width = rect.Width;

            int height = rect.Height;
            int border = (fm.Width - fm.ClientSize.Width) / 2;//边框宽度
            int caption = (fm.Height - fm.ClientSize.Height) - border;//标题栏高度
            //        MessageBox.Show(width + "  " + height);
            Bitmap bit = new Bitmap(width + fm.treeView1.Width, height + caption + border + fm.toolStrip1.Height);
            Graphics g = Graphics.FromImage(bit);

            //g.CopyFromScreen(this.vtkControl.Location, new Point(0, 0), bit.Size);
            g.CopyFromScreen(fm.treeView1.Width + border * 6, fm.menuStrip1.Height + fm.toolStrip1.Height + caption + border, 0, 0, bit.Size);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bmp|*.bmp|jpg|*.jpg|gif|*.gif";
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                bit.Save(saveFileDialog.FileName);
            }
            g.Dispose();
        }
        /// <summary>
        /// 将野点移除点集-确认聚类效果时做
        /// </summary>
        static public void removeErrorPointFromClustering(List<Point3D> rawData)
        {
            int sum = rawData.Count;
            if (rawData == null) return;
            rawData.RemoveAll((delegate(Point3D p) { return p.clusterId == 0; }));
            Console.WriteLine("剔除野点个数为 : "+ (rawData.Count-sum));
        }
        /// <summary>
        /// 将过滤点从点集中删除
        /// </summary>
        /// <param name="dataSet">需要过滤的点集</param>
        /// <param name="filterID">需要过滤的编号集合</param>
        static public void removeFilterPointFromClustering(List<Point3D> dataSet, List<int> filterID)
        {
            if (filterID.Count == 0) return;
            dataSet.RemoveAll((delegate(Point3D p) { return (filterID.Contains(p.clusterId)); }));
        }

        /// <summary>
        /// 获取扫描点聚类质心
        /// </summary>
        static public List<Point3D> getFixedPtsCentroid(List<Point3D>[] fixedData,bool isIgnoreDuplication)
        {
            List<Point3D>  scanCen = new List<Point3D>();

            for (int i = 0; i < fixedData.Length; i++)
            {
                Point3D tmp = new Point3D();
                int insideNum = 0;
                for (int j = 0; j < fixedData[i].Count; j++)
                {
                    if (fixedData[i][j].clusterId != 0 && isIgnoreDuplication)
                    {
                        tmp.X += fixedData[i][j].X;
                        tmp.Y += fixedData[i][j].Y;
                        tmp.Z += fixedData[i][j].Z;
                        insideNum++;
                    }
                    else
                    {
                        tmp.X += fixedData[i][j].X * fixedData[i][j].ptsCount;
                        tmp.Y += fixedData[i][j].Y * fixedData[i][j].ptsCount;
                        tmp.Z += fixedData[i][j].Z * fixedData[i][j].ptsCount;
                        insideNum += fixedData[i][j].ptsCount;
                    }
                }
                tmp.X = tmp.X / insideNum;
                tmp.Y = tmp.Y / insideNum;
                tmp.Z = tmp.Z / insideNum;
                tmp.pointName = fixedData[i][0].pointName;
                tmp.ifShown = true;
                scanCen.Add(tmp);
            }
            return scanCen;
        }
        /// <summary>
        /// 获取聚类中心集合
        /// </summary>
        /// <param name="clus">聚类数</param>
        /// <param name="rawData">所有点</param>
        /// <returns></returns>
        public static void getClusterCenter(int clus, List<Point3D> rawData, List<Point3D> centers, List<Point3D>[] grouping)
        {   
            //初始化
            double[] ccenters = new double[clus * 3];//分别记录x y z
            int[] counts = new int[clus];//记录数量

            for (int i = 0; i < ccenters.Length; i++)
            {
                ccenters[i] = 0.0;
            }
            for (int k = 0; k < counts.Length; k++)
            {
                counts[k] = 0;
            }

                foreach (Point3D p in rawData)
                {
                    if (p.clusterId != 0)
                    {
                        ccenters[(p.clusterId - 1) * 3] += p.X;
                        ccenters[(p.clusterId - 1) * 3 + 1] += p.Y;
                        ccenters[(p.clusterId - 1) * 3 + 2] += p.Z;
                        counts[p.clusterId - 1] += 1;
                        grouping[p.clusterId - 1].Add(p);
                    }
                }
            for (int i = 0; i < clus; i++)
            {
                centers.Add(new Point3D(ccenters[(i) * 3] / counts[i], ccenters[(i) * 3 + 1] / counts[i], ccenters[(i) * 3 + 2] / counts[i], i + 1, true));
            }
        }
        /// <summary>
        /// 确认Distance过滤结果 输出聚类 并把野点设为不可见
        /// <param name="isExport">是否输出过滤后文件</param>
        /// </summary>
        static public void cleanDataByDistance(bool isExport,List<Point3D> rawData,int bit) {
            int tmp = rawData.Count;
            rawData.RemoveAll(delegate(Point3D p) { return (p.isFilterByDistance); });
            MessageBox.Show("通过distance过滤 "+(tmp - rawData.Count)+" 个点");
            if (isExport)//如果需要输出文件
            {
                SaveFileDialog saveFile1 = new SaveFileDialog();
                saveFile1.Filter = "文本文件(.txt)|*.txt";
                saveFile1.Title = "输出过滤文件";
                saveFile1.FilterIndex = 1;
                bool skipRecru = true;
                while (skipRecru)
                {
                    DialogResult diaRs = saveFile1.ShowDialog();
                    if (diaRs == DialogResult.Cancel)
                    {
                        DialogResult dr = MessageBox.Show("确认不保存距离过滤后点集吗？", "提示", MessageBoxButtons.OKCancel);
                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            skipRecru = false;
                        }
                        else if (dr == System.Windows.Forms.DialogResult.Cancel)
                        {
                            continue;
                        }
                    }
                    if (diaRs == DialogResult.OK)
                    {
                        //Console.WriteLine(saveFile1.FileName);
                        System.IO.StreamWriter ssw = new System.IO.StreamWriter(saveFile1.FileName, false);
                        try
                        {
                            foreach (Point3D p3d in rawData)
                                ssw.WriteLine(p3d.motor_x.ToString("F" + bit) + "\t" + p3d.motor_y.ToString("F" + bit) + "\t" + p3d.Distance.ToString("F" + bit));
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            ssw.Close();
                            skipRecru = false;
                        }
                    }
                    
                }
            }
        }
        /// <summary>
        ///  导出聚类质心文件
        /// </summary>
        /// <param name="circles">质心</param>
        /// <param name="bit">小数点位</param>
        /// <param name="x_angle">电机x</param>
        /// <param name="y_angle">电机y</param>
        /// <param name="ss">输出路径</param>
        static public void exportClustersCenterFile(List<Point3D> centers,int bit,double x_angle,double y_angle,string ss)
        {           //导出聚类数据     
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ss, false);
                    double x,y,z,motor_x,motor_y,d,xita,phi;
                    try
                    {
                        int ccc = 1;
                        //输出均值 电机x 电机y 距离distance
                        for (int j = 0; j < centers.Count; j++)
                        {
                            x= centers[j].X;
                            y= centers[j].Y;
                            z= centers[j].Z;
                            phi = Math.Asin(y / z);
                            xita = Math.Atan(x/(z*Math.Cos(phi)));
                            motor_x = ( xita * (-90.0) / Math.PI)+x_angle;
                            motor_y = ( phi * 90 /Math.PI) +y_angle;
                            d = z / Math.Cos(xita);
                            sw.WriteLine(motor_x.ToString("F" + bit) + "\t"+ motor_y.ToString("F" + bit) + "\t"+ d.ToString("F" + bit));
                            ccc++;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输出文件失败!", "提示");
                        throw;
                    }
                    finally
                    {
                        sw.Close();
                    }       
        }
        static public List<Point3D>[] getGroupsFromClusterID(List<Point3D> rawData,int clus) {
            if (clus == 0) return null;
            List<Point3D>[] hulls = new List<Point3D>[clus];//hulls = new List<Point2D>[cs];
            foreach (Point3D p in rawData)
            {
                hulls[p.clusterId-1].Add(p);
            }
            return hulls;
        }
        static public void exportClustersPointsFile(List<Point3D>[] grouping, int bit, double x_angle, double y_angle, string ss)
        {   //导出聚类数据
            System.IO.StreamWriter sw = new System.IO.StreamWriter(ss, false);
            try
            {
                //输出均值 电机x 电机y 距离distance
                for (int j = 0; j < grouping.Length; j++)
                {
                    foreach (Point3D p3d in grouping[j])
                    {
                        sw.WriteLine(p3d.clusterId + "\t" + p3d.motor_x.ToString("F" + bit) + "\t" + p3d.motor_y.ToString("F" + bit) + "\t" + p3d.Distance.ToString("F" + bit));
                    }
                }
            }
            catch
            {
                MessageBox.Show("输出文件失败!", "提示");
                throw;
            }
            finally
            {
                sw.Close();
            }
        }
        /// <summary>
        /// 获取所有聚类的外接圆
        /// </summary>
        /// <param name="hulls">每个数组是同一个聚类点的List</param>
        /// <param name="clusterSum">聚类数</param>
        static public List<Point2D> getCircles(List<Point2D>[] hulls, int clusterSum)
        {
            List<Point2D> circles = new List<Point2D>();
            for (int j = 0; j < clusterSum; j++)
            {
                if (hulls[j].Count == 0) continue;
                else Console.WriteLine(hulls[j].Count);
                List<Point2D> m_points = Geometry.MakeConvexHull(hulls[j]);
                Polygon pgon = new Polygon(m_points.ToArray());
                Point2D CircleCenter;//圆心点
                double CircleRadius = -1;
                Geometry.FindMinimalBoundingCircle(m_points, out CircleCenter, out CircleRadius);
                CircleCenter.radius = CircleRadius;
                CircleCenter.clusID = j+1;
                circles.Add(CircleCenter);
            }
            return circles;
        }
        /// <summary>
        /// 阈值过滤窗体同步显示-扫描点
        /// </summary>
        /// <param name="disMax">距离过滤最大值</param>
        /// <param name="disMin">距离过滤最小值</param>
        /// <returns>返回值注释</returns>
        static public  void FilterByDistance_ScanPoint(List<Point3D> rawData,double disMax,double disMin)//修正过滤阈值后界面同步
        {
            if (rawData == null||rawData.Count ==0) {
                MessageBox.Show("没有数据，不能过滤！", "提示");
                return;
            }
            for(int i=0;i<rawData.Count;i++){
                if (rawData[i].Distance < disMax && rawData[i].Distance>disMin)
                {
                    rawData[i].isFilterByDistance = false;
                }
                else {
                    rawData[i].isFilterByDistance = true;
                }
            }
        }
        /// <summary>
        /// 阈值过滤窗体同步显示-扫描点
        /// </summary>
        /// <param name="disMax">距离过滤最大值</param>
        /// <param name="disMin">距离过滤最小值</param>
        /// <returns>返回值注释</returns>
        static public void FilterByDistance_FixedPoint(List<Point3D>[] grouping, double disMax, double disMin)//修正过滤阈值后界面同步
        {
            if (grouping == null || grouping.Length == 0) {
                MessageBox.Show("没有数据，不能过滤！", "提示");
                return;
            }
            for (int i = 0; i < grouping.Length; i++)
            {
                foreach (Point3D p in grouping[i])
                {
                    if (p.Distance>disMax ||p.Distance<disMin)
                    {
                        p.isFilterByDistance = true;
                    }
                    else
                    {
                        p.isFilterByDistance = false;
                    }

                }
                    
            }
        }




    }
}
