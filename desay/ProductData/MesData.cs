using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desay.ProductData
{
    public class MesData
    {
        //public static MesData Instance = new MesData();
        public static object CarrierDataLock = new object();
        public struct CarrierData
        {
            public string SN;

            public string FN;

            public string StartTime;
        }
        public static object CleanDataLock = new object();
        public struct CleanData
        {
            public CarrierData carrierData;

            public string WbData;

            public string WbResult;

            public string WbLResult;

            public string HaveLens;

            public bool CleanResult;
        }
        public static object GlueDataLock = new object();
        public struct GlueData
        {
            public CleanData cleanData;

            public string GlueParam;

            public string GluePosResult;

            public string GlueCheckResult;

            public bool glueResult;
        }
        public static object AADataLock = new object();
        public struct AAData
        {
            public bool aaResult;           // AA工位结果标志
            public bool lightCameraRst;     // 点亮结果
            public bool preAAPosRst;        // 预AA位置结果
            public bool searchPosRst;       // 搜索定位结果
            public bool ocAdjustRst;        // 中心调整结果
            public bool tiltAdjustRst;      // 倾斜调整结果
            public bool uvBeforeRst;        // UV前结果
            public bool uvAfterRst;         // UV后结果
            public bool haveLensRst;        // 有无料检测结果
            public bool whiteLightRst;      // 白板点亮结果
            public bool whiteBoardRst;      // 白板检测结果
            public bool gluePosRst;         // 点胶定位结果
            public bool glueCheckRst;       // 点胶检测结果
            public string ResultCode;       // AA结果代码
        }
        
        public static CarrierData carrierData = new CarrierData();
        public static CleanData cleanData = new CleanData();
        public static GlueData glueData = new GlueData();
        public static object MesDataLock = new object();
        public static Dictionary<string, GlueData> MesDataList = new Dictionary<string, GlueData>();
        public static Dictionary<string, AAData> ResultList = new Dictionary<string, AAData>();
        public static List<AAData> AADataList = new List<AAData>();
        public static string NeedShowFN = "";
        public static object AALock = new object();
        /// <summary>
        /// Mes数据词典
        /// </summary>
        //public static Dictionary<string, Data> MesDataDictionary = new Dictionary<string, Data>();
    }
}
