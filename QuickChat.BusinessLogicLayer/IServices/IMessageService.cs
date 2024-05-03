using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IServices
{
    public interface IMessageService
    {
        public Task IncomingMessageSignalInvoke(string userName, MessageResponse message);
        public Task<MessageResponse> SendMessage(string senderUserName, string recieverUserName, string message);
        public Task<ChatResponse> GetChatsAsync(string currentUserId, bool IsSpam, int minimumMessagesRequested);
        public Task<bool> UpdateMessageToDelivered(string senderUserName, MessageUpdateModal messageUpdateModal);
        public Task<bool> UpdateMessagesToDelivered(string currentUserName, string currentUserId, DateTime dbFetchedTime);
        public Task<bool> UpdateMessagesToSeen(string currentUserId, MessagesSeenModel messagesSeenModel);
    }
}
