using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace System.Toolkit.Helpers
{
    /// <summary>
    /// 项目文件序列化管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializerManager<T>
    {
        /// <summary>
        /// 单件方式实例
        /// </summary>
        public static SerializerManager<T> Instance = new SerializerManager<T>();

        private readonly XmlSerializer _serializer;

        private SerializerManager()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <param name="fileName">包含绝对路径的文件名字</param>
        public T Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                string path = fileName;
                XmlDocument xmlDocument = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDocument.AppendChild(xmlDeclaration);
                XmlElement rootElement = xmlDocument.CreateElement(typeof(T).Name);
                xmlDocument.AppendChild(rootElement);
                xmlDocument.Save(path);
            }
            using (var filestream = new FileStream(fileName, FileMode.Open))
            {
                return Load(filestream);
            }
        }

        /// <summary>
        /// 从流加载
        /// </summary>
        /// <param name="stream"> </param>
        public T Load(Stream stream)
        {
            var reader = new XmlTextReader(stream);
            return Load(reader);
        }

        /// <summary>
        /// 从 XmlTextReader 加载
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public T Load(XmlTextReader reader)
        {
            return (T)_serializer.Deserialize(reader);
        }

        /// <summary>
        /// 序列化对象到文件
        /// </summary>
        /// <param name="fileName">包含绝对路径的文件名字</param>
        /// <param name="config">项目配置对象</param>
        public void Save(string fileName, T config)
        {
            using (var filestream = new FileStream(fileName, FileMode.Create))
            {
                Save(filestream, config);
            }
        }

        /// <summary>
        /// 序列化对象到数据流
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="config">项目配置对象</param>
        public void Save(Stream stream, T config)
        {
            // UTF8编码 换行符
            var writer = new XmlTextWriter(stream, Encoding.UTF8) { Formatting = Formatting.Indented };
            Save(writer, config);
        }

        /// <summary>
        /// 序列化对象到 XmlTextWriter
        /// </summary>
        /// <param name="writer">XmlTextWriter的实例</param>
        /// <param name="config">项目配置对象</param>
        public void Save(XmlTextWriter writer, T config)
        {
            var xmlns = new XmlSerializerNamespaces();
            //xmlns.Add("", "Serializer");
            xmlns.Add(String.Empty, string.Empty);
            _serializer.Serialize(writer, config, xmlns);
        }
    }
}
