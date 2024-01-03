using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.UserService.Queries.GetUser;

public class GetUserQueryHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly ILogger<GetUserQueryHandler> _logger;

    public GetUserQueryHandler(
        IDapperUserRepository dapperUserRepository,
        ILogger<GetUserQueryHandler> logger)
    {
        _dapperUserRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<GetUserResult>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        try
        {
            if (await _dapperUserRepository.GetUserById(query.Id) is not User user)
            {
                return Errors.Common.NotFound;
            }

            return new GetUserResult(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}