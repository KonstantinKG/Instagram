using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.User.AllUserSubscriptions;

public record AllUserSubscriptionsResponse(
    long Current,
    long Total,
    List<UserShortResponse> Subscriptions
);