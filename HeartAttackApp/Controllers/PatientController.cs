using CardinalPServices.Interfaces;
using CardinalPServices.ModelData;
using Microsoft.AspNetCore.Mvc;
using HeartAttackApp.Filters;
using HeartAttackApp.Models;
using HeartAttackApp.Helper;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using System.Data;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Document = iTextSharp.text.Document;
using iTextSharp.text.pdf;
using CardinalPServices.IServices;

namespace HeartAttackApp.Controllers
{
    [CheckSession]
    public class PatientController : Controller
    {
        private IPatient _PatientService;
        public PatientController(IPatient patient)
        {
            _PatientService = patient;
        }
        public ActionResult PatientList()
        {
            PatientList model = new PatientList();
            model.patientList = _PatientService.GetPatientList(Convert.ToInt32(HttpContext.Session.GetString("UserID")));
            return View(model);
        }
        public ActionResult PatientInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientInfo(PatientModel patientModel)
        {
            int iResult = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    VMPatientModel objpatient = new VMPatientModel();
                    objpatient.UserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                    objpatient.UserName = String.Empty;
                    objpatient.SugarLvl = patientModel.SugarLvl;
                    objpatient.BloodPressure = patientModel.BloodPressure;
                    objpatient.ChlorestrolLvl = patientModel.ChlorestrolLvl;

                    iResult = _PatientService.AddPatientInfo(objpatient);
                    if (iResult >= 1)
                    {
                         
                        return RedirectToAction("PatientList");
                    }
                    else if (iResult == 2)
                    {
                        TempData["Message"] = "Try Again";
                        return RedirectToAction("PatientList");
                    }
                    else
                    {
                        TempData["Message"] = "Try Again";
                        return RedirectToAction("PatientList");
                    }
                }
                else
                {
                    TempData["Message"] = "Try Again";
                    return RedirectToAction("PatientList");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Try Again";
                return RedirectToAction("PatientList");
            }
        }

        public ViewResult RiskFactor()
        {
           
            return View();
        }

        [HttpGet]
        public ActionResult GetFactor(int iResult)
        {
            iResult = Convert.ToInt32(Encryption.Decrypt(Request.Query["iResult"].ToString()));
            DataTable dt = new DataTable();
            RiskFactorCalculation objRisk = new RiskFactorCalculation();
            dt = _PatientService.GetRiskFactor(iResult);
            objRisk.ColRiskPercent= Convert.ToDecimal(dt.Rows[0]["Chlorestrol%"].ToString());
            objRisk.SugRiskPercent = Convert.ToDecimal(dt.Rows[0]["Diabetes%"].ToString());
            objRisk.BpRiskPercent = Convert.ToDecimal(dt.Rows[0]["BP%"].ToString());
            objRisk.AvgRiskPercent = Convert.ToDecimal(dt.Rows[0]["AverageRisk%"].ToString());
            objRisk.BpRisk = dt.Rows[0]["BloodPressure"].ToString();
            objRisk.ColRisk = dt.Rows[0]["Chlorestrol"].ToString();
            objRisk.SugRisk = dt.Rows[0]["SugarLevel"].ToString();
            return View("RiskFactor", objRisk);
        }

        public ActionResult DeletePatientData()
        {
            try
            {
                string UserIDfrmView = Request.Query["PatientID"].ToString();
                if (!string.IsNullOrEmpty(UserIDfrmView))
                {
                    Console.WriteLine("Decrypt" + Encryption.Decrypt(UserIDfrmView));
                    int iResult = _PatientService.DeletePatientData(Convert.ToInt32(Encryption.Decrypt(UserIDfrmView.ToString())));
                    if (iResult == 1)
                    {
                        return RedirectToAction("PatientList", "Patient");
                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong, try again later";
                        return RedirectToAction("PatientList", "Patient");
                    }
                }
                else
                {
                    TempData["Message"] = "Something Went Wrong, try again later";
                    return RedirectToAction("PatientList", "Patient");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something Went Wrong, try again later";
                return RedirectToAction("PatientList", "Patient");
            }
        }

        //[HttpPost]
       // public FileResult ExportToPDF(string GridHtml)
        //{
          //  using (MemoryStream stream = new System.IO.MemoryStream())
            //{
              //  StringReader sr = new StringReader(GridHtml);
                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                //pdfDoc.Open();
                //PdfContentByte cb = writer.DirectContent;

                // select the font properties
                //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //cb.SetColorFill(BaseColor.DARK_GRAY);
                //cb.SetFontAndSize(bf, 8);

                //// write the text in the pdf content
                //cb.BeginText();
                //string text = "&copy; + Cardinal Prophecy 2022";
                //// put the alignment and coordinates here
                //cb.ShowTextAligned(1, text, 420, 640, 0);
                //cb.EndText();
                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                //pdfDoc.Close();
                //return File(stream.ToArray(), "application/pdf", "PredictionResults.pdf");
            //}
        //}
    }

}
