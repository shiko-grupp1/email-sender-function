
using Azure.Messaging.ServiceBus;
using EmailSender.Function.Abstractions;
using EmailSender.Function.Dtos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EmailSender.Function.Functions;

public class SendVerificationEmailFunctions(IEmailSender emailSender, ILogger<SendVerificationEmailFunctions> logger, CancellationToken ct = default)
{
    private readonly ILogger<SendVerificationEmailFunctions> _logger = logger;
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    [Function(nameof(SendVerificationEmailFunctions))]
    public async Task Run(
        [ServiceBusTrigger("%EmailQueueName%", Connection = "AzureServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        string body = message.Body.ToString();

        EmailMessageRequest? request = JsonSerializer.Deserialize<EmailMessageRequest>(body, _jsonOptions)
            ?? throw new InvalidOperationException("Message could not be deserialized");

        if (!IsValid(request))
        {
            throw new InvalidOperationException("Message is missing required fields.");
        }

        await emailSender.SendAsync(request, ct);

        // Complete the message, remove from queue
        await messageActions.CompleteMessageAsync(message);
    }

    private static bool IsValid(EmailMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.MessageType)) return false;
        if (string.IsNullOrWhiteSpace(request.To)) return false;
        if (string.IsNullOrWhiteSpace(request.Subject)) return false;
        if (string.IsNullOrWhiteSpace(request.PlainTextBody) && string.IsNullOrWhiteSpace(request.HtmlBody)) return false;

        return true;
    }
}