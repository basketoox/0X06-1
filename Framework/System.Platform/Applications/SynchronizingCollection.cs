using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Platform.Foundation;


namespace System.Platform.Applications
{
    /// <summary>
    /// 表示将其所有项与指定原始集合的项同步的集合。
    /// 当原始集合通过<see cref="INotifyCollectionChanged"/> 接口通知更改时
    ///这个集合使它自己的项与原始项同步.
    /// </summary>
    /// <typeparam name="T">集合中元素的类型.</typeparam>
    /// <typeparam name="TOriginal">原始集合中的元素类型.</typeparam>
    public class SynchronizingCollection<T, TOriginal> : ReadOnlyObservableList<T>
    {
        private readonly ObservableCollection<T> _innerCollection;
        private readonly List<Tuple<TOriginal, T>> _mapping;
        private readonly IEnumerable<TOriginal> _originalCollection;
        private readonly Func<TOriginal, T> _factory;
        private readonly IEqualityComparer<T> _itemComparer;
        private readonly IEqualityComparer<TOriginal> _originalItemComparer;


        /// <summary>
        /// 初始化<see cref="SynchronizingCollection&lt;T, TOriginal&gt;"/>类的新实例.
        /// </summary>
        /// <param name="originalCollection">原来的集合.</param>
        /// <param name="factory">用于在此集合中创建新元素的工厂.</param>
        /// <exception cref="ArgumentNullException">参数originalCollection必须不为空.</exception>
        /// <exception cref="ArgumentNullException">参数factory不能为空.</exception>
        public SynchronizingCollection(IEnumerable<TOriginal> originalCollection, Func<TOriginal, T> factory)
            : base(new ObservableCollection<T>())
        {
            if (originalCollection == null) { throw new ArgumentNullException("originalCollection"); }
            if (factory == null) { throw new ArgumentNullException("factory"); }

            _mapping = new List<Tuple<TOriginal, T>>();
            _originalCollection = originalCollection;
            _factory = factory;
            _itemComparer = EqualityComparer<T>.Default;
            _originalItemComparer = EqualityComparer<TOriginal>.Default;

            var collectionChanged = originalCollection as INotifyCollectionChanged;
            if (collectionChanged != null)
            {
                CollectionChangedEventManager.AddHandler(collectionChanged, OriginalCollectionChanged);
            }

            _innerCollection = (ObservableCollection<T>)Items;
            foreach (var item in _originalCollection)
            {
                _innerCollection.Add(CreateItem(item));
            }
        }

        private T CreateItem(TOriginal oldItem)
        {
            var newItem = _factory(oldItem);
            _mapping.Add(new Tuple<TOriginal, T>(oldItem, newItem));
            return newItem;
        }

        // ReSharper disable UnusedMethodReturnValue.Local
        private bool RemoveCore(TOriginal oldItem)
        //ReSharper restore UnusedMethodReturnValue.Local
        {
            var tuple = _mapping.First(t => _originalItemComparer.Equals(t.Item1, oldItem));
            _mapping.Remove(tuple);
            return _innerCollection.Remove(tuple.Item2);
        }

        private void RemoveAtCore(int index)
        {
            var newItem = this[index];
            var tuple = _mapping.First(t => _itemComparer.Equals(t.Item2, newItem));
            _mapping.Remove(tuple);
            _innerCollection.RemoveAt(index);
        }

        private void ClearCore()
        {
            _innerCollection.Clear();
            _mapping.Clear();
        }

        private void OriginalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex >= 0)
                {
                    var i = e.NewStartingIndex;
                    foreach (TOriginal item in e.NewItems)
                    {
                        _innerCollection.Insert(i, CreateItem(item));
                        i++;
                    }
                }
                else
                {
                    foreach (TOriginal item in e.NewItems)
                    {
                        _innerCollection.Add(CreateItem(item));
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldStartingIndex >= 0)
                {
                    for (var i = 0; i < e.OldItems.Count; i++)
                    {
                        RemoveAtCore(e.OldStartingIndex);
                    }
                }
                else
                {
                    foreach (TOriginal item in e.OldItems)
                    {
                        RemoveCore(item);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                if (e.NewStartingIndex >= 0)
                {
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        _innerCollection[i + e.NewStartingIndex] = CreateItem((TOriginal)e.NewItems[i]);
                    }
                }
                else
                {
                    foreach (TOriginal item in e.OldItems) { RemoveCore(item); }
                    foreach (TOriginal item in e.NewItems) { _innerCollection.Add(CreateItem(item)); }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                for (var i = 0; i < e.NewItems.Count; i++)
                {
                    _innerCollection.Move(e.OldStartingIndex + i, e.NewStartingIndex + i);
                }
            }
            else // Reset
            {
                ClearCore();
                foreach (var item in _originalCollection)
                {
                    _innerCollection.Add(CreateItem(item));
                }
            }
        }
    }
}
