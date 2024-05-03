using QuickChat.BusinessLogicLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class MessageModel
    {

        [Required]
        [MinLength(1)]
        public string RecieverUserName { get; set; }
        [Required]
        [MinLength(1)]
        public string Message { get; set; }
    }
    public class MessageUpdateModal
    {
        public int MessageId { get; set; }

        public string FriendUserName { get; set; }

        public MessageStatus NewMessageStatus { get; set; }
    }
}
