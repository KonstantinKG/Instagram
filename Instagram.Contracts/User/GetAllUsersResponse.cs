namespace Instagram.Contracts.User;

public record GetAllUsersResponse(
    List<GetAllUser> Users
);

public record GetAllUser(
    long Id,
    string Username,
    string Fullname,
    string? Image
);