using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using Moq;
using ServiceLayer.Services;
using ServiceLayer.SubModels;
using ServiceLayer.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class EmailServiceTest
    {
        [Test]
        public void SendEmailSuccessTest()
        {
            // Arrange
            var emailInformation = new EmailInformation
            {
                From = "sender@gmail.com",
                To = "recipient@example.com",
                Address = "recipient@example.com",
                Body = "<html><body>Hello World!</body></html>"
            };

            var smtpClientMock = new Mock<ServiceLayer.Services.Abstract.ISmtpClient>();

            smtpClientMock.Setup(client => client.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));
            smtpClientMock.Setup(client => client.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
            smtpClientMock.Setup(client => client.Send(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()));
            smtpClientMock.Setup(client => client.Disconnect(It.IsAny<bool>(), It.IsAny<CancellationToken>()));

            var emailSender = new EmailService();

            // Act
            emailSender.SendEmail(emailInformation, smtpClientMock.Object);

            // Assert
            smtpClientMock.Verify(client => client.Connect("smtp.gmail.com", 465, true, new CancellationToken()), Times.Once);
            smtpClientMock.Verify(client => client.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClientMock.Verify(client => client.Send(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), It.IsAny<ITransferProgress>()), Times.Once);
            smtpClientMock.Verify(client => client.Disconnect(true, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
