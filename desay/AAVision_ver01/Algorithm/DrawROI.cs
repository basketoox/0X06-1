using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;

namespace desay.AAVision.Algorithm
{
     class DrawROI
    {
        public HTuple row = new HTuple(0.0);
        public HTuple column = new HTuple(0.0);
        public HTuple radius = new HTuple(0.0);
        public HObject circle = null;
        public void DrawCricle(HWindow window)
        {
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");
            HOperatorSet.DrawCircle(window, out row, out column, out radius);
            HOperatorSet.GenCircle(out circle, row, column, radius);
            HOperatorSet.DispObj(circle, window);
        }
    }
}
