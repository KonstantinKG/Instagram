using FluentValidation;
using Instagram.Application.Services.Authentication.Commands.Register;

namespace Instagram.Application.Services.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
    }
}