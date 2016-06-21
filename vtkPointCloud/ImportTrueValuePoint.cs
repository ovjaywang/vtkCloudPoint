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
        public int ptsRb = 0;
        public string selPath = "";
        public int xdir = 2;
        public int ydir = 1;
        public double x_angle=0.0,y_angle=0.0;
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
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = @"Choose folder path";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(dlg.SelectedPath);
                this.PathSeltxt.Text = dlg.SelectedPath;
                this.selPath = dlg.SelectedPath;
            }
        }
    }
}
