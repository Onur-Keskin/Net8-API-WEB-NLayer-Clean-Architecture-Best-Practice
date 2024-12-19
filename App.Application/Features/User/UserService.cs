using App.Application.Contracts.Persistance;
using App.Application.Features.Login;
using App.Application.Features.User.Dto;
using App.Application.Features.User.Update;
using AutoMapper;
using System.Net;

namespace App.Application.Features.User
{
    public class UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork, ITokenService tokenService) 
        : IUserService
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

        public async Task<ServiceResult> UpdateUserAsync(UpdateUserRequest request)
        {
            var isUserExist = await userRepository.AnyAsync(x=>x.Id == request.Id);

            if (!isUserExist)
                return ServiceResult.Fail("Kullanıcı bulunamadı");

            var user = mapper.Map<Domain.Entities.User>(request);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            userRepository.Update(user);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }   
}
