using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Toolkit;
namespace desay.ProductData
{
    [Serializable]
    public class Position
    {
        public static Position Instance = new Position();

        #region 参数设置
        /// <summary>
        /// 清洗Z轴是否启用缓冲
        /// </summary>
        public bool IsCleanZBuffer;
        /// <summary>
        /// 清洗Z轴缓冲速度
        /// </summary>
        public double CleanZBufferSpeed;
        /// <summary>
        /// 清洗Z轴缓冲距离
        /// </summary>
        public double CleanZBufferDistance;
        /// <summary>
        /// 点胶Z轴是否启用缓冲
        /// </summary>
        public bool IsGlueZBuffer;
        /// <summary>
        /// 点胶Z轴缓冲速度
        /// </summary>
        public double GlueZBufferSpeed;
        /// <summary>
        /// 点胶Z轴缓冲距离
        /// </summary>
        public double GlueZBufferDistance;
        /// <summary>
        /// 开始清洗延时
        /// </summary>
        public double StartCleanDelay;
        /// <summary>
        /// 结束清洗延时
        /// </summary>
        public double StopCleanDelay;
        /// <summary>
        /// 开始点胶延时,即聚胶时间
        /// </summary>
        public double StartGlueDelay;
        /// <summary>
        /// 结束点胶延时,即断胶时间
        /// </summary>
        public double StopGlueDelay;
        /// <summary>
        /// 清洗轨迹速度
        /// </summary>
        public double CleanPathSpeed;
        /// <summary>
        /// 点胶轨迹速度
        /// </summary>
        public double GluePathSpeed;
        /// <summary>
        /// 镜筒清洗次数
        /// </summary>
        public int ConeCleanNum;
        /// <summary>
        /// 镜片清洗次数
        /// </summary>
        public int LensCleanNum;
        /// <summary>
        /// UV后NG报警时间
        /// </summary>
        public int UVAfterAlarmTime = 2000;
        /// <summary>
        /// 相机拍照延时
        /// </summary>
        public int TriggerCameraDelay = 500;

        /// <summary>
        /// 测高偏差上限
        /// </summary>
        public double DetectHeightOffsetUp=10;


        /// <summary>
        /// 测高偏差下限
        /// </summary>
        public double DetectHeightOffsetDown = 10;

        /// <summary>
        /// 启动空胶角度
        /// </summary>
        public int StartGlueAngle;
        /// <summary>
        /// 拖胶角度
        /// </summary>
        public int DragGlueAngle;
        /// <summary>
        /// 拖胶高度
        /// </summary>
        public double DragGlueHeight;
        /// <summary>
        /// 拖胶延时
        /// </summary>
        public double DragGlueDelay;
        /// <summary>
        /// 拖胶速度
        /// </summary>
        public double DragGlueSpeed;
        /// <summary>
        /// 二次点胶角度
        /// </summary>
        public int SecondGlueAngle;
        /// <summary>
        /// 点胶定位偏差X
        /// </summary>
        public double GlueOffsetX;
        /// <summary>
        /// 点胶定位偏差Y
        /// </summary>
        public double GlueOffsetY;
        /// <summary>
        /// 点胶高度
        /// </summary>
        public double GlueHeight;
        /// <summary>
        /// 点胶半径
        /// </summary>
        public double GlueRadius;
        /// <summary>
        /// 点胶基准高度
        /// </summary>
        public double GlueBaseHeight;
        /// <summary>
        /// 测高值-点胶基准高度
        /// </summary>
        public double DetectHeight2BaseHeight;
        /// <summary>
        /// 白板电源-电流
        /// </summary>
        public double[] Current = new double[3];
        /// <summary>
        /// 白板电源-电压
        /// </summary>
        public double[] Voltage = new double[3];
        #endregion

