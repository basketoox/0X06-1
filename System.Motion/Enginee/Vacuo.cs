using Motion.Interfaces;

namespace Motion.Enginee
{
    /// <summary>
    ///     真空
    /// </summary>
    public class Vacuo : Automatic, IVacuo
    {
        private readonly IoPoint _brokenIo;
        private readonly IoPoint _keepIo;
        private readonly IoPoint _suckIo;

        public Vacuo(IoPoint suckIo) : this(suckIo, null)
        {
        }

        public Vacuo(IoPoint suckIo, IoPoint brokenIo)
        {
            _suckIo = suckIo;
            _brokenIo = brokenIo;
            State = VacuoStates.None;
        }

        public Vacuo(IoPoint suckIo, IoPoint brokenIo, IoPoint keepIo)
        {
            _suckIo = suckIo;
            _brokenIo = brokenIo;
            _keepIo = keepIo;
            State = VacuoStates.None;
        }

        #region Implementation of IVacuo

        /// <summary>
        ///     状态
        /// </summary>
        /// <remarks>1:吸 -1：破 0:停</remarks>
        public VacuoStates State { get; private set; }

        /// <summary>
        ///     吸真空
        /// </summary>
        public void Suck()
        {
            if (State == VacuoStates.Sucked) return;

            if (_brokenIo == null)
            {
                _suckIo.Value = true;
            }
            else
            {
                _brokenIo.Value = false;
                _suckIo.Value = true;
            }
            if (_keepIo != null)
            {
                _keepIo.Value = false;
            }

            State = VacuoStates.Sucked;
        }

        /// <summary>
        ///     破真空
        /// </summary>
        public void Broken()
        {
            if (State == VacuoStates.Broken) return;

            if (_brokenIo == null)
            {
                _suckIo.Value = false;
            }
            else
            {
                _suckIo.Value = false;
                _brokenIo.Value = true;
            }
            if (_keepIo != null)
            {
                _keepIo.Value = true;
            }
            State = VacuoStates.Broken;
        }
 
        /// <summary>
        ///     停真空
        /// </summary>
        public void Stop()
        {
            //if (State == VacuoStates.None) return;

            if (_brokenIo == null)
            {
                _suckIo.Value = false;
            }
            else
            {
                _suckIo.Value = false;
                _brokenIo.Value = false;
            }
            if (_keepIo != null)
            {
                _keepIo.Value = true;
            }
            State = VacuoStates.None;
        }

        #endregion
    }
}