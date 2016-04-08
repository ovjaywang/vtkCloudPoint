using System;
using System.Collections.Generic;
using System.Text;

namespace vtkPointCloud
{
    public class Point2D
    {
        public double x { get; set; }
        public double y { get; set; }
        public int clusID { get; set; }
        public double ratio { get; set; }
        public bool isFilter{ get;set; }
        public double radius { get; set; }
        public Point2D() { }
        public Point2D(double xx, double yy, int cc)
        {
            this.x = xx;
            this.y = yy;
            this.clusID = cc;
        }
        public Point2D(double xx, double yy, double rr,bool isFilter) {
            this.x = xx;
            this.y = yy;
            this.radius = rr;
            this.isFilter = isFilter;
        }
        public Point2D(double xx, double yy)
        {
            this.x = xx;
            this.y = yy;
        }
    };

    //3维点所有属性 包括电机xyz 球面坐标xyz 聚类ID 是否已被分类 是否已被匹配 是否被遍历到 是否是关键点 路径Id
    public class Point3D
    {
        public Point3D() { }
        public Point3D(double xx, double yy, double zz)
        {
            this.X = xx;
            this.Y = yy;
            this.Z = zz;
        }
        public Point3D(double xx, double yy, double zz, int clusterId ,bool isShown)
        {
            this.X = xx;
            this.Y = yy;
            this.Z = zz;
            this.clusterId = clusterId;
            this.ifShown = isShown;
        }
        public double motor_x { get; set; }//点机x
        public double motor_y { get; set; }//点机y
        public double Distance { get; set; }//距离
        public double X { get; set; }//三维x
        public double Y { get; set; }//三维y
        public double Z { get; set; }//三维z
        public int clusterId { get; set; }//聚类id
        public int pathId { get; set; }//路径id
        public Boolean ifShown { get; set; }//是否显示
        public Boolean isFilter { get; set; }//是否过滤
        public Boolean isClassed { get; set; }//是否已被分类
        public Boolean isKeyPoint { get; set; }//是否是关键点
        public Boolean isMatched { get; set; }//是否已被匹配
        public int matchNum { get; set; }//匹配号
        public string pointName { get; set; }//真值点名
        public double tmp_X { get; set; }//转换暂时坐标x
        public double tmp_Y { get; set; }//转换暂时坐标y
        public double tmp_Z { get; set; }//转换暂时坐标z
        public double matched_X { get; set; }//匹配坐标x
        public double matched_Y { get; set; }//匹配坐标y
        public double matched_Z { get; set; }//匹配坐标z
    };


    public class Line2D
    {
        public Point2D startPoint;
        public Point2D endPoint;
    };

    public class Line3D
    {
        public Point3D startPoint;
        public Point3D endPoint;
    };

    public class Pixel
    {
        public int x;
        public int y;
        public int value;
    }
    public class Rectangle2D {
         public Rectangle2D() { }
         public Rectangle2D(double xx, double yy, double width, double height)
        {
            this.xx = xx;
            this.yy = yy;
            this.width = width;
            this.height = height;
        }
         public double xx { get; set; }
         public double yy{get;set;}
         public double width { get; set; }
         public double height { get; set; }
         public double Top { get;set; }
         public double Right { get;set; }
         public double Left { get; set;}
         public int Bottom { get; set; }
    }
}
