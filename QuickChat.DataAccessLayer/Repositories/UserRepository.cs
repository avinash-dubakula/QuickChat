using Microsoft.EntityFrameworkCore;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.DataAccessLayer.Context;

namespace QuickChat.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyIdentityDbContext _myIdentityDbContext;
        public UserRepository(MyIdentityDbContext myIdentityDbContext)
        {
            _myIdentityDbContext = myIdentityDbContext;
        }
        public async Task<string?> GetUserId(string userName)
        {
            var res = await _myIdentityDbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            if (res != null)
            {
                return res.Id;
            }
            return null;

        }

        public async Task<IEnumerable<UserData>> GetUsers(string userName, int noOfMatches, string searchText = "")
        {
            if (searchText.Length == 0)
            {
                var users = _myIdentityDbContext.Users
                    .Where(item => item.UserName != userName)
                    .Take(noOfMatches)
                    .Select(item => new UserData()
                    {
                        Email = item.Email,
                        FullName = item.FirstName + " " + item.LastName,
                        UserName = item.UserName
                    }).ToList();
                return users;
            }
            else
            {
                var query = _myIdentityDbContext.Users.Where(user => (user.UserName.Contains(searchText) ||
                                    user.Email.Contains(searchText) ||
                                    user.PhoneNumber.Contains(searchText)) && (!user.UserName.Equals(userName)));
                var users = await query.Take(noOfMatches).Select(item => new UserData()
                {
                    Email = item.Email,
                    FullName = item.FirstName + " " + item.LastName,
                    UserName = item.UserName
                    ,
                    ProfilePhotoUrl = item.ProfilePhotoUrl
                }).ToListAsync();
                return users;
            }
        }
    }
}
