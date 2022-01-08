using Shared.Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.Domain.Entities
{
    public class AppDef : AuditEntity
    {
        public uint AppDefId { get; set; }
        [StringLength(16)]
        [Display(Name="Uygulama Adı")]
        public string Name { get; set; }
        /// <summary>
        /// kontrol edilecek uygulamanın hostname alanıdır.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Dakika cinsinden bir interval
        /// </summary>
        public ushort Interval { get; set; }
        public IEnumerable<AppContact> NotificationContacts { get; set; }
        public IEnumerable<AppNotification> Notifications { get; set; }
    }
}
