using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
namespace desay
{
  public  class SaveImage
    {
        public static void Save(HWindow window)
        {
            string filename = DateTime.Now.Year.ToString() + "_"
                + DateTime.Now.Month.ToString() + "_"
                + DateTime.Now.Day.ToString() + "_"
                + DateTime.Now.DayOfWeek.ToString() + "_"
                + DateTime.Now.Hour.ToString() + "_"
                + DateTime.Now.Minute.ToString() + "_"
                + DateTime.Now.Second.ToString();
            HObject Image = null;
            HOperatorSet.DumpWindowImage(out Image, window);
            HOperatorSet.WriteImage(Image, "jpg", 0, AppDomain.CurrentDomain.BaseDirectory + "//glueImage//" +　filename);
        }
        
    }
}
