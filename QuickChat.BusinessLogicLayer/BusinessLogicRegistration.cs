using Microsoft.Extensions.DependencyInjection;
using QuickChat.BusinessLogicLayer.Hubs;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.BusinessLogicLayer.Services;

namespace QuickChat.BusinessLogicLayer
{
    public static class BusinessLogicRegistration
    {
        public static void AddBusinessLogicService(this IServiceCollection services)
        {
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IFriendshipService, FriendShipService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddSingleton<ThreadSafeDictionary, ThreadSafeDictionary>();
        }
    }
}
