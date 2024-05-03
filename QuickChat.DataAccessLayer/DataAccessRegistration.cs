using Microsoft.Extensions.DependencyInjection;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.DataAccessLayer.Repositories;

namespace QuickChat.DataAccessLayer
{
    public static class DataAccessRegistration
    {

        public static void AddDataAccessService(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFriendhipRepository, FriendshipRepository>();
        }

    }
}
