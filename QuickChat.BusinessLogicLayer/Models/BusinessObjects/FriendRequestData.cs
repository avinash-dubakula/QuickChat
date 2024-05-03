namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class FriendRequestData
    {
        public string SenderName { get; set; }

        public string SenderUserName { get; set; }

        public string FriendUserId { get; set; }
        public DateTime SentAt { get; set; }

        public int CommonFriendsCount { get; set; }

        public string? ProfileUrl { get; set; }
    }
}
