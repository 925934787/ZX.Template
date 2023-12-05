using Mapster;
using MediatR;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZX.Template.ApplicationService.System;
using ZX.Template.Core;
using ZX.Template.Core.Entities;

namespace ZX.Template.ApplicationService.MediatRHandlers
{
    public class PartNosHandler : INotificationHandler<PartNosDto>
    {
        ISqlSugarClient db;
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public PartNosHandler()
        {
            db = DbContext.Instance;
        }

        public async Task Handle(PartNosDto notification, CancellationToken cancellationToken)
        {
            var entity= notification.Adapt<PartNos>();
            await db.Insertable(entity).ExecuteCommandAsync();
            logger.Debug($"插入数据库成功{notification.Name}");
        }
    }
}
