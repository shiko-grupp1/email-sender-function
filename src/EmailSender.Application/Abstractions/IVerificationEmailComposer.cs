using EmailSender.Contracts.Contracts;

namespace EmailSender.Application.Abstractions;

public interface IVerificationEmailComposer
{
    ComposedEmailMessage Compose(VerificationEmailMessage message);

}
