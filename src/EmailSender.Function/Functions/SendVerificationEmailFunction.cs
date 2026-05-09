
using Azure.Messaging.ServiceBus;
using EmailSender.Application.Abstractions;
using EmailSender.Contracts.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EmailSender.Function.Functions;
// queue message -> verification model -> composed email -> ACS sender
public class SendVerificationEmailFunctions(IVerificationEmailComposer emailComposer, IEmailSender emailSender, ILogger<SendVerificationEmailFunctions> logger, CancellationToken ct = default)
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

        VerificationEmailMessage? request = JsonSerializer.Deserialize<VerificationEmailMessage>(body, _jsonOptions)
            ?? throw new InvalidOperationException("Message could not be deserialized");

        if (!IsValid(request))
            throw new InvalidOperationException("Message is missing required fields.");

        ComposedEmailMessage composedMessage = emailComposer.Compose(request);
        await emailSender.SendAsync(composedMessage, ct);

        // Complete the message, remove from queue
        await messageActions.CompleteMessageAsync(message);
    }

    private static bool IsValid(VerificationEmailMessage request)
    {
        if (string.IsNullOrWhiteSpace(request.To)) return false;
        if (string.IsNullOrWhiteSpace(request.VerificationCode)) return false;

        return true;
    }
}
