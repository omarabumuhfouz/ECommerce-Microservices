using EmailService.Api.Contracts;

namespace EmailService.Api.Abstractions;

public interface IMailService
{
    Task SendEmailAsync(SendEmailRequest request);
}