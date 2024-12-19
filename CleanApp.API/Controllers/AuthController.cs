using App.Application.Features.Login;
using App.Application.Features.Register;
using App.Application.Features.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.API.Controllers
{
    public class AuthController(ILoginService loginService,IRegisterService registerService,
        IUserService userService) : CustomBaseController
    {

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
            => CreateActionResult(await loginService.AuthenticateAsync(request));

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) =>
            CreateActionResult(await registerService.RegisterAsync(request));

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() => 
            CreateActionResult(await userService.GetAllUserListsAsync());

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.Claims.FirstOrDefault()!.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userProfile = await userService.GetUserProfileByIdAsync(Convert.ToInt32(userId));

            return CreateActionResult(userProfile);
        }

    }
}
