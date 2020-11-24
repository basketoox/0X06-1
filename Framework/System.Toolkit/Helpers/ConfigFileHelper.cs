using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Toolkit.Helpers
{
    public class ConfigFileHelper : ConfigFileHelper<string, string>
    {
        public void Load(string filePath)
        {
            base.Load(filePath, delegate(string keyString, out string key)
            {
                key = keyString;
                return true;
            }, delegate(string valueString, out string value)
            {
                value = valueString;
                return true;
            });
        }

        public void Load(Stream stream)
        {
            base.Load(stream, delegate(string keyString, out string key)
            {
                key = keyString;
                return true;
            }, delegate(string valueString, out string value)
            {
                value = valueString;
                return true;
            });
        }
    }

    public class ConfigFileHelper<K, V>
    {
        public delegate bool DelTryParseKey(string keyString, out K key);
        public delegate bool DelTryParseValue(string valueString, out V value);

        public Dictionary<K, V> ConfigDictionary { get; private set; }

        public ConfigFileHelper()
        {
            this.ConfigDictionary = new Dictionary<K, V>();
        }

        public V this[K key]
        {
            get
            {
                return this.ConfigDictionary[key];
            }
            set
            {
                this.ConfigDictionary[key] = value;
            }
        }

        public bool ContainsKey(K key)
        {
            return this.ConfigDictionary.ContainsKey(key);
        }

        public void Load(string filePath, DelTryParseKey tryParseKey, DelTryParseValue tryParseValue)
        {
            using (var file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                this.Load(file, tryParseKey, tryParseValue);
            }
        }

        public void Load(Stream stream, DelTryParseKey tryParseKey, DelTryParseValue tryParseValue)
        {
            string line = null;
            var markIndex = 0;
            string t_line = null;
            var key = default(K);
            var value = default(V);
            var reader = new StreamReader(stream, Encoding.UTF8);
            while ((line = reader.ReadLine()) != null)
            {
                t_line = line.Trim();
                if (t_line == "")
                    continue;
                //注释
                if (t_line.StartsWith("--") || t_line.StartsWith(";"))
                    continue;
                //不含等号
                if ((markIndex = t_line.IndexOf('=')) < 1)
                    continue;
                //无法转换key
                if (!tryParseKey(t_line.Substring(0, markIndex), out key))
                    continue;
                //无法转换value
                if (markIndex + 1 == t_line.Length && !tryParseValue("", out value))
                    continue;
                else if (!tryParseValue(t_line.Substring(markIndex + 1), out value))
                    continue;
                ConfigDictionary[key] = value;
            }
        }


        public void Save(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                this.Save(file);
            }
        }

        public void Save(Stream stream)
        {
            var writer = new StreamWriter(stream);
            foreach (var item in this.ConfigDictionary)
                writer.WriteLine(item.Key.ToString() + "=" + item.Value.ToString());
            writer.Flush();
        }
    }
}
