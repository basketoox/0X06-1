
using System.Runtime.InteropServices;
using System.Text;

namespace Ini
{
    public class IniFile
    {
        public string path;

        [DllImport("kernel32",CharSet=CharSet.Ansi)]
        private static extern long WritePrivateProfileString(string section, string key,
                    string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileString(string section, string key, string def,
                    StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string FilePath);
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        public string ReadKey(string Key, string Section)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
        public string ReadSectionData(string Section)
        {
            byte[] buffer = new byte[20480];
            if(GetPrivateProfileSection(Section,buffer,buffer.Length, this.path) >0)
            {
                string str = Encoding.ASCII.GetString(buffer).Trim('\0');
                return str;
            }
            return "";
        }
    }
}
