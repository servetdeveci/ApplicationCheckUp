using ApplicationHealth.Domain;
using ApplicationHealth.Infrastructure.Context;
using Shared.Infrastructure;

namespace ApplicationHealth.Infrastructure
{
    public class AppUnitOfWork : UnitOfWorkBase<AppDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(AppDbFactory dbFactory) : base(dbFactory)
        {
            dbFactory.Context.Database.EnsureCreated();
        }
    }
}
