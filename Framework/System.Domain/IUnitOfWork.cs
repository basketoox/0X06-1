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

namespace System.Domain
{
    /// <summary>
    /// 有关更多信息，请参阅 http://martinfowler.com/eaaCatalog/unitOfWork.html or
    /// http://msdn.microsoft.com/en-us/magazine/dd882510.aspx
    /// 在这个解决方案中，工作单元是使用开箱即用实现的
    /// 实体框架上下文(EF 4.1 DbContext)持久化引擎。但是为了
    /// 遵循我们领域的PI(持久性无关)原则，我们实现了这个接口/合同。
    /// 这个接口/合同应该由与这个域一起使用的任何UoW实现遵守。
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 提交容器中所做的所有更改.
        /// </summary>
        ///<remarks>
        ///如果实体具有固定的属性，并且存在任何乐观并发问题，则抛出异常
        ///</remarks>
        void Commit();

        /// <summary>
        /// 提交容器中所做的所有更改.
        /// </summary>
        ///<remarks>
        ///如果实体具有固定的属性，并且存在任何乐观的并发问题，
        ///则刷新“客户端更改”——客户端获胜
        ///</remarks>
        void CommitAndRefreshChanges();


        /// <summary>
        /// 回滚跟踪变化。参见UnitOfWork模式参考
        /// </summary>
        void RollbackChanges();
    }
}