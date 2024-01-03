using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public class GetHomePostsSliderQueryValidator : AbstractValidator<GetHomePostsSliderQuery>
{
    public GetHomePostsSliderQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0)
            .WithErrorCode(string.Format(Errors.Validation.Required.Code, "page"));
    }
}