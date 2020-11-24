using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace System.Toolkit.Attributes
{
    /// <summary>
    /// 属性本地化.
    /// </summary>
    [AttributeUsage(AttributeTargets.All,Inherited = false,AllowMultiple = true)]
    public sealed class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        #region Public methods.
        // ------------------------------------------------------------------

        /// <summary>
        /// 初始化<see cref="LocalizableDescriptionAttribute"/>类的新实例.
        /// </summary>
        /// <param name="description">描述.</param>
        /// <param name="resourcesType">资源类型.</param>
        public LocalizableDescriptionAttribute(string description,Type resourcesType) : base(description)
        {
            _resourcesType = resourcesType;
        }

        // ------------------------------------------------------------------
        #endregion

        #region Public properties.
        // ------------------------------------------------------------------

        /// <summary>
        /// 从资源中获取字符串值.
        /// </summary>
        /// <value></value>
        /// <returns>存储在此特性中的描述.</returns>
        public override string Description
        {
            get
            {
                if (!_isLocalized)
                {
                    var resMan = _resourcesType.InvokeMember(
                         @"ResourceManager",
                         BindingFlags.GetProperty | BindingFlags.Static |
                         BindingFlags.Public | BindingFlags.NonPublic,
                         null,
                         null,
                         new object[] { }) as ResourceManager;

                    var culture = _resourcesType.InvokeMember(
                         @"Culture",
                         BindingFlags.GetProperty | BindingFlags.Static |
                         BindingFlags.Public | BindingFlags.NonPublic,
                         null,
                         null,
                         new object[] { }) as CultureInfo;

                    _isLocalized = true;

                    if (resMan != null)
                    {
                        DescriptionValue =
                             resMan.GetString(DescriptionValue, culture);
                    }
                }

                return DescriptionValue;
            }
        }

        // ------------------------------------------------------------------
        #endregion

        #region Private variables.
        // ------------------------------------------------------------------

        private readonly Type _resourcesType;
        private bool _isLocalized;

        // ------------------------------------------------------------------
        #endregion      
    }
}
