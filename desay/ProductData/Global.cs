using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motion.AdlinkAps;
using Motion.Interfaces;
using System.IO;
using System.Toolkit;

namespace desay.ProductData
{
    public class Global
    {
        public static bool CleanProductDone;
        public static bool IsLocating = false;
        public static bool isErrorExit = false;

        public static VelocityCurve LXmanualSpeed = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public static VelocityCurve LYmanualSpeed = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public static VelocityCurve LZmanualSpeed = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };

        public static VelocityCurve RXmanualSpeed = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public static VelocityCurve RYmanualSpeed = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public static VelocityCurve RZmanualSpeed = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };

        public static bool FactoryUser = false;
        public static bool WritePulse = true;
        //用户相关信息
        public static string userName, AdminPassword, OperatePassword;
        public static UserLevel userLevel = UserLevel.None;
        public static string SNdata = "123";

        public static int Office = 4000;


        public struct Fault
        {
            /// <summary>
            /// 故障代码
            /// </summary>
            public int FaultCode;
            /// <summary>
            /// 故障信息
            /// </summary>
            public string FaultMessage;
            /// <summary>
            /// 故障次数
            /// </summary>
            public int FaultCount;

        }

        /// <summary>
        /// 故障词典
        /// </summary>
        public static Dictionary<string, Fault> FaultDictionary = new Dictionary<string, Fault>();
        /// <summary>
        /// 所有故障记录
        /// </summary>
        public static Dictionary<string, Fault> FaultDictionary1 = new Dictionary<string, Fault>();

    }
}
