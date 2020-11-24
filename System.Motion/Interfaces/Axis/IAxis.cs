using System.Collections.Generic;
using System.Toolkit.Interfaces;
namespace Motion.Interfaces
{
    /// <summary>
    ///     表示一个运动轴。
    /// </summary>
    public interface IAxis : IAxis<string, VelocityCurve>
    {
    }

    /// <summary>
    ///     表示一个运动轴。
    /// </summary>
    /// <typeparam name="TMotionPathId">运动轴路径 ID。</typeparam>
    /// <typeparam name="TVelocityCurve">运行时参数。</typeparam>
    public interface IAxis<TMotionPathId, TVelocityCurve> : IAutomatic
    {
        /// <summary>
        ///     轴号。
        /// </summary>
        int NoId { get; set; }

        /// <summary>
        ///     当前 Absolute 位置。
        /// </summary>
        double CurrentPos { get; }

        /// <summary>
        ///     运转速度（转/秒）。
        /// </summary>
        double? Speed { get; set; }

        /// <summary>
        ///     是否已完成最后运动指令。
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        ///     运动路径。
        /// </summary>
        IList<MotionPath<TMotionPathId, TVelocityCurve>> MotionPaths { get; }

        /// <summary>
        ///     运动轴轴移动到指定的位置。
        /// </summary>
        /// <param name="value">将要移动到的位置。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        void MoveTo(double value, TVelocityCurve velocityCurve = default(TVelocityCurve));

        /// <summary>
        ///     运动轴相对移动到指定位置。
        /// </summary>
        /// <param name="value">要移动到的距离。</param>
        /// <param name="velocityCurve"></param>
        void MoveDelta(double value, TVelocityCurve velocityCurve = default(TVelocityCurve));

        /// <summary>
        ///     正向移动。
        /// </summary>
        void Postive();

        /// <summary>
        ///     反向移动。
        /// </summary>
        void Negative();

        /// <summary>
        ///     轴停止运动。
        /// </summary>
        /// <param name="velocityCurve"></param>
        void Stop(TVelocityCurve velocityCurve = default(TVelocityCurve));
    }
}