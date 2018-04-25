namespace VessiFlowers.Business.Services
{
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using VessiFlowers.Business.Contracts;

    public class EmailService : IEmailService
    {
        private readonly string host = "smtp.gmail.com";

        private readonly int port = 587;

        private readonly bool ssl = true;

        private readonly string user = ConfigurationManager.AppSettings["Email"];

        private readonly string pass = ConfigurationManager.AppSettings["Password"];

        public void Send(string from, string to, string subject, string body, bool isHtml = false)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Host = this.host;
                smtp.Port = this.port;
                smtp.EnableSsl = this.ssl;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(this.user, this.pass);

                using (var message = new MailMessage(from, to))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;
                    smtp.Send(message);
                }
            }
        }
    }
}