namespace Motion.Interfaces
{
    //2018.7.14修改..jiang
    /// <summary>
    /// 外部信号 
    /// </summary>
    public interface IExternal:IAlarmReset
    {
        /// <summary>
        /// 气压信号
        /// </summary>
        bool AirSignal { get; set; }
        /// <summary>
        /// 手自动动模式
        /// </summary>
        bool ManualAutoMode { get; set; }
        /// <summary>
        /// 初始化完成标志
        /// </summary>
        bool InitializingDone { get; set; }
    }
}
