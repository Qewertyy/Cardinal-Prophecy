using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalPServices.ModelData
{
    public class VMPatientModel
    {
        public int? PatientID  { get; set; }
        public string? PatientName { get; set; }
        public string? ChlorestrolLvl { get; set; }
        public string? BloodPressure { get; set; }
        public string? SugarLvl { get; set; }

        public string? UserName { get; set; }
        public int? UserID { get; set; }
    }
}
