using desay.AAVision.Algorithm;
using HalconDotNet;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace desay
{
    class CuleCheck
    {
        #region//控制参数
        //tolerance 
        static private HTuple Tol = new HTuple(3.0);
        public HTuple tol {
            get { return Tol[0]; }
            set { Tol[0] = value; }
        }
        //胶圈是否的断开
        bool hv_is_opened = true;
        #endregion
        private void action(Bitmap bmp1,Bitmap bmp2, HWindow Window)
        {
            // 声明图像变量 
            #region
            HObject ho_ImageT, ho_ImageF;
            HObject ho_TransImage;
            HObject ho_ImageSub, ho_ImageMean;
            HObject ho_Regions, ho_RegionClosing, ho_RegionOpening1;
            HObject ho_RegionUnion, ho_Rectangle, ho_RegionDifference;
            HObject ho_RegionOpening, ho_Regionc, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionFillUp1, ho_RegionErosion;
            HObject ho_Contours1, ho_Contours = null;
            #endregion
            // 声明控制变量 
            #region
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_RowsT = null, hv_ColsT = null, hv_RowsF = null;
            HTuple hv_ColsF = null, hv_CoRRJunctions = null, hv_CoRCJunctions = null;
            HTuple hv_CoCCJunctions = null, hv_RowArea = null, hv_ColumnArea = null;
            HTuple hv_CoRRArea = null, hv_CoRCArea = null, hv_CoCCArea = null;
            HTuple hv_CoRRJunctions1 = null, hv_CoRCJunctions1 = null;
            HTuple hv_CoCCJunctions1 = null, hv_RowArea1 = null, hv_ColumnArea1 = null;
            HTuple hv_CoRRArea1 = null, hv_CoRCArea1 = null, hv_CoCCArea1 = null;
            HTuple hv_HomMat2D = null, hv_Points1 = null, hv_Points2 = null;
            HTuple hv_Number1 = null, hv_Number2 = null;
            #endregion
            // 初始化控制变量 
            #region
            HOperatorSet.GenEmptyObj(out ho_ImageT);
            HOperatorSet.GenEmptyObj(out ho_ImageF);
            HOperatorSet.GenEmptyObj(out ho_TransImage);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Regionc);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_Contours1);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            #endregion
            ho_ImageT.Dispose();
            Bitmap2HObject.Bitmap2HObj(bmp1, out ho_ImageT);
            ho_ImageF.Dispose();
            Bitmap2HObject.Bitmap2HObj(bmp2, out ho_ImageF);
            HOperatorSet.DispObj(ho_ImageF, Window);
            //获取图像的尺寸信息
            HOperatorSet.GetImageSize(ho_ImageF, out hv_Width, out hv_Height);
            //********
            #region
            //选图片一小块区域用于特征点提取
            //ho_Circle.Dispose();
            //HOperatorSet.GenCircle(out ho_Circle, hv_Height / 2, hv_Width / 2, 450);
            //ho_ImageReduced.Dispose();
            //HOperatorSet.ReduceDomain(ho_ImageT, ho_Circle, out ho_ImageReduced);
            //ho_ImageReduced1.Dispose();
            //HOperatorSet.ReduceDomain(ho_ImageF, ho_Circle, out ho_ImageReduced1);
            //特征点提取，harris角点
            //points_harris (ImageReduced, 3, 2, 0.08, 8000, RowsT, ColsT)
            //points_harris (ImageReduced1, 3, 2, 0.08, 8000, RowsF, ColsF)
            //********
            //提取特征点
            //points_harris (ImageT, 3, 2, 0.08, 8000, RowsT, ColsT)
            //points_harris (ImageF, 3, 2, 0.08, 8000, RowsF, ColsF)
            #endregion
            //********主逻辑
            #region
            HOperatorSet.PointsFoerstner(ho_ImageF, 1, 2, 3, 4000, 0.1, "gauss", "true",
                out hv_RowsF, out hv_ColsF, out hv_CoRRJunctions, out hv_CoRCJunctions, out hv_CoCCJunctions,
                out hv_RowArea, out hv_ColumnArea, out hv_CoRRArea, out hv_CoRCArea, out hv_CoCCArea);
            HOperatorSet.PointsFoerstner(ho_ImageT, 1, 2, 3, 4000, 0.1, "gauss", "true",
                out hv_RowsT, out hv_ColsT, out hv_CoRRJunctions1, out hv_CoRCJunctions1,
                out hv_CoCCJunctions1, out hv_RowArea1, out hv_ColumnArea1, out hv_CoRRArea1,
                out hv_CoRCArea1, out hv_CoCCArea1);

            //* 根据特征点求两张图之间的仿射矩阵：HomMat2D
            HOperatorSet.ProjMatchPointsRansac(ho_ImageT, ho_ImageF,  hv_RowsF, hv_ColsF,
                hv_RowsT, hv_ColsT, "ssd", 10, 0, 0, 1000, 1000, (new HTuple((new HTuple(-0.2)).TupleRad()
                )).TupleConcat((new HTuple(0.2)).TupleRad()), 30, "gold_standard", 0.4, 42,
                out hv_HomMat2D, out hv_Points1, out hv_Points2);

            //*将ImageF进行放射变换以对齐两张图片
            ho_TransImage.Dispose();
            HOperatorSet.ProjectiveTransImage(ho_ImageT, out ho_TransImage, hv_HomMat2D,
                "bilinear", "false", "false");


            //将点完胶的图片减去对齐后点胶之前的图片
            ho_ImageSub.Dispose();
            HOperatorSet.SubImage(ho_ImageF, ho_TransImage,  out ho_ImageSub, 1, 80);



            //**对相减得到的图片进行处理，提出胶圈轮廓
            //平滑图像
            ho_ImageMean.Dispose();
            HOperatorSet.MeanImage(ho_ImageSub, out ho_ImageMean, 5, 5);
            //*阈值分割，采用区域生长法，控制参数tol=3, region阈值100000
            ho_Regions.Dispose();
            HOperatorSet.Regiongrowing(ho_ImageMean, out ho_Regions, 3, 3, Tol, 500000);
        
            //对region进行开运算和闭运算，并合并region
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_Regions, out ho_RegionClosing, 30);
            ho_RegionOpening1.Dispose();
            HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening1, 11);
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_RegionOpening1, out ho_RegionUnion);
            //提取胶圈区域region
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_Height, hv_Width);
            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_Rectangle, ho_RegionUnion, out ho_RegionDifference
                );
            //
            HOperatorSet.CountObj(ho_RegionDifference, out hv_Number1);
            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 7);
            ho_Regionc.Dispose();
            HOperatorSet.ClosingCircle(ho_RegionOpening, out ho_Regionc, 9);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regionc, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                70);
            ho_RegionFillUp1.Dispose();
            HOperatorSet.FillUpShape(ho_SelectedRegions, out ho_RegionFillUp1, "area", 1,
                200);
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_RegionFillUp1, out ho_RegionErosion, 2.5);
            ho_Contours1.Dispose();
            HOperatorSet.GenContourRegionXld(ho_RegionErosion, out ho_Contours1, "border_holes");
            #endregion
            //判断胶圈是否封闭
            #region
            HOperatorSet.CountObj(ho_Contours1, out hv_Number2);

            if ((int)((new HTuple((new HTuple(hv_Number2.TupleEqual(2))).TupleAnd(new HTuple(hv_Number1.TupleEqual(
                1))))).TupleNot()) != 0)
            {
                hv_is_opened = true;
                HOperatorSet.SetLineWidth(Window, 2);
                HOperatorSet.SetColor(Window, "red");
                HOperatorSet.DispObj(ho_Contours1, Window);
                HOperatorSet.DispText(Window, "胶圈不完整！", "window",
                    12, 7, "black", new HTuple(), new HTuple());
            }
            else
            {
                hv_is_opened = false;
                HOperatorSet.SetLineWidth(Window, 2);
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.DispObj(ho_Contours, Window);
            }
            #endregion
            //释放图像变量
            #region
            ho_ImageT.Dispose();
            ho_ImageF.Dispose();
            ho_TransImage.Dispose();
            ho_ImageSub.Dispose();
            ho_ImageMean.Dispose();
            ho_Regions.Dispose();
            ho_RegionClosing.Dispose();
            ho_RegionOpening1.Dispose();
            ho_RegionUnion.Dispose();
            ho_Rectangle.Dispose();
            ho_RegionDifference.Dispose();
            ho_RegionOpening.Dispose();
            ho_Regionc.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp1.Dispose();
            ho_RegionErosion.Dispose();
            ho_Contours1.Dispose();
            ho_Contours.Dispose();
            #endregion
        }
    }
}
