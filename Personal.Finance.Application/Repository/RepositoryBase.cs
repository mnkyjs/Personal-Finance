using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Personal.Finance.Application.Interface;
using Personal.Finance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Personal.Finance.Application.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext DatabaseContext { get; set; }

        public RepositoryBase(DataContext repositoryContext)
        {
            DatabaseContext = repositoryContext;
        }
        public void Create(T entity)
        {
            DatabaseContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            DatabaseContext.Set<T>().Update(entity);
        }
        public async Task Delete(int id)
        {
            DatabaseContext.Set<T>().Remove(await DatabaseContext.Set<T>().FindAsync(id).ConfigureAwait(false));
        }

        public async Task<IEnumerable<T>> FindListConditionAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = DatabaseContext.Set<T>().AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (include != null) query = include(query);

            if (orderBy != null)
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> FindByConditionSingleAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = DatabaseContext.Set<T>().AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (include != null) query = include(query);

            if (orderBy != null)
                return await orderBy(query).FirstOrDefaultAsync().ConfigureAwait(false);
            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
