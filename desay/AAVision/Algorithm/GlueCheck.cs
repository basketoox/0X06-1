using desay.AAVision.Algorithm;
using desay.ProductData;
using HalconDotNet;
using System.Drawing;
using Vision_Assistant;

namespace desay
{
    static class GlueCheck
    {
        #region//控制参数

        #region//胶水判断参数
        //胶宽
        private static HTuple hv_glueWidth = new HTuple(135);
        public static HTuple GlueWidth { get { return hv_glueWidth; } set { hv_glueWidth = value; } }
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
        //阈值分割
        private static HTuple hv_threshold_gray_min = new HTuple(65.0);
        private static HTuple hv_threshold_gray_max = new HTuple(255.0);
        public static double threshold_min
        {
            get { return hv_threshold_gray_min[0]; }
            set { hv_threshold_gray_min[0] = value; }
        }
        public static double threshold_max
        {
            get { return hv_threshold_gray_max[0]; }
            set { hv_threshold_gray_max[0] = value; }
        }

        //Shape面积
        private static HTuple _Minarea = new HTuple(300000);
        public static double Shapearea_min
        {
            get { return _Minarea[0]; }
            set { _Minarea[0] = value; }
        }
        //Shape面积
        private static HTuple _Maxarea = new HTuple(500000);
        public static double Shapearea_max
        {
            get { return _Maxarea[0]; }
            set { _Maxarea[0] = value; }
        }
        //填充面积
        private static HTuple _fillarea = new HTuple(100000.0);
        public static double Fillarea
        {
            get { return _fillarea[0]; }
            set { _fillarea[0] = value; }
        }
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
        public static void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant)
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
                bool result = true;

                #region 声明图像变量
                HObject ho_Image = null, ho_ROI_0 = null, ho_TMP_Region = null;
                HObject ho_ImageReduced = null, ho_R = null, ho_G = null, ho_B = null;
                HObject ho_ImageMean = null, ho_Regions = null, ho_ConnectedRegions = null;
                HObject ho_SelectedRegions1 = null, ho_Contours = null, ho_SelectedContours = null;
                HObject ho_Contour = null, ho_Image2 = null, ho_ImageAbsDiff = null;
                HObject ho_ImageReduced1 = null, ho_R1 = null, ho_G1 = null, ho_B1 = null;
                HObject ho_Regions1 = null, ho_RegionFillUp1 = null, ho_ConnectedRegions1 = null;
                HObject ho_RegionClosing1 = null, ho_Contours1 = null, ho_glueOutterContour = null;
                HObject ho_glueInnerContour = null, ho_Contour_I_sap = null;
                HObject ho_Contour_O_sap = null;
                #endregion

