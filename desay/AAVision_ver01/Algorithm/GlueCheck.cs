using desay.AAVision.Algorithm;
using desay.ProductData;
using HalconDotNet;
using System.Drawing;

namespace desay
{
    static class GlueCheck
    {
        #region//控制参数
        #region//胶水判断参数
        //flag胶圈是否合格
        private static bool is_qualified = true;
        //胶水外溢判断阈值
        private static HTuple glueoverflow_Outter = new HTuple(30);
        public static HTuple glueOverflowOutter { get { return glueoverflow_Outter; } set { glueoverflow_Outter = value; } }
        //胶水内溢判断阈值
        private static HTuple glueoverflow_Inner = new HTuple(30);
        public static HTuple glueOverflowInner { get { return glueoverflow_Inner; } set { glueoverflow_Inner = value; } }
        //胶水外圈缺胶判断阈值
        private static HTuple glueLack_Outter = new HTuple(40);
        public static HTuple glueLackOutter { get { return glueLack_Outter; } set { glueLack_Outter = value; } }
        //胶水内圈缺胶判断阈值
        private static HTuple glueLack_Inner = new HTuple(30);
        public static HTuple glueLackInner { get { return glueLack_Inner; } set { glueLack_Inner = value; } }
        //胶圈偏移判断阈值
        private static HTuple glueOffset_ = new HTuple(40);
        public static HTuple glueOffset { get { return glueOffset_; } set { glueOffset_ = value; } }
        #endregion

        #region//阈值分割参数
        //区域之间的tolerance
        public static HTuple tol = new HTuple(1.0);
        //region的最小面积阈值
        public static HTuple area = new HTuple(500000.0);
        //开运算核大小
        public static HTuple kernel = new HTuple(200.0);
        #endregion

        #region//ROI控制参数
        //圆心坐标
        private static HTuple hv_circle_center_x = new HTuple(950.0);
        private static HTuple hv_circle_center_y = new HTuple(1300.0);
        public static double circleCenter_x
        {
            get { return hv_circle_center_x[0]; }
            set { hv_circle_center_x[0] = value; }
        }
        public static double circleCenter_y
        {
            get { return hv_circle_center_y[0]; }
            set { hv_circle_center_y[0] = value; }
        }
        //圆半径
        private static HTuple hv_circle_radius = new HTuple(730.0);
        public static double circleRadius
        {
            get { return hv_circle_radius[0]; }
            set { hv_circle_radius[0] = value; }
        }
        #endregion
        #endregion

        private static bool action(Bitmap bmp_with_no_glue, Bitmap bmp_with_glue, HWindow Window)
        {

            #region// 声明图像变量 
            HObject ho_ImageT, ho_ImageF, ho_TransImage;
            HObject ho_ImageSub, ho_ImageGauss, ho_Regions_R, ho_RegionClosing;
            HObject ho_RegionOpening1, ho_RegionUnion, ho_Rectangle;
            HObject ho_RegionDifference, ho_RegionOpening, ho_Regionc;
            HObject ho_ConnectedRegions, ho_SelectedRegions, ho_RegionFillUp1;
            HObject ho_RegionErosion, ho_Contours1, ho_Circle_mask_S = null;
            HObject ho_target_ring = null, ho_target_ring_scale = null;
            HObject ho_Regions = null, ho_RegionFillUp = null, ho_RegionTrans = null;
            HObject ho_Contours = null, ho_glueOutterContour = null, ho_glueInnerContour = null;
            HObject ho_Contour_I_sap = null, ho_Contour_O_sap = null, ho_ContCircle_outter = null;
            HObject ho_ContCircle_Inner = null, Regionoping = null;
            #endregion

            #region//初始化图像变量
            HOperatorSet.GenEmptyObj(out ho_ImageT);
            HOperatorSet.GenEmptyObj(out ho_ImageF);
            HOperatorSet.GenEmptyObj(out ho_TransImage);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_Regions_R);
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
            HOperatorSet.GenEmptyObj(out ho_Circle_mask_S);
            HOperatorSet.GenEmptyObj(out ho_target_ring);
            HOperatorSet.GenEmptyObj(out ho_target_ring_scale);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_glueOutterContour);
            HOperatorSet.GenEmptyObj(out ho_glueInnerContour);
            HOperatorSet.GenEmptyObj(out ho_Contour_I_sap);
            HOperatorSet.GenEmptyObj(out ho_Contour_O_sap);
            HOperatorSet.GenEmptyObj(out ho_ContCircle_outter);
            HOperatorSet.GenEmptyObj(out ho_ContCircle_Inner);
            HOperatorSet.GenEmptyObj(out Regionoping);
            #endregion