        #region plasma清洗平台位置数据
        /// <summary>
        /// 清洗安全位置
        /// </summary>
        public Point3D<double> CleanSafePosition;
        /// <summary>
        /// 有无料判断位置
        /// </summary>
        public Point3D<double> LensDetectPosition;
        /// <summary>
        /// 白板测试位置
        /// </summary>
        public Point3D<double> AdjustLightPosition;
        /// <summary>
        /// 清洗镜筒轨迹第一点位置
        /// </summary>
        public Point3D<double> CleanConeFirstPosition;
        /// <summary>
        /// 清洗镜筒轨迹第二点位置
        /// </summary>
        public Point3D<double> CleanConeSecondPosition;
        /// <summary>
        /// 清洗镜筒轨迹第三点位置
        /// </summary>
        public Point3D<double> CleanConeThirdPositon;
        /// <summary>
        /// 清洗镜筒轨迹第四点位置
        /// </summary>
        public Point3D<double> CleanConeForthPosition;
        /// <summary>
        /// 清洗镜筒轨迹第五点位置
        /// </summary>
        public Point3D<double> CleanConeFifthPosition;
        /// <summary>
        /// 清洗镜筒轨迹第一点机械位置
        /// </summary>
        public Point<float> CleanConeFirstPositionReal;
        /// <summary>
        /// 清洗镜筒轨迹第二点机械位置
        /// </summary>
        public Point<float> CleanConeSecondPositionReal;
        /// <summary>
        /// 清洗镜筒轨迹第三点机械位置
        /// </summary>
        public Point<float> CleanConeThirdPositonReal;
        /// <summary>
        /// 清洗镜筒轨迹第四点机械位置
        /// </summary>
        public Point<float> CleanConeForthPositionReal;
        /// <summary>
        /// 清洗镜筒轨迹第五点机械位置
        /// </summary>
        public Point<float> CleanConeFifthPositionReal;
        /// <summary>
        /// 清洗镜筒轨迹圆心机械位置
        /// </summary>
        public Point<float> CleanConeCenterPositionReal;
        /// <summary>
        /// 清洗镜片轨迹第一点位置
        /// </summary>
        public Point3D<double> CleanLensFirstPosition;
        /// <summary>
        /// 清洗镜片轨迹第二点位置
        /// </summary>
        public Point3D<double> CleanLensSecondPosition;
        /// <summary>
        /// 清洗镜片轨迹第三点位置
        /// </summary>
        public Point3D<double> CleanLensThirdPositon;
        /// <summary>
        /// 清洗镜片轨迹第四点位置
        /// </summary>
        public Point3D<double> CleanLensForthPosition;
        /// <summary>
        /// 清洗镜片轨迹第五点位置
        /// </summary>
        public Point3D<double> CleanLensFifthPosition;
        /// <summary>
        /// 清洗镜片轨迹第一点机械位置
        /// </summary>
        public Point<float> CleanLensFirstPositionReal;
        /// <summary>
        /// 清洗镜片轨迹第二点机械位置
        /// </summary>
        public Point<float> CleanLensSecondPositionReal;
        /// <summary>
        /// 清洗镜片轨迹第三点机械位置
        /// </summary>
        public Point<float> CleanLensThirdPositonReal;
        /// <summary>
        /// 清洗镜片轨迹第四点机械位置
        /// </summary>
        public Point<float> CleanLensForthPositionReal;
        /// <summary>
        /// 清洗镜片轨迹第五点机械位置
        /// </summary>
        public Point<float> CleanLensFifthPositionReal;
        /// <summary>
        /// 清洗镜片轨迹圆心机械位置
        /// </summary>
        public Point<float> CleanLensCenterPositionReal;
        #endregion

        #region 点胶平台位置数据
        /// <summary>
        /// 点胶安全位置
        /// </summary>
        public Point3D<double> GlueSafePosition;
        /// <summary>
        /// 点胶相机标定位置
        /// </summary>
        public Point3D<double> GlueCameraCalibPosition;
        /// <summary>
        /// 点胶相机拍照位置
        /// </summary>
        public Point3D<double> GlueCameraPosition;
        /// <summary>
        /// 点胶中心位置
        /// </summary>
        public Point3D<double> GlueCenterPosition;
        /// <summary>
        /// 点胶开始位置
        /// </summary>
        public Point3D<double> GlueStartPosition;
        /// <summary>
        /// 点胶轨迹第二点位置
        /// </summary>
        public Point3D<double> GlueSecondPosition;
        /// <summary>
        /// 点胶轨迹第三点位置
        /// </summary>
        public Point3D<double> GlueThirdPositon;
        /// <summary>
        /// 点胶轨迹第一点机械位置
        /// </summary>
        public Point<float> GlueFirstPositionReal;
        /// <summary>
        /// 点胶轨迹第二点机械位置
        /// </summary>
        public Point<float> GlueSecondPositionReal;
        /// <summary>
        /// 点胶轨迹第三点机械位置
        /// </summary>
        public Point<float> GlueThirdPositonReal;
        /// <summary>
        /// 点胶轨迹圆心机械位置
        /// </summary>
        public Point<float> GlueCenterPositionReal;
        /// <summary>
        /// 点胶对针位置
        /// </summary>
        public Point3D<double> GlueAdjustPinPosition;
        /// <summary>
        /// 胶重点检位置
        /// </summary>
        public Point3D<double> WeightGluePosition;
        /// <summary>
        /// 测胶高位置
        /// </summary>
        public Point3D<double> GlueHeightPosition;
        /// <summary>
        /// 自动对针位置1
        /// </summary>
        public Point3D<double> CutGlueStartPosition;
        /// <summary>
        /// 自动对针位置2
        /// </summary>
        public Point3D<double> CutGlueEndPosition;
        #endregion
        /// <summary>
        /// 胶针偏移
        /// </summary>
        public Point3D<double> NeedleOffset;


        /// <summary>
        /// 记录CCD到胶针的偏差值，Z轴暂不用
        /// </summary>
        public Point3D<double> CCD2NeedleOffset;
        /// <summary>
        /// 产品圆心到相机视野中心的偏差值，Z轴暂不用
        /// </summary>
        public Point3D<double> PCB2CCDOffset;



