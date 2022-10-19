using System.ComponentModel.DataAnnotations;

namespace HeartAttackApp.Models
{
    public class PatientModel
    {
        [Key]
        public int? PatientID { get; set; }

        public string? PatientName { get; set; }


        [Required(ErrorMessage ="Please Enter Correct Values")]
        [Display(Name = "Chlorestrol Level")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Values can only be in numeric")]
        public string? ChlorestrolLvl { get; set; }


        [Required(ErrorMessage = "Please Enter Correct Values")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Values can only be in numeric")]
        [Display(Name = "Blood Pressure")]
        public string? BloodPressure { get; set; }


        [Required(ErrorMessage = "Please Enter Correct Values")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Values can only be numeric")]
        [Display(Name = "Sugar Level")]
        public string? SugarLvl { get; set; }

        public int? UserID { get; set; }
        public string? UserName { get; set; }
    }
}
