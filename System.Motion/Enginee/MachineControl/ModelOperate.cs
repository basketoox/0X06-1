using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Motion.Enginee
{
    public partial class ModelOperate : UserControl
    {
        public ModelOperate()
        {
            InitializeComponent();
        }
        public StationInitialize StationIni { get; set; }
        public StationOperate StationOpe { get; set; }

        #region 操作
        private void btnSingleStart_MouseDown(object sender, MouseEventArgs e)
        {
            StationOpe.Start = true;
        }

        private void btnSingleStart_MouseUp(object sender, MouseEventArgs e)
        {
            StationOpe.Start = false;
        }

        private void btnJogStart_MouseDown(object sender, MouseEventArgs e)
        {
            StationOpe.JogStart = true;
            btnJogStart.BackColor = Color.Green;
        }

        private void btnJogStart_MouseUp(object sender, MouseEventArgs e)
        {
            StationOpe.JogStart = false;
            btnJogStart.BackColor = Color.Transparent;
        }

        private void btnPause_MouseDown(object sender, MouseEventArgs e)
        {
            StationOpe.Stop = true;
            btnPause.BackColor = Color.Green;
        }

        private void btnPause_MouseUp(object sender, MouseEventArgs e)
        {
            StationOpe.Stop = false;
            btnPause.BackColor = Color.Transparent;
        }

        private void btnStop_MouseDown(object sender, MouseEventArgs e)
        {
            if (StationOpe.Running)
            {
                StationOpe.Stop = true;
                StationOpe.Reset = true;
            }
            if (StationIni.Running) StationIni.Estop = true;
            btnStop.BackColor = Color.Green;
        }

        private void btnStop_MouseUp(object sender, MouseEventArgs e)
        {
            StationOpe.Stop = false;
            StationOpe.Reset = false;
            StationIni.Estop = false;
            btnStop.BackColor = Color.Transparent;
        }

        private void btnSpliceReset_MouseDown(object sender, MouseEventArgs e)
        {
            StationIni.Start = true;
        }

        private void btnSpliceReset_MouseUp(object sender, MouseEventArgs e)
        {
            StationIni.Start = false;
        }
        #endregion

        public void Refreshing()
        {
            lblInitializeStatus.Text = StationIni.Status.ToString();
            lblInitializeStatus.ForeColor = InitializeStatusColor(StationIni.Status);
            if (StationIni.Running) btnSpliceReset.BackColor = Color.Green;
            else btnSpliceReset.BackColor = Color.Transparent;
            lblOperateStatus.Text = StationOpe.Status.ToString();
            lblOperateStatus.ForeColor = StationStatusColor(StationOpe.Status);
            if (StationOpe.SingleRunning) btnSingleStart.BackColor = Color.Green;
            else btnSingleStart.BackColor = Color.Transparent;
        }
        private Color InitializeStatusColor(InilitiazeStatus status)
        {
            switch (status)
            {
                case InilitiazeStatus.初始化故障:
                    return Color.Red;
                case InilitiazeStatus.初始化未准备好:
                    return Color.Orange;
                case InilitiazeStatus.初始化完成:
                    return Color.Blue;
                case InilitiazeStatus.初始化运行中:
                    return Color.Green;
                case InilitiazeStatus.初始化暂停中:
                    return Color.Blue;
                default:
                    return Color.Red;
            }
        }
        private Color StationStatusColor(StationStatus status)
        {
            switch (status)
            {
                case StationStatus.模组报警:
                    return Color.Red;
                case StationStatus.模组未准备好:
                    return Color.Orange;
                case StationStatus.模组准备好:
                    return Color.Blue;
                case StationStatus.模组运行中:
                    return Color.Green;
                case StationStatus.模组暂停中:
                    return Color.Purple;
                default:
                    return Color.Red;
            }
        }
    }
}
