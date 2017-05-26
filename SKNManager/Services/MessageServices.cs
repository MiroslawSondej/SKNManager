using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKNManager.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Test", "webdevtest@onet.pl"));
            msg.To.Add(new MailboxAddress("User", email));
            msg.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            msg.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.mailtrap.io", 25, false);
                if(!client.IsConnected)
                {
                    new Exception("Not connected");
                }
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                //client.AuthenticationMechanisms.Remove("");
                // Note: since we don't have an OAuth2 token, disable 	
                // the XOAUTH2 authentication mechanism.     
                client.Authenticate("8d956aed502290", "85d5a688315495");

                if (!client.IsAuthenticated)
                   new Exception("Not authenticated");

                client.Send(msg);
                client.Disconnect(true);
            }

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
