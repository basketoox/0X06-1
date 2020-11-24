using Motion.Interfaces;

namespace Motion.Enginee
{
    /// <summary>
    ///     IoPoint 驱动的回转体
    /// </summary>
    public class Wheel : Automatic, IWheel
    {
        private readonly IoPoint _directIoPoint;
        private readonly IoPoint _reverseIoPoint;

        public Wheel(IoPoint directIoPoint)
        {
            _directIoPoint = directIoPoint;
        }

        public Wheel(IoPoint directIoPoint, IoPoint reverseIoPoint)
        {
            _directIoPoint = directIoPoint;
            _reverseIoPoint = reverseIoPoint;
        }

        #region Implementation of IWheel

        /// <summary>
        ///     正向
        /// </summary>
        public void Direct()
        {
            _directIoPoint.Value = true;
        }

        /// <summary>
        ///     反向
        /// </summary>
        public void Reverse()
        {
            if(_reverseIoPoint == null) return;
            _reverseIoPoint.Value = true;
        }


        /// <summary>
        ///     停止
        /// </summary>
        public void Stop()
        {
            _directIoPoint.Value = false;
            if (_reverseIoPoint == null) return;
            _reverseIoPoint.Value = false;
        }

        #endregion
    }
}