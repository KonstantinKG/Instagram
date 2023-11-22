using FluentValidation;
using Instagram.Application.Services.Authentication.Commands.Register;

namespace Instagram.Application.Services.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email).Matches(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
        RuleFor(x => x.Password).MinimumLength(8);
    }
}