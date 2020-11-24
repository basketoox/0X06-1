using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
//XML引用
using System.Xml;
//序列化引用
using System.Runtime.Serialization.Formatters.Binary;
namespace System.Toolkit.Helpers
{
    public static class IniHelper
    {
        #region 读写INI文件相关
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("kernel32.dll ", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);
        #endregion


        #region INI文件操作

        //函数作用：从私有文件中获取字符串（读Ini文件）
        //section:欲在其中查找条目的小节名称
        //key:欲获取的项名或条目名
        //def:指定的条目没有找到时返回的默认值。可设为空（""） 
        //retVal:指定一个字串缓冲区，长度至少为size 
        //size:缓冲区大小
        //filePath:INI文件的完整路径


        /// <summary>
        /// 写ini文件函数
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">关键字</param>
        /// <param name="Value">要设置的值</param>
        /// <param name="filepath">ini文件路径</param>
        public static long WriteValue(string Section, string Key, string Value, string filepath)//对ini文件进行写操作的函数
        {
            long r = WritePrivateProfileString(Section, Key, Value, filepath);
            return r;
        }
        /// <summary>
        /// 读ini文件函数
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">关键字</param>
        /// <param name="filepath">文件路径</param>
        /// <returns>返回string</returns>
        public static T ReadValue<T>(string Section, string Key, string filepath, T DefaultValue)//对ini文件进行读操作的函数
        {
            StringBuilder temp = new StringBuilder(255);
            long i = GetPrivateProfileString(Section, Key, null, temp, 255, filepath);
            if (temp.ToString().Trim() == "" || temp.ToString() == null) return DefaultValue;
            return (T)Convert.ChangeType(temp.ToString(), typeof(T));
        }

        /// <summary>
        /// 读取一个ini里面所有的节
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns>-1:没有节信息，0:正常</returns>
        public static int GetAllSectionNames(out string[] sections, string path)
        {
            var MAX_BUFFER = 0x7fff;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
            {
                sections = null;
                return -1;
            }
            string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return 0;
        }
        /// <summary>
        /// 返回指定配置文件下的节名称列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetAllSectionNames(string path)
        {
            List<string> sectionList = new List<string>();
            var MAX_BUFFER = 0x7fff;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned != 0)
            {
                string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();
                Marshal.FreeCoTaskMem(pReturnedString);
                sectionList.AddRange(local.Substring(0, local.Length - 1).Split('\0'));
            }
            return sectionList;
        }

