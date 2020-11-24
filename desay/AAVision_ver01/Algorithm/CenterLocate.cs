using desay.AAVision.Algorithm;
using desay.ProductData;
using HalconDotNet;
using System.Drawing;
namespace desay
{
    static class CenterLocate
    {
        public static Bitmap LastCenterLocateBMP; //存储上一次圆心识别的图片，用于胶水识别中的模板匹配

        #region // 控制参数
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

        //分割阈值
        private static HTuple hv_threshold_gray_min = new HTuple(0.0);
        private static HTuple hv_threshold_gray_max = new HTuple(50.0);
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
        //选取的区域面积
        private static HTuple _areaMin = new HTuple(400000.0);
        public static double areaMin
        {
            get { return _areaMin[0]; }
            set { _areaMin[0] = value; }
        }
        private static HTuple _areaMax = new HTuple(1000000.0);
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
            HObject ho_src_image, ho_Circle_mask_S, ho_target_ROI;
            HObject ho_target_ring_scale, ho_Regions, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionFillUp, ho_RegionTrans;
            HObject ho_Contours, ho_Cross,Regionopening;
            #endregion
            // 声明控制变量 
            #region
            HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
            HTuple hv_Row = null, hv_Column = null;
            HTuple hv_Radius = null, hv_StartPhi = null, hv_EndPhi = null;
            HTuple hv_PointOrder = null, hv_target_center_Y = null;
            HTuple hv_target_center_X = null;
            #endregion
            //初始化图像变量
            #region
            HOperatorSet.GenEmptyObj(out ho_src_image);
            HOperatorSet.GenEmptyObj(out ho_Circle_mask_S);
            HOperatorSet.GenEmptyObj(out ho_target_ROI);
            HOperatorSet.GenEmptyObj(out ho_target_ring_scale);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out Regionopening);
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
            ho_target_ROI.Dispose();
            HOperatorSet.ReduceDomain(ho_src_image, ho_Circle_mask_S, out ho_target_ROI);


            //**精确定位点胶区域
            //增强对比度
            ho_target_ring_scale.Dispose();
            HOperatorSet.ScaleImageMax(ho_target_ROI, out ho_target_ring_scale);
            //阈值划分
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_target_ring_scale, out ho_Regions, hv_threshold_gray_min,
                hv_threshold_gray_max);
            Regionopening.Dispose();
            HOperatorSet.OpeningCircle(ho_Regions, out Regionopening, 25);
            //联通区域
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(Regionopening, out ho_ConnectedRegions);
            //选出面积最大的Region
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", _areaMin, _areaMax);
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
            hv_target_center_Y = hv_Row.Clone();
            hv_target_center_X = hv_Column.Clone();


            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_target_center_Y, hv_target_center_X,
                180, 0.785398);
            HOperatorSet.SetLineWidth(Window, 2);
            HOperatorSet.SetColor(Window, "red");
            HOperatorSet.DispObj(ho_Cross, Window);
            HOperatorSet.DispObj(ho_Contours, Window);

            //释放图像变量
            #region
            ho_src_image.Dispose();
            ho_Circle_mask_S.Dispose();
            ho_target_ROI.Dispose();
            ho_target_ring_scale.Dispose();
            ho_Regions.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionTrans.Dispose();
            ho_Contours.Dispose();
            ho_Cross.Dispose();
            #endregion
            double[] offset = new double[2];
            offset[0] = (hv_target_center_X - (hv_Width / 2)) * 0.0096;
            offset[1] = -1 * ((hv_target_center_Y - (hv_Height / 2)) * 0.0096);

            HOperatorSet.DispText(Window, "X方向偏差：" + System.Math.Round(offset[0], 4) + " mm",
                 "window", 12, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "Y方向偏差：" + System.Math.Round(offset[1], 4) + " mm",
                "window", 30, 7, "black", new HTuple(), new HTuple());

            return offset;
        }

        public static void TestBmp(Bitmap bmp, HWindow hWindow, bool save)
        {
            LastCenterLocateBMP = bmp;
            double[] Res = action(bmp, hWindow);

            Position.Instance.PCB2CCDOffset.X = Res[0];
            Position.Instance.PCB2CCDOffset.Y = Res[1];
            if (save)
            {
                SaveImage.Save(hWindow);
            }
        }
    }
}
