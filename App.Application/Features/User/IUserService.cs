using App.Application.Features.User.Dto;

namespace App.Application.Features.User
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserDto>>> GetAllUserListsAsync();
        Task<ServiceResult<UserProfileDto>> GetUserProfileByIdAsync(int userId);
    }
}
