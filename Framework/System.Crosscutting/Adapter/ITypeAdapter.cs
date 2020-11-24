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

namespace System.Crosscutting.Adapter
{
    /// <summary>
    ///为map dto到聚合或聚合到dto的基本契约。
    ///     <remarks>
    ///这是一份与“自动”制图者(自动制图者、发射器制图者、valueinjecter…)合作的合同。
    ///或临时制图者
    ///     </remarks>
    /// </summary>
    public interface ITypeAdapter
    {
        /// <summary>
        ///     将源对象调整为类型的实例<typeparamref name="TTarget" />
        /// </summary>
        /// <typeparam name="TSource">源项类型</typeparam>
        /// <typeparam name="TTarget">目标项目类型</typeparam>
        /// <param name="source">实例适应</param>
        /// <returns><paramref name="source" /> mapped to <typeparamref name="TTarget" /></returns>
        TTarget Adapt<TSource, TTarget>(TSource source)
            where TTarget : class, new()
            where TSource : class;


        /// <summary>
        ///     使源对象适应类型的实例 <typeparamref name="TTarget" />
        /// </summary>
        /// <typeparam name="TTarget">目标项目类型</typeparam>
        /// <param name="source">适配实例</param>
        /// <returns><paramref name="source" /> mapped to <typeparamref name="TTarget" /></returns>
        TTarget Adapt<TTarget>(object source)
            where TTarget : class, new();

        /// <summary>
        ///     将源值注入目标
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">TSource实例</param>
        /// <param name="target">TTarget实例</param>
        void Adapt<TSource, TTarget>(TSource source, TTarget target)
            where TTarget : class
            where TSource : class;
    }
}