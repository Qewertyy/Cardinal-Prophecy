using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using CardinalPServices.Interfaces;
using CardinalPServices.ModelData;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace CardinalPServices.IServices
{
    public class UserProfileService : IUserProfile
    {
        private readonly string _StrCon;

        public UserProfileService(IConfiguration configuration)
        {
            _StrCon = configuration.GetConnectionString("IOTDB").ToString();
        }
        public int IsValidUser(string? EmailID, string? UserPassword)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_ValidateUser";
                    cmd.Parameters.AddWithValue("@EmailID", EmailID);
                    cmd.Parameters.AddWithValue("@UserPassword", UserPassword);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }

        public int IUserRegistration(UserProfileVM userProfile)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_UserRegistration";
                    cmd.Parameters.AddWithValue("@EmailID", userProfile.EmailID);
                    //cmd.Parameters.AddWithValue("@DeviceID", userProfile.DeviceID);
                    cmd.Parameters.AddWithValue("@UserName", userProfile.UserName);
                    cmd.Parameters.AddWithValue("@UserPassword", userProfile.UserPassword);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }
        public UserProfileVM GetUserDetailByEmailID(string EmailID)
        {
            DataTable dt = new DataTable();
            UserProfileVM objUser = new UserProfileVM();
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_GetUserDtlByEmailID";
                    cmd.Parameters.AddWithValue("@EmailID", EmailID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                objUser.UserName = dt.Rows[0]["UserName"].ToString();
                objUser.UserID = Convert.ToInt32(dt.Rows[0]["UserID"].ToString());
                objUser.RoleID = Convert.ToInt32(dt.Rows[0]["RoleID"].ToString());
            }
            return objUser;
        }

        public UserProfileVM ResetPassword(string? EmailID, out int OutReturn)
        {
            DataTable dt = new DataTable();
            UserProfileVM objUser = new UserProfileVM();
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_ResetPassword";
                    cmd.Parameters.AddWithValue("@EmailID", EmailID);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        objUser.UserName = dt.Rows[0]["UserName"].ToString();
                        objUser.UserID = Convert.ToInt32(dt.Rows[0]["UserID"].ToString());
                    }

                    OutReturn = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return objUser;

        }

        public int LinkDuration(int? UserID, int ValidDurationInMin)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InsertResetUserPassword";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@ValidDurationInMin", ValidDurationInMin);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }

        public int IsLinkValid(int? ID)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_IsLinkValid";
                    cmd.Parameters.AddWithValue("@ID", ID);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }

        public int UpdateResetPassword(int? ID, string? UserPassword)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_UpdateResetPassword";
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@UserPassword", UserPassword);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }

        public IList<UserProfileVM> GetUsers()
        {
            DataTable dt = new DataTable();
            List<UserProfileVM> ListofObj = new List<UserProfileVM>();
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_BindAdminDtlList";

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListofObj.Add(new UserProfileVM
                    {
                        UserID = Convert.ToInt32(dt.Rows[i]["UserID"].ToString()),
                        UserName = dt.Rows[i]["UserName"].ToString(),
                        EmailID = dt.Rows[i]["EmailID"].ToString(),
                        Role = dt.Rows[i]["Role"].ToString(),
                        CreatedBy = dt.Rows[i]["CreatedBy"].ToString(),
                        CreatedAt = dt.Rows[i]["CreatedAt"].ToString()
                    });
                }
            }
            return ListofObj;
        }

        public int DeleteUsers(int? UserID)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_DeleteUser";
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }

        public int IAdminRegistration(UserProfileVM userProfile, int UserID)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_AdminRegistration";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@EmailID", userProfile.EmailID);
                    cmd.Parameters.AddWithValue("@UserName", userProfile.UserName);
                    cmd.Parameters.AddWithValue("@UserPassword", userProfile.UserPassword);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }

        public int ChangePassword(int? UserID, string? CurrentPassowrd, string? ChangedPassword)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_ChangeUserPassword";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@CurrentPassword", CurrentPassowrd);
                    cmd.Parameters.AddWithValue("@ChangedPassword", ChangedPassword);

                    SqlParameter iResultParameter = new SqlParameter();
                    iResultParameter.ParameterName = "@Output";
                    iResultParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cmd.Parameters.Add(iResultParameter).Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    iResult = Convert.ToInt32(iResultParameter.Value);
                }
                Con.Close();
            }
            return iResult;
        }
    }
}

