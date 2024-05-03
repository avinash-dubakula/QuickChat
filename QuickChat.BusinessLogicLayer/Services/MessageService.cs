using Microsoft.AspNetCore.Identity;
using QuickChat.BusinessLogicLayer.Enums;
using QuickChat.BusinessLogicLayer.IRepositories;
using QuickChat.BusinessLogicLayer.IServices;
using QuickChat.BusinessLogicLayer.Models.BusinessObjects;
using QuickChat.BusinessLogicLayer.Models.Entities;
using QuickChat.BusinessLogicLayer.Models.Entities.Identity;

namespace QuickChat.BusinessLogicLayer.Services
{
    public class MessageService : IMessageService
    {
        private readonly ISignalRService _signalRService;
        private readonly IMessageRepository _messageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly int _minimumMessageThreshold = 0;
        public MessageService(ISignalRService signalRService, IMessageRepository messageRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
        {
            _signalRService = signalRService;
            _messageRepository = messageRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task IncomingMessageSignalInvoke(string userName, MessageResponse messageResponseReciver)
        {
            await _signalRService.SendIncomingMessageSignal(userName, messageResponseReciver);
        }
        public async Task<MessageResponse> SendMessage(string senderUserName, string recieverUserName, string message)
        {
            var recieverUserId = (await _userManager.FindByNameAsync(recieverUserName)).Id;
            var senderUserId = (await _userManager.FindByNameAsync(senderUserName)).Id;
            if (await IsValidMessageRequest(senderUserName, recieverUserName, message))
            {
                var sendingresult = await _messageRepository.SendMessage(senderUserId, recieverUserId, message);
                if (sendingresult != null)
                {

                    var result1 = GetMessageData(sendingresult, senderUserId, recieverUserId);
                    var result2 = GetMessageData(sendingresult, recieverUserId, senderUserId);
                    var messageresponseSender = new MessageResponse()
                    {
                        Message = result1,
                        FriendUserName = recieverUserName,
                        BatchDate = result1.ActionAt.Value.ToString("yyyy-MM-dd")
                    };
                    var messageresponseReciever = new MessageResponse()
                    {
                        Message = result2,
                        FriendUserName = senderUserName,
                        BatchDate = result1.ActionAt.Value.ToString("yyyy-MM-dd")
                    };
                    if (recieverUserName != senderUserName)
                    {
                        IncomingMessageSignalInvoke(recieverUserName, messageresponseReciever);
                    }
                    return messageresponseSender;
                }
            }
            return null;
        }

        public async Task<bool> UpdateMessageToDelivered(string currentUserName, MessageUpdateModal messageUpdateModal)
        {
            var friendUserId = (await _userManager.FindByNameAsync(messageUpdateModal.FriendUserName)).Id;
            var currentUserId = (await _userManager.FindByNameAsync(currentUserName)).Id;
            if (await IsValidMessageRequest(currentUserName, messageUpdateModal.FriendUserName))
            {
                var messageInDB = await _messageRepository.GetMessage(messageUpdateModal.MessageId, currentUserId, friendUserId);
                if (messageInDB != null)
                {
                    var result = await _messageRepository.UpdateMessageStatus(messageUpdateModal.MessageId, messageUpdateModal.NewMessageStatus);
                    var messageData = GetMessageData(result, friendUserId, currentUserId);
                    var messageresponseReciever = new MessageResponse()
                    {
                        Message = messageData,
                        FriendUserName = currentUserName,
                        BatchDate = messageData.ActionAt.Value.ToString("yyyy-MM-dd")
                    };
                    _signalRService.SendMessageUpdateSignal(messageUpdateModal.FriendUserName, messageresponseReciever);
                    return true;
                }
            }
            return false;
        }
        public MessageData GetMessageData(UserMessage message, string currentUserId, string friendUserId)
        {
            if (message.Id == 0)
            {
                return null;// Guard clause for invalid message.
            }
            bool isIncoming = false;
            isIncoming = (friendUserId == message.SenderUserId && currentUserId != friendUserId);
            DateTime? actionTime = null;
            if (message.MessageStatus == MessageStatus.Sent)
            {
                // For outgoing messages, actionTime is when the message was sent.
                actionTime = message.MessageSentAt;
            }
            else if (message.MessageStatus == MessageStatus.Delivered || message.MessageStatus == MessageStatus.Seen)
            {
                // For incoming messages, actionTime is when the message was delivered.
                // For outgoing messages, it remains when the message was sent.
                actionTime = isIncoming ? message.MessageDeliveredAt : message.MessageSentAt;
            }

            var messageData = new MessageData()
            {
                Id = message.Id,
                IsIncoming = isIncoming,
                Message = message.MessageStatus == MessageStatus.Deleted ? "" : message.Message,
                MessageStatus = message.MessageStatus,
                ActionAt = actionTime,

            };
            return messageData;
        }

        public async Task<bool> UpdateMessagesToDelivered(string currentUserName, string currentUserId, DateTime dbFetchedTime)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                return false;
            }
            else
            {
                DateTime DeliveredTime = DateTime.Now;
                var senderNames = await _messageRepository.GetMessagesForDeliver(currentUserId, dbFetchedTime, DeliveredTime);
                if (senderNames != null && senderNames.Count() > 0)
                {
                    //Send Notification through signal R
                    MessagesUpdateModel model = new MessagesUpdateModel()
                    {
                        DeliveredTime = DeliveredTime,
                        DbFetchedTime = dbFetchedTime,
                        NewStatus = MessageStatus.Delivered,
                        FriendUserName = currentUserName,
                    };
                    _signalRService.SendMessagesUpdatedSignal(senderNames, model);
                }
                return true;
            }
        }
        public async Task<bool> UpdateMessagesToSeen(string currentUserId, MessagesSeenModel messagesSeenModel)
        {
            string? friendUserId = await _userRepository.GetUserId(messagesSeenModel.FriendUserName);
            if (string.IsNullOrEmpty(currentUserId) && string.IsNullOrEmpty(friendUserId))
            {
                return false;
            }
            var currentTime = DateTime.Now;
            var updateResult = await _messageRepository.UpdateMessagesForSeen(currentUserId, currentUserId, currentTime);
            if (updateResult > 1)
            {
                //Invoke SignalR
            }
            return true;
        }
        public async Task<bool> IsValidMessageRequest(string senderUserName, string recieverUserName, string? message = null)
        {
            var reciever = await _userManager.FindByNameAsync(recieverUserName);
            var sender = await _userManager.FindByNameAsync(senderUserName);
            if (reciever != null && sender != null && ((message == null) || (message != null && message.Length > 0)))
            {
                return true;
            }
            return false;
        }
        public async Task<ChatResponse> GetChatsAsync(string currentUserId, bool IsSpam, int minimumMessagesRequested)
        {
            minimumMessagesRequested = Math.Max(minimumMessagesRequested, _minimumMessageThreshold);
            var userfriendIds = await _messageRepository.GetFriendIdsAsync(currentUserId);
            userfriendIds.Add(currentUserId);
            var userChats = new List<ChatData>();
            var timeDbFetched = DateTime.Now;
            foreach (var friendUserId in userfriendIds)
            {
                var earliestUnreadMessageDate = await _messageRepository.GetEarliestUnDeliveredMessageDateAsync(currentUserId, friendUserId);
                DateTime startDate = earliestUnreadMessageDate ?? DateTime.Now;

                var messages = await _messageRepository.GetMessagesBetweenUsersAsync(currentUserId, friendUserId);
                // Group messages into batches based on the date
                //var messageDataList = messages.Select(um => GetMessageData(um, currentUserId, friendUserId)).ToList();

                // Now, group the converted MessageData objects by ActionAt (which reflects either MessageDeliveredAt or MessageSentAt)
                var groupedMessages = messages
                            .GroupBy(message => message.MessageSentAt.ToString("yyyy-MM-dd"))
                            .Select(g => new MessageBatch
                            {
                                BatchDate = g.Key, // g.Key is the Date part of ActionAt
                                messages = g.ToList().Select(msg => GetMessageData(msg, currentUserId, friendUserId))
                            })
                            .OrderByDescending(g => g.BatchDate) // Sort batches by BatchDate descending
                            .ToList();


                // Logic to filter batches based on criteria (batch size, unread messages, etc.)
                var resultBatches = new List<MessageBatch>();
                int noOfMessagesInResultBatch = 0;

                foreach (var batch in groupedMessages)
                {
                    bool batchIsAfterStartDate = DateTime.Parse(batch.BatchDate) >= DateTime.Parse(startDate.ToString("yyyy-MM-dd"));

                    if (batchIsAfterStartDate ||
                        noOfMessagesInResultBatch < minimumMessagesRequested)
                    {
                        batch.messages = batch.messages.OrderBy(msg => msg.ActionAt);
                        resultBatches.Insert(0, batch);
                        noOfMessagesInResultBatch += batch.messages.Count();
                    }
                    else
                    {
                        // Break the loop if the conditions to add more batches are not met
                        break;
                    }
                }
                var messagesData = new List<MessageData>();
                resultBatches.ForEach(rb =>
                {
                    messagesData.AddRange(rb.messages);
                });

                userChats.Add(new ChatData()
                {
                    Chat = await _messageRepository.GetChatModel(friendUserId, messagesData),
                    MessageBatches = resultBatches
                }); ;


            }
            userChats.OrderByDescending(userchat => userchat.Chat.LastMessageDate);

            var chatResponse = new ChatResponse()
            {
                ChatData = userChats,
                DataFetchedTime = timeDbFetched
            };

            return chatResponse;


        }
    }
}
