using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;

public class GetUserPostsSliderQueryValidator : AbstractValidator<GetUserPostsSliderQuery>
{
    public GetUserPostsSliderQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}