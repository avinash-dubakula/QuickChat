using QuickChat.BusinessLogicLayer.Enums;

namespace QuickChat.BusinessLogicLayer.Models.Entities
{
    public class UserFriendRequest
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }

        public string RecieverUserId { get; set; }
        public FriendRequestStatus FriendRequestStatus { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
