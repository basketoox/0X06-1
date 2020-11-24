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
using System.Linq.Expressions;

namespace System.Domain.Specification
{
    /// <summary>
    /// 基本契约的规格模式，更多信息
    /// 关于这个模式，请参阅http://martinfowler.com/apsupp/spec.pdf
    /// 或http://en.wikipedia.org/wiki/Specification_pattern。
    /// 这确实是一个变体实现，其中我们添加了Linq和
    /// lambda表达式。
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        /// <summary>
        ///检查这个规范是否符合特定的lambda表达式
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}