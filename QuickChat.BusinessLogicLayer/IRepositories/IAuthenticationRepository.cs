using QuickChat.BusinessLogicLayer.Models.BusinessObjects;

namespace QuickChat.BusinessLogicLayer.IRepositories
{
    public interface IAuthenticationRepository
    {
        Task<string> RegisterUser(SignUpModel registerUser, string role);
        Task<AuthenticationData> Login(LoginModel loginModel);

    }
}
