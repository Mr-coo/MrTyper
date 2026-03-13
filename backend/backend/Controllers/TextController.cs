using backend.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly TextService _service;

        public TextController(TextService service)
        {
            _service = service;
        }

        private Guid? GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim == null ? null : Guid.Parse(claim);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();

            var result = await _service.Get(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserId();

            var result = await _service.GetById(id, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("chapter-id/{id}")]
        public async Task<IActionResult> GetByChapterId(Guid id)
        {
            var userId = GetUserId();

            var result = await _service.GetByChapterId(id, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string content, Guid chapterId)
        {
            var text = await _service.Create(content, chapterId);

            return Ok(text);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] string content)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var success = await _service.Update(id, content, userId.Value);

            if (!success)
                return Forbid();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var success = await _service.Delete(id, userId.Value);

            if (!success)
                return Forbid();

            return Ok();
        }
    }
}