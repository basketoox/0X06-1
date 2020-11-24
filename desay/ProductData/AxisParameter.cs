using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motion.AdlinkAps;
using Motion.Interfaces;
namespace desay.ProductData
{
    [Serializable]
    public class AxisParameter
    {
        [NonSerialized]
        public static AxisParameter Instance = new AxisParameter();

        //最大速度
        public double LXvelocityMax { get; set; } = 600.000;
        public double LYvelocityMax { get; set; } = 800.000;
        public double LZvelocityMax { get; set; } = 600.000;
        public double RXvelocityMax { get; set; } = 600.000;
        public double RYvelocityMax { get; set; } = 800.000;
        public double RZvelocityMax { get; set; } = 600.000;

        //速度参数
        public VelocityCurve LXspeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve LYspeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve LZspeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve CleanPathSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };

        public VelocityCurve RXspeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve RYspeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve RZspeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve GluePathSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };

        //回零速度
        public VelocityCurve LXhomeSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve LYhomeSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve LZhomeSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };

        public VelocityCurve RXhomeSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve RYhomeSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };
        public VelocityCurve RZhomeSpeed { get; set; } = new VelocityCurve()
        { Strvel = 5.00, Maxvel = 20.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T };

        //传动参数
        public TransmissionParams LXTransParams { get; set; } = new TransmissionParams() { Lead = 10, SubDivisionNum = 10000 };
        public TransmissionParams LYTransParams { get; set; } = new TransmissionParams() { Lead = 10, SubDivisionNum = 10000 };
        public TransmissionParams LZTransParams { get; set; } = new TransmissionParams() { Lead = 10, SubDivisionNum = 10000 };
        public TransmissionParams RXTransParams { get; set; } = new TransmissionParams() { Lead = 10, SubDivisionNum = 10000 };
        public TransmissionParams RYTransParams { get; set; } = new TransmissionParams() { Lead = 10, SubDivisionNum = 10000 };
        public TransmissionParams RZTransParams { get; set; } = new TransmissionParams() { Lead = 10, SubDivisionNum = 10000 };
    }
}
