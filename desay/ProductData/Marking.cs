using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using desay.Flow;
using System.Diagnostics;
namespace desay.ProductData
{
    public class Marking
    {

        public static double CleanCycleTime;
        public static double GlueCycleTime;
        public static double TotalCycleTime;
        public static string CycleRunTime;

        public static Stopwatch CleanSinglewatch = new Stopwatch();
        public static Stopwatch GlueSinglewatch = new Stopwatch();
        public static Stopwatch Totalwatch = new Stopwatch();

        #region 工作信号 
        /// <summary>
        /// 请求出料
        /// </summary>
        public static bool CarrierCallOut;
        /// <summary>
        /// 出料完成
        /// </summary>
        public static bool CarrierCallOutFinish; 
        /// <summary>
        /// 请求入料
        /// </summary>
        public static bool CarrierCallIn;        
        /// <summary>
        /// 入料完成
        /// </summary>
        public static bool CarrierCallInFinish;  
        /// <summary>
        /// 接驳台工作中
        /// </summary>
        public static bool CarrierWorking; 
        /// <summary>
        /// 接驳台有料
        /// </summary>
        public static bool CarrierHaveProduct;
        /// <summary>
        /// 扫描开启
        /// </summary>
        public static bool ScannerEnable;
        /// <summary>
        /// 产品扫描枪屏蔽
        /// </summary>
        public static bool SnScannerShield = true;
        /// <summary>
        /// 接驳台屏蔽
        /// </summary>
        public static bool CarrierShield;

        /// <summary>
        /// 清洗工位工作状态信号
        /// </summary>
        public static bool CleanCallIn;        
        /// <summary>
        /// 清洗工位入料请求
        /// </summary>
        public static bool CleanCallInFinish;  
        /// <summary>
        /// 清洗工位工作中
        /// </summary>
        public static bool CleanWorking;        
        /// <summary>
        /// 清洗工位复位中
        /// </summary>
        public static bool CleanHoming;         
        /// <summary>
        ///  清洗工作结束
        /// </summary>
        public static bool CleanWorkFinish;    
        /// <summary>
        /// 清洗工位出料请求
        /// </summary>
        public static bool CleanCallOut;        
        /// <summary>
        /// 清洗工位出料完成
        /// </summary>
        public static bool CleanCallOutFinish;  
        /// <summary>
        /// 清洗工位有料标志
        /// </summary>
        public static bool CleanHaveProduct;   
        /// <summary>
        /// 镜头有无判断屏蔽
        /// </summary>
        public static bool HaveLensShield;
        /// <summary>
        /// Plasma功能屏蔽
        /// </summary>
        public static bool PlasmaShield;      
        /// <summary>
        /// 白板检测屏蔽
        /// </summary>
        public static bool WhiteShield;       
        /// <summary>
        /// 白板检测结果
        /// </summary>
        public static bool WhiteBoardResult; 
        /// <summary>
        /// 白板点亮结果
        /// </summary>
        public static bool WhiteLightResult;
        /// <summary>
        /// 白板再次点亮标志
        /// </summary>
        public static bool WbCheckAgainFlg;
       
        /// <summary>
        /// 清洗工位屏蔽
        /// </summary>
        public static bool CleanShield;       
        /// <summary>
        /// 清洗功能是否关闭
        /// </summary>
        public static bool CleanRun;         
        /// <summary>
        /// 清洗工位结果标志
        /// </summary>
        public static bool CleanResult;       
        /// <summary>
        /// 清洗周期运行
        /// </summary>
        public static bool CleanRecycleRun;    
        /// <summary>
        /// 清洗开始
        /// </summary>
        public static bool CleanStart;        
        /// <summary>
        /// 1次清洗完成
        /// </summary>
        public static bool OnceCleanFinish;    
        /// <summary>
        /// 2次清洗完成
        /// </summary>
        public static bool TwiceCleanFinish;  
        /// <summary>
        ///  清洗完成
        /// </summary>
        public static bool CleanFinish;     
        /// <summary>
        ///  白板检测结果OK
        /// </summary>
        public static bool CatchPitureResultOK;
        /// <summary>
        /// 白板检测结果NG
        /// </summary>
        public static bool CatchPitureResultNG;

        public static string WbData;


