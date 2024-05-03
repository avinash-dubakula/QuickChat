using QuickChat.BusinessLogicLayer.Enums;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Entities;

namespace QuickChat.BusinessLogicLayer.IRepositories
{
    public interface IMessageRepository
    {
        public Task<UserMessage> SendMessage(string senderUserId, string recieverUserId, string message);
        public Task<UserMessage> UpdateMessageStatus(int messageId, MessageStatus newMesageStatus);
        public Task<UserMessage> GetMessage(int messageId, string recieverUserId, string senderUserId);
        public Task<List<string>> GetFriendIdsAsync(string currentUserId);
        public Task<Chat> GetChatModel(string friendUserId, List<MessageData> messages);

        public Task<bool> UpdateMessageStatusToSeenAsync(string senderUserId, string receiverUserId);
        public Task<List<UserMessage>> GetMessagesBetweenUsersAsync(string currentUserId, string friendUserId);
        public Task<DateTime?> GetEarliestUnDeliveredMessageDateAsync(string currentUserId, string friendUserId);
        public Task<List<string>> GetMessagesForDeliver(string currentUserId, DateTime dbfetchedTime, DateTime DeliveredTime);
        public Task<int> UpdateMessagesForSeen(string currentUserId, string friendUserId, DateTime currentTime);

    }
}
