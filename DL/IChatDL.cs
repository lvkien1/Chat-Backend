using Backend.Models;

namespace Backend.DL
{
    public interface IChatDL
    {
        Task<bool> UpdateUserStatusAsync(string userId, bool isOnline);
        Task<bool> IsUserInConversationAsync(string userId, int conversationId);
        Task<Message?> SaveMessageAsync(string senderId, int conversationId, string content);
        Task<bool> MarkMessageAsReadAsync(string userId, int messageId);
        Task<List<Conversation>> GetUserConversationsAsync(string userId);
        Task<Conversation?> CreateConversationAsync(string creatorId, string name, List<string> participantIds);
        Task<List<Message>> GetConversationMessagesAsync(int conversationId, int skip, int take);
    }
}
