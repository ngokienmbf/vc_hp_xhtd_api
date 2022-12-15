using XHTDHP_API.Models;
using Microsoft.Extensions.Options;
using XHTDHP_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, EmailConfig setting);
    }
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }

    public class AuthMessageSender : IEmailSender, ISmsSender
    {

        public Task SendEmailAsync(string email, string subject, string message, EmailConfig setting)
        {
            // Plug in your email service here to send an email.
            Util.SendMail("", email, "", subject, message, setting);
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
          //  Util.SendSMS(message, number);
            return Task.FromResult(0);
        }
    }
}
