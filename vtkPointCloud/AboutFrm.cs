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
    public partial class AboutFrm : Form
    {
        public AboutFrm()
        {
            InitializeComponent();
            this.aboutTxtBox.Text = "            作者：王建阳\r\n\r\n       中国科学院遥感与数字地球所   \r\n\r\nvtkPointCloud点云显示聚类匹配软件 V2.00";
        }
    }
}
