using Phone_Ecommerce_Manage.Models;
using System.Net;
using System.Net.Mail;

namespace Phone_Ecommerce_Manage.Utilities
{
    public class SendMail
    {
        public static async Task SendGmail(string from, string to, string title, string content, string gmail, string password)
        {
            MailMessage message = new MailMessage(from, to, title, content);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                System.Net.NetworkCredential nc = new NetworkCredential(gmail, password);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;

                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        
    }
}
