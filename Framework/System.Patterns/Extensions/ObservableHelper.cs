namespace System.Patterns.Extensions
{
    /// <summary>
    /// 弱观察
    /// </summary>
    internal static class WeakObservation
    {
        public static IDisposable WeakSubscribe<T>(this IObservable<T> observable, IObserver<T> observer)
        {
            return new WeakSubscription<T>(observable, observer);
        }
    }
    /// <summary>
    /// 弱订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WeakSubscription<T> : IObserver<T>, IDisposable
    {
        private readonly WeakReference _reference;
        private readonly IDisposable _subscription;
        private bool _disposed;

        public WeakSubscription(IObservable<T> observable, IObserver<T> observer)
        {
            _reference = new WeakReference(observer);
            _subscription = observable.Subscribe(this);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _subscription.Dispose();
            }
        }

        public void OnCompleted()
        {
            var observer = (IObserver<T>)_reference.Target;
            if (observer != null) observer.OnCompleted();
            else Dispose();
        }

        public void OnError(Exception error)
        {
            var observer = (IObserver<T>)_reference.Target;
            if (observer != null) observer.OnError(error);
            else Dispose();
        }

        public void OnNext(T value)
        {
            var observer = (IObserver<T>)_reference.Target;
            if (observer != null) observer.OnNext(value);
            else Dispose();
        }
    }
}