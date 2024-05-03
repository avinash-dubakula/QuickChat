namespace QuickChat.BusinessLogicLayer.Models.Entities
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public string? ProfilePhotoUrl { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public IEnumerable<UserGroupParticipant> UserGroupParticipants { get; set; }
    }
}
