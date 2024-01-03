using FluentValidation;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.AllPosts;

public class AllPostsQueryValidator : AbstractValidator<AllPostsQuery>
{
    public AllPostsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}