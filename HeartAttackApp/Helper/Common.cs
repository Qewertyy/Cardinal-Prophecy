using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using HeartAttackApp.Controllers;

namespace HeartAttackApp.Helper
{
    public class Common
    {
        private IConfiguration Configuration;
   
        public Common(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public  string RandomPasswordGenerate(int Passlength)
        {
            int i = 0;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (i < Passlength)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
                i++;
            }
            return res.ToString();

        }
        public  void createuser(string RandomPwd,string EmailID,string UserName)
        {
            string EmailBody = string.Empty; 
            EmailBody = PopulateBody("CreateUser.html");
            EmailBody = EmailBody.Replace("{#EmailID}", EmailID);
            EmailBody = EmailBody.Replace("{#Name}", UserName);
            EmailBody = EmailBody.Replace("{#Password}", RandomPwd);
            SendMail("User Credentials", true, EmailBody, EmailID);
        }
        public void ForgotPassword(string EmailID, string UserName,string UrlLink)
        {
            string EmailBody = string.Empty;
            EmailBody = PopulateBody("ResetPassword.html");
            EmailBody = EmailBody.Replace("{#Name}", UserName);
            EmailBody = EmailBody.Replace("{#ForgotPasswordLink}", UrlLink);
            SendMail("Forgot Password Request", true, EmailBody, EmailID);
        }
        public void ChangePassword(string EmailID, string Password, string UserName)
        {
            string EmailBody = string.Empty;
            EmailBody = PopulateBody("ChangePassword.html");
            EmailBody = EmailBody.Replace("{#Name}", UserName);
            EmailBody = EmailBody.Replace("{#UserEmail}", EmailID);
            EmailBody = EmailBody.Replace("{#Time}", DateTime.Now.ToString());
            EmailBody = EmailBody.Replace("{#Password}", Password);

            SendMail("Password Changed Succesfully", true, EmailBody, EmailID);
        }
        private void  SendMail(string Subj, bool allowHTML, string strBody, string EmailTo)
        {
            try
            {
                string fromEmail = Configuration.GetSection("MailCredentials")["FromEmail"];
                string Pwd = Configuration.GetSection("MailCredentials")["Password"];
                string strMailServer = Configuration.GetSection("MailCredentials")["HostName"];
                string Port = Configuration.GetSection("MailCredentials")["Port"];
                string MailNotSent = string.Empty;
                string str = string.Empty;
                SmtpClient emailClient = new SmtpClient();
                MailMessage message = new MailMessage(fromEmail, EmailTo);
                message.IsBodyHtml = allowHTML;
                message.Subject = Subj;
                message.Body = strBody;
                emailClient.Port = Convert.ToInt32(Port);
                NetworkCredential NC = new NetworkCredential();
                NC.UserName = fromEmail;
                NC.Password = Pwd;
                emailClient.UseDefaultCredentials = false;
                emailClient.EnableSsl = true;
                emailClient.Credentials = NC;
                emailClient.Host = strMailServer;
                emailClient.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string  PopulateBody(String MailTemplates)
        {
            string body = string.Empty;
            string path = System.IO.Directory.GetCurrentDirectory();
            using (StreamReader reader = new StreamReader(string.Format("{0}\\MailTemplates\\{1}", path, MailTemplates)))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
        public static string? GetDmyToMdy(string strDate)
        {
            if (!string.IsNullOrEmpty(strDate))
            {
                return DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            }
            else
            {
                return null;
            }
        }
        public static string? GetMdyToDmy(string strDate)
        {
            if (!string.IsNullOrEmpty(strDate))
            {
                return Convert.ToDateTime(strDate).ToString("dd/MM/yyyy");
            }
            else
            {
                return null;
            }

        }
    }
}
