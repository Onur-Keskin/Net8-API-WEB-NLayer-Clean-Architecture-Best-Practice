using App.Application.Contracts.Persistance;
using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistance
{
    public class GenericRepository<T, TId>(AppDbContext context) : IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext Context = context;

        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<bool> AnyAsync(TId id) => _dbSet.AnyAsync(x => x.Id.Equals(id));
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => _dbSet.AnyAsync(predicate);
        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public Task<List<T>> GetAllAsync() => _dbSet.ToListAsync();
        public Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize) =>_dbSet.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
        public ValueTask<T?> GetByIdAsync(int id) => _dbSet.FindAsync(id);
        public void Update(T enitity) => _dbSet.Update(enitity);

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();
    }
}