            #region// 初始化控制变量 
            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
            HTuple hv_RowsF = null, hv_ColsF = null, hv_RowsT = null;
            HTuple hv_ColsT = null, hv_HomMat2D = null, hv_Points1 = null;
            HTuple hv_Points2 = null, hv_Number1 = null, hv_Number2 = null;
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_StartPhi = new HTuple(), hv_EndPhi = new HTuple();
            HTuple hv_PointOrder = new HTuple(), hv_glueWidth = new HTuple();
            HTuple hv_glueInnerCircle = new HTuple(), hv_glueOutterCircle = new HTuple();
            HTuple hv_glueCenter = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Col1 = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Col2 = new HTuple(), hv_Row_Inner_sampling = new HTuple();
            HTuple hv_Col_Inner_sampling = new HTuple(), hv_k = new HTuple();
            HTuple hv_i = new HTuple(), hv_Row_Outter_sampling = new HTuple();
            HTuple hv_Col_Outter_sampling = new HTuple(), hv_DistanceMin = new HTuple();
            HTuple hv_DistanceMax = new HTuple(), hv_glueWidthMin = new HTuple();
            HTuple hv_glueWidthMax = new HTuple(), hv_DistanceMin1 = new HTuple();
            HTuple hv_DistanceMax1 = new HTuple(), hv_Distance_inner_min = new HTuple();
            HTuple hv_Distance_inner_max = new HTuple(), hv_DistanceMin2 = new HTuple();
            HTuple hv_DistanceMax2 = new HTuple(), hv_Distance_outter_min = new HTuple();
            HTuple hv_Distance_outter_max = new HTuple(), hv_Row_Outter = new HTuple();
            HTuple hv_Column_Outter = new HTuple(), hv_Radius_Outter = new HTuple();
            HTuple hv_StartPhi1 = new HTuple(), hv_EndPhi1 = new HTuple();
            HTuple hv_PointOrder1 = new HTuple(), hv_DistanceMin_Outter = new HTuple();
            HTuple hv_DistanceMax_Outter = new HTuple(), hv_Ddistance_Outter = new HTuple();
            HTuple hv_Sdistance_Outter = new HTuple(), hv_Row_Inner = new HTuple();
            HTuple hv_Column_Inner = new HTuple(), hv_Radius_Inner = new HTuple();
            HTuple hv_StartPhi2 = new HTuple(), hv_EndPhi2 = new HTuple();
            HTuple hv_PointOrder2 = new HTuple(), hv_DistanceMin_Inner = new HTuple();
            HTuple hv_DistanceMax_Inner = new HTuple(), hv_Ddistance_Inner = new HTuple();
            HTuple hv_Sdistance_Inner = new HTuple(), hv_Row_glue = new HTuple();
            HTuple hv_Col_glue = new HTuple(), hv_Distance_offset = new HTuple();
            HTuple hv_Font = new HTuple();
            #endregion
            is_qualified = true;
            ho_ImageT.Dispose();
            Bitmap2HObject.Bitmap2HObj(bmp_with_no_glue, out ho_ImageT);
            bmp_with_no_glue = null;
            //CenterLocate.LastCenterLocateBMP.Dispose();
            ho_ImageF.Dispose();
            Bitmap2HObject.Bitmap2HObj(bmp_with_glue, out ho_ImageF);
            bmp_with_glue.Dispose();
            //获取图像的尺寸信息
            HOperatorSet.GetImageSize(ho_ImageF, out hv_Width, out hv_Height);
            //*******************主逻辑
            #region//提取特征点,根据特征点求两张图之间的仿射矩阵：HomMat2D,并对齐图像
            HOperatorSet.PointsHarris(ho_ImageF, 3, 2, 0.08, 8000, out hv_RowsF, out hv_ColsF);
            HOperatorSet.PointsHarris(ho_ImageT, 3, 2, 0.08, 8000, out hv_RowsT, out hv_ColsT);

