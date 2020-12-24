using desay.ProductData;
using HalconDotNet;
using System.Drawing;
using System.Toolkit;
using static desay.CenterLocate;
using desay.AAVision.Algorithm;

namespace desay
{
    class NeedleLocate
    {


        #region//ROI大小控制参数
        //圆心坐标
        private static HTuple hv_circle_center_x = new HTuple(972.0);
        private static HTuple hv_circle_center_y = new HTuple(1296.0);
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
        //内圆半径
        private static HTuple hv_circle_radius = new HTuple(1200.0);
        public static double circleRadius
        {
            get { return hv_circle_radius[0]; }
            set { hv_circle_radius[0] = value; }
        }
        private static HTuple areaMin = new HTuple(500000.0);
        public static double areaSizeMin
        {
            get { return areaMin; }
            set { areaMin = value; }
        }
        #endregion
        //图像处理主逻辑
        public static double[] action(Bitmap bmp, HWindow Window,
                                 Point3D<double> Needlepos, Point3D<double> CamPos)//图像处理代码
        {
            #region//声明控制变量
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_target_circle_center_X = null, hv_target_circle_center_Y = null;
            HTuple hv_Radius = null, hv_StartPhi = null, hv_EndPhi = null;
            HTuple hv_PointOrder = null;
            #endregion
            #region//声明需要用到的图像变量
            HObject ho_src_image;
            HObject ho_circle_mask;
            HObject ho_target_ROI;
            HObject ho_ImageGauss, ho_Regions, ho_SelectedRegions, ho_RegionOpening;
            HObject ho_RegionTrans, ho_Contours, ho_Cross;
            #endregion
            #region//初始化将要用到的图像变量
            HOperatorSet.GenEmptyObj(out ho_src_image);
            HOperatorSet.GenEmptyObj(out ho_circle_mask);
            HOperatorSet.GenEmptyObj(out ho_target_ROI);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            #endregion
            //将bitmap转换成HObject.
            Bitmap2HObject.Bitmap2HObj(bmp, out ho_src_image);
            HWindow hWindow = Window;
            //获取图片尺寸信息
            HOperatorSet.GetImageSize(ho_src_image, out hv_Width, out hv_Height);

            HOperatorSet.SetPart(hWindow, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_src_image, hWindow);
            //*粗略划分出ROI
            //圆区域控制参数
            ho_circle_mask.Dispose();
            HOperatorSet.GenCircle(out ho_circle_mask, hv_circle_center_x, hv_circle_center_y,
                                    hv_circle_radius);
            ho_target_ROI.Dispose();
            HOperatorSet.ReduceDomain(ho_src_image, ho_circle_mask, out ho_target_ROI);
            //去噪
            ho_ImageGauss.Dispose();
            HOperatorSet.GaussFilter(ho_target_ROI, out ho_ImageGauss, 3);
            //**阈值分割，采用区域生长法，tol=2, 区域阈值500000
            ho_Regions.Dispose();
            HOperatorSet.Regiongrowing(ho_ImageGauss, out ho_Regions, 4, 4, 1, areaMin);
            //选出圆度大于0.7的region
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_Regions, out ho_SelectedRegions, "circularity", "and", 0.8, 1);
            //对选出来的region进行开运算，去除“毛刺，杂点”
            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningCircle(ho_SelectedRegions, out ho_RegionOpening, 15);
            //将region形状转换成圆
            ho_RegionTrans.Dispose();
            HOperatorSet.ShapeTrans(ho_RegionOpening, out ho_RegionTrans, "outer_circle");
            //取得圆的XLD轮廓
            ho_Contours.Dispose();
            HOperatorSet.GenContourRegionXld(ho_RegionTrans, out ho_Contours, "border");

            //用圆拟合XLD轮廓，得到圆心坐标
            HOperatorSet.FitCircleContourXld(ho_Contours, "algebraic", -1, 0, 0, 3, 2, out hv_target_circle_center_Y,
                out hv_target_circle_center_X, out hv_Radius, out hv_StartPhi, out hv_EndPhi,
                out hv_PointOrder);

            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_target_circle_center_Y, hv_target_circle_center_X,
                180, 0.785398);
            HOperatorSet.SetLineWidth(Window, 2);
            HOperatorSet.SetColor(Window, "red");
            HOperatorSet.DispObj(ho_Cross, Window);
            HOperatorSet.DispObj(ho_Contours, Window);

