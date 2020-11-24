using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Patterns
{
    //在WPF程序里面，大部分业务对象都要实现InotifyPropertyChanged以便数据绑定，所以我们实现了这个接口，并让EditableObject从这个实现派生，从而让Editableobject也具有绑定支持。NotifiableObject类非常简单，如下：
    [Serializable]
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        private const string ERROR_MSG = "{0} is not a public property of {1}";
        private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> _eventArgCache;

        static NotifiableObject()
        {
            //缓存PropertyChangedEventArgs，以提高性能。
            _eventArgCache = new ConcurrentDictionary<string, PropertyChangedEventArgs>();
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("propertyName cannotbe null or empty.");
            }
            return _eventArgCache.GetOrAdd(propertyName, p => new PropertyChangedEventArgs(p));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            VerifyProperty(propertyName);
            var handler = PropertyChanged;
            if (handler != null)
            {
                var args = GetPropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            var type = GetType();
            var propInfo = type.GetProperty(propertyName);
            if (propInfo == null)
            {
                Debug.Fail(string.Format(ERROR_MSG, propertyName, type.FullName));
            }
        }
    }
}