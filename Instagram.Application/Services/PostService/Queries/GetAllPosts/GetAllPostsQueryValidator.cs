using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.GetAllPosts;

public class GetAllPostsQueryValidator : AbstractValidator<GetAllPostsQuery>
{
    public GetAllPostsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}