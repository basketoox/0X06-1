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
using System.Domain.Specification;
using System.Linq.Expressions;

namespace System.Domain
{
    /// <summary>
    /// 实现“存储库模式”的基本接口
    /// 关于此模式的更多信息请参阅 http://martinfowler.com/eaaCatalog/repository.html
    /// 或 http://blogs.msdn.com/adonet/archive/2009/06/16/using-repository-and-unit-of-work-patterns-with-entity-framework-4-0.aspx
    /// </summary>
    /// <remarks>
    /// 实际上，有人可能认为IDbSet已经是一个通用的存储库，因此
    /// /不需要这个项目。在我们的领域模型中使用这个接口可以确保PI原则
    /// </remarks>
    /// <typeparam name="TEntity">此存储库的实体类型</typeparam>
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        /// <summary>
        /// 获取此存储库中的工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 向存储库中添加项
        /// </summary>
        /// <param name="item">要添加到存储库的项</param>
        void Add(TEntity item);

        /// <summary>
        /// 删除项目        
        /// </summary>
        /// <param name="item">项删除</param>
        void Remove(TEntity item);

        /// <summary>
        /// 设置项目为已修改
        /// </summary>
        /// <param name="item">项目修改</param>
        void Modify(TEntity item);

        /// <summary>
        ///将实体跟踪到这个存储库中，实际上是在UnitOfWork中。
        ///在EF，这可以通过附加和更新NH
        /// </summary>
        /// <param name="item">项附加</param>
        void TrackItem(TEntity item);

        /// <summary>
        ///将修改后的实体设置为存储库。
        ///调用UnitOfWork中的Commit()方法时
        ///这些更改将保存到存储中
        /// </summary>
        /// <param name="persisted">持久化项</param>
        /// <param name="current">当前项</param>
        void Merge(TEntity persisted, TEntity current);

        /// <summary>
        /// 按实体键获取元素
        /// </summary>
        /// <param name="id">实体键值</param>
        /// <returns></returns>
        TEntity Get(Guid id);

        /// <summary>
        /// 在存储库中获取类型为TEntity的所有元素
        /// </summary>
        /// <returns>选定元素列表</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        ///获取与规范<paramref name=" Specification "/>匹配的类型TEntity的所有元素
        /// </summary>
        /// <param name="specification">结果符合规范</param>
        /// <returns></returns>
        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);

        /// <summary>
        /// 在存储库中获取类型为TEntity的所有元素
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageCount">每个页面中的元素数量</param>
        /// <param name="orderByExpression">以这个查询的表达式排序</param>
        /// <param name="ascending">指定顺序是否递增</param>
        /// <returns>选定元素列表</returns>
        IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageCount,
                                                 Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending);

        /// <summary>
        /// 在存储库中获取类型为TEntity的元素
        /// </summary>
        /// <param name="filter">过滤每个元素是否匹配</param>
        /// <returns>选定元素列表</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
    }
}