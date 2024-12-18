using App.Application.Features.Login;
using App.Application.Features.Register;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILoginService loginService,IRegisterService registerService) : CustomBaseController
    {

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
            => CreateActionResult(await loginService.AuthenticateAsync(request));


        //[HttpPost("login")]
        //public IActionResult Login([FromBody] UserRequest request)
        //{
        //    var token = loginService.AuthenticateAsync(request.Username, request.Password);
        //    return Ok(new { Token = token });
        //}

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request) =>
            CreateActionResult(await registerService.RegisterAsync(request));
        
    }
}
