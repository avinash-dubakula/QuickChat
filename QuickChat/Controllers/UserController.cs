using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.IServices;

namespace QuickChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserHelp _userHelp;
        public UserController(IUserService userService, IUserHelp userHelp)
        {
            _userService = userService;
            _userHelp = userHelp;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers(string? searchText)
        {
            try
            {
                var userName = _userHelp.GetCurrentUserName();
                if (userName != null)
                {
                    var result = await _userService.GetUsers(userName, searchText ?? string.Empty);
                    return Ok(result);
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception)
            {

                return BadRequest("Some Internal Error");
            }
        }
    }
}
