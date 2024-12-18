namespace App.Application.Features.Register
{
    public interface IRegisterService
    {
        Task<ServiceResult<bool>> RegisterAsync(RegisterRequest registerRequest);
    }
}
