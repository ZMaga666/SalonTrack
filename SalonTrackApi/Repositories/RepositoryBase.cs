using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Data;
using SalonTrackApi.Repository.Contract;
using System.Linq.Expressions;

namespace SalonTrackApi.Repositories
{

    public class RepositoryBase<T>(AppDbContext repositoryContext) : IRepositoryBase<T> where T : class
    {
        protected AppDbContext RepositoryContext = repositoryContext;

        public IQueryable<T> FindAll(bool trackChanges, string includeProperties = "")
        {
            IQueryable<T> query = !trackChanges
                ? RepositoryContext.Set<T>().AsNoTracking()
                : RepositoryContext.Set<T>();

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties
                             .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            IQueryable<T> query = !trackChanges
                ? RepositoryContext.Set<T>().Where(expression).AsNoTracking()
                : RepositoryContext.Set<T>().Where(expression);

            return query;
        }

        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
        public void DeleteRange(IEnumerable<T> entities) => RepositoryContext.Set<T>().RemoveRange(entities);
    }
}

