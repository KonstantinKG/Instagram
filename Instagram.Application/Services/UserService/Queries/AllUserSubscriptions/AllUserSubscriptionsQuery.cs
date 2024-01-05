namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public record AllUserSubscriptionsQuery(
    Guid SubscriberId,
    int Page
    );