using FluentValidation;

using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands;

public class  CreatePostCommandValidator : AbstractValidator< CreatePostCommand>
{
    public  CreatePostCommandValidator()
    {
        RuleFor(x => x.Content).MaximumLength(2200)
            .WithName(string.Format(Errors.Validation.MaximumLength.Code, "content"));
        RuleFor(x => x.Images).NotEmpty()
            .WithName(string.Format(Errors.Validation.Required.Code, "images"));;
    }
}