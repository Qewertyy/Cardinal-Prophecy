using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HeartAttackApp.Models
{
    public class UserProfile
    {
        [Key]
        public int? UserID { get; set; }


        [Required(ErrorMessage ="Please Enter Name!")]
        public string? UserName { get; set; }
        //public List<SelectListItem> Roles { get; set; }

        //[Required(ErrorMessage = "Please Enter Device Info!")]
        //[DisplayName("Device ID")]
        //public string? DeviceID { get; set; }


        [Required(ErrorMessage = "Please Enter EmailID!")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid e-mail")]
        public string? EmailID { get; set; }

        public string? UserPassword { get; set; }

    }
}
