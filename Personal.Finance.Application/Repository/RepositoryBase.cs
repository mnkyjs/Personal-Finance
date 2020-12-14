using Microsoft.EntityFrameworkCore;
using Personal.Finance.Application.Interface;
using Personal.Finance.Database;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Personal.Finance.Application.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext DatabaseContext { get; set; }

        public RepositoryBase(DataContext repositoryContext)
        {
            DatabaseContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        {
            return DatabaseContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return DatabaseContext.Set<T>().Where(expression).AsNoTracking();
        }
        public void Create(T entity)
        {
            DatabaseContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            DatabaseContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            DatabaseContext.Set<T>().Remove(entity);
        }
    }
}
