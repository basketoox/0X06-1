using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motion.Enginee
{
    public class CylinderDelay
    {
        /// <summary>
        /// 原点到位时间
        /// </summary>
        public int OriginTime { get; set; }
        /// <summary>
        /// 动点到时时间
        /// </summary>
        public int MoveTime { get; set; }
        /// <summary>
        /// 报警延时
        /// </summary>
        public int AlarmTime { get; set; }
        public override string ToString()
        {
            return OriginTime.ToString() + "," + MoveTime.ToString() + "," + AlarmTime.ToString();
        }
        public static CylinderDelay Parse(string str)
        {
            string[] strValue = str.Split(',');
            var cylinderDelay = new CylinderDelay();
            cylinderDelay.OriginTime = int.Parse(strValue[0]);
            cylinderDelay.MoveTime = int.Parse(strValue[1]);
            cylinderDelay.AlarmTime = int.Parse(strValue[2]);
            return cylinderDelay;
        }
    }
}
