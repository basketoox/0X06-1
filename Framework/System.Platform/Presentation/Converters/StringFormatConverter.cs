using System.Globalization;
using System.Windows.Data;

namespace System.Platform.Presentation.Converters
{
    /// <summary>
    /// 将对象转换为格式化字符串的值转换器。格式规范通过ConverterParameter属性传递.
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public sealed class StringFormatConverter : IValueConverter
    {
        private static readonly StringFormatConverter _defaultInstance = new StringFormatConverter();

        /// <summary>
        /// 获取此转换器的默认实例.
        /// </summary>
        public static StringFormatConverter Default { get { return _defaultInstance; } }


        /// <summary>
        /// 将对象转换为格式化字符串.
        /// </summary>
        /// <param name="value">要转换的对象.</param>
        /// <param name="targetType">绑定目标属性的类型.这个参数将被忽略.</param>
        /// <param name="parameter">用于格式化对象的格式规范.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>格式化的字符串.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string ?? "{0}";

            return string.Format(culture, format, value);
        }

        /// <summary>
        /// 不支持此方法，并在调用该方法时抛出异常.
        /// </summary>
        /// <param name="value">绑定目标生成的值.</param>
        /// <param name="targetType">要转换的类型.</param>
        /// <param name="parameter">要使用的转换器参数.</param>
        /// <param name="culture">在转换器中使用的文化.</param>
        /// <returns>没什么，因为这个方法会抛出异常.</returns>
        /// <exception cref="NotSupportedException">在调用该方法时抛出此异常.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
