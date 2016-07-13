using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace vtkPointCloud
{
    public partial class FixedPtsMatch_Export : Form
    {
        public List<Point3D> FixedPtsTrueValueList;
        public string pathOut;
        public FixedPtsMatch_Export()
        {
            InitializeComponent();
        }
        private void pathSelbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter += "点云数据(*.txt)|*.txt";
            openFile.Title = "打开固定点真值文件";
            String fullFilePath;
            if (openFile.ShowDialog() == DialogResult.OK && openFile.FileName.Length > 1)
            {
               
                FixedPtsTrueValueList = new List<Point3D>();
                fullFilePath = openFile.FileName;
                this.PathSeltxt.Text = fullFilePath;
                //获得文件路径
                int index = fullFilePath.LastIndexOf("\\");
                string filePath = fullFilePath.Substring(0, index);
                //获得文件名称
                string fileName = fullFilePath.Substring(index + 1);

                FileMap fileMap = new FileMap();
                try
                {
                    List<string> pointsList = fileMap.ReadFile(fullFilePath);
                Point3D ppp;
                double pX, pY, pZ;
                for (int i = 0; i < pointsList.Count; i++)
                {
                    if ((pointsList[i] == null) || (pointsList[i] == "")) continue;
                    string[] tmpxyz = pointsList[i].Split(',');
                    if (!double.TryParse(tmpxyz[1], out pX))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    if (!double.TryParse(tmpxyz[2], out pY))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    if (!double.TryParse(tmpxyz[3], out pZ))
                    {
                        MessageBox.Show("输入的文件格式有误，请重新输入");
                        return;
                    }
                    ppp = new Point3D();
                    ppp.pointName = tmpxyz[0];
                    ppp.X = pX;
                    ppp.Y = pY;
                    ppp.Z = pZ;
                    ppp.ifShown = true;
                    FixedPtsTrueValueList.Add(ppp);
                }
                }
                catch (Exception)
                {
                    MessageBox.Show("读取真值文件出错");
                    throw;
                }
            }
        }
        private void pathSel2Btn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                this.PathSeltxt2.Text = saveFile1.FileName;
            }
        }
        private void SureBtn_Click(object sender, EventArgs e)
        {
            if (this.PathSeltxt.Text.Equals("未选择路径"))
            {
                MessageBox.Show("未输入真值文件路径");
                return;
            } if (this.PathSeltxt2.Text.Equals("未选择路径"))
            {
                MessageBox.Show("未输入匹配文件保存路径");
                return;
            }
            this.pathOut = this.PathSeltxt2.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FixedPtsMatch_Export_Load(object sender, EventArgs e)
        {

        }
    }
}
