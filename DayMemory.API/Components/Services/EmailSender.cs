using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace DayMemory.Core.Services
{
    public class EmailSenderSettings
    {
        [Required]
        public string? Host { get; set; }

        public int Port { get; set; }

        public bool EnableSSL { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? FromEmail { get; set; }

        [Required]
        public string? FromName { get; set; }
    }

    public interface IEmailSender
    {
        void SendMail(string toAddress, string subject, string body);
    }

    public class EmailSender : IEmailSender
    {
        public EmailSenderSettings Settings { get; private set; }

        private readonly ILogger<EmailSender> _logger;

        private readonly IEmailTemplateGenerator _emailTemplateGenerator;

        // Get our parameterized configuration
        public EmailSender(EmailSenderSettings settings, ILogger<EmailSender> logger, IEmailTemplateGenerator emailTemplateGenerator)
        {
            this.Settings = settings;
            _logger = logger;
            _emailTemplateGenerator = emailTemplateGenerator;
        }

        public void SendMail(MailMessage mail)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                using (var client = new SmtpClient(Settings.Host, Settings.Port)
                {
                    Credentials = new NetworkCredential(Settings.UserName, Settings.Password),
                    EnableSsl = Settings.EnableSSL
                })
                {
                    client.Send(mail);
                }
            }
            catch (SmtpException e)
            {
                string msg = string.Format("<br/>Subject: {0};<br/>Body: {1};<br/>From: {2};<br/>To: {3} ", mail.Subject, mail.Body, mail.From!.Address, mail.To[0].Address);
                _logger.LogError(e, msg);
                throw;
            }
        }

        public async Task SendMailAsync(string toAddress, string subject, string body, Attachment? attachment)
        {
            await Task.Run(() => SendMail(toAddress, subject, body, attachment));
        }

        public async Task SendMailAsync(string toAddress, string subject, string body)
        {
            await Task.Run(() => SendMail(toAddress, subject, body, null));
        }

        public void SendMail(string toAddress, string subject, string body)
        {
            SendMail(toAddress, subject, body, null);
        }

        public void SendMail(string toAddress, string subject, string body, Attachment? attachment)
        {
            using (MailMessage mail = BuildMessage(toAddress, subject, body, attachment))
            {
                SendMail(mail);
            }
        }

        private MailMessage BuildMessage(string toAddress, string subject, string body, Attachment? attachment)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Settings.FromEmail!, Settings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            if (attachment != null)
            {
                message.Attachments.Add(attachment);
            }

            string[] tos = toAddress.Split(';');

            foreach (string to in tos)
            {
                string mail = to.Trim();
                if (!string.IsNullOrEmpty(mail))
                    message.To.Add(new MailAddress(to));
            }

            return message;
        }
    }
}
