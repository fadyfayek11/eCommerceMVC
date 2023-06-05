using eCommerce.Shared.Helpers;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            try
            {

                var smtpClient = new SmtpClient(ConfigurationsHelper.SMTP_Server, int.Parse(ConfigurationsHelper.SMTP_Port));
                smtpClient.EnableSsl = false;
                smtpClient.Timeout = 5000;
                smtpClient.UseDefaultCredentials = false;
                var apiKey = ConfigurationsHelper.SendGrid_APIKey;
                var from = new EmailAddress(ConfigurationsHelper.FromEmailAddress, ConfigurationsHelper.FromEmailAddressName);
                var subject = message.Subject;
                var to = new EmailAddress(message.Destination);
                var plainTextContent = message.Body;
                var htmlContent = message.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                smtpClient.Credentials = new NetworkCredential(ConfigurationsHelper.SMTP_Username, ConfigurationsHelper.SMTP_Password);
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from.Email,from.Name);
                mailMessage.Subject = subject;
                mailMessage.To.Add(to.Email);
                mailMessage.Body = plainTextContent;
                if(apiKey!= null && apiKey.Length > 0) 
                {
                    var client = new SendGridClient(apiKey);
                    return client.SendEmailAsync(msg);
                }
                return smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }

        public Task SendToEmailAsync(string fromEmailAddressName, string fromEmailAddress, string toEmailAddress, string toEmailSubject, string toEmailBody)
        {
            try
            {
                var smtpClient = new SmtpClient(ConfigurationsHelper.SMTP_Server, int.Parse(ConfigurationsHelper.SMTP_Port));
                smtpClient.EnableSsl = false;
                smtpClient.Timeout = 5000;
                smtpClient.UseDefaultCredentials = false; var apiKey = ConfigurationsHelper.SendGrid_APIKey;
                var from = new EmailAddress(fromEmailAddress, fromEmailAddressName);
                var subject = toEmailSubject;
                var to = new EmailAddress(toEmailAddress);
                var plainTextContent = toEmailBody;
                var htmlContent = toEmailBody;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                smtpClient.Credentials = new NetworkCredential(ConfigurationsHelper.SMTP_Username, ConfigurationsHelper.SMTP_Password);
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from.Email, from.Name);
                mailMessage.Subject = subject;
                mailMessage.To.Add(to.Email);
                mailMessage.Body = plainTextContent;
                if (apiKey != null && apiKey.Length > 0)
                {
                    var client = new SendGridClient(apiKey);
                    return client.SendEmailAsync(msg);
                }
                return smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }
    }
}