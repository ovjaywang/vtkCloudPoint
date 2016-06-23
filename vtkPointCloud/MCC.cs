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
            mf.SureFilterByRadius(this.checkBox1.Checked);
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
    }
}
