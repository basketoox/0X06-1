using System.Collections.Generic;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 为集合提供助手方法.
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// 在无法找到下一个元素时，获取集合中的下一个元素或默认值.
        /// </summary>
        /// <typeparam name="T">项目的类型.</typeparam>
        /// <param name="collection">集合.</param>
        /// <param name="current">当前项.</param>
        /// <returns>当无法找到下一个元素时，集合中的下一个元素或默认元素.</returns>
        /// <exception cref="ArgumentNullException">集合不能<c>null</c>.</exception>
        /// <exception cref="ArgumentException">集合不包含指定的当前项.</exception>
        public static T GetNextElementOrDefault<T>(IEnumerable<T> collection, T current)
        {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            var found = false;
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(enumerator.Current, current))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new ArgumentException("The collection does not contain the current item.");
            }

            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            return default(T);
        }
    }
}
