using System.Xml.Linq;

namespace System.Toolkit.Extensions
{
    /// <summary>
    /// 表示Xml扩展。
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// 获取指定名称的属性值并转换成字符串。
        /// </summary>
        /// <param name="element">Xml元素。</param>
        /// <param name="name">属性名称。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果存在指定名称属性，返回属性值字符串。否则返回string.Empty。</returns>
        public static string GetAttributeValueToString(this XElement element, string name, string defaultValue = "")
        {
            var attribute = element.Attribute(name);
            if (attribute != null)
            {
                if (attribute.Value.IsNullOrEmpty())
                {
                    return defaultValue;
                }
                return attribute.Value;
            }
            return defaultValue;
        }
    }
}
