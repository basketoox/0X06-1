using System;

namespace Motion.AdlinkDash
{
    /// <summary>
    ///     Adlink Dask Exception
    /// </summary>
    public class DaskException : ApplicationException
    {
        public DaskException()
        {
        }

        public DaskException(string message)
            : base(message)
        {
        }

        public DaskException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}