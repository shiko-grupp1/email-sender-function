namespace EmailSender.Application.Abstractions;

public interface IEmailSender
{
    Task SendAsync(EmailMessageRequest request, CancellationToken ct = default);
}
