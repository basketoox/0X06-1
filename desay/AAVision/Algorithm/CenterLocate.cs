using desay.AAVision.Algorithm;
using desay.ProductData;
using HalconDotNet;
using log4net;
using System;
using System.Drawing;
namespace desay
{
    static class CenterLocate
    {
        public static ILog log = LogManager.GetLogger(typeof(CenterLocate));
        public static double[] CenterLoc = new double[2];
        /// <summary>
        /// 存储上一次圆心识别的图片，用于胶水识别中的模板匹配
        /// </summary>
        public static Bitmap LastCenterLocateBMP; 

        #region // 控制参数
        //*圆区域控制参数
        //圆心坐标
        private static HTuple hv_circle_center_x = new HTuple(990.0);
        private static HTuple hv_circle_center_y = new HTuple(1200.0);
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
        //内外圆半径偏差
        private static HTuple hv_offset = new HTuple(-280.0);
        public static double rdaiusOffset
        {
            get { return hv_offset[0]; }
            set { hv_offset[0] = value; }
        }
        //分割阈值
        private static HTuple hv_threshold_gray_min = new HTuple(0.0);
        private static HTuple hv_threshold_gray_max = new HTuple(100.0);
        public static double threshold_min
        {
            get { return hv_threshold_gray_min[0]; }
            set { hv_threshold_gray_min = value; }
        }
        public static double threshold_max
        {
            get { return hv_threshold_gray_max; }
            set { hv_threshold_gray_max = value; }
        }
        //腐蚀核大小
        private static HTuple ero = new HTuple(5.0);
        public static double eroKernel
        {
            get { return ero; }
            set { ero = value; }
        }
        //选取的区域面积
        private static HTuple _areaMin = new HTuple(160000.0);
        public static double areaMin
        {
            get { return _areaMin[0]; }
            set { _areaMin[0] = value; }
        }
        private static HTuple _areaMax = new HTuple(250000.0);
        public static double areaMax
        {
            get { return _areaMax[0]; }
            set { _areaMax[0] = value; }
        }
        #endregion

        /// <summary>
        /// 显示图像
        /// </summary>
        public static void ShowIMg(Bitmap bmp, HWindow window)
        {
            try
            {
                HObject image = null;
                HOperatorSet.GenEmptyObj(out image);
                image.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp, out image);
                HTuple width, height;
                HOperatorSet.GetImageSize(image, out width, out height);
                HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                HOperatorSet.DispObj(image, window);

            }
            catch
            {

            }
        }

