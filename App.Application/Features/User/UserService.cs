using App.Application.Contracts.Persistance;
using App.Application.Features.Login;
using App.Application.Features.User.Dto;
using AutoMapper;

namespace App.Application.Features.User
{
    public class UserService(IUserRepository userRepository, IMapper mapper,ITokenService tokenService) : IUserService
    {
        public async Task<ServiceResult<List<UserDto>>> GetAllUserListsAsync()
        {
            var users = await userRepository.GetAllAsync();

            var usersAsDto = mapper.Map<List<UserDto>>(users);

            return ServiceResult<List<UserDto>>.Success(usersAsDto);
        }

        public async Task<ServiceResult<UserProfileDto>> GetUserProfileByIdAsync(int userId)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
                return ServiceResult<UserProfileDto>.Fail("Kullanıcı bulunamadı");

            var userProfileDto = mapper.Map<UserProfileDto>(user);

            return ServiceResult<UserProfileDto>.Success(userProfileDto);
        }
    }   
}
