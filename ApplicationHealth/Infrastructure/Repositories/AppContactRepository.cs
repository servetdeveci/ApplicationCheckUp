using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.EntityInterfaces;
using ApplicationHealth.Infrastructure;

namespace ApplicationHealth.Domain
{
    public class AppContactRepository : AppRepository<AppContact>, IAppContactRepository
    {
        public AppContactRepository(AppDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
