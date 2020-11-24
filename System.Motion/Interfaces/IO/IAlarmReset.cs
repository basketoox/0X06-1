namespace Motion.Interfaces
{
    //2018.7.14修改..jiang
    /// <summary>
    /// 报警复位
    /// </summary>
    public interface IAlarmReset
    {
        /// <summary>
        /// 报警复位
        /// </summary>
        bool AlarmReset { get; set; }
    }
}
