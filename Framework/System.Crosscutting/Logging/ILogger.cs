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

using System;

namespace System.Crosscutting.Logging
{
    /// <summary>
    /// 跟踪检测的通用合同。您可以在几个框架中实现这个反向操作。NET诊断API、EntLib、Log4Net、NLog等。
    /// <remarks>
    ///这个抽象的使用取决于您的实际需求和特定的特性
    ///您希望使用特定的现有实现。
    ///您还可以消除这种抽象，并直接在代码中使用“任意”实现，
    ///记录器。在EntLib中写入(new LogEntry())，或使用log4net…等。
    /// </remarks>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="message">The debug message</param>
        /// <param name="args">the message argument values</param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception">在调试消息中写入异常</param>
        /// <param name="args">消息参数值</param>
        void Debug(string message, Exception exception, params object[] args);

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="item">在调试中写入信息的项</param>
        void Debug(object item);

        /// <summary>
        /// 日志致命错误
        /// </summary>
        /// <param name="message">致命错误的消息</param>
        /// <param name="args">消息的参数值</param>
        void Fatal(string message, params object[] args);

        /// <summary>
        /// 日志致命错误
        /// </summary>
        /// <param name="message">致命错误的消息</param>
        /// <param name="exception">在这个致命消息中写入异常</param>
        /// <param name="args">消息的参数值</param>
        void Fatal(string message, Exception exception, params object[] args);

        /// <summary>
        /// 日志信息消息
        /// </summary>
        /// <param name="message">The information message to write</param>
        /// <param name="args">The arguments values</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// 日志警告消息
        /// </summary>
        /// <param name="message">The warning message to write</param>
        /// <param name="args">The argument values</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// 日志错误消息
        /// </summary>
        /// <param name="message">The error message to write</param>
        /// <param name="args">The arguments values</param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// 日志错误消息
        /// </summary>
        /// <param name="message">The error message to write</param>
        /// <param name="exception">The exception associated with this error</param>
        /// <param name="args">The arguments values</param>
        void LogError(string message, Exception exception, params object[] args);
    }
}