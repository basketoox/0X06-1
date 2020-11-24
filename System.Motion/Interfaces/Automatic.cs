using System.Toolkit.Interfaces;
namespace Motion.Interfaces
{
    /// <summary>
    ///     元器件（部件依赖项）
    /// </summary>
    public abstract class Automatic : IAutomatic
    {
        #region Implementation of IAutomatic

        public string Name { get; set; }

        public string Description { get; set; }

        #endregion
    }
}