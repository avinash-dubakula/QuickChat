using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.IServices;

namespace QuickChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendShipController : ControllerBase
    {
        private readonly IFriendshipService _friendShipService;
        private readonly IUserHelp _userHelp;
        public FriendShipController(IFriendshipService friendshipService, IUserHelp userHelp)
        {
            _friendShipService = friendshipService;
            _userHelp = userHelp;
        }

        [HttpGet("FriendRequest")]
        public async Task<IActionResult> GetAllFriendRequestAsync()
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _friendShipService.GetUserFriendRequests(userId);
                    return Ok(result);
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception)
            {

                return BadRequest("Some Internal Error");
            }
        }
        [HttpGet("Friends")]
        public async Task<IActionResult> GetAllFriendDetailsAsync()
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _friendShipService.GetUserFriendsData(userId);
                    return Ok(result);
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception)
            {

                return BadRequest("Some Internal Error");
            }
        }

        [HttpPost("Send/{recieverUserName}")]
        public async Task<IActionResult> SendFriendRequest(string recieverUserName)
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _friendShipService.SendFriendRequest(userId, recieverUserName);
                    if (result)
                    {

                        return Ok("Friend Request Sent Successfully");
                    }
                    else
                    {
                        return BadRequest("Sending Failed");
                    }
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("UserName Not Found"))
                {
                    return BadRequest("User Not Found");
                }
                if (ex.Message.StartsWith("Alredy sent a request"))
                {
                    return BadRequest($"Alredy sent a request to {recieverUserName}");
                }
                if (ex.Message.StartsWith("Sending a request to YourSelf is not Allowed"))
                {
                    return BadRequest("Sending a request to YourSelf is not Allowed");
                }
                return BadRequest("Some Internal Error" + ex.Message);
            }
        }
        [HttpPut("Accept/{recieverUserName}")]
        public async Task<IActionResult> AcceptFriendRequest(string recieverUserName)
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _friendShipService.AcceptFriendRequest(userId, recieverUserName);
                    return Ok("Friend Request Accepted Successfully");
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception)
            {

                return BadRequest("Some Internal Error");
            }
        }
        [HttpPut("Reject/{RequesterUserName}")]
        public async Task<IActionResult> RejetFriendRequest(string recieverUserName)
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _friendShipService.RejetFriendRequest(userId, recieverUserName);
                    return Ok("Friend Request Rejected Successfully");
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception)
            {

                return BadRequest("Some Internal Error");
            }
        }
        [HttpPut("Remove/{FriendUserName}")]
        public async Task<IActionResult> RemoveFriend(string FriendUserName)
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _friendShipService.RemoveFriend(userId, FriendUserName);
                    if (result == true)
                    {
                        return Ok("Friend Removed Successfully");
                    }
                    return BadRequest("Not a Valid User Name");
                }
                return BadRequest("Not A valid User");
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Cannot Find the Friendship"))
                {
                    return BadRequest(ex.Message);
                }
                if (ex.Message.StartsWith("You are Not Friends"))
                {
                    return BadRequest(ex.Message);
                }
                if (ex.Message.StartsWith("Cannot Update the Friend Request"))
                {
                    return BadRequest(ex.Message);
                }
                if (ex.Message.StartsWith("UserName Not Found"))
                {
                    return BadRequest(ex.Message);
                }
                return BadRequest("Some Internal Error");
            }
        }
    }
}
