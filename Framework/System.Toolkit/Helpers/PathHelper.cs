using System;
using System.IO;
using System.Reflection;

namespace System.Toolkit.Helpers
{
    public static class PathHelper
    {
        public static string GetExecutingDirectory()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        }

        public static Uri GetAssemblyUri(string assemblyName, string path)
        {
            Uri uri;
            Uri.TryCreate(string.Format("pack://application:,,,/{0};component/{1}", assemblyName, path), UriKind.RelativeOrAbsolute, out uri);
            return uri;
        }

        public static Uri GetCallingAssemblyUri(string path)
        {
            return GetAssemblyUri(Assembly.GetCallingAssembly().GetName().Name, path);
        }

        public static Uri GetExeAssemblyUri(string path)
        {
            return GetAssemblyUri(Assembly.GetEntryAssembly().GetName().Name, path);
        }
    }
}


