using System;
using System.Linq;

namespace ApplicationHealth.Domain.Enums
{
    public enum NotificationType : byte
    {
        None = 0,
        Email,
        Sms,
        EmailSms
    }
    
}
