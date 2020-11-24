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
    /// 实体验证器工厂
    /// </summary>
    public static class ValidatorFactory
    {
        #region Members

        private static IValidatorFactory _factory;

        #endregion

        #region Public Methods

        /// <summary>
        /// 设置要使用的日志工厂
        /// </summary>
        /// <param name="factory">Log factory to use</param>
        public static void SetCurrent(IValidatorFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Createt a new <ref name="System.Crosscutting.Validator.IValidator"/>
        /// </summary>
        /// <returns>Created IValidator</returns>
        public static IValidator CreateValidator()
        {
            return (_factory != null) ? _factory.Create() : null;
        }

        #endregion
    }
}