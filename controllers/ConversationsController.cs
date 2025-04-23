using Backend.BL;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly IChatBL _chatBL;

        public ConversationsController(IChatBL chatBL)
        {
            _chatBL = chatBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var conversations = await _chatBL.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var conversation = await _chatBL.CreateConversationAsync(userId, request.Name, request.ParticipantIds);
            if (conversation == null)
                return BadRequest(new { success = false, message = "Không thể tạo cuộc hội thoại" });

            return Ok(new { success = true, conversation });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversation(int id)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var conversations = await _chatBL.GetUserConversationsAsync(userId);
            var conversation = conversations.FirstOrDefault(c => c.Id == id);
            
            if (conversation == null)
                return NotFound(new { success = false, message = "Không tìm thấy cuộc hội thoại" });

            return Ok(new { success = true, conversation });
        }

        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetMessages(int id, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var messages = await _chatBL.GetConversationMessagesAsync(userId, id, skip, take);
            return Ok(new { success = true, messages });
        }
    }
}
