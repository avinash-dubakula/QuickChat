namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class Chat
    {
        public string FriendName { get; set; }

        public string LastMessage { get; set; }

        public DateTime LastMessageDate { get; set; }

        public int LastMessageId { get; set; }

        public string FriendUserName { get; set; }

        public int UnreadMessageCount { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
