namespace EmailSender.Contracts.Contracts;
// From email-queue, represents the data needed to send a verification email
public sealed record VerificationEmailMessage
(   string To,
    string VerificationCode,
    int ExpiresInMinutes,
    string? CorrelationId = null
);

