using ErrorOr;

using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.TemporaryRepositories;

namespace Instagram.Application.Services.Authentication.Commands.Logout;

public class LogoutCommandHandler
{
    private readonly IJwtTokenRepository _jwtTokenRepository;
    private readonly IJwtTokenHasher _jwtTokenHasher;

    public LogoutCommandHandler(
        IJwtTokenRepository jwtTokenRepository,
        IJwtTokenHasher jwtTokenHasher)
    {
        _jwtTokenRepository = jwtTokenRepository;
        _jwtTokenHasher = jwtTokenHasher;
    }

    public async Task<ErrorOr<bool>> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await _jwtTokenRepository.DeleteAllSessionTokens(command.SessionId);
        return true;
    }
}