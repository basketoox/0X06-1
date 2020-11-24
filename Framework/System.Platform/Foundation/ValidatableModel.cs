using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Platform.Foundation
{
    /// <summary>
    ///  为支持验证的模型定义基类.
    /// </summary>
    [Serializable]
    public abstract class ValidatableModel : Model, INotifyDataErrorInfo
    {
        private static readonly ValidationResult[] NoErrors = new ValidationResult[0];

        [NonSerialized] private readonly Dictionary<string, List<ValidationResult>> _errors;
        private readonly object _lock = new object();
        [NonSerialized] private bool _hasErrors;


        /// <summary>
        /// 初始化<see cref="ValidatableModel" />类的新实例.
        /// </summary>
        protected ValidatableModel()
        {
            _errors = new Dictionary<string, List<ValidationResult>>();
        }


        /// <summary>
        /// 获取一个值，该值指示实体是否存在验证错误.
        /// </summary>
        public bool HasErrors
        {
            get { return _hasErrors; }
            private set { SetProperty(ref _hasErrors, value); }
        }


        /// <summary>
        /// 当属性或整个实体的验证错误发生更改时发生.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }


        /// <summary>
        ///  获取整个实体的验证错误.
        /// </summary>
        /// <returns>实体的验证错误.</returns>
        public IEnumerable<ValidationResult> GetErrors()
        {
            return GetErrors(null);
        }

        /// <summary>
        ///  获取指定属性或整个实体的验证错误.
        /// </summary>
        /// <param name="propertyName">
        ///     要检索验证错误的属性的名称;
        /// 或null或String。空，以检索实体级错误.
        /// </param>
        /// <returns>属性或实体的验证错误.</returns>
        public IEnumerable<ValidationResult> GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                List<ValidationResult> result;
                if (_errors.TryGetValue(propertyName, out result))
                {
                    return result;
                }
                return NoErrors;
            }
            return _errors.Values.SelectMany(x => x).Distinct().ToArray();
        }

        /// <summary>
        ///   验证对象及其所有属性.验证结果被存储，可以通过GetErrors方法检索.
        ///   如果验证结果正在更改，则将引发ErrorsChanged事件.
        /// </summary>
        /// <returns>如果对象有效，则为真，否则为假.</returns>
        public bool Validate()
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            if (validationResults.Any())
            {
                _errors.Clear();
                foreach (var validationResult in validationResults)
                {
                    var propertyNames = validationResult.MemberNames.Any()
                        ? validationResult.MemberNames
                        : new[] { "" };
                    foreach (var propertyName in propertyNames)
                    {
                        if (!_errors.ContainsKey(propertyName))
                        {
                            _errors.Add(propertyName, new List<ValidationResult> { validationResult });
                        }
                        else
                        {
                            _errors[propertyName].Add(validationResult);
                        }
                    }
                }
                RaiseErrorsChanged();
                return false;
            }
            if (_errors.Any())
            {
                _errors.Clear();
                RaiseErrorsChanged();
            }
            return true;
        }

        /// <summary>
        ///  Asynchronous审定
        /// </summary>
        /// <returns></returns>
        public Task ValidateAsync()
        {
            return Task.Run(() =>
            {
                lock (_lock)
                {
                    Validate();
                }
            });
        }

        /// <summary>
        ///  使用指定的值设置属性并验证属性。
        ///  如果值不等于字段，则设置字段，引发PropertyChanged事件，验证属性并返回true.
        /// </summary>
        /// <typeparam name="T">属性类型.</typeparam>
        /// <param name="field">引用属性的后端字段.</param>
        /// <param name="value">属性的新值.</param>
        /// <param name="propertyName">
        ///  属性名.这个可选参数可以跳过，因为编译器可以自动创建它.
        /// </param>
        /// <returns>如果值已经更改，则为True;如果新旧值相等，则为false.</returns>
        /// <exception cref="ArgumentException">参数propertyName不能为空或空.</exception>
        protected bool SetPropertyAndValidate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("The argument propertyName must not be null or empty.");
            }

            if (SetProperty(ref field, value, propertyName))
            {
                ValidateProperty(value, propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 使用指定的值验证属性.
        /// 验证结果被存储，可以通过GetErrors方法检索.
        /// 如果验证结果正在更改，则将引发ErrorsChanged事件.
        /// </summary>
        /// <param name="value">属性的值.</param>
        /// <param name="propertyName">
        ///  属性名.这个可选参数可以跳过，因为编译器可以自动创建它.
        /// </param>
        /// <returns>如果属性值有效，则为真，否则为假.</returns>
        /// <exception cref="ArgumentException">参数propertyName不能为空或空.</exception>
        protected bool ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("The argument propertyName must not be null or empty.");
            }

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = propertyName },
                validationResults);
            if (validationResults.Any())
            {
                _errors[propertyName] = validationResults;
                RaiseErrorsChanged(propertyName);
                return false;
            }
            if (_errors.Remove(propertyName))
            {
                RaiseErrorsChanged(propertyName);
            }
            return true;
        }

        /// <summary>
        /// 引发<see cref="E:ErrorsChanged" />事件.
        /// </summary>
        /// <param name="e">包含事件数据的<see cref="System.ComponentModel.DataErrorsChangedEventArgs" />实例.</param>
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var handler = ErrorsChanged;
            if (handler == null) return;
            handler.Invoke(this, e);
        }

        private void RaiseErrorsChanged(string propertyName = "")
        {
            HasErrors = _errors.Any();
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }
    }
}