using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithName(string.Format(Errors.Validation.Required.Code, "page"));
    }
}