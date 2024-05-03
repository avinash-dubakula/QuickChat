using QuickChat.BusinessLogicLayer.Enums;

namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class MessagesUpdateModel
    {
        public string FriendUserName { get; set; }
        public DateTime DbFetchedTime { get; set; }
        public DateTime DeliveredTime { get; set; }
        public MessageStatus NewStatus { get; set; }
    }
    public class MessagesSeenModel
    {
        public string FriendUserName { get; set; }
        public int SeenTillMessageId { get; set; }
    }
}
