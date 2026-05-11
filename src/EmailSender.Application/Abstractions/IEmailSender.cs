using EmailSender.Contracts.Contracts;

namespace EmailSender.Application.Abstractions;

public interface IEmailSender
{
    Task SendAsync(ComposedEmailMessage message, CancellationToken ct = default);
}
