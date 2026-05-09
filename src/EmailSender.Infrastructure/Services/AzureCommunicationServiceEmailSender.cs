using Azure.Communication.Email;
using EmailSender.Function.Application.Abstractions;
using EmailSender.Function.Dtos;
using Microsoft.Extensions.Configuration;

namespace EmailSender.Infrastructure.Services;

public class AzureCommunicationServiceEmailSender(EmailClient emailClient, IConfiguration configuration) : IEmailSender
{
    public async Task SendAsync(EmailMessageRequest request, CancellationToken ct = default)
    {
        string? senderAddress = configuration["SenderAddress"]
            ?? throw new InvalidOperationException("SenderAddress is missing");

        EmailMessage emailMessage = new EmailMessage
            (
                senderAddress: senderAddress,
                content: new EmailContent(request.Subject)
                {
                    PlainText = request.PlainTextBody,
                    Html = request.HtmlBody
                },
                recipients: new EmailRecipients
                (
                    [new EmailAddress(request.To)]
                )
            );

        // Will wait until the email is sent before returning, and will throw if the email fails to send
        await emailClient.SendAsync(Azure.WaitUntil.Completed, emailMessage, ct);
    }
}
