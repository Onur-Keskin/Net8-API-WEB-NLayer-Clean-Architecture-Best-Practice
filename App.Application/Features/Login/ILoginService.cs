namespace App.Application.Features.Login
{
    public interface ILoginService
    {
        Task<ServiceResult<string>> AuthenticateAsync(LoginRequest loginRequest);
    }
}
