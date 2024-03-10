using MailKit;
using MailKit.Security;
using MimeKit;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServiceLayer.Services.Abstract
{
    public interface ISmtpClient
    {
        void Connect(string host, int port, bool useSsl, CancellationToken cancellationToken = default);
        void Authenticate(string userName, string password, CancellationToken cancellationToken = default);
        void Send(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress progress = null);
        void Disconnect(bool quit, CancellationToken cancellationToken = default);
    }
}