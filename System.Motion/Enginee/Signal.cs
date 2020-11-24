using Motion.Interfaces;

namespace Motion.Enginee
{
    /// <summary>
    ///     表示一个信号量。
    /// </summary>
    public class Signal : Automatic,ISignal<bool>
    {
        private readonly IoPoint _signalPoint;

        public Signal(IoPoint signalPoint)
        {
            _signalPoint = signalPoint;
        }

        #region Implementation of ISensor<out bool> and IResponser<bool>

        public bool Value
        {
            get { return _signalPoint.Value; }
            set { _signalPoint.Value = value; }
        }

        #endregion
    }
}