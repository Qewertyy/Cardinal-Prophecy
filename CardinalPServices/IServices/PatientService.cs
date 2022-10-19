using CardinalPServices.Interfaces;
using CardinalPServices.ModelData;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CardinalPServices.IServices
{
    public class PatientService : IPatient
    {
        private readonly string _StrCon;

        public PatientService(IConfiguration configuration)
        {
            _StrCon = configuration.GetConnectionString("IOTDB").ToString();
        }

        public int AddPatientInfo(VMPatientModel patientModel)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_AddPatientInfo";
                    cmd.Parameters.AddWithValue("@BloodPressure", patientModel.BloodPressure);
                    cmd.Parameters.AddWithValue("@ChlorestrolLvl", patientModel.ChlorestrolLvl);
                    cmd.Parameters.AddWithValue("@SugarLvl", patientModel.BloodPressure);
                    cmd.Parameters.AddWithValue("@UserID", patientModel.UserID);

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

        public int DeletePatientData(int? PatientID)
        {
            int iResult = 0;
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_DeleteUserData";
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);

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

        public IList<VMPatientModel> GetPatientList(int UserID)
        {
            DataTable dt = new DataTable();
            List<VMPatientModel> _patientList = new List<VMPatientModel>();
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_GetPatientDtl";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            _patientList.Add(new VMPatientModel
                            {
                                BloodPressure= dt.Rows[i]["BloodPressure"].ToString(),
                                PatientName = dt.Rows[i]["UserName"].ToString(),
                                UserName = dt.Rows[i]["UserName"].ToString(),
                                SugarLvl = dt.Rows[i]["SugarLvl"].ToString(),
                                PatientID = Convert.ToInt32(dt.Rows[i]["PatientID"].ToString()),
                                ChlorestrolLvl = dt.Rows[i]["ChlorestrolLvl"].ToString()
                            });
                        }
                    }
                }
            }
            return _patientList;
        }

        public DataTable GetRiskFactor(int? PatientID)
        {

            DataTable dt = new DataTable();
            using (SqlConnection Con = new SqlConnection(_StrCon))
            {
                Con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Prediction";
                    cmd.Parameters.AddWithValue("@PatientID", PatientID);

                    //SqlParameter iCOutput = new SqlParameter();
                    //iCOutput.ParameterName = "@COutput";
                    //iCOutput.SqlDbType = System.Data.SqlDbType.VarChar;
                    //iCOutput.Size = 250;
                    //cmd.Parameters.Add(iCOutput).Direction = System.Data.ParameterDirection.Output;

                    //SqlParameter iBpOutput = new SqlParameter();
                    //iBpOutput.ParameterName = "@BpOutput";
                    //iBpOutput.SqlDbType = System.Data.SqlDbType.VarChar;
                    //iBpOutput.Size = 250;
                    //cmd.Parameters.Add(iBpOutput).Direction = System.Data.ParameterDirection.Output;

                    //SqlParameter iSugOutput = new SqlParameter();
                    //iSugOutput.ParameterName = "@SugOutput";
                    //iSugOutput.SqlDbType = System.Data.SqlDbType.VarChar;
                    //iSugOutput.Size = 250;
                    //cmd.Parameters.Add(iSugOutput).Direction = System.Data.ParameterDirection.Output;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    //COutput = Convert.ToString(iCOutput.Value);
                    //BpOutput = Convert.ToString(iBpOutput.Value);
                    //SugOutput = Convert.ToString(iSugOutput.Value);
                }
            }
            return dt;
        }
    }
}
