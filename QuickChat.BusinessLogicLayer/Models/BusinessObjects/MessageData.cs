using QuickChat.BusinessLogicLayer.Enums;

namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class MessageResponse
    {
        public MessageData Message { get; set; }
        public string FriendUserName { get; set; }

        public string BatchDate { get; set; }
    }
    public class MessageData
    {
        public int Id { get; set; }
        public bool IsIncoming { get; set; }
        public string Message { get; set; }
        public MessageStatus MessageStatus { get; set; }

        public DateTime? ActionAt { get; set; }


    }
}
