using App.Application.Features.Login;
using App.Application.Features.Register;
using App.Application.Features.User;
using App.Application.Features.User.Update;
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
            var userId = User.Claims.FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await userService.GetUserProfileByIdAsync(Convert.ToInt32(userId));

            return CreateActionResult(user);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var userId = User.Claims.FirstOrDefault()?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            request.Id = Convert.ToInt32(userId);

            return CreateActionResult(await userService.UpdateUserAsync(request));
        }

    }
}
