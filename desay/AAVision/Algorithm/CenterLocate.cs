using desay.AAVision.Algorithm;
using desay.ProductData;
using HalconDotNet;
using System.Drawing;
namespace desay
{
    static class CenterLocate
    {
        //public static string ROIFileName = @"./Resources/CenterLocROI.hobj";
        //public static string ModelFileName = @"./Resources/CenterModle.shm";
        public static double[] CenterLoc = new double[2];
        public static Bitmap LastCenterLocateBMP; //存储上一次圆心识别的图片，用于胶水识别中的模板匹配

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

        public static double[] action(Bitmap bmp, HWindow Window)
        {
            // 声明图像变量
            #region
            HObject ho_src_image, ho_Circle_mask_S, ho_Circle_mask_L;
            HObject ho_ImageReduced, ho_target_ring_mask, ho_target_ring;
            HObject ho_target_ring_scale, ho_Regions, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionErosion, ho_Rectangle;
            HObject ho_Contours, ho_Cross;
            #endregion
            // 声明控制变量 
            #region
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_Row = null, hv_Column = null;
            HTuple hv_Row2 = null, hv_Column2 = null;
            HTuple hv_target_center_X = null, hv_target_center_Y = null;
            #endregion
            //初始化图像变量
            #region
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_src_image);
            HOperatorSet.GenEmptyObj(out ho_Circle_mask_S);
            HOperatorSet.GenEmptyObj(out ho_Circle_mask_L);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_target_ring_mask);
            HOperatorSet.GenEmptyObj(out ho_target_ring);
            HOperatorSet.GenEmptyObj(out ho_target_ring_scale);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            #endregion
            //**2020-6-18
            //识别点胶的环形区域
            //读取图片
            ho_src_image.Dispose();
            Bitmap2HObject.Bitmap2HObj(bmp, out ho_src_image);
            //获取图片尺寸
            HOperatorSet.GetImageSize(ho_src_image, out hv_Width, out hv_Height);
            //显示图片
            HOperatorSet.SetPart(Window, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_src_image, Window);


