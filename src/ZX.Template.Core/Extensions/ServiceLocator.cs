using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZX.Template.Core.Extensions
{
    public static class ServiceLocator
    {
        [AllowNull]
        public static IServiceProvider Instance { get; set; }
    }
}
