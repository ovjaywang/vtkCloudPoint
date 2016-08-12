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
        RegisteredWaitHandle registeredWaitHandle = null;
        double[] tmpAngle = { 1.0, 2.0, 1.0, 2.0 };
        public int bit = 0;
        vtkRenderer ren = null;
        string fullFilePath;
        TreeNode root = null;//根目录
        string selPath;//自身路径
        public bool isScaned;
        vtkFormsWindowControl vtkControl = null;
        List<double> xSet = new List<double>(), ySet = new List<double>(), zSet = new List<double>();//x,y,z坐标集合
        public double x_angle = 0.0, y_angle = 0.0;//x y灵位角度
        static int sumPts = 0;//总点数 总聚类数
        //dbscan相关
        //public DB dbb;//DB类对象
        public DBImproved dbb;
        static double threhold;//dbscan阈值
        static int pointsInthrehold;//dbscan点数
        static int ptsIncell;//分块内点数
        public bool isSureClusterRs = false;
        Clustering cp;
        public Dictionary<int, int> dick;//融合分块聚类的ID映射
        static List<Point3D>[] cells;//分块聚类集合
        public List<Point3D> clusForMerge;//暂时聚类分布
        System.Diagnostics.Stopwatch stwt;
        //点集相关
        public List<Point3D> rawData = new List<Point3D>();//raw是原始x y z值数据
        bool isIgnoreDuplication = true;//是否忽略重复点
        public List<ClusObj> clusList = null;//依据聚类ID的分组List 每个ClusObj存一个聚类
        List<List<Point3D>> classedrawData = new List<List<Point3D>>();//源文件匹配相关
        ArrayList pathList = new ArrayList();//路径列表
        public List<Point3D> centers = null;//聚类质心
        public List<Point3D> centers2D = null;
        public List<Point3D> trues = null;
        List<int> matchedID = new List<int>();//已匹配ID
        //多线程相关
        static int threadCount = 0;
        private delegate void funHandle(int nValue);//声明计算聚类的线程
        public delegate void CallBackDelegate(string message);
        private static WaitingForm progressForm = new WaitingForm();
        private ProgressBar psbar = progressForm.progressBar1;
        private delegate void UpdateStatusDelegate();
        private BackgroundWorker bkWorker = new BackgroundWorker();
        static private BackgroundWorker bkWorker3 = new BackgroundWorker();
        //icp相关
        //vtkPoints truePoints;//用以顯示的真值點集
        bool isSureRegion = true;//判斷是否確
        List<Point3D> InRegionTrues;//范围内均值
        vtkMatrix4x4 M;//刚性变换矩阵
        vtkPoints truePointCloud = new vtkPoints();//真值点云
        int[] truePointPid = new int[1];//真值点Id
        vtkCellArray truePointVertices = new vtkCellArray();//真值顶点
        vtkPolyVertex truePolyVertex = new vtkPolyVertex();
        vtkActor trueActor;//真值点actor
        int clock = 0;
        int clock_x = 1, clock_y = 1;//x y轴正负
        //简单聚类相关
        public static int clusterSum = 0;
        int pointSum = 0;
        double simple_x_step, simple_y_step;
        vtkActor actorLine = new vtkActor(), actorLine2 = new vtkActor(), actorLine3 = new vtkActor(), actorLine4 = new vtkActor();//画线
        public vtkActor actorA ,actorB,actorC;
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
        //public List<Point2D>[] hulls;//个数为聚类数 每个元素为某ID的所有点集
        public List<Point2D> circles;
        public List<Point2D> circles2D;
        public List<int> filterID = new List<int>();//阈值过滤的ID
        double[] scale = new double[3];//比例尺 三个方向
        double[] trueScale = new double[6];//真值范围
        double[] centroidScale;//质心范围
        vtkAxesActor axes = new vtkAxesActor();
        vtkOrientationMarkerWidget widget = new vtkOrientationMarkerWidget();
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
            bkWorker3.WorkerReportsProgress = true;
            bkWorker3.WorkerSupportsCancellation = true;
            bkWorker3.DoWork += new DoWorkEventHandler(DoWork3);
            bkWorker3.ProgressChanged += new ProgressChangedEventHandler(ProgessChanged3);
            bkWorker3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWork3);
            if (vtkControl == null)
            {
                vtkControl = new vtkFormsWindowControl();
            }
            widget.SetOutlineColor(0.9300, 0.5700, 0.1300);
            widget.SetOrientationMarker(axes);

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
        /// <summary>
        /// 在线程中更新线程池中的状态 当可用线程等于最大线程则退出
        /// </summary>

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
        public  void showMatchedLine(bool isShowUnmatchedCenterPts,bool isShowUnmatchedTruePts)//画匹配线
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
            for (int i = 0; i < centers.Count; i++)//质心点
            {
                if (centers[i].isMatched)//若质心被匹配
                {
                    match_Pid[0] = matchPoints.InsertNextPoint(centers[i].matched_X, centers[i].matched_Y, centers[i].matched_Z);
                    matchCellArry.InsertNextCell(1, match_Pid);

                    match_Pid[0] = matchPoints.InsertNextPoint(truePointCloud.GetPoint(centers[i].matchNum));
                    matchCellArry.InsertNextCell(1, match_Pid);
                }
                else if (isShowUnmatchedCenterPts)//若质心未被匹配
                {
                    match_Pid[0] = unMatchPointCloud.InsertNextPoint(centers[i].matched_X, centers[i].matched_Y, centers[i].matched_Z);
                    unMatchCellArry.InsertNextCell(1, match_Pid);

                }
            }
            if (isShowUnmatchedTruePts) {
                for (int i = 0; i < truePointCloud.GetNumberOfPoints(); i++)//真值点
                {
                    if (!(matchedID.Contains(i)))//若真值ID不在列表里
                    {
                        match_Pid[0] = unMatchTruePointCloud.InsertNextPoint(truePointCloud.GetPoint(i));
                        unMatchTrueCellArry.InsertNextCell(1, match_Pid);
                    }
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
            matchActor.GetProperty().SetColor(1, 0, 0);//红色
            matchActor.GetProperty().SetPointSize(5);
            unMatchActor.SetMapper(unMatchMapper);
            unMatchActor.GetProperty().SetColor(0, 1, 0);//绿色
            unMatchActor.GetProperty().SetPointSize(5);
            unMatchTrueActor.SetMapper(unMatchTrueMapper);
            unMatchTrueActor.GetProperty().SetColor(0, 0, 1);//蓝色
            unMatchTrueActor.GetProperty().SetPointSize(5);

            ren.AddActor(matchActor);
            ren.AddActor(unMatchActor);
            ren.AddActor(unMatchTrueActor);
            vtkLineSource lineSource;
            for (int j = 0; j < centers.Count; j++)
            {
                if (centers[j].isMatched)
                {
                    lineSource = new vtkLineSource();
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
            showAxes();
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
            if (type == 1 || type == 2 || type == 6 || type == 7)
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
                            //hulls[points[i].clusterId - 1].Add(new Point2D(points[i].X, points[i].Y));
                            //clusList[points[i].clusterId - 1].li.Add(new Point3D(points[i].X, points[i].Y, points[i].Z, points[i].clusterId, true));
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
                    else if (type == 6)
                    {
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
                actorA = new vtkActor();
                actorA.SetMapper(map_1);
                if (type == 1)
                {
                    actorA.GetProperty().SetPointSize(1.2f);
                }
                else if (type == 3)
                {
                    actorA.GetProperty().SetPointSize(4.5f);
                    actorA.GetProperty().SetColor(0.0, 0, 1.0);
                }
                else if (type == 7)
                {
                    actorA.GetProperty().SetColor(0.0, 1, 0);
                }
                ren.AddActor(actorA);
            }
            else if (type == 2 || type == 6)
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
                actorB = new vtkActor();
                actorB.SetMapper(map_2);
                actorB.GetProperty().SetPointSize(1.5f);
                actorB.GetProperty().SetColor(1.0, 0, 0);//红色
                ren.AddActor(actorB);
                actorC = new vtkActor();
                actorC.SetMapper(map_3);
                actorC.GetProperty().SetPointSize(1.2f);
                actorC.GetProperty().SetColor(0, 1, 0);//绿色
                ren.AddActor(actorC);
            }

            vtkControl.GetRenderWindow().AddRenderer(ren);

            if (type == 1 || type ==6 || type ==7 ) {
                showAxes();
            }
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
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("小于阈值点数：{0},大于阈值点数：{1}", count_3, count_2);
                    break;
                case 7:
                    this.toolStripStatusLabelCurrentPointCount.Text = String.Format("未被过滤点云数： {0}", count_1);
                    break;
                default:
                    break;
            }
        }

        public void deleteActor(vtkActor ac) {
            ren.RemoveActor(ac);
            vtkControl.Refresh();
        }
        public void addActor(vtkActor ac) {
            ren.AddActor(ac);
            vtkControl.Refresh();
        }
        /// <summary>
        /// 显示2D情况下的点集
        /// </summary>
        /// <param name="type">显示类型 1.阈值过滤-不显示过滤点 2.阈值过滤-显示过滤点 3.显示核心点和野点 4.显示质心</param>
        /// /// <param name="isF">是否固定点</param>
        public void Show2DPoints(List<Point3D> rd,int type) {
            vtkControl.GetRenderWindow().Clean();
            widget.SetEnabled(0);
            vtkPoints pts = new vtkPoints();
            vtkPoints pts2 = new vtkPoints();
            vtkPolyVertex vertex = new vtkPolyVertex();
            vtkPolyVertex vertex2 = new vtkPolyVertex();
            vtkDataSetMapper mapper2 = new vtkDataSetMapper();
            vtkDataSetMapper mapper22 = new vtkDataSetMapper();
            vtkUnstructuredGrid grid = new vtkUnstructuredGrid();
            vtkUnstructuredGrid grid2 = new vtkUnstructuredGrid();
            //vtkActor ac = new vtkActor();
            //vtkActor ac2 = new vtkActor();

            vtkPolyData pldata = new vtkPolyData();
            vtkPolyDataMapper mapper = new vtkPolyDataMapper();
            

            //vtkRegularPolygonSource source = new vtkRegularPolygonSource();
            //vtkRectangularButtonSource source = new vtkRectangularButtonSource();
            //vtkButtonSource source = new vtkButtonSource();
            vtkCubeSource source = new vtkCubeSource();
            source.SetXLength(0.005);
            source.SetYLength(0.005);
            source.SetZLength(0.005);
            //vtkPointSource source = new vtkPointSource();
            //source.SetNumberOfPoints(1);
            //source.SetWidth(1.0f);
            //source.SetHeight(1.0f);
            vtkGlyph2D glyph2D = new vtkGlyph2D(); ;
            int cout1 = 0, cout2 = 0; ;
            if(type ==2 || type ==1 || type==3)//1.只显示distance过滤后点 2.显示distance过滤和未过滤 3.显示核心点野点 4.质心
            {
                ren = new vtkRenderer();
                if (type == 1 || type == 2) {
                    foreach (Point3D p in rd)
                    {
                        if (!p.isFilterByDistance)
                        {
                            pts.InsertNextPoint(p.motor_x, p.motor_y, 0);
                            cout1++;
                        }
                        else if (type == 2)
                        {
                            pts2.InsertNextPoint(p.motor_x, p.motor_y, 0);
                            cout2++;
                        }
                    }
                }
                else if (type==3)
                {
                    foreach (Point3D p in rd)
                    {
                        if (p.clusterId == 0) {
                            pts.InsertNextPoint(p.motor_x, p.motor_y, 0);
                            cout1++;
                        }
                        else
                        {
                            pts2.InsertNextPoint(p.motor_x, p.motor_y, 0);
                            cout2++;
                        }
                    }
                }
                vertex.GetPointIds().SetNumberOfIds(cout1);
                for (int i = 0; i < cout1; i++)
                {
                    vertex.GetPointIds().SetId(i, i);
                }
                grid.SetPoints(pts);
                grid.InsertNextCell(vertex.GetCellType(), vertex.GetPointIds());
                mapper2.SetInput(grid);
                actorB = new vtkActor();
                actorB.SetMapper(mapper2);

                if (type == 2 || type == 3)
                {
                    vertex2.GetPointIds().SetNumberOfIds(cout2);
                    for (int i = 0; i < cout2; i++)
                    {
                        vertex2.GetPointIds().SetId(i, i);
                    }
                    grid2.SetPoints(pts2);
                    grid2.InsertNextCell(vertex2.GetCellType(), vertex2.GetPointIds());
                    mapper22.SetInput(grid2);
                    actorC = new vtkActor();
                    actorC.SetMapper(mapper22);
                    if (type == 2) {
                        actorB.GetProperty().SetPointSize(1.2f);
                        actorB.GetProperty().SetColor(0, 1, 0);
                        actorC.GetProperty().SetPointSize(3f);
                        actorC.GetProperty().SetColor(1, 0, 0);
                    }
                    else if (type==3)
                    {
                        actorB.GetProperty().SetPointSize(1.5f);
                        actorB.GetProperty().SetColor(1,0, 0);
                        actorC.GetProperty().SetPointSize(1.2f);
                        actorC.GetProperty().SetColor(0, 1, 0);
                    }
                    ren.AddActor(actorC);
                }
                if (type == 1) {
                    actorB.GetProperty().SetPointSize(1.2f);
                    actorB.GetProperty().SetColor(0, 1, 0);
                }
                ren.AddActor(actorB);
                vtkControl.GetRenderWindow().AddRenderer(ren);
                vtkControl.Refresh();
            }
            if (type == 4) {
                foreach (Point3D p in rd)
                {
                    pts.InsertNextPoint(p.X, p.Y, 0);
                    cout1++;
                }
                vertex.GetPointIds().SetNumberOfIds(cout1);
                for (int i = 0; i < cout1; i++)
                {
                    vertex.GetPointIds().SetId(i, i);
                }
                grid.SetPoints(pts);
                grid.InsertNextCell(vertex.GetCellType(), vertex.GetPointIds());
                mapper2.SetInput(grid);
                actorA = new vtkActor();
                actorA.SetMapper(mapper2);
                actorA.GetProperty().SetPointSize(3.4f);
                actorA.GetProperty().SetColor(0, 0, 1);

                ren.AddActor(actorA);
                vtkControl.Refresh();
            }
        }
        /// <summary>
        /// 显示外接圆 白色是原始图案 黄色是超过阈值图案
        /// </summary>
        /// <param name="ls">圆心点集列表</param>
        /// /// <param name="type">1.显示核心点和野点 +圆 2.显示剔除野点结果 + 圆 3.只显示半径过大的点集</param>
        public void showCircle(List<Point2D> ls, int type, List<Point3D> li, List<Point3D> cents)//显示圆形图案
        { //输入为各圆心的List
            ren = new vtkRenderer();
            if (type == 1) ShowPointsFromFile(li, 2);//不同颜色显示野点和核心点
            else if (type == 2) ShowPointsFromFile(li, 1);
            else if (type == 3)
            {
                List<int> toobig = new List<int>();
                foreach (Point2D p2 in ls)
                {
                    if (p2.radius > 0.2)
                    {
                        toobig.Add(p2.clusID);
                        Console.WriteLine("聚类ID为 ：" + p2.clusID);
                    }
                }
                li.RemoveAll((delegate(Point3D p) { return (!toobig.Contains(p.clusterId)); }));
                ls.RemoveAll((delegate(Point2D p) { return (!toobig.Contains(p.clusID)); }));
                centers.RemoveAll((delegate(Point3D p) { return (!toobig.Contains(p.clusterId)); }));
                ShowPointsFromFile(li, 1);
            }
            else if (type == 4)
            {
                li.RemoveAll((delegate(Point3D p) { return ((!dick.Keys.ToList().Contains(p.clusterId)) && (!dick.Values.ToList().Contains(p.clusterId))); }));
                ls.RemoveAll((delegate(Point2D p) { return ((!dick.Keys.ToList().Contains(p.clusID)) && (!dick.Values.ToList().Contains(p.clusID))); }));
                centers.RemoveAll((delegate(Point3D p) { return ((!dick.Keys.ToList().Contains(p.clusterId)) && (!dick.Values.ToList().Contains(p.clusterId))); }));
                ShowPointsFromFile(li, 1);
            }
            ShowPointsFromFile(cents, 3);//显示质心
            vtkRegularPolygonSource polygonSource;
            vtkPolyDataMapper mapper;
            vtkActor actor;
            for (int k = 0; k < ls.Count; k++)
            {
                if ((ls[k].radius < 0.2) && (type == 3))//
                {
                    continue;
                }
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
                if (type == 2)
                {
                    if (filterID.Contains(ls[k].clusID))
                    {
                        actor.GetProperty().SetColor(1, 1, 0);
                    }
                }
                ren.AddActor(actor);
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            if (type == 1) {
                showAxes();
            }
            vtkControl.Refresh();
        }
        public void showCircles2D(List<Point2D> cir, int type, List<Point3D> data, List<Point3D> ctr) {
            Show2DPoints(data,3);
            Show2DPoints(ctr,4);
            vtkRegularPolygonSource polygonSource;
            vtkPolyDataMapper mapper;
            vtkActor actor;
            for (int k = 0; k < cir.Count; k++)
            {
                polygonSource = new vtkRegularPolygonSource();
                polygonSource.GeneratePolygonOff(); // Uncomment this line to generate only the outline of the circle
                polygonSource.SetNumberOfSides(100);
                polygonSource.SetRadius(cir[k].radius);
                polygonSource.SetCenter(cir[k].x, cir[k].y, 0);
                // Visualize
                mapper = new vtkPolyDataMapper();
                mapper.SetInputConnection(polygonSource.GetOutputPort()); ;
                actor = new vtkActor();
                actor.SetMapper(mapper);
                actor.GetProperty().SetLineWidth(3);
                actor.GetProperty().SetOpacity(0.6);
                ren.AddActor(actor);
            }
            vtkControl.GetRenderWindow().AddRenderer(ren);
            vtkControl.Refresh();
        }

        private void showAxes() {
            vtkRenderWindowInteractor interactor = new vtkRenderWindowInteractor();
            interactor.SetRenderWindow(vtkControl.GetRenderWindow());
            vtkInteractorStyleTrackballCamera sty = new vtkInteractorStyleTrackballCamera();
            interactor.SetInteractorStyle(sty);
            widget.SetViewport(0, 0, 0.4, 0.4);
            widget.SetInteractor(interactor);
            widget.SetEnabled(1);
            widget.InteractiveOn();
            ren.ResetCamera();
            vtkControl.GetRenderWindow().Render();
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
        public double getDis(Point3D p1,Point3D p2){
            double dx = p1.tmp_X - p2.matched_X;
            double dy = p1.matched_Y - p2.matched_Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        void ICP()
        {
            //ren.RemoveAllViewProps();
            //vtkControl.GetRenderWindow().Clean();
            ren = new vtkRenderer();
            vtkPolyData SourcePolydata = Tools.ArrayList2PolyData(1, this.centers, this.trueScale, this.centroidScale,
                this.scale, this.clock, this.clock_y, this.clock_x);//对center处理
            vtkPolyData TargetPolydata = new vtkPolyData(); ;//对真值处理
            TargetPolydata.SetPoints(truePointCloud); //把点导入的polydata中去
            TargetPolydata.SetVerts(truePointVertices);
            //vtkPolyData showtruePolydata = new vtkPolyData();
            //showtruePolydata.SetPoints(visualizeTruePointCloud);
            //showtruePolydata.SetVerts(vtpVertices);

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
            vtkPolyData showLocPolydata = Tools.ArrayList2PolyData(2, this.centers, this.trueScale, this.centroidScale,
                this.scale, this.clock, this.clock_y, this.clock_x);
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
            //设置目标图形（真值点）
            vtkPolyDataMapper targetMapper = new vtkPolyDataMapper();
            targetMapper.SetInputConnection(TargetPolydata.GetProducerPort());
            vtkActor targetActor = new vtkActor();
            targetActor.SetMapper(targetMapper);
            targetActor.GetProperty().SetColor(0, 1, 0);
            targetActor.GetProperty().SetPointSize(4);
            //设置辅助显示真值点
            vtkPolyDataMapper solutionMapper = new vtkPolyDataMapper();
            solutionMapper.SetInputConnection(icpTransformFilter.GetOutputPort());
            vtkActor solutionActor = new vtkActor();
            solutionActor.SetMapper(solutionMapper);
            solutionActor.GetProperty().SetColor(0, 0, 1);
            solutionActor.GetProperty().SetPointSize(3);
            //vtkControl.GetRenderWindow().RemoveRenderer(ren);

            
            ren.AddActor(targetActor);
            ren.AddActor(solutionActor);
            //ren.SetBackground(0.3, 0.6, 0.3);
            // Render and interact

            vtkControl.GetRenderWindow().AddRenderer(ren);
            //ren.Render();
            //vtkControl.Refresh();
              

            SourcePolydata.FastDelete();
            TargetPolydata.FastDelete();
        }
        /// <summary>
        /// 添加扫描点聚类文件
        /// </summary>
        /// <param name="ptsPath">文件夹路径</param>
        /// <param name="xdir">扫描点x方向 1上2右3下4左</param>
        /// <param name="ydir">扫描点y方向 1上2右3下4左</param>
        /// <param name="typpe">1剔野+txt 2剔野+xls 3固定点+清除 4固定点+不清除</param>
        /// <param name="isXLS">是否是xls文件</param>
        private void AddFolder(String ptsPath, int xdir, int ydir, int typpe, bool isXLS)//添加数据
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
                //grouping = new List<Point3D>[files.Length];
                clusList = new List<ClusObj>();
                TreeNode treeDir = new TreeNode(ptsPath);
                root.Nodes.Add(treeDir);
                treeDir.Checked = true;
                double duplicatNum = 0;
                int pts = 0;
                String txtName;
                foreach (string file in files)
                {
                    txtName = file.Substring(ptsPath.Length + 1);
                    if (typpe == 3 || typpe == 4) {
                        clusList.Add(new ClusObj(txtName.Substring(0, txtName.IndexOf('.'))));//不剔野
                        isScaned = false;
                    }
                    else
                    {
                        isScaned = true;
                    }
                    Console.WriteLine(file);
                    //treeDir.Nodes.Add(Path.GetFileName(file));
                    treeDir.Nodes.Add(txtName);
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
                        String s = sheet.GetRow(1).GetCell(0).ToString();
                        bit = s.Length - s.LastIndexOf("-1") - 1;
                        Console.WriteLine("浮点位数为:" + bit);
                    }
                    else
                    {
                        try
                        {
                            pointsList = fileMap.ReadFile(file);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("文件很可能正在被使用！", "提示");
                            return;
                        }
                        line = pointsList.Count;
                        String ssss = pointsList[0].Split('\t')[0];
                        bit = ssss.Length - ssss.LastIndexOf(".") - 1;
                        Console.WriteLine("浮点位数为:" + bit);
                    }

                    double fangweijiao, yangjiao;
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
                        if (point.Distance == 0 || point.Distance > 1000) continue;//D过大 首先排除掉
                        //OriginalData.Add(new DataPoint(Motor_X, Motor_Y, Distance));
                        //点的路径
                        point.pathId = pathList.Count;
                        //点是否显示
                        point.ifShown = true;
                        //point.isFilter = false;
                        point.isClassed = false;
                        point.clusterId = 0;
                        if (typpe == 3 || typpe == 4)
                        {
                            point.clusterId = point.pathId + 1;
                            point.pointName = txtName.Substring(0, txtName.IndexOf('.'));//多添加一个点名字段
                        }
                        yangjiao = (-2) * (point.motor_x - this.x_angle) / 180 * Math.PI;
                        fangweijiao = 2 * (point.motor_y - this.y_angle) / 180 * Math.PI;

                        double tmpx = point.Distance * Math.Cos(yangjiao) * Math.Sin(fangweijiao);
                        double tmpy = point.Distance * Math.Sin(yangjiao) * Math.Cos(fangweijiao); 
                        switch (xdir)
                        {
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
                            if (plist.Count == 0) rawData.Add(point);
                            else duplicatNum += 1;
                        }
                        else if (typpe == 3 || typpe == 4)//固定点-清除或不清除
                        {
                            //if (grouping[pathList.Count].FindAll(delegate(Point3D p) { return (p.X == tmpx) && (p.Y == tmpy) && (p.Z == tmpz); }).Count == 0){
                            if (clusList[pathList.Count].li.FindAll(delegate(Point3D p) { return (p.X == tmpx) && (p.Y == tmpy) && (p.Z == tmpz); }).Count == 0)
                            {
                                point.ptsCount = 1;
                                clusList[pathList.Count].li.Add(point);
                                pts++;
                            }

                            else
                            {
                                clusList[pathList.Count].li[clusList[pathList.Count].li.FindIndex(0, clusList[pathList.Count].li.Count, delegate(Point3D p) { return (p.X == tmpx) && (p.Y == tmpy) && (p.Z == tmpz); })].ptsCount += 1;
                                if (typpe == 3) duplicatNum++;
                                else pts++;
                            }
                        }
                        else if (typpe == 2)//不清除重复-扫描点
                        {
                            rawData.Add(point);
                        }
                    }
                    pathList.Add(file);//文件名加入文件名List
                };
                setChildNodeCheckedState(treeDir, true);
                root.Checked = true;
                treeView1.ExpandAll();
                if (typpe == 1)
                {
                    MessageBox.Show("共" + (rawData.Count + duplicatNum) + "个点，其中" + duplicatNum + "个重复点，剩余" + rawData.Count + "个点。", "提示");
                    ShowPointsFromFile(rawData, 1);
                }
                else if (typpe == 2)
                {
                    MessageBox.Show("共" + rawData.Count + "个点。", "提示");
                    ShowPointsFromFile(rawData, 1);
                }
                else if (typpe == 3)
                {
                    MessageBox.Show("共" + (clusList.Count) + "个固定点，其中" + duplicatNum + "个重复点，剩余" + pts + "个数据点。", "提示");
                    showFixPointData(1);//显示完整数据
                }
                else if (typpe == 4)
                {
                    MessageBox.Show("共" + (clusList.Count) + "个固定点，" + pts + "个数据点。", "提示");
                    showFixPointData(1);//显示完整数据
                }
                SureDistanceFilter sdf = new SureDistanceFilter(typpe > 2);//用typpe判断是否固定点
                sdf.Left = 0;
                if (typpe == 1 || typpe == 2)
                {
                    sdf.textBox_maxD.Text = rawData.Max(m => m.Distance).ToString();
                    sdf.textBox_minD.Text = rawData.Min(m => m.Distance).ToString();
                }
                else if (typpe == 3 || typpe == 4)
                {
                    sdf.textBox_maxD.Text = Tools.GetGroupingManOrMin(this.clusList, 1).ToString();
                    sdf.textBox_minD.Text = Tools.GetGroupingManOrMin(this.clusList, 2).ToString();
                    sdf.rb_2d.Visible = false;
                    sdf.rb_3d.Visible = false;
                }
                sdf.Show(this);
            }


        }
        //执行dbscan的线程
        public void getClusterFromList(double tr, int pts, int ptsInCell)//执行dbscan聚类线程
        {
            MainForm.threhold = tr;
            MainForm.pointsInthrehold = pts;
            MainForm.ptsIncell = ptsInCell;
            foreach (Point3D p in rawData)
            {
                p.clusterId = 0;
                p.isClassed = false;
            }
            double x_Min = rawData.Min(m => m.X);//计算x最小
            double y_Min = rawData.Min(m => m.Y);//计算y最小
            double x_Max = rawData.Max(m => m.X);//计算x最大
            double y_Max = rawData.Max(m => m.Y);//计算y最大
            if (rawData == null || rawData.Count == 0) return;
            rawData.Sort((x, y) =>//按照与最小值最近距离排序
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
            Console.WriteLine("分块点数 = " + MainForm.ptsIncell);
            List<Point3D> cell = rawData.Take(MainForm.ptsIncell).ToList();
            //MessageBox.Show(rawData.Count+"");
            double cell_x = cell.Max(m => m.X) - x_Min;
            double cell_y = cell.Max(m => m.Y) - y_Min;
            int rows = (int)((y_Max - y_Min) / cell_y) + 1;
            int cols = (int)((x_Max - x_Min) / cell_x) + 1;
            cells = new List<Point3D>[rows * cols];
            cells[0] = cell;
            int index = 0;
            for (int p = 0; p < rows; p++)
            {
                for (int q = 0; q < cols; q++)
                {
                    if (index == 0) { index++; }
                    else
                    {
                        if ((p == (rows - 1)) && (q != (cols - 1)))
                        {
                            cells[index++] = Tools.getListByScale(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Min + (q + 1) * cell_x, y_Max);
                        }
                        else if ((p != (rows - 1)) && (q == (cols - 1)))
                        {
                            cells[index++] = Tools.getListByScale(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Max, y_Min + (p + 1) * cell_y);
                        }
                        else if ((p == (rows - 1)) && (q == (cols - 1)))
                        {
                            cells[index++] = Tools.getListByScale(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Max, y_Max);
                        }
                        else
                            cells[index++] = Tools.getListByScale(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Min + (q + 1) * cell_x, y_Min + (p + 1) * cell_y);
                    }
                }
            }
            Console.WriteLine("\n\r总分块数：" + cells.Length + ",共 " + rows + " 行 " + cols + " 列.");
            bkWorker3.RunWorkerAsync();
            progressForm = new WaitingForm();
            progressForm.progressBar1.Maximum = cells.Length;
            progressForm.Show();
        }
        public void getClusterFromMotor(double tr, int pts, int ptsInCell)//执行dbscan聚类线程
        {
            MainForm.threhold = tr;
            MainForm.pointsInthrehold = pts;
            MainForm.ptsIncell = ptsInCell;
            foreach (Point3D p in rawData)
            {
                p.clusterId = 0;
                p.isClassed = false;
            }
            double x_Min = rawData.Min(m => m.motor_x);//计算x最小
            double y_Min = rawData.Min(m => m.motor_y);//计算y最小
            double x_Max = rawData.Max(m => m.motor_x);//计算x最大
            double y_Max = rawData.Max(m => m.motor_y);//计算y最大
            if (rawData == null || rawData.Count == 0) return;
            rawData.Sort((x, y) =>//按照与最小值最近距离排序
            {
                int result;
                double d1 = Math.Max(x.motor_x - x_Min, x.motor_y - y_Min);
                double d2 = Math.Max(y.motor_x - x_Min, y.motor_y - y_Min);
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
            Console.WriteLine("分块点数 = " + MainForm.ptsIncell);
            List<Point3D> cell = rawData.Take(MainForm.ptsIncell).ToList();
            //MessageBox.Show(rawData.Count+"");
            double cell_x = cell.Max(m => m.motor_x) - x_Min;
            double cell_y = cell.Max(m => m.motor_y) - y_Min;
            int rows = (int)((y_Max - y_Min) / cell_y) + 1;
            int cols = (int)((x_Max - x_Min) / cell_x) + 1;
            cells = new List<Point3D>[rows * cols];
            cells[0] = cell;
            int index = 0;
            for (int p = 0; p < rows; p++)
            {
                for (int q = 0; q < cols; q++)
                {
                    if (index == 0) { index++; }
                    else
                    {
                        if ((p == (rows - 1)) && (q != (cols - 1)))
                        {
                            cells[index++] = Tools.getListByScale2(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Min + (q + 1) * cell_x, y_Max);
                        }
                        else if ((p != (rows - 1)) && (q == (cols - 1)))
                        {
                            cells[index++] = Tools.getListByScale2(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Max, y_Min + (p + 1) * cell_y);
                        }
                        else if ((p == (rows - 1)) && (q == (cols - 1)))
                        {
                            cells[index++] = Tools.getListByScale2(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Max, y_Max);
                        }
                        else
                            cells[index++] = Tools.getListByScale2(this.rawData, x_Min + q * cell_x, y_Min + p * cell_y, x_Min + (q + 1) * cell_x, y_Min + (p + 1) * cell_y);
                    }
                }
            }
            Console.WriteLine("\n\r总分块数：" + cells.Length + ",共 " + rows + " 行 " + cols + " 列.");
            bkWorker3.RunWorkerAsync();
            progressForm = new WaitingForm();
            progressForm.progressBar1.Maximum = cells.Length;
            progressForm.Show();
        }
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            ICP();
            this.BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { });
            e.Result = ProcessProgress(bkWorker, e);
        }
        private void UpdateStatus()
        {
            showAxes();
            vtkControl.Refresh();
            vtkControl.GetRenderWindow().Render();
            vtkControl.GetRenderWindow().Start();
        }
        public void ProgessChanged(object sender, ProgressChangedEventArgs e)
        {

         }
        public void CompleteWork(object sender, RunWorkerCompletedEventArgs e)
        {
            progressForm.Close();
            MessageBox.Show("处理完毕,效果如图");
            isShowLegend(7);
            calMatchedCoords();
            MatchingParams mp = new MatchingParams();
            mp.Left = 20;
            mp.Top = 150;
            mp.Show(this);
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

        public void DoWork3(object sender, DoWorkEventArgs e)
        {
            stwt = new System.Diagnostics.Stopwatch();
            stwt.Start();
            sumPts = 0;
            clusterSum = 1;
            threadCount = 0;
            for (int i = 0; i < cells.Length; i++)
            {
                ThreadPool.QueueUserWorkItem(StartCode, cells[i]);//将每个分块加入线程池分别计算聚类
            }
            this.BeginInvoke(new UpdateStatusDelegate(UpdateStatus3), new object[] { }); 
        }
        private void UpdateStatus3()
        {
            var mainAutoResetEvent = new AutoResetEvent(false);
            registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), new WaitOrTimerCallback(delegate(object obj, bool timeout)
            {
                int workerThreads = 0;
                int maxWordThreads = 0;
                int compleThreads = 0;
                ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
                ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);

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
            treeView1.Enabled = false;
        }
        public void ProgessChanged3(object sender, ProgressChangedEventArgs e)
        {
            //progressForm.SetNotifyInfo(e.ProgressPercentage, "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%");  
            //System.Threading.Thread.Sleep(1);
            progressForm.setprogressvalue(e.ProgressPercentage);
        }
        public void CompleteWork3(object sender, RunWorkerCompletedEventArgs e)
        {
            progressForm.Close();
            MessageBox.Show("聚类运行时间：" + stwt.Elapsed.ToString() + "\n总聚类数：" + clusterSum + "  聚类数据：" + sumPts + "个");
            this.cp.Visible = true;
            this.cp.DoClusteringBtn.Text = "重新聚类";
            this.cp.MergeBtn.Enabled = true;
            this.cp.SureMergeBtn.Enabled = true;
            this.cp.Left = 0;
            //System.IO.StreamWriter sw = new System.IO.StreamWriter("G:\\" + MainForm.ptsIncell + ".txt", false);//把cells分别按照聚类输出 ID需要合并 
            int idLast = cells[0][0].clusterId;//上一个ID是多少
            int idNow = 0, id, clusLen = 0;//当前聚类累加ID、当前cell内部ID以及当前聚类长度
            int delSum = 0;
            clusForMerge = new List<Point3D>();
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].Count == 0) continue;
                cells[i].Sort((x, y) =>//按照ID排序 否则
                {
                    int result;
                    if (x.clusterId == y.clusterId) result = 0;
                    else
                    {
                        if (x.clusterId > y.clusterId) result = 1;
                        else result = -1;
                    }
                    return result;
                });
                idLast = cells[i][0].clusterId;
                if (idLast != 0)
                {
                    idNow++;
                    clusLen = 1;
                }
                else
                {
                    clusLen = 0;
                }
                for (int j = 0; j < cells[i].Count; j++)
                {
                    id = cells[i][j].clusterId;
                    if (id == 0)
                    {
                        clusForMerge.Add(cells[i][j]);//若ID为0 则ID就是0
                    }
                    else
                    {
                        if ((id != idLast))
                        {
                            if ((clusLen <= 3) && (idLast != 0))//目前设为小于3个非正常聚类
                            {//如果聚类过小 1.把该聚类的ID设为0 2.ID值不自增
                                Console.WriteLine("聚类过小！" + idNow);
                                delSum++;
                                for (int k = 0; k < clusLen; k++)
                                {
                                    clusForMerge[clusForMerge.Count - 1 - k].clusterId = 0;//回溯之前的结果 把ID设为0
                                }
                            }
                            else
                            {
                                idNow++;
                            }
                            clusLen = 1;
                        }
                        else
                        {
                            clusLen++;
                        }
                        cells[i][j].clusterId = idNow;
                        clusForMerge.Add(cells[i][j]);
                        idLast = id;
                    }
                }
            }
            Console.WriteLine();
            dbb = new DBImproved();
            dbb.clusterAmount = clusterSum - delSum;
            dbb.cf = clusterSum - delSum - 1;//设置聚类初始ID
            List<Point3D> zeroList = clusForMerge.FindAll(delegate(Point3D p) { return (p.clusterId == 0); });
            clusForMerge.RemoveAll((delegate(Point3D p) { return (p.clusterId == 0); }));
            foreach (Point3D pp in zeroList)
            {
                pp.isClassed = false;
            }
            dbb.dbscan(zeroList, MainForm.threhold, pointsInthrehold);//生怕某聚类被分割开没有聚类到 再聚一次
            foreach (Point3D p in zeroList)
            {
                clusForMerge.Add(p);
            }
            Console.WriteLine("\n分块聚类合理聚类数: " + (clusterSum - delSum) +
                ",因过聚类小于3被删除的有" + delSum + "个,重新聚类后聚类数 :" + dbb.clusterAmount);
            centers = new List<Point3D>();
            centers2D = new List<Point3D>();
            clusList = new List<ClusObj>();
            ClusObj obj;
            for (int j = 0; j < dbb.clusterAmount; j++)
            {
                obj = new ClusObj();
                obj.clusId = j + 1;
                clusList.Add(obj);
            }
            Tools.GetClusList(clusForMerge, centers,centers2D, clusList, null);
            foreach (ClusObj ob in clusList)
            {
                Console.WriteLine(ob.clusId + "   " + ob.li.Count);
            }
            MainForm.clusterSum = dbb.clusterAmount;
            this.circles = Tools.getCircles(this.clusList,true);//计算外接圆
            this.circles2D = Tools.getCircles(this.clusList,false);//计算2D外接圆
            showCircle(this.circles, 1, clusForMerge, this.centers);
            isShowLegend(2);
            
        }       
        /// <summary>
        /// 导出匹配文件 仰角 方位角 距离 质心x y z
        /// </summary>
        public void exportMatchingFile()//导出匹配文件
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
                                         InRegionTrues[centers[j].matchNum].X + "\t" +
                                         InRegionTrues[centers[j].matchNum].Y + "\t" +
                                         InRegionTrues[centers[j].matchNum].Z );
                                    //truePointCloud.GetPoint(centers[j].matchNum)[0] + "\t" +
                                    //truePointCloud.GetPoint(centers[j].matchNum)[1] + "\t" +
                                    // truePointCloud.GetPoint(centers[j].matchNum)[2]);
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
            pathList.Clear();
            truePointPid = new int[1];
            //grouping = null;
            clusList = null;
            circles = null;
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
        /// 使用第三列对点进行过滤
        /// </summary>
        /// <param name="isEx">是否输出过滤后文件</param>
        public void ExcludePtsByDistance(bool isOutPut)
        {
            Tools.cleanDataByDistance(isOutPut, this.rawData, this.bit);
            this.ScanClustringToolStripMenuItem.Enabled = true;
            this.SourceClusteringToolStripMenuItem.Enabled = true;
            this.ExplainClusteringToolStripMenuItem.Enabled = true;
            ShowPointsFromFile(rawData, 1);
        }
        public void RejectPtsByDistanceFromFixed(bool isOutPut)
        {
            Tools.cleanDataByDistance2(isOutPut, this.clusList, this.bit);
            this.FixedPointMatchingToolStripMenuItem.Enabled = true;

        }
        /// <summary>
        /// 显示固定点数据
        /// </summary>
        /// <param name="type">1.显示全部 2.显示阈值内和野点 3.显示质心和正常点 4.只显示阈值内点</param>
        public void showFixPointData(int type)//显示固定点数据
        {
            vtkControl.GetRenderWindow().Clean();

            if (type == 1 || type == 2 || type == 4)
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
            for (int i = 0; i < clusList.Count; i++)
            {
                for (int j = 0; j < clusList[i].li.Count; j++)
                {
                    if (clusList[i].li[j].ifShown) {
                        if (type == 1 || type == 3)
                        {
                            pointCloud_1.InsertPoint(count_1, clusList[i].li[j].X, clusList[i].li[j].Y, clusList[i].li[j].Z);
                            count_1++;
                        }
                        else if ((type == 4) && (!clusList[i].li[j].isFilterByDistance))
                        {
                            pointCloud_1.InsertPoint(count_1, clusList[i].li[j].X, clusList[i].li[j].Y, clusList[i].li[j].Z);
                            count_1++;
                        }
                        else if (type == 2)
                        {
                            if (clusList[i].li[j].isFilterByDistance)
                            {
                                pointCloud_1.InsertPoint(count_1, clusList[i].li[j].X, clusList[i].li[j].Y, clusList[i].li[j].Z);
                                count_1++;
                            }
                            else
                            {
                                pointCloud_2.InsertPoint(count_2, clusList[i].li[j].X, clusList[i].li[j].Y, clusList[i].li[j].Z);
                                count_2++;
                            }
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
            if (count_2 != 0) polyVertex_2.GetPointIds().SetNumberOfIds(count_2);

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
            if (type == 1 || type == 4)
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
            else if (type == 2 || type == 3)
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
                }
                else if (type == 3)
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
            mcc.Left = 10;
            mcc.Show(this);
            this.toolStripStatusLabelCurrentPointCount.Text = String.Format("当前点云数：{0}，当前聚类数： {1}", pointSum, clusterSum);
        }
        /// <summary>
        /// 以外接圆半径过滤聚类点集-通过MCC调用
        /// </summary>
        /// <param name="radius"></param>
        public void FilterClustersByRadius(double radius)
        {
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
            showCircle(circles, 2, rawData, centers);
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
                try
                {

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
                Point3D ppp;
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
        /// <summary>
        /// 设置图例是否可见
        /// </summary>
        /// <param name="Visible">0.全部不可见 1.Distan过滤标注</param>
        /// <param name="Visible">2.分块聚类后野点核心点+外接圆 3.白色外接圆</param>
        /// <param name="Visible">4.黄白显示不同半径外接圆 5.固定点及其质心</param>
        /// <param name="Visible">6.左右对比质心和真值 7.匹配后显示真值和质心</param>
        public void isShowLegend(int Visible)//是否显示图例
        {
            if (Visible == 0)
            {//全都不显示
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
            else if (Visible == 1)
            {//按阈值过滤的图例
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\red_point.png");
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                //this.pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png"); 
                label1.Text = "被过滤点";
                label2.Text = "未过滤点";
            }
            else if (Visible == 2)
            {//聚类后显示红绿点图例+外接圆
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\red_point.png");
                this.pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png");
                this.pictureBox4.Image = Image.FromFile(Application.StartupPath + "\\white_circle.png");
                this.pictureBox4.Location = new Point(this.label3.Location.X + this.label3.Width + 10, this.pictureBox3.Location.Y);
                this.label4.Location = new Point(this.pictureBox4.Location.X + this.pictureBox4.Width - 10, this.pictureBox4.Location.Y + 10);
                label1.Text = "核心点";
                label2.Text = "野点";
                label3.Text = "聚类质心";
            }
            else if (Visible == 3)
            {//显示白色外接圆图例
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\white_circle.png");
                label1.Text = "聚类外接圆";
                label1.Location = new Point(this.pictureBox1.Location.X + this.pictureBox1.Width , this.pictureBox1.Location.Y + 10);
            }
            else if (Visible == 4)
            {//显示超过半径阈值和小于半径阈值的圆的图例
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\white_circle.png");
                label1.Text = "阈值半径" + "\n" + "内点集";
                label1.Location = new Point(this.pictureBox1.Location.X + this.pictureBox1.Width - 20, this.pictureBox1.Location.Y + 10);
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\yellow_circle.png");
                this.pictureBox2.Location = new Point(this.label1.Location.X + this.label1.Width - 10, this.pictureBox1.Location.Y);
                label2.Text = "阈值半径" + "\n" + "外点集";
                label2.Location = new Point(this.pictureBox2.Location.X + this.pictureBox2.Width - 20, this.pictureBox2.Location.Y + 10);
                this.label2.Visible = true;
                this.pictureBox2.Visible = true;
            }
            else if (Visible == 5)//显示固定点所有点和质心
            {
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\white_point.png");
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png");
                label1.Text = "数据点";
                label2.Text = "质心";
            }
            else if (Visible == 6)
            {
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\red_point.png");
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                this.pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\white_rectangle.png");
                label1.Text = "真值点";
                label2.Text = "质心点";
                label3.Text = "选择范围";
                label3.Location = new Point(this.pictureBox3.Location.X + this.pictureBox2.Width + 17, this.pictureBox3.Location.Y + 10);
            }
            else if (Visible == 7) {
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png");
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                label1.Text = "质心点";
                label2.Text = "真值点";
            }
            else if (Visible == 8)
            {
                this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\red_point.png");
                this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\green_point.png");
                this.pictureBox3.Image = Image.FromFile(Application.StartupPath + "\\blue_point.png");
                label1.Text = "已配对真值点质心点";
                label2.Text = "未配对质心点";
                label3.Text = "未配对真值点";
                pictureBox2.Location = new Point(this.label1.Location.X + this.label1.Width -10, this.pictureBox1.Location.Y);
                label2.Location = new Point(this.pictureBox2.Location.X + this.pictureBox2.Width -30, this.pictureBox3.Location.Y + 10);
                pictureBox3.Location = new Point(this.label2.Location.X + this.label2.Width -10, this.pictureBox2.Location.Y);
                label3.Location = new Point(this.pictureBox3.Location.X + this.pictureBox3.Width - 30, this.pictureBox3.Location.Y + 10);
            }
            this.label1.Visible = true;//默认显示一组
            this.pictureBox1.Visible = true;
            if (Visible == 1 || Visible == 4 || Visible == 5 || Visible ==7)//显示两个
            {
                this.label2.Visible = true;
                this.pictureBox2.Visible = true;
            }
            else if (Visible == 2)
            {//显示4个
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.label4.Visible = true;
                this.pictureBox2.Visible = true;
                this.pictureBox3.Visible = true;
                this.pictureBox4.Visible = true;
            }
            else if (Visible == 6 || Visible == 8)
            {
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.pictureBox2.Visible = true;
                this.pictureBox3.Visible = true;
            }
        }
        /// <summary>
        /// 导入真值点文件
        /// </summary>
        /// <param name="TruePath"></param>
        /// <param name="x_d"></param>
        /// <param name="y_d"></param>
        private void ImportTruePoint(String TruePath, int x_d, int y_d)
        {
            FileMap fileMap = new FileMap();
            List<string> pointsList = fileMap.ReadFile(TruePath);
            trues = new List<Point3D>();
            //truePolyVertex.GetPointIds().SetNumberOfIds(pointsList.Count);
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
                Point3D point = new Point3D(); ;
                switch (x_d)
                {
                    case 1:
                        point.X = pY;
                        break;
                    case 2:
                        point.X = pX;
                        break;
                    case 3:
                        point.X = -pY;
                        break;
                    case 4:
                        point.X = -pX;
                        break;
                }
                switch (y_d)
                {
                    case 1:
                        point.Y = pY;
                        break;
                    case 2:
                        point.Y = pX;
                        break;
                    case 3:
                        point.Y = -pY;
                        break;
                    case 4:
                        point.Y = -pX;
                        break;
                }
                point.Z = pZ;
                //truePointPid[0] = truePointCloud.InsertNextPoint(pX, pY, pZ);
                //truePointPid[0] = truePointCloud.InsertNextPoint(pX, pY, 0);
                trues.Add(point);
                //truePointVertices.InsertNextCell(1, truePointPid);
                //truePolyVertex.GetPointIds().SetId(i, i);
            }
        }
        private void DrawTruesAndCenters()
        {
            isSureRegion = false;
            showTruesAndCenters();//显示初步的真值点和质心点
            tmpAngle[0] = trueScale[0] - (trueScale[1] - trueScale[0]) / 20;//确定真值点初步四角
            tmpAngle[1] = trueScale[1] + (trueScale[1] - trueScale[0]) / 20;
            tmpAngle[2] = trueScale[2] - (trueScale[3] - trueScale[2]) / 20;
            tmpAngle[3] = trueScale[3] + (trueScale[3] - trueScale[2]) / 20;
            this.MoveStepTxt.Text = (int)((trueScale[1] - trueScale[0]) / 15) + "";
            showPtsInRegion();//显示范围内的点数
            showBounds(tmpAngle);//显示真值点初步边界
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
            cp = new Clustering();
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
            Console.WriteLine("\ncenters的数量 ：" + centers.Count);

            ImportTrueValuePoint trvp = new ImportTrueValuePoint();
            DialogResult rs = trvp.ShowDialog();
            if (rs == DialogResult.OK)
            {
                ImportTruePoint(trvp.selPath, trvp.xdir, trvp.ydir);
                Console.WriteLine("trues的数量：" + trues.Count);
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
                this.textBox3.Visible = true;
                this.MoveStepTxt.Visible = true;
                this.zoomRatioTxt.Visible = true;
                this.PtsInRegionTxt.Visible = true;
                this.SureRegionBtn.Visible = true;
                this.DoMatchBtn.Visible = true;
                isShowLegend(6);
                DrawTruesAndCenters();
            }
        }
        /// <summary>
        /// 导入固定点txt文件夹-菜单单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportTxtFixedPtsToolStripMenuItem_Click(object sender, EventArgs e)//导入固定点txt文件夹
        {
            //this.treeView1.Enabled = false;//暂时不可用
            ImportPts ip = new ImportPts();
            DialogResult rs = ip.ShowDialog();
            if (rs == DialogResult.OK)
            {
                this.isIgnoreDuplication = ip.isIgnoreDuplication;//是否忽略重复点
                this.selPath = ip.selPath;
                this.AddFolder(selPath, ip.xdir, ip.ydir, 2 + ((this.isIgnoreDuplication) ? 1 : 2), false);//3剔除重复 4不剔除重复
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
        /// 固定点同名点匹配并输出结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FixedPointMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixedPtsMatch_Export fpme = new FixedPtsMatch_Export();
            List<Point3D> FixedPtsTrueValueList;
            int matchCount = 0;
            if (fpme.ShowDialog() == DialogResult.OK)
            {
                FixedPtsTrueValueList = fpme.FixedPtsTrueValueList;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fpme.pathOut, false);//把cells分别按照聚类输出 ID需要合并 
                double xita, alpha;
                try
                {
                    foreach (ClusObj obj in clusList)
                    {
                        if (obj.visible)
                        {
                            Point3D tmpPoint = FixedPtsTrueValueList.Find(m => m.pointName == obj.clusName);
                            if (tmpPoint == null) continue;
                            matchCount++;
                            foreach (Point3D p in obj.li)
                            {
                                xita = (-2) * (p.motor_x - this.x_angle) / 180 * Math.PI;
                                alpha = 2 * (p.motor_y - this.y_angle) / 180 * Math.PI;
                                sw.WriteLine(obj.clusName + "\t" + xita + "\t" + alpha + "\t" + p.Distance + "\t" + tmpPoint.X + "\t" + tmpPoint.Y + "\t" + tmpPoint.Z);
                            }
                        }
                    }
                    MessageBox.Show("读入真值点" + FixedPtsTrueValueList.Count + "个，匹配了" + matchCount + "个固定点扫描文件。\n匹配文件储存在" + fpme.pathOut + "下。");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sw.Close();
                }
            }

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
            Tools.getClusterCenter(clusterSum, this.rawData, this.centers, this.clusList, null);
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
            Tools.getClusterCenter(dbb.clusterAmount, this.rawData, this.centers, this.clusList, null);//计算质心
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
                foreach (int jb in pathIdChecked)
                {
                    Console.WriteLine("PathId = " + jb);
                }
                if (isScaned)
                {
                    for (int p = 0; p < rawData.Count; p++)
                    {
                        if (!pathIdChecked.Contains(rawData[p].pathId))
                        {
                            rawData[p].ifShown = false;
                        }
                        else
                        {
                            rawData[p].ifShown = true;
                        }
                    }
                    ShowPointsFromFile(rawData, 1);
                }
                else
                {
                    
                    foreach (ClusObj ob in clusList)
                    {
                        if (!pathIdChecked.Contains(ob.li[0].pathId))
                        {
                            ob.visible = false;
                        }
                        else
                        {
                            ob.visible = true;
                        }
                        foreach (Point3D p in ob.li)
                        {
                            if (!pathIdChecked.Contains(p.pathId)) {
                                p.ifShown = false;
                            }
                            else
                            {
                                p.ifShown = true;
                            }
                        }
                    }
                    showFixPointData(1);
                }
                
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
            double[] dblEigenValue = new double[3] { 0, 0, 0 };
            Matrix mtxEigenVector = new Matrix(3, 3);
            int nMaxIt = 100;
            double eps = 0.0001;
            bool rs = Matrix.ComputeEvJacobi(m, dblEigenValue, mtxEigenVector, nMaxIt, eps);
            Console.WriteLine("\n" + "____________________________" + rs + "\t" + dblEigenValue[0] + "\t" + dblEigenValue[1] + "\t" + dblEigenValue[2]);
            Console.WriteLine(mtxEigenVector.ToString());
        }
        private void 测试ICpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point3D> sourceSet = new List<Point3D>();
            List<Point3D> dataSet = new List<Point3D>();
            Point3D point;

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
            Console.WriteLine("\n真实值：" + sourceSet.Count);
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
            Matrix R = Matrix.ZeroMatrix(3, 3);
            Matrix T = Matrix.ZeroMatrix(3, 1);
            double ee = 0.0001;
            icp.go_hell_ICP(sourceSet, dataSet, R, T, ee);
            Console.WriteLine(R.ToString());
            Console.WriteLine(T.ToString());
        }

        private void 测试图例ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isShowLegend(3);
        }
        public delegate void MessageBoxHand();
        private void 测试MessageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Invoke(new MessageBoxHand(delegate()
            {
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
            List<Point3D> cell = rawData.Take(2000).ToList();
            //MessageBox.Show(rawData.Count+"");
            double cell_x = cell.Max(m => m.X) - x_Min;
            double cell_y = cell.Max(m => m.Y) - y_Min;
            int rows = (int)((y_Max - y_Min) / cell_y) + 1;
            int cols = (int)((x_Max - x_Min) / cell_x) + 1;
            List<Point3D>[] cells = new List<Point3D>[rows * cols];
            cells[0] = cell;
            Console.WriteLine("rows : " + rows + "\tcols : " + cols);
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
            for (int i = 0; i < cells.Length; i++)
            {
                Console.Write(cells[i].Count + "\t");
                sum += cells[i].Count;
            }
            Console.WriteLine("\n 总点数 ： " + sum + "\t总分块数 ：" + cells.Length);
        }
        private void 测试多线程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ProcessScanPtsClustering();
        }
        private static void StartCode(object i)
        {
            List<Point3D> cell = i as List<Point3D>;
            DBImproved ThreadDB = new DBImproved();
            ThreadDB.dbscan(cell, MainForm.threhold, pointsInthrehold);
            sumPts += cell.Count;
            threadCount++;
            clusterSum += ThreadDB.clusterAmount;
            Console.WriteLine("完成第[" + threadCount + "]个线程，包含[" + cell.Count + "]个数据点，共["
                + ThreadDB.clusterAmount + "]个聚类，当前已聚类[" + clusterSum + "]个。");
            progressForm.setprogressvalue(threadCount);
            System.Windows.Forms.Application.DoEvents();  
        }
        private void 测试野点回调ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MergeCellData(200, 250);
        }
        private void 测试IndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String sss = "sdsf.231";
            Console.WriteLine(sss.IndexOf('.', 0, sss.Length - 1));
        }
        private void 测试状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile("C://1.gif");//加载Gif图片
            System.Drawing.Imaging.FrameDimension dim = new System.Drawing.Imaging.FrameDimension(img.FrameDimensionsList[0]);
            for (int i = 0; i < img.GetFrameCount(dim); i++)//遍历图像帧
            {
                img.SelectActiveFrame(dim, i);//激活当前帧
                for (int j = 0; j < img.PropertyIdList.Length; j++)//遍历帧属性
                {
                    if ((int)img.PropertyIdList.GetValue(j) == 0x5100)//.如果是延迟时间
                    {
                        System.Drawing.Imaging.PropertyItem pItem = (System.Drawing.Imaging.PropertyItem)img.PropertyItems.GetValue(j);//获取延迟时间属性
                        byte[] delayByte = new byte[4];//延迟时间，以1/100秒为单位
                        delayByte[0] = pItem.Value[i * 4];
                        delayByte[1] = pItem.Value[1 + i * 4];
                        delayByte[2] = pItem.Value[2 + i * 4];
                        delayByte[3] = pItem.Value[3 + i * 4];
                        int delay = BitConverter.ToInt32(delayByte, 0) * 10; //乘以10，获取到毫秒
                        MessageBox.Show(delay.ToString());//弹出消息框，显示该帧时长
                        break;
                    }
                }
            }
        }
        private void 测试深拷贝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point3D> a = new List<Point3D>();
            List<Point3D> b = new List<Point3D>();
            a.Add(new Point3D(1, 2, 3));
            a.Add(new Point3D(9, 8, 7));
            //a.ForEach(i => b.Add(i));
            a.ForEach(i => b.Add((Point3D)i.Clone()));
            foreach (Point3D p in b)
            {
                Console.WriteLine(p.X + "\t" + p.Y + "\t" + p.Z);
            }
            b[1].X = 4;
            b[1].Y = 5;
            b[1].Z = 6;
            foreach (Point3D p in a)
            {
                Console.WriteLine(p.X + "\t" + p.Y + "\t" + p.Z);
            }
        }

        private void 测试匹配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileMap fileMap = new FileMap();
            centers = new List<Point3D>();
            List<string> pointsList = fileMap.ReadFile("G:\\centers.txt");
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
                centers.Add(new Point3D(pX, pY, pZ));
                //truePolyVertex.GetPointIds().SetId(i, i);
            }
            Console.WriteLine("\ncenters的数量 ：" + centers.Count);

            ImportTrueValuePoint trvp = new ImportTrueValuePoint();
            DialogResult rs = trvp.ShowDialog();
            if (rs == DialogResult.OK)
            {
                ImportTruePoint(trvp.selPath, trvp.xdir, trvp.ydir);
                Console.WriteLine("trues的数量：" + trues.Count);
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
                this.textBox3.Visible = true;
                this.MoveStepTxt.Visible = true;
                this.zoomRatioTxt.Visible = true;
                this.PtsInRegionTxt.Visible = true;
                this.SureRegionBtn.Visible = true;
                this.DoMatchBtn.Visible = true;
                isShowLegend(6);
                测试画双点ToolStripMenuItem_Click(sender, e);
            }
        }

        private void showBounds(double[] angle)
        {//x_min x_max y_min y_max
            if (ren == null)
            {
                ren = new vtkRenderer();
                vtkControl.GetRenderWindow().Clean(); ;
            }
            vtkLineSource lineSource = new vtkLineSource();
            lineSource.SetPoint1(angle[0], angle[2], 0);
            lineSource.SetPoint2(angle[0], angle[3], 0);
            lineSource.Update();
            // Visualize
            vtkPolyDataMapper mapper = new vtkPolyDataMapper();
            mapper.SetInputConnection(lineSource.GetOutputPort());
            actorLine = new vtkActor();
            actorLine.SetMapper(mapper);
            actorLine.GetProperty().SetLineWidth(2);

            vtkLineSource lineSource2 = new vtkLineSource();
            lineSource2.SetPoint1(angle[1], angle[2], 0);
            lineSource2.SetPoint2(angle[1], angle[3], 0);
            lineSource2.Update();
            // Visualize
            vtkPolyDataMapper mapper2 = new vtkPolyDataMapper();
            mapper2.SetInputConnection(lineSource2.GetOutputPort());
            actorLine2 = new vtkActor();
            actorLine2.SetMapper(mapper2);
            actorLine2.GetProperty().SetLineWidth(2);

            vtkLineSource lineSource3 = new vtkLineSource();
            lineSource3.SetPoint1(angle[0], angle[2], 0);
            lineSource3.SetPoint2(angle[1], angle[2], 0);
            lineSource3.Update();
            // Visualize
            vtkPolyDataMapper mapper3 = new vtkPolyDataMapper();
            mapper3.SetInputConnection(lineSource3.GetOutputPort());
            actorLine3 = new vtkActor();
            actorLine3.SetMapper(mapper3);
            actorLine3.GetProperty().SetLineWidth(2);

            vtkLineSource lineSource4 = new vtkLineSource();
            lineSource4.SetPoint1(angle[0], angle[3], 0);
            lineSource4.SetPoint2(angle[1], angle[3], 0);
            lineSource4.Update();
            // Visualize
            vtkPolyDataMapper mapper4 = new vtkPolyDataMapper();
            mapper4.SetInputConnection(lineSource4.GetOutputPort());
            actorLine4 = new vtkActor();
            actorLine4.SetMapper(mapper4);
            actorLine4.GetProperty().SetLineWidth(2);

            ren.AddActor(actorLine);
            ren.AddActor(actorLine2);
            ren.AddActor(actorLine3);
            ren.AddActor(actorLine4);


            vtkControl.Refresh();
            //vtkControl.GetRenderWindow().Render();
        }

        private void 测试画矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showBounds(tmpAngle);
        }

        private void 测试删除actorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ren.RemoveActor(actorLine);
            //ren.RemoveActor(actorLine2);
            //ren.RemoveViewProp(actorLine3);
            //ren.RemoveActor(actorLine4);
            //tmpAngle[0] = tmpAngle[0] + 2;
            //tmpAngle[1] = tmpAngle[1] + 2;
            //showBounds(tmpAngle);
            vtkControl.Refresh();
            //vtkControl.GetRenderWindow().Render();
        }
        /// <summary>
        /// 显示真值点和质心点 
        /// 真值点初步【显示】采用文件输入的XY0 
        /// 质心点【显示】采用平衡至真值比例尺 同时平移至相近位置的坐标 tmpX' tmoyY' 0
        /// 质心更新tmpX tmpY为真值相近坐标，用该坐标进行【匹配】
        /// </summary>
        public void showTruesAndCenters()
        {
            ren = new vtkRenderer();
            vtkControl.GetRenderWindow().Clean();

            vtkPolyData truePolydata = new vtkPolyData(); ;//对真值处理
            vtkPoints truePoints = new vtkPoints();
            vtkCellArray trueCellArry = new vtkCellArray();

            vtkPolyData centerPolydata = new vtkPolyData();
            vtkPoints centerPoints = new vtkPoints();
            vtkCellArray centerCellArry = new vtkCellArray();

            int[] match_Pid = new int[1];
            centroidScale = new double[6];
            centroidScale[0] = centers.Min(m => m.X);
            centroidScale[1] = centers.Max(m => m.X);
            centroidScale[2] = centers.Min(m => m.Y);
            centroidScale[3] = centers.Max(m => m.Y);

            for (int i = 0; i < trues.Count; i++)
            {
                //match_Pid[0] = truePoints.InsertNextPoint(truePointCloud.GetPoint(i));
                match_Pid[0] = truePoints.InsertNextPoint(trues[i].X, trues[i].Y, 0);
                trueCellArry.InsertNextCell(1, match_Pid);
            }
            //centroidScale = centerPoints.GetBounds();
            trueScale = truePoints.GetBounds();
            Console.WriteLine("Center的范围" + centroidScale[0] + "\t" + centroidScale[1] + "\t" + centroidScale[2] + "\t" + centroidScale[3]);
            Console.WriteLine("Center的范围" + trueScale[0] + "\t" + trueScale[1] + "\t" + trueScale[2] + "\t" + trueScale[3]);
            scale[0] = (trueScale[1] - trueScale[0])
                    / (centroidScale[1] - centroidScale[0]);
            scale[1] = (trueScale[3] - trueScale[2])
                    / (centroidScale[3] - centroidScale[2]);
            Console.WriteLine(scale[0] + "\t" + scale[1]);

            foreach (Point3D p in centers)
            {
                p.tmp_X = p.X * scale[0];//記錄改變比例尺后的坐標
                p.tmp_Y = p.Y * scale[1];
            }

            double xmin = centers.Min(m => m.tmp_X);
            double xmax = centers.Max(m => m.tmp_X);
            double ymin = centers.Min(m => m.tmp_Y);
            double ymax = centers.Max(m => m.tmp_Y);

            for (int i = 0; i < centers.Count; i++)
            {
                match_Pid[0] = centerPoints.InsertNextPoint(centers[i].tmp_X - (xmin - trueScale[0]) + (xmax - xmin) * 1.1,
                    centers[i].tmp_Y - (ymin - trueScale[2]), 0);//這裡是顯示坐標系 不寫入屬性中
                centerCellArry.InsertNextCell(1, match_Pid);
            }

            truePolydata.SetPoints(truePoints); //把点导入的polydata中去
            truePolydata.SetVerts(trueCellArry);

            centerPolydata.SetPoints(centerPoints);
            centerPolydata.SetVerts(centerCellArry);

            //Mapper
            vtkPolyDataMapper TrueMapper = new vtkPolyDataMapper();
            TrueMapper.SetInputConnection(truePolydata.GetProducerPort());
            vtkPolyDataMapper CenterMapper = new vtkPolyDataMapper();
            CenterMapper.SetInputConnection(centerPolydata.GetProducerPort());

            trueActor = new vtkActor();
            vtkActor centerActor = new vtkActor();
            trueActor.SetMapper(TrueMapper);
            trueActor.GetProperty().SetColor(1, 0, 0);
            trueActor.GetProperty().SetPointSize(5);
            centerActor.SetMapper(CenterMapper);
            centerActor.GetProperty().SetColor(0, 1, 0);
            centerActor.GetProperty().SetPointSize(5);

            ren.AddActor(trueActor);
            ren.AddActor(centerActor);

            vtkControl.GetRenderWindow().AddRenderer(ren);//将渲染加入控制controler 否则界面不会自动刷新
            showAxes();
            vtkControl.Refresh();
        }

        private void 测试画双点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawTruesAndCenters();
        }

        /// <summary>
        /// 重写覆盖原始的按键绑定时间 通过上下左右 pageup pagedown来指导缩放和移动
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="keyData">键值</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)//取消方向键对控件的焦点的控件，用自己自定义的函数处理各个方向键的处理函数
        {
            if (isSureRegion) return base.ProcessCmdKey(ref msg, keyData);
            double zoom = 0, ste = 0;
            if ((keyData == Keys.Up) || (keyData == Keys.Down)
                || (keyData == Keys.Left) || (keyData == Keys.Right)
                || (keyData == Keys.PageUp) || (keyData == Keys.Next))
            {
                ren.RemoveActor(actorLine);
                ren.RemoveActor(actorLine2);
                ren.RemoveViewProp(actorLine3);
                ren.RemoveActor(actorLine4);
                if (!double.TryParse(this.MoveStepTxt.Text, out ste))
                {
                    MessageBox.Show("输入的文件格式有误，请重新输入");
                    return true;
                }
                if (!double.TryParse(this.zoomRatioTxt.Text, out zoom))
                {
                    MessageBox.Show("输入的文件格式有误，请重新输入");
                    return true;
                }
            }
            switch (keyData)
            {
                case Keys.Up:
                    tmpAngle[2] = tmpAngle[2] + Convert.ToDouble(ste);
                    tmpAngle[3] = tmpAngle[3] + Convert.ToDouble(ste);
                    Console.WriteLine(tmpAngle[0] + "\t" + tmpAngle[1] + "\t" + tmpAngle[2] + "\t" + tmpAngle[3] + "\t");
                    showBounds(tmpAngle);
                    vtkControl.Refresh();
                    showPtsInRegion();
                    return true;
                case Keys.Down:
                    tmpAngle[2] = tmpAngle[2] - Convert.ToDouble(ste);
                    tmpAngle[3] = tmpAngle[3] - Convert.ToDouble(ste);
                    Console.WriteLine(tmpAngle[0] + "\t" + tmpAngle[1] + "\t" + tmpAngle[2] + "\t" + tmpAngle[3] + "\t");
                    showBounds(tmpAngle);
                    vtkControl.Refresh();
                    showPtsInRegion();
                    return true;
                case Keys.Left:
                    tmpAngle[0] = tmpAngle[0] - Convert.ToDouble(ste);
                    tmpAngle[1] = tmpAngle[1] - Convert.ToDouble(ste);
                    Console.WriteLine(tmpAngle[0] + "\t" + tmpAngle[1] + "\t" + tmpAngle[2] + "\t" + tmpAngle[3] + "\t");
                    showBounds(tmpAngle);
                    showPtsInRegion();
                    vtkControl.Refresh();
                    return true;
                case Keys.Right:
                    tmpAngle[0] = tmpAngle[0] + Convert.ToDouble(ste);
                    tmpAngle[1] = tmpAngle[1] + Convert.ToDouble(ste);
                    Console.WriteLine(tmpAngle[0] + "\t" + tmpAngle[1] + "\t" + tmpAngle[2] + "\t" + tmpAngle[3] + "\t");
                    showBounds(tmpAngle);
                    showPtsInRegion();
                    vtkControl.Refresh();
                    return true;
                case Keys.PageUp:
                    tmpAngle[1] = tmpAngle[0] + (tmpAngle[1] - tmpAngle[0]) / Convert.ToDouble(zoom);
                    tmpAngle[3] = tmpAngle[2] + (tmpAngle[3] - tmpAngle[2]) / Convert.ToDouble(zoom);
                    Console.WriteLine(tmpAngle[0] + "\t" + tmpAngle[1] + "\t" + tmpAngle[2] + "\t" + tmpAngle[3] + "\t");
                    showBounds(tmpAngle);
                    showPtsInRegion();
                    vtkControl.Refresh();
                    return true;
                case Keys.Next:
                    tmpAngle[1] = tmpAngle[0] + (tmpAngle[1] - tmpAngle[0]) * Convert.ToDouble(zoom);
                    tmpAngle[3] = tmpAngle[2] + (tmpAngle[3] - tmpAngle[2]) * Convert.ToDouble(zoom);
                    Console.WriteLine(tmpAngle[0] + "\t" + tmpAngle[1] + "\t" + tmpAngle[2] + "\t" + tmpAngle[3] + "\t");
                    showBounds(tmpAngle);
                    showPtsInRegion();
                    vtkControl.Refresh();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// 显示所选范围内的点数
        /// </summary>
        private void showPtsInRegion()
        {
            InRegionTrues = new List<Point3D>();
            foreach (Point3D p in trues)
            {
                if ((p.X <= tmpAngle[1]) && (p.X >= tmpAngle[0]) && (p.Y <= tmpAngle[3]) && (p.Y >= tmpAngle[2]))
                {
                    InRegionTrues.Add(p);
                }
            }
            this.PtsInRegionTxt.Text = InRegionTrues.Count + "";
        }
        private void SureRegionBtn_Click(object sender, EventArgs e)
        {
            isSureRegion = true;//確認了就不相應函數了
            ren.RemoveActor(trueActor);
            ren.RemoveActor(actorLine);
            ren.RemoveActor(actorLine2);
            ren.RemoveViewProp(actorLine3);
            ren.RemoveActor(actorLine4);
            vtkPolyData truePolydata = new vtkPolyData(); ;//对真值处理
            truePointCloud = new vtkPoints();//刷新真值点云
            //vtkCellArray trueCellArry = new vtkCellArray();

            double x_tmp_min = InRegionTrues.Min(m => m.X);
            double x_tmp_max = InRegionTrues.Max(m => m.X);
            double y_tmp_min = InRegionTrues.Min(m => m.Y);
            double y_tmp_max = InRegionTrues.Max(m => m.Y);

            double scale_x = (trueScale[1] - trueScale[0]) / (x_tmp_max - x_tmp_min);
            double scale_y = (trueScale[3] - trueScale[2]) / (y_tmp_max - y_tmp_min);
            Console.WriteLine("" + scale_x + "\t" + scale_y);

            int[] truePointPid = new int[1];
            truePolyVertex.GetPointIds().SetNumberOfIds(InRegionTrues.Count);

            foreach (Point3D p in InRegionTrues)
            {
                //centerPoints.InsertNextPoint(centers[i].tmp_X - (xmin - trueScale[0]) + (xmax - xmin) * 1.1,
                //    centers[i].tmp_Y - (ymin - trueScale[2]), 0);
                p.tmp_X = trueScale[0] + (p.X - x_tmp_min) * scale_x;//轉換比例尺 并計入屬性
                p.tmp_Y = trueScale[2] + (p.Y - y_tmp_min) * scale_y;
                truePointPid[0] = truePointCloud.InsertNextPoint(p.tmp_X, p.tmp_Y, 0);
                truePointVertices.InsertNextCell(1, truePointPid);
            }
            trueScale = truePointCloud.GetBounds();

            truePolydata.SetPoints(truePointCloud); //把点导入的polydata中去
            truePolydata.SetVerts(truePointVertices);

            //Mapper
            vtkPolyDataMapper TrueMapper = new vtkPolyDataMapper();
            TrueMapper.SetInputConnection(truePolydata.GetProducerPort());

            trueActor = new vtkActor();
            trueActor.SetMapper(TrueMapper);
            trueActor.GetProperty().SetColor(1, 0, 0);
            trueActor.GetProperty().SetPointSize(5);
            ren.AddActor(trueActor);

            vtkControl.Refresh();
        }

        private void DoMatchBtn_Click(object sender, EventArgs e)
        {
            isShowLegend(0);
            this.textBox1.Visible = false;
            this.textBox2.Visible = false;
            this.textBox3.Visible = false;
            this.MoveStepTxt.Visible = false;
            this.zoomRatioTxt.Visible = false;
            this.PtsInRegionTxt.Visible = false;
            this.SureRegionBtn.Visible = false;
            this.DoMatchBtn.Visible = false;
            progressForm = new WaitingForm();
            progressForm.TopMost = false;
            progressForm.StartPosition = FormStartPosition.CenterParent;
            bkWorker.RunWorkerAsync();
            progressForm.ShowDialog();
        }
        public void calMatchedCoords(){
            for (int j = 0; j < centers.Count; j++)
            {
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
            }
        }
        public void RecorrectMatchingPtsByDistance(double matchDistance,
                bool isShowUnmatchedCenterPts,bool isShowUnmatchedTruePts) {
            int countMatched = 0;
            matchedID = new List<int>();
            double center2True = 0.0;
            double ddd = 0.0;
            for (int j = 0; j < centers.Count; j++)
            {
                int matchedId = 0;
                centers[j].isMatched = false;
                center2True = getDisP(truePointCloud.GetPoint(0), centers[j]);//设一个距离初值
                for (int i = 0; i < truePointCloud.GetNumberOfPoints(); i++)
                {
                    ddd = getDisP(truePointCloud.GetPoint(i), centers[j]);//找最小值
                    if (ddd < center2True)
                    {
                        center2True = ddd;
                        matchedId = i;
                    }
                }
                if (center2True < matchDistance)
                {
                    centers[j].isMatched = true;
                    centers[j].matchNum = matchedId;//这个ID是truepointcloud的下标
                    matchedID.Add(matchedId);
                    countMatched++;
                }
            }
            this.toolStripStatusLabelCurrentPointCount.Text = "总共" + centers.Count + "个聚类质心，总共" + truePointCloud.GetNumberOfPoints() + "个真值点，匹配" + countMatched + "个点";
            showMatchedLine(isShowUnmatchedCenterPts,isShowUnmatchedTruePts);
        }


        private void 测试清屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ren.RemoveAllViewProps();
            vtkControl.Refresh();
        }

        private void 测试添加actorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ren.AddActor(actorLine);
            vtkControl.Refresh();
        }
    }
}

