using Microsoft.EntityFrameworkCore;
using QuickChat.BusinessLogicLayer.Enums;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Entities;
using QuickChat.DataAccessLayer.Context;

namespace QuickChat.DataAccessLayer.Repositories
{
    public class FriendshipRepository : IFriendhipRepository
    {
        private readonly MyIdentityDbContext _myIdentityDbContext;
        public FriendshipRepository(MyIdentityDbContext myIdentityDbContext)
        {
            _myIdentityDbContext = myIdentityDbContext;
        }
        public async Task<bool> IsFriend(string user1Id, string user2Id)
        {
            var friendshipResult = await _myIdentityDbContext.UserFriendRequests
                .FirstOrDefaultAsync(record => (record.SenderUserId == user1Id && record.RecieverUserId == user2Id)
                    || (record.SenderUserId == user2Id && record.RecieverUserId == user1Id));
            if (friendshipResult?.FriendRequestStatus == FriendRequestStatus.Accepted)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> SendFriendRequest(string senderUserId, string recieverUserId)
        {
            try
            {
                var friendshipResult = await _myIdentityDbContext.UserFriendRequests
                .FirstOrDefaultAsync(record => (record.SenderUserId == senderUserId && record.RecieverUserId == recieverUserId)
                    || (record.SenderUserId == recieverUserId && record.RecieverUserId == senderUserId));
                if (friendshipResult != null)
                {
                    //Tring to Send a Request to one who alredy sent us a request
                    if (friendshipResult.RecieverUserId == senderUserId && friendshipResult.FriendRequestStatus == FriendRequestStatus.Sent)
                    {
                        friendshipResult.FriendRequestStatus = FriendRequestStatus.Accepted;
                        friendshipResult.UpdatedAt = DateTime.Now;
                    }
                    // If friend Request is declined and user sends again
                    else if (friendshipResult.FriendRequestStatus == FriendRequestStatus.Declined)
                    {
                        friendshipResult.FriendRequestStatus = FriendRequestStatus.Sent;
                        friendshipResult.SentAt = DateTime.Now;
                    }
                    else if (friendshipResult.RecieverUserId == recieverUserId && friendshipResult.FriendRequestStatus == FriendRequestStatus.Sent)
                    {
                        throw new Exception("Alredy sent a request");
                    }
                    var updateResult = await _myIdentityDbContext.SaveChangesAsync();
                    return updateResult > 0;
                }
                // Trying to add a record 
                var newFriendRequest = new UserFriendRequest()
                {
                    SenderUserId = senderUserId,
                    RecieverUserId = recieverUserId,
                    SentAt = DateTime.Now,
                    FriendRequestStatus = FriendRequestStatus.Sent
                };
                _myIdentityDbContext.Add(newFriendRequest);
                var insertResult = await _myIdentityDbContext.SaveChangesAsync();
                return insertResult > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> AcceptFriendRequest(string userId, string senderId)
        {
            var friendshipResult = await _myIdentityDbContext.UserFriendRequests
                .FirstOrDefaultAsync(record => record.SenderUserId == senderId && record.RecieverUserId == userId);
            if (friendshipResult == null)
            {
                throw new Exception("Cannot Find the Friend Request");
            }
            if (friendshipResult?.FriendRequestStatus == FriendRequestStatus.Sent)
            {
                friendshipResult.FriendRequestStatus = FriendRequestStatus.Accepted;
                friendshipResult.UpdatedAt = DateTime.Now;
                var updateResult = await _myIdentityDbContext.SaveChangesAsync();
                return updateResult > 0;
            }
            else
            {
                throw new Exception("Cannot Update the Friend Request");
            }
        }
        public async Task<bool> RejetFriendRequest(string userId, string senderId)
        {
            var friendshipResult = await _myIdentityDbContext.UserFriendRequests
                .FirstOrDefaultAsync(record => record.SenderUserId == senderId && record.RecieverUserId == userId);
            if (friendshipResult == null)
            {
                throw new Exception("Cannot Find the Friend Request");
            }
            if (friendshipResult?.FriendRequestStatus == FriendRequestStatus.Sent)
            {
                friendshipResult.FriendRequestStatus = FriendRequestStatus.Declined;
                friendshipResult.UpdatedAt = DateTime.Now;
                var updateResult = await _myIdentityDbContext.SaveChangesAsync();
                return updateResult > 0;
            }
            else
            {
                throw new Exception("Cannot Update the Friend Request");
            }
        }

        public async Task<bool> RemoveFriend(string userId, string senderId)
        {
            var friendshipResult = await _myIdentityDbContext.UserFriendRequests
                .FirstOrDefaultAsync(record => record.SenderUserId == senderId && record.RecieverUserId == userId);
            if (friendshipResult == null)
            {
                throw new Exception("Cannot Find the Friendship");
            }
            if (friendshipResult?.FriendRequestStatus == FriendRequestStatus.Accepted)
            {
                friendshipResult.FriendRequestStatus = FriendRequestStatus.Declined;
                friendshipResult.UpdatedAt = DateTime.Now;
                var updateResult = await _myIdentityDbContext.SaveChangesAsync();
                return updateResult > 0;
            }
            else if (friendshipResult?.FriendRequestStatus != FriendRequestStatus.Accepted)
            {
                throw new Exception("You are Not Friends");
            }
            else
            {
                throw new Exception("Cannot Update the Friend Request");
            }
        }
        public async Task<List<FriendRequestData>> GetUserFriendRequests(string userId)
        {
            var friendRequests = await _myIdentityDbContext.UserFriendRequests
                .Where(record => record.RecieverUserId == userId && record.FriendRequestStatus == FriendRequestStatus.Sent).ToListAsync();
            var friendRequestModels = friendRequests.Join(_myIdentityDbContext.Users,
                    fr => fr.SenderUserId,
                    u => u.Id,
                    (fr, u) => new FriendRequestData
                    {
                        SenderName = u.FirstName + " " + u.LastName,
                        SenderUserName = u.UserName,
                        SentAt = fr.SentAt,
                        FriendUserId = (fr.SenderUserId == userId) ? fr.RecieverUserId : fr.SenderUserId,
                        ProfileUrl = u.ProfilePhotoUrl,

                    })
                .ToList();
            return friendRequestModels;
        }

        public async Task<List<FriendData>> GetUserFriendsData(string userId)
        {
            var friendRequests = await _myIdentityDbContext.UserFriendRequests
                .Where(record => (record.RecieverUserId == userId || record.SenderUserId == userId) && record.FriendRequestStatus == FriendRequestStatus.Accepted).ToListAsync();
            var friendDataModels = friendRequests.Join(_myIdentityDbContext.Users,
                    fr => (userId == fr.SenderUserId ? fr.RecieverUserId : fr.SenderUserId),
                    u => u.Id,
                    (fr, u) => new FriendData
                    {
                        FullName = u.FirstName + " " + u.LastName,
                        UserName = u.UserName,
                        Email = u.Email,
                        FriendsFrom = fr.UpdatedAt,
                        ProfilePhotoUrl = u.ProfilePhotoUrl,
                        UserId = u.Id
                    })
                .ToList();
            return friendDataModels;
        }
        public async Task<IEnumerable<string>> GetFriendsUserNames(string userId)
        {
            var friendUserNames = await _myIdentityDbContext.UserFriendRequests
                .Where(record => (record.RecieverUserId == userId || record.SenderUserId == userId) && record.FriendRequestStatus == FriendRequestStatus.Accepted).Select(r => (userId == r.SenderUserId) ? r.RecieverUserId : r.SenderUserId).ToListAsync();
            return friendUserNames;
        }

    }
}
