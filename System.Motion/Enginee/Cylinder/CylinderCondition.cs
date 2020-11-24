using System;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public class CylinderCondition
    {
        private readonly Func<bool> _offcondition;
        private readonly Func<bool> _oncondition;
        public CylinderCondition(Func<bool> offCondition,Func<bool> onCondition )
        {
            _offcondition = offCondition;
            _oncondition = onCondition;
            External = new External();
        }
        /// <summary>
        /// 无原点屏蔽
        /// </summary>
        public bool NoOriginShield { get; set; }
        /// <summary>
        /// 无动点屏蔽
        /// </summary>
        public bool NoMoveShield { get; set; }
        /// <summary>
        /// 外部信号
        /// </summary>
        public External External { get; set; }
        /// <summary>
        /// 为OFF条件
        /// </summary>
        public bool IsOffCondition {
            get
            {
                try
                {
                    return _offcondition();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 为ON条件
        /// </summary>
        public bool IsOnCondition
        {
            get
            {
                try
                {
                    return _oncondition();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
