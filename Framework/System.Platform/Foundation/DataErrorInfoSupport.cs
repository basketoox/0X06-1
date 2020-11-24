using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 这个类为<see cref="IDataErrorInfo"/>接口提供一个实现
    /// 该接口使用<see cref="System.ComponentModel.DataAnnotations"/>命名空间中的验证类.
    /// </summary>
    public sealed class DataErrorInfoSupport : IDataErrorInfo
    {
        private readonly object _instance;


        /// <summary>
        /// 初始化<see cref="DataErrorInfoSupport"/>类的新实例.
        /// </summary>
        /// <param name="instance">实例.</param>
        /// <exception cref="ArgumentNullException">实例不能<c>null</c>.</exception>
        public DataErrorInfoSupport(object instance)
        {
            if (instance == null) { throw new ArgumentNullException("instance"); }
            _instance = instance;
        }


        /// <summary>
        /// 获取一条错误消息，指示此对象有何错误.
        /// </summary>
        /// <returns>指示此对象的错误信息。默认为空字符串("").</returns>
        public string Error { get { return this[""]; } }

        /// <summary>
        /// 获取具有给定名称的属性的错误消息.
        /// </summary>
        /// <param name="memberName">要获取其错误消息的属性的名称.</param>
        /// <returns>属性的错误消息。默认为空字符串("").</returns>
        public string this[string memberName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                if (string.IsNullOrEmpty(memberName))
                {
                    Validator.TryValidateObject(_instance, new ValidationContext(_instance, null, null), validationResults, true);
                }
                else
                {
                    var property = TypeDescriptor.GetProperties(_instance)[memberName];
                    if (property == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                            "The specified member {0} was not found on the instance {1}", memberName, _instance.GetType()));
                    }
                    Validator.TryValidateProperty(property.GetValue(_instance),
                        new ValidationContext(_instance, null, null) { MemberName = memberName }, validationResults);
                }

                return string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage));
            }
        }
    }
}
