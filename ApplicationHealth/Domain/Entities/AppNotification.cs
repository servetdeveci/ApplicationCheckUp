using ApplicationHealth.Domain.Enums;
using Shared.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.Domain.Entities
{
    public class AppNotification : EntityBase
    {
        public int AppNotificationId { get; set; }
        public int AppNotificationContactId { get; set; }
        public uint AppDefId { get; set; }
        [StringLength(250)]
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public AppContact Contact { get; set; }
        public AppDef AppDeff { get; set; }

    }
}
