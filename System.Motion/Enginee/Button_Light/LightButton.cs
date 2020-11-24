using Motion.Interfaces;

namespace Motion.Enginee
{
    /// <summary>
    ///     带指示灯的按钮
    /// </summary>
    public class LightButton : EventButton
    {
        private readonly IoPoint _lightIo;
        private bool _isLight;

        public LightButton(IoPoint pressedIo, IoPoint lightIo) : base(pressedIo)
        {
            _lightIo = lightIo;
        }

        public bool IsLight
        {
            get { return _isLight; }
            set
            {
                _lightIo.Value = value;
                _isLight = value;
            }
        }
    }
}