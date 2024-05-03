using QuickChat.IServices;
namespace QuickChat.Services
{
    public class UserHelp : IUserHelp
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserHelp(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetCurrentUserName()
        {
            return _httpContext.HttpContext?.User?.Identity?.Name;
        }

        public string GetCurrentUserID()
        {
            var userClaims = _httpContext.HttpContext?.User.Claims;

            // Find the claim for "UId" and retrieve its value
            var userId = userClaims?.FirstOrDefault(claim => claim.Type == "UId")?.Value;

            return userId;
        }

    }
}
