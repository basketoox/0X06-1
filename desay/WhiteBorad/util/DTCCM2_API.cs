using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DTCCM2_DLL
{
    public struct FrameInfo
    {
        public byte byChannel;
        public ushort uWidth;
        public ushort uHeight;
        public uint uDataSize;
        public ulong uiTimeStamp;
    };

    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi,Pack =1)]
    public struct FrameInfoEx
    {
        public byte byChannel;     ///< 图像通道标识，只有UH920/DTLC2支持
        [MarshalAs(UnmanagedType.ByValArray,SizeConst =3)]
        //[MarshalAs(UnmanagedType.ByValArray)]
        public  byte []resvl;      ///< 保留3字节，填充0
        public byte byImgFormat;   ///< 图像格式，D_RAW8、D_RAW10...
        public ushort uWidth;          ///< 图像的宽度，单位字节
        public ushort uHeight;     ///< 图像的高度，单位字节
        public uint uDataSize;     ///< 数据量大小，单位字节
        public uint uFrameTag;     ///< 功能升级标识
        double fFSTimeStamp;    ///< 帧开始的时间戳
        double fFETimeStamp;    ///< 帧结束的时间戳
        public uint uEccErrorCnt;  ///< 每帧的ECC错误计数，只对MIPI接口有效
        public uint uCrcErrorCnt;  ///< 每帧的CRC错误计数，只对MIPI接口有效
        public uint uFrameID;      ///< 帧计数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        //[MarshalAs(UnmanagedType.ByValArray)]
        public  uint[]resv;		///< 保留，填充0
    };

    /* MIPI ctrl 扩展结构体 */
    public struct MipiCtrlEx_t
    {
        /* mipi phy set (d-phy_1.5G/d-phy_2.5G,c-phy_2.0G */
        public byte byPhyType;

        /* lane个数设置，d-phy有1/2/4lane,c-phy有1/2/3lane*/
        public byte byLaneCnt;

        /* MIPI ctrl */
        //  DWORD dwCtrl;
        public uint dwCtrl;
        /* 设置接收的图像通道号，0/1/2/3 */
        public uint uVc;

        /* 使能过滤其他的虚拟通道 */
        bool bVCFilterEn;

        /* 使能输出的ID号 */
        public uint uPackID;

        /* 使能当前设置的ID号输出 */
        int bPackIDEn;

        /* 保留，填充0 */
        //   public byte resv[62];
        [MarshalAs(UnmanagedType.ByValArray)]
        public byte[] resv;

    } ;
    //PMU
    public enum SENSOR_POWER
    {
        /* A通道，或只有一个通道时 */
        POWER_AVDD = 0,     ///<A通道AVDD
        POWER_DOVDD,        ///<A通道DOVDD
        POWER_DVDD,         ///<A通道DVDD
        POWER_AFVCC,        ///<A通道AFVCC
        POWER_VPP,          ///<A通道VPP
        ////* B通道 */
        POWER_AVDD_B,       ///<B通道AVDD
        POWER_DOVDD_B,      ///<B通道DOVDD
        POWER_DVDD_B,       ///<B通道DVDD
        POWER_AFVCC_B,      ///<B通道AFVCC
        POWER_VPP_B,        ///<B通道VPP
    };

    public enum CURRENT_RANGE
    {
        CURRENT_RANGE_MA = 0,    ///<电流测试量程为mA
        CURRENT_RANGE_UA,        ///<电流测试量程为uA
        CURRENT_RANGE_NA         ///<电流测试量程为nA
    };
    public enum RUNMODE
    {
        RUNMODE_PLAY = 0,
        RUNMODE_PAUSE,
        RUNMODE_STOP,
    };

    //SENSOR
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SensorTab
    {
        public UInt16 width;
        public UInt16 height;
        public Byte type;
        public Byte pin;
        public Byte slaveID;
        public Byte mode;
        public UInt16 flagReg;
        public UInt16 flagData;
        public UInt16 flagMask;
        public UInt16 flagReg1;
        public UInt16 flagData1;
        public UInt16 flagMask1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] paraList;
        //public ushort* ParaList;
        public UInt16 paraListSize;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] sleepParaList;
        public UInt16 sleepParaListSize;
        public Byte outformat;
        public int mclk;
        public int avdd;
        public int dovdd;
        public int dvdd;
        public int afvcc;
        public Byte port;
        public UInt16 ext0;
        public UInt16 ext1;
        public UInt16 ext2;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] beaf_InitParaList;
        public UInt16 bfaf_InitParaListSize;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] bgaf_AutoParaList;
        public UInt16 bhaf_AutoParaListSize;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] bjaf_FarParaList;
        public UInt16 bkaf_FarParaListSize;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] blaf_NearParaList;
        public UInt16 boaf_NearParaListSize;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] bpexposure_ParaList;
        public UInt16 bqexposure_ParaListSize;
        [MarshalAs(UnmanagedType.ByValArray)]
        public UInt16[] bmgain_ParaList;
        public UInt16 bngain_ParaListSize;

        public int pclk_pol;
        public int hsync_pol;
        public int vsync_pol;
        public int sync_pol_auto;
    }
    class DTCCM2_API
    {
        public const string str_dll_file = "dtccm2.dll";
        private const int DEFAULT_DEV_ID = 0;

        /// @brief 枚举设备，获得设备名及设备个数。
        /// @param DeviceName：枚举的设备名
        /// @param iDeviceNumMax：指定枚举设备的最大个数
        /// @param pDeviceNum：枚举的设备个数
        /// @retval DT_ERROR_OK：枚举操作成功
        /// @retval DT_ERROR_FAILED:枚举操作失败
        /// @retval DT_ERROR_INTERNAL_ERROR:内部错误
        /// @note 获取的设备名称字符串需要用户程序调用GlobalFree()逐个释放。
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnumerateDevice")]
        public static extern int EnumerateDevice(IntPtr[] DeviceName, int iDeviceNumMax, ref int pDeviceNum);
        
      //  [DllImport("dtccm2.dll", EntryPoint = "EnumerateDevice", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
      //  public static extern int EnumerateDevice(IntPtr[] deviceName, int deviceNumMax, ref int DeviceNum);
        /// @brief 关闭设备，关闭设备后，不能再操作。
        /// @retval DT_ERROR_OK：关闭设备成功
        /// @retval DT_ERROR_FAILD：关闭设备失败
        /// DTFPM_API int _DTCALL_ CloseDevice(int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CloseDevice")]
        public static extern int CloseDevice(int iDevID = DEFAULT_DEV_ID);

        /// @brief 打开设备，只有打开成功后，设备才能操作;设备对象跟给的ID对应起来iDevID=1 则创建设备对象m_device[1]，iDevID=0 则创建设备对象m_device[0]；
		/// @param pszDeviceName：打开设备的名称
		/// @param pDevID：返回打开设备的ID号
		/// @retval DT_ERROR_OK：打开设备成功
		/// @retval DT_ERROR_FAILD：打开设备失败
		/// @retval DT_ERROR_INTERNAL_ERROR：内部错误
		/// @retval DT_ERROR_PARAMETER_INVALID：参数无效
		/// DTFPM_API int _DTCALL_ OpenDevice(const char *pszDeviceName,int *pDevID,int iDevID=DEFAULT_DEV_ID);
		[DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OpenDevice")]
        public static extern int OpenDevice(string DeviceName, ref int pDevID, int iDevID = DEFAULT_DEV_ID);

        /// @brief 判断设备是否打开。 
        /// @retval DT_ERROR_OK：设备已经连接打开
        /// @retval DT_ERROR_FAILED：设备没有连接成功
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "IsDevConnect")]
        public static extern int IsDevConnect(int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置柔性接口是否使能上拉电阻。
        /// @param bPullup：柔性接口上拉使能，bPullup=TRUE使能上拉电阻，bPullup=FALSE关闭上拉电阻
        /// @retval DT_ERROR_OK：设置成功
        /// @retval DT_ERROR_FAILED：设置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSoftPinPullUp")]
        public static extern int SetSoftPinPullUp(bool pullup, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置柔性接口。
        /// @param PinConfig：柔性接口配置定义
        /// @retval DT_ERROR_OK：柔性接口配置成功
        /// @retval DT_ERROR_FAILED：柔性接口配置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSoftPin")]
        public static extern int SetSoftPin(byte[] pinConfig, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置是否使能柔性接口，没使能时为高阻状态。
        /// @param bEnable：柔性接口使能
        /// @retval DT_ERROR_OK：设置成功
        /// @retval DT_ERROR_FAILED：设置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnableSoftPin")]
        public static extern int EnableSoftPin(bool enble, int defDevID = DEFAULT_DEV_ID);

        /// @brief 使能GPIO。
        /// @param bEnable：使能GPIO
        /// @retval DT_ERROR_OK：设置成功
        /// @retval DT_ERROR_FAILED：设置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnableGpio")]
        public static extern int EnableGpio(bool enble, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置SENSOR的输入时钟。
        /// @param bOnOff：使能SENSOR的输入时钟，为TRUE开启输入时钟，为FALSE关闭输入时钟
        /// @param uHundKhz：SENSOR的输入时钟值，单位为100Khz
        /// @retval DT_ERROR_OK：设置SENSOR输入时钟成功
        /// @retval DT_ERROR_FAILED：设置SENSOR输入时钟失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSensorClock")]
        public static extern int SetSensorClock(bool onoff, ushort hundKhz, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置与SENSOR通讯的I2C总线速率，400Kbps或100Kbps。
        /// @param b400K：b400K=TURE，400Kbps；b400K=FALSE,100Kbps							 
        /// @retval DT_ERROR_OK：设置总线速率操作成功
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSensorI2cRate")]
        public static extern int SetSensorI2cRate(bool b400K, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置与SENSOR通讯的I2C总线速率，范围10Khz-2Mhz。
        /// @param uKpbs：设置I2C总线速率，范围值为10-2000						 
        /// @retval DT_ERROR_OK：设置总线速率操作成功
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSensorI2cRateEx")]
        public static extern int SetSensorI2cRateEx(uint uKpbs, int defDevID = DEFAULT_DEV_ID);

        /// @brief 使能与SENSOR通讯的I2C总线为Rapid模式。
        /// @param  bRapid=1表示，强制灌电流输出高电平;=0，I2C管脚为输入状态，借助外部上拉变成高电平							 
        /// @retval DT_ERROR_OK：设置I2C总线Rapid模式成功
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSensorI2cRapid")]
        public static extern int SetSensorI2cRapid(bool brapid, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置I2C的字节间隔(HS系列,PE950支持)
        /// @brief uInterval：字节间隔设置,单位us
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetI2CInterval")]
        public static extern int SetI2CInterval(UInt32 ctrl, int defDevID = DEFAULT_DEV_ID);

        /// @brief 通过Reset,PWDN管脚开启或关闭SENSOR。
        /// @param byPin：Reset，PWDN，PWDN2
        /// @param bEnable：开启或关闭SENSOR
        /// @retval DT_ERROR_OK：开启或关闭SENSOR操作成功
        /// @retval DT_ERROR_FAILED：开启或关闭SENSOR操作失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SensorEnable")]
        public static extern int SensorEnable(byte pin, bool enable, int defDevID = DEFAULT_DEV_ID);

        /// @brief 复位与Sensor通讯的I2C总线。
        /// @retval DT_ERROR_OK：复位I2C操作成功
        /// @retval DT_ERROR_FAILED：复位I2C操作失败
       [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ResetSensorI2cBus")]
        public static extern int ResetSensorI2cBus(int defDevID = DEFAULT_DEV_ID);

        /// @brief 检查插上的SENSOR是不是当前指定的，并且可以同时给SENSOR进行一次复位。
        /// @param pInfo：SENSOR信息，参见SensorTab结构体
        /// @param byChannel：通道选择，A/B通道，参见宏定义“多SENSOR模组通道定义”
        /// @param bReset：给SENSOR复位
        /// @retval DT_ERROR_OK：找到SENSOR
        /// @retval DT_ERROR_FAILED：没有找到SENSOR
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SensorIsMe")]
        public static extern int SensorIsMe(ref SensorTab info, byte channel, bool reset, int defDevID = DEFAULT_DEV_ID);

        /// @brief 初始化SENSOR。
        /// @param uDevAddr：SENSOR器件地址
        /// @param pParaList：SENSOR的参数列表
        /// @param uLength：pParaList的大小
        /// @param byI2cMode：访问SENSOR的I2C模式，参见枚举类型I2CMODE
        /// @retval DT_ERROR_OK：初始化SENSOR成功
        /// @retval DT_ERROR_FAILED：初始化SENSOR失败
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitSensor")]
        public static extern int InitSensor(byte addr, ushort[] paraList, ushort length, byte i2cmode, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置MIPI接口接收器时钟相位。
        /// @param byPhase：MIPI接口接收器时钟相位（可以设置的值是0-7）
        /// @retval DT_ERROR_OK：设置MIPI接口接收器时钟相位成功
        /// @retval DT_ERROR_FAILED：设置MIPI接口接收器时钟相位失败
        /// @retval DT_ERROR_TIME_OUT：设置超时
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetMipiClkPhase")]
        public static extern int SetMipiClkPhase(byte phase, int defDevID = DEFAULT_DEV_ID);

        /// @brief 读SESNOR寄存器,I2C通讯模式byI2cMode的设置值见I2CMODE定义。
        /// @param uAddr：从器件地址
        /// @param uReg：寄存器地址
        /// @param pValue：读到的寄存器的值
        /// @param byMode：I2C模式
        /// @retval DT_ERROR_OK：读SENSOR寄存器操作成功
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_INVALID：byMode参数无效
        /// @retval DT_ERROR_TIME_OUT：通讯超时
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReadSensorReg")]
        public static extern int ReadSensorReg(byte devAddr, ushort regAddr, ref ushort data, byte mode, int defDevID = DEFAULT_DEV_ID);

        /// @brief 写SENSOR寄存器，支持向一个寄存器写入一个数据块（不超过255字节）。
        /// @param uDevAddr：从器件地址
        /// @param uRegAddr：寄存器地址
        /// @param uRegAddrSize：寄存器地址的字节数
        /// @param pData：写入寄存器的数据块
        /// @param uSize：写入寄存器的数据块的字节数（不超过255字节(HS300/HS300D/HV910/HV910D一次不能超过253字节)）
        /// @retval DT_ERROR_OK：完成写SENSOR寄存器块操作成功
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_INVALID：uSize参数无效
        /// @retval DT_ERROR_TIME_OUT：通讯超时
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReadSensorI2c")]
         public static extern int ReadSensorI2c(byte devAddr, ushort regAddr, byte regAddrSize, IntPtr lp, ushort size, bool noStop = false, int defDevID = DEFAULT_DEV_ID);

        /// @brief 获取电源电压，如果能获取检测到的，尽量使用检测到的数据，否则返回电压设置值。
        /// @param Power：电源类型，参见枚举类型“SENSOR_POWER”
        /// @param Voltage：获取的电源电压值，单位mV
        /// @param iCount：电源路数
        /// @retval DT_ERROR_OK：设置电源电压成功
        /// @retval DT_ERROR_FAILD：设置电源电压失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_OUT_OF_BOUND：参数超出了范围
        /// @see SENSOR_POWER
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "PmuGetVoltage")]
        public static extern int PmuGetVoltage(SENSOR_POWER[] power, int[] voltage, int count, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置电源电压。
        /// @param Power：电源类型，参见枚举类型“SENSOR_POWER”
        /// @param Voltage：设置的电源电压值，单位mV
        /// @param iCount：电源路数
        /// @retval DT_ERROR_OK：设置电源电压成功
        /// @retval DT_ERROR_FAILD：设置电源电压失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_OUT_OF_BOUND：参数超出了范围
        /// @see SENSOR_POWER
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "PmuSetVoltage")]
        public static extern int PmuSetVoltage(SENSOR_POWER[] power, int[] voltage, int count, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置电源开关状态。
        /// @param Power：电源类型，参见枚举类型“SENSOR_POWER”
        /// @param OnOff：设置电源开关状态，TRUE为开启，FALSE为关闭
        /// @param iCount：电源路数
        /// @retval DT_ERROR_OK：设置电源开关状态成功
        /// @retval DT_ERROR_FAILD：设置电源开关状态失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_OUT_OF_BOUND：参数超出了范围
        /// @see SENSOR_POWER
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "PmuSetOnOff")]
        public static extern int PmuSetOnOff(SENSOR_POWER[] power, bool[] onoff, int count, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置电源电流量程。
        /// @param Power：电源类型，参见枚举类型“SENSOR_POWER”
        /// @param Range：电源电流量程，参见枚举类型“CURRENT_RANGE”
        /// @param iCount：电源路数
        /// @retval DT_ERROR_OK：设置电源电流量程成功
        /// @retval DT_ERROR_FAILD：设置电源电流量程失败
        /// @see SENSOR_POWER
        /// @see CURRENT_RANGE
        /// @note 该函数仅UV910/DTLC2/UH910/UH920/UF920/PE350/PE950支持。
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "PmuSetCurrentRange")]
        public static extern int PmuSetCurrentRange(SENSOR_POWER[] power, CURRENT_RANGE[] range, int count, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置过流保护的电流限制,设定值(CurrentLimit)单位:mA。
        /// @param Power：电源类型，参见枚举类型“SENSOR_POWER”
        /// @param CurrentLimit：设置过流保护的电流限制值，单位mA
        /// @param iCount：电源路数
        /// @retval DT_ERROR_OK：设置过流保护的电流限制成功
        /// @retval DT_ERROR_FAILD：设置过流保护的电流限制失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_OUT_OF_BOUND：参数超出了范围
        /// @see SENSOR_POWER
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "PmuSetOcpCurrentLimit")]
        public static extern int PmuSetOcpCurrentLimit(SENSOR_POWER[] power, int[] currentlimit, int count, int defDevID = DEFAULT_DEV_ID);

        /// @brief 返回设备的型号，区分不同的测试板。
        ///
        /// @retval 0x0010：HS128测试板
        /// @retval 0x0020：HS230测试板
        /// @retval 0x0030：HS300测试板
        /// @retval 0x0031：HS300D测试板
        /// @retval 0x0092：HV910测试板
        /// @retval 0x0093：HV910D测试板
        /// @retval 0x0082：HV810测试板
        /// @retval 0x0083：HV810D测试板
        ///
        /// @retval 0x0130：PE300测试板
        /// @retval 0x0131：PE300D测试板
        /// @retval 0x0190：PE910测试板
        ///	@retval 0x0191：PE910D测试板
        /// @retval 0x0180：PE810测试板
        ///	@retval 0x0181：PE810D测试板
        /// @retval 0x0132：PE350测试板
        /// @retval 0x0192：PE950测试板
        /// @retval 0x0193：MP950测试板
        ///
        ///	@retval 0x0231：UT300测试板
        /// @retval 0x0232：UO300测试板
        /// @retval 0x0233: UM330测试板
        /// @retval 0x0295：UM900测试板
        /// @retval 0x0296：MU950测试板
        /// @retval 0x0297：DMU956测试板
        /// @retval 0x0239：ULV330测试板
        /// @retval 0x0299：ULV913测试板
        ///	@retval 0x0292：UV910测试板
        ///	@retval 0x0293：UH910测试板
        ///	@retval 0x02A1：DTLC2测试板
        /// @retval 0x0295：UF920测试板
        ///	@retval 0x0294：UH920测试板
        /// DTFPM_API DWORD _DTCALL_ GetKitType(int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetKitType")]
        public static extern uint GetKitType(int iDevID = 0);

        /// @brief OS测试参数配置。
        ///
        /// @param Voltage：测试电压，单位uV
        /// @param HighLimit：Open测试标准数组，测试之前应该把每一个测试pin的开路标准初始化好，单位uV
        /// @param LowLimit：Short测试标准数组，测试之前应该把每一个测试pin的开路标准初始化好，单位uV
        /// @param PinNum：管脚数，这个决定HighLimit、LowLimit数组大小
        /// @param PowerCurrent：电源pin电流，单位uA
        /// @param GpioCurrent：GPIOpin电流，单位uA
        ///
        /// @retval DT_ERROR_OK：OS测试参数配置成功
        /// @retval DT_ERROR_FAILD：OS测试参数配置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// DTFPM_API int _DTCALL_ OS_Config(int Voltage, int HighLimit[], int LowLimit[], int PinNum, int PowerCurrent, int GpioCurrent, int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OS_Config")]
        public static extern int OS_Config(int Voltage, int[] HighLimit, int[] LowLimit, int PinNum, int PowerCurrent, int GpioCurrent, int iDevID = 0);

        /// @brief LC/OS测试操作配置。
        /// 
        /// @param Command：操作码，参见宏定义“OS/LC测试配置定义”
        /// @param IoMask：有效管脚标识位，每字节的每bit对应一个管脚，如果这些bit为1，表示对应的管脚将参与测试
        /// @param PinNum：管脚数，这个决定IoMask数组大小，一般情况下IoMask的字节数为：PinNum/8+(PinNum%8!=0)
        ///
        /// @retval DT_ERROR_OK：LC/OS测试操作配置成功
        /// @retval DT_ERROR_FAILD：LC/OS测试操作配置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        ///  DTFPM_API int _DTCALL_ LC_OS_CommandConfig(DWORD Command, UCHAR IoMask[], int PinNum, int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LC_OS_CommandConfig")]
        public static extern int LC_OS_CommandConfig(uint Command, byte[] IoMask, int PinNum, int iDevID = 0);

        /// @brief OS测试结果读取。
		///
		/// @param VoltageH：正向pos测试结果，单位uV
		/// @param VoltageH：反向pos测试结果，单位uV
		/// @param Result：开短路测试结果，参见宏定义“OS测试结果定义”
		/// @param PosEn：正向测试使能 
		/// @param NegEn：反向测试使能
		/// @param PinNum：管脚数，这个决定VoltageH、VoltageL，Result数组大小
		/// 
		/// @retval DT_ERROR_OK：OS测试结果读取成功
		/// @retval DT_ERROR_FAILD：OS测试结果读取失败
		/// @retval DT_ERROR_COMM_ERROR：通讯错误
		/// DTFPM_API int _DTCALL_ OS_Read(int VoltageH[], int VoltageL[], UCHAR Result[], BOOL PosEn, BOOL NegEn, int PinNum, int iDevID=DEFAULT_DEV_ID);
		[DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OS_Read")]
        public static extern int OS_Read(int[] VoltageH, int[] VoltageL, byte[] Result, Boolean PosEn, Boolean NegEn, int PinNum, int iDevID = 0);

        /// @brief 采集一帧图像，并且返回帧的一些信息，通过帧信息结构体可以获取帧的时间戳、当前帧的ECC错误计数、CRC错误计数等
        ///
        /// @param pInBuffer：采集图像BUFFER
        /// @param uBufferSize：采集图像BUFFER大小，单位字节
        /// @param pGrabSize：实际抓取的图像数据大小，单位字节
        /// @param pInfo：返回的图像数据信息
        ///
        /// @retval DT_ERROR_OK：采集一帧图像成功
        /// @retval DT_ERROR_FAILD：采集一帧图像失败，可能不是完整的一帧图像数据
        /// @retval DT_ERROR_TIME_OUT：采集超时
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误 
        /// 
        /// @note 调用该函数之前，请先根据图像大小获取到足够大的缓存区用于装载图像数据。\n
        /// 同时，缓存区的大小也需要作为参数传入到GrabFrameEx函数，以防止异常情况下导致的内存操作越界问题。 
     
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GrabFrameEx")]
        public static extern int GrabFrameEx(IntPtr pInBuffer, uint uBufferSize, ref uint pGrabSize, ref FrameInfoEx pInfo,  int iDevID = 0);
        /// @brief 采集一帧图像，并且返回帧的一些信息，A通道和B通道都是使用GrabFrame函数获取图像数据，通过帧信息可以区分图像数据所属的通道。
        /// @param pInBuffer：采集图像BUFFER
        /// @param uBufferSize：采集图像BUFFER大小，单位字节
        /// @param pGrabSize：实际抓取的图像数据大小，单位字节
        /// @param pInfo：返回的图像数据信息
        /// @retval DT_ERROR_OK：采集一帧图像成功
        /// @retval DT_ERROR_FAILD：采集一帧图像失败，可能不是完整的一帧图像数据
        /// @retval DT_ERROR_TIME_OUT：采集超时
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误 
        /// @note 调用该函数之前，请先根据图像大小获取到足够大的缓存区用于装载图像数据。\n
        /// 同时，缓存区的大小也需要作为参数传入到GrabFrame函数，以防止异常情况下导致的内存操作越界问题。
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GrabFrame")]
         public static extern int GrabFrame(IntPtr pInBuffer,uint uBufferSize, ref uint pGrabSize, ref FrameInfo pInfo, int iDevID = 0);
       // unsafe public static extern int GrabFrame(byte* imagebuffer, uint buffersize, ref uint grabsize, ref FrameInfo info, int defDevID = DEFAULT_DEV_ID);
        // DTCCM_API int _DTCALL_ GrabFrame(BYTE* pInBuffer, ULONG uBufferSize, ULONG* pGrabSize, FrameInfo* pInfo, int iDevID = DEFAULT_DEV_ID);
        /// @brief 对RAW图像数据进行图像处理(MONO,WB,ColorChange,Gamma,Contrast)。
        ///
        /// @param pImage：RAW图像数据
        /// @param pBmp24：经过图像处理后的数据
        /// @param uWidth：图像数据宽度
        /// @param uHeight：图像数据高度
        /// @param pInfo：帧信息，参见结构体“FrameInfo”
        /// 
        /// @retval DT_ERROR_OK：图像处理成功
        /// @retval DT_ERROR_PARAMETER_INVALID：pData无效的参数
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误
        /// DTFPM_API int _DTCALL_ ImageProcess(BYTE *pImage, BYTE *pBmp24, int nWidth, int nHeight,FrameInfo *pInfo,int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProcess")]
        public static extern int ImageProcess(IntPtr pImage, IntPtr pBmp24, int nWidth, int nHeight, ref FrameInfo pInfo, int iDevID = 0);


        /// @brief 显示RGB图像数据。
        ///
        /// @param pBmp24：待显示的RGB24格式的数据
        /// @param pInfo：帧信息，参见结构体“FrameInfo”
        ///
        /// @retval DT_ERROR_OK：显示RGB图像成功
        /// @retval DT_ERROR_FAILD：显示RGB图像失败 
        /// DTFPM_API int _DTCALL_ DisplayRGB24(BYTE *pBmp24,FrameInfo *pInfo=NULL,int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DisplayRGB24")]
        public static extern int DisplayRGB24(IntPtr pBmp24, ref FrameInfo pInfo, int iDevID = 0);
      
        /// @brief 开启图像数据采集。
        ///
        /// @param uImgBytes：图像数据大小，单位字节
        ///
        /// @retval DT_ERROR_OK：开启图像数据采集成功
        /// @retval DT_ERROR_FAILD：开启图像数据采集失败
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// DTFPM_API int _DTCALL_ OpenVideo(UINT uImgBytes,int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OpenVideo")]
        public static extern int OpenVideo(uint uImgBytes, int iDevID = 0);
        /// @brief 关闭图像数据采集。
        ///
        /// @retval DT_ERROR_OK：关闭图像数据采集成功
        /// @retval DT_ERROR_FAILD：关闭图像数据采集失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// DTFPM_API int _DTCALL_ CloseVideo(int iDevID=DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CloseVideo")]
        public static extern int CloseVideo(int iDevID = 0);

        /// @brief 初始化显示，支持2个窗口显示，如果使用2个sensor，须要使用hWndEx指定第二个窗口。
        /// @param hWnd：显示A通道图像的窗口句柄
        /// @param uImgWidth：图像数据宽度
        /// @param uHeight：图像数据高度
        /// @param byImgFormat：图像数据格式，如：RAW/YUV
        /// @param hWndEx：hWndEx：显示B通道图像的窗口句柄
        /// 
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitDisplay")]
        public static extern int InitDisplay(uint hwnd, ushort imgWidth, ushort imgHeight, byte imgFormat, byte channel, IntPtr hwndEx, int defDevID = DEFAULT_DEV_ID);

        /// @brief 初始化ISP
        /// @param uImgWidth：图像数据宽度
        /// @param uHeight：图像数据高度
        /// @param byImgFormat：图像数据格式，如：RAW/YUV
        /// @param byChannel：A/B通道选择
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitIsp")]
        public static extern int InitIsp(ushort imgWidth, ushort imgHeight, byte imgFormat, byte imgChannel, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置ROI。
        /// @param roi_x0：起始点水平坐标，单位像素
        /// @param roi_y0：起始点垂直坐标，单位像素
        /// @param roi_hw：水平方向ROI图像宽度，单位像素
        /// @param roi_vw：垂直方向ROI图像高度，单位像素
        /// @param roi_hb：水平方向blank宽度，单位像素
        /// @param roi_vb：水平方向blank高度，单位像素
        /// @param roi_hnum：水平方向ROI数量，单位像素
        /// @param roi_vnum：垂直方向ROI数量，单位像素
        /// @param byImgFormat：图像数据格式，如：RAW/YUV
        /// @param roi_en：ROI使能
        /// @retval DT_ERROR_OK：ROI设置成功
        /// @retval DT_ERROR_FAILD：ROI设置失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @note 该函数中指定宽度和水平位置是以像素为单位，并且要保证宽度转为字节后是16字节的整数倍。
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitRoi")]
        public static extern int InitRoi(ushort roi_x0, ushort roi_y0, ushort roi_hw, ushort roi_vw, ushort roi_hb, ushort roi_vb, ushort roi_hnum, ushort roi_vnum, byte imgFormat, bool enable, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置SENSOR图像数据接口类型。
        /// @param byPort：SENSOR图像数据接口类型，参见枚举类型“SENSOR_PORT”
        /// @param uWidth：图像数据宽度
        /// @param uHeight：图像数据高度
        /// @retval DT_ERROR_OK：设置SENSOR图像数据接口类型成功
        /// @retval DT_ERROR_FAILD：设置SENSOR图像数据接口类型失败 
        /// @retval DT_ERROR_PARAMETER_INVALID：无效的图像数据接口类型参数
        /// @see SENSOR_PORT
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSensorPort")]
        public static extern int SetSensorPort(byte port, ushort width, ushort height, int defDevID = DEFAULT_DEV_ID);

        /// @brief 返回实际抓取图像数据的大小（单位字节）。
        /// @param pGrabSize：返回实际抓取图像数据大小
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CalculateGrabSize")]
        public static extern int CalculateGrabSize(ref uint grabSize, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置GAMMA值。
        /// @param iGamma：设置的GAMMA值
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGamma")]
        public static extern int SetGamma(int gamma, int defDevID = DEFAULT_DEV_ID);

        /// @brief 获取对比度设置值。
        /// @param pContrast：返回的对比度设置值
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetContrast")]
        public static extern int SetContrast(int contrast, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置饱和度。
        /// @param iSaturation：设置饱和度值
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetSaturation")]
        public static extern int SetSaturation(int saturation, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置RGB数字增益。
        /// @param fRGain：R增益值
        /// @param fGGain：G增益值
        /// @param fBGain：B增益值
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDigitalGain")]
        public static extern int SetDigitalGain(float gainR, float gainG, float gainB, int defDevID = DEFAULT_DEV_ID);

        /// @brief 设置RAW格式，参见枚举类型“RAW_FORMAT”。
        /// @param byRawMode：RAW格式设置
        /// @see RAW_FORMAT
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetRawFormat")]
        public static extern int SetRawFormat(byte rawmode, int defDevID = DEFAULT_DEV_ID);

        
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetMipiCtrlEx")]
        public static extern int GetMipiCtrlEx(ref MipiCtrlEx_t pMipiCtrl, int defDevID = DEFAULT_DEV_ID);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetMipiCtrlEx")]
        public static extern int SetMipiCtrlEx(MipiCtrlEx_t pMipiCtrl, int defDevID = DEFAULT_DEV_ID);

        /// @brief 校准sensor接收，建议openvideo之后调用，校准成功再进行抓帧操作,建议超时时间大于1000ms
        /// 
        /// @param uTimeOut：校准超时时间设置，单位ms
        /// 
        /// @retval DT_ERROR_OK：校准成功，可以采集图像
        /// @retval DT_ERROR_TIME_OUT：校准超时
        // DTCCM_API int _DTCALL_ CalibrateSensorPort(ULONG uTimeOut, int iDevID = DEFAULT_DEV_ID);
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CalibrateSensorPort")]
        public static extern int CalibrateSensorPort(uint uTimeOut , int defDevID = DEFAULT_DEV_ID);

        /// @brief 获取设备的唯一序列号
        ///
        /// @param pSN：返回的设备唯一序列号
        /// @param iBufferSize：设置要获取序列号字节的长度,最大支持32字节
        /// @param pRetLen：返回实际设备序列号字节长度
        /// 
        /// @retval DT_ERROR_OK：获取设备的序列号成功
        /// @retval DT_ERROR_FAILED：获取设备的序列号失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        ///     
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDeviceSN")]
        //DTCCM_API int _DTCALL_ GetDeviceSN(BYTE* pSN, int iBufferSize, int* pRetLen, int iDevID = DEFAULT_DEV_ID);
        public static extern int GetDeviceSN(Byte[] pSN, int iBufferSize ,ref int pRetLen, int iDevID = DEFAULT_DEV_ID);

        /////////////////////////////////////自己添加/////////////////////////////////////////////////////
        ////// @brief 设置并行接口控制器。
        ///
        /// @param dwCtrl：并行接口控制器操作码，参见宏定义“同步并行接口特性的位定义”
        ///
        /// @retval DT_ERROR_OK：设置并行接口控制器成功
        /// @retval DT_ERROR_FAILED：设置并行接口控制器失败
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetParaCtrl")]
        public static extern int SetParaCtrl(uint dwCtrl, int iDevID = DEFAULT_DEV_ID);


        /// @brief 初始化设备，该函数主要用于初始化设备的SENSOR接口类型，图像格式，图像宽高信息，同时还要求用户传入用于视频显示的窗口句柄。
        ///
        /// @param hWnd：显示A通道图像的窗口句柄
        /// @param uImgWidth，uImgHeight：设置SENSOR输出的宽高信息（单位：像素，可能ROI之后的结果）
        /// @param bySensorPort：SENSOR输出接口类型，如：MIPI/并行
        /// @param byImgFormat：图像数据格式，如：RAW/YUV
        /// @param byChannel：A通道/B通道/AB同时工作
        /// @param hWndEx：显示B通道图像的窗口句柄
        ///
        /// @retval DT_ERROR_OK：初始化成功
        /// @retval DT_ERROR_FAILD：初始化失败
        /// @retval DT_ERROR_PARAMETER_INVALID：bySensorPort参数无效
        ///
        /// @note InitDevice函数支持初始化双通道测试板（如DTLC2/UH910），如果须要使用这类测试板的B通道，请做如下额外操作：
        /// @note byChannel参数传入CHANNEL_A|CHANNEL_B；hWndEx参数传入用于B通道视频显示的窗口句柄。
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitDevice")]
        public static extern int InitDevice(IntPtr hWnd,
                                          ushort uImgWidth,
                                          ushort uImgHeight,
                                          byte bySensorPort,
                                          byte byImgFormat,
                                          byte byChannel = 0x01,
                                          Form hWnd1 = null,
                                          int iDevID = DEFAULT_DEV_ID);


        /// @brief 写SENSOR寄存器,I2C通讯模式byI2cMode的设置值见I2CMODE定义。
        /// 
        /// @param uAddr：从器件地址
        /// @param uReg：寄存器地址
        /// @param uValue：写入寄存器的值
        /// @param byMode：I2C模式
        ///
        /// @retval DT_ERROR_OK：写SENSOR寄存器操作成功
        /// @retval DT_ERROR_COMM_ERROR：通讯错误
        /// @retval DT_ERROR_PARAMETER_INVALID：byMode参数无效
        /// @retval DT_ERROR_TIME_OUT：通讯超时
        /// @retval DT_ERROR_INTERNAL_ERROR：内部错误
        ///
        /// @see I2CMODE
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "WriteSensorReg")]
        public static extern int WriteSensorReg(byte uAddr, ushort uReg, ushort uValue, byte byMode, int iDevID = DEFAULT_DEV_ID);



        /// @brief RAW/YUV转RGB，源数据格式由ImgFormat指定。
        ///
        /// @param pIn：源图像数据
        /// @param pOut：转为RGB24的数据
        /// @param uWidth：图像数据宽度
        /// @param uHeight：图像数据高度
        /// @param byImgFormat：源数据的格式
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DataToRGB24")]
        public static extern int DataToRGB24(byte[] pIn, byte[] pOut, ushort uWidth, ushort uHeight, byte byImgFormat, int iDevID = DEFAULT_DEV_ID);

        /// @brief 设置YUV格式，参见枚举类型“YUV_FORMAT”。
        ///
        /// @param byYuvMode：YUV格式设置
        ///
        /// @see YUV_FORMAT
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetYUV422Format")]
        public static extern int SetYUV422Format(byte byYuvMode, int iDevID = DEFAULT_DEV_ID);

    }
}
