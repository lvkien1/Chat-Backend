using Backend.Models;

namespace Backend.BL
{
    public interface IChatBL
    {
        Task<bool> UpdateUserStatusAsync(string userId, bool isOnline);
        Task<bool> IsUserInConversationAsync(string userId, int conversationId);
        Task<MessageDTO?> SaveMessageAsync(string senderId, int conversationId, string content);
        Task<bool> MarkMessageAsReadAsync(string userId, int messageId);
        Task<List<ConversationDTO>> GetUserConversationsAsync(string userId);
        Task<ConversationDTO?> CreateConversationAsync(string creatorId, string name, List<string> participantIds);
        Task<List<MessageDTO>> GetConversationMessagesAsync(string userId, int conversationId, int skip, int take);
    }
}
