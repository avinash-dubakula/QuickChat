namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class FriendData : UserData
    {
        public DateTime FriendsFrom { get; set; }
        public int CommonFriendsCount { get; set; }
    }
}
