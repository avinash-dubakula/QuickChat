using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuickChat.DataAccessLayer.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        public async Task<string> RegisterUser(SignUpModel registerUser, string role)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
                if (userExist != null)
                {
                    return "User with this email already exists.";
                }
                userExist = await _userManager.FindByNameAsync(registerUser.UserName);
                if (userExist != null)
                {
                    return "UserName already Exists";
                }
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return "Role does not exist.";
                }

                ApplicationUser user = new ApplicationUser
                {

                    Email = registerUser.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerUser.UserName,
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName
                };

                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return "Failed to create user.";
                }

                await _userManager.AddToRoleAsync(user, role);
                return "User created successfully.";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }


        public async Task<AuthenticationData> Login(LoginModel loginModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserNameOrEmail);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(loginModel.UserNameOrEmail);
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var authenticationData = new AuthenticationData() { emailId = user.Email, userId = user.Id, userName = user.UserName, TokenExpiry = DateTime.Now.AddMinutes(30) };
                    var roles = await _userManager.GetRolesAsync(user);
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var roleClaims = new List<Claim>();

                    for (int i = 0; i < roles.Count; i++)
                    {
                        roleClaims.Add(new Claim("roles", roles[i]));
                    }

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub.ToString(), user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("Email", user.Email),
                        new Claim("UId", user.Id)
                    }
                    .Union(userClaims)
                    .Union(roleClaims);
                    var jwtToken = GetToken(claims);
                    authenticationData.token = new JwtSecurityTokenHandler().WriteToken(jwtToken); ;
                    return authenticationData;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(60 * 24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}