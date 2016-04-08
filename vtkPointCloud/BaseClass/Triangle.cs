using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace vtkPointCloud
{
    public class Triangle : Polygon
    {
        public Triangle(Point2D p0, Point2D p1, Point2D p2)
        {
            Points = new Point2D[] { p0, p1, p2 };
        }
    }
}
