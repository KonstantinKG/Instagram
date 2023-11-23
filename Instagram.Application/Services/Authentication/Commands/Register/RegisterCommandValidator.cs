using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).MaximumLength(32);
        RuleFor(x => x.Email).Matches(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
        RuleFor(x => x.Password).MinimumLength(8);
    }
}