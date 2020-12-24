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
        //胶宽
        private static HTuple hv_glueWidth = new HTuple(135);
        public static HTuple GlueWidth { get {return hv_glueWidth; } set { hv_glueWidth= value; } }
        //胶圈内圆
        private static HTuple hv_glueInnerCircle = new HTuple(535.0);
        public static HTuple GlueInnerCircle { get { return hv_glueInnerCircle; } set { hv_glueInnerCircle = value; } }
        //flag胶圈是否合格
        private static bool is_qualified = true;
        //胶水外溢判断阈值
        private static HTuple glueoverflow_Outter = new HTuple(60);
        public static HTuple glueOverflowOutter { get { return glueoverflow_Outter; } set { glueoverflow_Outter = value; } }
        //胶水内溢判断阈值
        private static HTuple glueoverflow_Inner = new HTuple(60);
        public static HTuple glueOverflowInner { get { return glueoverflow_Inner; } set { glueoverflow_Inner = value; } }
        //胶水外圈缺胶判断阈值
        private static HTuple glueLack_Outter = new HTuple(70);
        public static HTuple glueLackOutter { get { return glueLack_Outter; } set { glueLack_Outter = value; } }
        //胶水内圈缺胶判断阈值
        private static HTuple glueLack_Inner = new HTuple(70);
        public static HTuple glueLackInner { get { return glueLack_Inner; } set { glueLack_Inner = value; } }
        //胶圈偏移判断阈值
        private static HTuple glueOffset_ = new HTuple(60);
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
        #endregion
        public static void SetString(HWindow window, HTuple str, HTuple color, HTuple size)
        {
            HOperatorSet.SetColor(window, color);
            set_display_font(window, size, "mono", "true", "false");
            HOperatorSet.SetTposition(window, -100, 2000);
            HOperatorSet.WriteString(window, str);
            set_display_font(window, 15, "mono", "true", "false");
        }
        public static void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
    HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variab();les 

            HTuple hv_OS = null, hv_Fonts = new HTuple();
            HTuple hv_Style = null, hv_Exception = new HTuple(), hv_AvailableFonts = null;
            HTuple hv_Fdx = null, hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Restore previous behaviour
                hv_Size_COPY_INP_TMP = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt();
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Courier";
                hv_Fonts[1] = "Courier 10 Pitch";
                hv_Fonts[2] = "Courier New";
                hv_Fonts[3] = "CourierNew";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Consolas";
                hv_Fonts[1] = "Menlo";
                hv_Fonts[2] = "Courier";
                hv_Fonts[3] = "Courier 10 Pitch";
                hv_Fonts[4] = "FreeMono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Luxi Sans";
                hv_Fonts[1] = "DejaVu Sans";
                hv_Fonts[2] = "FreeSans";
                hv_Fonts[3] = "Arial";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Times New Roman";
                hv_Fonts[1] = "Luxi Serif";
                hv_Fonts[2] = "DejaVu Serif";
                hv_Fonts[3] = "FreeSerif";
                hv_Fonts[4] = "Utopia";
            }
            else
            {
                hv_Fonts = hv_Font_COPY_INP_TMP.Clone();
            }
            hv_Style = "";
            if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Bold";
            }
            else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Bold";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Italic";
            }
            else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Slant";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
            {
                hv_Style = "Normal";
            }
            HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
            hv_Font_COPY_INP_TMP = "";
            for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
            {
                hv_Indices = hv_AvailableFonts.TupleFind(hv_Fonts.TupleSelect(hv_Fdx));
                if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_Fdx);
                        break;
                    }
                }
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                throw new HalconException("Wrong value of control parameter Font");
            }
            hv_Font_COPY_INP_TMP = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
            HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        private static bool action(Bitmap bmp_with_no_glue, Bitmap bmp_with_glue, HWindow Window)
        {

            try
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
                HObject SmoothedGlueOutterContour = null, SmoothedGlueInnerContour = null;
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
                HTuple hv_PointOrder = new HTuple();
                //HTuple hv_glueInnerCircle = new HTuple(), hv_glueWidth = new HTuple();
                HTuple hv_glueOutterCircle = new HTuple();
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

                HObject  img_old;             
                HOperatorSet.GenEmptyObj(out img_old);
                HTuple htuple;
                //彩色变灰色
                HOperatorSet.CountChannels(ho_ImageT, out htuple);
                img_old.Dispose();
                if (htuple == 3)
                {
                    HOperatorSet.Rgb1ToGray(ho_ImageT, out img_old);
                    ho_ImageT.Dispose();
                    ho_ImageT = img_old.Clone();
                }
               

                //CenterLocate.LastCenterLocateBMP.Dispose();
                ho_ImageF.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp_with_glue, out ho_ImageF);
                bmp_with_glue.Dispose();
                //彩色变灰色
                HOperatorSet.CountChannels(ho_ImageF, out htuple);
                img_old.Dispose();
                if (htuple == 3)
                {
                    HOperatorSet.Rgb1ToGray(ho_ImageF, out img_old);
                    ho_ImageF.Dispose();
                    ho_ImageF = img_old.Clone();
                }

                //获取图像的尺寸信息
                HOperatorSet.GetImageSize(ho_ImageF, out hv_Width, out hv_Height);
                //*******************主逻辑
                #region//提取特征点,根据特征点求两张图之间的仿射矩阵：HomMat2D,并对齐图像
                HOperatorSet.PointsHarris(ho_ImageF, 2.5, 2, 0.08, 2500, out hv_RowsF, out hv_ColsF);
                HOperatorSet.PointsHarris(ho_ImageT, 2.5, 2, 0.08, 2500, out hv_RowsT, out hv_ColsT);

                //* 根据特征点求两张图之间的仿射矩阵：HomMat2D
                HOperatorSet.ProjMatchPointsRansac(ho_ImageT, ho_ImageF, hv_RowsT, hv_ColsT,
                    hv_RowsF, hv_ColsF, "sad", 10, 0, 0, 100, 100, (new HTuple((new HTuple(-0.2)).TupleRad()
                    )).TupleConcat((new HTuple(0.2)).TupleRad()), 10, "gold_standard", 0.2, 0,
                    out hv_HomMat2D, out hv_Points1, out hv_Points2);
                //*将ImageT进行放射变换以对齐两张图片
                ho_TransImage.Dispose();
                HOperatorSet.ProjectiveTransImage(ho_ImageT, out ho_TransImage, hv_HomMat2D,
                    "bilinear", "false", "false");
                //ho_ImageT.Dispose();
                #endregion

                #region//提取胶圈轮廓
                ////将对齐后点胶之后的图片减去点胶之前图片
                //ho_ImageSub.Dispose();
                //HOperatorSet.SubImage(ho_ImageF, ho_TransImage, out ho_ImageSub, 0.95,110);
                ////**对相减得到的图片进行处理，提出胶圈轮廓
                ////平滑图像
                //ho_ImageGauss.Dispose();
                ////HOperatorSet.GaussFilter(ho_ImageSub, out ho_ImageGauss, 9);
                //HOperatorSet.MedianImage(ho_ImageSub, out ho_ImageGauss,"circle", 2.5,"cyclic");
                //ho_ImageSub.Dispose();
                ////*阈值分割，采用区域生长法，控制参数tol=3, region阈值500000
                //ho_Regions_R.Dispose();
                //HOperatorSet.Regiongrowing(ho_ImageGauss, out ho_Regions_R, 4, 4, tol, area);
                //ho_ImageGauss.Dispose();
                ////对region进行开运算和闭运算，并合并region
                //ho_RegionClosing.Dispose();
                //HOperatorSet.ClosingCircle(ho_Regions_R, out ho_RegionClosing, 30);
                //ho_RegionOpening1.Dispose();
                //HOperatorSet.OpeningCircle(ho_RegionClosing, out ho_RegionOpening1, 11);
                //ho_RegionUnion.Dispose();
                //HOperatorSet.Union1(ho_RegionOpening1, out ho_RegionUnion);

                ho_ImageSub.Dispose();

                //HOperatorSet.AbsDiffImage(ho_ImageF, ho_TransImage, out ho_ImageSub, GlueFindParam.Instance.abs_diff_image_value);
                HOperatorSet.SubImage(ho_ImageF, ho_TransImage, out ho_ImageSub, 0.95, 110);

                try
                {
                    HOperatorSet.WriteImage(ho_ImageSub, "jpg", 0, "C:\\Sub.jpg");
                }
                catch { }
                HObject hpowimage, hscareimage;
                HOperatorSet.GenEmptyObj(out hpowimage);
                HOperatorSet.GenEmptyObj(out hscareimage);
                hpowimage.Dispose();
             
                HOperatorSet.PowImage(ho_ImageSub, out hpowimage,1.0);
              

                hscareimage.Dispose();
                HOperatorSet.ScaleImageMax(hpowimage, out hscareimage);
                //**对相减得到的图片进行处理，提出胶圈轮廓
                //平滑图像
                ho_ImageGauss.Dispose();
                //HOperatorSet.GaussFilter(ho_ImageSub, out ho_ImageGauss, 11);
                HOperatorSet.MedianImage(hscareimage, out ho_ImageGauss, "circle", 2, "cyclic");
                ho_ImageSub.Dispose();
                //*阈值分割，采用区域生长法，控制参数tol=3, region阈值500000
                ho_Regions_R.Dispose();
                HOperatorSet.Regiongrowing(ho_ImageGauss, out ho_Regions_R, 4, 4, tol, area);

                //提取胶圈区域region
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_Height, hv_Width);
                ho_RegionDifference.Dispose();
                //HOperatorSet.Difference(ho_Rectangle, ho_RegionUnion, out ho_RegionDifference);
                HOperatorSet.Difference(ho_Rectangle, ho_Regions_R, out ho_RegionDifference);
                HOperatorSet.CountObj(ho_RegionDifference, out hv_Number1);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 30);
                ho_Regionc.Dispose();
                HOperatorSet.ClosingCircle(ho_RegionOpening, out ho_Regionc, 30);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Regionc, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area", 70);
                ho_RegionFillUp1.Dispose();
                HOperatorSet.FillUpShape(ho_SelectedRegions, out ho_RegionFillUp1, "area", 1, 500);
                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionCircle(ho_RegionFillUp1, out ho_RegionErosion, 2.5);
                ho_Contours1.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionErosion, out ho_Contours1, "border_holes");
                HOperatorSet.CountObj(ho_Contours1, out hv_Number2);
                #endregion
                HOperatorSet.QueryFont(Window, out hv_Font);
                HOperatorSet.SetFont(Window, (hv_Font.TupleSelect(0)) + "-Bold-13");

                #region//计算点胶区域内外圆
                //***********************
                //理论胶圈外圆
                hv_glueOutterCircle = hv_glueInnerCircle + hv_glueWidth;
                //理论胶圈圆心
                hv_glueCenter = new HTuple(2);
                hv_glueCenter[0] = CenterLocate.CenterLoc[1];
                hv_glueCenter[1] = CenterLocate.CenterLoc[0];
                HObject ho_Cross, ho_cir1, ho_cir2;
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.GenEmptyObj(out ho_cir1);
                HOperatorSet.GenEmptyObj(out ho_cir2);
                //xmz  显示 内外圆区域
                HOperatorSet.SetColor(Window, "green");
                HOperatorSet.SetDraw(Window,"margin"); ;
                ho_Cross.Dispose(); ;

                HOperatorSet.GenCrossContourXld(out ho_Cross, CenterLocate.CenterLoc[1], CenterLocate.CenterLoc[0],
                       180, 0.785398);
                HOperatorSet.DispObj(ho_ImageF, Window);
                HOperatorSet.DispObj(ho_Cross, Window);
                ho_cir1.Dispose();
                HOperatorSet.GenCircle(out ho_cir1, CenterLocate.CenterLoc[1], CenterLocate.CenterLoc[0], hv_glueInnerCircle);
                HOperatorSet.DispObj(ho_cir1, Window);
                ho_cir2.Dispose();
                HOperatorSet.GenCircle(out ho_cir2, CenterLocate.CenterLoc[1], CenterLocate.CenterLoc[0], hv_glueOutterCircle);
                HOperatorSet.DispObj(ho_cir2, Window);
                //***********************
                #endregion

                #region//检测胶圈是否封闭
                if ((int)((new HTuple((new HTuple(hv_Number2.TupleEqual(2))).TupleAnd(new HTuple(hv_Number1.TupleEqual(
            1))))).TupleNot()) != 0)
                {
                    is_qualified = false;
                    //HOperatorSet.DispObj(ho_ImageF, Window);
                    HOperatorSet.SetLineWidth(Window, 2);
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.DispObj(ho_Contours1, Window);
                    HOperatorSet.DispText(Window, "无胶或胶圈不完整NG", "window",
                        15, 480, "red", "box","false");
                    SetString(Window, "NG", "red", 100);
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
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Row2.TupleLength())) - 1); hv_i = (int)hv_i + 50)
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
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Row3.TupleLength())) - 1); hv_i = (int)hv_i + 50)
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
                    //平滑轮廓
                    HOperatorSet.SmoothContoursXld(ho_glueOutterContour, out SmoothedGlueOutterContour, 5);
                    HOperatorSet.SmoothContoursXld(ho_glueInnerContour, out SmoothedGlueInnerContour, 5);

                    //HOperatorSet.WriteString()
                    #endregion
                    if ((int)(new HTuple(hv_Distance_offset.TupleGreater(glueOffset_))) != 0)
                    {
                        is_qualified = false;
                        HOperatorSet.SetColor(Window, "red");
                        HOperatorSet.SetLineWidth(Window, 2);
                        HOperatorSet.DispObj(ho_ImageF, Window);
                        HOperatorSet.DispObj(ho_glueOutterContour, Window);
                        HOperatorSet.DispObj(ho_glueInnerContour, Window);
                        //**显示
                        HOperatorSet.DispText(Window, (("胶离内边界最大 / 最小距离：" + hv_Distance_inner_max) + " / ") + hv_Distance_inner_min,
                            "window", 15, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, (("胶离外边界最大 / 最小距离：" + hv_Distance_outter_max) + " / ") + hv_Distance_outter_min,
                            "window", 40, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "内外胶拟合圆宽度：" + hv_Radius,
                            "window", 65, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "最小胶宽度：" + hv_glueWidthMin,
                            "window", 90, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "最大胶宽度：" + hv_glueWidthMax,
                            "window", 115, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "胶偏移距离：" + hv_Distance_offset,
                            "window", 140, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "标准胶宽：" + hv_glueWidth,
                            "window", 165, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "胶圈偏移NG", "window", 40,
                            400, "red", "box",  "false");
                        SetString(Window, "NG", "red", 100); ;
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
                        
                        HOperatorSet.SetColor(Window, "green");
                        HOperatorSet.SetLineWidth(Window, 2);
                        HOperatorSet.DispObj(ho_ImageF, Window);
                        HOperatorSet.DispObj(ho_glueOutterContour, Window);
                        HOperatorSet.DispObj(ho_glueInnerContour, Window);
                        if ((int)(new HTuple(hv_Distance_outter_max.TupleGreater(glueoverflow_Outter))) != 0)
                        {
                            HOperatorSet.DispText(Window, "外胶溢胶!NG", "window", 15, 400, "red", "box",  "false");
                            is_qualified = false;
                        }
                        if ((int)(new HTuple(((hv_Distance_outter_min.TupleAbs())).TupleGreater(glueLack_Outter))) != 0)
                        {
                            HOperatorSet.DispText(Window, "外胶缺胶!NG", "window", 40, 400, "red", "box",  "false");
                            is_qualified = false;
                        }
                        if ((int)(new HTuple(((hv_Distance_inner_min.TupleAbs())).TupleGreater(glueoverflow_Inner))) != 0)
                        {
                            HOperatorSet.DispText(Window, "内胶溢胶!NG", "window", 65, 400, "red", "box",  "false");
                            is_qualified = false;
                        }
                        if ((int)(new HTuple(hv_Distance_inner_max.TupleGreater(glueLack_Inner))) != 0)
                        {
                            HOperatorSet.DispText(Window, "内胶缺胶!NG", "window", 90, 400, "red", "box",  "false");
                            is_qualified = false;
                        }
                        //if (hv_glueWidthMax > (hv_glueWidth + (glueOverflowOutter + glueOverflowInner) / 2.0))
                        //{
                        //    HOperatorSet.DispText(Window, "最大胶宽度超出！NG", "window", 115, 400, "red", "box", "false");
                        //    is_qualified = false;
                        //}
                        //if (hv_glueWidthMin < (hv_glueWidth - (glueLackOutter + glueLackInner) / 2.0))
                        //{
                        //    HOperatorSet.DispText(Window, "最小胶宽度超出！NG", "window", 140, 400, "red", "box", "false");
                        //    is_qualified = false;
                        //}
                        //**显示
                        HOperatorSet.DispText(Window, (("胶离内边界最大 / 最小距离：" + hv_Distance_inner_max) + " / ") + hv_Distance_inner_min,
                            "window", 15, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, (("胶离外边界最大 / 最小距离：" + hv_Distance_outter_max) + " / ") + hv_Distance_outter_min,
                            "window", 40, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "内外胶拟合圆宽度：" + hv_Radius,
                            "window", 65, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "最小胶宽度：" + hv_glueWidthMin,
                            "window", 90, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "最大胶宽度：" + hv_glueWidthMax,
                            "window", 115, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "胶偏移距离：" + hv_Distance_offset,
                            "window", 140, 7, "green", "box", "false");
                        HOperatorSet.DispText(Window, "标准胶宽：" + hv_glueWidth,
                            "window", 165, 7, "green", "box", "false");

                      if(is_qualified) SetString(Window, "OK", "green", 100);
                        else SetString(Window, "NG", "red", 100); 
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
            catch(HalconException ex) { SetString(Window, "NG", "red", 100); HOperatorSet.DispText(Window, ex.GetErrorMessage(), "window", 100, 0, "red", new HTuple(), new HTuple()); return false; }
        }

        public static void GlueCheck_C(Bitmap bmp, HWindow window, bool ok, double[] distance)
        {
            try
            {
                HObject image, img_old;
                HOperatorSet.GenEmptyObj(out image);
                HOperatorSet.GenEmptyObj(out img_old);
                img_old.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp, out img_old);

                HTuple htuple;
                HOperatorSet.CountChannels(img_old, out htuple);
                image.Dispose();
                if (htuple == 3) HOperatorSet.Rgb1ToGray(img_old, out image);
                else image = img_old.Clone();

                HTuple width, height;
                HOperatorSet.GetImageSize(image, out width, out height);
                HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                HOperatorSet.DispObj(image, window);
                HOperatorSet.SetLineWidth(window, 3);
                HOperatorSet.SetColor(window, "red");
                HOperatorSet.DispText(window, "胶水外边缘最大偏差：" + System.Math.Round(distance[0], 4) + " pix",
                     "window", 30, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "胶水内边缘最大偏差：" + System.Math.Round(distance[1], 4) + " pix",
                    "window", 60, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "胶水外边缘阈值：" + System.Math.Round(Position.Instance.OutsideDistance, 4) + " pix",
                     "window", 90, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "胶水内边缘阈值：" + System.Math.Round(Position.Instance.insideDistance, 4) + " pix",
                    "window", 120, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "胶圈X轴偏移：" + System.Math.Round(distance[2], 4) + " mm",
                     "window", 150, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "胶圈Y轴偏移：" + System.Math.Round(distance[3], 4) + "mm",
                    "window", 180, 7, "black", new HTuple(), new HTuple());
                if (ok)
                {
                    //LastCenterLocateBMP = bmp;
                    SetString(window, "OK", "green", 100);
                }
                else
                {
                    SetString(window, "NG", "red", 100);
                }
            }
            catch
            {
                try
                {
                    SetString(window, "NG", "red", 100);
                }
                catch { }
                Marking.CenterLocateTestSucceed = false;
            }
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
