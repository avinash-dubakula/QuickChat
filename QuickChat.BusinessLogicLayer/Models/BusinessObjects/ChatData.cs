namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class ChatResponse
    {
        public IEnumerable<ChatData> ChatData { get; set; }
        public DateTime DataFetchedTime { get; set; }
    }
    public class ChatData
    {
        public Chat Chat { get; set; }
        public List<int> NewMessageIds { get; set; } = new List<int>();
        public List<MessageBatch> MessageBatches { get; set; }
        public int? SeenTillMessageId { get; set; } = null;

    }
}
