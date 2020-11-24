using System;

namespace Motion.Interfaces
{
    /// <summary>
    ///     IO 端口类型
    /// </summary>
    [Flags]
    public enum IoModes
    {
        /// <summary>
        ///     只读端口
        /// </summary>
        Senser = 0x1,

        /// <summary>
        ///     只写端口
        /// </summary>
        Responser = 0x2,

        /// <summary>
        ///     读写端口
        /// </summary>
        Signal = 0x3
    }
}