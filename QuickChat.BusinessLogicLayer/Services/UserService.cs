using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserData>> GetUsers(string userName, string searchText)
        {
            var users = await _userRepository.GetUsers(userName, 10, searchText ?? string.Empty);
            return users;
        }
    }
}
