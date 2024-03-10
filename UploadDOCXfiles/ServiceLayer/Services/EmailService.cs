using MailKit.Net.Smtp;
using MimeKit;
using ServiceLayer.Helpers;
using ServiceLayer.Proxy;
using ServiceLayer.Services.Abstract;
using ServiceLayer.SubModels;
using ISmtpClient = ServiceLayer.Services.Abstract.ISmtpClient;

namespace ServiceLayer.Services
{
    public class EmailService : IEmailServices
    {
        private readonly string login;
        private readonly string password;
        private readonly string address;

        public EmailService ()
        {
            login = SecretsHelper.GetSecret<EmailService>("Email", "Login");
            address = SecretsHelper.GetSecret<EmailService>("Email", "Address");
            password = SecretsHelper.GetSecret<EmailService>("Email", "Password");
        }
       
        /// <summary>
        /// Sends an email using the provided email information.
        /// </summary>
        /// <param name="emailInformation">The information required to compose and send the email.</param>
        public void SendEmail(EmailInformation emailInformation )
        {
            var client = new SmtpClientProxy();
            SendEmail(emailInformation, client);
        }

        /// <summary>
        /// Sends an email using the provided email information.
        /// </summary>
        /// <param name="emailInformation">The information required to compose and send the email.</param>
        /// <param name="client">Smtp client for email sending.</param>
        public void SendEmail(EmailInformation emailInformation, ISmtpClient client)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailInformation.From, address));
            message.To.Add(new MailboxAddress(emailInformation.To, emailInformation.Address));
            message.Subject = "Yours file";

            message.Body = new BodyBuilder()
            {
                HtmlBody = emailInformation.Body
            }.ToMessageBody();


            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(login, password);
            client.Send(message);

            client.Disconnect(true);
        }
    }
}
