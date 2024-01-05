using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public record AllUserSubscriptionsResult(
    long Current,
    long Total,
    List<User> Subscriptions
    );