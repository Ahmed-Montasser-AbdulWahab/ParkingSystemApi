using System;
using System.Net.Mail;
using System.Net;
using DotNetEnv;

namespace Parking_System_API.Email
{
    public class EmailCode
    {
        public static bool SendEmail(string ToEmail, string password)
        {
            try
            {
                DotNetEnv.Env.Load();
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(System.Environment.GetEnvironmentVariable("EMAIL") ?? throw new NullReferenceException());
                mail.To.Add(ToEmail);
                mail.Subject = "Parking System Account Registration";
                mail.Body = $"Your Password is {password}";

                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(System.Environment.GetEnvironmentVariable("EMAIL") ?? throw new NullReferenceException(), System.Environment.GetEnvironmentVariable("PASSWORD") ?? throw new NullReferenceException());
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
