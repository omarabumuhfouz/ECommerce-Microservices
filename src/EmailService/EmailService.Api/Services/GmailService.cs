using System.Net;
using System.Net.Mail;
using EmailService.Api.Abstractions;
using EmailService.Api.Contracts;
using EmailService.Api.Models;
using Microsoft.Extensions.Options;

namespace EmailService.Api.Services;

public class GmailService : IMailService
{
    private readonly GmailOptions _gmailOptions;
    public GmailService(IOptions<GmailOptions> options)
    {
        _gmailOptions = options.Value;
        
    }
    public async Task SendEmailAsync(SendEmailRequest request)
    {
        MailMessage message = new MailMessage
        {
            From = new MailAddress(_gmailOptions.Email),
            Subject = request.Subject,
            Body = request.Body,
        };

        message.To.Add(request.Recipient);

        using var smtpClient = new SmtpClient();
        smtpClient.Host = _gmailOptions.Host;
        smtpClient.Port = _gmailOptions.Port;
        smtpClient.Credentials = new NetworkCredential(_gmailOptions.Email, _gmailOptions.Password);
        smtpClient.EnableSsl = true;

        await smtpClient.SendMailAsync(message);
    }
}