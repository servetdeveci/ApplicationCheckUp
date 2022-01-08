using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain
{
    public interface IDbFactory<TContext> where TContext : DbContext
    {
        TContext Context { get; }
    }
}