        /// <summary>
        /// 圆心模板匹配
        /// </summary>
        public static double[] CenterMatch(Bitmap bmp, HWindow window)
        {
            try
            {
                LastCenterLocateBMP = bmp;
                double[] data = new double[] { -100, -100 };

                #region 变量
                // Local iconic variables  
                HObject ho_Image, ho_ROI_0, ho_TMP_Region;
                HObject ho_ImageReduced, ho_R, ho_G, ho_B, ho_ImageMean;
                HObject ho_Regions, ho_ConnectedRegions, ho_SelectedRegions1;
                HObject ho_Contours = null, ho_Contour = null, ho_ContCircle;
                HObject ho_Cross, ho_SelectedContours = null;
                // Local control variables 
                HTuple htuple, hv_Number = null, hv_Row = new HTuple();
                HTuple hv_Column = new HTuple(), hv_Radius = new HTuple();
                HTuple hv_StartPhi = new HTuple(), hv_EndPhi = new HTuple();
                HTuple hv_PointOrder = new HTuple();
                #endregion

                #region 变量初始化
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
                HOperatorSet.GenEmptyObj(out ho_ContCircle);
                HOperatorSet.GenEmptyObj(out ho_Cross);
                #endregion                

                #region 获取圆心坐标
                ho_Image.Dispose();
                //bmp转HObiect
                Bitmap2HObject.Bitmap2HObj(bmp, out ho_Image);
                //转灰度图                
                HOperatorSet.CountChannels(ho_Image, out htuple);

                if (htuple == 3)
                {
                    ho_R.Dispose();
                    ho_G.Dispose();
                    ho_B.Dispose();
                    HOperatorSet.Decompose3(ho_Image, out ho_R, out ho_G, out ho_B);
                }
                else
                {
                    ho_B.Dispose();
                    ho_B = ho_Image.Clone();
                }

                HTuple width = null, height = null;
                HOperatorSet.GetImageSize(ho_B, out width, out height);

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
                    HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                    HOperatorSet.DispObj(ho_Image, window);
                    HOperatorSet.DispText(window, "圆心定位失败,ROI文件不存在！", "window", 12, 7, "black", new HTuple(), new HTuple());
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
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    return data;
                }

                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_B, ho_ROI_0, out ho_ImageReduced);
                ho_ImageMean.Dispose();
                HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, 10, 10);
                ho_Regions.Dispose();
                HOperatorSet.Threshold(ho_ImageMean, out ho_Regions, threshold_min, threshold_max);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "area", "and", areaMin, areaMax);
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
                    ho_ContCircle.Dispose();
                    HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_Row, hv_Column, hv_Radius, 0, 6.28318, "positive", 1);

                    data[0] = hv_Column;
                    data[1] = hv_Row;
                }
                else
                {
                    HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                    HOperatorSet.DispObj(ho_Image, window);
                    HOperatorSet.DispText(window, "圆心定位失败，阈值设置不正确！", "window", 12, 7, "black", new HTuple(), new HTuple());
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
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    return data;
                }

                #endregion

                #region 显示图像

                CenterLoc[0] = data[0];
                CenterLoc[1] = data[1];
                data[0] = (data[0] - width / 2) * Config.Instance.CameraPixelMM_X;
                data[1] = (data[1] - height / 2) * Config.Instance.CameraPixelMM_Y;
                //边缘
                HOperatorSet.SetDraw(window, "margin");
                HOperatorSet.SetLineWidth(window, 3);
                HOperatorSet.SetColor(window, "red");
                HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                HOperatorSet.DispObj(ho_Image, window);
                HOperatorSet.DispObj(ho_ContCircle, window);
                //中心
                HOperatorSet.SetColor(window, "green");
                HOperatorSet.GenEmptyObj(out ho_Cross);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, CenterLoc[1], CenterLoc[0],
                    180, 0.785398);
                HOperatorSet.DispObj(ho_Cross, window);
                //文字
                HOperatorSet.DispText(window, "X像素：" + System.Math.Round(CenterLoc[0], 3) + " pixel" + "    Y像素：" + System.Math.Round(CenterLoc[1], 3) + " pixel",
                                     "window", 10, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "X方向偏差：" + System.Math.Round(data[0], 4) + " mm",
                     "window", 30, 7, "black", new HTuple(), new HTuple());
                HOperatorSet.DispText(window, "Y方向偏差：" + System.Math.Round(data[1], 4) + " mm",
                    "window", 50, 7, "black", new HTuple(), new HTuple());

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
                ho_ContCircle.Dispose();
                ho_Cross.Dispose();

                return data;

                #endregion

            }
            catch (Exception ex)
            {
                HOperatorSet.DispText(window, "圆心定位失败！" + ex.Message, "window", 12, 7, "black", new HTuple(), new HTuple());
                log.Debug("圆心定位异常！" + ex.Message + ex.StackTrace);
                return new double[] { -100, -100 };
            }
        }

        public static void RectangleMatch(Bitmap bmp, HWindow window, bool ok, double a, double b)
        {
            try
            {
                HObject image;
                HOperatorSet.GenEmptyObj(out image);
                image.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp, out image);
                HTuple width, height;
                HOperatorSet.GetImageSize(image, out width, out height);
                HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                HOperatorSet.DispObj(image, window);
                HOperatorSet.SetLineWidth(window, 3);
                if (ok)
                {
                    HOperatorSet.DispText(window, "矩形X方向偏差：" + System.Math.Round(AcqToolEdit.offset_x / 96, 4) + " mm",
                     "window", 30, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(window, "矩形Y方向偏差：" + System.Math.Round(AcqToolEdit.offset_y / 96, 4) + " mm",
                        "window", 50, 7, "black", new HTuple(), new HTuple());
                    Marking.CenterLocateTestSucceed = true;
                    SetString(window, "OK", "green", 100);
                }
                else
                {
                    SetString(window, "NG", "red", 100);
                }
                HObject region;
                HTuple Length1 = 200;
                HTuple Length2 = 150;
                HTuple phi = 0;
                HTuple row = b;
                HTuple column = a;
                HOperatorSet.SetColor(window, "red");
                HOperatorSet.SetLineWidth(window, 1);
                HOperatorSet.SetDraw(window, "margin");
                HOperatorSet.GenRectangle2(out region, row, column, phi, Length1, Length2);
                HOperatorSet.DispObj(region, window);
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

        public static void CircularMatch(Bitmap bmp, HWindow window, bool ok)
        {
            try
            {
                HObject img_old;               
                HOperatorSet.GenEmptyObj(out img_old);
                img_old.Dispose();
                Bitmap2HObject.Bitmap2HObj(bmp, out img_old);
                HTuple width, height;
                HOperatorSet.GetImageSize(img_old, out width, out height);
                HOperatorSet.SetPart(window, 0, 0, height - 1, width - 1);
                HOperatorSet.DispObj(img_old, window);
                HOperatorSet.SetLineWidth(window, 3);
                HOperatorSet.SetColor(window, "red");
                if (ok)
                {
                    HOperatorSet.DispText(window, "圆心X方向偏差：" + System.Math.Round(Position.Instance.PCB2CCDOffset.X / 96, 4) + " mm",
                     "window", 30, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(window, "圆心Y方向偏差：" + System.Math.Round(Position.Instance.PCB2CCDOffset.Y / 96, 4) + " mm",
                        "window", 60, 7, "black", new HTuple(), new HTuple());
                    double[] CenterLocation = new double[2];
                    CenterLocation[0] = (Position.Instance.PCB2CCDOffset.X / 96) + (width / 2);
                    CenterLocation[1] = (Position.Instance.PCB2CCDOffset.Y / 96) + (height / 2);
                    //中心
                    HOperatorSet.SetColor(window, "green");
                    HObject ho_Cross = null;
                    HOperatorSet.GenEmptyObj(out ho_Cross);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, CenterLocation[1], CenterLocation[0], 180, 0.785398);
                    HOperatorSet.DispObj(ho_Cross, window);
                    Marking.CenterLocateTestSucceed = true;
                    SetString(window, "OK", "green", 100);
                }
                else
                {
                    SetString(window, "NG", "red", 100);
                    Marking.CenterLocateTestSucceed = false;
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

        public static void TestBmp(Bitmap bmp, HWindow hWindow, bool save)
        {
            try
            {
                LastCenterLocateBMP = bmp;
                double[] Res = CenterMatch(bmp, hWindow);
                if (Res[0] == -100 || Res[1] == -100)
                {
                    Marking.CenterLocateTestSucceed = false;
                }
                else
                {
                    Marking.CenterLocateTestSucceed = true;
                    Position.Instance.PCB2CCDOffset.X = Res[0];
                    Position.Instance.PCB2CCDOffset.Y = Res[1];
                }
                if (save)
                {
                    SaveImage.Save(hWindow);
                }
            }
            catch
            {
                Marking.CenterLocateTestSucceed = false;
            }
        }
    }
}
