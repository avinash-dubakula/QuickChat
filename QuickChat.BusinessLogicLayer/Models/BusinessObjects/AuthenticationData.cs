namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class AuthenticationData
    {
        public string userName { get; set; }
        public string userId { get; set; }
        public string emailId { get; set; }

        public string token { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
