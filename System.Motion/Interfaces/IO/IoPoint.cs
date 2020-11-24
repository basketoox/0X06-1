using System;
using System.Diagnostics;

namespace Motion.Interfaces
{
    /// <summary>
    ///     表示一个通讯开关量。
    /// </summary>
    public class IoPoint : Automatic
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="controller">通讯 IO 控制器。</param>
        /// <param name="boardNo"></param>
        /// <param name="portNo"></param>
        /// <param name="ioMode">通讯模式<see cref="IoModes" /></param>
        public IoPoint(ISwitchController controller, int boardNo,int portNo, IoModes ioMode)
        {
            Controller = controller;
            BoardNo = boardNo;
            PortNo = portNo;
            IoMode = ioMode;
        }

        /// <summary>
        ///     读写控制器
        /// </summary>
        protected ISwitchController Controller { get; private set; }

        /// <summary>
        ///     板卡号
        /// </summary>
        public int BoardNo { get; set; }
        /// <summary>
        ///     通道序号
        /// </summary>
        public int PortNo { get; set; }

        /// <summary>
        ///     通讯模式
        /// </summary>
        public IoModes IoMode { get; set; }

        /// <summary>
        ///     信号量值
        /// </summary>
        public bool Value
        {
            get
            {
                //if ((IoMode & IoModes.Responser) != 0)
                //    VerifyIoPoint(IoModes.Responser);
                //else
                    VerifyIoPoint(IoModes.Senser);
                return Controller.Read(this);
            }
            set
            {
                VerifyIoPoint(IoModes.Responser);
                Controller.Write(this, value);
            }
        }

        #region Overrides of Object

        public override string ToString()
        {
            return string.Format("IoPoint[{0},{1}]", BoardNo, PortNo);
        }

        #endregion

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyIoPoint(IoModes ioMode)
        {
            if ((IoMode & ioMode) == 0)
                throw new InvalidOperationException(String.Format("非法{0}操作{1}开关量：Board:{2} Index:{3}", ioMode, IoMode, BoardNo, PortNo));
        }
    }
}