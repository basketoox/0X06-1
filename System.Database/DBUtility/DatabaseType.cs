using System;
using System.Collections.Generic;
using System.Text;

namespace System.Database.DBUtility
{
    /// <summary>
    /// 数据库类型枚举，需要扩展类型可在此添加
    /// </summary>
    public enum DatabaseType
    {
        SQLSERVER,
        ORACLE,
        ACCESS,
        MYSQL
    }
}
