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
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace System.Domain.Specification
{
    /// <summary>
    /// 用于添加与和或参数的扩展方法
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// 组合两个表达式并合并到一个新的表达式中
        /// </summary>
        /// <typeparam name="T">表达式中参数的类型</typeparam>
        /// <param name="first">表达实例</param>
        /// <param name="second">表达式合并</param>
        /// <param name="merge">功能合并</param>
        /// <returns>新合并的表达式</returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            if (merge == null) { throw new ArgumentNullException("merge"); }

            // 构建参数映射(从第二参数到第一参数)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // 用第一个参数替换第二个lambda表达式中的参数
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            // 对第一个表达式中的参数应用lambda表达式体的组合
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// 与操作
        /// </summary>
        /// <typeparam name="T">表达式中参数的类型</typeparam>
        /// <param name="first">右侧与操作的表达式</param>
        /// <param name="second">左侧与操作的表达式</param>
        /// <returns>新的与表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
                                                       Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// 或操作
        /// </summary>
        /// <typeparam name="T">表达式中的参数类型</typeparam>
        /// <param name="first">右侧或操作的表达式</param>
        /// <param name="second">左侧或操作的表达式</param>
        /// <returns>新的或表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
                                                      Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}