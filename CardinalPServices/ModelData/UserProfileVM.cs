using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalPServices.ModelData
{
    public class UserProfileVM
    {
       
        public int? UserID { get; set; }
        public string? UserName { get; set; }

        public string? DeviceID { get; set; }

        public string? EmailID { get; set; }
        public int? RoleID { get; set; }
        public string? UserPassword { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? Role { get; set; }

        public int? UserDelID { get; set; }
    }
}
