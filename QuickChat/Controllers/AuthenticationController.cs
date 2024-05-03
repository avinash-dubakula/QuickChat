using Microsoft.AspNetCore.Mvc;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Error;

namespace QuickChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationRepository _service;

        public AuthenticationController(IAuthenticationRepository service)

        {

            _service = service;

        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] SignUpModel registerUser)
        {


            var result = await _service.RegisterUser(registerUser, "User");

            if (result.StartsWith("User created successfully"))
            {

                return Ok(result);

            }
            if (result.StartsWith("User with this email"))
            {

                var response = new ValidationErrorResponse
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        {"Email",new []{"Email alredy Exists"} }
                }
                };

                return BadRequest(response);

            }
            if (result.StartsWith("UserName already Exists"))
            {

                var response = new ValidationErrorResponse
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        {"UserName",new []{"UserName alredy Exists"} }
                    }
                };
                return BadRequest(response);

            }
            return BadRequest(result);

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)

        {

            var data = await _service.Login(loginModel);

            if (data != null)

            {

                return Ok(data);

            }

            return Unauthorized();

        }
    }
}
