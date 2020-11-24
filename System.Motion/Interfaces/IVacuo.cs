using System.Toolkit.Interfaces;
namespace Motion.Interfaces
{
    /// <summary>
    ///     真空
    /// </summary>
    public interface IVacuo : IAutomatic
    {
        /// <summary>
        ///     状态
        /// </summary>
        VacuoStates State { get; }

        /// <summary>
        ///     吸真空
        /// </summary>
        void Suck();

        /// <summary>
        ///     破真空
        /// </summary>
        void Broken();

        /// <summary>
        ///     停真空
        /// </summary>
        void Stop();
    }

    public enum VacuoStates
    {
        /// <summary>
        ///     停真空
        /// </summary>
        None,

        /// <summary>
        ///     吸真空
        /// </summary>
        Sucked,

        /// <summary>
        ///     破真空
        /// </summary>
        Broken
    }
}