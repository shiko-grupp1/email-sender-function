namespace EmailSender.Function.Contracts;
// Internal transport-objekt
public sealed record ComposedEmailMessage
(   string MessageType, 
    string To, 
    string Subject, 
    string PlainTextBody, 
    string? HtmlBody = null, 
    string? CorrelationId = null
);