        public double MaxNeedleOffsetX=41.0;
        public double MaxNeedleOffsetY=-105.0;
        public double MinNeedleOffsetX=38.0;
        public double MinNeedleOffsetY=-108.0;
        /// <summary>
        /// 自动对针设定偏差
        /// </summary>
        public Point3D<double> NeedleCalibOffset;
        /// <summary>
        /// 自动对针中心位置
        /// </summary>
        public Point3D<double> NeedleCalibCenter;
        #region 通讯相关
        /// <summary>
        /// 网口通讯超时时间
        /// </summary>
        public int SocketTimeout = 60000;
        /// <summary>
        /// MES通讯超时时间
        /// </summary>
        public int MesTimeout = 500;
        /// <summary>
        /// 自动扫码枪通讯设置字符串
        /// </summary>
        public string AutoConnectionString;
        /// <summary>
        /// 手动扫码枪通讯设置字符串
        /// </summary>
        public string ManualConnectionString;
        /// <summary>
        /// 点胶测高通讯设置字符串
        /// </summary>
        public string HeightConnectionString;
        /// <summary>
        /// 白板光源点亮电源通讯设置字符串
        /// </summary>
        public string WhiteBoardPowerConnectionString= "COM4,38400,None,8,One,5000,5000";
        /// <summary>
        /// AA通讯IP地址
        /// </summary>
        public string AAIP = "192.168.1.11";
        /// <summary>
        /// AA通讯端口号
        /// </summary>
        public int AAPort = 10000;
        /// <summary>
        /// Tcp通讯服务器IP地址
        /// </summary>
        public string ServerIP = "192.168.2.11";
        /// <summary>
        /// CCD通讯端口号
        /// </summary>
        public int CCDPort = 20000;
        /// <summary>
        /// 白板通讯端口号
        /// </summary>
        public int WBPort = 30000;
        /// <summary>
        /// MES通讯地址
        /// </summary>
        public string MesAddr;
        /// <summary>
        /// MES通讯端口
        /// </summary>
        public int MesPort;
        /// <summary>
        /// MES通讯允许的失败次数
        /// </summary>
        public int MesFailCount;
        /// <summary>
        /// Mes保存数据文件头(CSV文件)
        /// </summary>
        public string MesCsvHeader = @"Test Time,SN,FN,Clean Result,Glue Result,WhiteBoard Test Result,Hotpixel Particl Number,
                                   Badpixel Particl Number,Particl Coordinate,AA Test Result,TiltX,TiltY,UVBefore_OC_X,UVBefore_OC_Y,
                                   UVBefore_MTF_C,UVBefore_MTF_TL,UVBefore_MTF_TR,UVBefore_MTF_BR,UVBefore_MTF_BL,UVAfter_OC_X,
                                   UVAfter_OC_Y,UVAfter_MTF_C,UVAfter_MTF_TL,UVAfter_MTF_TR,UVAfter_MTF_BR,UVAfter_MTF_BL";
        #endregion
        /// <summary>
        /// 图片保存的天数
        /// </summary>
        public int DayOfImageSave;
        /// <summary>
        /// 启用矩形点胶
        /// </summary>
        public bool UseRectGlue = false;


        /// <summary>
        /// 白板光源气缸 不动作0，动作1
        /// </summary>
        public int WbLightCylinder = 0;
        /// <summary>
        /// 清洗上下气缸 不动作0，动作1
        /// </summary>
        public int CleanUpDownCylinder = 0;
        /// <summary>
        /// 圆形胶水外边缘溢胶/少胶的距离
        /// </summary>
        public double OutsideDistance = 0;
        /// <summary>
        /// 圆形胶水内边缘溢胶/少胶的距离
        /// </summary>
        public double insideDistance = 0;
        /// <summary>
        /// 圆形红色阈值
        /// </summary>
        public int RedMax_Threshold = 60;
        /// <summary>
        /// 圆形绿色阈值
        /// </summary>
        public int GreenMax_Threshold = 50;
        /// <summary>
        /// 圆形蓝色阈值
        /// </summary>
        public int BlueMax_Threshold = 255;
        /// <summary>
        /// FFT滤波
        /// </summary>
        public int FFT_Frequency = 7;


        /// <summary>
        /// 矩形质心X轴偏移
        /// </summary>
        public double CenterOffset_X;
        /// <summary>
        /// 矩形质心Y轴偏移
        /// </summary>
        public double CenterOffset_Y;
        /// <summary>
        /// 矩形胶水面积上限
        /// </summary>
        public double MaxGlueArea = 0;
        /// <summary>
        /// 矩形胶水面积下限
        /// </summary>
        public double MinGlueArea = 0;
        /// <summary>
        /// 矩形胶路阈值Min
        /// </summary>
        public int ManualThreshold = 160;


    }
}
