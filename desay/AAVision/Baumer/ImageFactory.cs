
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace desay
{
    public class ImageFactory
    {

        public static Bitmap CreateBitmap(byte[] data,int width,int height,string format)
        {
            if (data == null)
            {
                return null;
            }

            try
            {
                if ("Mono8" == format)
                {
                    Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);


                    int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数，图像的每行的宽度的实际字节数将变成大于等于它的那个离它最近的4的整倍数，所以图像的实际宽度为了补足为4的整倍数会插入空字节。
                    IntPtr ptr = bmpData.Scan0;                         // 获取首地址
                    int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度
                    //byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存


                    //int posSrc = 0, posScan = 0;                        // dataValues和grayValues的索引
                    //for (int i = 0; i < height; i++)
                    //{
                    //    for (int j = 0; j < width; j++)
                    //    {
                    //        grayValues[posScan++] = data[posSrc++];
                    //    }
                    //    // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel
                    //    posScan += offset;
                    //}

                    Marshal.Copy(data, 0, ptr, scanBytes);    //在托管对象（数组）和非托管对象（IntPtr）之间进行内容的复制
                    bitmap.UnlockBits(bmpData);

                    // 修改生成位图的索引表，从伪彩修改为灰度
                    System.Drawing.Imaging.ColorPalette palette;

                    using (Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
                    {
                        palette = bmp.Palette;
                    }
                    for (int i = 0; i < 256; i++)
                    {
                        palette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    // 修改生成位图的索引表
                    bitmap.Palette = palette;
                    return bitmap;
                }

                else //color
                {
                    Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


                    //int stride = bmpData.Stride;  // 扫描线的宽度,比实际图片要大
                    //int offset = stride - width * 3;  // 显示宽度与扫描线宽度的间隙
                    IntPtr ptr = bmpData.Scan0;   // 获取bmpData的内存起始位置的指针
                    //int scanBytesLength = stride * height;  // 用stride宽度，表示这是内存区域的大小

                    Marshal.Copy(data, 0, ptr, width*height*3);
                    bitmap.UnlockBits(bmpData);  // 解锁内存区域

                    return bitmap;

                }
            }

            catch (Exception)
            {
                return null;
            }
        }

     


        public static Bitmap CreateBitmap(ImageData data)
        {
            if (data.Data == null)
            {
                return null;
            }

            try
            {
                if ("Mono8" == data.ImageFormat)
                {
                    Bitmap bitmap = new Bitmap(data.Width, data.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, data.Width, data.Height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);


                    int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数
                    IntPtr ptr = bmpData.Scan0;                         // 获取首地址
                    int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度
                    byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存

                    for (int i = 0; i < bmpData.Height; i++)
                    {
                        Marshal.Copy(data.Data, 0 + i * bmpData.Width, ptr + i * bmpData.Stride, bmpData.Width);
                    }
                 //   Marshal.Copy(data.Data, 0, ptr, scanBytes);
                    //int posSrc = 0, posScan = 0;                        // dataValues和grayValues的索引
                    //for (int i = 0; i < data.Height; i++)
                    //{
                    //    for (int j = 0; j < data.Width; j++)
                    //    {
                    //        grayValues[posScan++] = data.Data[posSrc++];
                    //    }
                    //    // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel
                    //    posScan += offset;
                    //}

                 //   Marshal.Copy(data.Data, 0, ptr, scanBytes);
                    bitmap.UnlockBits(bmpData);

                    // 修改生成位图的索引表，从伪彩修改为灰度
                    System.Drawing.Imaging.ColorPalette palette;

                    using (Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
                    {
                        palette = bmp.Palette;
                    }
                    for (int i = 0; i < 256; i++)
                    {
                        palette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    // 修改生成位图的索引表
                    bitmap.Palette = palette;
                    return bitmap;
                }

                else //color
                {
                    Bitmap bitmap = new Bitmap(data.Width, data.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, data.Width, data.Height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


                    //int stride = bmpData.Stride;  // 扫描线的宽度,比实际图片要大
                    //int offset = stride - data.Width * 3;  // 显示宽度与扫描线宽度的间隙
                    IntPtr ptr = bmpData.Scan0;   // 获取bmpData的内存起始位置的指针
                    //int scanBytesLength = stride * data.Height;  // 用stride宽度，表示这是内存区域的大小

                    Marshal.Copy(data.Data, 0, ptr, data.Width*data.Height*3);
                    bitmap.UnlockBits(bmpData);  // 解锁内存区域
                //    bitmap.Save(@"C:\Users\Administrator\Desktop\2.bmp",System.Drawing.Imaging.ImageFormat.Bmp);
                    return bitmap;

                }
            }

            catch (Exception)
            {
                return null;
            }
        }

      
    }
}
