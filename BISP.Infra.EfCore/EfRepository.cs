using BISP.Infra.Entity.Data;
using BISP.Service.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BISP.Infra.EfCore;

public class EfRepository<T> : IRepository<T> where T : class
{
    private DbContext _dbContext;
    private DbSet<T> _dbSet;

    public EfRepository(BispContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid guid)
    {
        return await _dbSet.FindAsync(guid);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid guid)
    {
        T entity = await _dbSet.FindAsync(guid);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