        /// <summary>
        /// 点胶工位入料请求
        /// </summary>
        public static bool GlueCallIn;        
        /// <summary>
        /// 点胶工位入料完成
        /// </summary>
        public static bool GlueCallInFinish;   
        /// <summary>
        /// 点胶工位工作中
        /// </summary>
        public static bool GlueWorking;       
        /// <summary>
        /// 点胶工位复位中
        /// </summary>
        public static bool GlueHoming;        
        /// <summary>
        /// 点胶工作结束
        /// </summary>
        public static bool GlueWorkFinish;    
        /// <summary>
        /// 点胶工位出料请求
        /// </summary>
        public static bool GlueCallOut;       
        /// <summary>
        /// 点胶工位出料完成
        /// </summary>
        public static bool GlueCallOutFinish; 
        /// <summary>
        /// 点胶工位有料标志
        /// </summary>
        public static bool GlueHaveProduct;    
        /// <summary>
        /// 点胶工位屏蔽
        /// </summary>
        public static bool GlueShield;         
        /// <summary>
        /// CCD功能屏蔽
        /// </summary>
        public static bool CCDShield;       
        /// <summary>
        /// 点胶功能关闭
        /// </summary>
        public static bool GlueRun;           
        /// <summary>
        /// 点胶完成
        /// </summary>
        public static bool GlueFinish;
        /// <summary>
        /// 点胶识别结果标志
        /// </summary>
        public static bool GluePosResult;
        /// <summary>
        /// 点胶识别结果标志
        /// </summary>
        public static bool GlueCheckResult;       
        /// <summary>
        /// 点胶周期运行
        /// </summary>
        public static bool GlueRecycleRun;   

        /// <summary>
        /// AA屏蔽
        /// </summary>
        public static bool AAShield;
        /// <summary>
        /// AA工位结果标志
        /// </summary>
        public static bool AAResult;          
        /// <summary>
        /// AA工位入料请求
        /// </summary>
        public static bool AACallIn;         
        /// <summary>
        /// 有无料检测结果
        /// </summary>
        public static bool HaveLensRst;       
        /// <summary>
        /// 白板检测结果
        /// </summary>
        public static bool WhiteBoardRst;
        /// <summary>
        /// 白板点亮结果
        /// </summary>
        public static bool WhiteLightRst;
        /// <summary>
        /// 点胶定位结果
        /// </summary>
        public static bool GluePosRst;
        /// <summary>
        /// 点胶检测结果
        /// </summary>
        public static bool GlueCheckRst;      
        /// <summary>
        /// 点亮结果
        /// </summary>
        public static bool LightCameraRst;    
        /// <summary>
        /// 预AA位置结果
        /// </summary>
        public static bool PreAAPosRst;        
        /// <summary>
        /// 搜索定位结果
        /// </summary>
        public static bool SearchPosRst;       
        /// <summary>
        /// 中心调整结果
        /// </summary>
        public static bool OCAdjustRst;       
        /// <summary>
        /// 倾斜调整结果
        /// </summary>
        public static bool TiltAdjustRst;      
        /// <summary>
        /// UV前结果
        /// </summary>
        public static bool UVBeforeRst;       
        /// <summary>
        /// UV后结果
        /// </summary>
        public static bool UVAfterRst;         
        /// <summary>
        ///  UV后结果报警
        /// </summary>
        public static bool UVAfterAlarm;       
        /// <summary>
        /// AA数据
        /// </summary>
        public static string AAData;
        #endregion

        #region 产品相关
        /// <summary>
        /// 产品有无检测标志
        /// </summary>
        public static bool HaveLens;
        #endregion

        #region 通讯相关
        /// <summary>
        /// 请求白板结果
        /// </summary>
        public static bool WbRequestResultFlg;     
        /// <summary>
        /// 白板软件打开
        /// </summary>
        public static bool WbClientOpenFlg;         
        /// <summary>
        /// 白板软件关闭
        /// </summary>
        public static bool WbClientCloseFlg;      
        /// <summary>
        /// 获取到白板结果
        /// </summary>
        public static bool WbGetResultFlg;         
      //public static bool WbCheckAgainFlg;        // 白板首次未点亮标志位

        /// <summary>
        /// 开始触发扫SN
        /// </summary>
        public static bool BeginTriggerSN;         
        /// <summary>
        /// 获取到SN
        /// </summary>
        public static bool GetSNFlg;              
        /// <summary>
        /// 开始触发扫FN
        /// </summary>
        public static bool BeginTriggerFN;         
        /// <summary>
        /// 获取到FN
        /// </summary>
        public static bool GetFNFlg;               
        /// <summary>
        /// 请求CCD结果
        /// </summary>
        public static bool CcdRequestResultFlg;    
        /// <summary>
        /// 请求CCD定位坐标
        /// </summary>
        public static bool CcdRequestLocationFlg;  
        /// <summary>
        /// 获取到CCD定位坐标
        /// </summary>
        public static bool CcdGetLocationFlg;     
        /// <summary>
        /// 获取到CCD结果
        /// </summary>
        public static bool CcdGetResultFlg;         
        /// <summary>
        /// 获取CCD定位坐标失败标志位
        /// </summary>
        public static bool CcdGetLocationFailFlg;   
        /// <summary>
        /// 获取CCD结果失败标志位
        /// </summary>
        public static bool CcdGetResultFailFlg;     
        //public static bool CcdClientOpenFlg;       // CCD软件打开
        //public static bool CcdClientCloseFlg;      // CCD软件关闭


