using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DL
{
    public class ChatDL : IChatDL
    {
        private readonly ApplicationDbContext _context;

        public ChatDL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateUserStatusAsync(string userId, bool isOnline)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsOnline = isOnline;
            if (isOnline)
            {
                user.LastLogin = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserInConversationAsync(string userId, int conversationId)
        {
            return await _context.Participants
                .AnyAsync(p => p.UserId == userId && p.ConversationId == conversationId);
        }

        public async Task<Message?> SaveMessageAsync(string senderId, int conversationId, string content)
        {
            // Kiểm tra người dùng có trong cuộc hội thoại không
            var isParticipant = await IsUserInConversationAsync(senderId, conversationId);
            if (!isParticipant) return null;

            // Lấy conversation
            var conversation = await _context.Conversations.FindAsync(conversationId);
            if (conversation == null) return null;

            // Tạo message mới
            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ConversationId = conversationId,
                SentAt = DateTime.UtcNow,
                Status = MessageStatus.Sent
            };

            // Cập nhật LastMessageAt của conversation
            conversation.LastMessageAt = message.SentAt;

            // Lưu vào database
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            // Load sender
            await _context.Entry(message)
                .Reference(m => m.Sender)
                .LoadAsync();

            return message;
        }

        public async Task<bool> MarkMessageAsReadAsync(string userId, int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null) return false;

            // Kiểm tra xem người dùng có trong conversation không
            var isParticipant = await IsUserInConversationAsync(userId, message.ConversationId);
            if (!isParticipant) return false;

            // Không phải tin nhắn của người dùng hiện tại
            if (message.SenderId != userId)
            {
                message.Status = MessageStatus.Read;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<Conversation>> GetUserConversationsAsync(string userId)
        {
            return await _context.Conversations
                .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
                    .ThenInclude(m => m.Sender)
                .Where(c => c.Participants.Any(p => p.UserId == userId))
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

        public async Task<Conversation?> CreateConversationAsync(string creatorId, string name, List<string> participantIds)
        {
            // Kiểm tra người tạo có trong danh sách tham gia không
            if (!participantIds.Contains(creatorId))
            {
                participantIds.Add(creatorId);
            }

            // Kiểm tra toàn bộ participant có tồn tại không
            var participants = await _context.Users
                .Where(u => participantIds.Contains(u.Id))
                .ToListAsync();

            if (participants.Count != participantIds.Count)
            {
                return null; // Có người dùng không tồn tại
            }

            // Tạo conversation mới
            var conversation = new Conversation
            {
                Name = name,
                CreatedAt = DateTime.UtcNow,
                LastMessageAt = DateTime.UtcNow
            };

            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();

            // Thêm participants
            foreach (var userId in participantIds)
            {
                await _context.Participants.AddAsync(new Participant
                {
                    UserId = userId,
                    ConversationId = conversation.Id,
                    JoinedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            // Load participants
            await _context.Entry(conversation)
                .Collection(c => c.Participants)
                .Query()
                .Include(p => p.User)
                .LoadAsync();

            return conversation;
        }

        public async Task<List<Message>> GetConversationMessagesAsync(int conversationId, int skip, int take)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Attachments)
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
