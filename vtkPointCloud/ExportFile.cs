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
    public partial class ExportFile : Form
    {
        SaveFileDialog saveFile1 = new SaveFileDialog();
        MainForm mf;
        public ExportFile()
        {
            InitializeComponent();
        }

        private void out1btn_Click(object sender, EventArgs e)
        {
            showSaveFileDialog(this.outpath1txt, "聚类");
        }

        private void out2btn_Click(object sender, EventArgs e)
        {
            showSaveFileDialog(this.outpath2txt, "质心");
        }

        private void out3btn_Click(object sender, EventArgs e)
        {
            showSaveFileDialog(this.outpath3txt, "匹配");
        }
        private void showSaveFileDialog(TextBox tb, String ss)
        {
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.Title = "输出" + ss + "文件";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == DialogResult.OK)
            {
                tb.Text = saveFile1.FileName;
            }
        }

        private void SureBtn_Click(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (this.outpath1txt.Text == "未选择路径" && this.checkBox1.Checked)
            {
                MessageBox.Show("尚未选择聚类文件输出路径！");
                return;
            }
            if (this.outpath2txt.Text == "未选择路径" && this.checkBox2.Checked)
            {
                MessageBox.Show("尚未选择质心文件输出路径！");
                return;
            }
            if (this.outpath3txt.Text == "未选择路径" && this.checkBox2.Checked)
            {
                MessageBox.Show("尚未选择匹配文件输出路径！");
                return;
            }
            if (this.checkBox1.Checked) {//输出聚类
                System.IO.StreamWriter sw = new System.IO.StreamWriter(this.outpath1txt.Text, false);
                try
                {
                    foreach (ClusObj oj in mf.clusList)
                    {
                            foreach (Point3D p3 in oj.li)
                            {
                                sw.WriteLine(oj.clusId+"\t"+p3.motor_x + "\t" + p3.motor_y + "\t" + p3.Distance);
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
            if (this.checkBox2.Checked) {//输出质心
                System.IO.StreamWriter sw = new System.IO.StreamWriter(this.outpath2txt.Text, false);
                try
                {
                    foreach (ClusObj oj in mf.clusList)
                    {
                        sw.WriteLine(oj.li.Average(i => i.motor_x) + "\t" + oj.li.Average(i => i.motor_y) + "\t" + oj.li.Average(i => i.Distance));
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
            if (this.checkBox3.Checked)//输出匹配
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(this.outpath3txt.Text, false);
                double yangjiao, fangweijiao;
                try
                {
                    foreach (ClusObj oj in mf.clusList)
                    {
                        foreach (Point3D p3 in oj.li)
                        {
                            yangjiao = (-2) * (p3.motor_x - mf.x_angle) / 180 * Math.PI;
                            fangweijiao = 2 * (p3.motor_y - mf.y_angle) / 180 * Math.PI;
                            sw.WriteLine(yangjiao + "\t" + fangweijiao + "\t" + p3.Distance + "\t" + mf.trues[oj.clusId - 1].X
                                + "\t" + mf.trues[oj.clusId - 1].Y + "\t" + mf.trues[oj.clusId - 1].Z + "\t");
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认不输出文件吗?", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                return;
            }
        }

    }
}
