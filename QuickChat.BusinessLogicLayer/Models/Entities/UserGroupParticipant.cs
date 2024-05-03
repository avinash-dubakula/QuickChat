
using QuickChat.BusinessLogicLayer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickChat.BusinessLogicLayer.Models.Entities
{
    public class UserGroupParticipant
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public GroupRole GroupRole { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime AddedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
