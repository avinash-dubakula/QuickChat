using QuickChat.BusinessLogicLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuickChat.BusinessLogicLayer.Models.Entities
{

    public class UserMessage
    {
        public int Id { get; set; }
        [Required]
        public string SenderUserId { get; set; }
        [Required]
        public string RecieverUserId { get; set; }
        public string Message { get; set; }
        [Required]
        public MessageStatus MessageStatus { get; set; }
        [Required]
        public DateTime MessageSentAt { get; set; }
        public DateTime? MessageDeliveredAt { get; set; }
        public DateTime? MessageSeenAt { get; set; }

    }
}
