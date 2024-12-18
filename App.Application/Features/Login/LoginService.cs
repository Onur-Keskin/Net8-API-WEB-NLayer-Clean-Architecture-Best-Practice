using App.Application.Contracts.Persistance;
using AutoMapper;

namespace App.Application.Features.Login
{
    public class LoginService(IUserRepository userRepository, ITokenService tokenService,IMapper mapper): ILoginService
    {
        public async Task<ServiceResult<string>> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = userRepository.GetByUsername(loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Result.PasswordHash))
            {
                throw new UnauthorizedAccessException("Bilgiler hatalı");
            }

            var token = tokenService.GenerateToken(user.Result);
            return ServiceResult<string>.Success(token);
        }
    }
}
