using ApplicationHealth.Infrastructure.Context;
using Shared.Infrastructure;
using System;

namespace ApplicationHealth.Infrastructure
{
    public class AppDbFactory : DbFactoryBase<AppDbContext>
    {
        public AppDbFactory(Func<AppDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}
