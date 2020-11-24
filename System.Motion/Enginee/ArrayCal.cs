using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Toolkit;

namespace Motion.Enginee
{
    /// <summary>
    /// 偏差及坐标
    /// </summary>
    public struct PositionOffset
    {
        /// <summary>
        /// 基准点
        /// </summary>
        public Point3D<double> BasePos;
        /// <summary>
        /// 列方向上x偏差 Column x dir offset
        /// </summary>
        public double Cxf;
        /// <summary>
        /// 列方向上y偏差 Column y dir offset
        /// </summary>
        public double Cyf;
        /// <summary>
        /// 行方向上x偏差 Row x dir offset
        /// </summary>
        public double Rxf;
        /// <summary>
        /// 行方向上y偏差 Row y dir offset
        /// </summary>
        public double Ryf;
    }
}
