using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections;
using vtk;
using System.Threading;
//x是45.439，y 35.452
namespace vtkPointCloud
{
    public partial class MainForm : Form
    {
        //目录和可视化模块相关
        
        public int bit = 0;
        vtkRenderer ren = null;
        string fullFilePath;
        TreeNode root = null;//根目录
        string selPath;//自身路径
        vtkFormsWindowControl vtkControl = null;
        List<double> xSet = new List<double>(),ySet = new List<double>(),zSet = new List<double>();//x,y,z坐标集合
        public double x_angle = 0.0, y_angle = 0.0;//x y灵位角度
        //dbscan相关
        //public DB dbb;//DB类对象
        public DBImproved dbb;
        double threhold;//dbscan阈值
        int pointsInthrehold;//dbscan点数
        public bool isSureClusterRs = false;
        ClusterParameters cp ;
        //点集相关
        public List<Point3D> rawData = new List<Point3D>();//raw是原始x y z值数据
        //List<Point3D>[] fixedData;//固定点分组测量数据
        bool isIgnoreDuplication = true;
        public List<Point3D>[] grouping = null;
        List<List<Point3D>> classedrawData = new List<List<Point3D>>();//源文件匹配相关
        ArrayList pathList = new ArrayList();//路径列表
        public List<Point3D> centers = null;//同聚类中心
        List<Point3D> scanCen = null;//真值点
        List<int> matchedID = new List<int>();//已匹配ID
        //多线程相关
        private delegate void funHandle(int nValue);//声明计算聚类的线程
        public delegate void CallBackDelegate(string message);
        private static WaitingForm progressForm = new WaitingForm();
        private ProgressBar psbar = progressForm.progressBar1;
        private delegate void UpdateStatusDelegate();
        private BackgroundWorker bkWorker = new BackgroundWorker();
        private BackgroundWorker bkWorker2 = new BackgroundWorker();
        private int percentValue = 0;
        //icp相关
        vtkMatrix4x4 M;//刚性变换矩阵
        vtkPoints truePointCloud = null;//真值点云
        vtkPoints centroidPointCloud = null;//质心点云
        int[] truePointPid = new int[1];//真值点Id
        vtkCellArray truePointVertices = new vtkCellArray();//真值顶点
        vtkPolyVertex truePolyVertex = new vtkPolyVertex();

        vtkPoints visualizeTruePointCloud = null;//为了可视化加入的旁侧真值点
        int clock = 0;
        int clock_x = 1, clock_y = 1;//x y轴正负
        int[] vtpc_Pid = new int[1];
        int[] vcpc_Pid = new int[1];
        vtkCellArray vtpVertices = new vtkCellArray();
        vtkCellArray vcpVertices = new vtkCellArray();
        //简单聚类相关
        public int clusterSum = 1;
        int pointSum = 0;
        double simple_x_step,simple_y_step;
        vtkActor actorLine = new vtkActor(), actorLine2 = new vtkActor(),actorLine3 = new vtkActor(),actorLine4 = new vtkActor();
        List<Point3D> sourceTrueList = null;
        //public double distanceFilterThrehold = 0.0;
        //源文件聚类相关
        int true_rotationRb = 0;
        double true_scale = 1;
        double true_xshift = 0;
        double true_yshift = 0;
        bool true_noTrans = true;
        bool true_xasix = false;
        bool true_yasix = false;
        List<Point3D> transPts = null;
        List<Point3D> showPts = null;
        //参数相关
        public List<Point2D>[] hulls;//个数为聚类数 每个元素为某ID的所有点集
        public List<Point2D> circles;
        public List<int> filterID = new List<int>();
        double[] scale = new double[3];//比例尺 三个方向
        double[] trueScale;//真值范围
        double[] centroidScale;//质心范围
        public MainForm()
        {
            InitializeComponent();
            string str = System.Environment.CurrentDirectory;
                 Console.Write(str);
            CheckForIllegalCrossThreadCalls = false;

            bkWorker.WorkerReportsProgress = true;
            bkWorker.WorkerSupportsCancellation = true;
            bkWorker.DoWork += new DoWorkEventHandler(DoWork);
            bkWorker.ProgressChanged += new ProgressChangedEventHandler(ProgessChanged);
            bkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWork);

            bkWorker2.WorkerReportsProgress = true;
            bkWorker2.WorkerSupportsCancellation = true;
            bkWorker2.DoWork += new DoWorkEventHandler(DoWork2);
            bkWorker2.ProgressChanged += new ProgressChangedEventHandler(ProgessChanged2);
            bkWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWork2);
            if (vtkControl == null)
            {
                vtkControl = new vtkFormsWindowControl();
            }

            root = new TreeNode("Cloud　Point");
            treeView1.Nodes.Add(root);

