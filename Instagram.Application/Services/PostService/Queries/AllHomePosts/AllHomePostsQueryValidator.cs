using FluentValidation;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.AllHomePosts;

public class AllHomePostsQueryValidator : AbstractValidator<AllHomePostsQuery>
{
    public AllHomePostsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}