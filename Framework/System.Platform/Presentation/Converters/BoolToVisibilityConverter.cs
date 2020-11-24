using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace System.Platform.Presentation.Converters
{
    /// <summary>
    /// 将布尔值转换为可见性枚举值和从可见性枚举值转换的值转换器.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        private static readonly BoolToVisibilityConverter _defaultInstance = new BoolToVisibilityConverter();


        /// <summary>
        /// 获取此转换器的默认实例.
        /// </summary>
        public static BoolToVisibilityConverter Default { get { return _defaultInstance; } }


        /// <summary>
        /// 将布尔值转换为可见性枚举值.
        /// </summary>
        /// <param name="value">布尔值.</param>
        /// <param name="targetType">绑定目标属性的类型.这个参数将被忽略.</param>
        /// <param name="parameter">使用字符串'Invert获得反向结果(可见的和折叠的交换)。
        /// 如果需要默认行为，不要指定此参数.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>当布尔值为true时可见;否则崩溃.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (bool?)value;
            var invert = IsInvertParameterSet(parameter);

            if (invert)
            {
                return flag == true ? Visibility.Collapsed : Visibility.Visible;
            }
            return flag == true ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 将可见性枚举值转换为布尔值.
        /// </summary>
        /// <param name="value">可见性枚举值.</param>
        /// <param name="targetType">绑定目标属性的类型.这个参数将被忽略.</param>
        /// <param name="parameter">使用字符串'Invert获得反向结果(true和false交换).
        /// 如果需要默认行为，不要指定此参数.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>当可见的可见性枚举值时为真;否则假.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)value;
            var invert = IsInvertParameterSet(parameter);

            if (invert)
            {
                return visibility != Visibility.Visible;
            }
            return visibility == Visibility.Visible;
        }

        private static bool IsInvertParameterSet(object parameter)
        {
            var invertParameter = parameter as string;
            if (!string.IsNullOrEmpty(invertParameter) && string.Equals(invertParameter, "invert", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
