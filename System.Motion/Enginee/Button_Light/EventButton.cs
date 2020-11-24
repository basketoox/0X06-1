using System;
using Motion.Interfaces;
// ReSharper disable DelegateSubtraction
namespace Motion.Enginee
{
    /// <summary>
    ///     带指示灯的按钮
    /// </summary>
    public class EventButton : Automatic
    {
        //private readonly IoPoint _pressedIo;

        private bool _isPressed;
        private EventHandler _pressed;
        private EventHandler _released;

        public EventButton(IoPoint pressedIo)
        {
            PressedIO = pressedIo;
        }
        public IoPoint PressedIO { get; }
        /// <summary>
        ///     Good Red 按钮是否按下
        /// </summary>
        public bool IsPressed
        {
            get
            {
                return _isPressed;
            }
            set
            {
                if (_isPressed != value)
                {
                    _isPressed = value;
                    if (value)
                    {
                        OnPressed();
                    }
                    else
                    {
                        OnReleased();
                    }
                }
            }
        }

        public event EventHandler Released
        {
            add { _released += value; }
            remove { _released -= value; }
        }

        public event EventHandler Pressed
        {
            add { _pressed += value; }
            remove { _pressed -= value; }
        }

        protected virtual void OnPressed()
        {
            if(_pressed == null) return;
            _pressed.Invoke(this, EventArgs.Empty);
        }

        private void OnReleased()
        {
            if (_released == null) return;
            _released.Invoke(this, EventArgs.Empty);
        }
    }
}