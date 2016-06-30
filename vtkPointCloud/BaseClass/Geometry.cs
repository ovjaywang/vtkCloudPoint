using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace vtkPointCloud
{
    static class Geometry
    {
        // For debugging.
        public static Point3D[] g_MinMaxCorners;
        public static Rectangle2D g_MinMaxBox;
        public static Point3D[] g_NonCulledPoints;

        // Find the points nearest the upper left, upper right,
        // lower left, and lower right corners.
        private static void GetMinMaxCorners(List<Point3D> points, ref Point3D ul, ref Point3D ur, ref Point3D ll, ref Point3D lr)
        {
            // Start with the first point as the solution.
            ul = points[0];
            ur = ul;
            ll = ul;
            lr = ul;
            // Search the other points.
            foreach (Point3D pt in points)
            {
                if (-pt.X - pt.Y > -ul.X - ul.Y) ul = pt;
                if (pt.X - pt.Y > ur.X - ur.Y) ur = pt;
                if (-pt.X + pt.Y > -ll.X + ll.Y) ll = pt;
                if (pt.X + pt.Y > lr.X + lr.Y) lr = pt;
            }
            g_MinMaxCorners = new Point3D[] { ul, ur, lr, ll }; // For debugging.
        }

        // Find a box that fits inside the MinMax quadrilateral.
        private static Rectangle2D GetMinMaxBox(List<Point3D> points)
        {
            // Find the MinMax quadrilateral.
            Point3D ul = new Point3D(0, 0,0), ur = ul, ll = ul, lr = ul;
            GetMinMaxCorners(points, ref ul, ref ur, ref ll, ref lr);

            // Get the coordinates of a box that lies inside this quadrilateral.
            double xmin, xmax, ymin, ymax;
            xmin = ul.X;
            ymin = ul.Y;

            xmax = ur.X;
            if (ymin < ur.Y) ymin = ur.Y;

            if (xmax > lr.X) xmax = lr.X;
            ymax = lr.Y;

            if (xmin < ll.X) xmin = ll.X;
            if (ymax > ll.Y) ymax = ll.Y;

            Rectangle2D result = new Rectangle2D(xmin, ymin, xmax - xmin, ymax - ymin);
            g_MinMaxBox = result;    // For debugging.
            return result;
        }

        // Cull points out of the convex hull that lie inside the
        // trapezoid defined by the vertices with smallest and
        // largest X and Y coordinates.
        // Return the points that are not culled.
        private static List<Point3D> HullCull(List<Point3D> points)//获取外界多边形定点
        {
            // Find a culling box.
            Rectangle2D culling_box = GetMinMaxBox(points);

            // Cull the points.
            List<Point3D> results = new List<Point3D>();
            foreach (Point3D pt in points)
            {
                // See if (this point lies outside of the culling box.
                if (pt.X <= culling_box.Left ||
                    pt.X >= culling_box.Right ||
                    pt.Y <= culling_box.Top ||
                    pt.Y >= culling_box.Bottom)
                {
                    // This point cannot be culled.
                    // Add it to the results.
                    results.Add(pt);
                }
            }
            g_NonCulledPoints = new Point3D[results.Count];   // For debugging.
            results.CopyTo(g_NonCulledPoints);              // For debugging.
            return results;
        }

        public static List<Point2D> MakeConvexHull(List<Point3D> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest Y value.
            // if (there's a tie, take the one with the smaller X value.
            Point3D best_pt = points[0];
            foreach (Point3D pt in points)
            {
                if ((pt.Y < best_pt.Y) ||
                   ((pt.Y == best_pt.Y) && (pt.X < best_pt.X)))
                {
                    best_pt = pt;
                }
            }

            // Move this point to the convex hull.
            List<Point2D> hull = new List<Point2D>();
            //hull.Add(best_pt);
            hull.Add(new Point2D(best_pt.X, best_pt.Y));
            points.Remove(best_pt);

            // Start wrapping up the other points.
            double sweep_angle = 0;
            for (;;)
            {
                // Find the point with smallest AngleValue
                // from the last point.
                double X = hull[hull.Count - 1].x;
                double Y = hull[hull.Count - 1].y;
                best_pt = points[0];
                double best_angle = 3600;

                // Search the rest of the points.
                foreach (Point3D pt in points)
                {
                    double test_angle = AngleValue(X, Y, pt.X, pt.Y);
                    if ((test_angle >= sweep_angle) &&
                        (best_angle > test_angle))
                    {
                        best_angle = test_angle;
                        best_pt = pt;
                    }
                }

                // See if the first point is better.
                // If so, we are done.
                double first_angle = AngleValue(X, Y, hull[0].x, hull[0].y);
                if ((first_angle >= sweep_angle) &&
                    (best_angle >= first_angle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(new Point2D(best_pt.X, best_pt.Y));
                points.Remove(best_pt);

                sweep_angle = best_angle;

                // If all of the points are on the hull, we're done.
                if (points.Count == 0) break;
            }

            return hull;
        }
        // Return a number that gives the ordering of angles
        // WRST horizontal from the point (x1, y1) to (x2, y2).
        // In other words, AngleValue(x1, y1, x2, y2) is not
        // the angle, but if:
        //   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
        // then
        //   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
        // this angle is greater than the angle for another set
        // of points,) this number for
        //
        // This function is dy / (dy + dx).
        private static double AngleValue(double x1, double y1, double x2, double y2)
        {
            double dx, dy, ax, ay, t;

            dx = x2 - x1;
            ax = Math.Abs(dx);
            dy = y2 - y1;
            ay = Math.Abs(dy);
            if (ax + ay == 0)
            {
                // if (the two points are the same, return 360.
                t = 360f / 9f;
            }
            else
            {
                t = dy / (ax + ay);
            }
            if (dx < 0)
            {
                t = 2 - t;
            }
            else if (dy < 0)
            {
                t = 4 + t;
            }
            return t * 90;
        }
        public static void FindMinimalBoundingCircle(List<Point3D> points, out Point2D center, out double radius)
        {
            // Find the convex hull.
            List<Point2D> hull = MakeConvexHull(points);

            // The best solution so far.
            Point2D best_center = new Point2D(points[0].X, points[0].Y);
            double best_radius2 = double.MaxValue;

            // Look at pairs of hull points.
            for (int i = 0; i < hull.Count - 1; i++)
            {
                for (int j = i + 1; j < hull.Count; j++)
                {
                    // Find the circle through these two points.
                    Point2D test_center = new Point2D(
                        (hull[i].x + hull[j].x) / 2f,
                        (hull[i].y + hull[j].y) / 2f);
                    double dx = test_center.x - hull[i].x;
                    double dy = test_center.y - hull[i].y;
                    double test_radius2 = dx * dx + dy * dy;

                    // See if this circle would be an improvement.
                    if (test_radius2 < best_radius2)
                    {
                        // See if this circle encloses all of the points.
                        if (CircleEnclosesPoints(test_center, test_radius2, hull, i, j, -1))
                        {
                            // Save this solution.
                            best_center = test_center;
                            best_radius2 = test_radius2;
                        }
                    }
                } // for i
            } // for j

            // Look at triples of hull points.
            for (int i = 0; i < hull.Count - 2; i++)
            {
                for (int j = i + 1; j < hull.Count - 1; j++)
                {
                    for (int k = j + 1; k < hull.Count; k++)
                    {
                        // Find the circle through these three points.
                        Point2D test_center;
                        double test_radius2;
                        FindCircle(hull[i], hull[j], hull[k], out test_center, out test_radius2);

                        // See if this circle would be an improvement.
                        if (test_radius2 < best_radius2)
                        {
                            // See if this circle encloses all of the points.
                            if (CircleEnclosesPoints(test_center, test_radius2, hull, i, j, k))
                            {
                                // Save this solution.
                                best_center = test_center;
                                best_radius2 = test_radius2;
                            }
                        }
                    } // for k
                } // for i
            } // for j

            center = best_center;
            if (best_radius2 == double.MaxValue)
                radius = 0;
            else
                radius = (double)Math.Sqrt(best_radius2);
        }

        // Return true if the indicated circle encloses all of the points.
        private static bool CircleEnclosesPoints(Point2D center,
            double radius2, List<Point2D> points, int skip1, int skip2, int skip3)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if ((i != skip1) && (i != skip2) && (i != skip3))
                {
                    Point2D point = points[i];
                    double dx = center.x - point.x;
                    double dy = center.y - point.y;
                    double test_radius2 = dx * dx + dy * dy;
                    if (test_radius2 > radius2) return false;
                }
            }
            return true;
        }

        // Find a circle through the three points.
        private static void FindCircle(Point2D a, Point2D b, Point2D c, out Point2D center, out double radius2)
        {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            double x1 = (b.x + a.x) / 2;
            double y1 = (b.y + a.y) / 2;
            double dy1 = b.x - a.x;
            double dx1 = -(b.y - a.y);

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            double x2 = (c.x + b.x) / 2;
            double y2 = (c.y + b.y) / 2;
            double dy2 = c.x - b.x;
            double dx2 = -(c.y - b.y);

            // See where the lines intersect.
            bool lines_intersect, segments_intersect;
            Point2D intersection, close_p1, close_p2;
            FindIntersection(
                new Point2D(x1, y1),
                new Point2D(x1 + dx1, y1 + dy1),
                new Point2D(x2, y2),
                new Point2D(x2 + dx2, y2 + dy2),
                out lines_intersect,
                out segments_intersect,
                out intersection,
                out close_p1,
                out close_p2);

            center = intersection;
            double dx = center.x - a.x;
            double dy = center.y - a.y;
            radius2 = dx * dx + dy * dy;
        }
        private static void FindIntersection(Point2D p1, Point2D p2, Point2D p3, Point2D p4,
            out bool lines_intersect, out bool segments_intersect,
            out Point2D intersection, out Point2D close_p1, out Point2D close_p2)
        {
            // Get the segments' parameters.
            double dx12 = p2.x - p1.x;
            double dy12 = p2.y - p1.y;
            double dx34 = p4.x - p3.x;
            double dy34 = p4.y - p3.y;

            // Solve for t1 and t2
            double denominator = (dy12 * dx34 - dx12 * dy34);

            double t1;
            try
            {
                t1 = ((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34) / denominator;
            }
            catch
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new Point2D(double.NaN, double.NaN);
                close_p1 = new Point2D(double.NaN, double.NaN);
                close_p2 = new Point2D(double.NaN, double.NaN);
                return;
            }
            lines_intersect = true;

            double t2 = ((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12) / -denominator;

            // Find the point of intersection.
            intersection = new Point2D(p1.x + dx12 * t1, p1.y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new Point2D(p1.x + dx12 * t1, p1.y + dy12 * t1);
            close_p2 = new Point2D(p3.x + dx34 * t2, p3.y + dy34 * t2);
        }
    }
}
