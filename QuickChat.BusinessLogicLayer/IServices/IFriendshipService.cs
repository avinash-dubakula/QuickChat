using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IServices
{
    public interface IFriendshipService
    {
        public Task<bool> IsFriend(string userId, string user2Id);
        public Task<bool> SendFriendRequest(string senderUserId, string recieverUserName);
        public Task<bool> AcceptFriendRequest(string userId, string senderUserName);
        public Task<bool> RejetFriendRequest(string userId, string senderUserName);
        public Task<FriendRequestData> GetUserFriendRequests(string userId);
        public Task<IEnumerable<FriendData>> GetUserFriendsData(string userId);
        public Task<bool> RemoveFriend(string userId, string senderUserName);
    }
}
