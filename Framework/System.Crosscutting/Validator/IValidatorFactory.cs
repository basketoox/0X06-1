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

namespace System.Crosscutting.Validator
{
    /// <summary>
    /// 验证器抽象工厂的基本契约
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Create a new IValidator
        /// </summary>
        /// <returns>IValidator</returns>
        IValidator Create();
    }
}