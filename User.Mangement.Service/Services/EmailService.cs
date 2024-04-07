using MimeKit;
using MailKit.Net.Smtp;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Management.Service.Models;
using User.Mangement.Service.Models;
using User.Mangement.Service.Services;

namespace User.Management.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("email",_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = message.Content;

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private void Send(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
