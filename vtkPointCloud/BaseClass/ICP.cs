using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vtkPointCloud
{
    class ICP
    {
        /// <summary>
        /// ICP主函数
        /// </summary>
        /// <param name="model">模型点-真值</param>
        /// <param name="data">数据点-测量</param>
        /// <param name="R">旋转矩阵 3*3 矩阵</param>
        /// <param name="T">平移矩阵 3*1矩阵</param>
        /// <param name="e">阈值</param>
        public void go_hell_ICP(List<Point3D> model,List<Point3D> data,Matrix R,Matrix T,double e) {
            double pre_d = 0.0, d = 0.0;
            List<Point3D> Y, P;
            int round = 0;
            P = new List<Point3D>(data.ToArray()); // copy of data
       do
       {
            pre_d = d;
		    Matrix R1= new Matrix(3,3), T1=new Matrix(3,1);
           //每次遍历找到最近点集的最近对应点集
		    Y=FindClosestPointSet(model, P);
		    Point3D _mean_P, _mean_Y;

		    _mean_P=CalculateMeanPoint3D(P);
		    _mean_Y=CalculateMeanPoint3D(Y);
            Console.WriteLine("Means of P : "+_mean_P.X + "\t" + _mean_P.Y + "\t"+_mean_P.Z);
            Console.WriteLine("Means of Y : " + _mean_Y.X + "\t" + _mean_Y.Y + "\t" + _mean_Y.Z);
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
            Console.WriteLine(m.ToString());
		    Matrix mean_P=new Matrix(3,1), mean_Y=new Matrix(1,3), mul2=new Matrix(3,3);

		    mean_P[0,0] = _mean_P.X;
		    mean_P[1,0] = _mean_P.Y;
		    mean_P[2,0] = _mean_P.Z;

		    mean_Y[0,0] = _mean_Y.X;
		    mean_Y[0,1] = _mean_Y.Y;
		    mean_Y[0,2] = _mean_Y.Z;

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
			    Q[i , 1] = m[(i-1) ,0];
			    Q[i , 2] = m[(i-1) ,1];
			    Q[i , 3] = m[(i-1) ,2];
		    }
		    double[] eigen = new double[4];
            Matrix qr=new Matrix(4,4);
            //public static bool ComputeEvJacobi(Matrix m,double[] dblEigenValue, Matrix mtxEigenVector, int nMaxIt, double eps)
            double epss = 0.0001;

		    bool rs=Matrix.ComputeEvJacobi(Q, eigen, qr,100, epss);
            Console.WriteLine("解求特征值结果 : " + rs);
		    CalculateRotation(qr, R1);

		    Matrix qt=new Matrix(3,1);
		    for(int i=0; i<3; i++)
			    qt[i,0] = mean_Y[0,i];

		    Matrix mul1= new Matrix(3,1);
            //R1 3*3;mean_P 3*1;mull 3*1
            mul1 = Matrix.Multiply(R1, mean_P);
            qt = qt - mul1;

		    for(int i=0; i<3; i++)
			    T1[i,0] = qt[0,i];

		    d = 0.0;
		    //it1 = P.begin();
		    //it2 = Y.begin();
            for (int p = 0; p < P.Count;p++ )
            {
                double sum = (P[p].X - Y[p].X) * (P[p].X - Y[p].X) + (P[p].Y - Y[p].Y) * (P[p].Y - Y[p].Y) + (P[p].Z - Y[p].Z) * (P[p].Z - Y[p].Z);
                d += sum;
            }
		    round ++;

		    if(round > 1)
		    {
			    //printf("*******%d\n", round);
                Console.WriteLine("*******循环 " + round+"次*********");
			    //printf("R:\n");
                Console.WriteLine("R:");
			    for(int i=0; i<3; i++){
                    Console.WriteLine(R[i, 0] + " " + R[i, 1] + " " + R[i,2]);
                }
				    //printf("%lf %lf %lf\n", R[3*i], R[3*i + 1], R[3*i + 2]);
			    //printf("T:\n%lf %lf %lf\n", T[0], T[1], T[2]);
		    }
		//printf("d:\n%lf\n", d);
		        if(Math.Abs(d - pre_d) >= e)
		        {
			        if(round == 1)
			        {
                        for (int i = 0; i < 3; i++)
                        {
                            R[i,0] = R1[i,0];
                            R[i,1] = R1[i,1];
                            R[i,2] = R1[i,2];
                        }

				        for(int i=0; i<3; i++)
                            T[i, 0] = T1[i, 0];
			        }
			        else
			        {
                         Matrix tempR = new Matrix(3,3);
				        Matrix tempT= new Matrix(3,1);
                        tempR = R1 * R;
                        tempT = R1 * T;

                        for (int i = 0; i < 9; i++) {
                            R[i, 0] = tempR[i, 0];
                            R[i, 1] = tempR[i, 1];
                            R[i, 2] = tempR[i, 2];
                        }
				        for(int i=0; i<3; i++)
                            T[i, 0] =  tempT[i, 0]+T1[i, 0];
			        }
			         P=TransPoint(data, R, T);
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

		        Y.Add(model[order]);
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
            R[0, 0] = q[0, 0] * q[0, 0] + q[1, 0] * q[1, 0] - q[2, 0] * q[2, 0] - q[3, 0] * q[3, 0];
            R[0, 1] = 2.0 * (q[1, 0] * q[2, 0] - q[0, 0] * q[3, 0]);
            R[0, 2] = 2.0 * (q[1, 0] * q[3, 0] + q[0, 0] * q[2, 0]);
            R[1, 0] = 2.0 * (q[1, 0] * q[2, 0] + q[0, 0] * q[3, 0]);
            R[1, 1] = q[0, 0] * q[0, 0] - q[1, 0] * q[1, 0] + q[2, 0] * q[2, 0] - q[3, 0] * q[3, 0];
            R[1, 2] = 2.0 * (q[2, 0] * q[3, 0] - q[0, 0] * q[1, 0]);
            R[2, 0] = 2.0 * (q[1, 0] * q[3, 0] - q[0, 0] * q[2, 0]);
            R[2, 1] = 2.0 * (q[2, 0] * q[3, 0] + q[0, 0] * q[1, 0]);
            R[2, 2] = q[0, 0] * q[0, 0] - q[1, 0] * q[1, 0] - q[2, 0] * q[2, 0] + q[3, 0] * q[3, 0];
        }
        //List<Point3D> TransPoint(List<Point3D> src, Matrix R, Matrix T)
        //{
        //    List<Point3D> dst = new List<Point3D>();

        //    for(int i=0;i<src.Count;i++)
        //    {
        //        Matrix p=new Matrix(3,1), r=new Matrix(3,1);
        //        p[0,0] = src[i].X;
        //        p[1, 0] = src[i].Y;
        //        p[2, 0] = src[i].Z;
        //        r = R * p;
        //        r = r + T;

        //        Point3D point = new Point3D();
        //        point.X = r[0,0];
        //        point.Y = r[1, 0];
        //        point.Z = r[2, 0];

        //        dst.Add(point);
        //    }
        //    return dst;
        //}
}


        


}
