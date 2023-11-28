using FluentValidation;

using Instagram.Application.Services.Authentication.Queries.Login;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithName("Page is required");
    }
}