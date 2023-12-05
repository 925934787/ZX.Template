using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ZX.Template.Core.Entities
{
    public abstract class BaseEntity : ISoftDelete
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(InsertServerTime = true, IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }

        [SugarColumn(InsertServerTime = true, UpdateServerTime = true)]
        public DateTime UpdateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
