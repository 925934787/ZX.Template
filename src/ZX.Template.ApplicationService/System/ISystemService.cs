using AntDesign.ProLayout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZX.Template.ApplicationService.System
{
    public interface ISystemService
    {
        void CodeFirst();
        List<MenuDataItem> GetIconSideMenuItems();
        List<string> GetTable();
        Task<Dictionary<string, DataTable>> GetTableData();
        Task Publish();
    }
}
