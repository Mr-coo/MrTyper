using backend.Application.Dtos;
using backend.Application.Services;
using backend.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly ChapterService _chapterService;

        public ChapterController(ChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        // Anyone can view chapters
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                Guid? userId = null;

                var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(claim))
                    userId = Guid.Parse(claim);

                var chapters = await _chapterService.get(userId);
                return Ok(chapters);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Anyone can view a chapter
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Guid? userId = null;

                var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(claim))
                    userId = Guid.Parse(claim);

                var chapter = await _chapterService.getById(id, userId);

                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Only authenticated users can create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateChapterRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var chapter = await _chapterService.create(
                    request.Name,
                    request.IsPrivate,
                    userId
                );

                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Only authenticated users can update
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateChapterRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var chapter = await _chapterService.update(id, request.Name, request.IsPrivate, userId);

                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Only authenticated users can delete
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var chapter = await _chapterService.delete(id, userId);

                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}