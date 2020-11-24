
namespace Motion.Interfaces
{
    /// <summary>
    /// 电磁阀抽象类
    /// </summary>
    public abstract class Cylinder : Automatic
    {
        /// <summary>
        /// 报警可复位
        /// </summary>
        public abstract bool HaveAlarmReset { get; }
        /// <summary>
        /// 原点信号状态
        /// </summary>
        public abstract bool OutOriginStatus { get; }
        /// <summary>
        /// 动点信号状态
        /// </summary>
        public abstract bool OutMoveStatus { get; }
        /// <summary>
        /// 输出监控
        /// </summary>
        public abstract bool IsOutMove { get; }
        /// <summary>
        /// 模块初始化操作
        /// </summary>
        public abstract void InitExecute();
        /// <summary>
        /// 手动操作
        /// </summary>
        public abstract void ManualExecute();
        /// <summary>
        /// 自动置位操作
        /// </summary>
        public abstract void Set();
        /// <summary>
        /// 自动复位操作
        /// </summary>
        public abstract void Reset();
    }
}
