using EmailSender.Application.Abstractions;
using EmailSender.Contracts.Contracts;

namespace EmailSender.Application.Composers;
// Decides the content/structure of the verification email based on the incoming message
public sealed class VerificationEmailComposer : IVerificationEmailComposer
{
    public ComposedEmailMessage Compose(VerificationEmailMessage message)
    {
        string subject = "Verify your email";
        string plainTextBody = $"Your verification code is {message.VerificationCode}.";
        string htmlBody = $"<p>Your verification code is <strong>{message.VerificationCode}</strong>.</p>";

        return new ComposedEmailMessage(
            To: message.To,
            Subject: subject,
            PlainTextBody: plainTextBody,
            HtmlBody: htmlBody,
            CorrelationId: message.CorrelationId
        );
    }
}
