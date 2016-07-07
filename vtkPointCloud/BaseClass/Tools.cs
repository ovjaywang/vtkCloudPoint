using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using vtk;
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
        /// 获取固定点聚类质心
        /// </summary>
        static public List<Point3D> getFixedPtsCentroid(List<ClusObj> clusList,bool isIgnoreDuplication)
        {
            List<Point3D>  scanCen = new List<Point3D>();

            for (int i = 0; i < clusList.Count; i++)
            {
                Point3D tmp = new Point3D();
                int insideNum = 0;
                for (int j = 0; j < clusList[i].li.Count; j++)
                {
                    if (clusList[i].li[j].clusterId != 0 && isIgnoreDuplication)
                    {
                        tmp.X += clusList[i].li[j].X;
                        tmp.Y += clusList[i].li[j].Y;
                        tmp.Z += clusList[i].li[j].Z;
                        insideNum++;
                    }
                    else
                    {
                        tmp.X += clusList[i].li[j].X * clusList[i].li[j].ptsCount;
                        tmp.Y += clusList[i].li[j].Y * clusList[i].li[j].ptsCount;
                        tmp.Z += clusList[i].li[j].Z * clusList[i].li[j].ptsCount;
                        insideNum += clusList[i].li[j].ptsCount;
                    }
                }
                tmp.X = tmp.X / insideNum;
                tmp.Y = tmp.Y / insideNum;
                tmp.Z = tmp.Z / insideNum;
                tmp.pointName = clusList[i].li[0].pointName;
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
        public static void getClusterCenter(int clus, List<Point3D> rawData, List<Point3D> centers, List<ClusObj> clusList,List<int> idList)
        {
            Dictionary<int, int> idTmp = new Dictionary<int, int>();
            int i = 1;
            if (idList != null) {
                foreach (int j in idList)
                {
                    idTmp.Add(j, i);
                    i++;
                }
            }
            else
            {
                for (int t = 0; t < clus; t++)
                {
                    idTmp.Add(t+1, t+1);
                }
            }
            Console.WriteLine("ID数目为 ： "+idTmp.Count);
            int tm;
            foreach (Point3D p in rawData)
            {
                if (p.clusterId != 0)
                {
                    tm=idTmp[p.clusterId] - 1;
                    p.clusterId = idTmp[p.clusterId];
                    clusList[tm].li.Add(p);//加入分组聚类
                }
            }
            int dd = 1;
            foreach (ClusObj obj in clusList)
            {
                obj.clusId = dd++;
                if (obj.li.Count == 0) continue;
                obj.clusId = obj.li[0].clusterId;
                centers.Add(new Point3D(obj.li.Average(m => m.X), obj.li.Average(m => m.Y), obj.li.Average(m => m.Z), obj.clusId, true));//计算质心
            }
        }
        public static void GetClusList(List<Point3D> rawData, List<Point3D> centers, List<ClusObj> clusList, List<int> idList)
        {
            Dictionary<int, int> idTmp = new Dictionary<int, int>();
            int i = 1;
            if (idList != null)
            {
                foreach (int j in idList)
                {
                    idTmp.Add(j, i);
                    i++;
                }
            }
            else
            {
                for (int t = 0; t < clusList.Count; t++)
                {
                    idTmp.Add(t + 1, t + 1);
                }
            }
            foreach (Point3D p in rawData)
            {
                if (p.clusterId != 0) {
                    clusList[p.clusterId - 1].li.Add(p);
                }
            }
            foreach (ClusObj obj in clusList)
            {
                //obj.clusId = dd++;
                if (obj.li.Count == 0) continue;
                //obj.clusId = obj.li[0].clusterId;
                centers.Add(new Point3D(obj.li.Average(m => m.X), obj.li.Average(m => m.Y), obj.li.Average(m => m.Z), obj.clusId, true));//计算质心
            }
        }
        /// <summary>
        /// 确认Distance过滤结果 输出聚类 并把野点设为不可见--扫描点
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
                saveFile1.Title = "输出扫描点过滤文件";
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
        /// 确认Distance过滤结果 输出聚类 并把野点设为不可见--固定点
        /// <param name="isExport">是否输出过滤后文件</param>
        /// </summary>
        static public void cleanDataByDistance2(bool isExport, List<ClusObj> clusList, int bit)
        {
            int tmp = 0;
            //tmp=grouping.Sum(m => m.Count);//计算数据数量
            tmp = clusList.Sum(m => m.li.Count);
            for (int i = 0; i < clusList.Count; i++)
            {
                clusList[i].li.RemoveAll(delegate(Point3D p) { return (p.isFilterByDistance); });
            }
            MessageBox.Show("通过distance过滤 " + (clusList.Sum(m => m.li.Count) - tmp) + " 个点");
            if (isExport)//如果需要输出文件
            {
                SaveFileDialog saveFile1 = new SaveFileDialog();
                saveFile1.Filter = "文本文件(.txt)|*.txt";
                saveFile1.Title = "输出固定点过滤文件";
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
                            for (int i = 0; i < clusList.Count; i++)
                            {
                                foreach (Point3D p3d in clusList[i].li)
                                    ssw.WriteLine(p3d.motor_x.ToString("F" + bit) + "\t" + p3d.motor_y.ToString("F" + bit) + "\t" + p3d.Distance.ToString("F" + bit));
                            }
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
                            //sw.WriteLine(motor_x.ToString("F" + bit) + "\t"+ motor_y.ToString("F" + bit) + "\t"+ d.ToString("F" + bit));
                            sw.WriteLine(x.ToString("F" + bit) + "\t" + y.ToString("F" + bit) + "\t" + z.ToString("F" + bit));
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
        static public void exportClustersPointsFile(List<ClusObj> clusList, int bit, double x_angle, double y_angle, string ss)
        {   //导出聚类数据
            System.IO.StreamWriter sw = new System.IO.StreamWriter(ss, false);
            try
            {
                //输出均值 电机x 电机y 距离distance
                for (int j = 0; j < clusList.Count; j++)
                {
                    foreach (Point3D p3d in clusList[j].li)
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
        //static public List<Point2D> getCircles(List<Point2D>[] hulls, int clusterSum)
        static public List<Point2D> getCircles(List<ClusObj> clusList)
        {
            List<Point2D> circles = new List<Point2D>();
            for (int j = 0; j < clusList.Count; j++)
            {
                //else Console.WriteLine(clusList[j].li.Count);
                Point2D CircleCenter;//圆心点
                double CircleRadius = -1;
                Geometry.FindMinimalBoundingCircle(clusList[j].li, out CircleCenter, out CircleRadius);//依据外接定点计算外接圆
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
        static public void FilterByDistance_FixedPoint(List<ClusObj> clusList, double disMax, double disMin)//修正过滤阈值后界面同步
        {
            if (clusList == null || clusList.Count == 0)
            {
                MessageBox.Show("没有数据，不能过滤！", "提示");
                return;
            }
            for (int i = 0; i < clusList.Count; i++)
            {
                foreach (Point3D p in clusList[i].li)
                {
                    if (p.Distance<disMax && p.Distance>disMin)
                    {
                        p.isFilterByDistance = false;
                    }
                    else
                    {
                        p.isFilterByDistance = true;
                    }

                }
                    
            }
        }
        /// <summary>
        /// 获取分组后的最大最小值
        /// </summary>
        /// <param name="grouping">分组数组</param>
        /// <param name="typpe">1最大 2最小</param>
        /// <returns></returns>
        static public double GetGroupingManOrMin(List<ClusObj> clusList,int typpe){
            if (clusList == null || clusList.Count == 0)
            {
                return 0;
            }
            double result = clusList[0].li[0].Distance;
            double tmp;
            if (typpe==1)//最大
            {
                for (int i = 0; i < clusList.Count; i++)
                {
                    tmp = clusList[i].li.Max((m => m.Distance));
                    if(result < tmp){
                        result = tmp;
                    }
                }
            }
            else
            {
                for (int i = 0; i < clusList.Count; i++)
                {
                    tmp = clusList[i].li.Min((m => m.Distance));
                    if (result > tmp)
                    {
                        result = tmp;
                    }
                }
            }            
            return result;
        }
        /// <summary>
        /// 通过矩阵范围从点集筛选满足条件的点
        /// </summary>
        /// <param name="rawData">点集</param>
        /// <param name="min_x">x最小值</param>
        /// <param name="min_y">y最小值</param>
        /// <param name="max_x">x最大值</param>
        /// <param name="max_y">y最大值</param>
        /// <returns></returns>
        static public List<Point3D> getListByScale(List<Point3D> rawData, double min_x, double min_y, double max_x, double max_y){
            return rawData.FindAll(delegate(Point3D p) { return (p.X > min_x) && (p.Y > min_y) && (p.X <= max_x) && (p.Y <= max_y);});
        }

        /// <summary>
        /// 根据距离阈值更新聚类效果 融合接近聚类
        /// </summary>
        /// <param name="centers">质心点集</param>
        /// <param name="tmpList">重新分配好ID的数据点集</param>
        /// <param name="dic">ID映射列表</param>
        /// <param name="clusList"></param>
        /// <returns></returns>
        static public List<Point3D> refreshCensAndClusByDictionary(Dictionary<int, int> dic,List<ClusObj> clusList)
        {
            //dic存放 对应的clusID而非Index 按照value（ID）排序（讲道理，ID = index + 1）
            //grouping 序列对应ID
            List<int> keys =dic.Keys.ToList();
            //tmpList.Sort((x, y) =>//按照ID排序 否则
            //{
            //    int result;
            //    if (x.clusterId == y.clusterId) result = 0;
            //    else
            //    {
            //        if (x.clusterId > y.clusterId) result = 1;
            //        else result = -1;
            //    }
            //    return result;
            //});
            //Console.WriteLine("最大ID :" + tmpList[tmpList.Count - 1].clusterId);
            foreach (ClusObj ob in clusList)
            {
                if (keys.Contains(ob.clusId)) {
                    foreach (Point3D p in ob.li)
                    {
                        clusList[dic[ob.clusId] - 1].li.Add(p);
                    }
                }
            }
            clusList.RemoveAll((delegate(ClusObj p) { return (keys.Contains(p.clusId)); }));
            List<Point3D> centers = new List<Point3D>();
            foreach (ClusObj obj in clusList)
            {
                centers.Add(new Point3D(obj.li.Average(m => m.X), obj.li.Average(m => m.Y), obj.li.Average(m => m.Z), obj.clusId, true));//计算质心
            }
            Console.WriteLine("keys中包含"+keys.Count+"质心有"+centers.Count);
            return centers;
        }
        /// <summary>
        /// 将小于阈值范围的质心点集置入Dictionary 遍历之 融合之
        /// </summary>
        /// <param name="l">数据点集</param>
        /// <param name="cts">质心点集</param>
        /// <param name="thre">融合阈值</param>
        /// <returns></returns>
        static public Dictionary<int, int> IntegratingClusID( List<Point3D> centers, double thre)
        {//按照ID排序好的Centroid
            Dictionary<int, int> dic = new Dictionary<int, int>();
            int mergeCount = 0;
            for (int i = 0; i < centers.Count; i++)
            {
                if (!centers[i].isTraversal)//若尚未遍历
                {
                    centers[i].isTraversal = true;
                    //寻找与本质心小于阈值的点集
                    List<Point3D> plist = centers.FindAll(delegate(Point3D p)
                    {
                        return ((Math.Abs(p.X - centers[i].X) + (Math.Abs(p.Y - centers[i].Y))) < thre)
                            && (!p.isTraversal);
                    });
                    if (plist.Count != 0)
                    {
                        mergeCount += plist.Count;
                        //Console.Write("\t当前ID:" + centers[i].clusterId + "\t 阈值内ID:");
                        foreach (Point3D p in plist)
                        {
                            //Console.Write("ID:" + p.clusterId + "__Index：");
                            int index = centers.FindIndex(0, delegate(Point3D p22) { return (p22.clusterId == p.clusterId); });
                            //Console.Write(index + "\t");
                            dic.Add(p.clusterId, centers[i].clusterId);//明确加入的是聚类编号而不是Index
                            centers[index].isTraversal = true;
                        }
                        //Console.WriteLine();
                    }
                }
            }
            Console.WriteLine("被融合聚类 ： " + mergeCount);
            foreach (KeyValuePair<int, int> k in dic)
            {
                Console.WriteLine("取消聚类:{0}, 扩大聚类:{1}", k.Key, k.Value);
            }
            return dic;
        }
        static public vtkPolyData ArrayList2PolyData(int type,List<Point3D> centers,double[] trueScale, double[] centroidScale
            , double[] scale, int clock, int clock_y, int clock_x)//ArrayList转成可视化PolyData
        {
            vtkPolyData polydata = new vtkPolyData();
            vtkPoints SourcePoints = new vtkPoints();
            vtkCellArray SourceVertices = new vtkCellArray();
            int[] pid = new int[1];
            if (type == 1)
            {
                for (int i = 0; i < centers.Count; i++)
                {
                    //if (!centers[i].isFilter)
                    //{
                    pid[0] = SourcePoints.InsertNextPoint(centers[i].tmp_X, centers[i].tmp_Y, centers[i].tmp_Z);
                    SourceVertices.InsertNextCell(1, pid);
                    //}
                }
            }
            polydata.SetPoints(SourcePoints); //把点导入的polydata中去
            polydata.SetVerts(SourceVertices);
            return polydata;
        }
        static public double[] GetBoundsByPoint3D(List<Point3D> rawData) {
            double[] bounds = { 0, 0, 0, 0 };
            if (rawData == null || rawData.Count == 0) return bounds;
            bounds[0] = rawData.Min(m => m.X);
            bounds[1] = rawData.Max(m => m.X);
            bounds[2] = rawData.Min(m => m.Y);
            bounds[3] = rawData.Max(m => m.Y);
            return bounds;
        }



    }
}
