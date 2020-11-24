using System;

namespace Motion.Enginee
{
    /// <summary>
    /// 单模组初始化
    /// </summary>
    public class StationInitialize : IStationInitialize
    {
        #region 字段
        private readonly Func<bool> m_condition;
        private readonly Func<bool> m_isAlarm;
        private bool m_StartForbid;
        private bool m_Start;
        private bool m_Estop;
        private bool m_RunSignal;
        #endregion
        public StationInitialize(Func<bool> Condition, Func<bool> IsAlarm)
        {
            m_condition = Condition;
            m_isAlarm = IsAlarm;
        }
        /// <summary>
        /// 启动
        /// </summary>
        public bool Start { set { m_Start = value; } }
        /// <summary>
        /// 急停
        /// </summary>
        public bool Estop { set { m_Estop = value; } }
        /// <summary>
        /// 初始化完成
        /// </summary>
        public bool InitializeDone { get; set; }
        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running { get; private set; }
        /// <summary>
        /// 初始化状态
        /// </summary>
        public InilitiazeStatus Status { get; private set; }
        /// <summary>
        /// 流程步骤
        /// </summary>
        public int Flow { get; set; }
        public void Run()
        {
            //获取执行条件
            var _condition = m_condition();
            //获取故障状态
            var _isAlarm = m_isAlarm() || Flow == -1;
            //首次触发，清除初始化状态，运行标志状态，运行状态
            if (!m_StartForbid && m_Start)
            {
                m_RunSignal = false;
                Running = false;
                Flow = 0;
                InitializeDone = false;
                m_StartForbid = true;
            }
            if (!m_Start) m_StartForbid = false;
            //急停
            if (m_Estop)
            {
                m_RunSignal = false;
                Running = false;
                Flow = 0;
                InitializeDone = false;
            }
            //判断运行标志状态
            if (m_Start && !InitializeDone && _condition && !_isAlarm) m_RunSignal = true;
            //判断是否运行
            if ((m_Start || Running) && _condition && !_isAlarm && m_RunSignal) Running = true;
            else Running = false;
            //判断是否初始化完成
            if (InitializeDone || _isAlarm) m_RunSignal = false;
            //初始化状态
            if (_isAlarm) Status = InilitiazeStatus.初始化故障;
            if (!_isAlarm && !m_RunSignal && !Running && !InitializeDone) Status = InilitiazeStatus.初始化未准备好;
            if (!_isAlarm && InitializeDone) Status = InilitiazeStatus.初始化完成;
            if (!_isAlarm && m_RunSignal && Running) Status = InilitiazeStatus.初始化运行中;
            if (!_isAlarm && m_RunSignal && !Running) Status = InilitiazeStatus.初始化暂停中;
        }
    }
}
