using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Device
{
    public class Comm
    {
        //字符转ASCII码：
        public static byte[] Asc(string character)
        {

            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            return asciiEncoding.GetBytes(character);

        }
        //ascii码转字符串
        public static string Chr(byte[] asciiCode)
        {

            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();

            return asciiEncoding.GetString(asciiCode);

        }
        //和校验
        public static byte CheckBySum(byte[] asc)
        {
            byte result = 0;
            foreach (byte i in asc)
            {
                result = (byte)(result + i);

            }
            return result;
        }
        //异或校验
        public static byte CheckByXor(byte[] asc)
        {
            byte result = 0;
            foreach (byte i in asc)
            {
                result = (byte)(result ^ i);

            }
            return result;

        }
        
    }
}
