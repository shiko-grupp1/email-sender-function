namespace EmailSender.Contracts.Contracts;
// Internal transport-objekt
public sealed record ComposedEmailMessage
(   
    string To, 
    string Subject, 
    string PlainTextBody, 
    string? HtmlBody = null, 
    string? CorrelationId = null
);

