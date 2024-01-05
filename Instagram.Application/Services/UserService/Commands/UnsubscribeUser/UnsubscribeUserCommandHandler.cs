using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.UserService.Commands.UnsubscribeUser;

public class UnsubscribeUserCommandHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IEfUserRepository _efUserRepository;
    private ILogger<UnsubscribeUserCommandHandler> _logger;

    public UnsubscribeUserCommandHandler(
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        ILogger<UnsubscribeUserCommandHandler> logger)
    {
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<UnsubscribeUserResult>> Handle(UnsubscribeUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var subscription = await _dapperUserRepository.GetUserSubscription(command.SubscriberId, command.UserId);
            if (subscription != null)
            {
                await _efUserRepository.DeleteUserSubscription(subscription);
            }

            return new UnsubscribeUserResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}