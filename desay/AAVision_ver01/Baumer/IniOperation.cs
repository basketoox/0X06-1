using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace desay
{
    class IniOperation
    {
        /// <summary>  
        /// 读取INI文件中指定的Key的值  
        /// </summary>  
        /// <param name="secName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>  
        /// <param name="keyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>  
        /// <param name="lpDefault">读取失败时的默认值</param>  
        /// <param name="lpReturnedString">StringBuilder内容缓冲区</param>  
        /// <param name="nSize">内容缓冲区的长度</param>  
        /// <param name="iniFile">INI文件名</param>  
        /// <returns>实际读取到的长度</returns>  
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileString(string secName, string keyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string iniFile);

        /// <summary>  
        /// 将指定的键和值写到指定的节点，如果已经存在则替换  
        /// </summary>  
        /// <param name="secName">节点名称</param>  
        /// <param name="keyName">键名称。如果为null，则删除指定的节点及其所有的项目</param>  
        /// <param name="lpString">值内容。如果为null，则删除指定节点中指定的键。</param>  
        /// <param name="iniFile">INI文件</param>  
        /// <returns>操作是否成功</returns>  
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string secName, string keyName, string lpString, string iniFile);

        public static void WriteStringValue(string iniFile, string secName, string keyName, string lpString)
        {
            WritePrivateProfileString(secName, keyName, lpString, iniFile);
        }

        /// <summary>  
        /// 读取INI文件中指定KEY的字符串型值  
        /// </summary>  
        /// <param name="iniFile">Ini文件</param>  
        /// <param name="secName">节点名称</param>  
        /// <param name="keyName">键名称</param>  
        /// <param name="defaultValue">如果没此KEY所使用的默认值</param>  
        /// <returns>读取到的值</returns>  
        public static string ReadStringValue(string iniFile, string secName, string keyName, string defaultValue)
        {
            string value = defaultValue;
            const int SIZE = 1024 * 10;

            if (string.IsNullOrEmpty(secName))
            {
                throw new ArgumentException("必须指定节点名称", "secName");
            }

            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("必须指定键名称(keyName)", "keyName");
            }

            StringBuilder sb = new StringBuilder(SIZE);
            uint bytesReturned = GetPrivateProfileString(secName, keyName, defaultValue, sb, SIZE, iniFile);

            if (bytesReturned != 0)
            {
                value = sb.ToString();
            }
            sb = null;

            return value;
        }

        public static int ReadIntValue(string iniFile, string secName, string keyName, int defaultValue)
        {
            string value = ReadStringValue(iniFile, secName, keyName, defaultValue.ToString());
            int returnValue = int.Parse(value);
            return returnValue;
        }
        public static void WriteIntValue(string iniFile, string secName, string keyName, int lpInt)
        {
            string lpString = lpInt.ToString();
            WritePrivateProfileString(secName, keyName, lpString, iniFile);
        }
        public static uint ReadUIntValue(string iniFile, string secName, string keyName, uint defaultValue)
        {
            string value = ReadStringValue(iniFile, secName, keyName, defaultValue.ToString());
            uint returnValue = uint.Parse(value);
            return returnValue;
        }
        public static void WriteUIntValue(string iniFile, string secName, string keyName, uint lpInt)
        {
            string lpString = lpInt.ToString();
            WritePrivateProfileString(secName, keyName, lpString, iniFile);
        }
        public static double ReadDoubleValue(string iniFile, string secName, string keyName, double defaultValue)
        {
            string value = ReadStringValue(iniFile, secName, keyName, defaultValue.ToString());
            double returnValue = double.Parse(value);
            return returnValue;
        }
        public static void WriteDoubleValue(string iniFile, string secName, string keyName, double lpDouble)
        {
            string lpString = lpDouble.ToString();
            WritePrivateProfileString(secName, keyName, lpString, iniFile);
        }
        public static float ReadFloatValue(string iniFile, string secName, string keyName, float defaultValue)
        {
            string value = ReadStringValue(iniFile, secName, keyName, defaultValue.ToString());
            float returnValue = float.Parse(value);
            return returnValue;
        }
        public static void WriteFloatValue(string iniFile, string secName, string keyName, float lpFloat)
        {
            string lpString = lpFloat.ToString();
            WritePrivateProfileString(secName, keyName, lpString, iniFile);
        }

        public static bool ReadBoolValue(string iniFile, string secName, string keyName, bool defaultValue)
        {
            string value = ReadStringValue(iniFile, secName, keyName, defaultValue.ToString());
            bool returnValue = bool.Parse(value);
            return returnValue;
        }
        public static void WriteBoolValue(string iniFile, string secName, string keyName, bool lpBool)
        {
            string lpString = lpBool.ToString();
            WritePrivateProfileString(secName, keyName, lpString, iniFile);
        }
    }
}
