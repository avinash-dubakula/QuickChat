using Microsoft.AspNetCore.SignalR;
using QuickChat.BusinessLogicLayer.Hubs;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ThreadSafeDictionary _threadSafeDictionary;

        public SignalRService(IHubContext<ChatHub> hubContext, ThreadSafeDictionary threadSafeDictionary)
        {
            _hubContext = hubContext;
            _threadSafeDictionary = threadSafeDictionary;
        }
        public async Task SendIncomingMessageSignal(string receiverUserName, MessageResponse messageResponseReciever)
        {
            // Call the SendMessageToUser method in ChatHub to send a message
            var effectedClientIds = _threadSafeDictionary.GetClientIds(receiverUserName);
            await _hubContext.Clients.Clients(effectedClientIds).SendAsync("RecieveMessage", messageResponseReciever);
        }
        public async Task SendMessageUpdateSignal(string receiverUserName, MessageResponse messageResponseReciever)
        {
            var effectedClientIds = _threadSafeDictionary.GetClientIds(receiverUserName);
            // Call the SendMessageToUser method in ChatHub to send a message
            await _hubContext.Clients.Clients(effectedClientIds).SendAsync("MessageUpdated", messageResponseReciever);
        }
        public async Task SendMessagesUpdatedSignal(List<string> userNames, MessagesUpdateModel updateModel)
        {
            var effectedClientIds = _threadSafeDictionary.GetClientIds(userNames);
            await _hubContext.Clients.Clients(effectedClientIds).SendAsync("MessagesUpdated", updateModel);
        }


    }
}
