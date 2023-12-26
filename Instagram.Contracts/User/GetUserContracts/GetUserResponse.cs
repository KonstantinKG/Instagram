namespace Instagram.Contracts.User.GetUserContracts;

public record GetUserResponse(
    Guid Id,
    string Username,
    string Fullname,
    string? Email,
    string? Phone,
    GetUserUserProfileResponse Profile
    );
    
public record GetUserUserProfileResponse(
    string? Image,
    string? Bio,
    string? Gender
    );