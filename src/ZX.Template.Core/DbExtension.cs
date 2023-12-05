using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZX.Template.Core.Entities;

namespace ZX.Template.Core
{
    public static class DbExtension
    {
        public static void FalseDelete<T>(this ISqlSugarClient db, Expression<Func<T, bool>> exp) where T : class, ISoftDelete, new()//约束3个不能少
        {
            db.Updateable<T>()
                    .SetColumns(it => new T() { IsDeleted = true },
                     true)//true 支持更新数据过滤器赋值字段一起更新
                    .Where(exp).ExecuteCommand();
        }


        public static async Task FalseDeleteAsync<T>(this ISqlSugarClient db, Expression<Func<T, bool>> exp) where T : class, ISoftDelete, new()//约束3个不能少
        {
            await db.Updateable<T>()
                     .SetColumns(it => new T() { IsDeleted = true },
                      true)//true 支持更新数据过滤器赋值字段一起更新
                     .Where(exp).ExecuteCommandAsync();
        }
    }
}
