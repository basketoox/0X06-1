using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Resources;

namespace System.Toolkit.Helpers
{
    /// <summary>
    /// 资源帮助类
    /// </summary>
    public class ResourceHelper
    {
        private static ConcurrentDictionary<string, ResourceManager> _resourceManager = new ConcurrentDictionary<string, ResourceManager>();

        /// <summary>
        /// 获取程序集中默认资源文件(Resources)的ResourceManager
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static System.Resources.ResourceManager GetResourceManager(Assembly assembly)
        {
            return _resourceManager.GetOrAdd(assembly.FullName,
                name => new ResourceManager(string.Format("{0}.Properties.Resources", assembly.GetName().Name), assembly));
        }

        public static System.Resources.ResourceManager GetResourceManager(Type resourceSource)
        {
            return _resourceManager.GetOrAdd(resourceSource.FullName,
                name => new ResourceManager(resourceSource));
        }

        /// <summary>
        /// 获取调用当前正在执行的方法的方法所在的Assembly中默认资源文件(Resources)的Object资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCallingAssemblyResourceObject(string key)
        {
            return GetResourceManager(Assembly.GetCallingAssembly()).GetObject(key);
        }
        /// <summary>
        /// 获取调用当前正在执行的方法的方法所在的Assembly中默认资源文件(Resources)的字符串资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCallingAssemblyResourceString(string key)
        {
            return GetResourceManager(Assembly.GetCallingAssembly()).GetString(key);
        }

        /// <summary>
        /// 获取调用当前正在执行的方法的方法所在的Assembly中默认资源文件(Resources)的流资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static UnmanagedMemoryStream GetCallingAssemblyResourceStream(string key)
        {
            return GetResourceManager(Assembly.GetCallingAssembly()).GetStream(key);
        }
        /// <summary>
        /// 获取指定Assembly中中默认资源文件(Resources)的Object资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetResourceObject(Assembly assembly, string key)
        {
            return GetResourceManager(assembly).GetObject(key);
        }
        /// <summary>
        /// 获取指定Assembly中中默认资源文件(Resources)的字符串资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetResourceString(Assembly assembly, string key)
        {
            return GetResourceManager(assembly).GetString(key);
        }

        /// <summary>
        /// 获取指定Assembly中中默认资源文件(Resources)的流资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static UnmanagedMemoryStream GetResourceStream(Assembly assembly, string key)
        { 
            return GetResourceManager(assembly).GetStream(key);
        }

        /// <summary>
        /// 获取指定资源文件类类型的Object资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetResourceObject(Type resourceSource, string key)
        {
            return GetResourceManager(resourceSource).GetObject(key);
        }
        /// <summary>
        /// 获取指定资源文件类类型的字符串资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetResourceString(Type resourceSource, string key)
        {
            return GetResourceManager(resourceSource).GetString(key);
        }

        /// <summary>
        /// 获取指定资源文件类类型中的字符串资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static UnmanagedMemoryStream GetResourceStream(Type resourceSource, string key)
        {
            return GetResourceManager(resourceSource).GetStream(key);
        }
    }
}
