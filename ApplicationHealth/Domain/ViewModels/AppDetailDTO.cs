using ApplicationHealth.Domain.Entities;
using System.Collections.Generic;

namespace ApplicationHealth.Domain.ViewModels
{
    public class AppDetailDTO
    {
        public AppDef App { get; set; }
        public List<AppContact> Contacts { get; set; }
        public List<AppNotification> Notifications { get; set; }
    }
}
