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

using System.Collections.Generic;
using System.Platform.Resources;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 验证错误的自定义异常
    /// </summary>
    public class ValidationErrorsException
        : Exception
    {
        #region Properties

        private readonly IEnumerable<string> _validationErrors;

        /// <summary>
        /// 获取或设置验证错误消息
        /// </summary>
        public IEnumerable<string> ValidationErrors
        {
            get { return _validationErrors; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 创建应用程序验证错误异常的新实例
        /// </summary>
        /// <param name="validationErrors">验证错误的集合</param>
        public ValidationErrorsException(IEnumerable<string> validationErrors)
            : base(Messages.exception_ApplicationValidationExceptionDefaultMessage)
        {
            _validationErrors = validationErrors;
        }

        #endregion
    }
}