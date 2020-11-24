using System;
using System.Threading;

namespace Motion.Enginee
{
    public static class WaitHandleHelper
    {
        public static bool SignalAndWait(this WaitHandle toSignal, WaitHandle toWaitOn, TimeSpan timeout,
            bool exitContext)
        {
            return WaitHandle.SignalAndWait(toSignal, toWaitOn, timeout, exitContext);
        }

        public static bool SignalAndWait(this WaitHandle toSignal, WaitHandle toWaitOn, int millisecondsTimeout,
            bool exitContext)
        {
            return WaitHandle.SignalAndWait(toSignal, toWaitOn, millisecondsTimeout, exitContext);
        }

        public static bool SignalAndWait(this WaitHandle toSignal, WaitHandle[] toWaitOns, TimeSpan timeout,
            bool exitContext)
        {
            throw new NotImplementedException();
        }

        public static bool SignalAndWait(this WaitHandle toSignal, WaitHandle[] toWaitOn, int millisecondsTimeout,
            bool exitContext)
        {
            throw new NotImplementedException();
        }
    }
}