using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IServices
{
    public interface ISignalRService
    {
        public Task SendIncomingMessageSignal(string receiverUserName, MessageResponse messageResponseReciver);
        public Task SendMessageUpdateSignal(string receiverUserName, MessageResponse messageResponseReciever);
        public Task SendMessagesUpdatedSignal(List<string> userNames, MessagesUpdateModel updateModel);
    }
}
