using Microsoft.AspNetCore.SignalR;

namespace QuickChat.BusinessLogicLayer.Hubs
{

    public class ChatHub : Hub
    {
        private readonly ThreadSafeDictionary _threadSafeDictionary;

        public ChatHub(ThreadSafeDictionary threadSafeDictionary)
        {
            _threadSafeDictionary = threadSafeDictionary;
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine("ConnectionID Connected : " + connectionId);
            await base.OnConnectedAsync();
        }
        public async void ConnectedAcknowledgement(string userName)
        {
            var connectionId = Context.ConnectionId;
            //Console.WriteLine("ConnectedAcknowledgeEment " + connectionId + " : " + userName);
            var result = await _threadSafeDictionary.AddConnectionId(connectionId, userName);
            if (result != null)
            {
                SendUserConnectedSignal(result);
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine("ConnectionID DisConnected : " + connectionId);
            await RemoveConnection(connectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task RemoveConnection(string connectionId)
        {
            await Task.Delay(5000);
            var result = await _threadSafeDictionary.RemoveConnectionId(connectionId);
            if (result != null)
            {
                SendUserDisConnectedSignal(result);
            }
        }
        public async void SendUserDisConnectedSignal(string userName)
        {
            Console.WriteLine("User Disconnected" + userName);
            await Clients.All.SendAsync("userDisConnected", userName);
        }
        public async void SendUserConnectedSignal(string userName)
        {
            Console.WriteLine("User Connected" + userName);
            await Clients.All.SendAsync("userConnected", userName);
        }
        public async Task<string[]> GetActiveUserNames(string[] friendUserNames)
        {
            Console.WriteLine("Invoked from client side");
            //_threadSafeDictionary.PrintDictionary();
            var activeUsers = _threadSafeDictionary.GetActiveusers();
            var activeFriends = activeUsers.Intersect(friendUserNames).ToArray();
            return activeFriends;

        }

    }
}
