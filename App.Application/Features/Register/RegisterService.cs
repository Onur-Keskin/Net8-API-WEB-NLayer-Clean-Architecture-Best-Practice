using App.Application.Contracts.Persistance;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Register
{
    public class RegisterService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRegisterService
    {
        public async Task<ServiceResult<bool>> RegisterAsync(RegisterRequest registerRequest)
        {
            var isExistUser = await userRepository.AnyAsync(u => u.Username == registerRequest.Username);
            if (isExistUser)
            {
                return ServiceResult<bool>.Fail("Bu kullanıcı zaten alınmış");
            }

            var user = mapper.Map<User>(registerRequest);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            user.Role = "User";

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<bool>.Success(true);
        }
    }
}
