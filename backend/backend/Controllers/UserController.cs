using backend.Application.Dtos;
using backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(CreateUserRequestDto request)
        {
            var result = await _userService.addUser(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginRequestDto request)
        {
            var user = await _userService.login(request);

            return Ok(new
            {
                user.id,
                user.username,
                user.name
            });
        }
    }
}
