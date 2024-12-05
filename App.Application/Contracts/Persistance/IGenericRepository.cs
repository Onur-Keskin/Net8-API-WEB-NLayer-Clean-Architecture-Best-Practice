using System.Linq.Expressions;

namespace App.Application.Contracts.Persistance
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct
    {
        Task<bool> AnyAsync(TId id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate); //dinamik bir şart yazmak istersek
        ValueTask<T?> GetByIdAsync(int id); //Task' lardan daha hafif value type memory' de daha az yer kaplıyor
        ValueTask AddAsync(T entity);
        void Update(T enitity);
        void Delete(T entity);
    }
}