        /// <summary>
        /// 得到某个节点下面所有的key和value组合
        /// </summary>
        /// <param name="section">指定的节名称</param>
        /// <param name="keys">Key数组</param>
        /// <param name="values">Value数组</param>
        /// <param name="path">INI文件路径</param>
        /// <returns></returns>
        public static int GetAllKeyValues(string section, out string[] keys, out string[] values, string path)
        {
            byte[] b = new byte[65535];//配置节下的所有信息
            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);//配置信息
            string[] tmp = s.Split((char)0);//Key\Value信息
            List<string> result = new List<string>();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            values = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].Split(new char[] { '=' });//Key=Value格式的配置信息
                //Value字符串中含有=的处理，
                //一、Value加""，先对""处理
                //二、Key后续的都为Value
                if (item.Length > 2)
                {
                    keys[i] = item[0].Trim();
                    values[i] = result[i].Substring(keys[i].Length + 1);
                }
                if (item.Length == 2)//Key=Value
                {
                    keys[i] = item[0].Trim();
                    values[i] = item[1].Trim();
                }
                else if (item.Length == 1)//Key=
                {
                    keys[i] = item[0].Trim();
                    values[i] = "";
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                    values[i] = "";
                }
            }
            return 0;
        }
        /// <summary>
        /// 得到某个节点下面所有的key
        /// </summary>
        /// <param name="section">指定的节名称</param>
        /// <param name="keys">Key数组</param>
        /// <param name="path">INI文件路径</param>
        /// <returns></returns>
        public static int GetAllKeys(string section, out string[] keys, string path)
        {
            byte[] b = new byte[65535];

            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            ArrayList result = new ArrayList();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    keys[i] = item[0].Trim();
                }
                else if (item.Length == 1)
                {
                    keys[i] = item[0].Trim();
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取指定节下的Key列表
        /// </summary>
        /// <param name="section">指定的节名称</param>
        /// <param name="path">配置文件名称</param>
        /// <returns>Key列表</returns>
        public static List<string> GetAllKeys(string section, string path)
        {
            List<string> keyList = new List<string>();
            byte[] b = new byte[65535];
            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            List<string> result = new List<string>();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].Split(new char[] { '=' });
                if (item.Length == 2 || item.Length == 1)
                {
                    keyList.Add(item[0].Trim());
                }
                else if (item.Length == 0)
                {
                    keyList.Add(string.Empty);
                }
            }
            return keyList;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetAllValues(string section, string path)
        {
            List<string> keyList = new List<string>();
            byte[] b = new byte[65535];
            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            List<string> result = new List<string>();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].Split(new char[] { '=' });
                if (item.Length == 2 || item.Length == 1)
                {
                    keyList.Add(item[1].Trim());
                }
                else if (item.Length == 0)
                {
                    keyList.Add(string.Empty);
                }
            }
            return keyList;
        }

        #endregion

        #region 通过值查找键（一个节中的键唯一，可能存在多个键值相同，因此反查的结果可能为多个）

        /// <summary>
        /// 第一个键
        /// </summary>
        /// <param name="section"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFirstKeyByValue(string section, string path, string value)
        {
            foreach (string key in GetAllKeys(section, path))
            {
                if (ReadValue<string>(section, key, path, "") == value)
                {
                    return key;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 所有键
        /// </summary>
        /// <param name="section"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> GetKeysByValue(string section, string path, string value)
        {
            List<string> keys = new List<string>();
            foreach (string key in GetAllKeys(section, path))
            {
                if (ReadValue<string>(section, key, path, "") == value)
                {
                    keys.Add(key);
                }
            }
            return keys;
        }
        #endregion


        #region 具体类型的读写

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="path"></param>
        public static void DeleteKey(string sectionName, string keyName, string path)
        {
            WritePrivateProfileString(sectionName, keyName, null, path);
        }

        /// <summary>
        /// 删除指定节下的所有项
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="path"></param>
        public static void EraseSection(string sectionName, string path)
        {
            WritePrivateProfileString(sectionName, null, null, path);
        }

        /// <summary>
        /// 指定节知否存在
        /// </summary>
        /// <param name="section"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ExistSection(string section, string fileName)
        {
            string[] sections = null;
            GetAllSectionNames(out sections, fileName);
            if (sections != null)
            {
                foreach (var s in sections)
                {
                    if (s == section)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 指定节下的键是否存在
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ExistKey(string section, string key, string fileName)
        {
            string[] keys = null;
            GetAllKeys(section, out keys, fileName);
            if (keys != null)
            {
                foreach (var s in keys)
                {
                    if (s == key)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 同一Section下添加多个Key\Value
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keyList"></param>
        /// <param name="valueList"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool AddSectionWithKeyValues(string section, List<string> keyList, List<string> valueList, string path)
        {
            bool bRst = true;
            //判断Section是否已经存在，如果存在，返回false
            //已经存在，则更新
            //if (GetAllSectionNames(path).Contains(section))
            //{
            //    return false;
            //}
            //判断keyList中是否有相同的Key，如果有，返回false

            //添加配置信息
            for (int i = 0; i < keyList.Count; i++)
            {
                WriteValue(section, keyList[i], valueList[i], path);
            }
            return bRst;
        }
        #endregion

        #region  序列化

        //注意：序列化需要在需序列化的对像前设置属性，如下所示
        //[Serializable]
        //private struct structTest { public byte one; public int two; public string three; }

        /// <summary>
        /// 序列化至TXT文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="source">序列化对象</param>
        /// <param name="mode">指定操作系统打开文件的方式</param>
        /// <param name="access">定义用于控制对文件的读访问、写访问或读/写访问的常数</param>
        /// <param name="share">包含用于控制其他 System.IO.FileStream 对象对同一文件可以具有的访问类型的常数</param>
        /// <returns></returns>
        public static void SerializeToTxt(string path, object source, FileMode mode, FileAccess access, FileShare share)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writeFileStream = new FileStream(path, mode, access, share);
            formatter.Serialize(writeFileStream, source);
            writeFileStream.Close();
        }

        /// <summary>
        /// 从TXT文件反序列化
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="mode">指定操作系统打开文件的方式</param>
        /// <param name="access">定义用于控制对文件的读访问、写访问或读/写访问的常数</param>
        /// <param name="share">包含用于控制其他 System.IO.FileStream 对象对同一文件可以具有的访问类型的常数</param>
        /// <returns></returns>
        public static object DeserializationFromTxt(string path, FileMode mode, FileAccess access, FileShare share)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream readFileStream = new FileStream(path, mode, access, share);
            object temp = (object)formatter.Deserialize(readFileStream);
            readFileStream.Close();
            return temp;
        }

        #endregion
    }
}
