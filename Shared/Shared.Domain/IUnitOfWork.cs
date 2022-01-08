using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task<int> CommitAsync();
        int Commit();
        TContext Context { get; }
    }
}
