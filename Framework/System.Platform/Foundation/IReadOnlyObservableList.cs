using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 表示可由索引访问的只读元素列表。另外，列表会将更改通知侦听器，例如什么时候添加和删除项.
    /// </summary>
    /// <typeparam name="T">
    /// 只读列表中元素的类型。这个类型参数是协变的.
    /// 也就是说，您可以使用指定的类型或任何派生程度更高的类型.
    /// </typeparam>
    public interface IReadOnlyObservableList<out T> : IReadOnlyList<T>, INotifyCollectionChanged
    {
    }
}
