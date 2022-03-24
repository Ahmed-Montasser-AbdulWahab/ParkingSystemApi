using System;
using System.Net.Mail;
using System.Net;
using DotNetEnv;

namespace Parking_System_API.Email
{
    public class EmailCode
    {
        public static bool SendEmail(string ToEmail, string password, string mode = "Registration")
        {
            try
            {
                Env.Load();
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(System.Environment.GetEnvironmentVariable("EMAIL") ?? throw new ArgumentNullException()) ;
                mail.To.Add(ToEmail);
                mail.Subject = $"Parking System Account {mode}";
                mail.Body = $"Your Password is {password}";

                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(System.Environment.GetEnvironmentVariable("EMAIL") ?? throw new ArgumentNullException(), System.Environment.GetEnvironmentVariable("PASSWORD") ?? throw new ArgumentNullException());
                smtp.EnableSsl = true;

                smtp.Send(mail);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
