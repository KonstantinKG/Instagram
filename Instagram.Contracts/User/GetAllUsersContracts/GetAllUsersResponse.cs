namespace Instagram.Contracts.User.GetAllUsersContracts;

public record GetAllUsersResponse(
    List<GetAllUser> Users
);

public record GetAllUser(
    Guid Id,
    string Username,
    string Fullname,
    string? Image
);