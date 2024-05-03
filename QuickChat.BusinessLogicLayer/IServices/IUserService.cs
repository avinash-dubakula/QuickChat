using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IServices
{
    public interface IUserService
    {
        public Task<IEnumerable<UserData>> GetUsers(string userName, string searchText);
    }
}
