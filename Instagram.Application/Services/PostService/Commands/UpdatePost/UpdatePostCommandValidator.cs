using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.Content).MaximumLength(2200)
            .WithErrorCode(string.Format(Errors.Validation.MaximumLength.Code, "content", 2200));
    }
}