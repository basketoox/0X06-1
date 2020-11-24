using Motion.Interfaces;

namespace Motion.Enginee
{
    /// <summary>
    ///     表示一个开关量传感器。
    /// </summary>
    public class Sensor : Automatic, ISensor<bool>
    {
        private readonly IoPoint _ioPoint;

        public Sensor(IoPoint ioPoint)
        {
            _ioPoint = ioPoint;
        }

        #region Implementation of ISensor<out bool>

        public bool Value { get { return _ioPoint.Value; } } 

        #endregion
    }
}