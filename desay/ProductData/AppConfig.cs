using System;
using System.IO;

namespace desay.ProductData
{
    public class AppConfig
    {
        public static string VisionName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Vision\\{Config.Instance.CurrentProductType}.xml");
            }
        }
        public static string ModelName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Vision\\ModelParam.xml");
            }
        }
        public static string VisionPicturePass
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VisionPicture\\Pass\\");
            }
        }
        public static string VisionPictureFail
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VisionPicture\\Fail\\");
            }
        }
        public static string ConfigFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Config.xml");
            }
        }
        public static string ConfigCameraName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Camera.xml");
            }
        }
        public static string ConfigIniCardfile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Param\\param.xml");
            }
        }
        public static string ConfigEniCardfile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Param\\param.eni");
            }
        }
        public static string ConfigTrayName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Tray.ini");
            }
        }
        public static string ConfigAxisName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\AxisParam.xml");
            }
        }
        public static string ConfigDelayName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\CylinderDelay.xml");
            }
        }
        public static string ConfigPositionName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data\\{Config.Instance.CurrentProductType}.xml");
            }
        }
        public static string LogFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            }
        }
        /// <summary>
        /// 生产信息文件路径
        /// </summary>
        public static string ProdutionInfoFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProdutionInfo.ini");
            }
        }
        /// <summary>
        /// MES配置参数文件路径
        /// </summary>
        public static string MESConfigFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MESConfig.ini");
            }
        }
        public static string MesShareFileFolderName
        {
            get
            {
                return "D:\\files\\";
            }
        }
        public static string MesDataPassFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MesData\\Pass\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
            }
        }
        public static string MesDataFailFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MesData\\Fail\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
            }
        }
        /// <summary>
        /// 本地数据库文件夹路径
        /// </summary>
        public static string dataBaseDirectoryPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
            }
        }
        /// <summary>
        /// 本地数据库文件路径
        /// </summary>
        public static string dataBaseFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.db3", DateTime.Now.ToString("yyyyMMdd")));
            }
        }
        /// <summary>
        /// 视觉路径
        /// </summary>
        public static string VisonPath
        {
            get
            {
                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + $"VisionModel\\{Config.Instance.CurrentProductType}")) == false)
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + $"VisionModel\\{Config.Instance.CurrentProductType}"));
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + $"VisionModel\\{Config.Instance.CurrentProductType}");
            }
        }
    }
}
