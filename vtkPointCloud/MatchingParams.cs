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
    public partial class MatchingParams : Form
    {
        MainForm mf;
        public double matchingParams;
        bool isAmend = false;//判断是否通过阈值调整过
        public MatchingParams()
        {
            InitializeComponent();
        }
        private void SureMatchingParams_Click(object sender, EventArgs e)
        {
            if (this.ExportMatchedPtsCheckBox.Checked)
            {
                mf = (MainForm)this.Owner;
                mf.exportMatchingFile();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void CancleMatchingParams_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void AdjustMatchDistance_Click(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            if (!double.TryParse(this.textBox1.Text, out this.matchingParams))
            {
                MessageBox.Show("输入的不是整数，请重新输入");
                return;
            }
            if (!isAmend) {
                isAmend = true;
                mf.isShowLegend(0);
                mf.isShowLegend(8);
                this.IsShowUnmatchedCenterPtsCheckBox.Enabled = true;
                this.IsShowUnmatchedTruePtsCheckBox.Enabled = true;
                this.SureMatchingParams.Enabled = true;
            }
            mf.RecorrectMatchingPtsByDistance(this.matchingParams,
                this.IsShowUnmatchedCenterPtsCheckBox.Checked,this.IsShowUnmatchedTruePtsCheckBox.Checked);
        }

        private void IsShowUnmatchedPtsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            mf.showMatchedLine(this.IsShowUnmatchedCenterPtsCheckBox.Checked, this.IsShowUnmatchedTruePtsCheckBox.Checked);
        }

        private void IsShowUnmatchedTruePtsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mf = (MainForm)this.Owner;
            mf.showMatchedLine(this.IsShowUnmatchedCenterPtsCheckBox.Checked, this.IsShowUnmatchedTruePtsCheckBox.Checked);
        }
    }
}
