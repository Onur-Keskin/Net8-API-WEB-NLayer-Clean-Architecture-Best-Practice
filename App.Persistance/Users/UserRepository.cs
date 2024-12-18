using App.Application.Contracts.Persistance;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistance.Users
{
    public class UserRepository(AppDbContext context) : GenericRepository<User, int>(context), IUserRepository
    {
        public Task<User> GetByUsername(string username)
        {
            var user = context.Users.FirstOrDefaultAsync(x=>x.Username.Equals(username));

            if (user.Result == null)
            {
                throw new Exception();
            }

            return user;
        }
    }
}
