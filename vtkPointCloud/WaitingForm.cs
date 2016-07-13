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
    public partial class  WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();

        }
        public void setprogressvalue(int value)
        {
            this.progressBar1.Value = value;
            //this.textbox2.text = "progress :" + value.tostring() + "%";
            //if (value == this.progressBar1.Maximum - 1) this.Close();
        }  

    }
}
