using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motion.Enginee
{
    /// <summary>
    /// 报警信息判断
    /// </summary>
    public class Alarm
    {
        private readonly Func<bool> _condition;
        public Alarm(Func<bool> condition)
        {
            _condition = condition;
        }
        /// <summary>
        ///     报警级别
        /// </summary>
        public AlarmLevels AlarmLevel { get; set; }
        /// <summary>
        /// 是否报警
        /// </summary>
        public bool IsAlarm
        {
            get
            {
                try
                {
                    return _condition();
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// 报警名称
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// 报警类型
    /// </summary>
    public struct AlarmType
    {
        public bool IsAlarm;
        public bool IsPrompt;
        public bool IsWarning;
    }
}
