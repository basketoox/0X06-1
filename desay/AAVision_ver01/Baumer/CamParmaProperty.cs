using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace desay
{
    class CamParmaProperty
    {
        private BaumerCamera _ACQfifotool;
        public BaumerCamera ACQfifotool
        {
            get { return _ACQfifotool; }
            set { _ACQfifotool = value; }
        }

        private double _ExposureTime;
        public double ExposureTime
        {
            get
            {             
                return _ACQfifotool.ExposureTime;
            }
            set { _ACQfifotool.ExposureTime = value; }
        }

        private double _Gain;
        public double Gain
        {
            get { return _ACQfifotool.Gain; }
            set { _ACQfifotool.Gain = value; }
        }

        private string _CamSN;
        public string CamSN
        {
            get { return _ACQfifotool.SerialNumber; }
            
        }

        private int _TimeOut;
        public int TimeOut
        {
            get { return _ACQfifotool.TimeOut; }
            set
            {
                _ACQfifotool.TimeOut = value;
                
            }
           
        }

        
    }
}
