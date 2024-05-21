using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IRepositories
{
    public interface IFriendhipRepository
    {
        public Task<bool> IsFriend(string user1Id, string user2Id);
        public Task<bool> SendFriendRequest(string senderUserId, string recieverUserId);
        public Task<bool> AcceptFriendRequest(string userId, string senderId);
        public Task<bool> RejetFriendRequest(string userId, string senderId);
        public Task<List<FriendRequestData>> GetUserFriendRequests(string userId);
        public Task<List<FriendData>> GetUserFriendsData(string userId);
        public Task<bool> RemoveFriend(string userId, string senderId);
        public Task<IEnumerable<string>> GetFriendsUserNames(string userId);
    }
}
