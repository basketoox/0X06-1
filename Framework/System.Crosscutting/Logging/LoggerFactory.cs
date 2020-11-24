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


namespace System.Crosscutting.Logging
{
    /// <summary>
    /// 日志工厂模式
    /// </summary>
    public static class LoggerFactory
    {
        #region Members

        private static ILoggerFactory _currentLogFactory;

        #endregion

        #region Public Methods

        /// <summary>
        /// 设置要使用的日志工厂
        /// </summary>
        /// <param name="logFactory">使用日志工厂</param>
        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        /// <summary>
        /// 创建新的日志<ref name="System.Domain.Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <returns>Created ILoger</returns>
        public static ILogger CreateLog()
        {
            return (_currentLogFactory != null) ? _currentLogFactory.Create() : null;
        }

        #endregion
    }
}