using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using System.Collections.Generic;

namespace ApplicationHealth.Domain.DataTable
{
    public class AppDefDataTable : BaseDataTable
    {
        public List<AppDef> data { get; set; }
    }
}