            //* 根据特征点求两张图之间的仿射矩阵：HomMat2D
            HOperatorSet.ProjMatchPointsRansac(ho_ImageT, ho_ImageF, hv_RowsT, hv_ColsT,
                hv_RowsF, hv_ColsF, "ssd", 10, 0, 0, 1000, 1000, (new HTuple((new HTuple(-0.2)).TupleRad()
                )).TupleConcat((new HTuple(0.2)).TupleRad()), 10, "gold_standard", 0.2, 42,
                out hv_HomMat2D, out hv_Points1, out hv_Points2);
            //*将ImageT进行放射变换以对齐两张图片
            ho_TransImage.Dispose();
            HOperatorSet.ProjectiveTransImage(ho_ImageT, out ho_TransImage, hv_HomMat2D,
                "bilinear", "false", "false");
            //ho_ImageT.Dispose();
            #endregion

            #region//提取胶圈轮廓
            //将对齐后点胶之后的图片减去点胶之前图片
            ho_ImageSub.Dispose();
            HOperatorSet.AbsDiffImage(ho_ImageF, ho_TransImage, out ho_ImageSub, 2);
            //**对相减得到的图片进行处理，提出胶圈轮廓
            //平滑图像
            ho_ImageGauss.Dispose();
            HOperatorSet.GaussFilter(ho_ImageSub, out ho_ImageGauss, 11);
            ho_ImageSub.Dispose();
            //*阈值分割，采用区域生长法，控制参数tol=3, region阈值500000
            ho_Regions_R.Dispose();
            HOperatorSet.Regiongrowing(ho_ImageGauss, out ho_Regions_R, 4, 4, tol, area);
            ho_ImageGauss.Dispose();
            //对region进行开运算和闭运算，并合并region
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_Regions_R, out ho_RegionClosing, 30);
            ho_RegionOpening1.Dispose();
            HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening1, 11);
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_RegionOpening1, out ho_RegionUnion);
            //提取胶圈区域region
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_Height, hv_Width);
            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_Rectangle, ho_RegionUnion, out ho_RegionDifference);
            HOperatorSet.CountObj(ho_RegionDifference, out hv_Number1);
            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 7);
            ho_Regionc.Dispose();
            HOperatorSet.ClosingCircle(ho_RegionOpening, out ho_Regionc, 9);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regionc, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",70);
            ho_RegionFillUp1.Dispose();
            HOperatorSet.FillUpShape(ho_SelectedRegions, out ho_RegionFillUp1, "area", 1,200);
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_RegionFillUp1, out ho_RegionErosion, 2.5);
            ho_Contours1.Dispose();
            HOperatorSet.GenContourRegionXld(ho_RegionErosion, out ho_Contours1, "border_holes");
            HOperatorSet.CountObj(ho_Contours1, out hv_Number2);
            #endregion
            HOperatorSet.QueryFont(Window, out hv_Font);
            HOperatorSet.SetFont(Window, (hv_Font.TupleSelect(0)) + "-Bold-15");
            #region//检测胶圈是否封闭
            if ((int)((new HTuple((new HTuple(hv_Number2.TupleEqual(2))).TupleAnd(new HTuple(hv_Number1.TupleEqual(
        1))))).TupleNot()) != 0)
            {
                is_qualified = false;
                HOperatorSet.DispObj(ho_ImageF, Window);
                HOperatorSet.SetLineWidth(Window, 2);
                HOperatorSet.SetColor(Window, "red");
                HOperatorSet.DispObj(ho_Contours1, Window);
                HOperatorSet.DispText(Window, "无胶或胶圈不完整！", "window",
                    15, 480, "red", new HTuple(), new HTuple());
                #region
                ho_ImageT.Dispose();
                ho_ImageF.Dispose();
                ho_TransImage.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageGauss.Dispose();
                ho_Regions_R.Dispose();
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
                ho_Circle_mask_S.Dispose();
                ho_target_ring.Dispose();
                ho_target_ring_scale.Dispose();
                ho_Regions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionTrans.Dispose();
                ho_Contours.Dispose();
                ho_glueOutterContour.Dispose();
                ho_glueInnerContour.Dispose();
                ho_Contour_I_sap.Dispose();
                ho_Contour_O_sap.Dispose();
                ho_ContCircle_outter.Dispose();
                ho_ContCircle_Inner.Dispose();
                #endregion
                return is_qualified;
            }
            #endregion

            #region//胶圈封闭则进一步检查其它指标是否合格
            else
            {
                #region//计算点胶区域内外圆
                //***********************
                //创建圆并取得圆环状Region
                ho_Circle_mask_S.Dispose();
                HOperatorSet.GenCircle(out ho_Circle_mask_S, hv_circle_center_x, hv_circle_center_y,
                    hv_circle_radius);
                HOperatorSet.ReduceDomain(ho_ImageT, ho_Circle_mask_S, out ho_target_ring);
                //**精确定位点胶区域
                //增强对比度
                ho_target_ring_scale.Dispose();
                HOperatorSet.ScaleImageMax(ho_target_ring, out ho_target_ring_scale);
                //阈值划分
                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_target_ring_scale, out ho_Regions, CenterLocate.threshold_min ,
                    CenterLocate.threshold_max);
                Regionoping.Dispose();
                HOperatorSet.OpeningCircle(ho_Regions,out Regionoping, 25);
                //联通区域
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(Regionoping, out ho_ConnectedRegions);
                //选出面积最大的Region
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and",  CenterLocate.areaMin, CenterLocate.areaMax);
                //填充区域
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUpShape(ho_SelectedRegions, out ho_RegionFillUp, "area", 1,
                    10000);
                //外接圆
                ho_RegionTrans.Dispose();
                HOperatorSet.ShapeTrans(ho_RegionFillUp, out ho_RegionTrans, "outer_circle");
                //取得XLD轮廓，用于显示
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionTrans, out ho_Contours, "border");
                HOperatorSet.FitCircleContourXld(ho_Contours, "algebraic", -1, 0, 0, 3, 2, out hv_Row,
                    out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);

                //理论胶宽
                hv_glueWidth = 140;
                //理论胶圈内圆
                hv_glueInnerCircle = hv_Radius.Clone();
                //理论胶圈外圆
                hv_glueOutterCircle = hv_Radius + hv_glueWidth;
                //理论胶圈圆心
                hv_glueCenter = new HTuple();
                hv_glueCenter = hv_glueCenter.TupleConcat(hv_Row);
                hv_glueCenter = hv_glueCenter.TupleConcat(hv_Column);
                //***********************
                #endregion
                #region
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Regions_R, out ho_RegionClosing, kernel);
                ho_RegionOpening1.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening1, 15);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_RegionOpening1, out ho_RegionUnion);

                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_Height, hv_Width);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_RegionUnion, out ho_RegionDifference
                    );

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
                HOperatorSet.FillUpShape(ho_SelectedRegions, out ho_RegionFillUp1, "area",
                    1, 20000);
                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionCircle(ho_RegionFillUp1, out ho_RegionErosion, 2.5);

                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionErosion, out ho_Contours, "border_holes");
                ho_glueOutterContour.Dispose();
                HOperatorSet.SelectObj(ho_Contours, out ho_glueOutterContour, 1);
                ho_glueInnerContour.Dispose();
                HOperatorSet.SelectObj(ho_Contours, out ho_glueInnerContour, 2);
                HOperatorSet.GetContourXld(ho_glueInnerContour, out hv_Row2, out hv_Col1);
                HOperatorSet.GetContourXld(ho_glueOutterContour, out hv_Row3, out hv_Col2);
                hv_Row_Inner_sampling = new HTuple();
                hv_Col_Inner_sampling = new HTuple();
                hv_k = 0;
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Row2.TupleLength())) - 1); hv_i = (int)hv_i + 90)
                {
                    if (hv_Row_Inner_sampling == null)
                        hv_Row_Inner_sampling = new HTuple();
                    hv_Row_Inner_sampling[hv_k] = hv_Row2.TupleSelect(hv_i);
                    if (hv_Col_Inner_sampling == null)
                        hv_Col_Inner_sampling = new HTuple();
                    hv_Col_Inner_sampling[hv_k] = hv_Col1.TupleSelect(hv_i);
                    hv_k = hv_k + 1;
                }
                if (hv_Row_Inner_sampling == null)
                    hv_Row_Inner_sampling = new HTuple();
                hv_Row_Inner_sampling[new HTuple(hv_Row_Inner_sampling.TupleLength())] = hv_Row_Inner_sampling.TupleSelect(
                    0);
                if (hv_Col_Inner_sampling == null)
                    hv_Col_Inner_sampling = new HTuple();
                hv_Col_Inner_sampling[new HTuple(hv_Col_Inner_sampling.TupleLength())] = hv_Col_Inner_sampling.TupleSelect(
                    0);
                hv_Row_Outter_sampling = new HTuple();
                hv_Col_Outter_sampling = new HTuple();
                hv_k = 0;
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Row3.TupleLength())) - 1); hv_i = (int)hv_i + 90)
                {
                    if (hv_Row_Outter_sampling == null)
                        hv_Row_Outter_sampling = new HTuple();
                    hv_Row_Outter_sampling[hv_k] = hv_Row3.TupleSelect(hv_i);
                    if (hv_Col_Outter_sampling == null)
                        hv_Col_Outter_sampling = new HTuple();
                    hv_Col_Outter_sampling[hv_k] = hv_Col2.TupleSelect(hv_i);
                    hv_k = hv_k + 1;
                }
                if (hv_Row_Outter_sampling == null)
                    hv_Row_Outter_sampling = new HTuple();
                hv_Row_Outter_sampling[new HTuple(hv_Row_Outter_sampling.TupleLength())] = hv_Row_Outter_sampling.TupleSelect(
                    0);
                if (hv_Col_Outter_sampling == null)
                    hv_Col_Outter_sampling = new HTuple();
                hv_Col_Outter_sampling[new HTuple(hv_Col_Outter_sampling.TupleLength())] = hv_Col_Outter_sampling.TupleSelect(
                    0);
                ho_Contour_I_sap.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour_I_sap, hv_Row_Inner_sampling,
                    hv_Col_Inner_sampling);
                ho_Contour_O_sap.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour_O_sap, hv_Row_Outter_sampling,
                    hv_Col_Outter_sampling);
                ho_glueInnerContour.Dispose();
                ho_glueInnerContour = ho_Contour_I_sap.CopyObj(1, -1);
                ho_glueOutterContour.Dispose();
                ho_glueOutterContour = ho_Contour_O_sap.CopyObj(1, -1);
                //*最大最小胶宽
                HOperatorSet.DistancePc(ho_glueOutterContour, hv_Row2, hv_Col1, out hv_DistanceMin,
                    out hv_DistanceMax);
                HOperatorSet.TupleMin(hv_DistanceMin, out hv_glueWidthMin);
                HOperatorSet.TupleMax(hv_DistanceMin, out hv_glueWidthMax);
                //*胶水内轮廓离边界的距离最大值最小值
                HOperatorSet.DistancePc(ho_glueInnerContour, hv_glueCenter.TupleSelect(0),
                    hv_glueCenter.TupleSelect(1), out hv_DistanceMin1, out hv_DistanceMax1);
                hv_Distance_inner_min = hv_DistanceMin1 - hv_glueInnerCircle;
                hv_Distance_inner_max = hv_DistanceMax1 - hv_glueInnerCircle;
                //*胶水外轮廓离边界的距离最大最小值
                HOperatorSet.DistancePc(ho_glueOutterContour, hv_glueCenter.TupleSelect(0),
                    hv_glueCenter.TupleSelect(1), out hv_DistanceMin2, out hv_DistanceMax2);
                hv_Distance_outter_min = hv_DistanceMin2 - hv_glueOutterCircle;
                hv_Distance_outter_max = hv_DistanceMax2 - hv_glueOutterCircle;
                //*用胶轮廓与拟合圆间的最大距离判断缺胶溢胶情况
                HOperatorSet.FitCircleContourXld(ho_glueOutterContour, "algebraic", -1, 0,
                    0, 3, 2, out hv_Row_Outter, out hv_Column_Outter, out hv_Radius_Outter,
                    out hv_StartPhi1, out hv_EndPhi1, out hv_PointOrder1);
                ho_ContCircle_outter.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle_outter, hv_Row_Outter, hv_Column_Outter,
                    hv_Radius_Outter, 0, 6.28318, "positive", 1);
                HOperatorSet.DistancePc(ho_glueOutterContour, hv_Row_Outter, hv_Column_Outter,
                    out hv_DistanceMin_Outter, out hv_DistanceMax_Outter);
                hv_Ddistance_Outter = hv_DistanceMax_Outter - hv_Radius_Outter;
                hv_Sdistance_Outter = hv_Radius_Outter - hv_DistanceMin_Outter;
                HOperatorSet.FitCircleContourXld(ho_glueInnerContour, "algebraic", -1, 0, 0,
                    3, 2, out hv_Row_Inner, out hv_Column_Inner, out hv_Radius_Inner, out hv_StartPhi2,
                    out hv_EndPhi2, out hv_PointOrder2);
                ho_ContCircle_Inner.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle_Inner, hv_Row_Inner, hv_Column_Inner,
                    hv_Radius_Inner, 0, 6.28318, "positive", 1);
                HOperatorSet.DistancePc(ho_glueInnerContour, hv_Row_Inner, hv_Column_Inner,
                    out hv_DistanceMin_Inner, out hv_DistanceMax_Inner);
                hv_Ddistance_Inner = hv_Radius_Inner - hv_DistanceMin_Inner;
                hv_Sdistance_Inner = hv_DistanceMax_Inner - hv_Radius_Inner;
                //*利用拟合内外胶的圆心的平均值与拟合基准圆的圆心坐标判断偏移情况
                hv_Row_glue = (hv_Row_Outter + hv_Row_Inner) / 2;
                hv_Col_glue = (hv_Column_Outter + hv_Column_Inner) / 2;
                HOperatorSet.DistancePp(hv_glueCenter.TupleSelect(0), hv_glueCenter.TupleSelect(
                    1), hv_Row_glue, hv_Col_glue, out hv_Distance_offset);
                //*算出拟合胶水的宽度
                hv_Radius = hv_Radius_Outter - hv_Radius_Inner;
                //**显示
                HOperatorSet.DispObj(ho_ImageF, Window);
                HOperatorSet.SetLineWidth(Window, 2);
                HOperatorSet.DispObj(ho_glueOutterContour, Window);
                HOperatorSet.DispObj(ho_glueInnerContour, Window);
                HOperatorSet.DispText(Window, (("胶离内边界最大 / 最小距离：" + hv_Distance_inner_max) + " / ") + hv_Distance_inner_min,
                    "window", 15, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(Window, (("胶离外边界最大 / 最小距离：" + hv_Distance_outter_max) + " / ") + hv_Distance_outter_min,
                    "window", 40, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(Window, "内外胶拟合圆宽度：" + hv_Radius,
                    "window", 65, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(Window, "最小胶宽度：" + hv_glueWidthMin,
                    "window", 90, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(Window, "最大胶宽度：" + hv_glueWidthMax,
                    "window", 115, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(Window, "胶偏移距离：" + hv_Distance_offset,
                    "window", 140, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(Window, "标准胶宽：" + hv_glueWidth,
                    "window", 165, 7, "black", new HTuple(), new HTuple());
                #endregion
                if ((int)(new HTuple(hv_Distance_offset.TupleGreater(glueOffset_))) != 0)
                {
                    is_qualified = false;
                    HOperatorSet.DispText(Window, "胶圈偏移", "window", 40,
                        400, "red", new HTuple(), new HTuple());
                    #region//释放图像变量
                    ho_ImageT.Dispose();
                    ho_ImageF.Dispose();
                    ho_TransImage.Dispose();
                    ho_ImageSub.Dispose();
                    ho_ImageGauss.Dispose();
                    ho_Regions_R.Dispose();
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
                    ho_Circle_mask_S.Dispose();
                    ho_target_ring.Dispose();
                    ho_target_ring_scale.Dispose();
                    ho_Regions.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_RegionTrans.Dispose();
                    ho_Contours.Dispose();
                    ho_glueOutterContour.Dispose();
                    ho_glueInnerContour.Dispose();
                    ho_Contour_I_sap.Dispose();
                    ho_Contour_O_sap.Dispose();
                    ho_ContCircle_outter.Dispose();
                    ho_ContCircle_Inner.Dispose();
                    #endregion
                    return is_qualified;
                }
                else
                {

                    if ((int)(new HTuple(hv_Distance_outter_max.TupleGreater(glueoverflow_Outter))) != 0)
                    {
                        HOperatorSet.DispText(Window, "外胶溢胶!", "window", 15, 400, "red", new HTuple(), new HTuple());
                        is_qualified = false;
                    }
                    if ((int)(new HTuple(((hv_Distance_outter_min.TupleAbs())).TupleGreater(glueLack_Outter))) != 0)
                    {
                        HOperatorSet.DispText(Window, "外胶缺胶!", "window", 40, 400, "red", new HTuple(), new HTuple());
                        is_qualified = false;
                    }
                    if ((int)(new HTuple(((hv_Distance_inner_min.TupleAbs())).TupleGreater(glueoverflow_Inner))) != 0)
                    {
                        HOperatorSet.DispText(Window, "内胶溢胶!", "window", 65, 400, "red", new HTuple(), new HTuple());
                        is_qualified = false;
                    }
                    if ((int)(new HTuple(hv_Distance_inner_max.TupleGreater(glueLack_Inner))) != 0)
                    {
                        HOperatorSet.DispText(Window, "内胶缺胶!", "window", 90, 400, "red", new HTuple(), new HTuple());
                        is_qualified = false;
                    }

                    #region//释放图像变量
                    ho_ImageT.Dispose();
                    ho_ImageF.Dispose();
                    ho_TransImage.Dispose();
                    ho_ImageSub.Dispose();
                    ho_ImageGauss.Dispose();
                    ho_Regions_R.Dispose();
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
                    ho_Circle_mask_S.Dispose();
                    ho_target_ring.Dispose();
                    ho_target_ring_scale.Dispose();
                    ho_Regions.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_RegionTrans.Dispose();
                    ho_Contours.Dispose();
                    ho_glueOutterContour.Dispose();
                    ho_glueInnerContour.Dispose();
                    ho_Contour_I_sap.Dispose();
                    ho_Contour_O_sap.Dispose();
                    ho_ContCircle_outter.Dispose();
                    ho_ContCircle_Inner.Dispose();
                    #endregion

                    return is_qualified;
                }
            }
            #endregion
        }
        public static void TestBmp(Bitmap no_glue_bmp, Bitmap glue_bmp, HWindow hWindow, bool save)
        {
            Marking.GlueResult = action(no_glue_bmp, glue_bmp, hWindow);

            if (save)
            {
                SaveImage.Save(hWindow);
            }

        }
    }
}
