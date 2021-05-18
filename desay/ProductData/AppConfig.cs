using System;
using System.IO;

namespace desay.ProductData
{
    public class AppConfig
    {
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

        public static string VisionConfig
        {
            get
            {
                return Path.Combine(VisonPath, "AAVisionConfig.ini");
            }
        }

        public static string VisionLocateROI
        {
            get
            {
                return Path.Combine(VisonPath, "CenterLocROI.hobj");
            }
        }

        public static string VisionLocateROI_Out
        {
            get
            {
                return Path.Combine(VisonPath, "CenterLocROI_out.hobj");
            }
        }

        public static string DryRunPic
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"VisionModel\\{Config.Instance.CurrentProductType}\\DryRunPic.jpg");
            }
        }

        public static string PosMode_Rect
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"VisionModel\\{Config.Instance.CurrentProductType}\\Mode.png");
            }
        }

        public static string WbIniPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ProjectConfig\\ProjectConfig.ini");
            }
        }
    }
}
