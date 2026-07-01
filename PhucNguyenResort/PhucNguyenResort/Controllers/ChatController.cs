using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Gemini;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IGeminiService _geminiService;

        public ChatController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Nội dung tin nhắn không được để trống.");
            }

            var responseText = await _geminiService.ChatAsync(request.Message, request.History);
            return Ok(new { response = responseText });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = "";
        public List<ChatMessageDto> History { get; set; } = new();
    }
}
