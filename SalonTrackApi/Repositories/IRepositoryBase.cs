using System.Linq.Expressions;

namespace SalonTrackApi.Repositories
{

    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges, string includeProperties = "");
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
