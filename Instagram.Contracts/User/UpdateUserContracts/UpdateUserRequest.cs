using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.User.UpdateUserContracts;

public record UpdateUserRequest(
    string Username,
    string Fullname,
    string Email,
    IFormFile? Image,
    string? Phone,
    string? Bio,
    string? Gender
);