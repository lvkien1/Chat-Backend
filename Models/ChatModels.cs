using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;
        public List<Participant> Participants { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }

    public class Participant
    {
        public string UserId { get; set; } = string.Empty;
        public int ConversationId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser? User { get; set; }
        public Conversation? Conversation { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public int ConversationId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public MessageStatus Status { get; set; } = MessageStatus.Sent;
        public ApplicationUser? Sender { get; set; }
        public Conversation? Conversation { get; set; }
        public List<Attachment> Attachments { get; set; } = new();
    }

    public class Attachment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public Message? Message { get; set; }
    }

    public enum MessageStatus
    {
        Sent = 0,
        Delivered = 1,
        Read = 2
    }

    // DTO cho Chat
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public UserDetail Sender { get; set; } = new UserDetail();
        public int ConversationId { get; set; }
        public DateTime SentAt { get; set; }
        public MessageStatus Status { get; set; }
        public List<AttachmentDTO> Attachments { get; set; } = new();
    }

    public class AttachmentDTO
    {
        public int Id { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public int FileSize { get; set; }
    }

    public class ConversationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastMessageAt { get; set; }
        public List<UserDetail> Participants { get; set; } = new();
        public MessageDTO? LastMessage { get; set; }
    }

    // Request Models
    public class CreateConversationRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<string> ParticipantIds { get; set; } = new();
    }

    public class SendMessageRequest
    {
        [Required]
        public int ConversationId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
