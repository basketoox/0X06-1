using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Platform.Foundation
{
    /// <summary>
    ///  定义模型的基类.
    /// </summary>
    [Serializable]
    public abstract class Model : INotifyPropertyChanged
    {
        /// <summary>
        /// 提供对派生类的PropertyChanged事件处理程序的访问.
        /// </summary>
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get { return PropertyChanged; }
        }

        /// <summary>
        /// 当属性值更改时发生.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 验证此视图模型中是否存在属性名.
        /// 可以在使用属性之前调用此方法，例如在调用RaisePropertyChanged之前.
        /// 当更改属性名但遗漏了某些位置时，它可以避免错误.
        /// </summary>
        /// <remarks>此方法仅在DEBUG模式下有效.</remarks>
        /// <param name="propertyName">
        ///  要检查的属性的名称.
        /// </param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            var myType = GetType();

            if (!string.IsNullOrEmpty(propertyName) && myType.GetProperty(propertyName) == null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                var descriptor = this as ICustomTypeDescriptor;

                if (descriptor != null)
                {
                    if (descriptor.GetProperties()
                        .Cast<PropertyDescriptor>()
                        .Any(property => property.Name == propertyName))
                    {
                        return;
                    }
                }
                throw new ArgumentException(@"Property not found", propertyName);
            }
        }

        /// <summary>
        ///  使用指定的值设置属性。如果值不等于字段，则设置字段，引发PropertyChanged事件并返回true.
        /// </summary>
        /// <typeparam name="T">属性类型.</typeparam>
        /// <param name="field">引用属性的后端字段.</param>
        /// <param name="value">属性的新值.</param>
        /// <param name="propertyName">
        /// 属性名。这个可选参数可以跳过，因为编译器可以自动创建它.
        /// </param>
        /// <returns>如果值已经更改，则为True;如果新旧值相等，则为false.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 引发<see cref="E:PropertyChanged" />事件.
        /// </summary>
        /// <param name="propertyName">
        /// 已更改的属性的属性名。
        /// 这个可选参数可以跳过，因为编译器可以自动创建它.
        /// </param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 引发<see cref="E:PropertyChanged" />事件.
        /// </summary>
        /// <param name="e"><see cref="System.ComponentModel.PropertyChangedEventArgs" />包含事件数据的实例.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// 如果需要，引发PropertyChanged事件.
        /// </summary>
        /// <typeparam name="T">
        ///  改变的属性的类型.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///  一个表达式，用于标识更改的属性.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [SuppressMessage("Microsoft.Design", "CA1006:GenericMethodsShouldProvideTypeParameter",
            Justification = "This syntax is more convenient than other alternatives.")]
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var propertyName = GetPropertyName(propertyExpression);
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 从表达式中提取属性名.
        /// </summary>
        /// <typeparam name="T">属性的类型.</typeparam>
        /// <param name="propertyExpression">返回属性名称的表达式.</param>
        /// <returns>表达式返回的属性的名称.</returns>
        /// <exception cref="ArgumentNullException">如果表达式为空.</exception>
        /// <exception cref="ArgumentException">如果表达式不表示属性.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "This syntax is more convenient than the alternatives."),
         SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
             Justification = "This syntax is more convenient than the alternatives.")]
        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException(@"Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException(@"Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }
    }
}