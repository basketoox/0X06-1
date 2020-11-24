using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace System.Platform.Presentation
{
    // 该类存储卸载控件的验证错误。当再次加载控件时，它将恢复ValidationErrors.
    internal class ValidationReloadedTracker
    {
        private readonly ValidationTracker validationTracker;
        private readonly IEnumerable<ValidationError> errors;


        public ValidationReloadedTracker(ValidationTracker validationTracker, object validationSource,
            IEnumerable<ValidationError> errors)
        {
            this.validationTracker = validationTracker;
            this.errors = errors;

            if (validationSource is FrameworkElement)
            {
                ((FrameworkElement)validationSource).Loaded += ValidationSourceLoaded;
            }
            else
            {
                ((FrameworkContentElement)validationSource).Loaded += ValidationSourceLoaded;
            }
        }


        private void ValidationSourceLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                ((FrameworkElement)sender).Loaded -= ValidationSourceLoaded;
            }
            else
            {
                ((FrameworkContentElement)sender).Loaded -= ValidationSourceLoaded;
            }

            validationTracker.AddErrors(sender, errors);
        }
    }
}
