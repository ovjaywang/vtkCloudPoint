using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vtkPointCloud
{
    class ICP
    {
        void ICP(List<Point3D> model,List<Point3D> data,double[] R,double[] T,double e) {
            double pre_d = 0.0, d = 0.0;
            List<Point3D> Y, P;
            int round = 0;
            P = new List<Point3D>(data.ToArray()); // copy of data
            do
            {
                pre_d = d;

		Matrix R1= new Matrix(3,3), T1=new Matrix(3,1);

		Y=FindClosestPointSet(model, P);

		Point3D _mean_P, _mean_Y;

		_mean_P=CalculateMeanPoint3D(P);
		_mean_Y=CalculateMeanPoint3D(Y);

		Matrix A=new Matrix(3,3), delta=new Matrix(3,1);
        Matrix m=Matrix.ZeroMatrix(3,3);

		for(int i=0; i<P.Count; i++)
		{
			Matrix p= new Matrix(3,1), y=new Matrix(1,3), mul= new Matrix(3,3);

			p[0,0] = P[i].X;
			p[1,0] = P[i].Y;
			p[2,0] = P[i].Z;

			y[0,0] = Y[i].X;
			y[0,1] = Y[i].Y;
			y[0,2] = Y[i].Z;

            mul= p * y;
			m= m + mul;
		}
			m= Matrix.Multiply((double)(1/P.Count),m);

		Matrix mean_P=new Matrix(3,1), mean_Y=new Matrix(1,3), mul2=new Matrix(3,3);

		mean_P[0,0] = _mean_P.X;
		mean_P[1,0] = _mean_P.Y;
		mean_P[2,0] = _mean_P.Z;

		mean_Y[0,0] = _mean_Y.X;
		mean_Y[1,0] = _mean_Y.Y;
		mean_Y[2,0] = _mean_Y.Z;

        mul2 = mean_P * mean_Y;
		m = m + mul2;

		Matrix m_T=new Matrix(3,3);
		m_T = Matrix.Transpose(m);

        A= m.Duplicate();
		A = A-m_T;

		delta[0,0] = A[1,2];
		delta[1,0] = A[2,0];
		delta[2,0] = A[0,0];

		double tr = Matrix.TR(m);
		m=m+ m_T;

		Matrix I3= Matrix.ZeroMatrix(3,3);
        I3[0,0]=tr;
        I3[1,1]=tr;
        I3[2,2]=tr;

		m = m- I3;

		Matrix Q=new Matrix(4,4);//Q是四乘四矩阵
		Q[0,0] = tr;

		Q[0,1] = delta[0,0];
        Q[0,2] = delta[1,0];
        Q[0,3] = delta[2,0];

        Q[1,0] = delta[0,0];
        Q[2,0] = delta[1,0];
        Q[3,0] = delta[2,0];

		for(int i=1; i<=3; i++)
		{
			Q[i*4 , 1] = m[4*(i-1) ,0];
			Q[i*4 , 2] = m[4*(i-1) ,1];
			Q[i*4 , 3] = m[4*(i-1) ,2];
		}

		double eigen;
        Matrix qr[4];
		MatrixEigen(Q, &eigen, qr, 4);

		CalculateRotation(qr, R1);

		double qt[3];
		for(int i=0; i<3; i++)
			qt[i] = mean_Y[i];

		Matrix mul1= new Matrix(3,1);
		MatrixMul(R1, mean_P, mul1, 3, 3, 3, 1);
		MatrixDiv(qt, mul1, 3, 1);

		for(int i=0; i<3; i++)
			T1[i] = qt[i];

		d = 0.0;
		it1 = P.begin();
		it2 = Y.begin();
		for(; it1!=P.end(); it1++, it2++)
		{
			double sum = (it1->x - it2->x)*(it1->x - it2->x) + (it1->y - it2->y)*(it1->y - it2->y) + (it1->z - it2->z)*(it1->z - it2->z);

			d += sum;
		}

		round ++;

		if(round > 1)
		{
			printf("*******%d\n", round);
			printf("R:\n");
			for(int i=0; i<3; i++)
				printf("%lf %lf %lf\n", R[3*i], R[3*i + 1], R[3*i + 2]);
			printf("T:\n%lf %lf %lf\n", T[0], T[1], T[2]);
		}

		printf("d:\n%lf\n", d);

		if(abs(d - pre_d) >= e)
		{
			if(round == 1)
			{
				for(int i=0; i<9; i++)
					R[i] = R1[i];

				for(int i=0; i<3; i++)
					T[i] = T1[i];
			}
			else
			{
                 Matrix tempR = new Matrix(3,3);
				Matrix tempT= new Matrix(3,1);
                tempR = R1 * R;
                tempT = R1 * T;

				for(int i=0; i<9; i++)
					R[i] = tempR[i];

				for(int i=0; i<3; i++)
					T[i] = tempT[i] + T1[i];
			}

			TransPoint(data, P, R, T);

}
                
            } while (Math.Abs(d - pre_d) >= e);
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
        /// <summary>
        /// 对数据集每个点，在模型点寻找对应最近点集
        /// </summary>
        /// <param name="?"></param>
        List<Point3D> FindClosestPointSet(List<Point3D> model, List<Point3D> data)
        {
	        List<Point3D> Y= new List<Point3D>();
	        Y.Clear();

	        for(int i=0;i<data.Count;i++)
	        {
		        double min;
                int j = 0,order = 0;
                min = (data[i].X - model[j].X) * (data[i].X - model[j].X) + (data[i].Y - model[j].Y) * (data[i].Y - model[j].Y) + (data[i].Z - model[j].Z) * (data[i].Z - model[j].Z);
		        j ++;
		        for(; j<model.Count; j++)
		        {
			        double d;
                    d = (data[i].X - model[j].X) * (data[i].X - model[j].X) + (data[i].Y - model[j].Y) * (data[i].Y - model[j].Y) + (data[i].Z - model[j].Z) * (data[i].Z - model[j].Z);

			        if(d < min)
			        {
				        min = d;
				        order = j;
			        }
		        }

		        Y.push_back(model[order]);
	        }
            return Y;
        }
        /// <summary>
        /// 计算点集质心
        /// </summary>
        /// <param name="?"></param>
        Point3D CalculateMeanPoint3D(List<Point3D> P)
        {
            Point3D mean = new Point3D();
	        mean.X = 0;
	        mean.Y = 0;
	        mean.Z = 0;

            for (int i = 0;i<P.Count ; i++)
            {
                mean.X += P[i].X;
                mean.Y += P[i].Y;
                mean.Z += P[i].Z;
            }

            mean.X = mean.X / P.Count;
            mean.Y = mean.Y / P.Count;
            mean.Z = mean.Z / P.Count;
            return mean;
        }
        void CalculateRotation(Matrix q, Matrix R)
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



        




    }
}
