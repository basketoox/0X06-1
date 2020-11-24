using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using desay.ProductData;
namespace desay
{
    class DrawROI
    {
        public string FilePath = @"./Resources";
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
            HOperatorSet.WriteRegion(circle, AppConfig.VisonPath + "\\RegionROI.hobj");
        }
        public void DrawRectangleROI(HWindow window, string fileName)
        {
            HObject region;
            HTuple row1 = null, column1 = null, phi = null, Length1 = null, Length2 = null;
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");
            HOperatorSet.DrawRectangle2(window, out row1, out column1, out phi, out Length1, out Length2);
            HOperatorSet.GenRectangle2(out region, row1, column1, phi, Length1, Length2);
            HOperatorSet.DispObj(region, window);
            DialogResult Result = MessageBox.Show("是否保存ROI！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {

                //if (Directory.Exists(FilePath) == false)
                //    Directory.CreateDirectory(FilePath);
                HOperatorSet.WriteRegion(region, fileName);
                MessageBox.Show("已保存文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return;
            }
        }
        public void DrawCircleRing(HWindow window, string fileName)
        {
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");
            HOperatorSet.DrawCircle(window, out row, out column, out radius);
            HOperatorSet.GenCircle(out circle, row, column, radius);
            HOperatorSet.DispObj(circle, window);
            DialogResult Result = MessageBox.Show("是否保存ROI！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                //if (Directory.Exists(FilePath) == false)
                //    Directory.CreateDirectory(FilePath);
                //HOperatorSet.WriteRegion(circle, FilePath + @"/" + fileName);
                HOperatorSet.WriteRegion(circle, fileName);
                MessageBox.Show("已保存文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return;
            }
        }
        public void DrawCircleRingROI(HWindow window, string fileName)
        {
            HObject ho_Circle, ho_Circle2, RingRegion;
            HTuple hv_Row2 = null, hv_Column3 = null, hv_Radius3 = null;
            HTuple hv_Row3 = null, hv_Column4 = null, hv_Radius4 = null;
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out RingRegion);

            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");
            MessageBox.Show("设定外边界", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            HOperatorSet.DrawCircle(window, out hv_Row2, out hv_Column3,
                                    out hv_Radius3);
            MessageBox.Show("设定内边界！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            HOperatorSet.DrawCircle(window, out hv_Row3, out hv_Column4, out hv_Radius4);

            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Row2, hv_Column3, hv_Radius3);
            ho_Circle2.Dispose();
            HOperatorSet.GenCircle(out ho_Circle2, hv_Row3, hv_Column4, hv_Radius4);
            HOperatorSet.Difference(ho_Circle, ho_Circle2, out RingRegion);
            HOperatorSet.DispObj(RingRegion, window);

            DialogResult Result = MessageBox.Show("是否保存ROI！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                //if (Directory.Exists(FilePath) == false)
                //    Directory.CreateDirectory(FilePath);
                HOperatorSet.WriteRegion(RingRegion,fileName);
                MessageBox.Show("已保存文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return;
            }

        }
        public void DrawRectangleRingROI(HWindow window, string fileName)
        {
            HObject region, region1, region2;
            HTuple row1 = null, column1 = null, phi = null, Length1 = null, Length2 = null;
            HTuple row2 = null, column2 = null, phi1 = null, Length3 = null, Length4 = null;
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");
            MessageBox.Show("设定外边界", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            HOperatorSet.DrawRectangle2(window, out row1, out column1, out phi, out Length1, out Length2);
            MessageBox.Show("设定内边界！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            HOperatorSet.DrawRectangle2(window, out row2, out column2, out phi1, out Length3, out Length4);
            HOperatorSet.GenRectangle2(out region, row1, column1, phi, Length1, Length2);
            HOperatorSet.GenRectangle2(out region1, row2, column2, phi1, Length3, Length4);
            HOperatorSet.Difference(region, region1, out region2);
            HOperatorSet.DispObj(region2, window);
            DialogResult Result = MessageBox.Show("是否保存ROI！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {

                //if (Directory.Exists(FilePath) == false)
                //    Directory.CreateDirectory(FilePath);
                HOperatorSet.WriteRegion(region2,  fileName);
                MessageBox.Show("已保存文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return;
            }
        }
    }
}
