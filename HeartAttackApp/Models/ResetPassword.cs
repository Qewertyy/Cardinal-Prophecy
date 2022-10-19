using System.ComponentModel.DataAnnotations;

namespace HeartAttackApp.Models
{
    public class ResetPassword
    {
        [Required]
        [StringLength(15, ErrorMessage = "Must be between 6 and 15 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password should be of 6 characters minimum")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [RegularExpression(@"(?=^.{6,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage ="Password should consist of 6 characters, 1 special character and number")]
        public string? ConfirmPassword { get; set; }

        public string? RID { get; set; }
        
    }
}
