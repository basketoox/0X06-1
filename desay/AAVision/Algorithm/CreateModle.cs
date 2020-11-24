using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Windows.Forms;
using System.IO;

namespace desay.AAVision.Algorithm
{
    public class CreateModle
    {
        private string _filePath = @"./Resources";
        private string _fileName;
        public CreateModle(string FileName)
        {
            _fileName = FileName;
        }
        /// <summary>
        /// 创建圆形模板
        /// </summary>
        /// <param name="window">绘图窗口</param>
        /// <param name="image">输入图像</param>
        public void DrawCircle(HWindow window, HObject image)
        {
            HObject ho_Circle, ho_targetROI,ho_coutours;
            HTuple hv_Row2 = null, hv_Column3 = null, hv_Radius3 = null, hv_ModelID = null; ;
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_targetROI);
            HOperatorSet.GenEmptyObj(out ho_coutours);
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");

            HOperatorSet.DrawCircle(window, out hv_Row2, out hv_Column3,
                                    out hv_Radius3);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Row2, hv_Column3, hv_Radius3);
            ho_targetROI.Dispose();
            HOperatorSet.DispObj(ho_Circle, window);
            HTuple hv_Area, hv_Row4, hv_Column4;
            HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate;
            HObject ho_ContoursAffinTrans, ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            DialogResult Result = MessageBox.Show("是否保存模板！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                HOperatorSet.ReduceDomain(image, ho_Circle, out ho_targetROI);
                HOperatorSet.AreaCenter(ho_targetROI, out hv_Area, out hv_Row4, out hv_Column4);
                HOperatorSet.SetSystem("border_shape_models", "false");
                //HOperatorSet.CreateShapeModel(ho_targetROI, 4, -0.39, 0.79, "auto", "auto", "ignore_color_polarity",
                //                              "auto", "auto", out hv_ModelID);
                HOperatorSet.CreateShapeModel(ho_targetROI, 4, -0.39, 0.79, "auto", "point_reduction_high", "ignore_global_polarity",
                                              "auto", "auto", out hv_ModelID);
                //if (!Directory.Exists(_filePath))
                //{ Directory.CreateDirectory(_filePath); }
                HOperatorSet.WriteShapeModel(hv_ModelID, /*_filePath + "//" + */_fileName);
              
                ho_coutours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_coutours, hv_ModelID,1);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row4, hv_Column4, out hv_HomMat2DTranslate);
                ho_ContoursAffinTrans.Dispose();
                HOperatorSet.AffineTransContourXld(ho_coutours, out ho_ContoursAffinTrans, hv_HomMat2DTranslate);
               
                HOperatorSet.DispObj(image, window);
                HOperatorSet.DispObj(ho_ContoursAffinTrans, window);
                HOperatorSet.SetColor(window, "green");
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row4, hv_Column4,
                       180, 0.785398);
                HOperatorSet.DispObj(ho_Cross, window);
                MessageBox.Show("已保存模板", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                HOperatorSet.DispObj(image, window);
                return;
            }

        }
        /// <summary>
        ///创建矩形模板
        /// </summary>
        /// <param name="window"></param>
        /// <param name="image"></param>
        public void DrawRectangle(HWindow window, HObject image)
        {
            HObject ho_Rectangle, ho_targetROI, ho_coutours;
            HTuple hv_Row = null, hv_Column = null, hv_Phi = null;
            HTuple hv_Length1 = null, hv_Length2 = null, hv_ModelID;
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_targetROI);
            HOperatorSet.GenEmptyObj(out ho_coutours);
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");

            HOperatorSet.DrawRectangle2(window, out hv_Row, out hv_Column,
                          out hv_Phi, out hv_Length1, out hv_Length2);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
            ho_targetROI.Dispose();
            HOperatorSet.DispObj(ho_Rectangle, window);

            HTuple hv_Area, hv_Row4, hv_Column4;
            HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate;
            HObject ho_ContoursAffinTrans, ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            DialogResult Result = MessageBox.Show("是否保存模板！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                HOperatorSet.ReduceDomain(image, ho_Rectangle, out ho_targetROI);
                HOperatorSet.AreaCenter(ho_targetROI, out hv_Area, out hv_Row4, out hv_Column4);
                //HOperatorSet.CreateShapeModel(ho_targetROI, 3, -0.39, 0.79, "auto", "auto", "ignore_color_polarity",
                //                              "auto", "auto", out hv_ModelID);
                HOperatorSet.CreateShapeModel(ho_targetROI, 4, -0.39, 0.79, "auto", "point_reduction_high", "ignore_global_polarity",
                                             "auto", "auto", out hv_ModelID);
                //if (!Directory.Exists(_filePath))
                //{ Directory.CreateDirectory(_filePath); }
                HOperatorSet.WriteShapeModel(hv_ModelID, /*_filePath + "//" + */_fileName);
                HOperatorSet.GetShapeModelContours(out ho_coutours, hv_ModelID, 1);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row4, hv_Column4, out hv_HomMat2DTranslate);
                ho_ContoursAffinTrans.Dispose();
                HOperatorSet.AffineTransContourXld(ho_coutours, out ho_ContoursAffinTrans, hv_HomMat2DTranslate);

                HOperatorSet.DispObj(image, window);
                HOperatorSet.DispObj(ho_ContoursAffinTrans, window);
                HOperatorSet.SetColor(window, "green");
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row4, hv_Column4,
                       180, 0.785398);
                HOperatorSet.DispObj(ho_Cross, window);
                MessageBox.Show("已保存模板", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                HOperatorSet.DispObj(image, window);
                return;
            }

        }
        /// <summary>
        /// 椭圆形模板
        /// </summary>
        /// <param name="window"></param>
        /// <param name="image"></param>
        public void DrawEllipse(HWindow window, HObject image)
        {
            HObject ho_Ellipse, ho_targetROI, ho_coutours;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_Phi1 = null, hv_Radius1 = null;
            HTuple hv_Radius2 = null, hv_ModelID;
            HOperatorSet.GenEmptyObj(out ho_Ellipse);
            HOperatorSet.GenEmptyObj(out ho_targetROI);
            HOperatorSet.GenEmptyObj(out ho_coutours);
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");

            HOperatorSet.DrawEllipse(window, out hv_Row1, out hv_Column1,
                                    out hv_Phi1, out hv_Radius1, out hv_Radius2);
            ho_Ellipse.Dispose();
            HOperatorSet.GenEllipse(out ho_Ellipse, hv_Row1, hv_Column1, hv_Phi1, hv_Radius1, hv_Radius2);
            ho_targetROI.Dispose();
            HOperatorSet.DispObj(ho_Ellipse, window);

            HTuple hv_Area, hv_Row4, hv_Column4;
            HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate;
            HObject ho_ContoursAffinTrans, ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            DialogResult Result = MessageBox.Show("是否保存模板！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                HOperatorSet.ReduceDomain(image, ho_Ellipse, out ho_targetROI);
                HOperatorSet.AreaCenter(ho_targetROI, out hv_Area, out hv_Row4, out hv_Column4);
                //HOperatorSet.CreateShapeModel(ho_targetROI, 3, -0.39, 0.79, "auto", "auto", "ignore_color_polarity",
                //                              "auto", "auto", out hv_ModelID);
                HOperatorSet.CreateShapeModel(ho_targetROI, 4, -0.39, 0.79, "auto", "point_reduction_high", "ignore_global_polarity",
                                             "auto", "auto", out hv_ModelID);
                //if (!Directory.Exists(_filePath))
                //{ Directory.CreateDirectory(_filePath); }
                HOperatorSet.WriteShapeModel(hv_ModelID, /*_filePath + "//" +*/ _fileName);
                ho_coutours.Dispose();
                HOperatorSet.GetShapeModelContours(out ho_coutours, hv_ModelID, 1);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row4, hv_Column4, out hv_HomMat2DTranslate);
                ho_ContoursAffinTrans.Dispose();
                HOperatorSet.AffineTransContourXld(ho_coutours, out ho_ContoursAffinTrans, hv_HomMat2DTranslate);
               
                HOperatorSet.DispObj(image, window);
                HOperatorSet.DispObj(ho_ContoursAffinTrans, window);
                HOperatorSet.SetColor(window, "green");
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row4, hv_Column4,
                       180, 0.785398);
                HOperatorSet.DispObj(ho_Cross, window);
                MessageBox.Show("已保存模板", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                HOperatorSet.DispObj(image, window);
                return;
            }

        }
        /// <summary>
        /// 任意形状模板
        /// </summary>
        /// <param name="window"></param>
        /// <param name="image"></param>
        public void DrawRegion(HWindow window, HObject image)
        {
            HObject ho_Region, ho_targetROI, ho_coutours;

            HTuple hv_ModelID;
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_targetROI);
            HOperatorSet.GenEmptyObj(out ho_coutours);
            HOperatorSet.SetColor(window, "red");
            HOperatorSet.SetLineWidth(window, 2);
            HOperatorSet.SetDraw(window, "margin");

            HOperatorSet.DrawRegion(out ho_Region, window);
            ho_Region.Dispose();
            ho_targetROI.Dispose();
            HOperatorSet.DispObj(ho_Region, window);

            HTuple hv_Area, hv_Row4, hv_Column4;
            HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate;
            HObject ho_ContoursAffinTrans, ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            DialogResult Result = MessageBox.Show("是否保存模板！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                HOperatorSet.ReduceDomain(image, ho_Region, out ho_targetROI);
                HOperatorSet.AreaCenter(ho_targetROI, out hv_Area, out hv_Row4, out hv_Column4);
                //HOperatorSet.CreateShapeModel(ho_targetROI, 3, -0.39, 0.79, "auto", "auto", "ignore_color_polarity",
                //                              "auto", "auto", out hv_ModelID);
                HOperatorSet.CreateShapeModel(ho_targetROI, 4, -0.39, 0.79, "auto", "point_reduction_high", "ignore_global_polarity",
                                             "auto", "auto", out hv_ModelID);
                //if (!Directory.Exists(_filePath))
                //{ Directory.CreateDirectory(_filePath); }
                HOperatorSet.WriteShapeModel(hv_ModelID, /*_filePath + "//" + */_fileName);
                HOperatorSet.GetShapeModelContours(out ho_coutours, hv_ModelID, 1);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row4, hv_Column4, out hv_HomMat2DTranslate);
                ho_ContoursAffinTrans.Dispose();
                HOperatorSet.AffineTransContourXld(ho_coutours, out ho_ContoursAffinTrans, hv_HomMat2DTranslate);

                HOperatorSet.DispObj(image, window);
                HOperatorSet.DispObj(ho_ContoursAffinTrans, window);
                HOperatorSet.SetColor(window, "green");
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row4, hv_Column4,
                       180, 0.785398);
                HOperatorSet.DispObj(ho_Cross, window);
                MessageBox.Show("已保存模板", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                HOperatorSet.DispObj(image, window);
                return;
            }

        }
        public void DrawCircleRing(HWindow window, HObject image)
        {
            HObject ho_Circle, ho_targetROI, ho_Circle2, RingRegion, ho_coutours;
            HTuple hv_Row2 = null, hv_Column3 = null, hv_Radius3 = null, hv_ModelID = null;
            HTuple hv_Row3 = null, hv_Column4 = null, hv_Radius4 = null;
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Circle2);
            HOperatorSet.GenEmptyObj(out RingRegion);
            HOperatorSet.GenEmptyObj(out ho_targetROI);
            HOperatorSet.GenEmptyObj(out ho_coutours);
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
            HOperatorSet.DispObj(ho_Circle, window);
            ho_targetROI.Dispose();

            HTuple hv_Area, hv_Row7, hv_Column7;
            HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate;
            HObject ho_ContoursAffinTrans, ho_Cross;
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            DialogResult Result = MessageBox.Show("是否保存模板！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                HOperatorSet.ReduceDomain(image, RingRegion, out ho_targetROI);
                HOperatorSet.AreaCenter(ho_targetROI, out hv_Area, out hv_Row7, out hv_Column7);
                HOperatorSet.CreateShapeModel(ho_targetROI, 3, -0.39, 0.79, "auto", "auto", "ignore_color_polarity",
                                              "auto", "auto", out hv_ModelID);
                //if (!Directory.Exists(_filePath))
                //{ Directory.CreateDirectory(_filePath); }
                HOperatorSet.WriteShapeModel(hv_ModelID,/* _filePath + "//" + */_fileName);
                HOperatorSet.GetShapeModelContours(out ho_coutours, hv_ModelID, 1);
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row7, hv_Column7, out hv_HomMat2DTranslate);
                ho_ContoursAffinTrans.Dispose();
                HOperatorSet.AffineTransContourXld(ho_coutours, out ho_ContoursAffinTrans, hv_HomMat2DTranslate);

                HOperatorSet.DispObj(image, window);
                HOperatorSet.DispObj(ho_ContoursAffinTrans, window);
                HOperatorSet.SetColor(window, "green");
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row7, hv_Column7,
                       180, 0.785398);
                HOperatorSet.DispObj(ho_Cross, window);
                MessageBox.Show("已保存模板", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                HOperatorSet.DispObj(image, window);
                return;
            }
        }

    }
}
