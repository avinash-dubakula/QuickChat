//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace QuickChat.BusinessLogicLayer.Filters.Action_Filters
//{


//    public class ManualJwtValidationAttribute : Attribute, IActionFilter
//    {
//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            var request = context.HttpContext.Request;
//            var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

//            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

//            // Validate the token
//            if (!ValidateToken(token, out var claimsPrincipal))
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            // Set the user in the HttpContext
//            context.HttpContext.User = claimsPrincipal;
//        }

//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//            // Can be used for logging, error handling, etc.
//        }

//        private bool ValidateToken(string token, out ClaimsPrincipal claimsPrincipal)
//        {
//            claimsPrincipal = null;
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var validationParameters = GetValidationParameters();

//            try
//            {
//                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        private TokenValidationParameters GetValidationParameters()
//        {
//            return new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = "YourIssuer", // Replace these with your valid issuer
//                ValidAudience = "YourAudience", // Replace these with your valid audience
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey")) // Replace with your signing key
//            };
//        }
//    }

//}
