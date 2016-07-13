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
    public partial class MCC : Form
    {
        public int ptsRb = 0;
        //public double Threshold = 0.0;
        private bool isFirstIn = true;
        MainForm mf;
        SaveFileDialog saveFile1 = new SaveFileDialog();
        public MCC()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (isFirstIn) {
                if (MessageBox.Show("确认不使用半径过滤吗?", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                else {
                    return;
                }
            }
            if (this.outpath1txt.Text == "未选择路径" && this.checkBox1.Checked)
            {
                MessageBox.Show("尚未选择聚类输出路径");
                return;
            }
            if (this.outpath2txt.Text == "未选择路径" && this.checkBox2.Checked)
            {
                MessageBox.Show("尚未选择质心输出路径");
                return;
            }
            Tools.removeFilterPointFromClustering(mf.rawData, mf.filterID);//清除属于大半径的数据点
            Tools.removeFilterPointFromClustering(mf.centers, mf.filterID);//清除属于大半径的质心点
            mf.ShowPointsFromFile(mf.rawData, 1);
            mf.ExplainClusteringToolStripMenuItem.Enabled = false;
            mf.iCPToolStripMenuItem.Enabled = true;
            mf.isShowLegend(0);
            if (this.checkBox1.Checked) Tools.exportClustersCenterFile(mf.centers, mf.bit, mf.x_angle, mf.y_angle, this.outpath2txt.Text);//输出质心
            if (this.checkBox2.Checked) Tools.exportClustersPointsFile(mf.clusList, mf.bit, mf.x_angle, mf.y_angle, this.outpath1txt.Text);//输出聚类

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            double Threshold;
            if (!double.TryParse(this.threshold_txtbox.Text, out Threshold))
            {
                MessageBox.Show("输入的不是浮点数，请重新输入");
                return;
            }
            mf.FilterClustersByRadius(Threshold);
            if (isFirstIn)
            {
                mf.isShowLegend(4);
                isFirstIn = false;
            }   
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.out1btn.Enabled = this.checkBox1.Checked;
            this.outpath1txt.Enabled = this.checkBox1.Checked;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.out2btn.Enabled = this.checkBox2.Checked;
            this.outpath2txt.Enabled = this.checkBox2.Checked;
        }
        private void out1btn_Click(object sender, EventArgs e)
        {
            
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.Title = "输出聚类文件";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == DialogResult.OK) {
                this.outpath1txt.Text = saveFile1.FileName;
            }
        }

        private void out2btn_Click(object sender, EventArgs e)
        {
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.Title = "输出质心文件";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == DialogResult.OK)
            {
                this.outpath2txt.Text = saveFile1.FileName;
            }
        }


    }
}
