using Azure.Communication.Email;
using EmailSender.Application.Abstractions;
using EmailSender.Contracts.Contracts;
using Microsoft.Extensions.Configuration;

namespace EmailSender.Infrastructure.Services;
// Maps ComposedEmailMessage to Azure Communiation Service EmailMessage and sends it using EmailClient
public class AzureCommunicationServiceEmailSender(EmailClient emailClient, IConfiguration configuration) : IEmailSender
{
    public async Task SendAsync(ComposedEmailMessage message, CancellationToken ct = default)
    {
        string? senderAddress = configuration["SenderAddress"]
            ?? throw new InvalidOperationException("SenderAddress is missing");

        EmailMessage emailMessage = new EmailMessage
            (
                senderAddress: senderAddress,
                content: new EmailContent(message.Subject)
                {
                    PlainText = message.PlainTextBody,
                    Html = message.HtmlBody
                },
                recipients: new EmailRecipients
                (
                    [new EmailAddress(message.To)]
                )
            );

        // Will wait until the email is sent before returning, and will throw if the email fails to send
        await emailClient.SendAsync(Azure.WaitUntil.Completed, emailMessage, ct);
    }
}
