using System.ComponentModel.DataAnnotations;

namespace QuickChat.BusinessLogicLayer.Models.BusinessObjects
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "This is not a valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
    }
}
