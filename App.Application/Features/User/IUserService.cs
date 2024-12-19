using App.Application.Features.User.Dto;
using App.Application.Features.User.Update;

namespace App.Application.Features.User
{
    public interface IUserService
    {
        public Task<ServiceResult<List<UserDto>>> GetAllUserListsAsync();
        public Task<ServiceResult<UserProfileDto>> GetUserProfileByIdAsync(int userId);
        public Task<ServiceResult> UpdateUserAsync(UpdateUserRequest request);
    }
}
