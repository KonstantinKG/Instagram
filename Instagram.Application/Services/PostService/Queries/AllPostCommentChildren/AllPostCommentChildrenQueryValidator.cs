using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;

public class AllPostCommentChildrenQueryValidator : AbstractValidator<AllPostCommentChildrenQuery>
{
    public AllPostCommentChildrenQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}