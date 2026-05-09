using EmailSender.Application.Abstractions;
using EmailSender.Contracts.Contracts;

namespace EmailSender.Application.Composers;
// Decides the content/structure of the verification email based on the incoming message
public sealed class VerificationEmailComposer : IVerificationEmailComposer
{
    public ComposedEmailMessage Compose(VerificationEmailMessage message)
    {
        string subject = "Verify your email";
        string plainTextBody = $"Your verification code is {message.VerificationCode}. The code expires in {message.ExpiresInMinutes} minutes.";
        string htmlBody = $"""
                <html>
                    <body>
                        <p>Your verification code is:</p>
                        <h2>{message.VerificationCode}</h2>
                        <p>The code expires in {message.ExpiresInMinutes} minutes.</p>
                    </body>
                </html>
                """;

        return new ComposedEmailMessage(
            To: message.To,
            Subject: subject,
            PlainTextBody: plainTextBody,
            HtmlBody: htmlBody,
            CorrelationId: message.CorrelationId
        );
    }
}
