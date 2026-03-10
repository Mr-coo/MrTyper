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

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(
            [FromBody] CreateUserRequestDto request
        )
        {
            var result = await _userService.addUser(request);
            return Ok(result);
        }
    }
}
