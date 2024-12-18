using App.Domain.Entities;

namespace App.Application.Features.Login
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
