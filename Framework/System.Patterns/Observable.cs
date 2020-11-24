using System.Collections.Generic;

namespace System.Patterns
{
    public class Observable<T> : IObservable<T>
    {
        #region Class

        private class Unsubscriber : IDisposable
        {
            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _Observers = observers;
                _Observer = observer;
            }

            private List<IObserver<T>> _Observers { get; set; }
            private IObserver<T> _Observer { get; set; }

            public void Dispose()
            {
                if (_Observer != null && _Observers.Contains(_Observer))
                {
                    _Observers.Remove(_Observer);
                }
            }
        }

        #endregion

        #region Enum

        private enum NotifyType
        {
            Next,
            Completed,
            Error
        }

        #endregion

        #region Var

        private List<IObserver<T>> _observers;

        #endregion

        #region Protected Property

        protected List<IObserver<T>> Observers
        {
            get { return _observers ?? (_observers = new List<IObserver<T>>()); }
        }

        #endregion

        #region Private Method

        private void NotifyError(Exception error)
        {
            foreach (var observer in Observers)
            {
                observer.OnError(error);
            }
        }

        #endregion

        #region Protected Method

        protected void NotifyNext(T obj)
        {
            try
            {
                foreach (var observer in Observers)
                {
                    observer.OnNext(obj);
                }
            }
            catch (Exception e)
            {
                NotifyError(e);
            }
        }

        protected void NotifyCompleted()
        {
            try
            {
                foreach (var observer in Observers)
                {
                    observer.OnCompleted();
                }
            }
            catch (Exception e)
            {
                NotifyError(e);
            }
        }

        #endregion

        #region Public Method

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
            return new Unsubscriber(Observers, observer);
        }

        #endregion
    }
}