using CardinalPServices.ModelData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalPServices.Interfaces
{
    public interface IPatient
    {
        IList<VMPatientModel> GetPatientList(int UserID);

        int AddPatientInfo(VMPatientModel patientModel);
        DataTable GetRiskFactor(int? PatientID);

        int DeletePatientData(int? PatientID);
    }
}
