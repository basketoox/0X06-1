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
    /// 或逻辑规范
    /// </summary>
    /// <typeparam name="T">检查特定的实体类型</typeparam>
    public sealed class OrSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {
        #region Members

        private readonly ISpecification<T> _leftSideSpecification;
        private readonly ISpecification<T> _rightSideSpecification;

        #endregion

        #region Public Constructor

        /// <summary>
        /// 默认构造函数与逻辑规范
        /// </summary>
        /// <param name="leftSide">左侧规范</param>
        /// <param name="rightSide">右侧规范</param>
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == null)
                throw new ArgumentNullException("leftSide");

            if (rightSide == null)
                throw new ArgumentNullException("rightSide");

            _leftSideSpecification = leftSide;
            _rightSideSpecification = rightSide;
        }

        #endregion

        #region Composite Specification overrides

        /// <summary>
        /// 左侧规范
        /// </summary>
        public override ISpecification<T> LeftSideSpecification
        {
            get { return _leftSideSpecification; }
        }

        /// <summary>
        /// 右侧规范
        /// </summary>
        public override ISpecification<T> RightSideSpecification
        {
            get { return _rightSideSpecification; }
        }

        /// <summary>
        /// <see cref="ISpecification{TEntity}"/>
        /// </summary>
        /// <returns><see cref="ISpecification{TEntity}"/></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = _leftSideSpecification.SatisfiedBy();
            var right = _rightSideSpecification.SatisfiedBy();

            return (left.Or(right));
        }

        #endregion
    }
}