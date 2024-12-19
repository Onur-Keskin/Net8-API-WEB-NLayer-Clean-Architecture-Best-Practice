namespace App.Application.Features.Login
{
    public interface ITokenService
    {
        public string GenerateToken(Domain.Entities.User user);
    }
}
