using System.Reflection;
using System.IO;

namespace System.Platform.Applications
{
    /// <summary>
    /// 该类提供关于正在运行的应用程序的信息.
    /// </summary>
    public static class ApplicationInfo
    {
        // ReSharper disable InconsistentNaming
        private static readonly Lazy<string> productName = new Lazy<string>(GetProductName);
        private static readonly Lazy<string> productDescription = new Lazy<string>(GetProductDescription);
        private static readonly Lazy<string> version = new Lazy<string>(GetVersion);
        private static readonly Lazy<string> company = new Lazy<string>(GetCompany);
        private static readonly Lazy<string> copyright = new Lazy<string>(GetCopyright);
        private static readonly Lazy<string> applicationPath = new Lazy<string>(GetApplicationPath);
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// 获取应用程序的产品名称.
        /// </summary>
        public static string ProductName { get { return productName.Value; } }

        /// <summary>
        /// 获取应用程序的产品描述.
        /// </summary>
        public static string ProductDescription { get { return productDescription.Value; } }

        /// <summary>
        /// 获取应用程序的版本号.
        /// </summary>
        public static string Version { get { return version.Value; } }

        /// <summary>
        /// 获取应用程序的公司.
        /// </summary>
        public static string Company { get { return company.Value; } }

        /// <summary>
        /// 获取应用程序的版权信息.
        /// </summary>
        public static string Copyright { get { return copyright.Value; } }

        /// <summary>
        /// 获取启动应用程序的可执行文件的路径，不包括可执行文件名.
        /// </summary>
        public static string ApplicationPath { get { return applicationPath.Value; } }

        private static string GetProductName()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
                    entryAssembly, typeof(AssemblyProductAttribute)));
                return (attribute != null) ? attribute.Product : "";
            }
            return "";
        }

        private static string GetProductDescription()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(
                    entryAssembly, typeof(AssemblyDescriptionAttribute)));
                return (attribute != null) ? attribute.Description : "";
            }
            return "";
        }

        private static string GetVersion()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                return entryAssembly.GetName().Version.ToString();
            }
            return "";
        }

        private static string GetCompany()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(
                    entryAssembly, typeof(AssemblyCompanyAttribute)));
                return (attribute != null) ? attribute.Company : "";
            }
            return "";
        }

        private static string GetCopyright()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                    entryAssembly, typeof(AssemblyCopyrightAttribute));
                return attribute != null ? attribute.Copyright : "";
            }
            return "";
        }

        private static string GetApplicationPath()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                return Path.GetDirectoryName(entryAssembly.Location);
            }
            return "";
        }
    }
}
