using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.UserService.Commands.SubscribeUser;

public class SubscribeUserCommandHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IEfUserRepository _efUserRepository;
    private readonly ILogger<SubscribeUserCommandHandler> _logger;

    public SubscribeUserCommandHandler(
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        ILogger<SubscribeUserCommandHandler> logger)
    {
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<SubscribeUserResult>> Handle(SubscribeUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userSubscription = await _dapperUserRepository.GetUserSubscription(command.SubscriberId, command.UserId);
            if (userSubscription == null)
            {
                var subscription = new UserSubscription
                {
                    SubscriberId = command.SubscriberId, 
                    UserId = command.UserId
                };
                await _efUserRepository.AddUserSubscription(subscription);
            }

            return new SubscribeUserResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}