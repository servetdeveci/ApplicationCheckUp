using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.EntityInterfaces;
using ApplicationHealth.Infrastructure;

namespace ApplicationHealth.Domain
{
    public class AppDefRepository : AppRepository<AppDef>, IAppDefRepository
    {
        public AppDefRepository(AppDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
