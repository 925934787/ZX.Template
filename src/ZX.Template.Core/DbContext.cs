using SqlSugar;
using System.Collections.Generic;
using ZX.Template.Core.Entities;

namespace ZX.Template.Core
{
    /// <summary>
    /// 数据库上下文对象
    /// </summary>
    public static class DbContext
    {
        public static List<ConnectionConfig> ConnectionConfigs { get; set; } = new();

        /// <summary>
        /// SqlSugar 数据库实例
        /// </summary>
        public static readonly SqlSugarScope Instance = new(
            // 读取 appsettings.json 中的 ConnectionConfigs 配置节点
            ConnectionConfigs
            , db =>
            {
                // 这里配置全局事件，比如拦截执行 SQL
                db.QueryFilter.AddTableFilter<ISoftDelete>(it => it.IsDeleted == false);

            });
    }
}