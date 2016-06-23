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
        /// 将野点移除点集
        /// </summary>
        static public void removeErrorPointFromClustering(List<Point3D> rawData)
        {
            if (rawData == null) return;
            rawData.RemoveAll((delegate(Point3D p) { return p.clusterId == 0; }));
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
        static public List<Point3D> getScanCentroid(List<Point3D>[] fixedData)
        {
            List<Point3D>  scanCen = new List<Point3D>();

            for (int i = 0; i < fixedData.Length; i++)
            {
                Point3D tmp = new Point3D();
                int insideNum = 0;
                for (int j = 0; j < fixedData[i].Count; j++)
                {
                    if (fixedData[i][j].clusterId != 0)
                    {
                        tmp.X += fixedData[i][j].X;
                        tmp.Y += fixedData[i][j].Y;
                        tmp.Z += fixedData[i][j].Z;
                        insideNum++;
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
        static public List<Point3D> getClusterCenter(int clus,List<Point3D> rawData)
        {
            List<Point3D> centers = new List<Point3D>();
            double[] ccenters = new double[clus * 3];
            int[] counts = new int[clus];
            for (int i = 0; i < ccenters.Length; i++)
            {
                ccenters[i] = 0.0;
            }
            for (int k = 0; k < counts.Length; k++)
            {
                counts[k] = 0;
            }
            for (int i = 0; i < rawData.Count; i++)
            {
                if (rawData[i].clusterId != 0)
                {
                    ccenters[(rawData[i].clusterId - 1) * 3] += rawData[i].X;
                    ccenters[(rawData[i].clusterId - 1) * 3 + 1] += rawData[i].Y;
                    ccenters[(rawData[i].clusterId - 1) * 3 + 2] += rawData[i].Z;
                    counts[rawData[i].clusterId - 1] += 1;
                }
            }
            for (int i = 0; i < clus; i++)
            {
                centers.Add(new Point3D(ccenters[(i) * 3] / counts[i], ccenters[(i) * 3 + 1] / counts[i], ccenters[(i) * 3 + 2] / counts[i], i + 1, true));
            }
            return centers;
        }
        /// <summary>
        /// 确认Distance过滤结果 输出聚类 并把野点设为不可见
        /// <param name="isExport">是否输出过滤后文件</param>
        /// </summary>
        static public void cleanDataByDistance(bool isExport,List<Point3D> rawData,int bit) {
            Console.WriteLine("当前点数 : " + rawData.Count);
            rawData.RemoveAll(delegate(Point3D p) { return (p.isFilterByDistance); });
            Console.WriteLine("阈值过滤后点数 : " + rawData.Count);
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
        /// 导出扫描点聚类文件 x电机 y电机 distance剧烈
        /// </summary>
        static public void exportClusterFile(List<Point3D> rawData)
        {   //导出聚类数据
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.Title = "输出聚类文件";
            saveFile1.FilterIndex = 1;
            bool skipRecru = true;
            while (skipRecru) {
                DialogResult rssss = saveFile1.ShowDialog();
                if (rssss == System.Windows.Forms.DialogResult.Cancel)//选择取消
                {
                    DialogResult dr = MessageBox.Show("确认不保存聚类结果吗？", "提示", MessageBoxButtons.OKCancel);
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        skipRecru = false;
                    }
                    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                    {
                        continue;
                    }
                }
                if (rssss == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)//选择确认
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile1.FileName, false);
                    //System.IO.StreamWriter ssw = new System.IO.StreamWriter("E:\\789.txt", false);
                    try
                    {
                        int ccc = 1;
                        //输出均值 电机x 电机y 距离distance
                        for (int j = 0; j < rawData.Count; j++)
                        {
                            if (rawData[j].clusterId != 0 && (!rawData[j].isFilter))
                            {
                                sw.WriteLine(ccc + "\t" + rawData[j].motor_x + "\t" + rawData[j].motor_y + "\t" + rawData[j].Distance);
                            }
                            ccc++;
                        }
                        //输出均值XYZ
                        //for (int j = 0; j < centers.Count; j++) {
                        //    ssw.WriteLine(centers[j].X + "\t" + centers[j].Y + "\t" + centers[j].Z);
                        //}
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        sw.Close();
                        //ssw.Close();
                        skipRecru = false;
                    }
                }
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


    }
}
