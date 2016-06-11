using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vtkPointCloud
{
    class ICP
    {
        void ICP(List<Point3D> model,List<Point3D> data,double[] R,double[] T,double e) {
            double pred = 0.0, d = 0.0;
            List<Point3D> Y, P;
            int round = 0;
            P = new List<Point3D>(data.ToArray()); // copy of data

        }

        void CalculateRotation(double[] q, double[] R)
        {
            R[0] = q[0] * q[0] + q[1] * q[1] - q[2] * q[2] - q[3] * q[3];
            R[1] = 2.0 * (q[1] * q[2] - q[0] * q[3]);
            R[2] = 2.0 * (q[1] * q[3] + q[0] * q[2]);
            R[3] = 2.0 * (q[1] * q[2] + q[0] * q[3]);
            R[4] = q[0] * q[0] - q[1] * q[1] + q[2] * q[2] - q[3] * q[3];
            R[5] = 2.0 * (q[2] * q[3] - q[0] * q[1]);
            R[6] = 2.0 * (q[1] * q[3] - q[0] * q[2]);
            R[7] = 2.0 * (q[2] * q[3] + q[0] * q[1]);
            R[8] = q[0] * q[0] - q[1] * q[1] - q[2] * q[2] + q[3] * q[3];
        }
        List<Point3D>  TransPoint(List<Point3D> src, Matrix R, Matrix T)
        {
	        List<Point3D> dst = new List<Point3D>();
	        dst.Clear();

	        for(int i=0;i<src.Count;i++)
	        { 
                Matrix p = new Matrix(3,1);
                Matrix r = new Matrix(3,1);
                Matrix z = new Matrix(3, 1);
                p[0,0]= src[i].X;
		        p[1,0] = src[i].Y;
                p[2,0] = src[i].Z;
                r=Matrix.Multiply(R, p);
                z = Matrix.Add(r, T);

		        Point3D point = new Point3D();
                point.X = z[0,0];
		        point.Y = z[1,0];
		        point.Z = z[2,0];

		        dst.Add(point);
	        }
            return dst;
}






    }
}
