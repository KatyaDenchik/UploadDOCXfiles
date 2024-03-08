using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

namespace BlobFunction
{
    public class BlobFunction
    {
        private readonly ILogger _logger;

        public BlobFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BlobFunction>();
        }

        [Function(nameof(BlobFunction))]
        public void Run([BlobTrigger("docxfiles/{name}", Connection = "AzureWebJobsStorage")] string myBlob, string name)
        {
            SendMail();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        }
        public static void SendMail()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Green Фея", "wederkar@gmail.com"));
            message.To.Add(new MailboxAddress("Админу", "katyadenchik11@gmail.com"));
            message.Subject = "Нове замовлення";

            message.Body = new BodyBuilder()
            {
                HtmlBody ="TEST"
            }.ToMessageBody();

            using SmtpClient client = new SmtpClient();

            client.Connect("smtp.gmail.com", 465, true); // use port 465 or 587
            client.Authenticate("wederkar", "cxccjrzpjtzttixi"); //login-password from the account
            client.Send(message);

            client.Disconnect(true);

        }

    }
}