            //**圆心坐标到图像中心的距离*像素分辨率
            double[] offset = new double[2];
            double offset_x = 1 * ((hv_target_circle_center_X[0] - (hv_Width[0] / 2)) * Config.Instance.CameraPixelMM_X);
            double offset_y = 1 * ((hv_target_circle_center_Y[0] - (hv_Height[0] / 2)) * Config.Instance.CameraPixelMM_Y);
            HOperatorSet.DispText(Window, "X方向偏差：" + System.Math.Round(offset_x, 4) + " mm",
                "window", 12, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "Y方向偏差：" + System.Math.Round(offset_y, 4) + " mm",
                "window", 30, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "圆心X轴坐标：" + System.Math.Round((double)(1 * hv_target_circle_center_X[0]),4) + " pix",
                "window", 48, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "圆心Y轴坐标：" + System.Math.Round((double)(1 * hv_target_circle_center_Y[0]),4) + " pix",
                "window", 66, 7, "black", new HTuple(), new HTuple());
            #region//释放图像变量
            ho_src_image.Dispose();
            ho_circle_mask.Dispose();
            ho_target_ROI.Dispose();
            ho_ImageGauss.Dispose();
            ho_Regions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionOpening.Dispose();
            ho_RegionTrans.Dispose();
            ho_Contours.Dispose();
            ho_Cross.Dispose();
            #endregion
            offset[0] = CamPos.X - Needlepos.X - offset_x;
            offset[1] = CamPos.Y - Needlepos.Y + offset_y;
            return offset;
        }

        public static void FindNeedleLoc(Bitmap bmp, HWindow Window,double[] data,double[] offset)//图像处理代码
        {
            #region//声明控制变量
            HTuple hv_Width = null, hv_Height = null;
            #endregion
            #region//声明需要用到的图像变量
            HObject ho_src_image;
            #endregion
            #region//初始化将要用到的图像变量
            HOperatorSet.GenEmptyObj(out ho_src_image);
            #endregion
            //将bitmap转换成HObject.
            Bitmap2HObject.Bitmap2HObj(bmp, out ho_src_image);
            HWindow hWindow = Window;
            //获取图片尺寸信息
            HOperatorSet.GetImageSize(ho_src_image, out hv_Width, out hv_Height);

            HOperatorSet.SetPart(hWindow, 0, 0, hv_Height - 1, hv_Width - 1);
            HOperatorSet.DispObj(ho_src_image, hWindow);
            SetString(Window, "OK", "green", 100);
            HOperatorSet.DispText(Window, "X方向偏差：" + System.Math.Round(offset[0], 4) + " mm",
                "window", 12, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "Y方向偏差：" + System.Math.Round(offset[1], 4) + " mm",
                "window", 30, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "圆心X轴坐标：" + System.Math.Round(data[0], 4) + " pix",
                "window", 48, 7, "black", new HTuple(), new HTuple());
            HOperatorSet.DispText(Window, "圆心Y轴坐标：" + System.Math.Round(data[1], 4) + " pix",
                "window", 66, 7, "black", new HTuple(), new HTuple());
            #region//释放图像变量
            ho_src_image.Dispose();
            #endregion
        }

        public static void Test(HWindow hWindow)
        {
            string picPath = "IMG0001.jpg"; //图片的绝对路径
            Point3D<double> Needlepos = Point3D<double>.Parse("1.0, 2.0, 3.0");
            Point3D<double> Campos = Point3D<double>.Parse("1.0, 2.0, 3.0");                                                                                   //string picPath = "D:\\MyPicture\\Head1.jpg"; //@字符让转译字符"\"保持原意，不要转译
            Bitmap bmp1 = new Bitmap(Image.FromFile(picPath));
            double[] Res = action(bmp1, hWindow, Needlepos, Campos);
            foreach (double r in Res)
            {
                System.Console.WriteLine(r.ToString());
            }
        }
        public static void TestBmp(Bitmap bmp, HWindow hWindow,
                                 Point3D<double> Needlepos, Point3D<double> CamPos,bool save)
        {
            double[] Res = action(bmp, hWindow, Needlepos, CamPos);

            Position.Instance.CCD2NeedleOffset.X = Res[0];
            Position.Instance.CCD2NeedleOffset.Y = Res[1];
            if (save)
            {
                SaveImage.Save(hWindow);
            }


        }
    }
}
