using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 代表一个只读<see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>.
    /// 这个类实现了IReadOnlyObservableList接口，并提供了公共的CollectionChanged和PropertyChanged事件.
    /// </summary>
    /// <typeparam name="T">集合中元素的类型.</typeparam>
    [Serializable]
    public class ReadOnlyObservableList<T> : ReadOnlyObservableCollection<T>, IReadOnlyObservableList<T>
    {
        /// <summary>
        /// <see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection&lt;T&gt;"/>的新实例初始化类，
        /// 作为指定的包装器<see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>.
        /// </summary>
        /// <param name="list">
        /// <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>
        /// 创建<see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection&lt;T&gt;"/>类的实例
        /// </param>
        /// <exception cref="ArgumentNullException">列表为空.</exception>
        public ReadOnlyObservableList(ObservableCollection<T> list)
            : base(list)
        {
        }


        /// <summary>
        /// 当集合更改时发生.
        /// </summary>
        public new event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }

        /// <summary>
        /// 当属性值更改时发生.
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged
        {
            add { base.PropertyChanged += value; }
            remove { base.PropertyChanged -= value; }
        }
    }
}
