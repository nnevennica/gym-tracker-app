using GymTracker.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GymTracker.Infrastructure.Services;

public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SendGridEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var senderEmail = _configuration["SendGrid:SenderEmail"];
        var senderName = _configuration["SendGrid:SenderName"];

        if (string.IsNullOrEmpty(apiKey))
        {
            return;
        }

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(senderEmail, senderName);
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        await client.SendEmailAsync(msg, cancellationToken);
    }
}