namespace Instagram.Contracts.User.GetAllUsersContracts;

public record GetAllUsersResponse(
    long Current,
    long Total,
    List<GetAllUser> Users
);

public record GetAllUser(
    Guid Id,
    string Username,
    string Fullname,
    string? Image
);