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
            try
            {
                var result = await _userService.addUser(request);
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginRequestDto request)
        {
            try
            {
                var user = await _userService.login(request);

                return Ok(new
                {
                    user.id,
                    user.username,
                    user.name
                });
            } catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
