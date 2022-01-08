using Shared.Domain;
using Shared.Domain.Base;

namespace ApplicationHealth.Domain
{
    public interface IAppRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
    }
}
