using System.Collections.Generic;
using System.ComponentModel;

namespace System.Toolkit.Interfaces
{
    /// <summary>
    ///     零部件
    /// </summary>
    public interface IPart : IEnumerable<IPart>, INotifyPropertyChanged
    {
        /// <summary>
        ///     父部件
        /// </summary>
        IPart Parent { get; }

        /// <summary>
        ///     名字
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     元器件索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IPart this[string name] { get; }

        /// <summary>
        ///     元器件枚举数
        /// </summary>
        IEnumerable<IAutomatic> Automatics { get; }
    }
}