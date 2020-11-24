using System.ComponentModel;
using System.Globalization;
using System.Platform.Foundation;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Platform.Applications
{
    /// <summary>
    /// 表示最近的文件.
    /// </summary>
    public sealed class RecentFile : Model, IXmlSerializable
    {
        private bool _isPinned;
        private string _path;


        /// <summary>
        /// 此构造函数重载被保留，不应使用。XmlSerializer在内部使用它.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RecentFile() { }

        /// <summary>
        /// 初始化<see cref="RecentFile"/>类的新实例.
        /// </summary>
        /// <param name="path">文件路径.</param>
        /// <exception cref="ArgumentException">参数路径不能为空或空.</exception>
        public RecentFile(string path)
        {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentException("The argument path must not be null or empty."); }
            _path = path;
        }

        /// <summary>
        /// 获取或设置一个值，该值指示最近的文件是否固定.
        /// </summary>
        public bool IsPinned
        {
            get { return _isPinned; }
            set { SetProperty(ref _isPinned, value); }
        }

        /// <summary>
        /// 获取文件路径.
        /// </summary>
        public string Path { get { return _path; } }

        XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) { throw new ArgumentNullException("reader"); }

            IsPinned = bool.Parse(reader.GetAttribute("IsPinned"));
            _path = reader.ReadElementContentAsString();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) { throw new ArgumentNullException("writer"); }

            writer.WriteAttributeString("IsPinned", IsPinned.ToString(CultureInfo.InvariantCulture));
            writer.WriteString(Path);
        }
    }
}
