namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class MessageBatch
    {
        public string BatchDate { get; set; }

        public IEnumerable<MessageData> messages { get; set; }
    }
}
