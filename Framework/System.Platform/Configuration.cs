using System.Windows;
using System.ComponentModel;

namespace System.Platform
{
    /// <summary>
    /// Configuration settings for the WPF Application Framework (WAF).
    /// </summary>
    public static class Configuration
    {
        private static readonly bool _isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());

        /// <summary>
        /// Gets a value indicating whether the code is running in design mode.
        /// </summary>
        /// <value><c>true</c> if the code is running in design mode; otherwise, <c>false</c>.</value>
        public static bool IsInDesignMode { get { return _isInDesignMode; } }
    }
}
