//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
using System.Globalization;
using NLog;

namespace System.Crosscutting.Logging
{
    /// <summary>
    /// Implementation of contract <see cref="System.Crosscutting.Logging.ILogger"/>
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class NLogLog
        : System.Crosscutting.Logging.ILogger
    {
        #region Members

        Logger source;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public NLogLog()
        {
            // Create default source
            source = LogManager.GetCurrentClassLogger();
        }

        #endregion

        #region ILogger Members

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void LogInfo(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Info, messageInfo);
            }
        }
        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void LogWarning(string message, params object[] args)
        {

            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Warn, messageInfo);
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void LogError(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Error, messageInfo);
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="exception"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void LogError(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception

                source.Log(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageInfo, exceptionData));
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void Debug(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Debug, messageInfo);
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="exception"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void Debug(string message, Exception exception,params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception
                source.Log(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageInfo, exceptionData));
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="item"><see cref="ILogger"/></param>
        public void Debug(object item)
        {
            if (item != null)
            {
                source.Log(LogLevel.Trace, item.ToString());
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="args"><see cref="ILogger"/></param>
        public void Fatal(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Fatal, messageInfo);
            }
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="exception"><see cref="ILogger"/></param>
        public void Fatal(string message, Exception exception,params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception
                source.Log(LogLevel.Fatal, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageInfo, exceptionData));
            }
        }


        #endregion
    }
}
