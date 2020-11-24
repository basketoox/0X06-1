using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Toolkit;
using System.Toolkit.Helpers;
namespace desay.ProductData
{
    [Serializable]
    public class Config
    {
        [NonSerialized]
        public static Config Instance = new Config();
        //用户相关信息
        public string userName, AdminPassword = SecurityHelper.TextToMd5("321"), OperatePassword = SecurityHelper.TextToMd5("123");
        public UserLevel userLevel = UserLevel.None;

        public string SerialParam = "COM2,115200,0,8,1,1500,1500";
        /// <summary>
        /// 当前产品型号
        /// </summary>
        public string CurrentProductType  = "defualt";

        /// <summary>
        /// 相机解析度X  
        /// </summary>
        public double CameraPixelMM_X = 0.02102165;
        /// <summary>
        /// 相机解析度Y  
        /// </summary>
        public double CameraPixelMM_Y = 0.020949;

        #region 产品计数
        /// <summary>
        /// 清洗产品OK总数
        /// </summary>
        public int CleanProductOkTotal;
        /// <summary>
        /// 清洗产品NG总数
        /// </summary>
        public int CleanProductNgTotal;
        /// <summary>
        /// 点胶产品OK总数
        /// </summary>
        public int GlueProductOkTotal;
        /// <summary>
        /// 点胶产品NG总数
        /// </summary>
        public int GlueProductNgTotal;
        /// <summary>
        /// AA产品OK总数
        /// </summary>
        public int AAProductOkTotal;
        /// <summary>
        /// AA产品NG总数
        /// </summary>
        public int AAProductNgTotal;
        /// <summary>
        /// 生产总数
        /// </summary>
        public int ProductTotal { get { return AAProductOkTotal + AAProductNgTotal; } }
                                     
        public string GlueSwPath = "D:\\做料软件\\胶水检测";
        #endregion

        /// <summary>
        /// 0X 06  为7  老机台为6
        /// </summary>
        public int MesWorkNum = 7;

        public double[] RectX = new double[10];

        public double[] RectY = new double[10];
        public double RectZ=0.0;
        public int GlueRectNOoneDelayTime = 100;
    }
}
