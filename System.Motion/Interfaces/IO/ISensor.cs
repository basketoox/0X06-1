using System.Toolkit.Interfaces;
namespace Motion.Interfaces
{
    /// <summary>
    ///     表示一个传感器。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISensor<out T> : IAutomatic where T : struct
    {
        T Value { get; }
    }
}