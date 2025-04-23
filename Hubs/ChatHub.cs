using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend.BL;

namespace Backend.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatBL _chatBL;
        
        public ChatHub(IChatBL chatBL)
        {
            _chatBL = chatBL;
        }
        
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.FindFirst("userId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _chatBL.UpdateUserStatusAsync(userId, true);
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                await base.OnConnectedAsync();
            }
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("userId")?.Value;
            Console.WriteLine($"[SignalR] User {userId} disconnected: {Context.ConnectionId} - Lý do: {exception?.Message}");
            if (!string.IsNullOrEmpty(userId))
            {
                await _chatBL.UpdateUserStatusAsync(userId, false);
            }
            await base.OnDisconnectedAsync(exception);
        }
        
        public async Task JoinConversation(int conversationId)
        {
            var userId = Context.User!.FindFirst("userId")?.Value;
            var isAllowed = await _chatBL.IsUserInConversationAsync(userId!, conversationId);
            
            if (isAllowed)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
            }
        }
        
        public async Task SendMessage(int conversationId, string content)
        {
            var userId = Context.User!.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId)) return;
            
            var message = await _chatBL.SaveMessageAsync(userId, conversationId, content);
            
            if (message != null)
            {
                await Clients.Group($"conversation_{conversationId}").SendAsync("ReceiveMessage", message);
            }
        }
        
        public async Task MarkAsRead(int conversationId, int messageId)
        {
            var userId = Context.User!.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId)) return;
            
            await _chatBL.MarkMessageAsReadAsync(userId, messageId);
            await Clients.Group($"conversation_{conversationId}").SendAsync("MessageRead", userId, messageId);
        }
        
        public async Task SendTypingNotification(int conversationId)
        {
            var userId = Context.User!.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId)) return;
            
            await Clients.Group($"conversation_{conversationId}")
                .SendAsync("UserTyping", userId, conversationId);
        }
    }
}
