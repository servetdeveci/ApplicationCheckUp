using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Domain;
using Shared.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public class RepositoryBase<T, TContext> : IRepository<T>
    where T : EntityBase
    where TContext : DbContext
    {
        private readonly DbFactoryBase<TContext> _dbFactory;
        protected DbSet<T> _dbSet;
        protected DbSet<T> DbSet => _dbSet ?? (_dbSet = _dbFactory.Context.Set<T>());
        public RepositoryBase(DbFactoryBase<TContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        #region CRUD
        public IQueryable<T> Table => DbSet;
        public T GetById(int id)
        {
            return DbSet.Find(id);
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }
        public void Update(T entity)
        {
            DbSet.Attach(entity);
            _dbFactory.Context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                Delete(entity, true);
            }
        }
        public void Delete(T entity, bool realDelete = true)
        {
            if (!realDelete)
            {
                if (entity.GetType().GetProperty("IsDeleted") != null)
                {
                    T _entity = entity;
                    _entity.GetType().GetProperty("IsDeleted")?.SetValue(_entity, true);
                    Update(_entity);
                }
                else
                {
                    throw new Exception("IsDeleted alanı bulunamadı");
                }
            }
            else
            {
                EntityEntry<T> deletedEntity = _dbFactory.Context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
            }
        }
        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? DbSet.ToList() : DbSet.Where(filter).ToList();
        }
        public void AddRange(List<T> entities)
        {
            DbSet.AddRange(entities);
        }
        public void RemoveRange(List<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
        public void RemoveRange(Expression<Func<T, bool>> filter = null)
        {
            var deleted = filter == null ? DbSet : DbSet.Where(filter);
            _dbFactory.Context.RemoveRange(deleted);
        }
        public int CountAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? DbSet.Count() : DbSet.Count(filter);
        }
        public bool Any(Expression<Func<T, bool>> filter)
        {
            return DbSet.Any(filter);
        }
        public double Sum(Expression<Func<T, double>> filter = null)
        {
            return DbSet.Sum(filter);
        }
        public double Max(Expression<Func<T, double>> filter = null)
        {
            return DbSet.Max(filter);
        }
        public double Min(Expression<Func<T, double>> filter = null)
        {
            return DbSet.Min(filter);
        }
        public double Average(Expression<Func<T, double>> filter = null)
        {
            return DbSet.Average(filter);
        }

        #endregion

        #region AsyncCRUD     
        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task AddRangeAsync(List<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }
        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }
        public async Task DeleteAsync(T entity, bool realDelete = false)
        {
            await Task.Run(async () =>
            {
                if (!realDelete)
                {
                    if (entity.GetType().GetProperty("IsDeleted") != null)
                    {
                        T _entity = entity;
                        _entity.GetType().GetProperty("IsDeleted")?.SetValue(_entity, true);
                        await this.UpdateAsync(_entity);
                    }
                    else
                    {
                        throw new Exception("IsDeleted alanı bulunamadı");
                    }
                }
                else
                {
                    EntityEntry<T> deletedEntity = _dbFactory.Context.Entry(entity);
                    deletedEntity.State = EntityState.Deleted;
                }
            });
        }
        public async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                var entity = this.GetById(id);
                if (entity == null) return;
                else
                {
                    if (entity.GetType().GetProperty("IsDelete") != null)
                    {
                        T _entity = entity;
                        _entity.GetType().GetProperty("IsDelete").SetValue(_entity, true);

                        this.Update(_entity);
                    }
                    else
                    {
                        Delete(entity);
                    }
                }
            });

        }
        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() =>
            {
                DbSet.Attach(entity);
                _dbFactory.Context.Entry(entity).State = EntityState.Modified;
            });
        }
        public async Task<int> CountAllAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await DbSet.CountAsync();
            }
            return await DbSet.CountAsync(filter);

        }
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await DbSet.ToListAsync();
            }
            return await DbSet.Where(filter).ToListAsync();

        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await DbSet.FirstOrDefaultAsync(filter);
        }
        public async Task RemoveRangeAsync(List<T> entities)
        {
            await Task.Run(() =>
            {
                DbSet.RemoveRange(entities);
            });
        }
        public async Task RemoveRangeAsync(Expression<Func<T, bool>> filter = null)
        {
            await RemoveRangeAsync(GetAll(filter));
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await DbSet.AnyAsync(filter);
        }

        #endregion
    }
}
