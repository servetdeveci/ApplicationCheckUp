using ApplicationHealth.Domain;
using ApplicationHealth.Infrastructure.Context;
using Shared.Domain.Base;
using Shared.Infrastructure;

namespace ApplicationHealth.Infrastructure
{
    public class AppRepository<TEntity> : RepositoryBase<TEntity, AppDbContext>, IAppRepository<TEntity>
        where TEntity : EntityBase
    {
        public AppRepository(AppDbFactory dbFactory) : base(dbFactory)
        {
            dbFactory.Context.Database.EnsureCreated();
        }
    }
}
