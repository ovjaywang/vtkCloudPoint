using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace vtkPointCloud
{
    public class DBImproved
    {
        public int clusterAmount = 0;
        public int pointsAmount = 0;
        public static int iritatorNum = 0;
        public int cf = 0;
        public static double getDisP(Point3D p1, Point3D p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            double dz = p1.Z - p2.Z;
            iritatorNum++;
            //double rrrr = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double rrrr = Math.Abs(dx) + Math.Abs(dy);
            //Console.WriteLine(rrrr);
            return rrrr;
            //return Math.Sqrt(dx * dx + dy * dy);
        }
        //public static double getDisP2D(Point3D p1, Point3D p2)
        //{
        //    double dx = p1.X - p2.X;
        //    double dy = p1.Y - p2.Y;
        //    return Math.Sqrt(dx * dx + dy * dy);
        //}
        //判断是否是核心点 minPts是阈值范围内最小邻域点数 e是阈值半径 返回值邻域数组
        private static ArrayList isKeyPoint(List<Point3D> lst, Point3D p, double e, int minPts)
        {
            int count = 0;
            ArrayList tmpList = new ArrayList();
            for (int i = 0; i < lst.Count; i++)
            {
                Point3D p2 = lst[i];
                //if (!p2.ifShown) { continue; }
                if (getDisP(p, p2) <= e)
                {
                    ++count;//若小于距离 则邻域数目加一
                    tmpList.Add(i);
                }
            }
            if (count >= minPts)
            {
                p.isKeyPoint = true;//达到最小邻域数目 是核心点
                return tmpList;
            }
            //tmpList.empty();
            return tmpList;
        }
        //拓展每个聚类簇
        private static void expandCluster(Point3D p, ArrayList nei, int c, double e, int minPts, List<Point3D> lst)
        {
            p.clusterId = c;//簇序列
            for (int i = 0; i < nei.Count; i++)
            {
                Point3D dpp = (Point3D)lst[(int)nei[i]];
                //if (!dpp.ifShown) { continue; }
                if (dpp.isClassed == false)
                {	//未访问
                    dpp.isClassed = true;
                    ArrayList tmpList = new ArrayList();
                    tmpList = isKeyPoint(lst, dpp, e, minPts);
                    if (tmpList.Count >= minPts)
                    {
                        for (int k = 0; k < tmpList.Count; k++)
                        {
                            bool flag = false;
                            for (int j = 0; j < nei.Count; j++)
                            {
                                if (nei[j] == tmpList[k])
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag == false)
                                nei.Add(tmpList[k]);
                        }
                    }
                }
                //加入c
                dpp.clusterId = c;
                //Console.WriteLine("加入 " + c);
            }
        }
        public void dbscan(List<Point3D> lst, double e, int minPts)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                Point3D dpp = lst[i];
                //if (!dpp.ifShown) { continue; }
                //else
                //{
                    pointsAmount++;
                //}
                if (dpp.isClassed)
                    continue;
                ArrayList tmpList = new ArrayList();
                tmpList = isKeyPoint(lst, dpp, e, minPts);
                if (tmpList.Count >= minPts)
                {
                    cf++;
                    //Console.WriteLine("新聚类 " + c);
                    expandCluster(dpp, tmpList, cf, e, minPts, lst);
                }
            }
            this.clusterAmount = cf;
            //Console.WriteLine("当前聚类 ： "+c);
        }
    }
}
