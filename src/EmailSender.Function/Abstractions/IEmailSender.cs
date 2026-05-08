using EmailSender.Function.Dtos;

namespace EmailSender.Function.Abstractions;

public interface IEmailSender
{
    Task SendAsync(EmailMessageRequest request, CancellationToken ct = default);
}
