using Motion.Interfaces;
namespace Motion.Enginee
{
    //2018.7.14修改..jiang
    /// <summary>
    /// 外部条件类
    /// </summary>
    public class External : IExternal
    {
        /// <summary>
        /// 气压信号
        /// </summary>
        public bool AirSignal { get; set; }
        /// <summary>
        /// 报警复位
        /// </summary>
        public bool AlarmReset { get; set; }
        /// <summary>
        /// 手自动动模式
        /// </summary>
        public bool ManualAutoMode { get; set; }
        /// <summary>
        /// 初始化完成标志
        /// </summary>
        public bool InitializingDone { get; set; }
    }
}