                #region 初始化图像变量
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenEmptyObj(out ho_ROI_0);
                HOperatorSet.GenEmptyObj(out ho_TMP_Region);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_R);
                HOperatorSet.GenEmptyObj(out ho_G);
                HOperatorSet.GenEmptyObj(out ho_B);
                HOperatorSet.GenEmptyObj(out ho_ImageMean);
                HOperatorSet.GenEmptyObj(out ho_Regions);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
                HOperatorSet.GenEmptyObj(out ho_Contours);
                HOperatorSet.GenEmptyObj(out ho_SelectedContours);
                HOperatorSet.GenEmptyObj(out ho_Contour);
                HOperatorSet.GenEmptyObj(out ho_Image2);
                HOperatorSet.GenEmptyObj(out ho_ImageAbsDiff);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
                HOperatorSet.GenEmptyObj(out ho_R1);
                HOperatorSet.GenEmptyObj(out ho_G1);
                HOperatorSet.GenEmptyObj(out ho_B1);
                HOperatorSet.GenEmptyObj(out ho_Regions1);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
                HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
                HOperatorSet.GenEmptyObj(out ho_Contours1);
                HOperatorSet.GenEmptyObj(out ho_glueOutterContour);
                HOperatorSet.GenEmptyObj(out ho_glueInnerContour);
                HOperatorSet.GenEmptyObj(out ho_Contour_I_sap);
                HOperatorSet.GenEmptyObj(out ho_Contour_O_sap);

                #endregion

                #region 初始化控制变量 
                HTuple hv_is_opened = new HTuple(), hv_glueoverflow_O = new HTuple();
                HTuple hv_glueoverflow_I = new HTuple(), hv_gluelack_O = new HTuple();
                HTuple hv_gluelack_I = new HTuple(), hv_glueoffset = new HTuple();
                HTuple hv_glueWidth = new HTuple(), hv_Number = new HTuple();
                HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
                HTuple hv_Radius = new HTuple(), hv_StartPhi = new HTuple();
                HTuple hv_EndPhi = new HTuple(), hv_PointOrder = new HTuple();
                HTuple hv_thvalue = new HTuple(), hv_fillarea = new HTuple();
                HTuple hv_areavalue = new HTuple(), hv_Number1 = new HTuple();
                HTuple hv_Number2 = new HTuple(), hv_glueCenter = new HTuple();
                HTuple hv_glueInnerCircle = new HTuple(), hv_glueOutterCircle = new HTuple();
                HTuple hv_Row2 = new HTuple(), hv_Col1 = new HTuple();
                HTuple hv_Row3 = new HTuple(), hv_Col2 = new HTuple();
                HTuple hv_Row_Inner_sampling = new HTuple(), hv_Col_Inner_sampling = new HTuple();
                HTuple hv_k = new HTuple(), hv_i = new HTuple(), hv_Row_Outter_sampling = new HTuple();
                HTuple hv_Col_Outter_sampling = new HTuple(), hv_DistanceMin = new HTuple();
                HTuple hv_DistanceMax = new HTuple(), hv_glueWidthMin = new HTuple();
                HTuple hv_glueWidthMax = new HTuple(), hv_DistanceMin1 = new HTuple();
                HTuple hv_DistanceMax1 = new HTuple(), hv_Distance_inner_min = new HTuple();
                HTuple hv_Distance_inner_max = new HTuple(), hv_DistanceMin2 = new HTuple();
                HTuple hv_DistanceMax2 = new HTuple(), hv_Distance_outter_min = new HTuple();
                HTuple hv_Distance_outter_max = new HTuple(), hv_Row_Outter = new HTuple();
                HTuple hv_Column_Outter = new HTuple(), hv_Radius_Outter = new HTuple();
                HTuple hv_StartPhi1 = new HTuple(), hv_EndPhi1 = new HTuple();
                HTuple hv_PointOrder1 = new HTuple(), hv_Row_Inner = new HTuple();
                HTuple hv_Column_Inner = new HTuple(), hv_Radius_Inner = new HTuple();
                HTuple hv_StartPhi2 = new HTuple(), hv_EndPhi2 = new HTuple();
                HTuple hv_PointOrder2 = new HTuple(), hv_Row_glue = new HTuple();
                HTuple hv_Col_glue = new HTuple(), hv_Distance_offset = new HTuple();
                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                HTuple hv_WindowHandle = new HTuple(), hv_Font = new HTuple();
                HTuple htuple = new HTuple(), htuple1 = new HTuple();
                #endregion

                #region 判断参数
                //胶圈是否断开
                hv_is_opened = 1;
                //胶水外溢判断阈值
                hv_glueoverflow_O = glueOverflowOutter;
                //胶水内溢判断阈值
                hv_glueoverflow_I = glueOverflowInner;
                //胶水外圈缺胶判断胶阈值
                hv_gluelack_O = glueLackOutter;
                //胶水内圈缺胶判断阈值
                hv_gluelack_I = glueLackInner;
                //胶圈偏移判断阈值
                hv_glueoffset = glueOffset;
                //理论胶宽
                hv_glueWidth = GlueWidth;
                #endregion

                ho_Image.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp_with_no_glue, out ho_Image);
                //24转8
                ho_R.Dispose(); ho_G.Dispose(); ho_B.Dispose();
                HOperatorSet.CountChannels(ho_Image, out htuple);
                if (htuple == 3)
                {
                    HOperatorSet.Decompose3(ho_Image, out ho_R, out ho_G, out ho_B);
                }
                else
                {
                    ho_B = ho_Image.Clone();
                }
                //读取圆心Region
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                try
                {
                    ho_ROI_0.Dispose();
                    HOperatorSet.ReadRegion(out ho_ROI_0, AppConfig.VisionLocateROI);
                    ho_TMP_Region.Dispose();
                    HOperatorSet.ReadRegion(out ho_TMP_Region, AppConfig.VisionLocateROI_Out);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.SymmDifference(ho_ROI_0, ho_TMP_Region, out ExpTmpOutVar_0);
                        ho_ROI_0.Dispose();
                        ho_ROI_0 = ExpTmpOutVar_0;
                    }
                }
                catch
                {
                    HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispText(Window, "胶水检查失败,圆心ROI文件不存在！", "window", 12, 7, "black", new HTuple(), new HTuple());
                    ho_Image.Dispose();
                    ho_ROI_0.Dispose();
                    ho_TMP_Region.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_R.Dispose();
                    ho_G.Dispose();
                    ho_B.Dispose();
                    ho_ImageMean.Dispose();
                    ho_Regions.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions1.Dispose();
                    ho_Contours.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_Contour.Dispose();
                    ho_Image2.Dispose();
                    ho_ImageAbsDiff.Dispose();
                    ho_ImageReduced1.Dispose();
                    ho_R1.Dispose();
                    ho_G1.Dispose();
                    ho_B1.Dispose();
                    ho_Regions1.Dispose();
                    ho_RegionFillUp1.Dispose();
                    ho_ConnectedRegions1.Dispose();
                    ho_RegionClosing1.Dispose();
                    ho_Contours1.Dispose();
                    ho_glueOutterContour.Dispose();
                    ho_glueInnerContour.Dispose();
                    ho_Contour_I_sap.Dispose();
                    ho_Contour_O_sap.Dispose();
                    return false;
                }
                //获取无胶圆心坐标
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_B, ho_ROI_0, out ho_ImageReduced);
                ho_ImageMean.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, 10, 10);
                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageMean, out ho_Regions, CenterLocate.threshold_min, CenterLocate.threshold_max);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "area", "and", CenterLocate.areaMin, CenterLocate.areaMax);
                HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
                {
                    ho_Contours.Dispose();
                    HOperatorSet.GenContourRegionXld(ho_SelectedRegions1, out ho_Contours, "border_holes");
                    ho_SelectedContours.Dispose();
                    HOperatorSet.SelectContoursXld(ho_Contours, out ho_SelectedContours, "contour_length", 1000, 20000, -0.5, 0.5);
                    ho_Contour.Dispose();
                    HOperatorSet.SelectObj(ho_SelectedContours, out ho_Contour, 2);
                    HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_Row,
                        out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                }
                else
                {
                    HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                    HOperatorSet.DispObj(ho_Image, Window);
                    HOperatorSet.DispText(Window, "圆心定位失败，阈值设置不正确！", "window", 12, 7, "black", new HTuple(), new HTuple());
                    ho_Image.Dispose();
                    ho_ROI_0.Dispose();
                    ho_TMP_Region.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_R.Dispose();
                    ho_G.Dispose();
                    ho_B.Dispose();
                    ho_ImageMean.Dispose();
                    ho_Regions.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions1.Dispose();
                    ho_Contours.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_Contour.Dispose();
                    ho_Image2.Dispose();
                    ho_ImageAbsDiff.Dispose();
                    ho_ImageReduced1.Dispose();
                    ho_R1.Dispose();
                    ho_G1.Dispose();
                    ho_B1.Dispose();
                    ho_Regions1.Dispose();
                    ho_RegionFillUp1.Dispose();
                    ho_ConnectedRegions1.Dispose();
                    ho_RegionClosing1.Dispose();
                    ho_Contours1.Dispose();
                    ho_glueOutterContour.Dispose();
                    ho_glueInnerContour.Dispose();
                    ho_Contour_I_sap.Dispose();
                    ho_Contour_O_sap.Dispose();
                    return false;
                }
                //读取有胶图片
                ho_Image2.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp_with_glue, out ho_Image2);
                //图像差值
                ho_ImageAbsDiff.Dispose();
                HOperatorSet.AbsDiffImage(ho_Image, ho_Image2, out ho_ImageAbsDiff, 1);
                //设置胶水Region
                ho_ROI_0.Dispose();
                HOperatorSet.GenCircle(out ho_ROI_0, hv_Row, hv_Column, hv_Radius - hv_glueWidth - 100);
                ho_TMP_Region.Dispose();
                HOperatorSet.GenCircle(out ho_TMP_Region, hv_Row, hv_Column, hv_Radius + 100);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SymmDifference(ho_ROI_0, ho_TMP_Region, out ExpTmpOutVar_0);
                    ho_ROI_0.Dispose();
                    ho_ROI_0 = ExpTmpOutVar_0;
                }
                //裁剪图片
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageAbsDiff, ho_ROI_0, out ho_ImageReduced1);
                ho_R1.Dispose(); ho_G1.Dispose(); ho_B1.Dispose();
                HOperatorSet.CountChannels(ho_Image2, out htuple1);
                if (htuple1 == 3)
                {
                    HOperatorSet.Decompose3(ho_ImageReduced1, out ho_R1, out ho_G1, out ho_B1);
                }
                else
                {
                    ho_B1 = ho_ImageReduced1.Clone();
                }
                //获取轮廓
                ho_Regions1.Dispose();
                HOperatorSet.Threshold(ho_B1, out ho_Regions1, threshold_min, threshold_max);
                ho_RegionFillUp1.Dispose();
                HOperatorSet.FillUpShape(ho_Regions1, out ho_RegionFillUp1, "area", 1, Fillarea);
                ho_ConnectedRegions1.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp1, out ho_ConnectedRegions1);
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "area", "and", Shapearea_min, Shapearea_max);
                ho_RegionClosing1.Dispose();
                HOperatorSet.ClosingCircle(ho_SelectedRegions1, out ho_RegionClosing1, 25);                
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_RegionClosing1, out ho_Contours, "border_holes");
                ho_Contours1.Dispose();
                HOperatorSet.SelectContoursXld(ho_Contours, out ho_Contours1, "contour_length", 200, 20000, -0.5, 0.5);
                //判断胶圈是否封闭
                HOperatorSet.CountObj(ho_Contours1, out hv_Number1);
                if ((int)((new HTuple(hv_Number1.TupleEqual(2))).TupleNot()) != 0)
                {
                    hv_is_opened = 1;
                    HOperatorSet.SetLineWidth(Window, 2);
                    HOperatorSet.SetColor(Window, "red");
                    HOperatorSet.DispObj(ho_Image2, Window);
                    HOperatorSet.DispObj(ho_Contours, Window);
                    HOperatorSet.DispText(Window, "胶圈不完整！", "window", 12, 7, "black", new HTuple(), new HTuple());
                    return false;
                }
                else
                {
                    hv_is_opened = 0;
                    //理论胶圈圆心
                    hv_glueCenter = new HTuple();
                    hv_glueCenter = hv_glueCenter.TupleConcat(hv_Row);
                    hv_glueCenter = hv_glueCenter.TupleConcat(hv_Column);
                    //理论胶圈内圆
                    hv_glueInnerCircle = hv_Radius - hv_glueWidth;
                    //理论胶圈外圆
                    hv_glueOutterCircle = hv_Radius.Clone();
                    //选择对应轮廓
                    ho_glueOutterContour.Dispose();
                    HOperatorSet.SelectObj(ho_Contours, out ho_glueOutterContour, 1);
                    ho_glueInnerContour.Dispose();
                    HOperatorSet.SelectObj(ho_Contours, out ho_glueInnerContour, 2);
                    //轮廓坐标
                    HOperatorSet.GetContourXld(ho_glueInnerContour, out hv_Row2, out hv_Col1);
                    HOperatorSet.GetContourXld(ho_glueOutterContour, out hv_Row3, out hv_Col2);
                    //采样
                    hv_Row_Inner_sampling = new HTuple();
                    hv_Col_Inner_sampling = new HTuple();
                    hv_k = 0;
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Row2.TupleLength())) - 1); hv_i = (int)hv_i + 25)
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
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Row3.TupleLength())) - 1); hv_i = (int)hv_i + 25)
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
                    //获取新的轮廓
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
                    //*胶水离内轮廓边界的距离最大值最小值
                    HOperatorSet.DistancePc(ho_glueInnerContour, hv_glueCenter.TupleSelect(0),
                        hv_glueCenter.TupleSelect(1), out hv_DistanceMin1, out hv_DistanceMax1);
                    hv_Distance_inner_min = hv_DistanceMin1 - hv_glueInnerCircle;
                    hv_Distance_inner_max = hv_DistanceMax1 - hv_glueInnerCircle;
                    //*胶水离外轮廓边界的距离最大最小值
                    HOperatorSet.DistancePc(ho_glueOutterContour, hv_glueCenter.TupleSelect(0),
                        hv_glueCenter.TupleSelect(1), out hv_DistanceMin2, out hv_DistanceMax2);
                    hv_Distance_outter_min = hv_DistanceMin2 - hv_glueOutterCircle;
                    hv_Distance_outter_max = hv_DistanceMax2 - hv_glueOutterCircle;
                    //*用胶轮廓与拟合圆间的最大距离判断缺胶溢胶情况
                    HOperatorSet.FitCircleContourXld(ho_glueOutterContour, "algebraic", -1, 0,
                        0, 3, 2, out hv_Row_Outter, out hv_Column_Outter, out hv_Radius_Outter,
                        out hv_StartPhi1, out hv_EndPhi1, out hv_PointOrder1);
                    HOperatorSet.FitCircleContourXld(ho_glueInnerContour, "algebraic", -1, 0,
                        0, 3, 2, out hv_Row_Inner, out hv_Column_Inner, out hv_Radius_Inner,
                        out hv_StartPhi2, out hv_EndPhi2, out hv_PointOrder2);
                    //*利用拟合内外胶的圆心的平均值与拟合基准圆的圆心坐标判断偏移情况
                    hv_Row_glue = (hv_Row_Outter + hv_Row_Inner) / 2;
                    hv_Col_glue = (hv_Column_Outter + hv_Column_Inner) / 2;
                    HOperatorSet.DistancePp(hv_glueCenter.TupleSelect(0), hv_glueCenter.TupleSelect(
                        1), hv_Row_glue, hv_Col_glue, out hv_Distance_offset);
                    //*算出拟合胶水的宽度
                    hv_Radius = hv_Radius_Outter - hv_Radius_Inner;
                    //**显示
                    HOperatorSet.GetImageSize(ho_Image2, out hv_Width, out hv_Height);
                    HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
                    HOperatorSet.DispObj(ho_Image2, Window);
                    HOperatorSet.QueryFont(Window, out hv_Font);
                    HOperatorSet.SetFont(Window, (hv_Font.TupleSelect(0)) + "-Bold-15");
                    HOperatorSet.SetColor(Window, "green");
                    HOperatorSet.SetLineWidth(Window, 2);
                    HOperatorSet.DispObj(ho_glueOutterContour, Window);
                    HOperatorSet.DispObj(ho_glueInnerContour, Window);
                    HOperatorSet.DispText(Window, (("胶离内边界最大 / 最小距离：" + hv_Distance_inner_max) + " / ") + hv_Distance_inner_min, "window", 15, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(Window, (("胶离外边界最大 / 最小距离：" + hv_Distance_outter_max) + " / ") + hv_Distance_outter_min, "window", 40, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(Window, "最小胶宽度：" + hv_glueWidthMin, "window", 65, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(Window, "最大胶宽度：" + hv_glueWidthMax, "window", 90, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(Window, "内外胶拟合圆宽度：" + hv_Radius, "window", 115, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(Window, "胶偏移距离：" + hv_Distance_offset, "window", 140, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(Window, "最大胶宽：" + hv_glueWidth, "window", 165, 7, "black", new HTuple(), new HTuple());
                    if ((int)(new HTuple(hv_Distance_offset.TupleGreater(hv_glueoffset))) != 0)
                    {
                        HOperatorSet.DispText(Window, "胶圈偏移", "window", 240, 300, "red", new HTuple(), new HTuple());
                        result = false;
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Distance_outter_max.TupleGreater(hv_glueoverflow_O))) != 0)
                        {
                            result = false;
                            HOperatorSet.DispText(Window, "外圈溢胶", "window", 215, 300, "red", new HTuple(), new HTuple());
                        }
                        if ((int)(new HTuple(((hv_Distance_outter_min.TupleAbs())).TupleGreater(hv_gluelack_O))) != 0)
                        {
                            result = false;
                            HOperatorSet.DispText(Window, "外圈缺胶", "window", 240, 300, "red", new HTuple(), new HTuple());
                        }
                        if ((int)(new HTuple(((hv_Distance_inner_min.TupleAbs())).TupleGreater(hv_glueoverflow_I))) != 0)
                        {
                            result = false;
                            HOperatorSet.DispText(Window, "内圈溢胶", "window", 265, 300, "red", new HTuple(), new HTuple());
                        }
                        if ((int)(new HTuple(hv_Distance_inner_max.TupleGreater(hv_gluelack_I))) != 0)
                        {
                            result = false;
                            HOperatorSet.DispText(Window, "内圈缺胶", "window", 290, 300, "red", new HTuple(), new HTuple());
                        }
                    }
                }
                ho_Image.Dispose();
                ho_ROI_0.Dispose();
                ho_TMP_Region.Dispose();
                ho_ImageReduced.Dispose();
                ho_R.Dispose();
                ho_G.Dispose();
                ho_B.Dispose();
                ho_ImageMean.Dispose();
                ho_Regions.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_Contours.Dispose();
                ho_SelectedContours.Dispose();
                ho_Contour.Dispose();
                ho_Image2.Dispose();
                ho_ImageAbsDiff.Dispose();
                ho_ImageReduced1.Dispose();
                ho_R1.Dispose();
                ho_G1.Dispose();
                ho_B1.Dispose();
                ho_Regions1.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_RegionClosing1.Dispose();
                ho_Contours1.Dispose();
                ho_glueOutterContour.Dispose();
                ho_glueInnerContour.Dispose();
                ho_Contour_I_sap.Dispose();
                ho_Contour_O_sap.Dispose();

                return result;
            }
            catch (HalconException ex)
            {
                HOperatorSet.DispText(Window, ex.GetErrorMessage(), "window", 0, 10, "red", new HTuple(), new HTuple());
                return false;
            }
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
            }
        }

        public static void GlueCheck_R(Bitmap bmp, HWindow window, bool ok)
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
                HOperatorSet.DispText(window, "胶路面积：" + RectGlueCheck.TotalAreas.ToString("") + " pix",
                     "window", 30, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "等效矩形长宽：" + RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 2].ToString("")
                                                      + " ，" + RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 3].ToString(""), "window", 60, 7, "black", new HTuple(), new HTuple());
                double MassPos_X = RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 0];
                double MassPos_Y = RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 1];
                HOperatorSet.DispText(window, "胶圈质心X偏移：" + System.Math.Round(MassPos_X - 1296, 0) + " pix",
                     "window", 90, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "胶圈质心Y偏移：" + System.Math.Round(MassPos_Y - 972, 0) + " pix",
                    "window", 120, 7, "black", new HTuple(), new HTuple());
                if (ok)
                {
                    SetString(window, "OK", "green", 100);
                }
                else
                {
                    SetString(window, "NG", "red", 100);
                }

                HObject rect;
                HTuple Length1 = Position.Instance.CenterOffset_X;
                HTuple Length2 = Position.Instance.CenterOffset_Y;
                HTuple phi = 0;
                HTuple row = 972;
                HTuple column = 1296;
                HOperatorSet.SetLineWidth(window, 3);
                HOperatorSet.SetDraw(window, "margin");
                HOperatorSet.GenRectangle2(out rect, row, column, phi, Length1, Length2);
                HOperatorSet.DispObj(rect, window);

                HObject circ;
                HOperatorSet.GenCircle(out circ, MassPos_Y, MassPos_X, 5);
                HOperatorSet.DispObj(circ, window);


            }
            catch
            {
                try
                {
                    SetString(window, "NG", "red", 100);
                }
                catch { }
            }
        }

        public static void TestBmp(Bitmap no_glue_bmp, Bitmap glue_bmp, HWindow hWindow, bool save)
        {
            Marking.GlueCheckResult = action(no_glue_bmp, glue_bmp, hWindow);

            if (Marking.GlueCheckResult)
            {
                SetString(hWindow, "OK", "green", 100);
            }
            else
            {
                SetString(hWindow, "NG", "red", 100);
            }
            if (save)
            {
                SaveImage.Save(hWindow);
            }

        }
    }
}
