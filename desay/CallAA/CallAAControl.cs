using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desay
{
    public class CallAAControl
    {

        public void CallAAdlg()
        {
            CallAA.ShowAAFuntionDlg();
            IntPtr DlgHandle = CallAA.GetAAFuntionDlgHwnd();
            CallAA.ShowWindow(DlgHandle, 1);//1为显示，0为隐藏


            CallAA.SetWindowPos(DlgHandle, 0, 0, 0, 560, 800, 0);
        }


    }
}
