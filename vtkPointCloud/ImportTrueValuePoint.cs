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
    public partial class ImportTrueValuePoint : Form
    {
        public string selPath = "";
        public int xdir = 2;
        public int ydir = 1;
        public ImportTrueValuePoint()
        {
            InitializeComponent();
            System.Drawing.Image img = System.Drawing.Image.FromFile("arrowcross.png");
            System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp;
            img.Dispose();
        }
        private void pathSelbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter += "点云数据(*.txt)|*.txt";
            openFile.Title = "打开文件";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(openFile.FileName);
                this.PathSeltxt.Text = openFile.FileName;
            }
        }

        private void Sure_button_Click(object sender, EventArgs e)
        {
            if (this.PathSeltxt.Text.Equals("未选择路径"))
            {
                MessageBox.Show("未选择真值文件路径");
                return;
            }
            if (shang_rb1.Checked) xdir = 1;
            else if (you_rb1.Checked) xdir = 2;
            else if (xia_rb1.Checked) xdir = 3;
            else if (zuo_rb1.Checked) xdir = 4;

            if (shang_rb2.Checked) ydir = 1;
            else if (you_rb2.Checked) ydir = 2;
            else if (xia_rb2.Checked) ydir = 3;
            else if (zuo_rb2.Checked) ydir = 4;
            this.selPath = this.PathSeltxt.Text;
            if ((xdir + ydir) % 2 == 0)
            {
                MessageBox.Show("坐标选择有误");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
