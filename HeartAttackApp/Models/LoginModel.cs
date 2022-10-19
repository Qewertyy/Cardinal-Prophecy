using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttackApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter your EmailID!")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid e-mail")]
        public string? EmailID { get; set; }


        [Required(ErrorMessage = "Please Enter your Password!")]
        [RegularExpression(@"(?=^.{6,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Password should consist of 6 characters, 1 special character and number")]
        public string? UserPassword { get; set; }
    }
}
