using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motion.Enginee
{
    public class VacuoDelay
    {
        /// <summary>
        /// 吸气时间
        /// </summary>
        public int InhaleTime {  get; set; }
        /// <summary>
        /// 破真空时间
        /// </summary>
        public int BrokenTime {  get; set; }
        /// <summary>
        /// 报警延时
        /// </summary>
        public int AlarmTime {  get; set; }
        public override string ToString()
        {
            return InhaleTime.ToString() + "," + BrokenTime.ToString() + "," + AlarmTime.ToString();
        }
        public static VacuoDelay Parse(string str)
        {
            string[] strValue = str.Split(',');
            var vacuoDelay = new VacuoDelay();
            vacuoDelay.InhaleTime = int.Parse(strValue[0]);
            vacuoDelay.BrokenTime = int.Parse(strValue[1]);
            vacuoDelay.AlarmTime = int.Parse(strValue[2]);
            return vacuoDelay;
        }
    }
}
