using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.User.GetAllUsersContracts;

public record GetAllUsersResponse(
    long Current,
    long Total,
    List<UserShortResponse> Users
);