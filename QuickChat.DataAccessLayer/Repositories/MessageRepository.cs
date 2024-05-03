using Microsoft.EntityFrameworkCore;
using QuickChat.BusinessLogicLayer.Enums;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Entities;
using QuickChat.DataAccessLayer.Context;

namespace QuickChat.DataAccessLayer.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MyIdentityDbContext _myIdentityDbContext;

        public MessageRepository(MyIdentityDbContext myIdentityDbContext)
        {
            _myIdentityDbContext = myIdentityDbContext;

        }

        public async Task<DateTime?> GetEarliestUnDeliveredMessageDateAsync(string currentUserId, string friendUserId)
        {
            var earliestUnreadMessage = await _myIdentityDbContext.UserMessages
                .Where(um => (um.SenderUserId == friendUserId && um.RecieverUserId == currentUserId)
                             && um.MessageStatus == MessageStatus.Sent)
                .OrderBy(um => um.MessageSentAt)
                .FirstOrDefaultAsync();

            return earliestUnreadMessage?.MessageSentAt;
        }
        public async Task<UserMessage> SendMessage(string senderUserId, string recieverUserId, string message)
        {
            try
            {
                var userMessage = new UserMessage()
                {
                    SenderUserId = senderUserId,
                    RecieverUserId = recieverUserId,
                    Message = message,
                    MessageSentAt = DateTime.Now,
                    MessageStatus = MessageStatus.Sent
                };
                await _myIdentityDbContext.UserMessages.AddAsync(userMessage);
                var sendingResult = await _myIdentityDbContext.SaveChangesAsync();
                if (sendingResult > 0)
                {
                    return userMessage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<UserMessage> UpdateMessageStatus(int messageId, MessageStatus newMesageStatus)
        {
            var messageToUpdate = _myIdentityDbContext.UserMessages
                .FirstOrDefault(msg => msg.Id == messageId);
            if (messageToUpdate != null && messageToUpdate.Id > 0)
            {
                messageToUpdate.MessageStatus = newMesageStatus;
                if (newMesageStatus == MessageStatus.Sent)
                {
                    messageToUpdate.MessageDeliveredAt = DateTime.Now;
                    messageToUpdate.MessageSentAt = DateTime.Now;
                }
                else if (newMesageStatus == MessageStatus.Delivered)
                {
                    messageToUpdate.MessageDeliveredAt = DateTime.Now;
                }
                await _myIdentityDbContext.SaveChangesAsync();
                return messageToUpdate;
            }
            return null;
        }
        public async Task<bool> UpdateMessageStatuses(int messageId, MessageStatus newMesageStatus)
        {
            var messagesToUpdate = _myIdentityDbContext.UserMessages
                .Where(msg => msg.Id <= messageId);
            foreach (var messageToUpdate in messagesToUpdate)
            {
                if (messageToUpdate != null && messageToUpdate.Id > 0)
                {
                    messageToUpdate.MessageStatus = newMesageStatus;
                    if (newMesageStatus == MessageStatus.Sent)
                    {
                        messageToUpdate.MessageDeliveredAt = DateTime.Now;
                        messageToUpdate.MessageSentAt = DateTime.Now;
                    }
                    else if (newMesageStatus == MessageStatus.Delivered)
                    {
                        messageToUpdate.MessageDeliveredAt = DateTime.Now;
                    }

                }
            }
            await _myIdentityDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<string>> GetMessagesForDeliver(string currentUserId, DateTime dbfetchedTime, DateTime DeliveredTime)
        {
            var messagesToUpdate = await _myIdentityDbContext.UserMessages
                .Where(message => message.RecieverUserId == currentUserId && message.MessageStatus == MessageStatus.Sent && message.MessageSentAt <= dbfetchedTime && message.MessageDeliveredAt == null).ToListAsync();
            foreach (var message in messagesToUpdate)
            {
                message.MessageStatus = MessageStatus.Delivered;
                message.MessageDeliveredAt = DeliveredTime;
            }
            var userIds = messagesToUpdate.Select(message => message.SenderUserId).Distinct().Where(item => item != currentUserId);
            var userNames = await _myIdentityDbContext.Users.Where(user => userIds.Contains(user.Id)).Select(user => user.UserName).ToListAsync();
            await _myIdentityDbContext.SaveChangesAsync();
            return userNames;
        }
        public async Task<int> UpdateMessagesForSeen(string currentUserId, string friendUserId, DateTime currentTime)
        {
            var messagesTobeUpdated = await _myIdentityDbContext.UserMessages
                .Where(message => message.SenderUserId == friendUserId && message.RecieverUserId == currentUserId).ToListAsync();
            foreach (var message in messagesTobeUpdated)
            {
                message.MessageStatus = MessageStatus.Seen;
                message.MessageSeenAt = currentTime;
            }
            await _myIdentityDbContext.SaveChangesAsync();
            return messagesTobeUpdated.Count();
        }
        public async Task<UserMessage> GetMessage(int messageId, string recieverUserId, string senderUserId)
        {
            var messageToUpdate = await _myIdentityDbContext.UserMessages
                .Where(message => message.RecieverUserId == recieverUserId && message.SenderUserId == senderUserId).
                FirstOrDefaultAsync(msg => msg.Id == messageId);

            if (messageToUpdate != null)
            {
                return messageToUpdate;
            }
            return null;
        }
        public async Task<List<string>> GetFriendIdsAsync(string currentUserId)
        {
            var friendIds = await _myIdentityDbContext.UserFriendRequests
                 .Where(request => request.FriendRequestStatus == FriendRequestStatus.Accepted && (request.SenderUserId == currentUserId || request.RecieverUserId == currentUserId))
                 .Select(request => (request.SenderUserId == currentUserId) ? request.RecieverUserId : request.SenderUserId)
                 .ToListAsync();
            return friendIds;
        }
        public async Task<Chat> GetChatModel(string friendUserId, List<MessageData> messages)
        {
            try
            {
                var friend = await _myIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == friendUserId);
                if (friend != null)
                {

                    var chatModel = new Chat()
                    {
                        FriendName = friend.FirstName + " " + friend.LastName,
                        FriendUserName = friend.UserName,
                        UnreadMessageCount = messages.Where(um => um.IsIncoming == true && um.MessageStatus != MessageStatus.Seen && um.MessageStatus != MessageStatus.Deleted).Count(),
                        LastMessage = messages.LastOrDefault()?.Message ?? "",
                        LastMessageDate = messages.LastOrDefault()?.ActionAt ?? DateTime.Now,
                        LastMessageId = messages.LastOrDefault()?.Id ?? -1,
                        ProfilePhotoUrl = friend.ProfilePhotoUrl
                    }; // ToList might be necessary if the next operation cannot be translated to SQL

                    return chatModel;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> UpdateMessageStatusToSeenAsync(string senderUserId, string receiverUserId)
        {

            var messagesToUpdate = await _myIdentityDbContext.UserMessages
                .Where(um => ((um.SenderUserId == senderUserId && um.RecieverUserId == receiverUserId) ||
                              (um.SenderUserId == receiverUserId && um.RecieverUserId == senderUserId))
                             && um.MessageStatus != MessageStatus.Deleted
                             && um.MessageStatus != MessageStatus.Seen) // Filter for messages that are not already seen
                .ToListAsync();

            if (messagesToUpdate.Any())
            {
                foreach (var message in messagesToUpdate)
                {
                    message.MessageStatus = MessageStatus.Seen;
                    if (message.MessageStatus == MessageStatus.Sent && message.MessageDeliveredAt == null)
                    {
                        // If the message was Sent and not yet marked as Delivered, set both DeliveredAt and SeenAt
                        message.MessageDeliveredAt = DateTime.UtcNow;
                        message.MessageSeenAt = DateTime.UtcNow;
                    }
                    else if (message.MessageStatus == MessageStatus.Delivered)
                    {
                        // If the message was already Delivered, just set SeenAt
                        message.MessageSeenAt = DateTime.UtcNow;
                    }
                }

                return (await _myIdentityDbContext.SaveChangesAsync()) > 0;
            }

            return true; // Indicate success or that no update was needed
        }
        public async Task<List<UserMessage>> GetMessagesBetweenUsersAsync(string currentUserId, string friendUserId)
        {
            return await _myIdentityDbContext.UserMessages
                .Where(um => ((um.SenderUserId == currentUserId && um.RecieverUserId == friendUserId) ||
                              (um.SenderUserId == friendUserId && um.RecieverUserId == currentUserId))
                             && um.MessageStatus != MessageStatus.Deleted)
                .OrderByDescending(um => um.MessageSentAt)
                .ToListAsync();
        }

        // Determine the earliest unread message between two users


    }
}
