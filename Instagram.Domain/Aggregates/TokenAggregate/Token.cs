using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.TokenAggregate;

public sealed class Token : AggregateRoot<long>
{
    public string UserId { get; private set; }
    public string SessionId { get; private set; }
    public string Hash { get; private set; }


    private Token(
        string userId,
        string sessionId,
        string hash
        )
    {
        Id = default!;
        UserId = userId;
        SessionId = sessionId;
        Hash = hash;
    }

    public static Token Create(
        string userId,
        string sessionId,
        string hash
        )
    {
        return new Token(
            userId,
            sessionId,
            hash
            );
    }
    
    # pragma warning disable CS8618
    private Token()
    {
    }
    # pragma warning disable CS8618
}