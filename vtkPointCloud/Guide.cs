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
    public partial class Guide : Form
    {
        public Guide()
        {
            InitializeComponent();
            this.GuideTextBox.Text = "单击鼠标左键:控制绕焦点旋转 \r\n滚轮滑动（拖动鼠标右键）：控制缩放\r\n"+
                "滚轮拖动：控制图像平移\r\nC键：" +
                "\r\n    Camera视角 鼠标点击影像摄影机的角点和方位\r\nA键：" +
                "\r\n    Actor视角:鼠标点击物体才会动 点击背景不会动\r\nJ键" +
                "\r\n    joystick（位置敏感）模式：鼠标左键按下就旋转\r\nT键" +
                "\r\n    trackball(动作敏感模式)模式：鼠标拖动才旋转\r\nE键"+
                "\r\n    退出应用";
        }
    }
}
