using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram.Application.Common.Interfaces.Pipeline;

public class BasePipeline<TRequest, TResponse>
{
    private readonly IServiceProvider _serviceProvider;

    protected BasePipeline(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual async Task<ErrorOr<TResponse>> Pipe(
        TRequest request,
        Func<TRequest, CancellationToken, Task<ErrorOr<TResponse>>> handler,
        CancellationToken cancellationToken = default
    )
    {
        var errors = await ValidateAsync(request, cancellationToken);
        if (errors is not null)
            return errors;
        
        return await handler(request, cancellationToken);
    }

    protected virtual async Task<List<Error>?> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        if (validator is null)
            return null;
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
            return null;
        
        var errors = validationResult.Errors.ConvertAll(e => Error.Validation(e.PropertyName));
        return errors;
    }
}