using System;
using System.Collections.Generic;
using System.Linq;
using System.ToolKit;
using Motion.Interfaces;
using Motion.AdlinkAps;
namespace Motion.Enginee
{
    public class AxisInterpolation
    {
        private readonly Axis _axis1, _axis2;
        private readonly ApsController ApsController;
        public AxisInterpolation(ApsController apsController,Axis axis1,Axis axis2)
        {
            ApsController = apsController;
            _axis1 = axis1;
            _axis2 = axis2;
        }
        /// <summary>
        /// 两轴做插补相对移动
        /// </summary>
        /// <param name="end">终点坐标</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveLine2Relative(Point<int> end, VelocityCurve velocityCurveParams)
        {
            ApsController.MoveLine2Relative(_axis1.NoId, _axis2.NoId, end.X, end.Y, velocityCurveParams);
        }
        /// <summary>
        ///     两轴直线插补绝对移动
        /// </summary>
        /// <param name="end">终点坐标</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveLine2Absolute(Point<int> end, VelocityCurve velocityCurveParams)
        {
            ApsController.MoveLine2Absolute(_axis1.NoId, _axis2.NoId, end.X, end.Y, velocityCurveParams);
        }
        /// <summary>
        /// 两轴直线插补相对轨迹移动
        /// </summary>
        /// <param name="end">终点坐标</param>
        /// <param name="velocityCurveParams">速度参数</param>
        /// <param name="Option">位集指定选项，该选项可以启用指定的参数和函数。</param>
        public void MoveLine2Relative(Point<double> end, VelocityCurve velocityCurveParams)
        {
            ApsController.MoveLine2(_axis1.NoId, _axis2.NoId, end.X, end.Y, velocityCurveParams, 
                (Int32)APS_Define.OPT_RELATIVE | (Int32)APS_Define.ITP_OPT_BUFFERED);
        }
        /// <summary>
        /// 两轴直线插补绝对轨迹移动
        /// </summary>
        /// <param name="end">终点坐标</param>
        /// <param name="velocityCurveParams">速度参数</param>
        /// <param name="Option">位集指定选项，该选项可以启用指定的参数和函数。</param>
        public void MoveLine2Absolute(Point<double> end, VelocityCurve velocityCurveParams)
        {
            ApsController.MoveLine2(_axis1.NoId, _axis2.NoId, end.X, end.Y, velocityCurveParams,
                (Int32)APS_Define.OPT_ABSOLUTE | (Int32)APS_Define.ITP_OPT_BUFFERED);
        }
        /// <summary>
        /// 两轴圆弧插补相对移动
        /// </summary>
        /// <param name="center">圆心坐标</param>
        /// <param name="Angle">角度</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Relative(Point<int> center, int Angle, VelocityCurve velocityCurveParams)
        {
            ApsController.MoveArc2Relative(_axis1.NoId, _axis2.NoId, center.X, center.Y, Angle, velocityCurveParams);
        }

        /// <summary>
        /// 两轴圆弧插补绝对移动
        /// </summary>
        /// <param name="center">圆心坐标</param>
        /// <param name="Angle">角度</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Absolute(Point<int> center, int Angle, VelocityCurve velocityCurveParams)
        {
            ApsController.MoveArc2Absolute(_axis1.NoId, _axis2.NoId, center.X, center.Y, Angle, velocityCurveParams);
        }
        /// <summary>
        /// 两轴圆弧插补相对轨迹移动
        /// </summary>
        /// <param name="first">起点</param>
        /// <param name="second">经过点</param>
        /// <param name="end">终点</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Relative(Point<double> first, Point<double> second, Point<double> end, VelocityCurve velocityCurveParams)
        {
            ArcParam<double> Arc = Calculate.ArcCalculate(first, second, end);
            ApsController.MoveArc2(_axis1.NoId,_axis2.NoId,Arc.X,Arc.Y, end.X, end.Y, (short)Arc.DIR,velocityCurveParams,
                (Int32)APS_Define.OPT_RELATIVE | (Int32)APS_Define.ITP_OPT_BUFFERED);
        }
        /// <summary>
        /// 两轴圆弧插补绝对轨迹移动
        /// </summary>
        /// <param name="first">起点</param>
        /// <param name="second">经过点</param>
        /// <param name="end">终点</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Absolute(Point<double> first, Point<double> second, Point<double> end, VelocityCurve velocityCurveParams)
        {
            ArcParam<double> Arc = Calculate.ArcCalculate(first, second, end);
            ApsController.MoveArc2(_axis1.NoId, _axis2.NoId, Arc.X, Arc.Y, end.X, end.Y, (short)Arc.DIR, velocityCurveParams,
                (Int32)APS_Define.OPT_ABSOLUTE | (Int32)APS_Define.ITP_OPT_BUFFERED);
        }
    }
}
