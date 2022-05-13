

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TechnosoftModel;

namespace BussinessLayer
{
    public class EmailBL
    {
        private const string FROM_EMAIL = "asntechnosoft@gmail.com";
        private const string PASSWORD = "7066922020";

        private const string DISPLAY_NAME = "ASN Technosoft";

        public static void SendRegistrationEmail(User user)
        {
            MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(user.Username);
            mail.From = new MailAddress(FROM_EMAIL, DISPLAY_NAME, System.Text.Encoding.UTF8);
            mail.Subject = "Registration Successfully done!";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Thank you for registering with us. You can login and continue."+ Environment.NewLine + "<br /><b>URL Link</b>: asntechnosoft.com" + Environment.NewLine + "Username: " + user.Username + Environment.NewLine + "Password: " + user.Password;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            SendEmail(mail);
        }

        public static void SendResetPasswordMail(User user)
        {
            MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(user.Username);
            mail.From = new MailAddress(FROM_EMAIL, DISPLAY_NAME, System.Text.Encoding.UTF8);
            mail.Subject = "Password reset Successfully done!";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Your password reset successfully. You can login and continue." + Environment.NewLine + "<br /><b>URL Link</b>: asntechnosoft.com" + Environment.NewLine + "Username: " + user.Username + Environment.NewLine + "Password: " + user.Password;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            SendEmail(mail);
        }
        public static void SendStaffRegistrationEmail(User user)
        {
            MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(user.Username);
            mail.From = new MailAddress(FROM_EMAIL, DISPLAY_NAME, System.Text.Encoding.UTF8);
            mail.Subject = "Registration Successfully done!";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Thank you for connecting with us. You can login and continue." + Environment.NewLine + "<br /><b>URL Link</b>: asntechnosoft.com" + Environment.NewLine + "Username: " + user.Username + Environment.NewLine + "Password: " + user.Password;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            SendEmail(mail);
        }

        private static void SendEmail(MailMessage mail)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";   // We use gmail as our smtp client
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(FROM_EMAIL, PASSWORD);
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
 
    }
}
