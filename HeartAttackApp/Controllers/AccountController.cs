using HeartAttackApp.Models;
using Microsoft.AspNetCore.Mvc;
using CardinalPServices;
using CardinalPServices.Interfaces;
using CardinalPServices.ModelData;
using HeartAttackApp.Helper;
using System.Web;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using HeartAttackApp.Filters;

namespace HeartAttackApp.Controllers
{
    public class AccountController : Controller
    {

        private IUserProfile _UserProfileService;
        private IConfiguration _configuration;

        public AccountController(IUserProfile user, IConfiguration configuration)
        {
            _UserProfileService = user;
            _configuration = configuration;


        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            int iResult = 0;
            if (ModelState.IsValid)
            {
                //string strPwd1 = Encryption.Decrypt(login.UserPassword);
                string strPwd = Encryption.Encrypt(login.UserPassword);
                iResult = _UserProfileService.IsValidUser(login.EmailID, strPwd);
                if (iResult == 1)
                {
                    UserProfileVM objUser = _UserProfileService.GetUserDetailByEmailID(login.EmailID);
                    if (objUser != null)
                    {
                        HttpContext.Session.SetString("EmailID", login.EmailID);
                        HttpContext.Session.SetString("UserID", Convert.ToString(objUser.UserID));
                        HttpContext.Session.SetString("UserName", objUser.UserName);
                        HttpContext.Session.SetString("RoleID", Convert.ToString(objUser.RoleID));
                        ViewBag.RoleID = objUser.RoleID;
                        ViewData["RoleID"] = objUser.RoleID;
                        if (objUser.RoleID != 1)
                        {
                            return RedirectToAction("PatientList", "Patient");
                        }
                        else { return RedirectToAction("Users", "Account"); }
                    }
                    else
                    {
                        TempData["Message"] = "Something went wrong!";
                        return View(login);
                    }

                }
                else if (iResult == 2)
                {
                    TempData["Message"] = "Please Check Your Password again!";
                    return View(login);

                }
                else if (iResult == 3)
                {
                    TempData["Message"] = "User Doesn't Exist with this Email";
                    return View(login);
                }
                else
                {
                    TempData["Message"] = "Something went wrong! please try again later";
                    return View(login);
                }
            }
            else
            {
                TempData["Message"] = "Please Enter proper values!";
                return View();
            }

        }

        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserProfile userProfile)
        {
            int iResult = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    Common objSentEmail = new Common(_configuration);
                    UserProfileVM objProfile = new UserProfileVM();
                    string strRandomPwd = objSentEmail.RandomPasswordGenerate(6);
                    objProfile.EmailID = userProfile.EmailID;
                    objProfile.UserName = userProfile.UserName;
                    objProfile.UserPassword = Encryption.Encrypt(strRandomPwd);

                    iResult = _UserProfileService.IUserRegistration(objProfile);
                    if (iResult == 1)
                    {
                        objSentEmail.createuser(strRandomPwd, objProfile.EmailID, objProfile.UserName);

                        TempData["Message"] = "Registered Successfully!";
                        return RedirectToAction("Login");

                    }
                    else if (iResult == 2)
                    {
                        TempData["Message"] = "User already exist!";
                        return View(userProfile);

                    }
                    else
                    {
                        TempData["Message"] = "User not registered!";
                        return View(userProfile);
                    }

                }
                else
                {
                    TempData["Message"] = "User not registered!";
                    return View(userProfile);
                }

            }
            catch (Exception ex)
            {

                TempData["Message"] = "User not registered!";
                return View(userProfile);
            }

        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public ViewResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string EmailID)
        {
            int OutReturn = 0;
            try
            {
                if (!string.IsNullOrEmpty(EmailID))
                {
                    UserProfileVM objup = _UserProfileService.ResetPassword(EmailID, out OutReturn);
                    if (OutReturn == 1)
                    {
                        int Duration = Convert.ToInt32(_configuration.GetSection("LinkDuration")["DefaultDuration"]);
                        int ILinkD = _UserProfileService.LinkDuration(objup.UserID, Duration);

                        if (ILinkD != 0)
                        {
                            string URl = "https://" + HttpContext.Request.Host.Value + "/Account/ConfirmResetPassword?RID=" + HttpUtility.UrlEncode(Encryption.Encrypt(ILinkD.ToString()));
                            new Common(_configuration).ForgotPassword(EmailID, objup.UserName, URl);

                            TempData["Message"] = "We have sent a link to reset your password to your registered email address";
                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong, try again later";
                        }

                        return View();
                    }
                    else
                    {
                        TempData["Message"] = "User Doesn't Exist";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something Went Wrong, try again later";
            }

            return View();
        }

        public ActionResult ConfirmResetPassword()
        {
            string User_ID = Request.Query["RID"].ToString();
            if (!string.IsNullOrEmpty(User_ID))
            {
                int UserIDfrmLink = Convert.ToInt32(Encryption.Decrypt(User_ID));
                int iResult = _UserProfileService.IsLinkValid(UserIDfrmLink);

                if (iResult == 0)
                {

                    return RedirectToAction("LinkExpired");
                }
                else
                {
                    ViewBag.RID = Encryption.Encrypt(UserIDfrmLink.ToString());
                    return View();
                }
            }
            return RedirectToAction("LinkExpired");
        }

        [HttpPost]
        public ActionResult ConfirmResetPassword(ResetPassword resetPassword)
        {

            if (ModelState.IsValid)
            {
                int iResult = _UserProfileService.UpdateResetPassword(Convert.ToInt32(Encryption.Decrypt(resetPassword.RID)), Encryption.Encrypt(resetPassword.Password));
                if (iResult == 1)
                {
                    TempData["Message"] = "Password reset successfully!";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Message"] = "Something Went Wrong, try again later";
                    return View();
                }
            }
            else
            {
                TempData["Message"] = "Something Went Wrong, try again later";
                return View();
            }


        }

        public ViewResult LinkExpired()
        {
            return View();
        }

        [CheckSession]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            try
            {
                int iResult = 0;

                if (ModelState.IsValid)
                {
                    Common objSentEmail = new Common(_configuration);
                    UserProfileVM objProfile = new UserProfileVM();
                    string SessionEmailID = HttpContext.Session.GetString("EmailID");
                    string SessionUsername = HttpContext.Session.GetString("UserName");
                    iResult = _UserProfileService.ChangePassword(Convert.ToInt32(HttpContext.Session.GetString("UserID")), Encryption.Encrypt(changePassword.CurrentPassword).ToString(), Encryption.Encrypt(changePassword.ConfirmPassword));
                    if (iResult == 3)
                    {
                        objSentEmail.ChangePassword(SessionEmailID, changePassword.ConfirmPassword, SessionUsername);

                        TempData["Message"] = "Password Changed Successfully! \\n You''ll be logged out now";
                        return RedirectToAction("LogOut");

                    }
                    else if (iResult == 2)
                    {
                        TempData["Message"] = "New Password Shouldn't be same as the currrent one";
                        return View();

                    }
                    else if (iResult == 1)
                    {
                        TempData["Message"] = "Current Password is wrong";
                        return View();

                    }
                    else if (iResult == 4)
                    {
                        TempData["Message"] = "Please enter correct values";
                        return View();
                    }
                    else
                    {
                        TempData["Message"] = "Something Went wrong, try again later";
                        return View();
                    }
                }
                else
                {
                    TempData["Message"] = "Something Went wrong, try again later";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something Went wrong, try again later";
                return View();
            }
 
        }

        [CheckSession]
        public ActionResult Users()
        {
            UserList obj = new();
            try
            {
                if (Convert.ToInt32(HttpContext.Session.GetString("RoleID")) == 1)
                {
                    obj.userList = _UserProfileService.GetUsers();
                }
                else
                {
                    return RedirectToAction("UnAuthorized", "AccessDenied"); 
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something Went wrong, try again later";
            }
            return View(obj);
        }

        [CheckSession]
        public ActionResult DeleteUser()
        {
            try
            {
                string UserIDfrmView = Request.Query["UserID"].ToString();
                if (!string.IsNullOrEmpty(UserIDfrmView))
                {
                    Console.WriteLine("Decrypt" + Encryption.Decrypt(UserIDfrmView));
                    int iResult = _UserProfileService.DeleteUsers(Convert.ToInt32(Encryption.Decrypt(UserIDfrmView.ToString())));
                    if (iResult == 1)
                    {
                        return RedirectToAction("Users", "Account");
                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong, try again later";
                        return RedirectToAction("Users", "Account");
                    }
                }
                TempData["Message"] = "Something Went Wrong, try again later";
                return RedirectToAction("Users", "Account");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something Went Wrong, try again later";
                return RedirectToAction("Users", "Account");
            }

        }
        [CheckSession]
        public ActionResult AddAdmin()
        {
            if (Convert.ToInt32(HttpContext.Session.GetString("RoleID")) == 1)
            { return View(); }
            else
            {
                return RedirectToAction("UnAuthorized", "AccessDenied");
            }
        }

        [HttpPost]
        [CheckSession]
        public ActionResult AddAdmin(UserProfile userProfile)
        {
            int iResult = 0;
            try
            {
                if (Convert.ToInt32(HttpContext.Session.GetString("RoleID")) == 1)
                {
                    if (ModelState.IsValid)
                    {
                        Common objSentEmail = new Common(_configuration);
                        UserProfileVM objProfile = new UserProfileVM();
                        string strRandomPwd = objSentEmail.RandomPasswordGenerate(6);
                        objProfile.EmailID = userProfile.EmailID;
                        objProfile.UserName = userProfile.UserName;
                        //objProfile.DeviceID = userProfile.DeviceID;
                        objProfile.UserPassword = Encryption.Encrypt(strRandomPwd);

                        iResult = _UserProfileService.IAdminRegistration(objProfile, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                        if (iResult == 1)
                        {
                            objSentEmail.createuser(strRandomPwd, objProfile.EmailID, objProfile.UserName);
                            TempData["Message"] = "Admin Registered Successfully!";
                            return RedirectToAction("Users", "Account");
                        }
                        else if (iResult == 2)
                        {
                            TempData["Message"] = "User already exist!";
                            return View(userProfile);

                        }
                        else
                        {
                            TempData["Message"] = "User not registered!";
                            return View(userProfile);
                        }

                    }
                    else
                    {
                        TempData["Message"] = "User not registered!";
                        return View(userProfile);
                    }
                }
                else
                {
                    return RedirectToAction("UnAuthorized", "AccessDenied");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = "User not registered!";
                return View(userProfile);
            }
        }

    }
}