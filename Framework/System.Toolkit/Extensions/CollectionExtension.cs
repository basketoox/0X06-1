using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace System.Toolkit.Extensions
{
    /// <summary>
    ///     表示集合扩展。
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        ///     获取迭代器是否为空。
        /// </summary>
        /// <typeparam name="T">类型。</typeparam>
        /// <param name="items">迭代器。</param>
        /// <returns>如果迭代器为空，返回true。否则返回false。</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            return !items.Any();
        }

        /// <summary>
        ///     获取迭代器是否为null或者为空。
        /// </summary>
        /// <typeparam name="T">类型。</typeparam>
        /// <param name="items">迭代器。</param>
        /// <returns>如果迭代器为null或者空，返回true。否则返回false。</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            if (items == null)
            {
                return true;
            }
            return items.IsEmpty();
        }

        /// <summary>
        ///     将指定的集合添加到当前集合末尾。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="source">当前集合。</param>
        /// <param name="items">指定的集合。</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            items.ToList().ForEach(source.Add);
        }

        /// <summary>
        ///     转换为ObservableCollection。
        /// </summary>
        /// <typeparam name="T">迭代器的数据类型。</typeparam>
        /// <param name="items">迭代器实例。</param>
        /// <returns>返回ObservableCollection实例</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }

        /// <summary>
        ///     转换为BindingList。
        /// </summary>
        /// <typeparam name="T">迭代器的数据类型。</typeparam>
        /// <param name="items">迭代器实例。</param>
        /// <returns>返回BindingList实例</returns>
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> items)
        {
            var collection = new BindingList<T>();
            collection.AddRange(items);
            return collection;
        }

        /// <summary>
        ///     对迭代器的每个元素执行指定操作
        /// </summary>
        /// <typeparam name="T">迭代器的数据类型。</typeparam>
        /// <param name="items">迭代器实例。</param>
        /// <param name="action">操作。</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        ///     带索引的遍历方法。
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> col, Action<T, int> action)

        {
            var index = 0;

            foreach (var item in col)
            {
                action(item, index++);
            }
        }

        /// <summary>
        ///     可以半途中断执行的遍历方法。
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> col, Func<T, bool> action)
        {
            foreach (var item in col)
            {
                if (!action(item)) break;
            }
        }

        /// <summary>
        ///     可以半途中段的带索引的遍历方法。
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> col, Func<T, int, bool> action)
        {
            var index = 0;

            foreach (var item in col)
            {
                if (!action(item, index++)) break;
            }
        }

        /// <summary>
        ///     给非强类型的IEnumerable返回头一个元素。
        /// </summary>
        public static object First(this IEnumerable col)
        {
            foreach (var item in col)
            {
                return item;
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        ///     给非强类型的IEnumerable返回头一个强类型的元素
        /// </summary>
        public static object First<T>(this IEnumerable col)
        {
            return (T) col.First();
        }

        #region 以下为IEnumerable<T>的非泛型实现

        public static void ForEach<T>(this IEnumerable col, Action<object> action)
        {
            foreach (var item in col)
            {
                action(item);
            }
        }


        public static void ForEach<T>(this IEnumerable col, Action<object, int> action)
        {
            var index = 0;

            foreach (var item in col)
            {
                action(item, index++);
            }
        }


        public static void ForEach<T>(this IEnumerable col, Func<object, bool> action)
        {
            foreach (var item in col)
            {
                if (!action(item)) break;
            }
        }


        public static void ForEach<T>(this IEnumerable col, Func<object, int, bool> action)
        {
            var index = 0;

            foreach (var item in col)
            {
                if (!action(item, index++)) break;
            }
        }

        #endregion
    }
}