using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.IO;
namespace desay
{
  public  class SaveImage
    {
        public static void Save(HWindow window)
        {
            string _filePath = @"./Image";
            string filename = DateTime.Now.Year.ToString() + "_"
                + DateTime.Now.Month.ToString() + "_"
                + DateTime.Now.Day.ToString() + "_"
                + DateTime.Now.DayOfWeek.ToString() + "_"
                + DateTime.Now.Hour.ToString() + "_"
                + DateTime.Now.Minute.ToString() + "_"
                + DateTime.Now.Second.ToString();
            HObject Image = null;
            HOperatorSet.DumpWindowImage(out Image, window);
            if (!Directory.Exists(_filePath))
            { Directory.CreateDirectory(_filePath); }
            //HOperatorSet.WriteImage(Image, "jpg", 0, AppDomain.CurrentDomain.BaseDirectory + "//glueImage//" +　filename);
            HOperatorSet.WriteImage(Image, "bmp", 0, $"{_filePath }//{ DateTime.Now.ToString("yy_MM_dd_HH_mm_ss_fff")}.jpg"); 
        }
        
    }
}
