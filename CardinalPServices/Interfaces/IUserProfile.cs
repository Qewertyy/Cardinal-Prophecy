using CardinalPServices.ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalPServices.Interfaces
{
    public interface IUserProfile
    {
        int IAdminRegistration(UserProfileVM userProfile, int UserID);
        int IUserRegistration(UserProfileVM userProfile);
        int IsValidUser(string? EmailID, string? UserPassword);
        UserProfileVM GetUserDetailByEmailID(string EmailID);

        UserProfileVM ResetPassword(string? EmailID, out int OutReturn);
        int LinkDuration(int? UserID, int ValidDurationInMin);

        int IsLinkValid(int? ID);

        int UpdateResetPassword(int? ID, string? UserPassword);

        IList<UserProfileVM> GetUsers();
        int DeleteUsers(int? UserID);
        //public List<UserProfileVM> Roles();
        int ChangePassword(int? UserID, string? CurrentPassowrd, string? ChangedPassword);

    }
}
