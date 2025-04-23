using Backend.DL;
using Backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace Backend.BL
{
    public class ChatBL : IChatBL
    {
        private readonly IChatDL _chatDL;

        public ChatBL(IChatDL chatDL)
        {
            _chatDL = chatDL;
        }

        public async Task<bool> UpdateUserStatusAsync(string userId, bool isOnline)
        {
            return await _chatDL.UpdateUserStatusAsync(userId, isOnline);
        }

        public async Task<bool> IsUserInConversationAsync(string userId, int conversationId)
        {
            return await _chatDL.IsUserInConversationAsync(userId, conversationId);
        }

        public async Task<MessageDTO?> SaveMessageAsync(string senderId, int conversationId, string content)
        {
            var message = await _chatDL.SaveMessageAsync(senderId, conversationId, content);
            if (message == null) return null;

            return MapMessageToDTO(message);
        }

        public async Task<bool> MarkMessageAsReadAsync(string userId, int messageId)
        {
            return await _chatDL.MarkMessageAsReadAsync(userId, messageId);
        }

        public async Task<List<ConversationDTO>> GetUserConversationsAsync(string userId)
        {
            var conversations = await _chatDL.GetUserConversationsAsync(userId);
            return conversations.Select(MapConversationToDTO).ToList();
        }

        public async Task<ConversationDTO?> CreateConversationAsync(string creatorId, string name, List<string> participantIds)
        {
            var conversation = await _chatDL.CreateConversationAsync(creatorId, name, participantIds);
            if (conversation == null) return null;

            return MapConversationToDTO(conversation);
        }

        public async Task<List<MessageDTO>> GetConversationMessagesAsync(string userId, int conversationId, int skip, int take)
        {
            // Kiểm tra người dùng có trong cuộc hội thoại không
            var isParticipant = await IsUserInConversationAsync(userId, conversationId);
            if (!isParticipant) return new List<MessageDTO>();

            var messages = await _chatDL.GetConversationMessagesAsync(conversationId, skip, take);
            return messages.Select(MapMessageToDTO).ToList();
        }

        #region Helper methods

        private MessageDTO MapMessageToDTO(Message message)
        {
            return new MessageDTO
            {
                Id = message.Id,
                Content = message.Content,
                ConversationId = message.ConversationId,
                SentAt = message.SentAt,
                Status = message.Status,
                Sender = new UserDetail
                {
                    Id = message.SenderId,
                    Username = message.Sender?.UserName ?? string.Empty,
                    Email = message.Sender?.Email ?? string.Empty,
                    FullName = message.Sender?.FullName ?? string.Empty,
                    IsOnline = message.Sender?.IsOnline ?? false
                },
                Attachments = message.Attachments?.Select(a => new AttachmentDTO
                {
                    Id = a.Id,
                    FileUrl = a.FileUrl,
                    FileName = a.FileName,
                    FileType = a.FileType,
                    FileSize = a.FileSize
                }).ToList() ?? new List<AttachmentDTO>()
            };
        }

        private ConversationDTO MapConversationToDTO(Conversation conversation)
        {
            var lastMessage = conversation.Messages?.OrderByDescending(m => m.SentAt).FirstOrDefault();

            return new ConversationDTO
            {
                Id = conversation.Id,
                Name = conversation.Name,
                CreatedAt = conversation.CreatedAt,
                LastMessageAt = conversation.LastMessageAt,
                Participants = conversation.Participants?.Select(p => new UserDetail
                {
                    Id = p.UserId,
                    Username = p.User?.UserName ?? string.Empty,
                    Email = p.User?.Email ?? string.Empty,
                    FullName = p.User?.FullName ?? string.Empty,
                    IsOnline = p.User?.IsOnline ?? false
                }).ToList() ?? new List<UserDetail>(),
                LastMessage = lastMessage != null ? MapMessageToDTO(lastMessage) : null
            };
        }

        #endregion
    }
}
