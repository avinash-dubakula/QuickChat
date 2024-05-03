using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.IServices;

namespace QuickChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserHelp _userHelp;
        public MessageController(IMessageService messageService, IUserHelp userHelp)
        {
            _messageService = messageService;
            _userHelp = userHelp;
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageModel messageModel)
        {
            try
            {
                var senderUserName = _userHelp.GetCurrentUserName();
                if (senderUserName != null)
                {
                    var result = await _messageService.SendMessage(senderUserName, messageModel.RecieverUserName, messageModel.Message);
                    if (result != null)
                    {
                        return Ok(result);

                    }
                }
                return Ok(false);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        [HttpPut("UpdateMessage")]
        public async Task<IActionResult> UpdateMessage(MessageUpdateModal messageUpdateModal)
        {
            try
            {
                var currentUserName = _userHelp.GetCurrentUserName();
                if (currentUserName != null)
                {
                    var result = await _messageService.UpdateMessageToDelivered(currentUserName, messageUpdateModal);
                    if (result == true)
                    {
                        return Ok(result);

                    }
                }
                return Ok(false);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        [HttpPut("RecievedAck/{latestfetchedDate}")]
        public async Task<IActionResult> RecievedAcknowledgement(string latestfetchedDate)
        {
            try
            {
                DateTime dbFetchedDate = DateTime.Parse(latestfetchedDate);
                var currentUserId = _userHelp.GetCurrentUserID();
                var currentUserName = _userHelp.GetCurrentUserName();
                if (currentUserId != null)
                {
                    var result = await _messageService.UpdateMessagesToDelivered(currentUserName, currentUserId, dbFetchedDate);
                    if (result == true)
                    {
                        return Ok(result);

                    }
                }
                return Ok(false);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        [HttpPut("SeenAck")]
        public async Task<IActionResult> SeenAcknowledgement(MessagesSeenModel model)
        {
            try
            {
                var currentUserId = _userHelp.GetCurrentUserID();
                var currentUserName = _userHelp.GetCurrentUserName();
                if (currentUserId != null)
                {
                    var result = await _messageService.UpdateMessagesToSeen(currentUserId, model);
                    return Ok(result);
                }
                return Ok(false);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        [HttpGet("Chats/{isSpam}/{minimumMessagesRequested}")]
        ///<summary>
        ///On Hitting this Action,Client will get a list of message batches
        /// if there are any messges that are not delivered to the Client, we will send all those batches
        /// else, we will send batches(dynamic number) so that it fulllfill the minimum threshold requested
        /// 
        /// </summary>
        public async Task<IActionResult> GetChats(bool isSpam, int minimumMessagesRequested)
        {
            try
            {
                var userId = _userHelp.GetCurrentUserID();
                if (userId != null)
                {
                    var result = await _messageService.GetChatsAsync(userId, isSpam, minimumMessagesRequested);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        //[HttpGet("Chat/{friendUserName}")]
        //public async Task<IActionResult> GetChat(string friendUserName)
        //{
        //    try
        //    {
        //        var userId = _userHelp.GetCurrentUserID();
        //        if (userId != null)
        //        {
        //            var result = await _messageService.GetChat(friendUserName, userId);
        //            return Ok(result);
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.StartsWith("User with"))
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //        return BadRequest();
        //    }

        //}
        //[HttpGet("Chat/Latest/{friendUserName}/{latestMessageId}")]
        //public async Task<IActionResult> GetLatestChat(string friendUserName, int latestMessageId)
        //{
        //    try
        //    {
        //        var userId = _userHelp.GetCurrentUserID();
        //        if (userId != null)
        //        {
        //            var result = await _messageService.GetLatestChat(friendUserName, userId, latestMessageId);
        //            return Ok(result);
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.StartsWith("User with"))
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //        return BadRequest();
        //    }

        //}
    }
}
