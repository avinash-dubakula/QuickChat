using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IRepositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserData>> GetUsers(string userName, int noOfMatches, string searchText = "");
        public Task<string?> GetUserId(string userName);
    }
}
