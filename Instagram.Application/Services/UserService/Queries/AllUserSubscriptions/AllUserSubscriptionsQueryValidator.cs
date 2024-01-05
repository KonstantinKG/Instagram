using FluentValidation;

using Instagram.Application.Services.Authentication.Queries.Login;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public class AllUserSubscriptionsQueryValidator : AbstractValidator<AllUserSubscriptionsQuery>
{
    public AllUserSubscriptionsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithName(string.Format(Errors.Validation.Required.Code, "page"));
    }
}