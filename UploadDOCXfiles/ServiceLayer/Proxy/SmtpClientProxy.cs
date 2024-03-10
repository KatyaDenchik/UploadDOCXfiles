using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using ServiceLayer.Services.Abstract;

namespace ServiceLayer.Proxy
{
    public class SmtpClientProxy : Services.Abstract.ISmtpClient
    {
        private readonly SmtpClient smtpClient = new();

        /// <summary>
        /// Authenticate using the specified user name and password.
        /// </summary>
        /// <remarks>
        /// <para>Authenticates using the supplied credentials.</para>
        ///<para>If the server supports one or more SASL authentication mechanisms, then
        /// the SASL mechanisms that both the client and server support (not including any
        /// OAUTH mechanisms) are tried in order of greatest security to weakest security.
        /// Once a SASL authentication mechanism is found that both client and server support,
        /// the credentials are used to authenticate.</para>
        /// <para>If the server does not support SASL or if no common SASL mechanisms
        /// can be found, then the default login command is used as a fallback.</para>
        /// <note type="tip">To prevent the usage of certain authentication mechanisms,
        /// simply remove them from the <see cref="AuthenticationMechanisms"/> hash set
        /// before calling this method.</note>
        /// </remarks>
        /// <example>
        /// <code language="c#" source="Examples\SmtpExamples.cs" region="SendMessage"/>
        /// </example>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="userName"/> is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para><paramref name="password"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The <see cref="MailService"/> has been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The <see cref="MailService"/> is not connected or is already authenticated.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        /// The operation was canceled via the cancellation token.
        /// </exception>
        /// <exception cref="MailKit.Security.AuthenticationException">
        /// Authentication using the supplied credentials has failed.
        /// </exception>
        /// <exception cref="MailKit.Security.SaslException">
        /// A SASL authentication error occurred.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred.
        /// </exception>
        /// <exception cref="ProtocolException">
        /// A protocol error occurred.
        /// </exception>
        public void Authenticate(string userName, string password, CancellationToken cancellationToken = default)
        {
            smtpClient.Authenticate(userName, password, cancellationToken);
        }

        /// <summary>
        /// Establish a connection to the specified mail server.
        /// </summary>
        /// <remarks>
        /// <para>Establishes a connection to the specified mail server.</para>
        /// <note type="note">
        /// <para>The <paramref name="useSsl"/> argument only controls whether or
        /// not the client makes an SSL-wrapped connection. In other words, even if the
        /// <paramref name="useSsl"/> parameter is <c>false</c>, SSL/TLS may still be used if
        /// the mail server supports the STARTTLS extension.</para>
        /// <para>To disable all use of SSL/TLS, use the
        /// <see cref="Connect(string,int,MailKit.Security.SecureSocketOptions,System.Threading.CancellationToken)"/>
        /// overload with a value of
        /// <see cref="MailKit.Security.SecureSocketOptions.None">SecureSocketOptions.None</see>
        /// instead.</para>
        /// </note>
        /// </remarks>
        /// <param name="host">The host to connect to.</param>
        /// <param name="port">The port to connect to. If the specified port is <c>0</c>, then the default port will be used.</param>
        /// <param name="useSsl"><value>true</value> if the client should make an SSL-wrapped connection to the server; otherwise, <value>false</value>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The <paramref name="host"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="port"/> is out of range (<value>0</value> to <value>65535</value>, inclusive).
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="host"/> is a zero-length string.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The <see cref="MailService"/> has been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The <see cref="MailService"/> is already connected.
        /// </exception>
        /// <exception cref="System.OperationCanceledException">
        /// The operation was canceled via the cancellation token.
        /// </exception>
        /// <exception cref="System.Net.Sockets.SocketException">
        /// A socket error occurred trying to connect to the remote host.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred.
        /// </exception>
        /// <exception cref="ProtocolException">
        /// A protocol error occurred.
        /// </exception>
        public void Connect(string host, int port, bool useSsl, CancellationToken cancellationToken = default)
        {
            smtpClient.Connect(host, port, useSsl, cancellationToken);
        }

        /// <summary>
        /// Disconnect the service.
        /// </summary>
        /// <remarks>
        /// If <paramref name="quit"/> is <c>true</c>, a <c>QUIT</c> command will be issued in order to disconnect cleanly.
        /// </remarks>
        /// <example>
        /// <code language="c#" source="Examples\SmtpExamples.cs" region="SendMessage"/>
        /// </example>
        /// <param name="quit">If set to <c>true</c>, a <c>QUIT</c> command will be issued in order to disconnect cleanly.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ObjectDisposedException">
        /// The <see cref="SmtpClient"/> has been disposed.
        public void Disconnect(bool quit, CancellationToken cancellationToken = default)
        {
            smtpClient.Disconnect(quit, cancellationToken);
        }

        /// <summary>
		/// Send the specified message.
		/// </summary>
		/// <remarks>
		/// <para>Sends the specified message.</para>
		/// <para>The sender address is determined by checking the following
		/// message headers (in order of precedence): Resent-Sender,
		/// Resent-From, Sender, and From.</para>
		/// <para>If either the Resent-Sender or Resent-From addresses are present,
		/// the recipients are collected from the Resent-To, Resent-Cc, and
		/// Resent-Bcc headers, otherwise the To, Cc, and Bcc headers are used.</para>
		/// </remarks>
		/// <example>
		/// <code language="c#" source="Examples\SmtpExamples.cs" region="SendMessage"/>
		/// </example>
		/// <returns>The final free-form text response from the server.</returns>
		/// <param name="message">The message.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="progress">The progress reporting mechanism.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="message"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ObjectDisposedException">
		/// The <see cref="MailTransport"/> has been disposed.
		/// </exception>
		/// <exception cref="ServiceNotConnectedException">
		/// The <see cref="MailTransport"/> is not connected.
		/// </exception>
		/// <exception cref="ServiceNotAuthenticatedException">
		/// Authentication is required before sending a message.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// <para>A sender has not been specified.</para>
		/// <para>-or-</para>
		/// <para>No recipients have been specified.</para>
		/// </exception>
		/// <exception cref="System.OperationCanceledException">
		/// The operation has been canceled.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// An I/O error occurred.
		/// </exception>
		/// <exception cref="CommandException">
		/// The send command failed.
		/// </exception>
		/// <exception cref="ProtocolException">
		/// A protocol exception occurred.
		/// </exception>
        public void Send(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            smtpClient.Send(message, cancellationToken);
        }
    }
}
