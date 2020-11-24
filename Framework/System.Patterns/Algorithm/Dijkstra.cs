using System.Collections.Generic;
using System.Linq;

namespace System.Patterns.Algorithm
{
    public class Path<T>
    {
        public T Source { get; set; }
        public T Destination { get; set; }

        /// <summary>
        /// 使用此路径从源到目的的成本
        /// </summary>
        /// <remarks>
        /// 低成本比高成本更可取
        /// </remarks>
        public int Cost { get; set; }
    } // class Path<T>


    /// <summary>
    ///  使用Dijkstra的算法计算各种路径之间的最佳路由
    /// </summary>
    /// <remarks>
    ///  复制算法的实现
    ///     <see cref="http://www.codeproject.com/Articles/22647/Dijkstra-Shortest-Route-Calculation-Object-Oriente" />.
    ///  实现被调整为支持泛型，并大量使用LINQ
    /// </remarks>
    public static class Dijkstra
    {
        public static LinkedList<Path<T>> CalculateShortestPathBetween<T>(T source, T destination,
            IEnumerable<Path<T>> paths)
        {
            return CalculateFrom(source, paths)[destination];
        }

        public static Dictionary<T, LinkedList<Path<T>>> CalculateShortestFrom<T>(T source, IEnumerable<Path<T>> paths)
        {
            return CalculateFrom(source, paths);
        }

        private static Dictionary<T, LinkedList<Path<T>>> CalculateFrom<T>(T source, IEnumerable<Path<T>> paths)
        {
            // 验证路径

            if (paths.Any(p => p.Source.Equals(p.Destination)))

                throw new ArgumentException("No path can have the same source and destination");


            // 跟踪到目前为止确定的最短路径

            var shortestPaths = new Dictionary<T, KeyValuePair<int, LinkedList<Path<T>>>>();

            // 跟踪已经完全处理过的位置

            var locationsProcessed = new List<T>();


            // 包含所有可能的步骤，包括Int.MaxValue成本

            paths.SelectMany(p => new[] { p.Source, p.Destination }) //联合来源和目的地
                .Distinct() //删除重复的
                .ToList() // ToList exposes ForEach
                .ForEach(s => shortestPaths.Set(s, Int32.MaxValue, null)); // add to ShortestPaths with MaxValue cost


            // update cost for self-to-self as 0; no path

            shortestPaths.Set(source, 0, null);


            // keep this cached

            var locationCount = shortestPaths.Keys.Count;


            while (locationsProcessed.Count < locationCount)
            {
                var locationToProcess = default(T);


                //搜索最近未处理的位置

                foreach (var location in shortestPaths.OrderBy(p => p.Value.Key).Select(p => p.Key).ToList())
                {
                    if (!locationsProcessed.Contains(location))
                    {
                        if (shortestPaths[location].Key == Int32.MaxValue)

                            return shortestPaths.ToDictionary(k => k.Key, v => v.Value.Value);
                        //ShortestPaths[destination].Value;


                        locationToProcess = location;

                        break;
                    }
                } // foreach


                var selectedPaths = paths.Where(p => p.Source.Equals(locationToProcess));

                foreach (var path in selectedPaths)
                {
                    if (shortestPaths[path.Destination].Key > path.Cost + shortestPaths[path.Source].Key)
                    {
                        shortestPaths.Set(
                            path.Destination,
                            path.Cost + shortestPaths[path.Source].Key,
                            shortestPaths[path.Source].Value.Union(new[] { path }).ToArray());
                    }
                } // foreach


                //将位置添加到处理后的位置列表中

                locationsProcessed.Add(locationToProcess);
            } // while


            return shortestPaths.ToDictionary(k => k.Key, v => v.Value.Value);

            //return ShortestPaths[destination].Value;
        }
    } // class Engine


    public static class ExtensionMethods
    {
        /// <summary>
        ///添加或更新字典，以包括目的地及其相关成本和完整路径(param数组使路径更容易使用)
        /// </summary>
        public static void Set<T>(this Dictionary<T, KeyValuePair<int, LinkedList<Path<T>>>> dictionary, T destination,
            int cost, params Path<T>[] paths)
        {
            var completePath = paths == null ? new LinkedList<Path<T>>() : new LinkedList<Path<T>>(paths);

            dictionary[destination] = new KeyValuePair<int, LinkedList<Path<T>>>(cost, completePath);
        }
    } // 
}