        /// <summary>
        /// 获取到AA数据
        /// </summary>
        public static bool AaGetDataFlg;          
        /// <summary>
        /// 发送码和结果给AA
        /// </summary>
        public static bool AaSendCodeFlg;         
        /// <summary>
        /// AA软件打开
        /// </summary>
        public static bool AaClientOpenFlg;        
        /// <summary>
        /// AA软件关闭
        /// </summary>
        public static bool AaClientCloseFlg;       
        /// <summary>
        /// 获取到AA结果
        /// </summary>
        public static bool AaGetResultFlg;        
        /// <summary>
        /// 通知AA放行
        /// </summary>
        public static bool AaAllowPassFlg;          
        /// <summary>
        /// 请求测高数据
        /// </summary>
        public static bool RequestHeightFlg;       
        /// <summary>
        /// 获取到测得的高度
        /// </summary>
        public static bool GetHeightFlg;           

        /// <summary>
        /// 启动CCD对针识别
        /// </summary>
        public static bool NeedleLocateTest = false;   
        /// <summary>
        /// 启动CCD点胶圆中心坐标识别
        /// </summary>
        public static bool CenterLocateTest = false;    
        /// <summary>
        /// 启动CCD胶水识别
        /// </summary>
        public static bool GlueCheckTest = false;      
        /// <summary>
        /// 启动CCD对针识别成功
        /// </summary>
        public static bool NeedleLocateTestSucceed = false;    
        /// <summary>
        /// 启动CCD点胶圆中心坐标识别成功
        /// </summary>
        public static bool CenterLocateTestSucceed = false;    
        /// <summary>
        /// 启动CCD胶水识别成功
        /// </summary>
        public static bool GlueCheckTestSucceed = false;       
        /// <summary>
        /// 启动CCD对针识别结束
        /// </summary>
        public static bool NeedleLocateTestFinish = false;   
        /// <summary>
        /// 启动CCD点胶圆中心坐标识别结束
        /// </summary>
        public static bool CenterLocateTestFinish = false;    
        /// <summary>
        /// 启动CCD胶水识别结束
        /// </summary>
        public static bool GlueCheckTestFinish = false;       
        /// <summary>
        /// 请求测高报错
        /// </summary>
        public static bool RequestHeightError;





        #endregion

        /// <summary>
        /// 安全门屏蔽
        /// </summary>
        public static bool DoorShield;
        /// <summary>
        /// 安全光幕屏蔽
        /// </summary>
        public static bool CurtainShield;
        /// <summary>
        /// 进料光纤感应失败计数
        /// </summary>
        public static int CarrierFailCount;
        [NonSerialized]
        public static double AutoDoorCloseDelay = 10.0;
        /// <summary>
        /// 蜂鸣器关闭
        /// </summary>
        public static bool VoiceClosed;
        /// <summary>
        /// TCP服务器打开标志位
        /// </summary>
        public static bool TcpServerOpenSuccess;
        /// <summary>
        /// 测高串口打开标志位
        /// </summary>
        public static bool HeightDetectorOpenSuccess;
        /// <summary>
        /// SN扫码枪串口打开标志位
        /// </summary>
        public static bool snScannerOpenSuccess;
        /// <summary>
        /// FN扫码枪串口打开标志位
        /// </summary>
        public static bool fnScannerOpenSuccess;
        /// <summary>
        /// 接驳台有无料屏蔽
        /// </summary>
        public static bool CarrierHaveProductSheild;
        /// <summary>
        /// 清洗有无料屏蔽
        /// </summary>
        public static bool CleanHaveProductSheild;
        /// <summary>
        /// 点胶有无料屏蔽
        /// </summary>
        public static bool GlueHaveProductSheild;
        /// <summary>
        /// 清洗结束状态
        /// </summary>
        public static bool CleanFinishBit;
        /// <summary>
        /// 点胶结束状态
        /// </summary>
        public static bool GlueFinishBit;
        /// <summary>
        /// 产品码
        /// </summary>
        public static string SN;
        /// <summary>
        /// 治具码
        /// </summary>
        public static string FN;
        /// <summary>
        /// 点胶高度补偿
        /// </summary>
        public static double GlueHeightOffset;
        /// <summary>
        /// 测到的高度
        /// </summary>
        public static double DetectHeight = 0.0;

        public static double GlueHeight_value = 0.0;
        /// <summary>
        /// 新增型号或者 切换型号时 重新加载；
        /// </summary>
        public static bool IsProductThrans = false;
        /// <summary>
        /// 屏蔽自动排胶功能
        /// </summary>
        public static bool LeaveShield = false;
        /// <summary>
        /// 空跑模式
        /// </summary>
        public static bool DryRun = false;
        /// <summary>
        /// 白板点检模式
        /// </summary>
        public static bool WhiteMode = false;
        /// <summary>
        /// 点胶点检模式
        /// </summary>
        public static bool GlueMode = false;
        /// <summary>
        /// AA点检模式
        /// </summary>
        public static bool AAMode = false;
        /// <summary>
        /// 称重点检模式
        /// </summary>
        public static bool WeighMode = false;
    }
}
