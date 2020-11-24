using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace System.Platform.Presentation
{
    // 这个类监听验证。所有者(控件)的错误事件。
    // 当引发错误事件时，它会将错误与其内部错误列表同步，并更新ValidationHelper.
    internal sealed class ValidationTracker
    {
        private readonly List<Tuple<object, ValidationError>> _errors;
        private readonly DependencyObject _owner;


        public ValidationTracker(DependencyObject owner)
        {
            _owner = owner;
            _errors = new List<Tuple<object, ValidationError>>();

            Validation.AddErrorHandler(owner, ErrorChangedHandler);
        }


        internal void AddErrors(object validationSource, IEnumerable<ValidationError> errors)
        {
            foreach (var error in errors)
            {
                AddError(validationSource, error);
            }

            ValidationHelper.InternalSetIsValid(_owner, !errors.Any());
        }

        private void AddError(object validationSource, ValidationError error)
        {
            _errors.Add(new Tuple<object, ValidationError>(validationSource, error));

            if (validationSource is FrameworkElement)
            {
                ((FrameworkElement)validationSource).Unloaded += ValidationSourceUnloaded;
            }
            else if (validationSource is FrameworkContentElement)
            {
                ((FrameworkContentElement)validationSource).Unloaded += ValidationSourceUnloaded;
            }
        }

        private void ErrorChangedHandler(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                AddError(e.OriginalSource, e.Error);
            }
            else
            {
                var error = _errors.FirstOrDefault(err => err.Item1 == e.OriginalSource && err.Item2 == e.Error);
                if (error != null) { _errors.Remove(error); }
            }

            ValidationHelper.InternalSetIsValid(_owner, !_errors.Any());
        }

        private void ValidationSourceUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                ((FrameworkElement)sender).Unloaded -= ValidationSourceUnloaded;
            }
            else
            {
                ((FrameworkContentElement)sender).Unloaded -= ValidationSourceUnloaded;
            }

            // 卸载的控件可能会再次加载。然后我们需要恢复验证错误.
            var errorsToRemove = _errors.Where(err => err.Item1 == sender).ToArray();
            if (errorsToRemove.Any())
            {
                // 因为它监听已加载的事件，所以它保持活跃.
                new ValidationReloadedTracker(this, errorsToRemove.First().Item1, errorsToRemove.Select(x => x.Item2));

                foreach (var error in errorsToRemove)
                {
                    _errors.Remove(error);
                }
            }

            ValidationHelper.InternalSetIsValid(_owner, !_errors.Any());
        }
    }
}
