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
    public partial class RowColFrm : Form
    {
        public int Rows=0;
        public int Cols = 0;
        public RowColFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(this.textBox2.Text, out this.Rows))
                {
                    MessageBox.Show("行不是整数");
                    return;
                }
            if (!int.TryParse(this.textBox3.Text, out this.Cols))
            {
                    MessageBox.Show("列不是整数");
                    return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
