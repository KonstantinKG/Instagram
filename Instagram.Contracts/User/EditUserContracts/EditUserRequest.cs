using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.User.EditUserContracts;

public record EditUserRequest(
    string Username,
    string Fullname,
    string Email,
    IFormFile? Image,
    string? Phone,
    string? Bio,
    string? Gender
);