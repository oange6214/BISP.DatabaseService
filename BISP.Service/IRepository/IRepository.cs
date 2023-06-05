using System.Linq.Expressions;

namespace BISP.Service.IRepository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid Guid);
    Task InsertAsync(T entity);
    Task InsertRangeAsync(IEnumerable<T> entities);
    Task Update(T entity);
    Task Delete(Guid Guid);
    Task DeleteRange(IEnumerable<T> entities);
}