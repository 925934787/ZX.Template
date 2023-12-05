using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZX.Template.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppStartupAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="order"></param>
        public AppStartupAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
