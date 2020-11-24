using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace desay
{
   public interface ICamera
    {
        ImageData OutputImageData { get; }
        string SerialNumber { get; }
        int TimeOut { get; set; }
        string Describe { get; }
        double ExposureTime { get; set; }
        double Gain { get; set; }
        double FrameRate { get; }
        double FrameRateLoss { get; }
        TriggerSource TriggerSource { get; set; }
        TriggerActivation TriggerAction { get; set; }
        bool IsOpen {get;}

        bool Initialize(string serialNumber);
        void Acquire();
        void Close();
        event  GrabCompleted Ran;
    }
   //public delegate void GrabCompleted(byte[] bytes, int width, int height, string format);
   public delegate void GrabCompleted(ImageData imageData);
   public class ImageData
   {
       public byte[] Data { get; set; }
       public int Width { get; set; }
       public int Height { get; set; }
       public string ImageFormat { get; set; }
   }


   public enum TriggerSource
   {
       Software,
       Hardware,
       All,
       Off
   }
   public enum TriggerActivation
   {
        RisingEdge,
        FallingEdge,
        None
   }

}
