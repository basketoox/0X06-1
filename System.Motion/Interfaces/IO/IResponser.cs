using System.Toolkit.Interfaces;
namespace Motion.Interfaces
{
    /// <summary>
    ///     表示一个响应器。
    /// </summary>
    public interface IResponser<T> : IAutomatic where T : struct
    {
        bool Value { set; }
    }
}