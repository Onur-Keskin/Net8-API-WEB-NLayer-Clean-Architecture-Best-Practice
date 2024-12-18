using App.Domain.Entities;

namespace App.Application.Contracts.Persistance
{
    public interface IUserRepository:IGenericRepository<User,int>
    {
        Task<User> GetByUsername(string username);
    }
}
