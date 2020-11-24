using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Platform.Presentation.Converters
{
    /// <summary>
    /// 将<see cref="ValidationError"/>集合转换为多行字符串错误消息的多值转换器。
    /// 当<see cref="ValidationError"/> 集合发生更改时，对第二个值使用绑定来更新目标。
    /// 在Count属性上设置第二个绑定的路径:binding path = "(Validation.Error).Count".
    /// </summary>
    /// 
    // Sample code (XAML) that shows how to use this converter:
    //
    // <Style.Triggers>
    //     <Trigger Property="Validation.HasError" Value="true">
    //         <Setter Property="ToolTip">
    //             <Setter.Value>
    //                 <MultiBinding Converter="{StaticResource ValidationErrorsConverter}">
    //                     <Binding Path="(Validation.Errors)" RelativeSource="{RelativeSource Self}"/>
    //                     <Binding Path="(Validation.Errors).Count" RelativeSource="{RelativeSource Self}"/>
    //                 </MultiBinding>
    //             </Setter.Value>
    //         </Setter>
    //     </Trigger>
    // </Style.Triggers>
    public sealed class ValidationErrorsConverter : IMultiValueConverter, IValueConverter
    {
        private static readonly ValidationErrorsConverter defaultInstance = new ValidationErrorsConverter();

        /// <summary>
        /// 获取此转换器的默认实例.
        /// </summary>
        public static ValidationErrorsConverter Default { get { return defaultInstance; } }


        /// <summary>
        /// 将<see cref="ValidationError"/>对象集合转换为多行错误消息字符串.
        /// </summary>
        /// <param name="values">第一个值是<see cref="ValidationError"/>对象的集合.</param>
        /// <param name="targetType">绑定目标属性的类型.这个参数将被忽略.</param>
        /// <param name="parameter">要使用的转换器参数.这个参数将被忽略.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>
        /// 当集合不包含错误时，多行错误消息或空字符串。如果第一个值参数<c>null</c>
        /// 或者不是IEnumerable&lt; validationerrorgt类型;这个方法返回 <see cref="DependencyProperty.UnsetValue"/>
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var validationErrors = values == null ? null : values.FirstOrDefault() as IEnumerable<ValidationError>;
            if (validationErrors != null)
            {
                return string.Join(Environment.NewLine, validationErrors.Select(x => x.ErrorContent.ToString()));
            }
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// 不支持此方法，并在调用该方法时抛出异常.
        /// </summary>
        /// <param name="value">绑定目标生成的值.</param>
        /// <param name="targetTypes">要转换的类型数组.</param>
        /// <param name="parameter">要使用的转换器参数.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>没什么，因为这个方法会抛出异常.</returns>
        /// <exception cref="NotSupportedException">在调用该方法时抛出此异常.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 过时:将<see cref="ValidationError"/>对象集合转换为多行错误消息字符串.
        /// </summary>
        /// <param name="value"><see cref="ValidationError"/>对象的集合.</param>
        /// <param name="targetType">绑定目标属性的类型。这个参数将被忽略.</param>
        /// <param name="parameter">要使用的转换器参数。这个参数将被忽略.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>
        /// 当集合不包含错误时，多行错误消息或空字符串。如果值参数<c>null</c>
        /// 或者不是IEnumerable&lt;validationerror&gt;类型;这个方法返回<参见 cref="DependencyProperty.UnsetValue"/>
        /// </returns>
        [Obsolete("Please use the Convert method that has as first parameter an array of values.")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(new[] { value }, targetType, parameter, culture);
        }

        /// <summary>
        /// 过时:不支持此方法，并在调用时抛出异常.
        /// </summary>
        /// <param name="value">绑定目标生成的值.</param>
        /// <param name="targetType">要转换的类型.</param>
        /// <param name="parameter">要使用的转换器参数.</param>
        /// <param name="culture">在转换器中使用的特定区信息.</param>
        /// <returns>没什么，因为这个方法会抛出异常.</returns>
        /// <exception cref="NotSupportedException">在调用该方法时抛出此异常.</exception>
        [Obsolete("ConvertBack is not supported.")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
