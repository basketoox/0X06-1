namespace Motion.Interfaces
{
    /// <summary>
    ///     表示一个信号量。
    /// </summary>
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface ISignal<T> : ISensor<T>, IResponser<T> where T : struct
    {
    }
}