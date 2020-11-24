using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desay.AAVision.Algorithm
{
    class Bitmap2HObject
    {
        public static void Bitmap2HObj(Bitmap bmp, out HObject ho_src_image)
        {
            try
            {
                
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                HOperatorSet.GenImageInterleaved(out ho_src_image, srcBmpData.Scan0, "bgr", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                ho_src_image = null;
                //System.Windows.MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}
