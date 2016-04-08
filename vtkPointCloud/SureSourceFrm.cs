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
    public partial class SureSourceFrm : Form
    {   
        public string strCentroid;
        public string strMatching;
        public SureSourceFrm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {         
            strCentroid = this.textBox1.Text;
            strMatching = this.textBox2.Text;
            if (strMatching.Equals("") || strCentroid.Equals("")) {
                MessageBox.Show("有未输入路径项目");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {                
                    this.textBox1.Text = saveFile1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.FilterIndex = 1;
            
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                this.textBox2.Text = saveFile1.FileName;
            }
        }
    }
}
