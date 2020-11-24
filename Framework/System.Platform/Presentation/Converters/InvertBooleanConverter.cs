using System.Globalization;
using System.Windows.Data;

namespace System.Platform.Presentation.Converters
{
    /// <summary>
    /// 转换布尔值的值转换器.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBooleanConverter : IValueConverter
    {
        private static readonly InvertBooleanConverter _defaultInstance = new InvertBooleanConverter();

        /// <summary>
        /// 获取此转换器的默认实例.
        /// </summary>
        public static InvertBooleanConverter Default { get { return _defaultInstance; } }


        /// <summary>
        /// 将布尔值转换为反转值.
        /// </summary>
        /// <param name="value">要反转的布尔值.</param>
        /// <param name="targetType">绑定目标属性的类型.这个参数将被忽略.</param>
        /// <param name="parameter">要使用的转换器参数.这个参数将被忽略.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>逆变器布尔值.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary>
        /// 将布尔值转换为反转值.
        /// </summary>
        /// <param name="value">要反转的布尔值.</param>
        /// <param name="targetType">要转换的类型.这个参数将被忽略.</param>
        /// <param name="parameter">要使用的转换器参数.这个参数将被忽略.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>逆变器布尔值.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
