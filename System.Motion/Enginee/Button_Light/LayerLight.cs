using System;
using Motion.Interfaces;
using System.Diagnostics;
namespace Motion.Enginee
{
    /// <summary>
    ///     多层指示灯
    /// </summary>
    public class LayerLight : Automatic, IRefreshing
    {
        private readonly Stopwatch _watch = new Stopwatch();
        private bool intervalSign;

        public LayerLight(IoPoint greenIo, IoPoint yellowIo, IoPoint redIo, IoPoint speakerIo)
        {
            GreenLamp = greenIo;
            YellowLamp = yellowIo;
            RedLamp = redIo;
            Speeker = speakerIo;
        }

        public IoPoint GreenLamp { get; set; }
        public IoPoint YellowLamp { get; set; }
        public IoPoint RedLamp { get; set; }
        public IoPoint Speeker { get; set; }
        public bool VoiceClosed { get; set; }
        public MachineStatus Status { private get; set; }

        public void Refreshing()
        {
            #region 间隔

            _watch.Stop();
            if (intervalSign)
            {
                if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                {
                    intervalSign = false;
                    _watch.Restart();
                }
            }
            else
            {
                if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                {
                    intervalSign = true;
                    _watch.Restart();
                }
            }
            _watch.Start();

            #endregion

            //红灯
            if (Status == MachineStatus.设备未准备好 ||
                (intervalSign && (Status == MachineStatus.设备报警中 || Status == MachineStatus.设备急停已按下)))
            {
                RedLamp.Value = true;
            }
            else
            {
                RedLamp.Value = false;
            }

            //蜂鸣器
            if (RedLamp.Value && !VoiceClosed && Status != MachineStatus.设备未准备好)
            {
                Speeker.Value = true;
            }
            else
            {
                Speeker.Value = false;
            }

            //黄灯
            if (!RedLamp.Value &&
                (((Status == MachineStatus.设备停止中 || Status == MachineStatus.设备暂停中 
                || Status == MachineStatus.设备复位中) && intervalSign)
                || Status == MachineStatus.设备准备好))
            {
                YellowLamp.Value = true;
            }
            else
            {
                YellowLamp.Value = false;
            }

            //绿灯
            if (!RedLamp.Value && Status == MachineStatus.设备运行中)
            {
                GreenLamp.Value = true;
            }
            else
            {
                GreenLamp.Value = false;
            }
        }
    }
}