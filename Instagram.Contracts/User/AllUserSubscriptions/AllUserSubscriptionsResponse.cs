using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.User.AllUserSubscriptions;

public record AllUserSubscriptionsResponse(
    long current,
    long total,
    List<UserResponse> subscriptions
);