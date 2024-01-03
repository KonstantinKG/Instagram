using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.AllUserPosts;

public class AllUserPostsQueryValidator : AbstractValidator<AllUserPostsQuery>
{
    public AllUserPostsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}