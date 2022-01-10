using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using System.Collections.Generic;

namespace ApplicationHealth.Domain.DataTable
{
    public class AppNotificationDataTable : BaseDataTable
    {
        public List<AppNotification> data { get; set; }
    }
}