            //this.treeView1.AfterCheck += new TreeViewEventHandler(treeView1_AfterCheck);
            vtkControl.Location = new Point(30, 30);
            vtkControl.Name = "vtkControl";
            vtkControl.TabIndex = 0;
            vtkControl.Text = "vtkFormsWindowControl";
            vtkControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            vtkControl.Dock = DockStyle.Fill;
            this.Controls.Add(vtkControl);
            //vtkInteractorStyleTrackballActor sty = new vtkInteractorStyleTrackballActor();
            vtkInteractorStyleTrackballCamera sty = new vtkInteractorStyleTrackballCamera();
            vtkRenderWindowInteractor ir = new vtkRenderWindowInteractor();
            ir.SetRenderWindow(this.vtkControl.GetRenderWindow());
            ir.SetInteractorStyle(sty);
            //加载图像
        }
        private void UpdateStatus()
        {
            ShowPointsFromFile(rawData, 2);
            vtkControl.Refresh();
            //MessageBox.Show("总共" + arr.Count + "个点云，总共生成" + dbb.getClustersNum(arr) + "个聚类", "提示");
            this.toolStripStatusLabelCurrentPointCount.Text = String.Format("当前点云数：{0}，当前聚类数： {1}", dbb.pointsAmount, dbb.clusterAmount);
            treeView1.Enabled = false;
        }
        private void UpdateStatus2()
        {
            vtkControl.Refresh();
            vtkControl.GetRenderWindow().Render();
            vtkControl.GetRenderWindow().Start();
        }
        /// <summary>
        /// 查询某treeNode节点下有多少节点被选中（递归实现，不受级数限制）
        /// </summary>
        /// <param name="tn">TreeNode节点</param>
        /// <returns></returns>
        private int GetNodeChecked(TreeNode tn)//查询treenode下被勾选数
        {
            int x = 0;
            if (tn.Checked)
            {
                x++;
            }
            foreach (TreeNode item in tn.Nodes)
            {
                x += GetNodeChecked(item);

            }
            return x;
        }

        /// <summary>
        /// 查询TreeView下节点被checked的数目
        /// </summary>
        /// <param name="treev"></param>
        /// <returns></returns>
        private int GetTreeViewNodeChecked(TreeView treev)//查询treeview被选中数
        {
            int k = 0;

            foreach (TreeNode it in root.Nodes)
            {
                foreach (TreeNode ii in it.Nodes)
                {
                    if (ii.Checked)
                    {
                        k++;
                    }
                }
            }

            return k;
        }
       
        /// <summary>
        /// 该方法自我迭代 遍历所有父节点 给父节点赋予父节点的值(在false时正确 true时不完善)
        /// </summary>
        /// <param name="currNode">当前节点</param>
        /// <param name="state">节点状态</param>
        /// <returns></returns>
        private void setParentNodeCheckedState(TreeNode currNode, bool state)//给父节点赋予子节点的false
        {
            TreeNode parentNode = currNode.Parent;

            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        /// <summary>
        /// 该方法自我迭代 遍历所有子节点 给子节点赋予父节点的值
        /// </summary>
        /// <param name="currNode">当前节点</param>
        /// <param name="state">节点状态</param>
        /// <returns></returns>
        private void setChildNodeCheckedState(TreeNode currNode, bool state)//给子节点赋予父节点的值
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
        }
        /// <summary>
        /// 展示匹配结果
        /// </summary>
        /// <returns></returns>
        public void showMatchResult() //显示匹配点
        {

            ren = new vtkRenderer();
            vtkControl.GetRenderWindow().Clean();
            if (centers.Count == 0) { return; }
            vtkPoints MatchedPoint = new vtkPoints();
            vtkCellArray vtMatched = new vtkCellArray();
            int nPts = centers.Count;
            int[] ids = new int[nPts];
            int count = 0;
            for (int i = 0; i < nPts; i++)
            {
                if (((Point3D)centers[i]).isMatched)
                {
                    ids[i] = MatchedPoint.InsertNextPoint(centers[i].X, centers[i].Y, centers[i].Z);
                    count++;
                }
            }
            vtMatched.InsertNextCell(count, ids);
            vtkPolyData matchedPointPoly = new vtkPolyData();
            matchedPointPoly.SetPoints(MatchedPoint);
            matchedPointPoly.SetVerts(vtMatched);

            vtkPolyDataMapper mapper1 = new vtkPolyDataMapper();
            mapper1.SetInput(matchedPointPoly);

            vtkActor actorMatched = new vtkActor();
            actorMatched.SetMapper(mapper1);
            actorMatched.GetProperty().SetColor(1.0, 0, 0);
            actorMatched.GetProperty().SetPointSize(3);
            ren.AddActor(actorMatched);

            count = 0;
            vtkPoints UnMatchedPoint = new vtkPoints();
            vtkCellArray vtUnMatched = new vtkCellArray();
            ids = new int[nPts];
            for (int i = 0; i < nPts; i++)
            {
                if (!((Point3D)centers[i]).isMatched)
                {
                    ids[i] = UnMatchedPoint.InsertNextPoint(((Point3D)centers[i]).X, ((Point3D)centers[i]).Y, ((Point3D)centers[i]).Z);
                    count++;
                }
            }
            vtUnMatched.InsertNextCell(count, ids);
            vtkPolyData unMatchedPointPoly = new vtkPolyData();
            unMatchedPointPoly.SetPoints(UnMatchedPoint);
            unMatchedPointPoly.SetVerts(vtUnMatched);


            vtkPolyDataMapper mapper2 = new vtkPolyDataMapper();
            mapper2.SetInput(unMatchedPointPoly);

            vtkActor actorUnMatched = new vtkActor();
            actorUnMatched.SetMapper(mapper2);
            actorUnMatched.GetProperty().SetColor(0, 1.0, 0);
            actorUnMatched.GetProperty().SetPointSize(3);
            ren.AddActor(actorUnMatched);

            //count = 0;
            //vtkPoints locPoints = new vtkPoints();
            vtkCellArray vtLoc = new vtkCellArray();
            //nPts = trueLoc.Count;
            //ids = new int[nPts];
            //for (int i = 0; i < nPts; i++)
            //{
            //    ids[i] = locPoints.InsertNextPoint(trueLoc[i].X,trueLoc[i].Y,trueLoc[i].Z);
            //        count++;
            //}
            vtLoc.InsertNextCell(truePointCloud.GetReferenceCount(), ids);
            vtkPolyData locPointPoly = new vtkPolyData();
            locPointPoly.SetPoints(truePointCloud);
            locPointPoly.SetVerts(vtLoc);


            vtkPolyDataMapper mapper3 = new vtkPolyDataMapper();
            mapper3.SetInput(locPointPoly);

            vtkActor actorLoc = new vtkActor();
            actorLoc.SetMapper(mapper3);
            actorLoc.GetProperty().SetColor(0, 0, 1.0);
            actorLoc.GetProperty().SetPointSize(3);
            ren.AddActor(actorLoc);


            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();

        }
        private void showMatchedLine()//画匹配线
        {

            ren = new vtkRenderer();
            vtkControl.GetRenderWindow().Clean();
            vtkPolyData matchPolydata = new vtkPolyData(); ;//对真值处理
            vtkPoints matchPoints = new vtkPoints();
            vtkCellArray matchCellArry = new vtkCellArray();

            vtkPolyData unMatchPolydata = new vtkPolyData();
            vtkPoints unMatchPointCloud = new vtkPoints();
            vtkCellArray unMatchCellArry = new vtkCellArray();

            vtkPolyData unMatchTruePolydata = new vtkPolyData();
            vtkPoints unMatchTruePointCloud = new vtkPoints();
            vtkCellArray unMatchTrueCellArry = new vtkCellArray();
            int[] match_Pid = new int[1];
            for (int i = 0; i < centers.Count; i++)
            {
                if (centers[i].isMatched)
                {
                    match_Pid[0] = matchPoints.InsertNextPoint(centers[i].matched_X, centers[i].matched_Y, centers[i].matched_Z);
                    matchCellArry.InsertNextCell(1, match_Pid);

                    match_Pid[0] = matchPoints.InsertNextPoint(truePointCloud.GetPoint(centers[i].matchNum));
                    matchCellArry.InsertNextCell(1, match_Pid);
                }
                else
                {
                    match_Pid[0] = unMatchPointCloud.InsertNextPoint(centers[i].matched_X, centers[i].matched_Y, centers[i].matched_Z);
                    unMatchCellArry.InsertNextCell(1, match_Pid);

                }
            }
            for (int i = 0; i < truePointCloud.GetNumberOfPoints(); i++)
            {
                if (!(matchedID.Contains(i)))
                {
                    match_Pid[0] = unMatchTruePointCloud.InsertNextPoint(truePointCloud.GetPoint(i));
                    unMatchTrueCellArry.InsertNextCell(1, match_Pid);
                }
            }
            matchPolydata.SetPoints(matchPoints); //把点导入的polydata中去
            matchPolydata.SetVerts(matchCellArry);

            unMatchPolydata.SetPoints(unMatchPointCloud);
            unMatchPolydata.SetVerts(unMatchCellArry);

            unMatchTruePolydata.SetPoints(unMatchTruePointCloud);
            unMatchTruePolydata.SetVerts(unMatchTrueCellArry);
            //Mapper
            vtkPolyDataMapper MatchDataMapper = new vtkPolyDataMapper();
            MatchDataMapper.SetInputConnection(matchPolydata.GetProducerPort());
            vtkPolyDataMapper unMatchMapper = new vtkPolyDataMapper();
            unMatchMapper.SetInputConnection(unMatchPolydata.GetProducerPort());
            vtkPolyDataMapper unMatchTrueMapper = new vtkPolyDataMapper();
            unMatchTrueMapper.SetInputConnection(unMatchTruePolydata.GetProducerPort());

            vtkActor matchActor = new vtkActor();
            vtkActor unMatchActor = new vtkActor();
            vtkActor unMatchTrueActor = new vtkActor();

            matchActor.SetMapper(MatchDataMapper);
            matchActor.GetProperty().SetColor(1, 0, 0);
            matchActor.GetProperty().SetPointSize(5);
            unMatchActor.SetMapper(unMatchMapper);
            unMatchActor.GetProperty().SetColor(0, 1, 0);
            unMatchActor.GetProperty().SetPointSize(5);
            unMatchTrueActor.SetMapper(unMatchTrueMapper);
            unMatchTrueActor.GetProperty().SetColor(0, 0, 1);
            unMatchTrueActor.GetProperty().SetPointSize(5);

            ren.AddActor(matchActor);
            ren.AddActor(unMatchActor);
            ren.AddActor(unMatchTrueActor);
            for (int j = 0; j < centers.Count; j++)
            {

                if (centers[j].isMatched)
                {
                    vtkLineSource lineSource = new vtkLineSource();
                    lineSource.SetPoint1(centers[j].matched_X, centers[j].matched_Y, centers[j].matched_Z);
                    lineSource.SetPoint2(truePointCloud.GetPoint(centers[j].matchNum)[0],
                        truePointCloud.GetPoint(centers[j].matchNum)[1], truePointCloud.GetPoint(centers[j].matchNum)[2]);
                    lineSource.Update();
                    // Visualize
                    vtkPolyDataMapper mapper = new vtkPolyDataMapper();
                    mapper.SetInputConnection(lineSource.GetOutputPort());
                    vtkActor actorLine = new vtkActor();
                    actorLine.SetMapper(mapper);
                    actorLine.GetProperty().SetLineWidth(4);
                    ren.AddActor(actorLine);
                }
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
        }
        //显示文本数据点云
        /// <summary>
        /// 显示不同方式的点云
        /// </summary>
        /// <param name="points">List Point3D 点集</param>
        /// <param name="type">//type 1 默认显示 2 核心点显示 误差点显示 3显示质心  4##  5简单过滤显示质心 </param>
        /// <param name="type">6 按照distance过滤-显示过滤点 7 按照distance过滤-不显示过滤点</param>
        public void ShowPointsFromFile(List<Point3D> points, int type) //不同方式显示点云
        {
            vtkControl.GetRenderWindow().Clean();
            //vtkRenderWindow renderWindow = vtkControl.GetRenderWindow();
            if (type == 1 || type == 2 || type==6 || type==7)
            {
                ren = new vtkRenderer();
            }
            vtkPoints pointCloud_1 = new vtkPoints();//原始点或质心点
            vtkPoints pointCloud_2 = new vtkPoints();//误差点
            vtkPoints pointCloud_3 = new vtkPoints();//核心点
            int count_1 = 0, count_2 = 0, count_3 = 0; ;
            if (type == 2)
            {
                int cs = 0;
                if (dbb == null)
                {
                    cs = clusterSum;
                    //MessageBox.Show(cs + "");
                }
                else
                {
                    cs = dbb.clusterAmount;
                }
                hulls = new List<Point2D>[cs];
                for (int i = 0; i < cs; i++)
                {
                    hulls[i] = new List<Point2D>();
                }
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].ifShown)
                {
                    if (type == 1 || type == 3)
                    {
                        pointCloud_1.InsertPoint(count_1, points[i].X, points[i].Y, points[i].Z);
                        count_1++;
                    }
                    else if (type == 2)
                    {
                        if (points[i].clusterId == 0)//不是核心点
                        {
                            pointCloud_2.InsertPoint(count_2, points[i].X, points[i].Y, points[i].Z);
                            count_2++;
                        }
                        else
                        {//是核心点
                            pointCloud_3.InsertPoint(count_3, points[i].X, points[i].Y, points[i].Z);
                            hulls[points[i].clusterId - 1].Add(new Point2D(points[i].X, points[i].Y));
                            count_3++;
                        }
                    }
                    //else if (type == 4)
                    //{
                    //    if (points[i].clusterId != 0 && (!points[i].isFilter))
                    //    {
                    //        pointCloud_1.InsertPoint(count_1, points[i].X, points[i].Y, points[i].Z);
                    //        count_1++;
                    //    }
                    //}
                    else if (type == 6) {
                        if (!points[i].isFilterByDistance)//未被过滤点
                        {
                            pointCloud_3.InsertPoint(count_3, points[i].X, points[i].Y, points[i].Z);//显示红点
                            count_3++;
                        }
                        else
                        {//被过滤点
                            pointCloud_2.InsertPoint(count_2, points[i].X, points[i].Y, points[i].Z);//显示绿点
                            count_2++;
                        }
                    }
                    else if (type == 7)
                    {
                        if (!points[i].isFilterByDistance)//被过滤点
                        {
                            pointCloud_1.InsertPoint(count_1, points[i].X, points[i].Y, points[i].Z);
                            count_1++;
                        }
                    }
                }
            }
            if (type == 1 || type == 3 || type == 7)
            {
                vtkPolyVertex polyVertex_1 = new vtkPolyVertex();
                polyVertex_1.GetPointIds().SetNumberOfIds(count_1);
                for (int i = 0; i < count_1; i++)
                {
                    polyVertex_1.GetPointIds().SetId(i, i);
                }
                vtkUnstructuredGrid grid_1 = new vtkUnstructuredGrid();
                grid_1.SetPoints(pointCloud_1);
                grid_1.InsertNextCell(polyVertex_1.GetCellType(), polyVertex_1.GetPointIds());

                vtkDataSetMapper map_1 = new vtkDataSetMapper();
                map_1.SetInput(grid_1);
                vtkActor actor_1 = new vtkActor();
                actor_1.SetMapper(map_1);
                if (type == 1)
                {
                    actor_1.GetProperty().SetPointSize(1.2f);
                }
                else if (type == 3)
                {
                    actor_1.GetProperty().SetPointSize(4.5f);
                    actor_1.GetProperty().SetColor(0.0, 0, 1.0);
                }
                else if (type == 7) {
                    actor_1.GetProperty().SetColor(0.0, 1, 0);
                }
                ren.AddActor(actor_1);
            }
            else if (type == 2||type ==6)
            {
                vtkPolyVertex polyVertex_2 = new vtkPolyVertex();
                vtkPolyVertex polyVertex_3 = new vtkPolyVertex();

                polyVertex_2.GetPointIds().SetNumberOfIds(count_2);
                polyVertex_3.GetPointIds().SetNumberOfIds(count_3);
                for (int i = 0; i < count_2; i++)
                {
                    polyVertex_2.GetPointIds().SetId(i, i);
                }
                for (int i = 0; i < count_3; i++)
                {
                    polyVertex_3.GetPointIds().SetId(i, i);
                }
                vtkUnstructuredGrid grid_2 = new vtkUnstructuredGrid();
                vtkUnstructuredGrid grid_3 = new vtkUnstructuredGrid();
                grid_2.SetPoints(pointCloud_2);
                grid_2.InsertNextCell(polyVertex_2.GetCellType(), polyVertex_2.GetPointIds());
                grid_3.SetPoints(pointCloud_3);
                grid_3.InsertNextCell(polyVertex_3.GetCellType(), polyVertex_3.GetPointIds());

                vtkDataSetMapper map_2 = new vtkDataSetMapper();
                map_2.SetInput(grid_2);
                vtkDataSetMapper map_3 = new vtkDataSetMapper();
                map_3.SetInput(grid_3);
                vtkActor actor_2 = new vtkActor();
                actor_2.SetMapper(map_2);
                actor_2.GetProperty().SetPointSize(1.5f);
                actor_2.GetProperty().SetColor(1.0, 0, 0);//红色
                ren.AddActor(actor_2);
                vtkActor actor_3 = new vtkActor();
                actor_3.SetMapper(map_3);
                actor_3.GetProperty().SetPointSize(1.2f);
                actor_3.GetProperty().SetColor(0, 1, 0);//绿色
                ren.AddActor(actor_3);
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
            switch (type)
            {   
                case 1:
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("当前点云个数： {0}", count_1);
                    break;
                case 2:
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("当前点云个数： {0},误差点:{1}，核心点：{2}", count_2 + count_3, count_2, count_3);
                    break;
                case 3:
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("当前质心个数： {0}", count_1);
                    break;
                case 6:
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("小于阈值点数：{0},大于阈值点数：{1}",count_3, count_2);
                    break;
                case 7:
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("未被过滤点云数： {0}", count_1);
                    break;
                default:
                    break;
            }
        }
       /// <summary>
       /// 显示外接圆 白色是原始图案 黄色是超过阈值图案
       /// </summary>
       /// <param name="ls">圆心点集列表</param>
        /// /// <param name="type">1.显示核心点和野点 2.显示过滤后的所有点</param>
        public void showCircle(List<Point2D> ls,int type, List<Point3D> li)//显示圆形图案
        { //输入为各圆心的List
            ren = new vtkRenderer();
            if (type == 1) ShowPointsFromFile(li, 2);//不同颜色显示野点和核心点
            else if (type == 2) ShowPointsFromFile(li, 1);
            ShowPointsFromFile(centers, 3);//显示质心
            vtkRegularPolygonSource polygonSource ;
            vtkPolyDataMapper mapper;
            vtkActor actor;
            for (int k = 0; k < ls.Count; k++)
            {
                if (ls[k].radius > 0.5)
                {
                    polygonSource = new vtkRegularPolygonSource();
                polygonSource.GeneratePolygonOff(); // Uncomment this line to generate only the outline of the circle
                polygonSource.SetNumberOfSides(100);
                polygonSource.SetRadius(ls[k].radius);
                polygonSource.SetCenter(ls[k].x, ls[k].y, centers[k].Z);
                // Visualize
                mapper = new vtkPolyDataMapper();
                mapper.SetInputConnection(polygonSource.GetOutputPort()); ;
                actor = new vtkActor();
                actor.SetMapper(mapper);
                actor.GetProperty().SetLineWidth(3);
                actor.GetProperty().SetOpacity(0.6);
//          if (ls[k].isFilter)
                if (type==2)
                {
                    if (filterID.Contains(ls[k].clusID))
                    {
                        actor.GetProperty().SetColor(1, 1, 0);
                    }
                }

                ren.AddActor(actor);
                }
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
        }
        private void showRectangle(List<Point2D[]> rtgPoints)//显示矩形图案
        {
            //foreach (Point2D[] pts in rtgPoints)
            ren = new vtkRenderer();
            ShowPointsFromFile(rawData, 2);
            ShowPointsFromFile(centers, 3);//不同颜色显示点M
            for (int i = 0; i < rtgPoints.Count; i++)
            {
                vtkPoints points = new vtkPoints();
                points.InsertNextPoint(rtgPoints[i][0].x, rtgPoints[i][0].y, centers[i].Z);
                points.InsertNextPoint(rtgPoints[i][1].x, rtgPoints[i][1].y, centers[i].Z);
                points.InsertNextPoint(rtgPoints[i][2].x, rtgPoints[i][2].y, centers[i].Z);
                points.InsertNextPoint(rtgPoints[i][3].x, rtgPoints[i][3].y, centers[i].Z);
                // Create the polygon
                vtkPolygon polygon = new vtkPolygon();
                polygon.GetPointIds().SetNumberOfIds(4); //make a quad
                polygon.GetPointIds().SetId(0, 0);
                polygon.GetPointIds().SetId(1, 1);
                polygon.GetPointIds().SetId(2, 2);
                polygon.GetPointIds().SetId(3, 3);

                // Add the polygon to a list of polygons
                vtkCellArray polygons = new vtkCellArray();
                polygons.InsertNextCell(polygon);

                // Create a PolyData
                vtkPolyData polygonPolyData = new vtkPolyData();
                polygonPolyData.SetPoints(points);
                polygonPolyData.SetPolys(polygons);

                // Create a mapper and actor
                vtkPolyDataMapper mapper = new vtkPolyDataMapper();
                mapper.SetInput(polygonPolyData);

                vtkActor actor = new vtkActor();
                actor.SetMapper(mapper);
                if (rtgPoints[i][0].isFilter)
                {
                    actor.GetProperty().SetEdgeColor(1, 1, 0);
                    actor.GetProperty().SetColor(1, 1, 0);
                }
                ren.AddActor(actor);
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
        }
        //计算聚类点质心（均值）

        /// <summary>
        /// 计算真值点与质心距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double getDisP(double[] p1, Point3D p2)//计算真值点与质心距离
        {
            double dx = p1[0] - p2.matched_X;
            double dy = p1[1] - p2.matched_Y;
            double dz = p1[2] - p2.matched_Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        void ICP()
        {
            vtkControl.GetRenderWindow().Clean();

            vtkPolyData SourcePolydata = ArrayList2PolyData(1);//对center处理
            vtkPolyData TargetPolydata = new vtkPolyData(); ;//对真值处理
            TargetPolydata.SetPoints(truePointCloud); //把点导入的polydata中去
            TargetPolydata.SetVerts(truePointVertices);
            vtkPolyData showtruePolydata = new vtkPolyData();
            showtruePolydata.SetPoints(visualizeTruePointCloud);
            showtruePolydata.SetVerts(vtpVertices);

            //开始用vtkIterativeClosestPointTransform类实现 ICP算法
            vtkIterativeClosestPointTransform icp = new vtkIterativeClosestPointTransform();
            icp.SetSource(SourcePolydata);
            icp.SetTarget(TargetPolydata);
            icp.GetLandmarkTransform().SetModeToRigidBody();
            icp.SetMaximumNumberOfIterations(100);
            icp.SetDebug(1);
            icp.StartByMatchingCentroidsOn();
            //icp.SetMeanDistanceModeToAbsoluteValue();
            icp.Modified();
            icp.Update();//执行ICP程序算法 较费时

            M = icp.GetMatrix();//获取变换矩阵

            Console.WriteLine("刚性变换矩阵为：" + M);
            vtkPolyData showLocPolydata = ArrayList2PolyData(2);
            vtkTransformPolyDataFilter icpTransformFilter = new vtkTransformPolyDataFilter();
            icpTransformFilter.SetInput(SourcePolydata);
            icpTransformFilter.SetTransform(icp);
            icpTransformFilter.Update();
            //设置源图形（质心点）
            vtkPolyDataMapper sourceMapper = new vtkPolyDataMapper();
            sourceMapper.SetInputConnection(SourcePolydata.GetProducerPort());
            vtkActor sourceActor = new vtkActor();
            sourceActor.SetMapper(sourceMapper);
            sourceActor.GetProperty().SetColor(1, 0, 0);
            sourceActor.GetProperty().SetPointSize(4);
            //设置辅助质心点（每次变化）
            //vtkPolyDataMapper showLocMapper = new vtkPolyDataMapper();
            //showLocMapper.SetInputConnection(showLocPolydata.GetProducerPort());
            //vtkActor showLocActor = new vtkActor();
            //showLocActor.SetMapper(showLocMapper);
            //showLocActor.GetProperty().SetColor(0, 0, 1);
            //showLocActor.GetProperty().SetPointSize(4);
            //设置目标图形（真值点）
            vtkPolyDataMapper targetMapper = new vtkPolyDataMapper();
            targetMapper.SetInputConnection(TargetPolydata.GetProducerPort());
            vtkActor targetActor = new vtkActor();
            targetActor.SetMapper(targetMapper);
            targetActor.GetProperty().SetColor(0, 1, 0);
            targetActor.GetProperty().SetPointSize(4);
            //设置辅助显示真值点
            //vtkPolyDataMapper showtrueMapper =new vtkPolyDataMapper();
            //showtrueMapper.SetInputConnection(showtruePolydata.GetProducerPort());
            //vtkActor showtrueActor =new vtkActor();
            //showtrueActor.SetMapper(showtrueMapper);
            //showtrueActor.GetProperty().SetColor(0, 1, 0);
            //showtrueActor.GetProperty().SetPointSize(4);

            vtkPolyDataMapper solutionMapper = new vtkPolyDataMapper();
            solutionMapper.SetInputConnection(icpTransformFilter.GetOutputPort());
            vtkActor solutionActor = new vtkActor();
            solutionActor.SetMapper(solutionMapper);
            solutionActor.GetProperty().SetColor(0, 0, 1);
            solutionActor.GetProperty().SetPointSize(3);

            ren = new vtkRenderer();

            //ren.AddActor(sourceActor);
            //ren.AddActor(showLocActor);
            ren.AddActor(targetActor);
            ren.AddActor(solutionActor);
            //ren.AddActor(showtrueActor);
            ren.SetBackground(0.3, 0.6, 0.3);
            // Render and interact
            vtkControl.GetRenderWindow().AddRenderer(ren);
            ren.Render();
            vtkControl.Refresh();
            //   renderWindow.Render();
            //  renderWindowInteractor.Start();

            SourcePolydata.FastDelete();
            TargetPolydata.FastDelete();
        }
        private vtkPolyData ArrayList2PolyData(int type)//ArrayList转成可视化PolyData
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
            else if (type == 2)
            {
                double param_y = 0;
                double param_x = 0;
                //x y z 质心的间距
                double x_dis = (trueScale[1] - trueScale[0]) * 0.5 - (centroidScale[1] * scale[0] - centroidScale[0] * scale[0]) * 0.5;
                double y_dis = (trueScale[3] - trueScale[2]) * 0.5 - (centroidScale[3] * scale[0] - centroidScale[2] * scale[0]) * 0.5;
                double z_dis = (trueScale[5] - trueScale[4]) * 0.5 - (centroidScale[5] * scale[0] - centroidScale[4] * scale[0]) * 0.5;
                if (clock == 0)
                {//y正方向朝上
                    param_y = 0.8;
                    if (clock_y == -1)
                    {
                        param_y = 0.4;
                    }
                }
                else if (clock == 6)
                {
                    param_y = 0.4;
                    if (clock_y == -1)
                    {
                        param_y = 0.8;
                    }
                }
                else if (clock == 9)
                {
                    param_y = 0.6;
                }
                else if (clock == 3)
                {
                    param_y = 0.6;
                }
                Console.WriteLine("时钟方向;" + clock + " " + clock_x + " " + clock_y);
                for (int j = 0; j < centers.Count; j++)
                {
                    pid[0] = SourcePoints.InsertNextPoint(
                        (centers[j].tmp_X + (trueScale[1] - trueScale[0]) * param_x),
                        (centers[j].tmp_Y + (trueScale[3] - trueScale[2]) * param_y),
                         visualizeTruePointCloud.GetPoint(0)[2]);
                    //truePointCloud.GetPoint(0)[2]);
                    SourceVertices.InsertNextCell(1, pid);
                }
            }
            polydata.SetPoints(SourcePoints); //把点导入的polydata中去
            polydata.SetVerts(SourceVertices);
            return polydata;
        }
        /// <summary>
        /// 添加扫描点聚类文件
        /// </summary>
        /// <param name="ptsPath">文件夹路径</param>
        /// <param name="xdir">扫描点x方向 1上2右3下4左</param>
        /// <param name="ydir">扫描点y方向 1上2右3下4左</param>
        /// <param name="typpe">1剔野+txt 2剔野+xls 3不剔野+txt 4不剔野+xls</param>
        /// <param name="isXLS">是否是xls文件</param>
        private void AddFolder(String ptsPath,int xdir,int ydir, int typpe, bool isXLS)//添加数据
        {
            string[] files = null; 
            if (isXLS)
            {
                files = Directory.GetFiles(ptsPath, "*.xls", SearchOption.AllDirectories);//搜索目录及子目录下所有的txt格式文件
            }
            else
            {
                files = Directory.GetFiles(ptsPath, "*.txt", SearchOption.AllDirectories);//搜索目录及子目录下所有的txt格式文件
            }
            if (files.Length == 0)
            {
                MessageBox.Show("未发现相应文件");
                return;
            }
            if (files.Length != 0)
            {
                Point3D point;
                //fixedData = new List<Point3D>[files.Length];
                grouping = new List<Point3D>[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    //fixedData[i] = new List<Point3D>();
                    grouping[i] = new List<Point3D>();
                }
                TreeNode treeDir = new TreeNode(ptsPath);
                root.Nodes.Add(treeDir);
                treeDir.Checked = true;
                double duplicatNum = 0;
                int pts = 0;
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    //treeDir.Nodes.Add(Path.GetFileName(file));
                    treeDir.Nodes.Add(file.Substring(ptsPath.Length + 1));
                    FileMap fileMap = new FileMap();
                    List<string> pointsList = null;
                    int line = 0;
                    ISheet sheet = null;
                    if (isXLS)
                    {
                        FileStream fs = File.OpenRead(file); //打开myxls.xls文件
                        HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                        sheet = wk.GetSheetAt(0);
                        line = sheet.LastRowNum + 1;
                    }
                    else
                    {
                        pointsList = fileMap.ReadFile(file);
                        line = pointsList.Count;
                    }

                    double fangweijiao, yangjiao;
                    if (isXLS) {
                        String s = sheet.GetRow(1).GetCell(0).ToString();
                        bit = s.Length - s.LastIndexOf("-1")-1;
                        Console.WriteLine("浮点位数为:" + bit);
                    } else {
                        String ssss =pointsList[0].Split('\t')[0];
                        bit = ssss.Length-ssss.LastIndexOf(".")-1;
                        Console.WriteLine("浮点位数为:" + bit);
                    }
                    IRow row;
                    string[] tmpxyz;
                    for (int i = 1; i < line; i++)
                    {
                        point = new Point3D();
                        if (isXLS)
                        {
                            row = sheet.GetRow(i);  //读取当前行数据
                            if (row != null)
                            {
                                point.motor_x = Convert.ToDouble(row.GetCell(0).ToString());
                                point.motor_y = Convert.ToDouble(row.GetCell(1).ToString());
                                point.Distance = Convert.ToDouble(row.GetCell(2).ToString());
                            }
                        }
                        else
                        {
                            tmpxyz = pointsList[i].Split('\t');
                            point.motor_x = Convert.ToDouble(tmpxyz[0]);//第一个字段
                            point.motor_y = Convert.ToDouble(tmpxyz[1]);//第二个字段
                            point.Distance = Convert.ToDouble(tmpxyz[2]);//第三个字段
                        }
                        if (point.Distance == 0 || point.Distance>666) continue;//D过大 首先排除掉
                        //OriginalData.Add(new DataPoint(Motor_X, Motor_Y, Distance));
                        //点的路径
                        point.pathId = pathList.Count;
                        //点是否显示
                        point.ifShown = true;
                        //point.isFilter = false;
                        point.isClassed = false;
                        point.clusterId = 0;
                        if (typpe == 3||typpe==4)
                        {
                            point.clusterId = point.pathId + 1;
                            point.pointName = file.Substring(ptsPath.Length + 1);
                        }
                        yangjiao = (-2) * (point.motor_x - this.x_angle) / 180 * Math.PI;
                        fangweijiao = 2 * (point.motor_y - this.y_angle) / 180 * Math.PI;
                        //yangjiao = (-2) * (point.motor_x - 45.439) / 180 * Math.PI;
                        //fangweijiao = 2 * (point.motor_y - 35.452) / 180 * Math.PI;

                        double tmpx = point.Distance * Math.Cos(yangjiao) * Math.Sin(fangweijiao);
                        double tmpy = point.Distance * Math.Sin(yangjiao) * Math.Cos(fangweijiao);

                        switch (xdir) { 
                            case 1:
                                point.X = tmpy;
                                break;
                            case 2:
                                point.X = tmpx;
                                break;
                            case 3:
                                point.X = -tmpy;
                                break;
                            case 4:
                                point.X = -tmpx;
                                break;
                        }
                        switch (ydir)
                        {
                            case 1:
                                point.Y = tmpy;
                                break;
                            case 2:
                                point.Y = tmpx;
                                break;
                            case 3:
                                point.Y = -tmpy;
                                break;
                            case 4:
                                point.Y = -tmpx;
                                break;
                        }

                        double tmpz = point.Distance * Math.Cos(yangjiao);
                        point.Z = tmpz;
                        if (typpe == 1)//清除重复-扫描点
                        {
                            List<Point3D> plist = rawData.FindAll(delegate(Point3D p) { return (p.X == tmpx) && (p.Y == tmpy) && (p.Z == tmpz); });
                            if (plist.Count == 0)rawData.Add(point);
                            else duplicatNum += 1;
                        }
                        else if (typpe == 3||typpe==4)//固定点-清除或不清除
                        {
                            if (grouping[pathList.Count].FindAll(delegate(Point3D p) { return (p.X == tmpx) && (p.Y == tmpy) && (p.Z == tmpz); }).Count == 0){
                                point.ptsCount = 1;
                                grouping[pathList.Count].Add(point);
                                pts++;
                            }      
                                
                            else
                            {
                                grouping[pathList.Count][grouping[pathList.Count].FindIndex(0, grouping[pathList.Count].Count, delegate(Point3D p) { return (p.X == tmpx) && (p.Y == tmpy) && (p.Z == tmpz); })].ptsCount += 1;
                                if (typpe == 3) duplicatNum++;
                                else pts++;
                            }
                        }
                        else if (typpe == 2 )//不清除重复-扫描点
                        {
                                rawData.Add(point);
                        }
                    }
                    pathList.Add(file);
                    //MessageBox.Show(pathList.Count + "   " + pathList[pathList.Count-1].ToString());
                };
                setChildNodeCheckedState(treeDir, true);
                root.Checked = true;
                treeView1.ExpandAll();
                if (typpe == 1)
                {
                    MessageBox.Show("共" + (rawData.Count + duplicatNum) + "个点，其中" + duplicatNum + "个重复点，剩余" + rawData.Count + "个点。","提示");
                    ShowPointsFromFile(rawData, 1);
                }
                else if (typpe == 2)
                {
                    MessageBox.Show("共" + rawData.Count + "个点。", "提示");
                    ShowPointsFromFile(rawData, 1);
                }
                else if (typpe == 3)
                {
                    MessageBox.Show("共" + (grouping.Length) + "个固定点，其中" + duplicatNum + "个重复点，剩余" + pts + "个数据点。", "提示");
                    showFixPointData(1);//显示完整数据
                }
                else if (typpe == 4)
                {
                    MessageBox.Show("共" + (grouping.Length) + "个固定点，" + pts + "个数据点。", "提示");
                    showFixPointData(1);//显示完整数据
                }
                SureDistanceFilter sdf = new SureDistanceFilter(typpe>2);//用typpe判断是否固定点
                sdf.Left = 0;
                if (typpe == 1 || typpe == 2) {
                    sdf.textBox_maxD.Text = rawData.Max(m => m.Distance).ToString();
                    sdf.textBox_minD.Text = rawData.Min(m => m.Distance).ToString();
                }
                else if (typpe == 3 || typpe == 4) {
                    sdf.textBox_maxD.Text = Tools.GetGroupingManOrMin(this.grouping,1).ToString();
                    sdf.textBox_minD.Text = Tools.GetGroupingManOrMin(this.grouping, 2).ToString();
                }
                sdf.Show(this);
            }


        }
        //执行dbscan的线程
        public void getClusterFromList(double tr,int pts)//执行dbscan聚类线程
        {
                this.threhold = tr;
                this.pointsInthrehold = pts;
                foreach (Point3D p in rawData) {
                    p.clusterId = 0;
                    p.isClassed = false;
                }
                progressForm.StartPosition = FormStartPosition.CenterParent;
                bkWorker.RunWorkerAsync();
                progressForm.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\loading.gif");
                progressForm.pictureBox1.Visible = true;
                progressForm.pictureBox1.Focus();
                progressForm.ShowDialog();
        }
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            // 事件处理，指定处理函数
            //dbb = new DB();
            dbb = new DBImproved();
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            dbb.dbscan(rawData, threhold, pointsInthrehold);
            MessageBox.Show("聚类运行时间："+stopwatch.Elapsed.ToString(),"消息");    
            clusterSum = dbb.clusterAmount;
            pointSum = dbb.pointsAmount;
            this.BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { });
            e.Result = ProcessProgress(bkWorker, e);
        }
        public void ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            // bkWorker.ReportProgress 会调用到这里，此处可以进行自定义报告方式  
            //progressForm.SetNotifyInfo(e.ProgressPercentage, "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%");  
        }
        public void CompleteWork(object sender, RunWorkerCompletedEventArgs e)
        {
            progressForm.Close();
            MessageBox.Show("处理完毕，总共" + rawData.Count + "个点云，生成" + dbb.clusterAmount + "个聚类", "提示");
            this.cp.Visible = true;
            this.cp.button1.Text = "重新聚类";
            this.cp.Left = 0;
        }
        private int ProcessProgress(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 1000; i++)
            {
                if (bkWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return -1;
                }
                else
                {
                    // 状态报告  
                    bkWorker.ReportProgress(i / 10);
                    // 等待，用于UI刷新界面，很重要  
                    System.Threading.Thread.Sleep(1);
                }
            }
            return -1;
        }
        public void DoWork2(object sender, DoWorkEventArgs e)
        {
            // 事件处理，指定处理函数
            ICP();
            this.BeginInvoke(new UpdateStatusDelegate(UpdateStatus2), new object[] { });

            e.Result = ProcessProgress(bkWorker, e);
        }
        public void ProgessChanged2(object sender, ProgressChangedEventArgs e)
        {
            // bkWorker.ReportProgress 会调用到这里，此处可以进行自定义报告方式  
            //progressForm.SetNotifyInfo(e.ProgressPercentage, "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%");  
        }
        public void CompleteWork2(object sender, RunWorkerCompletedEventArgs e)
        {
            progressForm.Close();
            MessageBox.Show("处理完毕,效果如图");
        }
        private int ProcessProgress2(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 1000; i++)
            {
                if (bkWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return -1;
                }
                else
                {
                    // 状态报告  
                    bkWorker.ReportProgress(i / 10);
                    // 等待，用于UI刷新界面，很重要  
                    System.Threading.Thread.Sleep(1);
                }
            }
            return -1;
        }
       /// <summary>
       /// 选择真值文件与聚类质心聚类
       /// </summary>
       /// <param name="transType"></param>
        void matchingPointCloud(int transType)//真值文件与质心文件聚类
        {
            if (truePointCloud == null)
            {
                truePointCloud = new vtkPoints();
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter += "点云数据(*.txt)|*.txt";
                openFile.Title = "打开文件";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    //trueLocArrayList = new ArrayList();
                    fullFilePath = openFile.FileName;
                    //获得文件路径
                    int index = fullFilePath.LastIndexOf("\\");
                    string filePath = fullFilePath.Substring(0, index);

                    //获得文件名称
                    string fileName = fullFilePath.Substring(index + 1);

                    FileMap fileMap = new FileMap();
                    List<string> pointsList = fileMap.ReadFile(fullFilePath);
                    truePolyVertex.GetPointIds().SetNumberOfIds(pointsList.Count);
                    for (int i = 0; i < pointsList.Count; i++)
                    {
                        string[] tmpxyz = pointsList[i].Split('\t');
                        double pX, pY, pZ;
                        if (!double.TryParse(tmpxyz[0], out pX))
                        {
                            MessageBox.Show("输入的文件格式有误，请重新输入");
                            return;
                        }
                        if (!double.TryParse(tmpxyz[1], out pY))
                        {
                            MessageBox.Show("输入的文件格式有误，请重新输入");
                            return;
                        }
                        if (!double.TryParse(tmpxyz[2], out pZ))
                        {
                            MessageBox.Show("输入的文件格式有误，请重新输入");
                            return;
                        }
                        truePointPid[0] = truePointCloud.InsertNextPoint(pX, pY, pZ);
                        truePointVertices.InsertNextCell(1, truePointPid);
                        //truePolyVertex.GetPointIds().SetId(i, i);
                    }
                    trueScale = truePointCloud.GetBounds();
                }
                /* 标准形式，基本完全匹配*/
            }
            if (centroidPointCloud == null)
            {
                centroidPointCloud = new vtkPoints();
                for (int i = 0; i < centers.Count; i++)
                {
                    centroidPointCloud.InsertPoint(i, centers[i].X, centers[i].Y, centers[i].Z);
                }
                centroidScale = centroidPointCloud.GetBounds();
                scale[0] = (truePointCloud.GetBounds()[1] - truePointCloud.GetBounds()[0])
                        / (centroidPointCloud.GetBounds()[1] - centroidPointCloud.GetBounds()[0]);
                scale[1] = (truePointCloud.GetBounds()[3] - truePointCloud.GetBounds()[2])
                        / (centroidPointCloud.GetBounds()[3] - centroidPointCloud.GetBounds()[2]);
                scale[2] = (truePointCloud.GetBounds()[5] - truePointCloud.GetBounds()[4])
                        / (centroidPointCloud.GetBounds()[5] - centroidPointCloud.GetBounds()[4]);
            }
            if (visualizeTruePointCloud == null)//辅助可视化真值点
            {
                visualizeTruePointCloud = new vtkPoints();
                for (int i = 0; i < truePointCloud.GetNumberOfPoints(); i++)
                {
                    vtpc_Pid[0] = visualizeTruePointCloud.InsertNextPoint(truePointCloud.GetPoint(i)[0] / 2 + (trueScale[1] - trueScale[0]) * 1.01,
                        (truePointCloud.GetPoint(i)[1] / 2 + (trueScale[3] - trueScale[2]) * 0.4), truePointCloud.GetPoint(i)[2]);
                    vtpVertices.InsertNextCell(1, vtpc_Pid);
                }
            }
            Console.WriteLine("质心计算范围比例x y z  " + scale[0] + "," + scale[1] + "," + scale[2]);
            //tmp是中间过渡坐标 若不进行中间转换 匹配效果奇差

            if (transType == 1)
            {//默认 只变换比例
                for (int j = 0; j < centers.Count; j++)
                {
                    centers[j].tmp_X = (centers[j].X + trueScale[0] - centroidScale[0]) * scale[0];
                    centers[j].tmp_Y = (centers[j].Y + trueScale[2] - centroidScale[2]) * scale[1];//* 0.98
                    centers[j].tmp_Z = (centers[j].Z + trueScale[4] - centroidScale[4]) * scale[2];
                }
            }
            else if (transType == 2)
            {//逆时针
                for (int j = 0; j < centers.Count; j++)
                {
                    double tmpx = centers[j].tmp_X;
                    double tmpy = centers[j].tmp_Y;
                    centers[j].tmp_X = -tmpy;//* 1.02
                    centers[j].tmp_Y = tmpx;
                }
            }
            else if (transType == 3)
            {//顺时针
                for (int j = 0; j < centers.Count; j++)
                {
                    double tmpx = centers[j].tmp_X;
                    double tmpy = centers[j].tmp_Y;
                    centers[j].tmp_X = tmpy;//* 1.02
                    centers[j].tmp_Y = -tmpx;
                }
            }
            else if (transType == 4)
            {//x镜面翻转
                for (int j = 0; j < centers.Count; j++)
                {
                    double tmpx = centers[j].tmp_X;
                    centers[j].tmp_X = -tmpx;
                }
            }
            else if (transType == 5)
            {//y镜面翻转
                for (int j = 0; j < centers.Count; j++)
                {
                    double tmpy = centers[j].tmp_Y;
                    centers[j].tmp_Y = -tmpy;
                }
            }
            progressForm.StartPosition = FormStartPosition.CenterParent;
            bkWorker2.RunWorkerAsync();
            progressForm.ShowDialog();
        }
        /// <summary>
        /// 导出匹配文件 仰角 方位角 距离 质心x y z
        /// </summary>
        void exportMatchingFile()//导出匹配文件
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile1.FileName, false);
                try
                {
                    int count = 0;
                    for (int j = 0; j < centers.Count; j++)//对所有刚性变换后的中心迭代
                    {
                        if (centers[j].isMatched)
                        {//若已被匹配
                            count++;
                            for (int i = 0; i < rawData.Count; i++)
                            {
                                if (centers[j].clusterId == rawData[i].clusterId)
                                {
                                    sw.WriteLine(count + "\t" +
                                        (-2) * (rawData[i].motor_x - x_angle) / 180 * Math.PI + "\t" +//需求要求导出仰角 方位角和距离
                                         2 * (rawData[i].motor_y - y_angle) / 180 * Math.PI + "\t" +
                                         rawData[i].Distance + "\t" +
                                    truePointCloud.GetPoint(centers[j].matchNum)[0] + "\t" +
                                    truePointCloud.GetPoint(centers[j].matchNum)[1] + "\t" +
                                     truePointCloud.GetPoint(centers[j].matchNum)[2]);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    sw.Close();
                }
            }
        }
        private void CallBack(string message)//回调函数
        {
            //主线程报告信息,可以根据这个信息做判断操作,执行不同逻辑.
            //MessageBox.Show(message);
            if (message.Equals("finish"))
            {

            }
        }
        void clearData()//清空所有数据
        {
            rawData.Clear();
            if (centers != null)
                centers.Clear();
            if (truePointCloud != null)
                truePointCloud = null;
            if (visualizeTruePointCloud != null) {
                visualizeTruePointCloud = null;
            }
            pathList.Clear();
            truePointPid = new int[1];
            grouping = null;
            //if(trueLocArrayList!=null)
            //    trueLocArrayList.Clear();
            root.Nodes.Clear();
            ren = new vtkRenderer();
            vtkControl.GetRenderWindow().Clean();
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
            vtkControl.Update();
        }
        
        /// <summary>
        /// 初步聚类中清除聚类过小的 合并距离相近的
        /// </summary>
        void clearError_combineClusters_FromScanList()//清楚野点 合并聚类接近点
        {
            for (int i = 0; i < grouping.Length; i++)//第一层 遍历所有聚类ID
            {
                Console.WriteLine(grouping[i].Count);
                for (int j = 0; j < grouping[i].Count; j++)//第二层
                {
                    int c = 0;
                    for (int k = 0; k < grouping[i].Count; k++)
                    {
                        if (DB.getDisP(grouping[i][j], grouping[i][k]) < 0.06) /////两个聚类之间距离过小 该数数值需要更改
                        {
                            c++;
                        }
                    }
                    if (c < 9)//聚类过小 设为野点
                    {
                        grouping[i][j].clusterId = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 显示固定点数据
        /// </summary>
        /// <param name="type">1.显示全部 2.显示阈值内和野点 3.显示质心和正常点 4.只显示阈值内点</param>
        public void showFixPointData(int type)//显示固定点数据
        {
            vtkControl.GetRenderWindow().Clean();

            if (type == 1 || type == 2 || type ==4)
            {
                ren = new vtkRenderer();
            }
            vtkPoints pointCloud_1 = new vtkPoints();
            vtkPoints pointCloud_2 = new vtkPoints();
            vtkPoints pointCloud_3 = new vtkPoints();
            vtkPolyVertex polyVertex_1 = new vtkPolyVertex();
            vtkPolyVertex polyVertex_2 = new vtkPolyVertex();
            vtkUnstructuredGrid grid_1 = new vtkUnstructuredGrid();
            vtkUnstructuredGrid grid_2 = new vtkUnstructuredGrid();
            vtkDataSetMapper map_1 = new vtkDataSetMapper();
            vtkDataSetMapper map_2 = new vtkDataSetMapper();
            vtkActor actor_1 = new vtkActor();
            vtkActor actor_2 = new vtkActor();

            int count_1 = 0, count_2 = 0; 
                for (int i = 0; i < grouping.Length; i++)
                {
                    for (int j = 0; j < grouping[i].Count; j++)
                    {
                        if (type == 1 || type ==3)
                        {
                            pointCloud_1.InsertPoint(count_1, grouping[i][j].X, grouping[i][j].Y, grouping[i][j].Z);
                            count_1++;
                        }
                        else if ((type == 4) && (!grouping[i][j].isFilterByDistance))
                        {
                            pointCloud_1.InsertPoint(count_1, grouping[i][j].X, grouping[i][j].Y, grouping[i][j].Z);
                            count_1++;
                        }
                        else if (type == 2)
                        {
                            if (grouping[i][j].isFilterByDistance)
                            {
                                pointCloud_1.InsertPoint(count_1, grouping[i][j].X, grouping[i][j].Y, grouping[i][j].Z);
                                count_1++;
                            }
                            else
                            {
                                pointCloud_2.InsertPoint(count_2, grouping[i][j].X, grouping[i][j].Y, grouping[i][j].Z);
                                count_2++;
                            }
                        }

                    }
                }
                if (type == 3)
                {
                    foreach (Point3D p in centers)
                    {
                        pointCloud_2.InsertPoint(count_2, p.X, p.Y, p.Z);
                                count_2++;
                    }
                }
                polyVertex_1.GetPointIds().SetNumberOfIds(count_1);
                if(count_2!=0)  polyVertex_2.GetPointIds().SetNumberOfIds(count_2);

                for (int i = 0; i < count_1; i++)
                {
                    polyVertex_1.GetPointIds().SetId(i, i);
                }
                for (int i = 0; i < count_2; i++)
                {
                    polyVertex_2.GetPointIds().SetId(i, i);
                }

                grid_1.SetPoints(pointCloud_1);
                grid_1.InsertNextCell(polyVertex_1.GetCellType(), polyVertex_1.GetPointIds());
                map_1.SetInput(grid_1);
                actor_1.SetMapper(map_1);
                if (type == 1 || type ==4)
                {
                    if (type == 1)
                    {
                        actor_1.GetProperty().SetPointSize(1.5f);
                        actor_1.GetProperty().SetColor(1.0, 1.0, 1.0);
                    }
                    else
                    {
                        actor_1.GetProperty().SetPointSize(2f);
                        actor_1.GetProperty().SetColor(0, 1.0, 0);
                    }
                    ren.AddActor(actor_1);
                }
                else if (type==2 || type ==3)
                {

                    grid_2.SetPoints(pointCloud_2);
                    grid_2.InsertNextCell(polyVertex_2.GetCellType(), polyVertex_2.GetPointIds());
  
                    map_2.SetInput(grid_2);
                    actor_2.SetMapper(map_2);
                    if (type == 2)
                    {
                        actor_1.GetProperty().SetPointSize(2f);
                        actor_1.GetProperty().SetColor(1.0, 0, 0);
                        
                        actor_2.GetProperty().SetPointSize(2f);
                        actor_2.GetProperty().SetColor(0, 1, 0);
                    }else if (type ==3)
	                {
		                actor_1.GetProperty().SetPointSize(2f);
                        actor_1.GetProperty().SetColor(1.0, 1.0, 1.0); 
                        actor_2.GetProperty().SetPointSize(6f);
                        actor_2.GetProperty().SetColor(0, 0, 1);
	                }
                    ren.AddActor(actor_1);
                    ren.AddActor(actor_2);
                }
                vtkControl.GetRenderWindow().AddRenderer(ren);
                vtkControl.Refresh();
            
        }

        /// <summary>
        /// 处理外接圆和外接矩形
        /// </summary>
        public void dealwithMCCandMCE()//处理外接圆和外接矩形
        {  
            MCC mcc = new MCC();
            mcc.Left = 0;
            mcc.Show(this);
            this.toolStripStatusLabelCurrentPointCount.Text = String.Format("当前点云数：{0}，当前聚类数： {1}", pointSum, clusterSum);
        }
        /// <summary>
        /// 以外接圆半径过滤聚类点集-通过MCC调用
        /// </summary>
        /// <param name="radius"></param>
        public void FilterClustersByRadius(double radius) {
            filterID.Clear();
            for (int j = 0; j < clusterSum; j++)
            {
                if (circles[j].radius > radius)
                {
                    //circles[j].isFilter = true;
                    filterID.Add(circles[j].clusID);
                }
            }
            this.toolStripStatusLabel2.Text = "超过阈值半径聚类数：" + filterID.Count; ;
            showCircle(circles,2,rawData);
        }
        /// <summary>
        /// 确认源文件聚类结果
        /// </summary>
        void sureSourceClustringandExprot()//确认源文件聚类结果
        {
            SureSourceFrm SSF = new SureSourceFrm();
            DialogResult ssfDr = SSF.ShowDialog();
            if (ssfDr == DialogResult.OK)
            {
                string strCentroid = SSF.strCentroid;
                string strMatching = SSF.strMatching;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(strCentroid, false);
                try{

                    int ccc = 1;
                    for (int j = 0; j < rawData.Count; j++)
                    {
                        if (rawData[j].clusterId != 0)//&& (!rawData[j].isFilter)
                        {
                            sw.WriteLine(ccc + "\t" + rawData[j].motor_x + "\t" + rawData[j].motor_y + "\t" + rawData[j].Distance);
                        }
                        ccc++;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    sw.Close();
                }
                System.IO.StreamWriter ssw = new System.IO.StreamWriter(strCentroid, false);
                try
                {
                    int abc = 0;
                    foreach (List<Point3D> pll in classedrawData)
                    {
                        if (pll.Equals(classedrawData[0])) continue;
                        foreach (Point3D pps in pll)
                        {
                            ssw.WriteLine(abc + "\t" +
                                (-2) * (pps.motor_x - x_angle) / 180 * Math.PI + "\t" +//需求要求导出仰角 方位角和距离
                                 2 * (pps.motor_y - y_angle) / 180 * Math.PI + "\t" +
                                 pps.Distance + "\t" + sourceTrueList[pps.clusterId].X + "\t" +
                                 sourceTrueList[pps.clusterId].Y + "\t" +
                                 sourceTrueList[pps.clusterId].Z + "\t");
                        }
                        abc++;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    ssw.Close();
                }
            }
            else if (ssfDr == DialogResult.Cancel)
            {
                ShowPointsFromFile(rawData, 1);
                return;
            }
            else if (ssfDr == DialogResult.Yes)
            {
                int sspp = showSourcePoint();

                while (sspp == 2)
                {
                    sspp = showSourcePoint();
                }
                if (sspp == 3)
                {
                    ShowPointsFromFile(rawData, 1);
                    return;
                }
                doingSourceClustring();
                sureSourceClustringandExprot();
                return;
            }
        }
        /// <summary>
        /// 进行源文件聚类
        /// </summary>
        void doingSourceClustring()//执行源文件聚类
        {
                List<Point3D> p_erro = new List<Point3D>();
                classedrawData.Add(p_erro);
                int sss = 0;
                double[] nearScale = Tools.calBounds(showPts);
                simple_x_step = (nearScale[1] - nearScale[0]) / Math.Sqrt(showPts.Count);
                simple_y_step = (nearScale[3] - nearScale[2]) / Math.Sqrt(showPts.Count);
                foreach (Point3D pd in showPts)
                {
                    List<Point3D> certainClusterPoints = new List<Point3D>();
                    List<Point3D> plist = rawData.FindAll(delegate(Point3D p)
                    {
                        return (Math.Abs(p.X - pd.X) + Math.Abs(p.Y - pd.Y))
                            < ((simple_x_step + simple_y_step) / 4);
                    });
                    if (plist.Count > 8)
                    {
                        foreach (Point3D px in plist)
                        {
                            //rawData[rawData.IndexOf(pd)].clusterId = clusterSum;
                            certainClusterPoints.Add(px);
                        }
                        classedrawData.Add(certainClusterPoints);
                        clusterSum++;
                        sss += plist.Count;
                    }
                }
                Console.WriteLine("已加入点集：" + sss);

                foreach (Point3D ppp in rawData)
                {
                    bool flag = false;
                    foreach (List<Point3D> ll in classedrawData)
                    {
                        if (ll.Contains(ppp)) { flag = true; break; }
                    }
                    if (!flag)
                    {
                        ppp.clusterId = 0;
                        classedrawData[0].Add(ppp);
                    }
                }
                Console.WriteLine("未分类数目为：" + classedrawData[0].Count);
                centers = new List<Point3D>();
                for (int i = 1; i < classedrawData.Count; i++)
                {
                    Point3D cen = new Point3D(0, 0, 0);
                    foreach (Point3D pds in classedrawData[i])
                    {
                        cen.X += pds.X;
                        cen.Y += pds.Y;
                        cen.Z += pds.Z;
                    }
                    cen.X = cen.X / classedrawData[i].Count;
                    cen.Y = cen.Y / classedrawData[i].Count;
                    cen.Z = cen.Z / classedrawData[i].Count;
                    centers.Add(cen);
                }
                Console.WriteLine("当前聚类数目为：" + centers.Count);
                List<Point3D> movedPts = new List<Point3D>();
                foreach (Point3D ps in classedrawData[0])
                {
                    for (int i = 1; i < centers.Count + 1; i++)
                    {
                        double dis = Math.Abs(ps.X - centers[i - 1].X) + Math.Abs(ps.Y - centers[i - 1].Y);
                        if (dis < (simple_x_step + simple_y_step) / 5)
                        {
                            classedrawData[i].Add(ps);
                            movedPts.Add(ps);
                        }
                    }
                }
                foreach (Point3D pp in movedPts)
                {
                    classedrawData[0].Remove(pp);
                }
                Console.WriteLine("未分类点变为：" + classedrawData[0].Count);

                List<int> tmp_id = new List<int>();
                List<List<Point3D>> lsss = new List<List<Point3D>>();
                for (int k = 1; k < centers.Count; k++)
                {
                    for (int t = centers.Count - 1; t != k; t--)
                    {
                        foreach (Point3D pp in classedrawData[k])
                        {
                            pp.clusterId = k;
                        }
                        double dis = DB.getDisP(centers[k], centers[t]);
                        if (dis < (simple_x_step + simple_y_step) / 4)
                        {
                            lsss.Add(classedrawData[t]);
                            foreach (Point3D pt in classedrawData[t])
                            {
                                pt.clusterId = k;
                                classedrawData[k].Add(pt);
                            }
                        }
                    }
                }
                foreach (List<Point3D> i in lsss)
                {
                    classedrawData.Remove(i);
                }
                Console.WriteLine("剩余聚类数：" + (classedrawData.Count - 1));
                centers = new List<Point3D>();
                rawData = new List<Point3D>();
                for (int i = 0; i < classedrawData.Count; i++)
                {
                    if (i == 0)
                    {
                        foreach (Point3D pds in classedrawData[i])
                        {
                            pds.clusterId = 0;
                            pds.ifShown = true;
                            pds.isKeyPoint = false;
                            rawData.Add(pds);
                        }
                    }
                    else
                    {
                        Point3D cen = new Point3D(0, 0, 0);
                        foreach (Point3D pds in classedrawData[i])
                        {
                            cen.X += pds.X;
                            cen.Y += pds.Y;
                            cen.Z += pds.Z;
                            cen.clusterId = i + 1;
                            cen.ifShown = true;
                            pds.clusterId = i;
                            pds.ifShown = true;
                            pds.isKeyPoint = true;
                            rawData.Add(pds);
                        }
                        cen.X = cen.X / classedrawData[i].Count;
                        cen.Y = cen.Y / classedrawData[i].Count;
                        cen.Z = cen.Z / classedrawData[i].Count;
                        cen.isMatched = true;
                        centers.Add(cen);
                    }
                }
                ShowPointsFromFile(rawData, 2);
                ShowPointsFromFile(centers, 3);//不同颜色显示点

                vtkControl.Refresh();
        }
        /// <summary>
        /// 源文件聚类中 显示源点
        /// </summary>
        private int showSourcePoint()//源文件聚类显示源数据
        {
            ShowSourceClustering ssc = new ShowSourceClustering();
            ssc.xaxialsymmetry.Checked = this.true_xasix;
            ssc.yaxialsymmetry.Checked = this.true_yasix;
            ssc.xmove.Text = this.true_xshift.ToString();
            ssc.ymove.Text = this.true_yshift.ToString();
            ssc.scale.Text = this.true_scale.ToString();
            if (true_rotationRb == 0) ssc.noRotationRb.Checked = true;
            else if (true_rotationRb == 1) ssc.Rotate90Rb.Checked = true;
            else if (true_rotationRb == 2) ssc.Rotate180Rb.Checked = true;
            else if (true_rotationRb == 3) ssc.Rotate270Rb.Checked = true;
            DialogResult drs = ssc.ShowDialog();
            if (drs == DialogResult.OK)
            {
                return 1; 
            }
            else if (drs == DialogResult.Cancel)
            {
                return 3;
            }
            else if (drs == DialogResult.Yes)
            {
                this.true_rotationRb = ssc.rotationRb;
                this.true_xshift = ssc.xsift;
                this.true_yshift = ssc.ysift;
                this.true_noTrans = ssc.noTrans;
                this.true_xasix = ssc.isXTrans;
                this.true_yasix = ssc.isYTrans;
                this.true_scale = ssc.ss;
            }
            showPts = new List<Point3D>();
            foreach (Point3D truePts in transPts)
            {
                double tmpx = truePts.X;
                double tmpy = truePts.Y;
                double tmp;
                if (true_rotationRb == 1)
                {
                    tmp = tmpx;
                    tmpx = tmpy;
                    tmpy = -tmp;
                }
                else if (true_rotationRb == 2)
                {
                    tmpx = -tmpx;
                    tmpy = -tmpy;
                }
                else if (true_rotationRb == 3)
                {
                    tmp = tmpy;
                    tmpy = tmpx;
                    tmpx = -tmp;
                }
                if (true_xasix)
                {
                    tmpx = -tmpx;
                }
                if (true_yasix)
                {
                    tmpy = -tmpy;
                }
                tmpx = tmpx * true_scale + true_xshift;
                tmpy = tmpy * true_scale + true_yshift;
                Point3D ppp = new Point3D(tmpx, tmpy, truePts.Z);
                ppp.ifShown = true;
                showPts.Add(ppp);
            }
            ShowPointsFromFile(rawData, 1);
            ShowPointsFromFile(showPts, 3);
            vtkControl.Refresh();
            return 2;
        }
        /// <summary>
        /// 查看真值点函数
        /// </summary>
        private void seeTruePointFromFile() //查看真值点
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter += "点云数据(*.txt)|*.txt";
            openFile.Title = "打开文件";
            rawData = new List<Point3D>();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //trueLocArrayList = new ArrayList();
                fullFilePath = openFile.FileName;
                //获得文件路径
                int index = fullFilePath.LastIndexOf("\\");
                string filePath = fullFilePath.Substring(0, index);

                //获得文件名称
                string fileName = fullFilePath.Substring(index + 1);

                FileMap fileMap = new FileMap();
                List<string> pointsList = fileMap.ReadFile(fullFilePath);
                Point3D ppp ;
                for (int i = 0; i < pointsList.Count; i++)
                {
                    string[] tmpxyz = pointsList[i].Split('\t');
                    double pX, pY, pZ;
                    if (!double.TryParse(tmpxyz[0], out pX))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    if (!double.TryParse(tmpxyz[1], out pY))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    if (!double.TryParse(tmpxyz[2], out pZ))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    ppp = new Point3D();
                    ppp.X = pX;
                    ppp.Y = pY;
                    ppp.Z = pZ;
                    ppp.ifShown = true;
                    rawData.Add(ppp);
                }
                ShowPointsFromFile(rawData, 1);
            }
        }       





        //////工具栏右侧隐藏按钮事件
        /// <summary>
        /// 确认自动匹配效果单击事件
        /// </summary>
        private void SureAutoMatchBtn_Click(object sender, EventArgs e)
        {
            matchedID.Clear();
            MatchingParams mp = new MatchingParams();
            DialogResult rs = mp.ShowDialog();
            double mpthrehold = 0.0;
            if (rs == DialogResult.OK)
            {
                mpthrehold = mp.matchingParams;
                int countMatched = 0;
                for (int j = 0; j < centers.Count; j++)
                {
                    int matchedId = 0;
                    //这里改正了centers的真值  因为不需要输出 修改为通过矩阵变换和真值接近的点
                    centers[j].matched_X = centers[j].tmp_X * M.GetElement(0, 0)
                        + centers[j].tmp_Y * M.GetElement(0, 1)
                        + centers[j].tmp_Z * M.GetElement(0, 2) + M.GetElement(0, 3);
                    centers[j].matched_Y = centers[j].tmp_X * M.GetElement(1, 0)
                        + centers[j].tmp_Y * M.GetElement(1, 1)
                        + centers[j].tmp_Z * M.GetElement(1, 2) + M.GetElement(1, 3);
                    centers[j].matched_Z = centers[j].tmp_X * M.GetElement(2, 0)
                        + centers[j].tmp_Y * M.GetElement(2, 1)
                        + centers[j].tmp_Z * M.GetElement(2, 2) + M.GetElement(2, 3);
                    centers[j].isMatched = false;
                    double center2True = getDisP(truePointCloud.GetPoint(0), centers[j]);//设一个距离初值
                    for (int i = 0; i < truePointCloud.GetNumberOfPoints(); i++)
                    {
                        double ddd = getDisP(truePointCloud.GetPoint(i), centers[j]);
                        if (ddd < center2True)
                        {
                            center2True = ddd;
                            matchedId = i;
                        }
                    }
                    if (center2True < mpthrehold)
                    {

                        ((Point3D)centers[j]).isMatched = true;
                        ((Point3D)centers[j]).matchNum = matchedId;
                        matchedID.Add(matchedId);
                        countMatched++;
                    }
                }
                MessageBox.Show("总共" + centers.Count + "个聚类质心，总共" + truePointCloud.GetNumberOfPoints() + "个真值点，匹配" + countMatched + "个点", "提示");
                showMatchedLine();
                this.XAxisChangeBtn.Visible = false;
                this.YAxisChangeBtn.Visible = false;
                this.SureAutoMatchBtn.Visible = false;
                this.ClockWiseBtn.Visible = false;
                this.AntiClockWiseBtn.Visible = false;
                this.SureMatchBtn.Visible = true;
                this.RecorrectMatch.Visible = true;
            }
            else if (rs == DialogResult.Cancel)
            {
                return;
            }
            //this.ExportMatchToolStripMenuItem.Enabled = true;
            this.SureMatchBtn.Enabled = true;
            this.RecorrectMatch.Enabled = true;
        }
        /// <summary>
        ///确认匹配结果单击事件
        /// </summary>
        private void SureMatchBtn_Click(object sender, EventArgs e)
        {
            exportMatchingFile();
        }
        /// <summary>
        ///重新修正匹配结果单击事件
        /// </summary>
        private void RecorrectMatch_Click(object sender, EventArgs e)
        {
            matchedID.Clear();
            MatchingParams mp = new MatchingParams();
            DialogResult rs = mp.ShowDialog();
            double mpthrehold = 0.0;
            if (rs == DialogResult.OK)
            {
                mpthrehold = mp.matchingParams;
                int countMatched = 0;
                for (int j = 0; j < centers.Count; j++)
                {
                    int matchedId = 0;
                    centers[j].isMatched = false;
                    double center2True = getDisP(truePointCloud.GetPoint(0), centers[j]);//设一个距离初值
                    for (int i = 0; i < truePointCloud.GetNumberOfPoints(); i++)
                    {
                        double ddd = getDisP(truePointCloud.GetPoint(i), centers[j]);
                        if (ddd < center2True)
                        {
                            center2True = ddd;
                            matchedId = i;
                        }
                    }
                    if (center2True < mpthrehold)
                    {

                        ((Point3D)centers[j]).isMatched = true;
                        ((Point3D)centers[j]).matchNum = matchedId;
                        matchedID.Add(matchedId);
                        countMatched++;
                    }
                }
                MessageBox.Show("总共" + centers.Count + "个聚类质心，总共" + truePointCloud.GetNumberOfPoints() + "个真值点，匹配" + countMatched + "个点", "提示");
                showMatchedLine();
            }
        }
        /// <summary>
        /// 确认按聚类半径过滤button的单击事件
        /// </summary>
   
        private void ClockWiseBtn_Click(object sender, EventArgs e)//顺时针
        {
            if (clock == 0) clock = 3;
            else if (clock == 3) clock = 6;
            else if (clock == 6) clock = 9;
            else if (clock == 9) clock = 0;
            matchingPointCloud(3);

        }
        private void AntiClockWiseBtn_Click(object sender, EventArgs e)//逆时针
        {
            if (clock == 0) clock = 9;
            else if (clock == 3) clock = 0;
            else if (clock == 6) clock = 3;
            else if (clock == 9) clock = 6;
            matchingPointCloud(2);
        }
        private void XAxisChangeBtn_Click(object sender, EventArgs e)//绕x翻转
        {
            if (clock_x == 1) clock_x = -1;
            else if (clock_x == -1) clock_x = 1;
            matchingPointCloud(4);
        }
        private void YAxisChangeBtn_Click(object sender, EventArgs e)//绕y翻转
        {
            if (clock_y == 1) clock_y = -1;
            else if (clock_y == -1) clock_y = 1;
            matchingPointCloud(5);
        }
        /// <summary>
        /// 设置图例是否可见
        /// </summary>
        /// <param name="Visible">0.全部不可见 1.Distan过滤标注</param>
        public void isShowLegend(int Visible)//是否显示图例
        {
            if (Visible == 0) {//全都不显示
                this.pictureBox1.Visible = false;
                this.label1.Visible = false;
                this.pictureBox2.Visible = false;
                this.label2.Visible = false;
                this.pictureBox3.Visible = false;
                this.label3.Visible = false;
                this.pictureBox4.Visible = false;
                this.label4.Visible = false;
                return;
            }
                else if (Visible == 1) {//按阈值过滤的图例
                    this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\red_point.png");
                    this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                    //this.pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png"); 
                    label1.Text = "被过滤点";
                    label2.Text = "未过滤点";
                }
                else if (Visible == 2) {//聚类后显示红绿点图例+外接圆
                    this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                    this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\red_point.png");
                    this.pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png");
                    this.pictureBox4.Image = Image.FromFile(Application.StartupPath + "\\white_circle.png");
                    this.pictureBox4.Location = new Point(this.label3.Location.X + this.label3.Width +20, this.pictureBox3.Location.Y);
                    this.label4.Location = new Point(this.pictureBox4.Location.X + this.pictureBox4.Width - 5, this.pictureBox4.Location.Y + 10);
                    label1.Text = "核心点";
                    label2.Text = "野点";
                    label3.Text = "聚类质心";
                }
                else if (Visible == 3) {//显示白色外接圆图例
                    this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\white_circle.png");
                    label1.Text = "聚类外接圆";
                    label1.Location = new Point(this.pictureBox1.Location.X + this.pictureBox1.Width - 5, this.pictureBox1.Location.Y + 10);
                }
                else if (Visible == 4) {//显示超过半径阈值和小于半径阈值的圆的图例
                    this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\white_circle.png");
                    label1.Text = "阈值半径"+"\n"+"内点集";
                    label1.Location = new Point(this.pictureBox1.Location.X+this.pictureBox1.Width-20, this.pictureBox1.Location.Y+10);                    
                    this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\yellow_circle.png");
                    this.pictureBox2.Location = new Point(this.label1.Location.X + this.label1.Width -10, this.pictureBox1.Location.Y);
                    label2.Text = "阈值半径"+"\n"+"外点集";
                    label2.Location = new Point(this.pictureBox2.Location.X + this.pictureBox2.Width - 20, this.pictureBox2.Location.Y + 10);
                    this.label2.Visible = true;
                    this.pictureBox2.Visible = true;
                }
            else if (Visible ==5)//显示固定点所有点和质心
            {
                    this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\white_point.png");
                    this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png");
                    label1.Text = "数据点";
                    label2.Text = "质心";
            }
            this.label1.Visible = true;//默认显示一组
            this.pictureBox1.Visible = true;
            if (Visible == 1 || Visible==4 || Visible == 5)//显示两个
            {
                this.label2.Visible = true;
                this.pictureBox2.Visible = true;
            }
            else if (Visible == 2) {//显示4个
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.label4.Visible = true;
                this.pictureBox2.Visible = true;
                this.pictureBox3.Visible = true;
                this.pictureBox4.Visible = true;
            }
        }


        //////菜单栏项目单击事件

        /// <summary>
        /// 导入txt文件夹
        /// </summary>
        private void ImporttxtFileToolStripMenuItem_Click(object sender, EventArgs e)//导入txt文件夹git
        {
            ImportPts ip = new ImportPts();
            DialogResult rs = ip.ShowDialog();
            if (rs == DialogResult.OK)
            {
                //int ImportPtsRadioBox = ip.ptsRb;
                this.selPath = ip.selPath;
                this.x_angle = ip.x_angle;
                this.y_angle = ip.y_angle;
                this.isIgnoreDuplication = ip.isIgnoreDuplication;//=1清除 =2不清除    
                this.AddFolder(selPath, ip.xdir, ip.ydir, (this.isIgnoreDuplication) ? 1 : 2, false);
                this.ScanClustringToolStripMenuItem.Enabled = true;
            }
            else if (rs == DialogResult.Cancel)
            {
                return;
            }
        }
        /// <summary>
        /// 导入xls文件夹
        /// </summary>
        private void ImportXLSFileToolStripMenuItem_Click(object sender, EventArgs e)//导入xls文件夹
        {
            ImportPts ip = new ImportPts();
            DialogResult rs = ip.ShowDialog();
            if (rs == DialogResult.OK)
            {
                this.selPath = ip.selPath;
                this.x_angle = ip.x_angle;
                this.y_angle = ip.y_angle;
                this.isIgnoreDuplication = ip.isIgnoreDuplication;//=1清除 =2不清除    
                this.AddFolder(selPath, ip.xdir, ip.ydir, (this.isIgnoreDuplication) ? 1 : 2, true);
                this.ScanClustringToolStripMenuItem.Enabled = true;
            }
            else if (rs == DialogResult.Cancel)
            {
                return;
            }
        }
        /// <summary>
        /// DBSCAN算法聚类单击事件
        /// </summary>
        private void ExplainClusteringToolStripMenuItem_Click(object sender, EventArgs e)//dbscan算法聚类
        {
            if (rawData.Count == 0)
            {
                MessageBox.Show("没有数据,不可以聚类");
                return;
            }
            if (GetTreeViewNodeChecked(treeView1) == 0)
            {
                MessageBox.Show("没有显示任何点，不可以聚类");
                return;
            }
            cp = new ClusterParameters();
            cp.Show(this);
            //调用显示
            //this.ExportClusterToolStripMenuItem.Enabled = true;
        }
        /// <summary>
        /// 源文件聚类菜单单击事件
        /// </summary>
        private void SourceClusteringToolStripMenuItem_Click(object sender, EventArgs e)//源文件聚类
        {
            if (rawData.Count == 0)
            {
                MessageBox.Show("没有数据,不可以聚类");
                return;
            }
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter += "点云数据(*.txt)|*.txt";
            openFile.Title = "打开文件";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                sourceTrueList = new List<Point3D>();
                fullFilePath = openFile.FileName;
                //获得文件路径
                int index = fullFilePath.LastIndexOf("\\");
                string filePath = fullFilePath.Substring(0, index);

                //获得文件名称
                string fileName = fullFilePath.Substring(index + 1);

                FileMap fileMap = new FileMap();
                List<string> pointsList = fileMap.ReadFile(fullFilePath);
                truePolyVertex.GetPointIds().SetNumberOfIds(pointsList.Count);
                for (int i = 0; i < pointsList.Count; i++)
                {
                    string[] tmpxyz = pointsList[i].Split('\t');
                    double pX, pY, pZ;
                    if (!double.TryParse(tmpxyz[0], out pX))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    if (!double.TryParse(tmpxyz[1], out pY))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    if (!double.TryParse(tmpxyz[2], out pZ))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    Point3D pu = new Point3D(pX, pY, pZ);
                    pu.ifShown = true;
                    sourceTrueList.Add(pu);
                }
                double[] trueScale = Tools.calBounds(sourceTrueList);
                double[] rawScale = Tools.calBounds(rawData);

                scale[0] = (trueScale[1] - trueScale[0]) / (rawScale[1] - rawScale[0]);
                scale[1] = (trueScale[3] - trueScale[2]) / (rawScale[3] - rawScale[2]);
                transPts = new List<Point3D>();
                foreach (Point3D truePts in sourceTrueList)
                {
                    Point3D ps = new Point3D();
                    ps.X = truePts.X / scale[0];
                    ps.Y = truePts.Y / scale[1];
                    transPts.Add(ps);
                }
                double[] trueTrans = Tools.calBounds(transPts);
                double cx = (rawScale[0] - trueTrans[0]);
                double cy = (rawScale[2] - trueTrans[2]);
                foreach (Point3D truePts in transPts)
                {
                    truePts.ifShown = true;
                    truePts.X = truePts.X + cx;
                    truePts.Y = truePts.Y + cy;
                    truePts.Z = rawData[0].Z;
                }

                ShowPointsFromFile(transPts, 3);
                int showRs = showSourcePoint();

                while (showRs == 2)
                {
                    showRs = showSourcePoint();
                }
                if (showRs == 3)
                {
                    ShowPointsFromFile(rawData, 1);
                    return;
                }
                doingSourceClustring();
                sureSourceClustringandExprot();
            }
        }
        /// <summary>
        /// 导入真值文件与质心匹配
        /// </summary>
        private void iCPToolStripMenuItem_Click(object sender, EventArgs e)//导入真值文件与质心匹配
        {
            matchingPointCloud(1);
            //testMatchingPointCloud(1);
            this.ClockWiseBtn.Visible = true;
            this.AntiClockWiseBtn.Visible = true;
            this.SureAutoMatchBtn.Visible = true;
            this.XAxisChangeBtn.Visible = true;
            this.YAxisChangeBtn.Visible = true;
        }
        /// <summary>
        /// 导入固定点txt文件夹-菜单单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportTxtFixedPtsToolStripMenuItem_Click(object sender, EventArgs e)//导入固定点txt文件夹
        {
            this.treeView1.Enabled = false;//暂时不可用
            //this.FixedPointClustringToolStripMenuItem.Enabled = true;
            ImportPts ip = new ImportPts();
            DialogResult rs = ip.ShowDialog();
            if (rs == DialogResult.OK)
            {
                this.isIgnoreDuplication = ip.isIgnoreDuplication;//是否忽略重复点
                this.selPath = ip.selPath;
                this.AddFolder(selPath, ip.xdir, ip.ydir, 2 + ((this.isIgnoreDuplication) ? 1 : 2), false);
            }
            else if (rs == DialogResult.Cancel)
            {
                return;
            }
        }
        /// <summary>
        /// 导入固定点xls文件夹-菜单单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportXlsFixedPtsToolStripMenuItem_Click(object sender, EventArgs e)//导入固定点xls
        {
            this.treeView1.Enabled = false;
            this.FixedPointMatchingToolStripMenuItem.Enabled = true;
            ImportPts ip = new ImportPts();
            DialogResult rs = ip.ShowDialog();
            if (rs == DialogResult.OK)
            {
                this.isIgnoreDuplication = ip.isIgnoreDuplication;//是否忽略重复点
                this.selPath = ip.selPath;
                this.AddFolder(selPath, ip.xdir, ip.ydir, 2 + ((this.isIgnoreDuplication) ? 1 : 2), true);
            }
            else if (rs == DialogResult.Cancel)
            {
                return;
            }
        }
        /// <summary>
        /// 查看真值点菜单单击事件
        /// </summary>
        private void LookTruePointToolStripMenuItem_Click(object sender, EventArgs e)//查看真值点
        {
            seeTruePointFromFile();
        }
        /// <summary>
        /// 清空所有图像和内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearDataToolStripMenuItem_Click(object sender, EventArgs e)//清空所有图像和内容菜单单击事件
        {
            clearData();
            treeView1.Enabled = true;
        }
        /// <summary>
        /// 截取当前屏幕信息
        /// </summary>
        private void GetScreen_Click(object sender, EventArgs e)//截屏
        {   //Bitmap bit1 = new Bitmap(this.Width, this.Height);
            //this.DrawToBitmap(bit1, new Rectangle(0, 0, this.Width, this.Height));
            //int border = (this.Width - this.ClientSize.Width) / 2;//边框宽度
            //int caption = (this.Height - this.ClientSize.Height) - border;//标题栏高度
            //Bitmap bit2 = bit1.Clone(new Rectangle(border, caption, this.ClientSize.Width, this.ClientSize.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //bit1.Save("E:\\AAA.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//包括标题栏和边框
            //bit2.Save("E:\\BBB.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//不包括标题栏和边框
            //bit1.Dispose();
            //bit2.Dispose();
            Tools.Screen(this);
        }
        /// <summary> 
        /// 指南菜单单击事件
        /// </summary>
        private void GuideToolStripMenuItem_Click(object sender, EventArgs e)//指南项目
        {
            Guide g = new Guide();
            DialogResult rs = g.ShowDialog();
            if (rs == DialogResult.OK)
            {
                return;
            }
        }
        /// <summary>
        /// 调用exe菜单栏项目
        /// </summary>
        private void 调用exeToolStripMenuItem_Click(object sender, EventArgs e)//调用exe
        {
            Help.ShowHelp(this, "H:\\cali_radar_main_pack.exe");
        }
        /// <summary>
        /// 关于菜单栏单击事件
        /// </summary>
        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)//关于
        {
            AboutFrm abf = new AboutFrm();
            DialogResult dr = abf.ShowDialog();

        }
        /// <summary>
        /// 固定点同名点匹配并输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FixedPointMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 使用第三列对点进行过滤
        /// </summary>
        /// <param name="isEx">是否输出过滤后文件</param>
        public void ExcludePtsByDistance(bool isOutPut) {
            Tools.cleanDataByDistance(isOutPut, this.rawData,this.bit);
            this.ScanClustringToolStripMenuItem.Enabled = true;
            this.SourceClusteringToolStripMenuItem.Enabled = true;
            this.ExplainClusteringToolStripMenuItem.Enabled = true;
            ShowPointsFromFile(rawData, 1);
        }
        public void RejectPtsByDistanceFromFixed(bool isOutPut)
        {
            Tools.cleanDataByDistance2(isOutPut, this.grouping, this.bit);
            this.FixedPointMatchingToolStripMenuItem.Enabled = true;

        }
        //////快捷按钮单击事件
        /// <summary>
        /// 加载固定点文件
        /// </summary>
        private void tsButton_ImportFixedPoint_Click(object sender, EventArgs e)
        {
 
        }
        /// <summary>
        /// 固定点剔野
        /// </summary>
        private void tsButton_CleanFixedPoint_Click(object sender, EventArgs e)//固定点剔野
        {
            //getClusterFromList();
            Tools.getClusterCenter(clusterSum,this.rawData,this.centers,this.grouping);
            ShowPointsFromFile(centers, 3);
        }
        /// <summary>
        /// 加载扫描点文件
        /// </summary>
        private void tsButton_ImportfixedData_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 清屏
        /// </summary>
        private void tsButton_CLEANALL_Click(object sender, EventArgs e)
        {
            clearData();
            treeView1.Enabled = true;
        }
        /// <summary>
        /// 扫描点聚类
        /// </summary>
        private void tsButton_ScanPtsClustering_Click(object sender, EventArgs e)
        {
            if (rawData.Count == 0)
            {
                MessageBox.Show("没有数据,不可以聚类");
                return;
            }
            if (GetTreeViewNodeChecked(treeView1) == 0)
            {
                MessageBox.Show("没有显示任何点，不可以聚类");
                return;
            }
            //getClusterFromList();//计算聚类
            Tools.getClusterCenter(dbb.clusterAmount,this.rawData,this.centers,this.grouping);//计算质心
            ShowPointsFromFile(centers, 3);//不同颜色显示点
            this.iCPToolStripMenuItem.Enabled = true;
        }
        /// <summary>
        /// 导处聚类文件
        /// </summary>
        private void tsButton_ExportClusteringFile_Click(object sender, EventArgs e)
        {
            //exportClusterFile();
        }
        /// <summary>
        /// 真值均值文件匹配
        /// </summary>
        private void tsButton_MatchingFromFile_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 输出匹配文件button的单击事件
        /// </summary>
        private void tsButton_ExportMatchedPoint_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 输出固定点扫描均值文件
        /// </summary>
        private void tsButton_ExportFixedPointAverageFile_Click(object sender, EventArgs e)
        {
            //exportFixedPointsCenterFile();
        }




        //界面交互触发事件

        /// <summary>
        /// treeview勾选改变后触发函数
        /// </summary>
        /// <param name="treev"></param>
        /// <returns></returns>
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)//树勾选触发事件
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    if (e.Node.Equals(root))
                    {
                        if (root.Nodes.Count == 0) { return; }
                        foreach (TreeNode tt in root.Nodes)
                        {
                            tt.Checked = true;
                            foreach (TreeNode tn in tt.Nodes)
                            {
                                tn.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        setChildNodeCheckedState(e.Node, true);
                    }
                }
                else
                {
                    if (e.Node.Equals(root))
                    {
                        if (root.Nodes.Count == 0) { return; }
                        foreach (TreeNode tt in root.Nodes)
                        {
                            tt.Checked = false;
                            foreach (TreeNode tn in tt.Nodes)
                            {
                                tn.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        setChildNodeCheckedState(e.Node, false);
                        //如果节点存在父节点，取消父节点的选中状态
                        if (e.Node.Parent != null)
                        {
                            setParentNodeCheckedState(e.Node, false);
                        }
                    }
                    //取消节点选中状态之后，取消所有父节点的选中状态                    
                }
                this.toolStripStatusLabel2.Text = String.Format("Root选中节点数： {0}", GetNodeChecked(root))
+ String.Format("    TreeView选中二级节点数：{0}", GetTreeViewNodeChecked(treeView1));

                ArrayList pathIdChecked = new ArrayList();
                int tmp_pathId = 0;
                foreach (TreeNode tt in root.Nodes)
                {
                    foreach (TreeNode tn in tt.Nodes)
                    {
                        if (tn.Checked)
                        {
                            pathIdChecked.Add(tmp_pathId);
                        }
                        tmp_pathId++;
                    }
                }

                for (int p = 0; p < rawData.Count; p++)
                {
                    if (pathIdChecked.Contains(rawData[p].pathId))
                    {
                        rawData[p].ifShown = true;
                    }
                    else
                    {
                        rawData[p].ifShown = false;
                    }
                }
                ShowPointsFromFile(rawData, 1);
            }
            //root.ExpandAll();            
        }
        /// <summary>
        /// 变更窗体大小刷新界面
        /// </summary>
        private void FrmMain_Resize(object sender, EventArgs e)//窗体变化刷新事件
        {
            if (vtkControl == null)
            {
                vtkControl = new vtkFormsWindowControl();
            }
            if (ren == null)
            {
                ren = new vtkRenderer();
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
        }
        /// <summary>
        /// 扫描点聚类菜单栏可用后，触发子项目可用
        /// </summary>
        private void ScanClustringToolStripMenuItem_EnabledChanged(object sender, EventArgs e)//扫面点菜单可用
        {
            this.ExplainClusteringToolStripMenuItem.Enabled = true;
            this.SourceClusteringToolStripMenuItem.Enabled = true;
        }



        private void 测试矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Matrix m = new Matrix(2, 2);
            //Matrix n = new Matrix(2, 2);
            //Matrix mul = new Matrix(2, 1);
            //m[0, 0] = 1; n[0, 0] = 2;
            //m[0, 1] = 2; n[0, 1] = -1;
            //m[1, 0] = 0; n[1, 0] = 4;
            //m[1,1] = 3; n[1,1] = -4;
            //m = m - n;
            //Console.WriteLine("\n"+m.ToString());
            Matrix m = new Matrix(3, 3);
            m[0, 0] = 1; m[0, 1] = 0; m[0, 2] = 2;
            m[1, 0] = 0; m[1, 1] = 2; m[1, 2] = 0;
            m[2, 0] = 2; m[2, 1] = 0; m[2, 2] = 3;
            //public static bool ComputeEvJacobi(Matrix m,double[] dblEigenValue, Matrix mtxEigenVector, int nMaxIt, double eps)
            double[] dblEigenValue = new double[3]{0,0,0};
            Matrix mtxEigenVector = new Matrix(3,3);
            int nMaxIt = 100;
            double eps = 0.0001;
            bool rs =Matrix.ComputeEvJacobi(m, dblEigenValue, mtxEigenVector, nMaxIt, eps);
            Console.WriteLine("\n" + "____________________________" + rs + "\t" + dblEigenValue[0] + "\t" + dblEigenValue[1] + "\t" + dblEigenValue[2]);
            Console.WriteLine(mtxEigenVector.ToString());
        }
        private void 测试ICpToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<Point3D> sourceSet = new List<Point3D>();
            List<Point3D> dataSet = new List<Point3D>();
            Point3D point;
            
                //treeDir.Nodes.Add(Path.GetFileName(file));

                FileMap fileMap = new FileMap();
                List<string> pointsList1 = null;
                int line = 0;

                pointsList1 = fileMap.ReadFile("C:\\Users\\Administrator\\Desktop\\Data\\实验数据\\真实值XYZ.txt");
                line = pointsList1.Count;
                for (int i = 1; i < line; i++)
                {
                    string[] tmpxyz = pointsList1[i].Split('\t');
                    point = new Point3D();
                    point.X = Convert.ToDouble(tmpxyz[0]);//第一个字段
                    point.Y = Convert.ToDouble(tmpxyz[1]);//第二个字段
                    point.Z = Convert.ToDouble(tmpxyz[2]);//第三个字段
                    sourceSet.Add(point);
                }
                Console.WriteLine("\n真实值："+sourceSet.Count);



                List<string> pointsList2 = null;

                pointsList2 = fileMap.ReadFile("C:\\Users\\Administrator\\Desktop\\Data\\实验数据\\聚类后质心XYZ.txt");
                line = pointsList2.Count;
                for (int i = 1; i < line; i++)
                {
                    string[] tmpxyz = pointsList2[i].Split('\t');
                    point = new Point3D();
                    point.X = Convert.ToDouble(tmpxyz[0]);//第一个字段
                    point.Y = Convert.ToDouble(tmpxyz[1]);//第二个字段
                    point.Z = Convert.ToDouble(tmpxyz[2]);//第三个字段
                    dataSet.Add(point);
                }
                Console.WriteLine("扫描点质心数：" + dataSet.Count);
                ICP icp = new ICP();
                //go_hell_ICP(List<Point3D> model,List<Point3D> data,Matrix R,Matrix T,double e);
            Matrix R = Matrix.ZeroMatrix(3,3);
            Matrix T = Matrix.ZeroMatrix(3,1);
            double ee = 0.0001;
            icp.go_hell_ICP(sourceSet,dataSet,R,T,ee);
            Console.WriteLine(R.ToString());
            Console.WriteLine(T.ToString());
            


        }

        private void 测试图例ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isShowLegend(2);
        }

        private void 测试真值点导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportTrueValuePoint trvp = new ImportTrueValuePoint();
            trvp.ShowDialog();
        }
        public delegate void MessageBoxHand();
        private void 测试MessageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Invoke(new MessageBoxHand(delegate() {
                MessageBox.Show(null, "呵呵呵", "666");
            }));

        }

        private void 测试输出双文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MCC mc = new MCC();
            mc.ShowDialog();
        }

        private void 测试最大最小值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rawData == null || rawData.Count == 0) return;
            MessageBox.Show(rawData.Max(m => m.Distance).ToString() + "\t" + rawData.Min(m => m.Distance).ToString());
        }

        private void 测试按照左上角排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double x_Min = rawData.Min(m => m.X);
            double y_Min = rawData.Min(m => m.Y);
            double x_Max = rawData.Max(m => m.X);
            double y_Max = rawData.Max(m => m.Y);
            if (rawData == null || rawData.Count == 0) return;
            rawData.Sort((x, y) =>
                {
                    int result;
                    double d1 = Math.Max(x.X - x_Min , x.Y - y_Min);
                    double d2 = Math.Max(y.X - x_Min, y.Y - y_Min);
                    if (d1 == d2)
                    {
                        result = 0;
                    }
                    else
                    {
                        if (d1 > d2)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    return result;
                }
              );
            List<Point3D> cell=rawData.Take(2000).ToList();
            //MessageBox.Show(rawData.Count+"");
            double cell_x = cell.Max(m => m.X) - x_Min;
            double cell_y = cell.Max(m => m.Y) - y_Min;
            int rows = (int)((y_Max - y_Min)/cell_y)+1;
            int cols =  (int)((x_Max - x_Min)/cell_x)+1;
            List<Point3D>[] cells= new List<Point3D>[rows * cols];
            cells[0] = cell;
            Console.WriteLine("rows : " + rows + "\tcols : " + cols);
            int index = 0;
            for (int p = 0; p < rows; p++) {
                for (int q = 0; q < cols; q++)
                {
                    if (index == 0) { index++; }
                    else
                    {
                        cells[index++] = Tools.getListByScale(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Min + (q + 1) * cell_x, y_Min + (p + 1) * cell_y);
                    }
                }
            }
            int sum = 0;
            for (int i = 0; i < cells.Length; i++)
            {
                Console.Write(cells[i].Count + "\t");
                sum += cells[i].Count;
            }
                Console.WriteLine("\n\r 总点数 ： " + sum+"\t总分块数 ："+cells.Length);
            }

        private void 测试多线程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double x_Min = rawData.Min(m => m.X);
            double y_Min = rawData.Min(m => m.Y);
            double x_Max = rawData.Max(m => m.X);
            double y_Max = rawData.Max(m => m.Y);
            if (rawData == null || rawData.Count == 0) return;
            rawData.Sort((x, y) =>
            {
                int result;
                double d1 = Math.Max(x.X - x_Min, x.Y - y_Min);
                double d2 = Math.Max(y.X - x_Min, y.Y - y_Min);
                if (d1 == d2)
                {
                    result = 0;
                }
                else
                {
                    if (d1 > d2)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
              );
            testParams tp = new testParams();
            int hee = 0;
            if (tp.ShowDialog() == DialogResult.OK) hee = tp.ptsNum;
            //MessageBox.Show("pts = " + hee);
            List<Point3D> cell = rawData.Take(hee).ToList();
            //MessageBox.Show(rawData.Count+"");
            double cell_x = cell.Max(m => m.X) - x_Min;
            double cell_y = cell.Max(m => m.Y) - y_Min;
            int rows = (int)((y_Max - y_Min) / cell_y) + 1;
            int cols = (int)((x_Max - x_Min) / cell_x) + 1;
            List<Point3D>[] cells = new List<Point3D>[rows * cols];
            cells[0] = cell;
            Console.Write("rows : " + rows + "\tcols : " + cols+"\t");
            int index = 0;
            for (int p = 0; p < rows; p++)
            {
                for (int q = 0; q < cols; q++)
                {
                    if (index == 0) { index++; }
                    else
                    {
                        cells[index++] = Tools.getListByScale(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Min + (q + 1) * cell_x, y_Min + (p + 1) * cell_y);
                    }
                }
            }
            int sum = 0;
            Console.WriteLine("\n\r总点数：" + sum + "\t总分块数：" + cells.Length);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < cells.Length; i++)
            {
                ThreadPool.QueueUserWorkItem(StartCode,cells[i]);                              
            }
            RegisteredWaitHandle registeredWaitHandle = null;
            var mainAutoResetEvent = new AutoResetEvent(false);
            registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), new WaitOrTimerCallback(delegate(object obj, bool timeout)
            {
                int workerThreads = 0;
                int maxWordThreads = 0;
                int compleThreads = 0;
                ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
                ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);
                //Console.WriteLine("Check 可用线程{0},最大线程{1}", workerThreads, maxWordThreads);
                //当可用的线数与池程池最大的线程相等时表示线程池中所有的线程已经完成
                if (workerThreads == maxWordThreads)
                {
                    Console.WriteLine("线程池里的线程都执行完了");
                    mainAutoResetEvent.Set();
                    registeredWaitHandle.Unregister(null);
                }
            }), null, 1000, false);
            Console.WriteLine("主线程进入等待");
            mainAutoResetEvent.WaitOne();
            Console.WriteLine("主线程继续执行");
            //Console.WriteLine("聚类运行时间：" + stopwatch.Elapsed.ToString(), "消息");
            MessageBox.Show("聚类运行时间：" + stopwatch.Elapsed.ToString());
            System.IO.StreamWriter sw = new System.IO.StreamWriter("G:\\" + hee + ".txt", false);//把cells分别按照聚类输出 ID需要合并 
            int idLastCell = 0;
            int idLast = 0;
            int idNow;
            try{
            for (int i = 0; i < cells.Length; i++)
            {
                
                if (cells[i].Count == 0) continue;
                cells[i].Sort((x, y) =>//按照ID排序 否则
                {
                    int result;
                    if (x.clusterId == y.clusterId)
                    {
                        result = 0;
                    }
                    else
                    {
                        if (x.clusterId > y.clusterId)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    return result;
                });
                idNow = 0;
                    foreach (Point3D p in cells[i])
                    {
                        if (p.clusterId == 0)
                        {
                            sw.WriteLine(p.X + "\t" + p.Y + "\t" + p.Z + "\t" + 0);
                            continue;
                        }
                        else if (idLast != p.clusterId)
                        {
                            idNow++;
                        }
                        if(p.clusterId != 0)
                        {
                            sw.WriteLine(p.X + "\t" + p.Y + "\t" + p.Z + "\t" + (idNow + idLastCell));
                        }
                        idLast = p.clusterId;
                    }
                    idLastCell = idLastCell + idNow;
                    Console.WriteLine("Count of cell " + cells[i][cells[i].Count - 1].clusterId);
                } }
                 catch
                {
                 throw;
                }
                finally
                {
                    sw.Close();
                }
        }
        private static void StartCode(object i)
        {
            List<Point3D> cell = i as List<Point3D>;
            DBImproved ThreadDB = new DBImproved();
            ThreadDB.dbscan(cell, 0.07, 7);
            Console.WriteLine(cell.Count+"个数据点，"+ThreadDB.clusterAmount + " 个聚类");
       }

        private void 测试野点回调ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileMap fileMap = new FileMap();
            List<string> pointsList = fileMap.ReadFile("G:\\200.txt");
            String[] tmpxyz;
            Point3D point ;
            List<Point3D> lis = new List<Point3D>();
            int lastClusterID = 0,clusterLength = 0,deleteNum=0,id;
            for (int i = 0; i < pointsList.Count; i++)
            {
                point = new Point3D();
                tmpxyz = pointsList[i].Split('\t');
                id = Convert.ToInt32(tmpxyz[3]);
                if ( id != 0)
                {                
                    if(id != lastClusterID)
                    {
                        if (clusterLength <= 3) {
                        deleteNum ++;
                            for (int t = 0; t < clusterLength; t++) {
                                lis[i - clusterLength + t].clusterId = 0;
                            }
                        }
                        lastClusterID = Convert.ToInt32(tmpxyz[3]);
                        clusterLength = 1;
                    }
                    else
                    {
                        clusterLength++;//聚类点数加1
                    }
                }
                point.X = Convert.ToDouble(tmpxyz[0]);//第一个字段
                point.Y = Convert.ToDouble(tmpxyz[1]);//第二个字段
                point.Z = Convert.ToDouble(tmpxyz[2]);//第三个字段
                if (id !=0)
                {
                    point.clusterId = id - deleteNum;
                    point.isClassed = true;
                }
                else
                {
                    point.clusterId = 0;
                    point.isClassed = false;
                }
                point.ifShown =true;
                lis.Add(point);
            }
            Console.WriteLine("因为聚类过小被删的有 ： " + deleteNum);
            
            List<Point3D> addList =lis.FindAll(delegate(Point3D p) { return (p.clusterId ==0); });
            dbb = new DBImproved();
            dbb.cf = 242 - deleteNum;
            dbb.dbscan(addList, 0.07, 7);
            Console.WriteLine(dbb.clusterAmount);
            double[] ccenters = new double[(242 - deleteNum) * 3];//分别记录x y z
            int[] counts = new int[(242 - deleteNum)];//记录数量

            foreach (Point3D p in lis)
            {
                //Console.WriteLine(p.clusterId);
                if (p.clusterId != 0)
                {
                    ccenters[(p.clusterId - 1) * 3] += p.X;
                    ccenters[(p.clusterId - 1) * 3 + 1] += p.Y;
                    ccenters[(p.clusterId - 1) * 3 + 2] += p.Z;
                    counts[p.clusterId - 1] += 1;
                    //grouping[p.clusterId - 1].Add(p);
                }
            }
            centers = new List<Point3D>();
            int clusDeleteNum = 0;
            List<int> deleteIDs = new List<int>();//剔除聚类过小的ID
            for (int i = 0; i < counts.Length; i++)
            {
                //if (counts[i] < 5)//排除聚类为小于5的
                //{
                //    clusDeleteNum ++;
                //    continue;
                //}
                centers.Add(new Point3D(ccenters[(i) * 3] / counts[i], ccenters[(i) * 3 + 1] / counts[i], ccenters[(i) * 3 + 2] / counts[i], i + 1 - clusDeleteNum, true));//ID也写进入
                //Console.WriteLine(centers[i - clusDeleteNum].X + "  " + centers[i - clusDeleteNum].Y + "  " + centers[i - clusDeleteNum].Z + "  " + centers[i - clusDeleteNum].clusterId);
            }
            Console.WriteLine("聚类数 ："+(counts.Length - clusDeleteNum));
            this.clusterSum = counts.Length - clusDeleteNum;//多一个零聚类
            //int zeroSum = 0;//记录0聚类的个数
            //foreach (Point3D pppp in lis)
            //{
            //    if (pppp.clusterId == 0)
            //    {
            //        List<Point3D> tmpL = centers.FindAll(delegate(Point3D p) { return (Math.Abs((p.X - pppp.X) + Math.Abs(p.Y - pppp.Y)) < 0.03); });
            //        if (tmpL.Count > 0)
            //        {
            //            Console.WriteLine("待选聚类数 : " + tmpL.Count);
            //            tmpL.Sort((x, y) =>
            //            {
            //                int result;
            //                double d1 = Math.Abs(x.X - pppp.X) + Math.Abs(x.Y - pppp.Y);
            //                double d2 = Math.Abs(y.X - pppp.X) + Math.Abs(y.Y - pppp.Y);
            //                if (d1 == d2)
            //                {
            //                    result = 0;
            //                }
            //                else
            //                {
            //                    if (d1 > d2)
            //                    {
            //                        result = 1;
            //                    }
            //                    else
            //                    {
            //                        result = -1;
            //                    }
            //                }
            //                return result;
            //            });   //这里需要写入参数rawData.Take(hee).ToList()
            //            pppp.clusterId = tmpL.Take(1).ToList()[0].clusterId;
            //        }
            //        else
            //        {
            //            zeroSum++;
            //        }
            //    }
            //}
            //Console.WriteLine("0聚类的个数为 ：" + zeroSum);
            ShowPointsFromFile(lis, 2);
            //Tools.getClusterCenter(clusterSum, lis, this.centers, this.grouping);//计算质心 计算分组
            this.circles = Tools.getCircles(this.hulls, clusterSum);//计算外接圆
            showCircle(this.circles, 1,lis);
            //            Tools.getClusterCenter(mf.dbb.clusterAmount,mf.rawData, mf.centers, mf.grouping);//计算质心 计算分组
            //mf.ShowPointsFromFile(mf.centers, 3);//不同颜色显示核心点与野点  这一步对聚类进行分组 计算外接多边形
            //mf.circles = Tools.getCircles(mf.hulls, mf.clusterSum);//计算外接圆
            //mf.showCircle(mf.circles, 1);
        }
        



      
    }
}
