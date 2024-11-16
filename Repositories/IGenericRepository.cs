using System.Linq.Expressions;

namespace App.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T,bool>> predicate); //dinamik bir şart yazmak istersek
        ValueTask<T?> GetByIdAsync(int id); //Task' lardan daha hafif value type memory' de daha az yer kaplıyor
        ValueTask AddAsync(T entity);
        void Update(T enitity);
        void Delete(T entity);
    }
}
