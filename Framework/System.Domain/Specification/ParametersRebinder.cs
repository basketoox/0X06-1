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
using System.Linq.Expressions;

namespace System.Domain.Specification
{
    /// <summary>
    ///在表达式中不使用调用方法的rebinder参数的助手
    ///(该方法在所有linq查询提供程序中不受支持，例如在Linq2Entities中不受支持)
    /// </summary>
    public sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="map">映射规格</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        ///用映射信息替换表达式中的参数
        /// </summary>
        /// <param name="map">映射信息</param>
        /// <param name="exp">替换参数表达式</param>
        /// <returns>替换参数表达式</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
                                                   Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// 访问模式方法
        /// </summary>
        /// <param name="p">一个参数表达式</param>
        /// <returns>新访问表达式</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}