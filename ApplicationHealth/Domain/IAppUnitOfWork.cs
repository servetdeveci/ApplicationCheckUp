using System.Threading.Tasks;

namespace ApplicationHealth.Domain
{
    public interface IAppUnitOfWork
    {
        Task<int> CommitAsync();
        int Commit();
    }
}
