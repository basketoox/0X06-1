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
    public class CallAA
    {
        //public const string str_AAFuntion_file = "DesayActiveAlignment.dll";


        public const string str_AAFuntion_file = @".\AA\DesayActiveAlignment.dll";
        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowAAFuntionDlg")]
        public static extern void ShowAAFuntionDlg();

        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ExitAAFuctionDlg")]
        public static extern void ExitAAFuctionDlg();

        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetAAFuntionDlgHwnd")]
        public static extern IntPtr GetAAFuntionDlgHwnd();

        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestAAFuntionDll")]
        public static extern void TestAAFuntionDll();


        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetTestResult")]
        public static extern Int32 GetTestResult(StringBuilder szTestData);

        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "StartAAFunction")]
        public static extern void StartAAFunction(string szBarcode, string szJigSN);

        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "bResetHome")]
        public static extern bool bResetHome();

        //         [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetBarcode")]
        //         public static extern void SetBarcode(string szBarcode, string szJigSN);

        [DllImport(str_AAFuntion_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetAAStatus")]
        public static extern Int32 GetAAStatus();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        
    }
}
