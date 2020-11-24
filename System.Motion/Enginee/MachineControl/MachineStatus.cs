namespace Motion.Enginee
{
    /// <summary>
    /// 初始化状态
    /// </summary>
    public enum InilitiazeStatus : byte
    {
        初始化故障,
        初始化未准备好,
        初始化完成,
        初始化运行中,
        初始化暂停中
    }
    /// <summary>
    /// 模组工站状态
    /// </summary>
    public enum StationStatus : byte
    {
        模组报警,
        模组未准备好,
        模组准备好,
        模组运行中,
        模组暂停中
    }
    /// <summary>
    /// 设备状态
    /// </summary>
    public enum MachineStatus : byte
    {
        设备未准备好,
        设备准备好,
        设备运行中,
        设备停止中,
        设备暂停中,
        设备复位中,
        设备报警中,
        设备急停已按下
    }
}