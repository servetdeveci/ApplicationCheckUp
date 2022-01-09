using Shared.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationHealth.Domain.Entities
{
    public class AppDef : AuditEntity
    {
        public int AppDefId { get; set; }
        [StringLength(16)]
        [Display(Name="Uygulama Adı")]
        public string Name { get; set; }
        /// <summary>
        /// kontrol edilecek uygulamanın hostname alanıdır.
        /// </summary>
        [Url]
        public string Url { get; set; }
        /// <summary>
        /// Dakika cinsinden bir interval
        /// </summary>
        [Range(1, short.MaxValue)]
        public short Interval { get; set; } = 1;
        public DateTime LastControlDateTime { get; set; }
        public bool IsUp { get; set; } = false;
        public IEnumerable<AppContact> NotificationContacts { get; set; }
        public IEnumerable<AppNotification> Notifications { get; set; }
    }
}
