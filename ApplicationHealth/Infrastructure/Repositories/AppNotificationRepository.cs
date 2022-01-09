using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.EntityInterfaces;
using ApplicationHealth.Infrastructure;

namespace ApplicationHealth.Domain
{
    public class AppNotificationRepository : AppRepository<AppNotification>, IAppNotificationRepository
    {
        public AppNotificationRepository(AppDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
