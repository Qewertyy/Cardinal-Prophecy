using System.ComponentModel.DataAnnotations;

namespace HeartAttackApp.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage ="Please Enter Current Password")]
        public string? CurrentPassword { get; set; }


        [Required(ErrorMessage ="Enter the new password")]
        [StringLength(15, ErrorMessage = "Must be between 6 and 15 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Please Confirm the new Password")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password should be of 6 characters minimum")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        [RegularExpression(@"(?=^.{6,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Password should consist of 6 characters, 1 special character and number")]
        public string? ConfirmPassword { get; set; }
    }
}
