using System.ComponentModel.DataAnnotations;

namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string? UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
