using Microsoft.AspNetCore.Identity;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Entities.Identity;

namespace QuickChat.BusinessLogicLayer.Services
{
    public class FriendShipService : IFriendshipService
    {
        private readonly IFriendhipRepository _friendhipRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public FriendShipService(IFriendhipRepository friendhipRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _friendhipRepository = friendhipRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> IsFriend(string userId, string user2Id)
        {
            var sender = await _userManager.FindByNameAsync(user2Id);
            if (sender != null)
            {
                var SenderUserId = sender.Id;
                var result = await _friendhipRepository.IsFriend(userId, SenderUserId);
                return result;
            }

            return false;
        }
        public async Task<bool> SendFriendRequest(string senderUserId, string recieverUserName)
        {
            var reciever = await _userManager.FindByNameAsync(recieverUserName);
            if (reciever != null)
            {
                var recieverUserId = reciever.Id;
                if (recieverUserId == senderUserId)
                {
                    throw new Exception("Sending a request to YourSelf is not Allowed");
                }
                var result = await _friendhipRepository.SendFriendRequest(senderUserId, recieverUserId);
                return result;
            }

            else
            {

                throw new Exception("UserName Not Found");
            }
        }
        public async Task<bool> AcceptFriendRequest(string userId, string senderUserName)
        {
            var sender = await _userManager.FindByNameAsync(senderUserName);
            if (sender != null)
            {
                var SenderUserId = sender.Id;
                var result = await _friendhipRepository.AcceptFriendRequest(userId, SenderUserId);
                return result;
            }

            return false;
        }
        public async Task<bool> RejetFriendRequest(string userId, string senderUserName)
        {
            var sender = await _userManager.FindByNameAsync(senderUserName);
            if (sender != null)
            {
                var SenderUserId = sender.Id;
                var result = await _friendhipRepository.RejetFriendRequest(userId, SenderUserId);
                return result;
            }

            return false;
        }
        public async Task<bool> RemoveFriend(string userId, string senderUserName)
        {
            var sender = await _userManager.FindByNameAsync(senderUserName);
            if (sender != null)
            {
                var SenderUserId = sender.Id;
                var result = await _friendhipRepository.RemoveFriend(userId, SenderUserId);
                return result;
            }

            return false;
        }
        public async Task<FriendRequestData> GetUserFriendRequests(string userId)
        {
            var friendRequests = await _friendhipRepository.GetUserFriendRequests(userId);
            foreach (var request in friendRequests.FriendRequestsRecieved)
            {
                request.CommonFriendsCount = await GetCommonFriendsCount(userId, request.FriendUserId);
            }
            foreach (var request in friendRequests.FriendRequestsSent)
            {
                request.CommonFriendsCount = await GetCommonFriendsCount(userId, request.FriendUserId);
            }
            return friendRequests;
        }
        public async Task<IEnumerable<FriendData>> GetUserFriendsData(string userId)
        {
            var userfriendsData = await _friendhipRepository.GetUserFriendsData(userId);
            return userfriendsData;
        }
        public async Task<int> GetCommonFriendsCount(string user1Id, string user2Id)
        {
            var user1Friends = await _friendhipRepository.GetFriendsUserNames(user1Id);
            var user2Friends = await _friendhipRepository.GetFriendsUserNames(user2Id);
            int numberOfCommonFriends = user1Friends.Intersect(user2Friends).Count();

            return numberOfCommonFriends;
        }
    }
}

