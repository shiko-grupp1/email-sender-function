using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmailSender.Function.Functions;

public class SendVerificationEmailFunction(ILogger<SendVerificationEmailFunction> logger)
{
    private readonly ILogger<SendVerificationEmailFunction> _logger = logger;

    [Function(nameof(SendVerificationEmailFunction))]
    public async Task Run(
        [ServiceBusTrigger("%EmailQueueName%", Connection = "AzureServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body.ToString());
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}