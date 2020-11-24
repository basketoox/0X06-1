using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Toolkit.Helpers
{
    public class XmlHelper : XmlDocument
    {
        private string _xmlFileName;
        public string XmlFileName
        {
            get
            {
                return this._xmlFileName;
            }
            set
            {
                this._xmlFileName = value;
            }
        }
        public XmlHelper(string xmlFile)
        {
            this.XmlFileName = xmlFile;
            this.Load(xmlFile);
        }
        public XmlNode FindNode(string xPath)
        {
            return base.SelectSingleNode(xPath);
        }
        public string GetNodeValue(string xPath)
        {
            XmlNode xmlNode = base.SelectSingleNode(xPath);
            return xmlNode.InnerText;
        }
        public XmlNodeList GetNodeList(string xPath)
        {
            return base.SelectSingleNode(xPath).ChildNodes;
        }
    }
}
