using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.AllPostComments;

public class AllPostCommentsQueryValidator : AbstractValidator<AllPostCommentsQuery>
{
    public AllPostCommentsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}