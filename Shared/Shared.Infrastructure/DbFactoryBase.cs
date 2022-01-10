using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure
{
    public class DbFactoryBase<TContext> : IDbFactory<TContext> where TContext : DbContext
    {
        private Func<TContext> _instanceFunc;
        private TContext _context;
        public TContext Context => _context ?? (_context = _instanceFunc.Invoke());
        public DbFactoryBase(Func<TContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }
        
    }
}
