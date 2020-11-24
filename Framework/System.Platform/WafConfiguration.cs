using System.Windows;
using System.ComponentModel;

namespace System.Platform
{
    /// <summary>
    /// WPF应用程序框架(WAF)的配置设置.
    /// </summary>
    public static class WafConfiguration
    {
        private static readonly bool _isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());

        /// <summary>
        /// 获取一个值，该值指示代码是否在设计模式下运行.
        /// </summary>
        /// <value><c>true</c>如果代码在设计模式下运行;否则, <c>false</c>.</value>
        public static bool IsInDesignMode { get { return _isInDesignMode; } }
    }
}
