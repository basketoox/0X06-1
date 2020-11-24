using System;
using System.Threading;
using System.Threading.Tasks;
using Motion.Interfaces;
using log4net;
using System.Collections.Generic;
using System.Toolkit.Interfaces;
using System.Windows.Forms;
namespace Motion.Enginee
{
    /// <summary>
    ///     支持测试线程的设备部件
    /// </summary>
    public abstract class StationPart : IThreadPart, IAutomatic
    {
        private CancellationTokenSource _cancelTokenSource;
        protected ILog log;
        public event DataReceiveCompleteEventHandler StationAppendTextReceive;
        protected External AlarmReset;
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public int step { get; set; }
        protected StationPart(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe, Type type)
        {
            ContinueEvent = new ManualResetEvent(true);
            StopEvent = new ManualResetEvent(false);
            AlarmReset = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
            log = LogManager.GetLogger(type);
        }

        /// <summary>
        ///     将指派给测试线程任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。
        /// </summary>
        protected CancellationToken CancelToken
        {
            get { return _cancelTokenSource.Token; }
        }

        /// <summary>
        ///     继续信号量。
        /// </summary>
        protected ManualResetEvent ContinueEvent { get; set; }

        /// <summary>
        ///     停止信号量。
        /// </summary>
        protected ManualResetEvent StopEvent { get; set; }

        private void Clean()
        {
            var ini = this as INeedClean;
            if (ini != null)
                ini.Clean();
            _cancelTokenSource = null;
        }

        #region Implementation of IThreadPart

        public RunStates RunState
        {
            get
            {
                if (_cancelTokenSource == null) return RunStates.Stop;
                return ContinueEvent.WaitOne(0) ? RunStates.Running : RunStates.Pause;
            }
        }

        /// <summary>
        ///     运行部件驱动线程。
        /// </summary>
        /// <param name="runningMode">运行模式。</param>
        public virtual void Run(RunningModes runningMode)
        {
            StopEvent.Reset();

            if (_cancelTokenSource == null)
            {
                _cancelTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Running(runningMode);
                        log.Debug($"{Name}部件启动。");
                    }
                    catch (OperationCanceledException ex2)
                    {
                        MessageBox.Show("yichang" + ex2.ToString());
                        //ignorl
                    }
                    catch (Exception ex)
                    {
                        log.Fatal("设备驱动程序异常", ex);
                        MessageBox.Show("yichang" + ex.ToString());
                    }
                }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning)
                .ContinueWith(task => Clean());
            }
        }

        /// <summary>
        ///     暂停任务线程
        /// </summary>
        public virtual void Pause()
        {
            if (_cancelTokenSource == null) return;
            ContinueEvent.Reset();
            log.Debug($"{Name}部件暂停。");
        }

        /// <summary>
        ///     唤醒任务线程
        /// </summary>
        public virtual void Resume()
        {
            if (_cancelTokenSource == null) return;
            ContinueEvent.Set();
            log.Debug($"{Name}部件唤醒。");
        }

        /// <summary>
        ///     停止任务线程
        /// </summary>
        public virtual void Stop()
        {
            if (_cancelTokenSource == null) return;
            if (_cancelTokenSource.Token.CanBeCanceled)
            {
                StopEvent.Set();
                _cancelTokenSource.Cancel();
                log.Debug($"{Name}部件停止。");
            }
        }
        protected void AppendText(string message)
        {
            StationAppendTextReceive?.Invoke(this, message);
        }
        /// <summary>
        ///     驱动部件运行。
        /// </summary>
        /// <param name="runningMode">运行模式。</param>
        public abstract void Running(RunningModes runningMode);
        public void AddPart()
        {
            Alarms = alarms();
            CylinderStatus = cylinderStatus();
        }
        protected abstract IList<ICylinderStatusJugger> cylinderStatus();
        protected abstract IList<Alarm> alarms();
        public IList<Alarm> Alarms { get; private set; }
        public IList<ICylinderStatusJugger> CylinderStatus { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }
        #endregion
    }
}