            //**将点胶区域target_ring粗略划分出来
            //创建圆并取得圆环状Region
            ho_Circle_mask_S.Dispose();
            HOperatorSet.GenCircle(out ho_Circle_mask_S, hv_circle_center_x, hv_circle_center_y,
                hv_circle_radius);
            ho_Circle_mask_L.Dispose();
            HOperatorSet.GenCircle(out ho_Circle_mask_L, hv_circle_center_x, hv_circle_center_y,
                hv_circle_radius + hv_offset);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_src_image, ho_Circle_mask_S, out ho_ImageReduced
                );
            ho_target_ring_mask.Dispose();
            HOperatorSet.Difference(ho_ImageReduced, ho_Circle_mask_L, out ho_target_ring_mask
                );
            ho_target_ring.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReduced, ho_target_ring_mask, out ho_target_ring
                );
            ho_Circle_mask_S.Dispose();
            ho_ImageReduced.Dispose();
            ho_target_ring_mask.Dispose();
            ho_Circle_mask_L.Dispose();
            //**精确定位点胶区域
            //增强对比度
            ho_target_ring_scale.Dispose();
            HOperatorSet.ScaleImageMax(ho_target_ring, out ho_target_ring_scale);
            //阈值划分
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_target_ring_scale, out ho_Regions, hv_threshold_gray_min,
                hv_threshold_gray_max);
            //腐蚀，去除“毛边”
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_Regions, out ho_RegionErosion, ero);
            //联通区域
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionErosion, out ho_ConnectedRegions);

            //选出面积最大的Region
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", _areaMin, _areaMax);

            //求region的最小外接矩形
            HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row, out hv_Column,
                 out hv_Row2, out hv_Column2);
            //创建矩形region并取得XLD轮廓，用于显示
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row, hv_Column, hv_Row2, hv_Column2);
            ho_Contours.Dispose();
            HOperatorSet.GenContourRegionXld(ho_Rectangle, out ho_Contours, "border");

            hv_target_center_X = hv_Column + ((hv_Column2 - hv_Column) / 2);
            hv_target_center_Y = hv_Row + ((hv_Row2 - hv_Row) / 2);

            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_target_center_Y, hv_target_center_X,
                180, 0.785398);
            HOperatorSet.SetLineWidth(Window, 2);
            HOperatorSet.SetColor(Window, "green");
            HOperatorSet.DispObj(ho_Cross, Window);
            HOperatorSet.DispObj(ho_Contours, Window);



            //释放图像变量
            #region
            ho_src_image.Dispose();




            ho_target_ring.Dispose();
            ho_target_ring_scale.Dispose();
            ho_Regions.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionErosion.Dispose();
            ho_Rectangle.Dispose();
            ho_Contours.Dispose();
            ho_Cross.Dispose();
            #endregion
            double[] offset = new double[2];
            offset[0] = ((hv_target_center_X[0] - ( hv_Width[0] / 2)) * 0.0096);
            offset[1] = -1 * ((hv_target_center_Y[0] - (hv_Height[0] / 2)) * 0.0096);

            HOperatorSet.DispText(Window, "X方向偏差：" + System.Math.Round(offset[0], 4) + " mm",
                 "window", 12, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "Y方向偏差：" + System.Math.Round(offset[1], 4) + " mm",
                "window", 30, 7, "black", new HTuple(), new HTuple());

            return offset;
        }
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
        /// <param name="window"></param>
        /// <param name="image"></param>
        /// <param name="ROIFileName"></param>
        /// <param name="ModelFileName"></param>
        /// <returns></returns>
        public static double[] CenterMatch(Bitmap bmp,  HWindow window)
        {
            try
            {
                HObject image ,img_old;
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
                double[] data = TempalteMatch.action(window, image, AppConfig.VisonPath + "\\CenterLocROI.hobj", AppConfig.VisonPath + "\\CenterModle.shm");
                if (data[0] == -100 || data[1] == -100)
                {
                    HOperatorSet.DispText(window, "识别失败",
                             "window", 12, 7, "black", new HTuple(), new HTuple());
                    try
                    {

                        SetString(window, "NG", "red", 100);
                    }
                    catch { }
                    return data;
                }
                else
                {
                    CenterLoc[0] = data[0];
                    CenterLoc[1] = data[1];
                    HObject ho_Cross = null;

                    HOperatorSet.DispText(window, "X像素：" + System.Math.Round(data[0], 4) + "  Y像素："+ System.Math.Round(data[1], 4) + " pixel",
                        "window", 10, 7, "black", new HTuple(), new HTuple());

                    HOperatorSet.GenEmptyObj(out ho_Cross);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, data[1], data[0],
                        180, 0.785398);

                    //data[0] = (data[0] - width / 2) * 0.0096;
                    //data[1] = -1 * ((data[1] - height / 2) * 0.0096);
                    
                    data[0] = (data[0] - width / 2) * Config.Instance.CameraPixelMM_X;
                    data[1] = 1 * ((data[1] - height / 2) * Config.Instance.CameraPixelMM_Y) ;

                    HOperatorSet.DispObj(ho_Cross, window);

                    HOperatorSet.DispText(window, "X方向偏差：" + System.Math.Round(data[0], 4) + " mm",
                         "window", 30, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(window, "Y方向偏差：" + System.Math.Round(data[1], 4) + " mm",
                        "window", 50, 7, "black", new HTuple(), new HTuple());
                    Marking.CenterLocateTestSucceed = true;
                    SetString(window, "OK", "green", 100);
                    return data;
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
                return null;
            } 
        }

        public static void RectangleMatch(Bitmap bmp, HWindow window,bool ok)
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
                if (ok)
                {
                    //LastCenterLocateBMP = bmp;

                    HOperatorSet.DispText(window, "圆心X方向偏差：" + System.Math.Round(Position.Instance.PCB2CCDOffset.X / 96, 4) + " mm",
                     "window", 30, 7, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(window, "圆心Y方向偏差：" + System.Math.Round(Position.Instance.PCB2CCDOffset.Y / 96, 4) + " mm",
                        "window", 60, 7, "black", new HTuple(), new HTuple());
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
        public static void TestBmp(Bitmap bmp, HWindow hWindow, bool save)
        {
            try
            {

                LastCenterLocateBMP = bmp;
                //double[] Res = action(bmp, hWindow);
                CenterMatch(bmp, hWindow);
                //Position.Instance.PCB2CCDOffset.X = Res[0];
                //Position.Instance.PCB2CCDOffset.Y = Res[1];
                if (save)
                {
                    SaveImage.Save(hWindow);
                }
            }
            catch { }
        }
    }
}
