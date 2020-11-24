using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
namespace desay
{
    public class CallWb
    {
        public const string str_Whitedll_file = @".\WhitePanel\AAImage.dll";
        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestAAImageDll")]
        public static extern void TestAAImageDll();

        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowAAImageDlg")]
        public static extern void ShowAAImageDlg();

        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ExitAAImageDlg")]
        public static extern void ExitAAImageDlg();

        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetAAImageDlgHwnd")]
        public static extern IntPtr GetAAImageDlgHwnd();

        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "StartAAImage")]
        public static extern void StartAAImage(string szBarcode, string szJigSN);

        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetAAImageTestResult")]
        public static extern Int32 GetAAImageTestResult(StringBuilder szTestData);

        [DllImport(str_Whitedll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetAAImageStatus")]
        public static extern Int32 GetAAImageStatus();


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        
    }
}
