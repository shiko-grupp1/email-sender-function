namespace EmailSender.Function.Dtos;

public sealed record EmailMessageRequest
(   string MessageType, 
    string To, 
    string Subject, 
    string PlainTextBody, 
    string? HtmlBody = null, 
    string? CorrelationId = null
);

