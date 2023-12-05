using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZX.Template.Core.Entities;

namespace ZX.Template.ApplicationService.System
{
    public class PartNosDto : INotification
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PartNosMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<PartNosDto, PartNos>()
                .Map(dest => dest.Name, src => src.Name + "Mapster");
        }
    }
}
