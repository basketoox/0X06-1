using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace System.Platform.Presentation
{
    /// <summary>
    /// 提供支持数据验证跟踪的方法和附加属性。
    /// 只有当参与数据验证的每个绑定都设置了NotifyOnValidationError=true时，这才有效.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// 标识启用IsEnabled的附加属性。此属性包含指示ValidationHelper是否启用的值.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled",
            typeof(bool), typeof(ValidationHelper), new FrameworkPropertyMetadata(false, IsEnabledChangedCallback));

        /// <summary>
        /// 标识有效的附加属性。此属性包含指示关联对象是否有效的值.
        /// </summary>
        /// <remarks>
        /// 这是一个只读属性。但是，允许为这个属性定义一个Mode=OneWayToSource的绑定.
        /// </remarks>
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid",
            typeof(bool), typeof(ValidationHelper), new FrameworkPropertyMetadata(true, IsValidChangedCallback));


        /// <summary>
        /// 获取一个值，该值指示是否启用ValidationHelper.
        /// </summary>
        /// <param name="obj">读取值的对象.</param>
        /// <returns>如果ValidationHelper是启用的，则为true.</returns>
        public static bool GetIsEnabled(DependencyObject obj) { return (bool)obj.GetValue(IsEnabledProperty); }

        /// <summary>
        /// 设置指示是否启用ValidationHelper的值.
        /// 默认值为“false”。不允许使用“false”调用此方法.
        /// </summary>
        /// <param name="obj">要将值设置为的对象.</param>
        /// <param name="value">如果设置为<c>true</c>，则为指定对象启用ValidationHelper.</param>
        /// <exception cref="ArgumentException">值不能设置为“false”.</exception>
        public static void SetIsEnabled(DependencyObject obj, bool value) { obj.SetValue(IsEnabledProperty, value); }

        /// <summary>
        /// 获取一个值，该值指示对象是否有效.
        /// </summary>
        /// <param name="obj">读取值的对象.</param>
        /// <returns>如果对象有效，则为true.</returns>
        public static bool GetIsValid(DependencyObject obj) { return (bool)obj.GetValue(IsValidProperty); }

        /// <summary>
        /// 不要调用此方法。仅供内部使用.
        /// </summary>
        /// <param name="obj">要将值设置为的对象.</param>
        /// <param name="value">如果设置为<c>true</c>，那么对象是有效的.</param>
        /// <exception cref="InvalidOperationException">当调用此方法时抛出.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetIsValid(DependencyObject obj, bool value)
        {
            throw new InvalidOperationException("The IsValid attached dependency property is readonly and must not be modified!");
        }

        internal static void InternalSetIsValid(DependencyObject obj, bool value)
        {
            if (GetIsValid(obj) != value)
            {
                // 只在值不同时才更改;否则会引发不必要的绑定更新.
                obj.SetCurrentValue(IsValidProperty, value);
            }
        }

        private static void IsEnabledChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!GetIsEnabled(obj))
            {
                throw new ArgumentException("The IsEnabled attached dependency property must not be set to 'false'.");
            }

            // 我们只创建了跟踪器的一个实例。因为它侦听错误事件，所以它保持活动.
            new ValidationTracker(obj);
        }

        private static void IsValidChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!GetIsEnabled(obj))
            {
                throw new InvalidOperationException("The IsValid attached property can only be used when the IsEnabled "
                    + "attached property is set to true.");
            }

            var binding = BindingOperations.GetBinding(obj, IsValidProperty);
            if (binding != null)
            {
                CheckBindingMode(binding.Mode);
                return;
            }

            var multiBinding = BindingOperations.GetMultiBinding(obj, IsValidProperty);
            if (multiBinding != null)
            {
                CheckBindingMode(multiBinding.Mode);
                return;
            }

            var priorityBinding = BindingOperations.GetPriorityBinding(obj, IsValidProperty);
            if (priorityBinding != null)
            {
                throw new InvalidOperationException("PriorityBinding is not supported for the IsValid attached dependency property!");
            }
        }

        private static void CheckBindingMode(BindingMode bindingMode)
        {
            if (bindingMode != BindingMode.OneWayToSource)
            {
                throw new InvalidOperationException("The binding mode of the IsValid attached dependency property must be 'Mode=OneWayToSource'!");
            }
        }
    }
}
