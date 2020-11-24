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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace System.Domain.Specification
{
    /// <summary>
    /// 表示表达式规范
    /// <remarks>
    ///规范重载操作符，用于创建和或不创建规范。
    ///另外重载和和运算符具有相同的意义(二进制和二进制或)。
    /// c#不能直接重载AND和或运算符，因为框架不允许这种疯狂。但
    ///对于重载的假操作符和真操作符，这是可能的。要解释这种行为，请阅读
    /// http://msdn.microsoft.com/en-us/library/aa691312(VS.71).aspx
    /// </remarks>
    /// </summary>
    /// <typeparam name="TValueObject">标准中的项目类型</typeparam>
    public abstract class Specification<TEntity>
        : ISpecification<TEntity>
        where TEntity : class
    {
        #region ISpecification<TEntity> Members

        /// <summary>
        /// 满足规范模式法
        /// </summary>
        /// <returns>满足此规范的表达式</returns>
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();

        #endregion

        #region Override Operators

        /// <summary>
        ///  与操作
        /// </summary>
        /// <param name="leftSideSpecification">在与操作中的左侧运算对象</param>
        /// <param name="rightSideSpecification">在与操作中的右侧运算对象</param>
        /// <returns>新的规范</returns>
        public static Specification<TEntity> operator &(
            Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// 或操作
        /// </summary>
        /// <param name="leftSideSpecification">在或操作中的左侧运算对象</param>
        /// <param name="rightSideSpecification">在或操作中的右侧运算对象</param>
        /// <returns>新的规范 </returns>
        public static Specification<TEntity> operator |(
            Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// 非规范
        /// </summary>
        /// <param name="specification">规范否定</param>
        /// <returns>新的规范</returns>
        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        /// 重载操作符false，仅用于支持与或操作符
        /// </summary>
        /// <param name="specification">规范实例</param>
        /// <returns>查看c#中的False运算符</returns>
        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }

        /// <summary>
        /// 重载操作符True，仅用于支持与或操作符
        /// </summary>
        /// <param name="specification">规范实例</param>
        /// <returns>查看c#中的False运算符</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specification")]
        public static bool operator true(Specification<TEntity> specification)
        {
            return false;
        }

        #endregion
    }
}