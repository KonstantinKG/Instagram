using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.User.GetAllUsersContracts;

public record GetAllUsersResponse(
    long current,
    long total,
    List<UserResponse> users
);