using System.Security.Claims;
using backend.Application.Dtos;
using backend.Application.Services;
using backend.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                return Ok(await _userService.addUser(request));
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
                return Ok(await _userService.login(request));
            } catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("login/github")]
        public IActionResult GitHubLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/User/github-callback"
            };

            return Challenge(properties, "GitHub");
        }

        [HttpGet("github-callback")]
        public async Task<IActionResult> GitHubCallback()
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            if (!result.Succeeded)
            {
                return BadRequest("GitHub authentication failed");
            }

            var githubId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = result.Principal.Identity?.Name;

            var createResult = await _userService.addOrLoginWithGithub(githubId, username);

            return Redirect(
                $"http://localhost:5173/oauth-success?accessToken={createResult.accessToken}&refreshToken={createResult.refreshToken}&message={createResult.message}"
            );
        }
    }
}
