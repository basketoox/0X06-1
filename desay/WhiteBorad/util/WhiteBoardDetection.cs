using DTCCM2_DLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DxDemoDtccm2.DesayView
{
    public class WhiteBoardDetection
    {
        ///DT反馈对比常量
        public const int DT_ERROR_FAILED = 0;
        public const int DT_ERROR_OK = 1;
        public const int DT_ERROR_IN_PROCESS = 2;
        public const int DT_ERROR_WAIT = 3;
        public const int DT_ERROR_BUSY = 4;

        /* 同步并行接口特性的位定义 */
        public const int PARA_PCLK_RVS = (1 << 3);  // PCLK取反
        public const int PARA_VSYNC_RVS = (1 << 4); // VSYNC取反
        public const int PARA_HSYNC_RVS = (1 << 5); // HSYNC取反
        public const int PARA_AUTO_POL = (1 << 6);  // VSYNC,HSYNC极性自动识别

        /* SENSOR输出图像类型定义(SensorTab::type的取值定义) */
        public const int D_RAW10 = 0x00;
        public const int D_RAW8 = 0x01;
        public const int D_MIPI_RAW8 = 0x01;
        public const int D_YUV = 0x02;
        public const int D_RAW16 = 0x03;
        public const int D_MIPI_RAW16 = 0x03;
        public const int D_RGB565 = 0x04;
        public const int D_YUV_SPI = 0x05;
        public const int D_MIPI_RAW10 = 0x06;
        public const int D_MIPI_RAW12 = 0x07;
        public const int D_RAW12 = 0x07;
        public const int D_YUV_MTK_S = 0x08;
        public const int D_YUV_10 = 0x09;
        public const int D_YUV_12 = 0x0a;

        public struct _Pin
        {
            public int m_pin1;
            public int m_pin2;
            public int m_pin3;
            public int m_pin4;
            public int m_pin5;
            public int m_pin6;
            public int m_pin7;
            public int m_pin8;
            public int m_pin9;
            public int m_pin10;
            public int m_pin11;
            public int m_pin12;
            public int m_pin13;
            public int m_pin14;
            public int m_pin15;
            public int m_pin16;
            public int m_pin17;
            public int m_pin18;
            public int m_pin19;
            public int m_pin20;
            public int m_pin21;
            public int m_pin22;
            public int m_pin23;
            public int m_pin24;
            public int m_pin25;
            public int m_pin26;
        }

        struct _DarkResult
        {
            int iCountSum;                  //亮点总和
        };
        struct _DarkCriterion
        {
            int iWDefect;                   //白坏点亮度差 （当前像素亮度-平均像素亮度）
            int iWDefectMaxNumInDusty;      //允许的连续白坏点个数 
            bool isCirclePicture;           //成像是否是圆形，若为false，则dCircleRadius无效
            int dCircleRadius;              //计算亮点区域的圆半径
            bool dDebugSavepicture;         //调试功能,是否保存图片用于异常分析
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct _tagDefectAndStainInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public int[] Center_x;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public int[] Center_y;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public int[] HorizontalSize;            //污坏点长宽+8
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public int[] VerticalSize;
        };
        public struct _tagDefectResult
        {
            public int DarkCount;                  //暗点个数
            public int BrightCount;                //亮点个数
            public int CountSum;                  //坏点总和
        };
        public struct _StrainCriterion
        {
            public int fLuxUpper;                  //深污点亮度差 （平均像素亮度-当前像素亮度）* 100【深污点阈值】
            public int fLuxLower;                  //浅污点亮度差 （平均像素亮度-当前像素亮度）* 100【浅污点阈值】
            public int lowStainNumInArea;          //联通的像素超过这个个数即认为是污点群           【浅污点群定义】
            public int highStainNumInArea;         //污点群的深污点个数超过该标准即认为是深污点群   【深污点群定义】
            public int lowStainAreaNum;            //允许的浅污点群个数                             【浅脏污个数】
            public int highStainAreaNum;           //允许的深污点群个数                             【深脏污个数】
            public int iJump;                      //图像压缩的比例
            public bool isCirclePicture;           //成像是否是圆形，若为false，则sCircleOffset和BinaryThreshold无效
            public int sCircleOffset;              //成像圆往内缩的像素
            public int BinaryThreshold;            //计算圆半径时二值化阈值
            public bool sDebugSavepicture;       //调试功能,是否保存图片用于异常分析
        };

        public struct _tagDefectCriterion
        {
            public float fDefect;                  //黑坏点亮度差 （（平均像素亮度-当前像素亮度）/平均像素亮度 ）   【参数界面的暗点阈值】（由于C++界面代码查询对应不十分明确，特写下注释）
            public int iWDefect;                   //白坏点亮度差 （当前像素亮度-平均像素亮度）                     【参数界面的亮点阈值】
            public int fDefectMaxNumInDusty;       //允许的连续黑坏点个数                                           【参数界面的连续暗个数】
            public int iWDefectMaxNumInDusty;      //允许的连续白坏点个数                                           【参数界面的连续亮个数】
            public bool isCirclePicture;           //成像是否是圆形，若为false，则dCircleOffset和BinaryThreshold无效
            public int dCircleOffset;              //成像圆往内缩的像素                                             【参数界面的成像圆内缩】
            public int BinaryThreshold;            //计算圆半径时二值化阈值                                         【binary threshold】
            public bool dDebugSavepicture;         //调试功能,是否保存图片用于异常分析
        };

        public struct _SStainDefectConfig
        {
            public _StrainCriterion m_sStrainCriterion;
            public _tagDefectCriterion m_sDefectCriterion;
            public int nBadPixel_Spec;
            public int nHotPixel_Spec;
            public int nLowBlemish_Spec;
            public int nDeepBlemish_Spec;
        }

        public _SStainDefectConfig m_sStainDefectConfig = new _SStainDefectConfig();

        public _tagDefectAndStainInfo m_tagDefectAndStainInfo = new _tagDefectAndStainInfo();


        public _Pin pin = new _Pin();

        public _tagDefectResult m_tagDefectResult = new _tagDefectResult();


        //坏点测试接口
        /*
        Description: 坏点测试接口
        input:    pTmpImgBuf---------------> 输入的图像数据, 图像的亮度(Y分量)
        nimgWidth ---------------> 图像宽度
        nimgHeight ---------------> 图像高度
        pDefectCriterion---------------> 坏点标准

         output: pDefectInfo---------------> 坏点群的坐标
                 pDefResult --------------->坏点输出结果
        return : 0 :图像有坏点
                 1 :图像无坏点
          */

        [DllImport("DesayAlgorithmLibrary.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Desay_DefectTest")]

        public static extern int Desay_DefectTest(byte[] pTmpImgBuf, int nImageWidth, int nImageHeight, _tagDefectCriterion pDefectCriterion, IntPtr pDefectInfo, IntPtr pDefResult);



        //暗环境下测亮点
        [DllImport("DesayAlgorithmLibrary.dll")]
        public static extern int Desay_DarkTest(IntPtr pTmpImgBuf, int nImageWidth, int nImageHeight, _tagDefectCriterion pDefectCriterion, IntPtr pDefectInfo, IntPtr pDefResult);

        //污点测试接口
        /*
        Description: 坏点测试接口
         input:        pdata---------------> 输入的图像数据, 图像的亮度(Y分量)
                    m_Width ---------------> 图像宽度
                   m_Height ---------------> 图像高度
            pStrainCriterion---------------> 污点标准

         output: pStainInfo---------------> 污点群的坐标
              pStainResult --------------->污点输出结果
        return : 0 :图像有污点
                 1 :图像无污点
          */
        [DllImport("DesayAlgorithmLibrary.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Desay_StainTest")]
        public static extern int Desay_StainTest(byte[] pTmpImgBuf, int nImageWidth, int nImageHeight, _StrainCriterion pStrainCriterion, IntPtr pDefectInfo, IntPtr pDefResult);
        
    }
}

