using Motion.Enginee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desay
{
    [Serializable]
    public class Delay
    {
        [NonSerialized]
        public static Delay Instance = new Delay();

        /// <summary>
        /// 清洗阻挡气缸
        /// </summary>
        public CylinderDelay cleanStopCylinderDelay = new CylinderDelay() { OriginTime = 50, MoveTime = 50, AlarmTime = 100 };
        public int cleanStopCylinderOriginTime=50;
        public int cleanStopCylinderMoveTime=50;
        public int cleanStopCylinderAlarmTime=500;

        /// <summary>
        /// 清洗顶升气缸
        /// </summary>
        public CylinderDelay cleanUpCylinderDelay = new CylinderDelay() { OriginTime = 100, MoveTime = 100, AlarmTime = 500 };
        public int cleanUpCylinderOriginTime=100;
        public int cleanUpCylinderMoveTime=100;
        public int cleanUpCylinderAlarmTime=500;

        /// <summary>
        /// 清洗夹紧气缸
        /// </summary>
        public CylinderDelay cleanClampCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int cleanClampCylinderOriginTime=0;
        public int cleanClampCylinderMoveTime=0;
        public int cleanClampCylinderAlarmTime=500;

        /// <summary>
        /// 清洗上下气缸
        /// </summary>
        public CylinderDelay cleanUpDownCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int cleanUpDownCylinderOriginTime=0;
        public int cleanUpDownCylinderMoveTime=0;
        public int cleanUpDownCylinderAlarmTime=500;

        /// <summary>
        /// 清洗旋转气缸
        /// </summary>
        public CylinderDelay cleanRotateCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int cleanRotateCylinderOriginTime=0;
        public int cleanRotateCylinderMoveTime=0;
        public int cleanRotateCylinderAlarmTime=500;

        /// <summary>
        /// 光源上下气缸
        /// </summary>
        public CylinderDelay lightUpDownCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int lightUpDownCylinderOriginTime=0;
        public int lightUpDownCylinderMoveTime=0;
        public int lightUpDownCylinderAlarmTime=500;


        /// <summary>
        /// 点胶阻挡气缸
        /// </summary>
        public CylinderDelay glueStopCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int glueStopCylinderOriginTime=0;
        public int glueStopCylinderMoveTime=0;
        public int glueStopCylinderAlarmTime=500;

        /// <summary>
        /// 点胶顶升气缸
        /// </summary>
        public CylinderDelay glueUpCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int glueUpCylinderOriginTime=0;
        public int glueUpCylinderMoveTime=0;
        public int glueUpCylinderAlarmTime=500;
        /// <summary>
        /// 接驳台平移气缸
        /// </summary>
        public CylinderDelay moveCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int moveCylinderOriginTime=0;
        public int moveCylinderMoveTime=0;
        public int moveCylinderAlarmTime=500;
        /// <summary>
        /// 接驳台顶升气缸
        /// </summary>
        public CylinderDelay carrierUpCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int carrierUpCylinderOriginTime=0;
        public int carrierUpCylinderMoveTime=0;
        public int carrierUpCylinderAlarmTime=500;
        /// <summary>
        /// 接驳台开夹气缸
        /// </summary>
        public CylinderDelay carrierClampCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int carrierClampCylinderOriginTime=0;
        public int carrierClampCylinderMoveTime=0;
        public int carrierClampCylinderAlarmTime=500;
        /// <summary>
        /// 产品下压气缸
        /// </summary>
        public CylinderDelay carrierPressCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int carrierPressCylinderOriginTime = 0;
        public int carrierPressCylinderMoveTime = 0;
        public int carrierPressCylinderAlarmTime = 500;
        /// <summary>
        /// 回流线阻挡气缸
        /// </summary>
        public CylinderDelay carrierStopCylinderDelay = new CylinderDelay() { OriginTime = 0, MoveTime = 0, AlarmTime = 500 };
        public int carrierStopCylinderOriginTime=0;
        public int carrierStopCylinderMoveTime=0;
        public int carrierStopCylinderAlarmTime=500;

    }
}
