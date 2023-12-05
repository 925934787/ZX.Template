using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZX.Template.Core.Entities;
using ZX.Template.Core;
using ZX.Template.Core.Extensions;
using AntDesign.ProLayout;
using MediatR;
using System.Data;

namespace ZX.Template.ApplicationService.System
{
    public class SystemService : ISystemService, ITransient
    {

        readonly ISqlSugarClient db;
        private readonly IMediator _mediat;


        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public SystemService(IMediator mediat)
        {
            db = DbContext.Instance;
            _mediat = mediat;
            //ax = DbContext.Instance.AsTenant().GetConnection("ax");
        }


        public List<MenuDataItem> GetIconSideMenuItems()
        {
            var menus = new List<MenuDataItem>
            {
                new (){Path = "/",Name = "welcome",Key = "welcome",Icon = "home",},
                new (){Path = "/weather",Name = "weather",Key = "weather",Icon = "smile",},
                new (){Path = "/dev",Name = "开发配置",Key = "dev",Icon = "code", },
            };

            return menus;
        }

        public List<string> GetTable()
        {
            return db.DbMaintenance.GetTableInfoList().Select(x => x.Name).ToList();
        }


        public async Task<Dictionary<string, DataTable>> GetTableData()
        {
            Dictionary<string, DataTable> tableData = new();

            var tables = db.DbMaintenance.GetTableInfoList().Select(x => x.Name).ToList();

            foreach (var item in tables)
            {
                var datas =  await db.Queryable<object>().AS(item).OrderBy("id desc").Take(10).ToDataTableAsync();
                tableData.Add(item, datas);
            }

            return tableData;

        }


        public async Task Publish()
        {
            foreach (var item in Enumerable.Range(1, 3))
            {
                _ = _mediat.Publish(new PartNosDto { Name = $"测试{item}" });
            }

            await Task.CompletedTask;
        }

        public void CodeFirst()
        {
            DbContext.Instance.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices
            {
                //注意:  这儿AOP设置不能少
                EntityService = (c, p) =>
                {
                    ///***低版本C#写法***/
                    //// int?  decimal?这种 isnullable=true 不支持string(下面.NET 7支持)
                    //if (p.IsPrimarykey == false && c.PropertyType.IsGenericType &&
                    //c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    //{
                    //    p.IsNullable = true;
                    //}

                    /***高版C#写法***/
                    //支持string?和string  
                    if (p.IsPrimarykey == false && new NullabilityInfoContext()
                     .Create(c).WriteState is NullabilityState.Nullable)
                    {
                        p.IsNullable = true;
                    }
                }
            };

            //选择所有表
            Type[] types = Assembly.LoadFrom($"{AppDomain.CurrentDomain.BaseDirectory}\\ZX.Template.Core.dll")//如果 .dll报错，可以换成 xxx.exe 有些生成的是exe 
                                    .GetTypes().Where(t => typeof(BaseEntity).IsAssignableFrom(t) && !t.FullName.Contains("BaseEntity"))//命名空间过滤，当然你也可以写其他条件过滤
                                    .ToArray();//断点调试一下是不是需要的Type，不是需要的在进行过滤

            DbContext.Instance.CodeFirst.SetStringDefaultLength(200).InitTables(types);//根据types创建表

        }


    }
}
