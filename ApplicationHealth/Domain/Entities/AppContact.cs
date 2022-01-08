using ApplicationHealth.Domain.Enums;
using Shared.Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.Domain.Entities
{
    public class AppContact : EntityBase
    {
        public int AppNotificationContactId { get; set; }
        public uint AppDefId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Posta Adresi")]
        [StringLength(100)]
        public string Email { get; set; }
        [Display(Name = "Telefon Numarası")]
        [StringLength(10)]
        public string Phone { get; set; }
        public NotificationType NotificationType { get; set; }
        public AppDef AppDef { get; set; }
        public IEnumerable<AppNotification> Notifications { get; set; }



    }
}
