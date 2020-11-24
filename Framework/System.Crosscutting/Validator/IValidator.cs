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
using System.Collections.Generic;

namespace System.Crosscutting.Validator
{
    /// <summary>
    /// 验证器基本契约
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 如果状态有效，则执行验证并返回
        /// </summary>
        /// <typeparam name="T">要验证的对象类型</typeparam>
        /// <param name="item">要验证的实例</param>
        /// <returns>如果对象状态有效，则为真</returns>
        bool IsValid<T>(T item) where T : class;

        /// <summary>
        /// 如果状态无效，返回错误集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="item">带有验证错误的实例</param>
        /// <returns>验证错误的集合</returns>
        IEnumerable<String> GetInvalidMessages<T>(T item)
            where T : class;
    }
}